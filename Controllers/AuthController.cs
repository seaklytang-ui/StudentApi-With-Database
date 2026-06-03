using Microsoft.AspNetCore.Mvc;
using StudentApi.DTOs;
using StudentApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace StudentApi.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _Service;

            public AuthController(IAuthService service)
            {
                _Service = service;
            }

            [HttpPost("login")]
            public async Task<ActionResult> Login(
                LoginDto dto)
            {
                var result = await _Service.LoginAsync(dto);
                return Ok(result);
            }

            [HttpPost("refresh")]
            public async Task<ActionResult> Refresh(
                string refreshToken)
            {
                var result = await _Service.RefreshTokenAsync(refreshToken);
                return Ok(result);
            }


        [Authorize]
        [HttpGet("Test-auth")]
        public IActionResult TestAuth()
        {
            return Ok(new
            {
                User = User.Identity?.Name,
                Roles = User.Claims
                    .Where(c => c.Type.Contains("role"))
                    .Select(c => c.Value)
            });
        }
    }

    
    
}
