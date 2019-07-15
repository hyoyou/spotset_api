using System.Net;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Exceptions;
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
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle1\"}, {\"name\": \"songTitle2\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);
            
            var setlistService = TestSetup.CreateSetlistFmServiceWithMocks(HttpStatusCode.OK, parsedSetlist);
            var setlistDto = setlistService.SetlistRequest("testId");
            
            var successSpotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK);
            await successSpotifyService.SpotifyRequest(setlistDto.Result);
            
            var successSpotSetService = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.OK, parsedSetlist);
            var result = await successSpotSetService.GetSetlist("testId");
    
            Assert.IsType<SpotSetDto>(result);
            Assert.Equal("testId", result.Id);
            Assert.Equal("07-30-2019", result.EventDate);
            Assert.Equal("artistName", result.Artist);
            Assert.Equal("venueName", result.Venue);
        }

        [Fact]
        public async void GetSetlistReturnsSetlistExceptionWhenSetlistFmServiceResultsInError()
        {
            var setlistService = TestSetup.CreateSetlistFmServiceWithMocks(HttpStatusCode.NotFound);
            await setlistService.SetlistRequest("testId");
            
            var spotSetService = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.NotFound);
            
            var ex = Assert.ThrowsAsync<SetlistNotFoundException>(() => spotSetService.GetSetlist("testId"));
            Assert.Equal("There was an error fetching the requested setlist!", ex.Result.Message);
        }
        
        [Fact]
        public async void GetSetlistReturnsSpotifyExceptionWhenSpotifyServiceResultsInAnEmptySpotifyTracksDto()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": []}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);
            
            var setlistService = TestSetup.CreateSetlistFmServiceWithMocks(HttpStatusCode.OK, parsedSetlist);
            var setlistDto = setlistService.SetlistRequest("testId");
            
            var spotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK);
            await spotifyService.SpotifyRequest(setlistDto.Result);

            var spotSetService = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.OK, parsedSetlist);

            var ex = Assert.ThrowsAsync<SpotifyNotFoundException>(() => spotSetService.GetSetlist("testId"));
            Assert.Equal("There was an error fetching track details for the requested setlist!", ex.Result.Message);
        }
    }
}