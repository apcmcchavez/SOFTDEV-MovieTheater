using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ViewModels
{
    /// <summary>
    /// Data returned by the Showtime API and service layer.
    /// </summary>
    public class ShowtimeDto
    {
        public Guid ShowtimeId { get; set; }

        public string MovieCode { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public DateTime ShowDateTime { get; set; }

        public decimal TicketPrice { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
    }

    /// <summary>
    /// Data accepted when creating or updating a Showtime.
    /// </summary>
    public class ShowtimeCreateUpdateDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "Movie Code")]
        public string MovieCode { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Movie Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Show Date and Time")]
        public DateTime ShowDateTime { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "100000.00")]
        [DataType(DataType.Currency)]
        [Display(Name = "Ticket Price")]
        public decimal TicketPrice { get; set; }
    }
}