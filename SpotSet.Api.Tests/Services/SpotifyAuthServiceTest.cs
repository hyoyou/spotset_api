using System.Net;
using Newtonsoft.Json.Linq;
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
            Assert.Equal("There was an error authenticating the app.", ex.Result.Message);
        }
    }
}