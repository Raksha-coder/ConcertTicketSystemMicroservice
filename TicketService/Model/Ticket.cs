using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketService.Model
{
    public class Ticket
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string ReservationCode { get; set; } = string.Empty;

        public DateTime? ReservationExpiresAt { get; set; }

        public bool IsPurchased { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PurchasedAt { get; set; }

        [Required]
        public Guid TicketTypeId { get; set; }

    }
}
