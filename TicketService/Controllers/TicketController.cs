using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketService.common;
using TicketService.Dto;
using TicketService.Model;
using TicketService.Service;

namespace TicketService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }


        [HttpPost("createTicketType")]
        public async Task<IActionResult> CreateTicketType([FromBody] TicketTypeAddDto tickets)
        {
            if (tickets == null)
                return BadRequest(new ResponseBody(false, "Please provide ticket type data"));


            var result = await _ticketService.CreateTicketType(tickets);
            return Ok(result);

        }

        [HttpPost("reserveTicket")]
        public async Task<IActionResult> ReserveTicket([FromQuery] Guid ticketTypeId)
        {
            if (ticketTypeId == Guid.Empty)
                return BadRequest(new ResponseBody(false, "Provide Valid Ticket Type Id"));

            var reservedTicket = await _ticketService.ReserveTicket(ticketTypeId, TimeSpan.FromMinutes(15));

            return Ok(reservedTicket);
        }

        [HttpPost("buyOrConfirmReservedTicket")] //After reservation buy the ticket
        public async Task<IActionResult> BuyReservedTicket(Guid ticketId, string reservationCode)
        {
            if (ticketId == Guid.Empty || string.IsNullOrWhiteSpace(reservationCode))
                return BadRequest(new ResponseBody(false, "Invalid ticketId and reservationCode"));

            var result = await _ticketService.BuyReservedTicket(ticketId, reservationCode);

            return Ok(result);
        }

        [HttpPost("cancelReservation")]
        public async Task<IActionResult> cancelReservation(string reservationCode)
        {
            if (string.IsNullOrWhiteSpace(reservationCode))
                return BadRequest(new ResponseBody(false, "reservation code is invalid"));


            var result = await _ticketService.CancelReservation(reservationCode);
            return Ok(result);
        }

        [HttpPost("purchaseTicket")]
        public async Task<IActionResult> PurchaseTicket(Guid ticketId)
        {
            if (ticketId == Guid.Empty)
                return BadRequest(new ResponseBody(false, "ticketId is invalid"));

            var result = await _ticketService.PurchaseTicket(ticketId);
            return Ok(result);
        }

    }
}
