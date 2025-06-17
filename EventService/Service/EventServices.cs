using Azure.Core;
using EventService.common;
using EventService.DTO.RequestDto;
using EventService.Model;
using EventService.Repository;

namespace EventService.Service
{
    public class EventServices : IEventService
    {
        private readonly IEventRepository _eventrepository;

        public EventServices(IEventRepository repository)
        {
            _eventrepository = repository;
        }

        public async Task<ResponseBody> CreateEvent(CreateEventDto eventData)
        {

            return await _eventrepository.CreateEventAsync(eventData);
        }

        public async Task<ResponseBody> UpdateEvent(CreateEventDto eventData)
        {

            return await _eventrepository.UpdateEventAsync(eventData);
        }

        public async Task<ResponseBody> GetAllEvent()
        {
            return await _eventrepository.GetAllEventAsync();
        }

        public async Task<ResponseBody> GetAllEventsByVenueId(Guid Id)
        {
            return await _eventrepository.GetAllEventByVenue(Id);

        }



    }
}
