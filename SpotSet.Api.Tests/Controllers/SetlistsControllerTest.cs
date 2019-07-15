using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Controllers;
using SpotSet.Api.Models;
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
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);
            
            var successSpotSetService = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.OK, parsedSetlist);
            var controller = CreateController(successSpotSetService);
            
            var result = await controller.GetSetlist("testId");
            
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResult_WhenCallingGetSetlist_ThenItReturnsAStatus404()
        {
            var service = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.NotFound);
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist("invalidId");
            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}