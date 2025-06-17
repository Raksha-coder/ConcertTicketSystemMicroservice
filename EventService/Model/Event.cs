using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace EventService.Model
{
    public class Event
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty; 

        [Required]
        public DateTime Date { get; set; } 

        [Required]
        public DateTime StartTime { get; set; } 

        [Required]
        public DateTime EndTime { get; set; } 

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty; 

        [Required]
        public Guid VenueId { get; set; } 

    }
}
