using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotSet.Api.Constants;
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
                var response = await setlistRequest(setlistId);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await DeserializeObject(response);
                }
            }

            return null;
        }

        private static async Task<HttpResponseMessage> setlistRequest(string setlistId)
        {
            var uri = $"setlist/{setlistId}";
            
            var httpClient = _httpFactory.CreateClient(HttpConstants.SetlistClient);
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return response;
        }

        private static async Task<Setlist> DeserializeObject(HttpResponseMessage response)
        {
            var setlist = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Setlist>(setlist);
        }

    }
}