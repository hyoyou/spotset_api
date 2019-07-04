using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public class SetlistService : ISetlistService
    {
        private static IHttpClientFactory _httpFactory;

        public SetlistService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<Setlist> GetSetlist(string setlistId)
        {
            {
                var httpClient = _httpFactory.CreateClient("GetSetlistClient");
                HttpResponseMessage response = await httpClient.GetAsync(setlistId);
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var setlist = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Setlist>(setlist);
                }
            }

            return null;
        }
    }
}