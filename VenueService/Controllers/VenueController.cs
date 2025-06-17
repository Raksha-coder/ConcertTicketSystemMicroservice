using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VenueService.common;
using VenueService.Dto;
using VenueService.Model;
using VenueService.Service;

namespace VenueService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueservice;
        public VenueController(IVenueService service)
        {
            _venueservice = service;
        }

        [HttpPost("createVenue")]
        public async Task<IActionResult> CreateVenue([FromBody] CreateVenueDto venueData)
        {
            if (venueData == null)
                return BadRequest(new ResponseBody(false, "Invalid data"));

            var result = await _venueservice.CreateVenu(venueData);

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("getVenueEventList")]
        public async Task<IActionResult> GetVenueEventList(Guid venueId)
        {
            if (venueId == Guid.Empty)
                return BadRequest(new ResponseBody(false, "Invalid venueId"));

            var result = await _venueservice.GetVenueEventList(venueId);

            return Ok(result);

        }
        }
}
