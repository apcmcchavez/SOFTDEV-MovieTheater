using StudentManagement.Interfaces;
using StudentManagement.Models;
using StudentManagement.ViewModels;

namespace StudentManagement.Services
{
    /// <summary>
    /// Implements business logic for Showtime records.
    /// </summary>
    public class ShowtimeService : IShowtimeService
    {
        private readonly IShowtimeRepository _repository;
        private readonly ILogger<ShowtimeService> _logger;

        public ShowtimeService(
            IShowtimeRepository repository,
            ILogger<ShowtimeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ShowtimeIndexViewModel> GetShowtimeListAsync(
            string? searchTerm,
            string sortColumn,
            string sortDirection,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var result = await _repository.GetPagedAsync(
                searchTerm,
                sortColumn,
                sortDirection,
                pageNumber,
                pageSize);

            return new ShowtimeIndexViewModel
            {
                Showtimes = result.Items,
                TotalRecords = result.TotalCount,
                SearchTerm = searchTerm,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<ShowtimeDto>> GetAllAsync()
        {
            var showtimes = await _repository.GetAllAsync();

            return showtimes.Select(MapToDto);
        }

        public async Task<ShowtimeDto?> GetByIdAsync(Guid id)
        {
            var showtime = await _repository.GetByIdAsync(id);

            return showtime is null
                ? null
                : MapToDto(showtime);
        }

        public async Task<(
            bool Success,
            ShowtimeDto? Showtime,
            string? ErrorMessage)> CreateAsync(
                ShowtimeCreateUpdateDto dto)
        {
            if (await _repository.ScheduleExistsAsync(
                dto.MovieCode,
                dto.ShowDateTime))
            {
                return (
                    false,
                    null,
                    $"Movie '{dto.MovieCode}' already has a showtime scheduled for " +
                    $"{dto.ShowDateTime:g}.");
            }

            var entity = new Showtime
            {
                MovieCode = dto.MovieCode.Trim(),
                Title = dto.Title.Trim(),
                Genre = dto.Genre.Trim(),
                ShowDateTime = dto.ShowDateTime,
                TicketPrice = dto.TicketPrice
            };

            var created = await _repository.AddAsync(entity);

            _logger.LogInformation(
                "Created showtime {MovieCode} at {ShowDateTime} " +
                "(Id={ShowtimeId})",
                created.MovieCode,
                created.ShowDateTime,
                created.ShowtimeId);

            return (true, MapToDto(created), null);
        }

        public async Task<(
            bool Success,
            ShowtimeDto? Showtime,
            string? ErrorMessage)> UpdateAsync(
                Guid id,
                ShowtimeCreateUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing is null)
            {
                return (false, null, "Showtime not found.");
            }

            if (await _repository.ScheduleExistsAsync(
                dto.MovieCode,
                dto.ShowDateTime,
                excludeShowtimeId: id))
            {
                return (
                    false,
                    null,
                    $"Movie '{dto.MovieCode}' already has a showtime scheduled for " +
                    $"{dto.ShowDateTime:g}.");
            }

            existing.MovieCode = dto.MovieCode.Trim();
            existing.Title = dto.Title.Trim();
            existing.Genre = dto.Genre.Trim();
            existing.ShowDateTime = dto.ShowDateTime;
            existing.TicketPrice = dto.TicketPrice;

            await _repository.UpdateAsync(existing);

            _logger.LogInformation(
                "Updated showtime Id={ShowtimeId}",
                id);

            return (true, MapToDto(existing), null);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (deleted)
            {
                _logger.LogInformation(
                    "Deleted showtime Id={ShowtimeId}",
                    id);
            }

            return deleted;
        }

        private static ShowtimeDto MapToDto(Showtime showtime)
        {
            return new ShowtimeDto
            {
                ShowtimeId = showtime.ShowtimeId,
                MovieCode = showtime.MovieCode,
                Title = showtime.Title,
                Genre = showtime.Genre,
                ShowDateTime = showtime.ShowDateTime,
                TicketPrice = showtime.TicketPrice,
                DateCreated = showtime.DateCreated,
                DateUpdated = showtime.DateUpdated
            };
        }
    }
}