using System.Net;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SpotSetServiceTest
    {
        [Fact]
        public async void GetSetlistReturnsASpotSetDtoWhenCalledWithSetlistIdThatReturnsValidData()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": []}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);
            
            var setlistService = TestSetup.CreateSetlistFmServiceWithMocks(HttpStatusCode.OK, parsedSetlist);
            var setlistDto = setlistService.SetlistRequest("testId");
            
            var successSpotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK);
            var spotifyDto = await successSpotifyService.SpotifyRequest(setlistDto.Result);
            
            var successSpotSetService = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.OK, parsedSetlist);
            var result = await successSpotSetService.GetSetlist("testId");
    
            Assert.IsType<SpotSetDto>(result);
            Assert.Equal("testId", result.Id);
            Assert.Equal("07-30-2019", result.EventDate);
            Assert.Equal("artistName", result.Artist);
            Assert.Equal("venueName", result.Venue);
        }
    }
}