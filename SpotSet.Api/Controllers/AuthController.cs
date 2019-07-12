using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotSet.Api.Services;

namespace SpotSet.Api.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ISpotifyService _spotifyService;

        public AuthController(ISpotifyService spotifyService)
        { 
            _spotifyService = spotifyService;
        }
    
        [HttpGet("/login")]
        public async Task<RedirectResult> GetUserAuthentication()
        {
            var result = await _spotifyService.GetUserAuthentication();
            return Redirect(result);
        }
        
        [HttpGet("/callback")]
        public async Task<ActionResult> GetUserAuthorization([FromQuery]string code)
        {
            var result = await _spotifyService.GetUserAuthorization(code);
            
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }
    }
}