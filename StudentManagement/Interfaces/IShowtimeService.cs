using StudentManagement.ViewModels;

namespace StudentManagement.Interfaces
{
    /// <summary>
    /// Business/application logic contract for Showtime operations.
    /// </summary>
    public interface IShowtimeService
    {
        Task<ShowtimeIndexViewModel> GetShowtimeListAsync(
            string? searchTerm,
            string sortColumn,
            string sortDirection,
            int pageNumber,
            int pageSize);

        Task<IEnumerable<ShowtimeDto>> GetAllAsync();

        Task<ShowtimeDto?> GetByIdAsync(Guid id);

        Task<(
            bool Success,
            ShowtimeDto? Showtime,
            string? ErrorMessage)> CreateAsync(
                ShowtimeCreateUpdateDto dto);

        Task<(
            bool Success,
            ShowtimeDto? Showtime,
            string? ErrorMessage)> UpdateAsync(
                Guid id,
                ShowtimeCreateUpdateDto dto);

        Task<bool> DeleteAsync(Guid id);
    }
}