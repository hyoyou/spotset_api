using Microsoft.AspNetCore.Mvc;
using SpotSet.Api.Controllers;
using SpotSet.Api.Models;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Mocks;
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
            var id = "setlistId";
            var eventDate = "01-07-2019";
            var artistData = new Artist { name = null };
            var venueData = new Venue { name = null };
            var setsData = new Sets { set = null };
            
            var newSetlist = MockSetup.CreateSetlist(id, eventDate, artistData, venueData, setsData);
            var service = MockSetup.CreateSuccessSetlistServiceWithMocks(newSetlist);
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist(newSetlist.id);
            
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResult_WhenCallingGetSetlist_ThenItReturnsAStatus404()
        {
            var service = MockSetup.CreateErrorSetlistServiceWithMocks();
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist("invalidId");
            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}