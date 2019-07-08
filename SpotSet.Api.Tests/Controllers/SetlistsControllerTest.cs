using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SpotSet.Api.Controllers;
using SpotSet.Api.Models;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Controllers
{
    public class SetlistsControllerTest
    {
        private SetlistsController CreateController(SetlistService setlistService)
        {
            return new SetlistsController(setlistService);
        }

        [Fact]
        public async void GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsAStatus200()
        {
            var setlist = new Setlist
            {
                id = "setlistId",
                sets = new Sets { set = new List<Set>() }
            };
            var service = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, setlist);
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist("setlistId");
            
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResult_WhenCallingGetSetlist_ThenItReturnsAStatus404()
        {
            var service = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.NotFound);
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist("invalidId");
            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}