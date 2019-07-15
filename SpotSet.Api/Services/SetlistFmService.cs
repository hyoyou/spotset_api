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
            var uri = $"setlist/{setlistId}";
            
            var httpClient = _httpClientFactory.CreateClient(HttpConstants.SetlistClient);
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var setlist = await DeserializeSetlist(response);
                setlist.Tracks = setlist.Sets.Set.SelectMany(s => s.Song).ToList();
                return setlist;
            }

            return null;
        }
        
        private static async Task<SetlistDto> DeserializeSetlist(HttpResponseMessage response)
        {
            var setlist = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SetlistDto>(setlist);
        }
    }
}