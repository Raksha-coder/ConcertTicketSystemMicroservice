using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketService.Model
{
    public class TicketType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; 

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; } 

        [Required]
        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; } 

        [Required]
        public Guid EventId { get; set; } 

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
