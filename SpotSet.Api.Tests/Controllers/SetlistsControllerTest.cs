using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Sets = new Sets
                {
                    Set = new List<Set>
                    {
                        new Set
                        {
                            Song = new List<Song>
                            {
                                new Song { Name = "Song Title" }, 
                                new Song { Name = "Another Song Title" }
                            }
                        }
                    }
                }
            };
            
            var service = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist("setlistId");
            
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResult_WhenCallingGetSetlist_ThenItReturnsAStatus404()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Sets = new Sets
                {
                    Set = new List<Set>
                    {
                        new Set
                        {
                            Song = new List<Song>
                            {
                                new Song { Name = "Song Title" }, 
                                new Song { Name = "Another Song Title" }
                            }
                        }
                    }
                }
            };
            
            var service = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.NotFound, newSetlist);
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist("invalidId");
            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}