using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    /// <summary>
    /// Represents a scheduled movie screening in the theater.
    /// </summary>
    public class Showtime
    {
        [Key]
        public Guid ShowtimeId { get; set; } = Guid.NewGuid();

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

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateUpdated { get; set; }
    }
}