using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ViewModels
{
    /// <summary>
    /// ViewModel used by the Showtime Create and Edit Razor forms.
    /// Only user-editable fields are included in form binding.
    /// </summary>
    public class ShowtimeViewModel
    {
        /// <summary>
        /// Guid.Empty when creating a new record.
        /// Contains the existing ID when editing.
        /// </summary>
        public Guid ShowtimeId { get; set; }

        [Required(ErrorMessage = "Movie code is required.")]
        [StringLength(
            20,
            MinimumLength = 2,
            ErrorMessage = "Movie code must be between 2 and 20 characters.")]
        [Display(Name = "Movie Code")]
        public string MovieCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Movie title is required.")]
        [StringLength(
            150,
            ErrorMessage = "Movie title cannot exceed 150 characters.")]
        [Display(Name = "Movie Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(
            50,
            ErrorMessage = "Genre cannot exceed 50 characters.")]
        public string Genre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Show date and time are required.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Show Date and Time")]
        public DateTime ShowDateTime { get; set; }

        [Required(ErrorMessage = "Ticket price is required.")]
        [Range(
            typeof(decimal),
            "0.01",
            "100000.00",
            ErrorMessage = "Ticket price must be greater than $0.00.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Ticket Price")]
        public decimal TicketPrice { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}