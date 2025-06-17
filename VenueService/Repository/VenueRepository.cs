using Microsoft.EntityFrameworkCore;
using VenueService.common;
using VenueService.Data;
using VenueService.Model;

namespace VenueService.Repository
{
    public class VenueRepository : IVenueRepository
    {
        private readonly VenueDbContext _context;
        public VenueRepository(VenueDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseBody> AddVenueAsync(Venue venue)
        {
            try
            {
                await _context.Venues.AddAsync(venue);
                await _context.SaveChangesAsync();
                return new ResponseBody(true, "Venue created successfully.");

            }
            catch(Exception ex)
            {
                return new ResponseBody(false, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Venue?> GetVenue(Guid venueId)
        {
                return  await _context.Venues.Where(ven => ven.Id == venueId).FirstOrDefaultAsync();

        }
    }
}
