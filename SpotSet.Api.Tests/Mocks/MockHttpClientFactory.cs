using System;
using System.Net.Http;

namespace SpotSet.Api.Tests.Mocks
{
    public class MockHttpClientFactory : IHttpClientFactory
    {
        public HttpClient HttpClient;
        public MockHttpClientFactory(HttpClient httpClient)
        {
            HttpClient = httpClient;
            HttpClient.BaseAddress = new Uri("https://test.com");
        }
        public HttpClient CreateClient(string name)
        {
            return HttpClient;
        }
    }
}