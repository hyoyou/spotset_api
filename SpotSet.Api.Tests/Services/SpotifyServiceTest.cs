using System.Net;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Models;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SpotifyServiceTest
    {
        [Fact]
        public async void SpotifyRequestReturnsASpotifyModelWhenCalledWithSetlistModel()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);

            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);
            var setlistDto = mockSetlistFmService.SetlistRequest("testId");
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            var result = await mockSpotifyService.SpotifyRequest(setlistDto.Result);

            Assert.IsType<SpotifyTracksModel>(result);
        }

        [Fact]
        public async void SpotifyRequestReturnsSpotifyTracksModelWithCorrectNumberOfTracksWhenCalledWithASetlistModel()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle1\"}, {\"name\": \"songTitle2\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);
            
            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);
            var setlistDto = mockSetlistFmService.SetlistRequest("testId");
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            var result = await mockSpotifyService.SpotifyRequest(setlistDto.Result);
            
            Assert.Equal(2, result.SpotifyTracks.Count);
        }
        
        [Fact]
        public async void SpotifyRequestReturnsEmptySpotifyTracksModelWhenCalledWithAnInvalidSetlistModel()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": []}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);
            
            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);
            var setlistDto = mockSetlistFmService.SetlistRequest("testId");
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            var result = await mockSpotifyService.SpotifyRequest(setlistDto.Result);
            
            Assert.Equal(0, result.SpotifyTracks.Count);
        }
    }
}