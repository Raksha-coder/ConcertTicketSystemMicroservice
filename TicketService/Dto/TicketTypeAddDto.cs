using System.ComponentModel.DataAnnotations;

namespace TicketService.Dto
{
    public class TicketTypeAddDto
    {
        public Guid Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;


        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "QuantityAvailable cannot be negative.")]
        public int QuantityAvailable { get; set; }

        [Required]
        public Guid EventId { get; set; }
    }
}
