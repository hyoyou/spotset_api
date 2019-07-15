using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SpotSet.Api.Tests.Mocks;

namespace SpotSet.Api.Tests.Helpers
{
    public static class TestSetup
    {
        public static MockHttpClientFactory CreateMockHttpClientFactory(HttpStatusCode statusCode, object content = null)
        {
            var serializedSetlist = SerializeObject(content);
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(statusCode, serializedSetlist);
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            return new MockHttpClientFactory(mockHttpClient);
        }

        private static string SerializeObject(object content)
        {
            return JsonConvert.SerializeObject(content);
        }

        private static Mock<HttpMessageHandler> CreateMockHttpMessageHandler(HttpStatusCode statusCode, string serializedContent)
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
    }
}