using System.Net.Http;
using VenueService.common;
using VenueService.Dto;
using VenueService.Model;
using VenueService.Repository;

namespace VenueService.Service
{
    public class VenueServices : IVenueService
    {
        private readonly IVenueRepository _venuerepository;
        private readonly HttpClient _httpClient;

        public VenueServices(IVenueRepository venueRepository, HttpClient httpClient)
        {
            _venuerepository = venueRepository;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://venueservice/");
        }
        public async Task<ResponseBody> CreateVenu(CreateVenueDto venueData)
        {
            try
            {
                var venue = new Venue
                {
                    Id = Guid.NewGuid(),
                    Name = venueData.Name,
                    Location = venueData.Location,
                    Capacity = venueData.Capacity,
                };

                var result = await _venuerepository.AddVenueAsync(venue);
                return result;
            }catch(Exception ex)
            {
                return new ResponseBody(false, $"An unexpected error occurred: {ex.Message}");
            }
  
        }

        public async Task<ResponseBody> GetVenueEventList(Guid venueId)
        {
            var venueDetails = await _venuerepository.GetVenue(venueId);

            if(venueDetails != null)
            {

            }
        }
    }
}
