namespace EventService.DTO.ResponseDto
{
    public class ViewEventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public DateTime EventStartTime { get; set; }

        public DateTime EventEndTime { get; set; }
        public string VenueName { get; set; } = string.Empty;
        //public List<TicketTypeViewModel> TicketTypes { get; set; } = new();
    }
}
