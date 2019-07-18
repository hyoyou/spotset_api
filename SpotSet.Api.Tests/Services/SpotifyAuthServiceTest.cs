using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Constants;
using SpotSet.Api.Exceptions;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Helpers;
using SpotSet.Api.Tests.Mocks;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SpotifyAuthServiceTest
    {
        [Fact]
        public void GetAccessTokenReturnsASpotifyAccessTokenStringWhenRequestIsSuccess()
        {
            var testAccessToken = "{\"access_token\": \"testToken\"}";
            JObject parsedAccessToken = JObject.Parse(testAccessToken);
            
            var mockHttpClientFactory = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedAccessToken);
            var mockConfiguration = new MockConfiguration();
            var mockSpotifyAuthService = new SpotifyAuthService(mockHttpClientFactory, mockConfiguration);
            
            var result = mockSpotifyAuthService.GetAccessToken();
            
            Assert.Equal("testToken", result.Result);
        }
        
        [Fact]
        public void GetAccessTokenReturnsAnExceptionIfAccessTokenNotReturned()
        {
            var mockHttpClientFactory = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockConfiguration = new MockConfiguration();
            var mockSpotifyAuthService = new SpotifyAuthService(mockHttpClientFactory, mockConfiguration);

            var ex = Assert.ThrowsAsync<SpotifyAuthException>(() => mockSpotifyAuthService.GetAccessToken());
            Assert.Equal(ErrorConstants.SpotifyAuthError, ex.Result.Message);
        }
        
        [Fact]
        public void HttpClientIsReturnedWithTheCorrectHeaders()
        {
            var content = JsonConvert.SerializeObject("sample");
            var mockHttpMessageHandler = TestSetup.CreateMockHttpMessageHandler(HttpStatusCode.OK, content);
            var mockHttpClientFactory = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockConfiguration = new MockConfiguration();
            var mockSpotifyAuthService = new SpotifyAuthService(mockHttpClientFactory, mockConfiguration);

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            mockSpotifyAuthService.AddHeaders(mockHttpClient);
            
            Assert.Equal("application/json", mockHttpClient.DefaultRequestHeaders.Accept.ToString());
            Assert.Contains("Basic", mockHttpClient.DefaultRequestHeaders.Authorization.ToString());
        }
    }
}