using StudentManagement.Models;

namespace StudentManagement.Interfaces
{
    /// <summary>
    /// Data-access contract for Showtime records.
    /// </summary>
    public interface IShowtimeRepository
    {
        Task<PagedResult<Showtime>> GetPagedAsync(
            string? searchTerm,
            string sortColumn,
            string sortDirection,
            int pageNumber,
            int pageSize);

        Task<IEnumerable<Showtime>> GetAllAsync();

        Task<Showtime?> GetByIdAsync(Guid id);

        Task<Showtime> AddAsync(Showtime showtime);

        Task UpdateAsync(Showtime showtime);

        /// <summary>
        /// Permanently deletes a Showtime record.
        /// Returns false when the supplied ID does not exist.
        /// </summary>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Checks whether the same movie is already scheduled
        /// at the same date and time.
        /// </summary>
        Task<bool> ScheduleExistsAsync(
            string movieCode,
            DateTime showDateTime,
            Guid excludeShowtimeId = default);
    }
}