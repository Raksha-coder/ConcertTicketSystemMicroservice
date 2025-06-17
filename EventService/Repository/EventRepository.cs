using EventService.common;
using EventService.Data;
using EventService.DTO.RequestDto;
using EventService.Model;
using EventService.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EventService.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _context;
        private readonly ILogger<EventRepository> _logger;

        public EventRepository(EventDbContext context, ILogger<EventRepository> logger)
        {
            _context = context;
            _logger = logger;

        }
        public async Task<ResponseBody> CreateEventAsync(CreateEventDto eventData)
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
            _logger.LogInformation("Event created successfully");
            return new ResponseBody(true, "Event created successfully.", eventEntity.Id);

        }


        public async Task<ResponseBody> UpdateEventAsync(CreateEventDto eventData)
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
            _logger.LogInformation("Event updated successfully");
            return new ResponseBody(true, "Event updated successfully.", IsEventAvailable.Id);


        }

        public async Task<ResponseBody> GetAllEventAsync()
        {

            IEnumerable<Event> eventList = await _context.Events.ToListAsync();
            _logger.LogInformation("Fetch Events Successfully at {time}", DateTime.UtcNow);
            return new ResponseBody(true, "Fetch Events Successfully", eventList);


        }

        public async Task<ResponseBody> GetAllEventByVenue(Guid Id)
        {

            IEnumerable<Event> eventList = await _context.Events.Where(eve => eve.VenueId == Id).ToListAsync();
            _logger.LogInformation("Fetch Events Successfully at {time}", DateTime.UtcNow);
            return new ResponseBody(true, "Fetch Events Successfully", eventList);


        }

    }
}
