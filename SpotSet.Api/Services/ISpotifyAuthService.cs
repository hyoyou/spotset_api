using System.Threading.Tasks;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public interface ISpotifyAuthService
    {
        Task<string> GetAccessToken();
    }
}