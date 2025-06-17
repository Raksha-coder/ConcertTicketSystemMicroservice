using TicketService.common;
using TicketService.Model;

namespace TicketService.Repository
{
    public interface ITicketRepository
    {
        Task<ResponseBody> CreateTicketType(TicketType ticketTypeData);
        Task<ResponseBody> CreateTicketAsync(List<Ticket> TicketListData);
        Task<ResponseBody> CheckTicketAvailability(Guid ticketId);
        Task<ResponseBody>  UpdateTicket(Ticket ticket);

        Task <Ticket?> GetTicketByReservationCode(string reservationCode);

        Task<Ticket> GetTicketById(Guid ticketId);



    }
}
