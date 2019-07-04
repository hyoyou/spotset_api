using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SpotSet.Api.Models;
using SpotSet.Api.Services;

namespace SpotSet.Api.Tests.Mocks
{
    public class MockSetlistService : ISetlistService
    {
        private static IHttpClientFactory _httpFactory;

        public MockSetlistService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<Setlist> GetSetlist(string setlistId)
        {
            var httpClient = _httpFactory.CreateClient();
               
            HttpResponseMessage response = await httpClient.GetAsync("http://test.com");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await response.Content.ReadAsStringAsync();
                return new Setlist();
            }

            return null;
        }
    }
}