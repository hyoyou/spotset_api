using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotSet.Api.Services;

namespace SpotSet.Api.Controllers
{
    [Route("api/[controller]")]
    public class SetlistController : Controller
    {
        private readonly ISetlistService _setlistService;

        public SetlistController(ISetlistService setlistService)
        { 
            _setlistService = setlistService;
        }
    
        [HttpGet("{setlistId}")]
        public async Task<ActionResult> GetSetlist(string setlistId)
        {
            var result = await _setlistService.GetSetlist(setlistId);

            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }
    }
}