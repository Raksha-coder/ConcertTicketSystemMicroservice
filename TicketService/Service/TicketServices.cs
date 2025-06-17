using Microsoft.Extensions.Logging;
using TicketService.common;
using TicketService.Dto;
using TicketService.Model;
using TicketService.Repository;

namespace TicketService.Service
{
    public class TicketServices : ITicketService
    {
        private readonly ITicketRepository  _ticketrepository;
        private readonly ILogger<TicketServices> _logger;

        public TicketServices(ITicketRepository ticketRepository, ILogger<TicketServices> logger)
        {
            _ticketrepository = ticketRepository;
            _logger = logger;
        }


        public async Task<ResponseBody> CreateTicketType(TicketTypeAddDto ticketTypeData)
        {
            try
            {
                TicketType ticketType = new TicketType
                {
                    Id = Guid.NewGuid(),
                    Name = ticketTypeData.Name,
                    EventId = ticketTypeData.EventId,
                    QuantityAvailable = ticketTypeData.QuantityAvailable,
                    Price = ticketTypeData.Price,
                };

                if (ticketTypeData.QuantityAvailable <= 0)
                    return new ResponseBody(false,"Please enter valid availability of ticket");

                ResponseBody ticketTypeResult = await _ticketrepository.CreateTicketType(ticketType);


                if (ticketTypeResult.Success)
                {
                    dynamic data = ticketTypeResult.Data;
                    var ticketsList = new List<Ticket>(ticketType.QuantityAvailable) { };
                    for (int i=1;i<= ticketType.QuantityAvailable;i++)
                    {
                        ticketsList.Add(new Ticket 
                        {  
                            Id = Guid.NewGuid(),
                            TicketTypeId = data,
                            ReservationCode = string.Empty,
                            IsPurchased = false,
                            ReservationExpiresAt = null

                        });
                    }

                    var result = await _ticketrepository.CreateTicketAsync(ticketsList);
                    return result;

                }
                else
                {
                    return ticketTypeResult;
                }

            }
            catch(Exception ex)
            {
                _logger.LogError("An unexpected error occurred :{ex}", ex.Message);
                return new ResponseBody(false, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ResponseBody> ReserveTicket(Guid ticketTypeId, TimeSpan waitingTime)
        {
            if (ticketTypeId == Guid.Empty)
                return new ResponseBody(false, "Invalid Ticet Type Id");

            var reserve_ticket = await _ticketrepository.CheckTicketAvailability(ticketTypeId);
            if (reserve_ticket.Success)
            {
                Ticket data = (Ticket)reserve_ticket.Data;

                data.ReservationCode = "res_" + Guid.NewGuid().ToString();
                data.ReservationExpiresAt = DateTime.UtcNow.Add(waitingTime);

                var result = await _ticketrepository.UpdateTicket(data);
                return result;
            }
            else
            {
                return reserve_ticket;
            }

        }   


        public async Task<ResponseBody> CancelReservation(string reservationCode)
        {
            try
            {
                var ticket = await _ticketrepository.GetTicketByReservationCode(reservationCode);

                if (ticket == null)
                    return new ResponseBody(false, "you haven't reserve any ticket");

                ticket.ReservationCode = string.Empty;
                ticket.ReservationExpiresAt = null;


                var result = await _ticketrepository.UpdateTicket(ticket);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred :{ex}", ex.Message);
                return new ResponseBody(false, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ResponseBody> PurchaseTicket(Guid ticketId)
        {
            try
            {
                var ticket = await _ticketrepository.GetTicketById(ticketId);

                if (ticket == null)
                    return new ResponseBody(false, "please select valid ticket to buy");

                if(!ticket.IsPurchased || string.IsNullOrEmpty(ticket.ReservationCode))
                {
                    ticket.IsPurchased = true;
                    var result = await _ticketrepository.UpdateTicket(ticket);
                    return result;
                }
                else
                {
                    _logger.LogInformation("Ticket has already sold out.");
                    return new ResponseBody(false, "Ticket has already sold out");
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred :{ex}", ex.Message);
                return new ResponseBody(false, $"An unexpected error occurred: {ex.Message}");
            }
        }
        public async Task<ResponseBody> BuyReservedTicket(Guid ticketId, string reservationCode)
        {
            if (ticketId == Guid.Empty)
                return new ResponseBody(false, "Invalid ticket Id");

            var ticket = await _ticketrepository.GetTicketById(ticketId);


            if(ticket == null)
            {
                _logger.LogInformation($"Data not found with given id {ticketId}");
                return new ResponseBody(false, "Data not found with given id");
            }

            if (ticket.IsPurchased || ticket.ReservationCode != reservationCode || !ticket.ReservationExpiresAt.HasValue ||
            ticket.ReservationExpiresAt < DateTime.UtcNow)
            {
                return new ResponseBody(false, "reservation code is not valid or else ticket is purchased");
            }

                ticket.IsPurchased = true;
                ticket.ReservationCode = string.Empty;
                ticket.ReservationExpiresAt = null;

                var result = await _ticketrepository.UpdateTicket(ticket);
                return result;

        }


    }
}
