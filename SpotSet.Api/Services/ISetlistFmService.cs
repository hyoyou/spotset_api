using System.Threading.Tasks;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public interface ISetlistFmService
    {
        Task<SetlistDto> SetlistRequest(string setlistId);
    }
}