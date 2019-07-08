using System.Threading.Tasks;

namespace SpotSet.Api.Services
{
    public interface ISpotifyService
    {
        Task<string> GetAccessToken();
    }
}