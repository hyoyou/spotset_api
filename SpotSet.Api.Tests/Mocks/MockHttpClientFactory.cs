using System;
using System.Net.Http;

namespace SpotSet.Api.Tests.Mocks
{
    public class MockHttpClientFactory : IHttpClientFactory
    {
        private HttpClient _httpClient;
        public MockHttpClientFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://test.com");
        }
        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}