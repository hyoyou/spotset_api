using System.Net;
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
            var accessToken = new SpotifyAccessToken { access_token = "accessToken" };
            
            var successSpotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK, accessToken);
            var result = successSpotifyService.GetAccessToken();
            
            Assert.Equal(accessToken.access_token, result.Result);
        }
        
        [Fact]
        public void GetUserAuthenticationReturnsASpotifyAuthenticationPageUrlWhenCalled()
        {
            var accessToken = new SpotifyAccessToken { access_token = "accessToken" };
            
            var successSpotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK, accessToken);
            var result = successSpotifyService.GetUserAuthentication();

            var expected =
                "https://accounts.spotify.com/authorize?client_id=SpotifyApiKey&response_type=code&redirect_uri=https%3a%2f%2flocalhost%3a5001%2fcallback&scope=playlist-modify-private+playlist-modify-public";
            
            Assert.Equal(expected, result.Result);
        }
        
        [Fact]
        public void GetUserAuthorizationReturnsASpotifyAccessTokenWhenCalled()
        {
            var accessToken = new SpotifyAccessToken { access_token = "accessToken" };
            
            var successSpotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK, accessToken);
            var result = successSpotifyService.GetUserAuthorization("code");

            Assert.Equal(accessToken.access_token, result.Result.access_token);
        }
    }
}