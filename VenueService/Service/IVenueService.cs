using VenueService.common;
using VenueService.Dto;

namespace VenueService.Service
{
    public interface IVenueService
    {
        Task<ResponseBody> CreateVenu(CreateVenueDto venueData);
        Task<ResponseBody> GetVenueEventList(Guid venueId);

    }
}
