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
            // TODO
            var url = $"https://api.setlist.fm/rest/1.0/setlist/{setlistId}";

            {
                var httpClient = _httpFactory.CreateClient();
                // TODO
                httpClient.DefaultRequestHeaders.Add("x-api-key", "dijireBcABogLtlblNC9u8lEYo0MDsSu8blF");
                // TODO
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                HttpResponseMessage response = await httpClient.GetAsync(url);
                
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