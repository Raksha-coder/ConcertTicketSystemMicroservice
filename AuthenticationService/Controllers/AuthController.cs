using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config) => _config = config;

        [HttpPost("token")]
        public IActionResult GetToken([FromBody] AuthRequest request)
        {
            if (request.ClientId == "event-service" && request.ClientSecret == "event-secret")
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, request.ClientId),
                new Claim("service", request.ClientId)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }


        [HttpPost("user-token")]
        public IActionResult GetUserToken([FromBody] UserLoginRequest request)
        {
            //dummy user for now
            if (request.Username == "testuser" && request.Password == "password123")
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "User"), 
            new Claim("user_id", Guid.NewGuid().ToString()) 
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiresIn = 1800
                });
            }

            return Unauthorized();
        }

    }
}
