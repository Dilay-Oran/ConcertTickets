using System.ComponentModel.DataAnnotations;

namespace ConcertTickets.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Hangi konser için?
        public int ConcertId { get; set; }
        public Concert? Concert { get; set; }

        // Kim aldı? (Identity kullanıcısının Id'si — string)
        [Required]
        public string UserId { get; set; }

        [Required]
        [Range(1, 100000)]
        public int Quantity { get; set; }   // kaç adet bilet

        public decimal TotalPrice { get; set; }   // toplam tutar

        public DateTime OrderDate { get; set; }
    }
}