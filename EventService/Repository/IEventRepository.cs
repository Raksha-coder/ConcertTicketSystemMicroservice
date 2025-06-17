using EventService.common;
using EventService.DTO.RequestDto;
using EventService.Model;

namespace EventService.Repository
{
    public interface IEventRepository
    {
        Task<ResponseBody> CreateEventAsync(CreateEventDto eventData);
        Task<ResponseBody> UpdateEventAsync(CreateEventDto eventData);
        Task<ResponseBody> GetAllEventAsync();

        Task<ResponseBody> GetAllEventByVenue(Guid Id);



    }
}
