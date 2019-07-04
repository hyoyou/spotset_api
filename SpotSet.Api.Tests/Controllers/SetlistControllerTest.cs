using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotSet.Api.Controllers;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Mocks;
using Xunit;

namespace SpotSet.Api.Tests.Controllers
{
    public class SetlistControllerTest
    {
        [Fact]
        public async Task GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsAStatus200()
        {
            MockHttpClientFactory mockHttpClientFactory = new MockHttpClientFactory();
            var mockSetlistService = new MockSetlistService(mockHttpClientFactory);
            var controller = new SetlistController(mockSetlistService);
            var result = await controller.GetSetlist("setlistId");

            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async Task GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsASetlist()
        {
            MockHttpClientFactory mockHttpClientFactory = new MockHttpClientFactory();
            var mockSetlistService = new MockSetlistService(mockHttpClientFactory);
            var result = await mockSetlistService.GetSetlist("53d6f3ad");
         
            Assert.IsType<Setlist>(result);
        }
    }
}