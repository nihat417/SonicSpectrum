using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Repository.Abstract;

namespace SonicSpectrum.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await _authService.Login(loginDTO);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult>Register(RegisterDTO registerDTO)
        {
            var response = await _authService.Register(registerDTO);
            return Ok(response);
        }
    }
}
