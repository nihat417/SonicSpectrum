using Azure;
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

        [HttpPost("ForgotPassword")]
        
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return BadRequest("User not found or email is not confirmed.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Auth", new { token, email }, Request.Scheme);
            var message = new Message(new string[] { email }, "Reset Password Link", resetLink!);
            _emailService.SendEmail(message);

            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
                return Ok("Password has been reset successfully.");
            else
                return BadRequest("Failed to reset password.");
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found.");

            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!passwordCheckResult)
                return BadRequest("Current password is incorrect.");

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (changePasswordResult.Succeeded)
                return Ok("Password has been changed successfully.");
            else
                return BadRequest("Failed to change password.");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return NotFound();

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                    return Ok();
                else
                    return BadRequest("Failed to confirm email");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
