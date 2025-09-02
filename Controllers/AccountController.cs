using BikeDealersProject.AuthModels;
using BikeDealersProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeDealersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var (status, message) = await _authService.RegisterAsync(model);
            return status == 1 ? Ok(message) : BadRequest(message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var (status, token, roles) = await _authService.LoginAsync(model);
            return status == 1 ? Ok(new { Token = token, Role = roles }) : Unauthorized(token);
        }
    }
}
