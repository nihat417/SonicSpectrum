using Microsoft.AspNetCore.Mvc;
using SonicSpectrum.Application.Repository.Abstract;

namespace SonicSpectrum.Presentation.Areas.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController () : ControllerBase
    {

        [HttpGet("allmusics")]
        public Task<IActionResult> GetAllMusics()
        {

        }

        
    }
}
