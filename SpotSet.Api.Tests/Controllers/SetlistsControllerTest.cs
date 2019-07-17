using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Controllers;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Controllers
{
    public class SetlistsControllerTest
    {
        private SetlistsController CreateController(SpotSetService spotSetService)
        {
            return new SetlistsController(spotSetService);
        }

        [Fact]
        public async void GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsAStatus200()
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

            var controller = CreateController(mockSpotSetService);
            
            var result = await controller.GetSetlist("testId");
            
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResultFromSetlistService_WhenCallingGetSetlist_ThenItReturnsAStatus404()
        {
            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);

            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);

            var mockSpotSetService = new SpotSetService(mockSetlistFmService, mockSpotifyService);

            var controller = CreateController(mockSpotSetService);

            var result = await controller.GetSetlist("invalidId");

            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResultFromSetlistService_WhenCallingGetSetlist_ThenItReturnsTheCorrectError()
        {
            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);

            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);

            var mockSpotSetService = new SpotSetService(mockSetlistFmService, mockSpotifyService);

            var controller = CreateController(mockSpotSetService);
            
            var result = await controller.GetSetlist("invalidId");
            
            var error = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No results found for setlist with an ID of invalidId. Please try your search again.", error.Value.ToString());
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResultFromSpotifyService_WhenCallingGetSetlist_ThenItReturnsAStatus404()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle1\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);

            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);

            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            
            var mockSpotSetService = new SpotSetService(mockSetlistFmService, mockSpotifyService);

            var controller = CreateController(mockSpotSetService);

            var result = await controller.GetSetlist("invalidId");

            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResultFromSpotifyService_WhenCallingGetSetlist_ThenItReturnsTheCorrectError()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle1\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);

            var mockHttpClientFactoryForSetlistFmService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactoryForSetlistFmService);

            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            
            var mockSpotSetService = new SpotSetService(mockSetlistFmService, mockSpotifyService);

            var controller = CreateController(mockSpotSetService);

            var result = await controller.GetSetlist("invalidId");
            
            var error = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("There was an error fetching track details for the requested setlist!", error.Value.ToString());
        }
    }
}