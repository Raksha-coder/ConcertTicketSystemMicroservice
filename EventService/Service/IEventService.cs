using EventService.common;
using EventService.DTO.RequestDto;
using EventService.Model;

namespace EventService.Service
{
    public interface IEventService
    {
        Task<ResponseBody> CreateEvent(CreateEventDto eventData);
        Task<ResponseBody> UpdateEvent(CreateEventDto eventData);
        Task<ResponseBody> GetAllEvent();
        Task<ResponseBody> GetAllEventsByVenueId(Guid venueId);
    }
}
