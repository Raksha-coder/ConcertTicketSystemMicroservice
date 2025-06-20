﻿using EventService.common;
using EventService.DTO.RequestDto;
using EventService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("createEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventData)
        {
            if (eventData == null)
                return BadRequest(new ResponseBody(false, "Please provide event data"));

            var result = await _eventService.CreateEvent(eventData);
            return Ok(result);
            
        }

        [HttpPut("updateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] CreateEventDto eventData)
        {
            if (eventData == null)
                return BadRequest(new ResponseBody(false, "Please provide event data"));

            var result = await _eventService.UpdateEvent(eventData);
            return Ok(result);
        }

        [HttpGet("getEvents")]
        public async Task<IActionResult> GetEvents()
        {
            var eventsList = await _eventService.GetAllEvent();

            if (eventsList.Data == null)
                return NotFound(new ResponseBody(false, "Events not found."));

            return Ok(eventsList);
        }



        //get all events based on venueId

        [HttpGet("getEventsByVenueId")]
        public async Task<IActionResult> GetEventsByVenueId(Guid venueId)
        {
            if (venueId == Guid.Empty)
                return BadRequest(new ResponseBody(false, "Please provide valid venue"));

            var venueEventList = await _eventService.GetAllEventsByVenueId(venueId);

            return Ok(venueEventList);
        }


        //Test if global exception is working or not
        [HttpGet("throw")]
        public IActionResult ThrowException()
        {
            throw new Exception("Test exception for global middleware");
        }
    }
}
