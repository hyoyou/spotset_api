using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotSet.Api.Services;

namespace SpotSet.Api.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ISpotifyAuthService _spotifyAuthService;

        public AuthController(ISpotifyAuthService spotifyAuthService)
        { 
            _spotifyAuthService = spotifyAuthService;
        }
    
        [HttpGet("/login")]
        public async Task<RedirectResult> GetUserAuthentication()
        {
            var result = await _spotifyAuthService.GetUserAuthentication();
            return Redirect(result);
        }
        
        [HttpGet("/callback")]
        public async Task<ActionResult> GetUserAuthorization([FromQuery]string code)
        {
            var result = await _spotifyAuthService.GetUserAuthorization(code);
            
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }
    }
}