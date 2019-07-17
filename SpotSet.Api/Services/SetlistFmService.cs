using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotSet.Api.Constants;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public class SetlistFmService : ISetlistFmService
    {
        private IHttpClientFactory _httpClientFactory;
        
        public SetlistFmService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<SetlistDto> SetlistRequest(string setlistId)
        {
            var httpClient = CreateHttpClient();
            var response = await SendRequest(setlistId, httpClient);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var setlist = await DeserializeSetlist(response);
                return AddTracksField(setlist);
            }

            return null;
        }
        
        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient(ApiConstants.SetlistClient);
            return httpClient;
        }

        private static async Task<HttpResponseMessage> SendRequest(string setlistId, HttpClient httpClient)
        {
            var uri = ApiConstants.SetlistSearchUri + setlistId;
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return response;
        }

        private static async Task<SetlistDto> DeserializeSetlist(HttpResponseMessage response)
        {
            var setlist = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SetlistDto>(setlist);
        }
        
        private static SetlistDto AddTracksField(SetlistDto setlist)
        {
            setlist.Tracks = setlist.Sets.Set.SelectMany(s => s.Song).ToList();
            return setlist;
        }
    }
}