using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SpotSet.Api.Models;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Mocks;

namespace SpotSet.Api.Tests.Helpers
{
    public static class TestSetup
    {
        public static string SerializeObject(object content)
        {
            return JsonConvert.SerializeObject(content);
        }
        
        public static Mock<HttpMessageHandler> CreateMockHttpMessageHandler(HttpStatusCode statusCode, string serializedContent)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(serializedContent, Encoding.UTF8, "application/json")
                })
                .Verifiable();
            
            return mockHttpMessageHandler;
        }
        
        public static SetlistService CreateSetlistServiceWithMocks(HttpStatusCode statusCode, object content = null)
        {
            var serializedSetlist = SerializeObject(content);
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(statusCode, serializedSetlist);
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory= new MockHttpClientFactory(mockHttpClient);
            
            return new SetlistService(mockHttpClientFactory);
        }

        public static SpotifyService CreateSpotifyServiceWithMocks(HttpStatusCode statusCode, SpotifyAccessToken accessToken)
        {
            var serializedToken = SerializeObject(accessToken);
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(statusCode, serializedToken);
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory= new MockHttpClientFactory(mockHttpClient);
            var mockConfiguration = new MockConfiguration();
            
            return new SpotifyService(mockHttpClientFactory, mockConfiguration);
        }
    }
}