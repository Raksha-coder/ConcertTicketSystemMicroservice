using System.ComponentModel.DataAnnotations;

namespace EventService.DTO.RequestDto
{
    public class CreateEventDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid VenueId { get; set; }
    }
}
