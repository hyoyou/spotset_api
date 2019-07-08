using System.Net;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SpotifyServiceTest
    {
        [Fact]
        public void GetAccessTokenReturnsASpotifyAccessTokenModelWhenCalled()
        {
            var accessToken = new SpotifyAccessToken { access_token = "accessToken" };
            
            var successSpotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK, accessToken);
            var result = successSpotifyService.GetAccessToken();
            
            Assert.Equal(accessToken.access_token, result.Result);
        }
    }
}