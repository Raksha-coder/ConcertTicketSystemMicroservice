using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VenueService.common;
using VenueService.Data;
using VenueService.Model;
using VenueService.Service;

namespace VenueService.Repository
{
    public class VenueRepository : IVenueRepository
    {
        private readonly VenueDbContext _context;
        private readonly ILogger<VenueRepository> _logger;
        public VenueRepository(VenueDbContext context, ILogger<VenueRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseBody> AddVenueAsync(Venue venue)
        {
            await _context.Venues.AddAsync(venue);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Venue created successfully.");
            return new ResponseBody(true, "Venue created successfully.");
        }

        public async Task<Venue?> GetVenue(Guid venueId)
        {
            return await _context.Venues.Where(ven => ven.Id == venueId).FirstOrDefaultAsync();
        }
    }
}
