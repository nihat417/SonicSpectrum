﻿using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Repository.Abstract;

namespace SonicSpectrum.Presentation.Areas.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController (IUnitOfWork _unitOfWork) : ControllerBase
    {
        #region getmethods

        [HttpGet("getTrackById/{trackId}")]
        public async Task<IActionResult> GetTrackById(string trackId)
        {
            try
            {
                var music = await _unitOfWork.MusicSettingService.GetTrackById(trackId);
                if (music == null) return NotFound();
                return Ok(music);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("allmusics")]
        public async Task<IActionResult> GetAllMusics(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tracks = await _unitOfWork.MusicSettingService.GetAllTracksAsync(pageNumber, pageSize);
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("allartists")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllArtists(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var artists = await _unitOfWork.MusicSettingService.GetAllArtistsAsync(pageNumber, pageSize);
                if (artists == null) return NotFound();
                return Ok(artists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getallalbumsforartist/{artistId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllAlbumsForArtist(string artistId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var albums = await _unitOfWork.MusicSettingService.GetAllAlbumsForArtistAsync(artistId, pageNumber, pageSize);
                if (albums == null || !albums.Any()) return NotFound();
                return Ok(albums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetMusicForAlbum/{albumId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMusicForAlbum(string albumId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tracks = await _unitOfWork.MusicSettingService.GetMusicFromAlbum(albumId, pageNumber, pageSize);
                if (tracks == null) return NotFound();
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}/playlists")]
        public async Task<IActionResult> GetPlaylistsFromUser(string userId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var playlists = await _unitOfWork.MusicSettingService.GetPlaylistFromUser(userId, pageNumber, pageSize);
                if(playlists == null) return NotFound();
                else return Ok(playlists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }

        [HttpGet("playlist/{playlistId}/tracks")]
        public async Task<IActionResult> GetMusicFromPlaylist(string playlistId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tracks = await _unitOfWork.MusicSettingService.GetMusicFromPlaylist(playlistId, pageNumber, pageSize);
                if (tracks == null) return NotFound();
                else return Ok(tracks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region postmethods

        [HttpPost("createPlaylist")]
        public async Task<IActionResult> CreatePlaylistAsync([FromForm] PlaylistDTO requestDto)
        {
            try
            {
                var result = await _unitOfWork.MusicSettingService.CreatePlaylistAsync(requestDto);
                if (result.Success) return Ok(result.Message);
                else return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("addTrackPlaylist")]
        public async Task<IActionResult> AddTrackToPlaylistAsync([FromBody] TrackPlaylistDTO requestDto)
        {
            try
            {
                var result = await _unitOfWork.MusicSettingService.AddTrackToPlaylistAsync(requestDto);
                if (result.Success) return Ok(result.Message);
                else return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion
    }
}
