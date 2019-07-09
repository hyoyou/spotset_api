using System.Threading.Tasks;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public interface ISetlistService
    {
        Task<SetlistDto> GetSetlist(string setlistId);
        Task<Setlist> SetlistRequest(string setlistId);
        Task<SpotifyTracksDto> SpotifyRequest(Setlist setlistmodel);
    }
}