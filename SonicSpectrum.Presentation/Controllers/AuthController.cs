using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Models;
using SonicSpectrum.Application.Repository.Abstract;
using SonicSpectrum.Domain.Entities;

namespace SonicSpectrum.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService, UserManager<User> _userManager, IEmailService _emailService) : ControllerBase
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
            if(response != null) {
                var user = await _userManager.FindByEmailAsync(registerDTO.Email);
                if(user != null)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);
                    var confirmLink = Url.Action("ConfirmEmail", "Auth", new { token, email = registerDTO.Email }, Request.Scheme);
                    var message = new Message(new string[] { registerDTO.Email }, "Confirmation Email Link", confirmLink!);
                    _emailService.SendEmail(message);
                    return Ok(response);
                }
                return BadRequest();
            }
            return BadRequest();
        }



        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(); // Возвращаем ошибку 404, если пользователь не найден
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    // Здесь можно добавить дополнительные действия при успешном подтверждении email, если это необходимо
                    return Ok(); // Возвращаем успешный результат
                }
                else
                {
                    // Возвращаем ошибку с сообщением о неудачном подтверждении email
                    return BadRequest("Failed to confirm email");
                }
            }
            catch (Exception ex)
            {
                // Возвращаем ошибку сервера с дополнительной информацией
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
