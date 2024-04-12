using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.Repository.Abstract;
using SonicSpectrum.Application.Repository.Concrete;

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



    }
}
