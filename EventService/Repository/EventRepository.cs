using EventService.common;
using EventService.Data;
using EventService.DTO.RequestDto;
using EventService.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EventService.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _context;
        public EventRepository(EventDbContext context)
        {
            _context = context;
        }


        public async Task<ResponseBody> CreateEventAsync(CreateEventDto eventData)
        {
            try
            {

                var eventEntity = new Event
                {
                    Id = Guid.NewGuid(),
                    Name = eventData.Name,
                    Date = eventData.Date,
                    StartTime = eventData.StartTime,
                    EndTime = eventData.EndTime,
                    Description = eventData.Description,
                    VenueId = eventData.VenueId,
                };


                if (eventEntity.StartTime >= eventEntity.EndTime)
                {
                    return new ResponseBody(false, "Event start time must be before end time.");
                }

                await _context.Events.AddAsync(eventEntity);

                await _context.SaveChangesAsync();

                return new ResponseBody(true, "Event created successfully.", eventEntity.Id);
            }
            catch (Exception ex)
            {
                return new ResponseBody(false, $"An unexpected error occurred: {ex.Message}");
            }
        }


        public async Task<ResponseBody> UpdateEventAsync(CreateEventDto eventData)
        {
            try
            {
                var IsEventAvailable = await _context.Events.FindAsync(eventData.Id);

                if (IsEventAvailable == null)
                {
                    return new ResponseBody(false, $"Event with ID '{eventData.Id}' not found.");
                }

                IsEventAvailable.Name = eventData.Name;
                IsEventAvailable.Date = eventData.Date;
                IsEventAvailable.StartTime = eventData.StartTime;
                IsEventAvailable.EndTime = eventData.EndTime;
                IsEventAvailable.Description = eventData.Description;
                IsEventAvailable.VenueId = eventData.VenueId;

                if (IsEventAvailable.StartTime >= IsEventAvailable.EndTime)
                {
                    return new ResponseBody(false, "Event start time must be before end time.");
                }

                _context.Events.Update(IsEventAvailable);
                await _context.SaveChangesAsync();

                return new ResponseBody(true, "Event updated successfully.", IsEventAvailable.Id);
            }
            catch (Exception ex)
            {
                return new ResponseBody(false, $"An unexpected error occurred during event update: {ex.Message}");
            }

        }

        public async Task<ResponseBody> GetAllEventAsync()
        {
            try
            {
                IEnumerable<Event> eventList = await _context.Events.ToListAsync();
                return new ResponseBody(true, "Fetch Events Successfully",eventList);

            }
            catch (Exception ex)
            {
                return new ResponseBody(false, $"An unexpected error occurred during event update: {ex.Message}");
            }
        }

        public async Task<ResponseBody> GetAllEventByVenue(Guid Id)
        {
            try
            {
                IEnumerable<Event> eventList = await _context.Events.Where(eve => eve.VenueId == Id).ToListAsync();
                return new ResponseBody(true, "Fetch Events Successfully", eventList);

            }
            catch (Exception ex)
            {
                return new ResponseBody(false, $"An unexpected error occurred during event update: {ex.Message}");
            }
        }

        }
}
