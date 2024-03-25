using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Repository.Abstract;

namespace SonicSpectrum.Presentation.Areas.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController (IMusicSettingService _musicSettingService) : ControllerBase
    {
        [HttpPost("addArtist")]
        public async Task<IActionResult> AddArtist([FromBody] ArtistDTO artistDto)
        {
            await _musicSettingService.AddArtistAsync(artistDto);
            return Ok("Artist added successfully.");
        }

        [HttpPost("addGenre")]
        public async Task<IActionResult> AddGenre([FromBody] GenreDTO genreDto)
        {
            await _musicSettingService.AddGenreAsync(genreDto);
            return Ok("Genre added successfully.");
        }

        [HttpPost("addTrack")]
        public async Task<IActionResult> AddTrack([FromForm] TrackDTO trackDto)
        {
            await _musicSettingService.AddTrackAsync(trackDto);
            return Ok("Track added successfully.");
        }

        [HttpDelete("deleteTrack/{trackId}")]
        public async Task<IActionResult> DeleteTrack(string trackId)
        {
            await _musicSettingService.DeleteTrackAsync(trackId);
            return Ok("Track deleted successfully.");
        }

        [HttpDelete("deleteArtist/{artistId}")]
        public async Task<IActionResult> DeleteArtist(string artistId)
        {
            await _musicSettingService.DeleteArtistAsync(artistId);
            return Ok("Artist deleted successfully.");
        }

        [HttpDelete("deleteGenre/{genreId}")]
        public async Task<IActionResult> DeleteGenre(string genreId)
        {
            await _musicSettingService.DeleteGenreAsync(genreId);
            return Ok("Genre deleted successfully.");
        }
    }
}
