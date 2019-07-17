using System.Net;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Constants;
using SpotSet.Api.Exceptions;
using SpotSet.Api.Models;
using SpotSet.Api.Services;
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
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle1\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);

            var testSpotifyTracks =
                "{\"SpotifyTracks\":[{\"Tracks\":{\"Items\":[{\"Name\":\"songTitle\",\"Uri\":\"spotify:track:uri1\"}]}}]}";
            JObject parsedSpotifyTracks = JObject.Parse(testSpotifyTracks);
            
            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);
            var setlistDto = mockSetlistFmService.SetlistRequest("testId");
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSpotifyTracks);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            await mockSpotifyService.SpotifyRequest(setlistDto.Result);

            var mockSpotSetService = new SpotSetService(mockSetlistFmService, mockSpotifyService);
            var result = await mockSpotSetService.GetSetlist("testId");
    
            Assert.IsType<SpotSetDto>(result);
            Assert.Equal("testId", result.Id);
            Assert.Equal("07-30-2019", result.EventDate);
            Assert.Equal("artistName", result.Artist);
            Assert.Equal("venueName", result.Venue);
        }

        [Fact]
        public async void GetSetlistReturnsSetlistExceptionWhenSetlistFmServiceResultsInError()
        {
            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);
            await mockSetlistFmService.SetlistRequest("testId");
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);

            var mockSpotSetService = new SpotSetService(mockSetlistFmService, mockSpotifyService);

            var ex = Assert.ThrowsAsync<SetlistNotFoundException>(() => mockSpotSetService.GetSetlist("testId"));
            var expected = ErrorConstants.SetlistError + "testId" + ErrorConstants.SetlistErrorTryAgain;
            
            Assert.Equal(expected, ex.Result.Message);
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
            
            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);
            var setlistDto = mockSetlistFmService.SetlistRequest("testId");
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            await mockSpotifyService.SpotifyRequest(setlistDto.Result);

            var mockSpotSetService = new SpotSetService(mockSetlistFmService, mockSpotifyService);

            var ex = Assert.ThrowsAsync<SpotifyNotFoundException>(() => mockSpotSetService.GetSetlist("testId"));
            Assert.Equal(ErrorConstants.SpotifyError, ex.Result.Message);
        }
    }
}