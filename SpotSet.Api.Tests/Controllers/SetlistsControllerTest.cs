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
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Mocks;
using Xunit;

namespace SpotSet.Api.Tests.Controllers
{
    public class SetlistsControllerTest
    {
        private Setlist CreateSetlist(string id, string eventDate, Artist artistData, Venue venueData, Sets setsData)
        {
            return new Setlist
            {
                id = id,
                eventDate = eventDate,
                artist = artistData,
                venue = venueData,
                sets = setsData
            };
        }
        
        private string SerializeSetlist(Setlist newSetlist)
        {
            return JsonConvert.SerializeObject(newSetlist);
        }
        
        private static Mock<HttpMessageHandler> CreateMockHttpMessageHandler(string serializedSetlist)
        {
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
            return mockHttpMessageHandler;
        }
        
        private SetlistsController CreateController(Setlist newSetlist)
        {
            var serializedSetlist = SerializeSetlist(newSetlist);
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(serializedSetlist);
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory= new MockHttpClientFactory(mockHttpClient);
            var setlistService = new SetlistService(mockHttpClientFactory);
            
            return new SetlistsController(setlistService);
        }

        [Fact]
        public async void GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsAStatus200()
        {
            var id = "setlistId";
            var eventDate = "01-07-2019";
            var artistData = new Artist { name = "Artist" };
            var venueData = new Venue { name = "Venue" };
            var setsData = new Sets { set = new List<Set>() };
            
            var newSetlist = CreateSetlist(id, eventDate, artistData, venueData, setsData);
            var controller = CreateController(newSetlist);
            
            var result = await controller.GetSetlist(newSetlist.id);
            
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsASetlistModel()
        {
            var id = "setlistId";
            var eventDate = "01-07-2019";
            var artistData = new Artist { name = "Artist" };
            var venueData = new Venue { name = "Venue" };
            var setsData = new Sets { set = new List<Set>() };
            
            var newSetlist = CreateSetlist(id, eventDate, artistData, venueData, setsData);
            var controller = CreateController(newSetlist);
            
            var result = await controller.GetSetlist(newSetlist.id);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var setlistModel = Assert.IsAssignableFrom<Setlist>(okResult.Value);
            Assert.IsType<Setlist>(setlistModel);
            Assert.Equal(newSetlist.id, setlistModel.id);
            Assert.Equal(newSetlist.eventDate, setlistModel.eventDate);
            Assert.Equal(newSetlist.artist.name, setlistModel.artist.name);
            Assert.Equal(newSetlist.venue.name, setlistModel.venue.name);
            Assert.Equal(newSetlist.sets.set, setlistModel.sets.set);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAResultWithMissingData_WhenCallingGetSetlist_ThenItReturnsASetlistModel()
        {
            var id = "setlistId";
            var eventDate = "02-07-2019";
            var artistData = new Artist { name = "Artist" };
            var venueData = new Venue { name = "" };
            var setsData = new Sets { set = new List<Set>() };
            
            var newSetlist = CreateSetlist(id, eventDate, artistData, venueData, setsData);
            var controller = CreateController(newSetlist);
            
            var result = await controller.GetSetlist(newSetlist.id);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var setlistModel = Assert.IsAssignableFrom<Setlist>(okResult.Value);
            Assert.IsType<Setlist>(setlistModel);
            Assert.Equal(newSetlist.id, setlistModel.id);
            Assert.Equal(newSetlist.eventDate, setlistModel.eventDate);
            Assert.Equal(newSetlist.artist.name, setlistModel.artist.name);
            Assert.Equal(newSetlist.venue.name, setlistModel.venue.name);
            Assert.Equal(newSetlist.sets.set, setlistModel.sets.set);
        }
    }
}