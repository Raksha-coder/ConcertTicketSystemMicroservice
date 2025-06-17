using System.ComponentModel.DataAnnotations;

namespace EventService.DTO.RequestDto
{
    public class CreateEventDto
    {
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

        /// <summary>
        /// Detailed description of the event.
        /// Maximum length capped to maintain database consistency.
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;


        [Required]
        public Guid VenueId { get; set; }
    }
}
