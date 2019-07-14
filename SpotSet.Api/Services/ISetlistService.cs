using System.Threading.Tasks;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public interface ISetlistService
    {
        Task<SpotSetDto> GetSetlist(string setlistId);
        Task<SpotifyTracksModel> SpotifyRequest(SetlistDto setlistmodel);
    }
}