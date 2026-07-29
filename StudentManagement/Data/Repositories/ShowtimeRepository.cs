using Microsoft.EntityFrameworkCore;
using StudentManagement.Interfaces;
using StudentManagement.Models;

namespace StudentManagement.Data.Repositories
{
    /// <summary>
    /// EF Core implementation of the Showtime repository.
    /// </summary>
    public class ShowtimeRepository : IShowtimeRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowtimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Showtime>> GetPagedAsync(
            string? searchTerm,
            string sortColumn,
            string sortDirection,
            int pageNumber,
            int pageSize)
        {
            IQueryable<Showtime> query = _context.Showtimes;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();

                query = query.Where(s =>
                    s.MovieCode.ToLower().Contains(term) ||
                    s.Title.ToLower().Contains(term) ||
                    s.Genre.ToLower().Contains(term));
            }

            var descending = string.Equals(
                sortDirection,
                "desc",
                StringComparison.OrdinalIgnoreCase);

            query = sortColumn switch
            {
                "MovieCode" => descending
                    ? query.OrderByDescending(s => s.MovieCode)
                    : query.OrderBy(s => s.MovieCode),

                "Title" => descending
                    ? query.OrderByDescending(s => s.Title)
                    : query.OrderBy(s => s.Title),

                "Genre" => descending
                    ? query.OrderByDescending(s => s.Genre)
                    : query.OrderBy(s => s.Genre),

                "TicketPrice" => descending
                    ? query.OrderByDescending(s => s.TicketPrice)
                    : query.OrderBy(s => s.TicketPrice),

                _ => descending
                    ? query.OrderByDescending(s => s.ShowDateTime)
                    : query.OrderBy(s => s.ShowDateTime)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResult<Showtime>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<IEnumerable<Showtime>> GetAllAsync()
        {
            return await _context.Showtimes
                .AsNoTracking()
                .OrderBy(s => s.ShowDateTime)
                .ToListAsync();
        }

        public async Task<Showtime?> GetByIdAsync(Guid id)
        {
            return await _context.Showtimes
                .FirstOrDefaultAsync(s => s.ShowtimeId == id);
        }

        public async Task<Showtime> AddAsync(Showtime showtime)
        {
            showtime.DateCreated = DateTime.UtcNow;

            _context.Showtimes.Add(showtime);
            await _context.SaveChangesAsync();

            return showtime;
        }

        public async Task UpdateAsync(Showtime showtime)
        {
            showtime.DateUpdated = DateTime.UtcNow;

            _context.Showtimes.Update(showtime);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var showtime = await _context.Showtimes
                .FirstOrDefaultAsync(s => s.ShowtimeId == id);

            if (showtime is null)
            {
                return false;
            }

            _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ScheduleExistsAsync(
            string movieCode,
            DateTime showDateTime,
            Guid excludeShowtimeId = default)
        {
            var normalizedCode = movieCode.Trim().ToLower();

            return await _context.Showtimes.AnyAsync(s =>
                s.MovieCode.ToLower() == normalizedCode &&
                s.ShowDateTime == showDateTime &&
                s.ShowtimeId != excludeShowtimeId);
        }
    }
}