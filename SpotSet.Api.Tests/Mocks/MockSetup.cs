using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SpotSet.Api.Models;
using SpotSet.Api.Services;

namespace SpotSet.Api.Tests.Mocks
{
    public static class MockSetup
    {
        public static Setlist CreateSetlist(string id, string eventDate, Artist artistData, Venue venueData, Sets setsData)
        {
            return new Setlist
            {
                id = id,
                eventDate = eventDate,
                artist = artistData,
                venue = venueData,
                sets = setsData
            };
        }
        
        private static string SerializeSetlist(Setlist newSetlist)
        {
            return JsonConvert.SerializeObject(newSetlist);
        }
        
        private static Mock<HttpMessageHandler> CreateMockSuccessHttpMessageHandler(string serializedSetlist)
        {
            var mockSuccessHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockSuccessHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(serializedSetlist, Encoding.UTF8, "application/json")
                })
                .Verifiable();
            
            return mockSuccessHttpMessageHandler;
        }
        
        private static Mock<HttpMessageHandler> CreateMockErrorHttpMessageHandler()
        {
            var mockErrorHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockErrorHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("content not found", Encoding.UTF8, "application/json")
                })
                .Verifiable();
            
            return mockErrorHttpMessageHandler;
        }

        public static SetlistService CreateSuccessSetlistServiceWithMocks(Setlist newSetlist)
        {
            var serializedSetlist = SerializeSetlist(newSetlist);
            var mockSuccessHttpMessageHandler = CreateMockSuccessHttpMessageHandler(serializedSetlist);
            var mockHttpClient = new HttpClient(mockSuccessHttpMessageHandler.Object);
            var mockHttpClientFactory= new MockHttpClientFactory(mockHttpClient);
            
            return new SetlistService(mockHttpClientFactory);
        }

        public static SetlistService CreateErrorSetlistServiceWithMocks()
        {
            var mockErrorHttpMessageHandler = CreateMockErrorHttpMessageHandler();
            var mockHttpClient = new HttpClient(mockErrorHttpMessageHandler.Object);
            var mockHttpClientFactory= new MockHttpClientFactory(mockHttpClient);
            
            return new SetlistService(mockHttpClientFactory);
        }
    }
}