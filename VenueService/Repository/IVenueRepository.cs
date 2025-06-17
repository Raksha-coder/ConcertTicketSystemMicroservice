using VenueService.common;
using VenueService.Model;

namespace VenueService.Repository
{
    public interface IVenueRepository
    {
        Task<ResponseBody> AddVenueAsync(Venue venue);
        Task<Venue?> GetVenue(Guid venueId);

    }
}
