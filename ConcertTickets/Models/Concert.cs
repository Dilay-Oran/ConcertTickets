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

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 100000)]
        public int Capacity { get; set; }   // toplam kontenjan

        public int Sold { get; set; }        // satılan adet
        public string? ImagePath { get; set; }
    }
}

