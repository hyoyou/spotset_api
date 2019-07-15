using System.Net;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Exceptions;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SpotifyAuthServiceTest
    {
        [Fact]
        public void GetAccessTokenReturnsASpotifyAccessTokenModelWhenCalled()
        {
            var testAccessToken = "{\"access_token\": \"testToken\"}";
            JObject parsedAccessToken = JObject.Parse(testAccessToken);
            
            var successSpotifyAuthService = TestSetup.CreateSpotifyAuthServiceWithMocks(HttpStatusCode.OK, parsedAccessToken);
            var result = successSpotifyAuthService.GetAccessToken();
            
            Assert.Equal("testToken", result.Result);
        }
        
        [Fact]
        public void GetAccessTokenReturnsAnExceptionIfAccessTokenNotReturned()
        {
            var spotifyAuthService = TestSetup.CreateSpotifyAuthServiceWithMocks(HttpStatusCode.NotFound);

            var ex = Assert.ThrowsAsync<SpotifyAuthException>(() => spotifyAuthService.GetAccessToken());
            Assert.Equal("There was an error with authenticating the app.", ex.Result.Message);
        }
    }
}