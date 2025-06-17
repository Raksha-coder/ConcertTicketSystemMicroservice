using TicketService.common;
using TicketService.Dto;

namespace TicketService.Service
{
    public interface ITicketService
    {
        Task<ResponseBody> CreateTicketType(TicketTypeAddDto ticketTypeData);

        Task<ResponseBody> ReserveTicket(Guid Id, TimeSpan waitingTime);

        Task<ResponseBody> BuyReservedTicket(Guid ticketId, string reservedCode);

        Task<ResponseBody> CancelReservation(string reservationCode);
        Task<ResponseBody> PurchaseTicket(Guid ticketId);

    }
}
