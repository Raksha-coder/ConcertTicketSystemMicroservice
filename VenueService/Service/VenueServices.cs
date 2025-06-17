using Newtonsoft.Json;
using SharedLibrary.Token;
using VenueService.common;
using VenueService.Dto;
using VenueService.Model;
using VenueService.Repository;

namespace VenueService.Service
{
    public class VenueServices(IVenueRepository venueRepository,
        HttpClient httpClient,
        IConfiguration config,
        ILogger<VenueServices> logger, TokenProvider tokenProvider) : IVenueService
    {
        private readonly IVenueRepository _venuerepository = venueRepository;
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _eventServiceUrl = config["EventService:BaseUrl"];
        private readonly ILogger<VenueServices> _logger = logger;
        private readonly TokenProvider _tokenProvider = tokenProvider;

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
                _logger.LogError("An unexpected error occurred :{ex}", ex.Message);
                return new ResponseBody(false, $"An unexpected error occurred: {ex.Message}");
            }
  
        }

        public async Task<ResponseBody> GetVenueEventList(Guid venueId)
        {
            var venueDetails = await _venuerepository.GetVenue(venueId);

            if (venueDetails == null)
                return new ResponseBody(false, "Venue not found.");

            try
            {
                _logger.LogInformation("Successfully fetch venue.");

                var token = await _tokenProvider.GetTokenAsync();

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_eventServiceUrl}={venueId}");
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);


                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var eventList = JsonConvert.DeserializeObject<GenericResponse<List<EventDto>>>(json);

                    return new ResponseBody(true,"fetch venue successfully", new
                    {
                        Venue = venueDetails,
                        Events = eventList?.Data
                    });

                }
                else
                {
                    _logger.LogWarning("Failed to fetch events from EventService.");
                    return new ResponseBody(false, "Failed to fetch events from EventService.");
                }
            }
            catch (Exception ex) {
                _logger.LogError("Error calling EventService :{ex}",ex.Message);
                return new ResponseBody(false, $"Error calling EventService: {ex.Message}");
            }
        }
    }
}
