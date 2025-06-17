namespace TicketService.Dto
{
    public class ViewTicketTypeDto
    {
        public Guid TicketTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int TotalQuantity { get; set; }
        public int AvailableCount { get; set; }
    }
}
