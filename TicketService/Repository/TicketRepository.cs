using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using TicketService.common;
using TicketService.Data;
using TicketService.Model;
using TicketService.Service;

namespace TicketService.Repository
{
    public class TicketRepository : ITicketRepository
    {
        public readonly TicketDbContext _context;
        private readonly ILogger<TicketRepository> _logger;

        public TicketRepository(TicketDbContext context, ILogger<TicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseBody> CreateTicketType(TicketType ticketTypeData)
        {
            try
            {
                await _context.TicketTypes.AddAsync(ticketTypeData);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ticket Types Created Successfully");
                return new ResponseBody(true, "Ticket Types Created Successfully", ticketTypeData.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("A database error occurred while creating the ticket type :{ex}",ex.Message);
                return new ResponseBody(false, "A database error occurred while creating the ticket type.");
            }catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while creating the ticket type. :{ex}", ex.Message);
                return new ResponseBody(false, "An unexpected error occurred while creating the ticket type.");
            }

        }


        public async Task<ResponseBody> CreateTicketAsync(List<Ticket> ticketData)
        {
            try
            {
                if (ticketData == null || ticketData.Count <= 0)
                {
                    return new ResponseBody(false, "Zero tickets to add");
                }

                await _context.Tickets.AddRangeAsync(ticketData);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ticket Created Successfully");
                return new ResponseBody(true, "Tickets are Created Successfully");

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("A database error occurred while creating the ticket :{ex}", ex.Message);
                return new ResponseBody(false, "A database error occurred while creating the ticket.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while creating the ticket :{ex}", ex.Message);
                return new ResponseBody(false, "An unexpected error occurred while creating the ticket.");
            }

        }


        public async Task<ResponseBody> CheckTicketAvailability(Guid ticketId)
        {
            try
            {
                var availableTicket = await _context.Tickets
                    .Where(tic => 
                        tic.IsPurchased == false
                        && tic.TicketTypeId == ticketId
                        && (tic.ReservationExpiresAt == null 
                        || tic.ReservationExpiresAt < DateTime.UtcNow) 
                        ).FirstOrDefaultAsync();

                var response = availableTicket == null
                    ? new ResponseBody(false, "Ticket is not available")
                    : new ResponseBody(true, "Ticket is available", availableTicket);

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while checking ticket availability :{ex}", ex.Message);
                return new ResponseBody(false, "An unexpected error occurred.");
            }
        }

        public async Task<ResponseBody> UpdateTicket(Ticket ticket)
        {
            try
            {
                _context.Tickets.Update(ticket);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Modified ticket successfully");
                return new ResponseBody(true, "Successfully modified ticket status",ticket);

            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while modifying the ticket :{ex}", ex.Message);
                return new ResponseBody(false, "An unexpected error occurred.");
            }
        }

        public async Task<Ticket?> GetTicketByReservationCode(string reservationCode)
        {
            return await _context.Tickets.Where(tic => tic.ReservationCode == reservationCode && !tic.IsPurchased).FirstOrDefaultAsync();
        }


        public async Task<Ticket> GetTicketById(Guid ticketId)
        {
                var ticket =  await _context.Tickets.FindAsync(ticketId);
                return ticket;
        }
    }
}
