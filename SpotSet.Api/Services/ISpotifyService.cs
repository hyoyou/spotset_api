using System.Threading.Tasks;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public interface ISpotifyService
    {
        Task<string> GetAccessToken();
        Task<string> GetUserAuthentication();
        Task<SpotifyAccessToken> GetUserAuthorization(string code);
    }
}