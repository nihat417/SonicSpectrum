using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.Repository.Abstract;

namespace SonicSpectrum.Presentation.Areas.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController (IMusicSettingService _musicSettingService) : ControllerBase
    {

        [HttpGet("allmusics")]
        public async Task<IActionResult> GetAllMusics(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tracks = await _musicSettingService.GetAllTracksAsync(pageNumber, pageSize);
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
                var artists = await _musicSettingService.GetAllArtistsAsync(pageNumber, pageSize);
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
                var albums = await _musicSettingService.GetAllAlbumsForArtistAsync(artistId, pageNumber, pageSize);
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
                var tracks = await _musicSettingService.GetMusicFromAlbum(albumId, pageNumber, pageSize);
                if (tracks == null) return NotFound();
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
