using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Repository.Abstract;

namespace SonicSpectrum.Presentation.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicSettingsController(IMusicSettingService _musicSettingService) : ControllerBase
    {
        [HttpPost("addArtist")]
        public async Task<IActionResult> AddArtist([FromBody] ArtistDTO artistDto)
        {
            var artistAdded = await _musicSettingService.AddArtistAsync(artistDto);
            if (artistAdded) return Ok("Artist added successfully.");
            return BadRequest("Artist Is null");
        }

        [HttpPost("addAlbum")]
        public async Task<IActionResult> AddAlbum([FromForm] AlbumDto albumDto)
        {
            var albumAdded = await _musicSettingService.AddAlbumAsync(albumDto);
            if (albumAdded) return Ok("Album added successfully.");
            return BadRequest("Error");
        }

        [HttpPost("addGenre")]
        public async Task<IActionResult> AddGenre([FromBody] GenreDTO genreDto)
        {
            var genreAdded = await _musicSettingService.AddGenreAsync(genreDto);
            if(genreAdded) return Ok("Genre added successfully.");
            return BadRequest("This Genre Already exist");
        }

        [HttpPost("addTrack")]
        public async Task<IActionResult> AddTrack([FromForm] TrackDTO trackDto)
        {
            var trackAdded = await _musicSettingService.AddTrackAsync(trackDto);
            if(trackAdded) return Ok("Track added successfully.");
            return BadRequest("Error");
        }

        [HttpDelete("deleteAlbum/{albumId}")]
        public async Task<IActionResult> DeleteAlbum(string albumId)
        {
            var albumDeleted = await _musicSettingService.DeleteAlbumAsync(albumId);
            if(albumDeleted) return Ok("Album deleted successfully.");
            return BadRequest("Error");
        }

        [HttpDelete("deleteTrack/{trackId}")]
        public async Task<IActionResult> DeleteTrack(string trackId)
        {
            var trackDeleted = await _musicSettingService.DeleteTrackAsync(trackId);
            if(trackDeleted) return Ok("Track deleted successfully.");
            return BadRequest("Error");
        }

        [HttpDelete("deleteArtist/{artistId}")]
        public async Task<IActionResult> DeleteArtist(string artistId)
        {
            var artistDeeleted = await _musicSettingService.DeleteArtistAsync(artistId);
            if(artistDeeleted) return Ok("Artist deleted successfully.");
            return BadRequest("Error");
        }

        [HttpDelete("deleteGenre/{genreId}")]
        public async Task<IActionResult> DeleteGenre(string genreId)
        {
            var genreDeleted = await _musicSettingService.DeleteGenreAsync(genreId);
            if(genreDeleted) return Ok("Genre deleted successfully.");
            return BadRequest("Error");
        }
    }
}
