using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SpotSet.Api.Controllers;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Mocks;
using Xunit;

namespace SpotSet.Api.Tests.Controllers
{
    public class SetlistsControllerTest
    {
        private SetlistsController _controller;
        private Setlist _expectedSetlist;
        public SetlistsControllerTest()
        {
            var artistData = new Artist { name = "Artist" };
            var venueData = new Venue { name = "Venue" };
            var setsData = new Sets { set = new List<Set>() };

            _expectedSetlist = new Setlist
            {
                id = "setlistId",
                eventDate = "01-07-2019",
                artist = artistData,
                venue = venueData,
                sets = setsData
            };
            
            var serializedSetlist = JsonConvert.SerializeObject(_expectedSetlist);
            
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(serializedSetlist, Encoding.UTF8, "application/json")
                })
                .Verifiable();
            
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            MockSetlistService mockSetlistService = new MockSetlistService(mockHttpClient);
            
            _controller = new SetlistsController(mockSetlistService);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsAStatus200()
        {
            var result = await _controller.GetSetlist(_expectedSetlist.id);
            
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsASetlistModel()
        {
            var result = await _controller.GetSetlist(_expectedSetlist.id);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Setlist>(okResult.Value);
            Assert.IsType<Setlist>(model);
            Assert.Equal(_expectedSetlist.id, model.id);
            Assert.Equal(_expectedSetlist.eventDate, model.eventDate);
            Assert.Equal(_expectedSetlist.artist.name, model.artist.name);
            Assert.Equal(_expectedSetlist.venue.name, model.venue.name);
            Assert.Equal(_expectedSetlist.sets.set, model.sets.set);
        }
    }
}