using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotSet.Api.Models;
using SpotSet.Api.Services;

namespace SpotSet.Api.Tests.Mocks
{
    public class MockSetlistService : ISetlistService
    {
        private HttpClient _httpClient;

        public MockSetlistService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Setlist> GetSetlist(string setlistId)
        {
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"http://test.com/{setlistId}");
                
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