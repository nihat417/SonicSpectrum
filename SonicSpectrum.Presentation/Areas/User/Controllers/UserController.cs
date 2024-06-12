﻿using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Repository.Abstract;

namespace SonicSpectrum.Presentation.Areas.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] FollowDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.FollowerId) || string.IsNullOrEmpty(dto.FolloweeId))
                return BadRequest("Invalid request data.");

            var result = await _unitOfWork.FollowService.FollowUserAsync(dto.FollowerId, dto.FolloweeId);
            if (result.Success) return Ok(result.Message);

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptFollowRequest([FromBody] FollowDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.FollowerId) || string.IsNullOrEmpty(dto.FolloweeId))
                return BadRequest("Invalid request data.");

            var result = await _unitOfWork.FollowService.AcceptFollowRequestAsync(dto.FollowerId, dto.FolloweeId);
            if (result.Success) return Ok(result.Message);

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] FollowDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.FollowerId) || string.IsNullOrEmpty(dto.FolloweeId))
                return BadRequest("Invalid request data.");

            var result = await _unitOfWork.FollowService.UnfollowUserAsync(dto.FollowerId, dto.FolloweeId);
            if (result.Success) return Ok(result.Message);

            return BadRequest(result.ErrorMessage);
        }
    }
}

