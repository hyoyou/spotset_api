using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotSet.Api.Services;

namespace SpotSet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetlistsController : Controller
    {
        private readonly ISpotSetService _spotSetService;

        public SetlistsController(ISpotSetService spotSetService)
        { 
            _spotSetService = spotSetService;
        }
    
        [HttpGet("{setlistId}")]
        public async Task<ActionResult> GetSetlist(string setlistId)
        {
            try
            {
                var result = await _spotSetService.GetSetlist(setlistId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}