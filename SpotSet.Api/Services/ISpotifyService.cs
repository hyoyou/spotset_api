using System.Threading.Tasks;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public interface ISpotifyService
    {
        Task<SpotifyTracksModel> SpotifyRequest(SetlistDto setlistmodel);
    }
}