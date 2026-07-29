using StudentManagement.Models;

namespace StudentManagement.ViewModels
{
    /// <summary>
    /// Holds Showtime records and list-page controls such as
    /// searching, sorting, and pagination.
    /// </summary>
    public class ShowtimeIndexViewModel
    {
        public IEnumerable<Showtime> Showtimes { get; set; }
            = new List<Showtime>();

        public int TotalRecords { get; set; }

        public string? SearchTerm { get; set; }

        public string SortColumn { get; set; } = "ShowDateTime";

        public string SortDirection { get; set; } = "asc";

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalPages =>
            PageSize > 0
                ? (int)Math.Ceiling(TotalRecords / (double)PageSize)
                : 0;

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public string OppositeSortDirection(string column)
        {
            bool isCurrentColumn = string.Equals(
                SortColumn,
                column,
                StringComparison.OrdinalIgnoreCase);

            bool currentlyAscending = string.Equals(
                SortDirection,
                "asc",
                StringComparison.OrdinalIgnoreCase);

            return isCurrentColumn && currentlyAscending
                ? "desc"
                : "asc";
        }
    }
}