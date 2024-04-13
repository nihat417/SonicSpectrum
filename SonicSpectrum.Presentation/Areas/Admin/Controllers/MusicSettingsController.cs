using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Repository.Abstract;

namespace SonicSpectrum.Presentation.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicSettingsController(IMusicSettingService _musicSettingService) : ControllerBase
    {

        #region Post

        [HttpPost("addArtist")]
        public async Task<IActionResult> AddArtist([FromBody] ArtistDTO artistDto)
        {
            var result = await _musicSettingService.AddArtistAsync(artistDto);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("addAlbum")]
        public async Task<IActionResult> AddAlbum([FromForm] AlbumDto albumDto)
        {
            var result = await _musicSettingService.AddAlbumAsync(albumDto);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("addGenre")]
        public async Task<IActionResult> AddGenre([FromBody] GenreDTO genreDto)
        {
            var result = await _musicSettingService.AddGenreAsync(genreDto);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("addTrack")]
        public async Task<IActionResult> AddTrack([FromForm] TrackDTO trackDto)
        {
            var result = await _musicSettingService.AddTrackAsync(trackDto);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("addGenreToTrack/{trackId}/{genreName}")]
        public async Task<IActionResult> AddGenreToTrack(string trackId, string genreName)
        {
            var operationResult = await _musicSettingService.AddGenreToTrackAsync(trackId, genreName);

            if (operationResult.Success) return Ok(operationResult.Message);
            else return BadRequest(operationResult.ErrorMessage);
        }


        [HttpPost("addLyricsToTrack/{trackId}")]
        public async Task<IActionResult> AddLyricsToTrack(string trackId, [FromBody] string lyricsText)
        {
            if (string.IsNullOrEmpty(lyricsText))return BadRequest("Lyrics text is null or empty.");
            var operationResult = await _musicSettingService.AddLyricsToTrackAsync(trackId, lyricsText);
            if (operationResult.Success) return Ok(operationResult.Message);
            else return BadRequest(operationResult.ErrorMessage);
        }




        #endregion

        #region Put

        [HttpPut("editAlbum/{albumId}")]
        public async Task<IActionResult> EditAlbum(string albumId, [FromBody] AlbumDto albumDto)
        {
            var result = await _musicSettingService.EditAlbumAsync(albumId, albumDto);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("editArtist/{artistId}")]
        public async Task<IActionResult> EditArtist(string artistId, [FromBody] ArtistDTO artistDto)
        {
            var result = await _musicSettingService.EditArtistAsync(artistId, artistDto);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("editGenre/{genreId}")]
        public async Task<IActionResult> EditGenre(string genreId, [FromBody] GenreDTO genreDto)
        {
            var result = await _musicSettingService.EditGenreAsync(genreId, genreDto);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("editTrack/{trackId}")]
        public async Task<IActionResult> EditTrack(string trackId, [FromBody] TrackDTO trackDto)
        {
            var result = await _musicSettingService.EditTrackAsync(trackId, trackDto);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }


        #endregion

        #region delete

        [HttpDelete("deleteAlbum/{albumId}")]
        public async Task<IActionResult> DeleteAlbum(string albumId)
        {
            var result = await _musicSettingService.DeleteAlbumAsync(albumId);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("deleteTrack/{trackId}")]
        public async Task<IActionResult> DeleteTrack(string trackId)
        {
            var result = await _musicSettingService.DeleteTrackAsync(trackId);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("deleteArtist/{artistId}")]
        public async Task<IActionResult> DeleteArtist(string artistId)
        {
            var result = await _musicSettingService.DeleteArtistAsync(artistId);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("deleteGenre/{genreId}")]
        public async Task<IActionResult> DeleteGenre(string genreId)
        {
            var result = await _musicSettingService.DeleteGenreAsync(genreId);
            if (result.Success) return Ok(result.Message);
            return BadRequest(result.ErrorMessage);
        }

        #endregion

    }
}
