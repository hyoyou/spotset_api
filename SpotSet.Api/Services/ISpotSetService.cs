using System.Threading.Tasks;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public interface ISpotSetService
    {
        Task<SpotSetDto> GetSetlist(string setlistId);
    }
}