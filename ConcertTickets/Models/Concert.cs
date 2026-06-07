using System.ComponentModel.DataAnnotations;

namespace ConcertTickets.Models
{
    public class Concert
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Artist { get; set; }

        [Required]
        [StringLength(300)]
        public string Venue { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public string? ImagePath { get; set; }
    }
}

