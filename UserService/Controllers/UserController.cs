using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Response;
using UserService.DTO;
using UserService.Service;

namespace UserService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userservice;
        public UserController(IUserServices service)
        {
            _userservice = service;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestDto request)
        {
            if (request == null)
                return BadRequest(new ResponseBody(false, "Invalid data"));

            var result = await _userservice.RegisterUserAsync(request);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserRegisterRequestDto request)
        {
            if (request == null)
                return BadRequest(new ResponseBody(false, "Invalid data"));

            var result = await _userservice.LoginUserAsync(request);

            return Ok(result);
        }

    }
}
