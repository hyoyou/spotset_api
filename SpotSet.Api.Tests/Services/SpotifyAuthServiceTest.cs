using System.Net;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Exceptions;
using SpotSet.Api.Models;
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
        
        [Fact]
        public void GetUserAuthenticationReturnsASpotifyAuthenticationPageUrlWhenCalled()
        {
            var accessToken = new SpotifyAccessToken { access_token = "accessToken" };
            
            var successSpotifyAuthService = TestSetup.CreateSpotifyAuthServiceWithMocks(HttpStatusCode.OK, accessToken);
            var result = successSpotifyAuthService.GetUserAuthentication();

            var expected =
                "https://accounts.spotify.com/authorize?client_id=SpotifyApiKey&response_type=code&redirect_uri=https%3a%2f%2flocalhost%3a5001%2fcallback&scope=playlist-modify-private+playlist-modify-public";
            
            Assert.Equal(expected, result.Result);
        }
        
        [Fact]
        public void GetUserAuthorizationReturnsASpotifyAccessTokenWhenCalled()
        {
            var accessToken = new SpotifyAccessToken { access_token = "accessToken" };
            
            var successSpotifyAuthService = TestSetup.CreateSpotifyAuthServiceWithMocks(HttpStatusCode.OK, accessToken);
            var result = successSpotifyAuthService.GetUserAuthorization("code");

            Assert.Equal(accessToken.access_token, result.Result.access_token);
        }
    }
}