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
        private SetlistsController CreateController(SetlistService setlistService)
        {
            return new SetlistsController(setlistService);
        }

        [Fact]
        public async void GivenSetlistServiceReturnsASuccessResult_WhenCallingGetSetlist_ThenItReturnsAStatus200()
        {
            var newSetlist = new Setlist
            {
                id = "setlistId",
                eventDate = "01-07-2019",
                artist = new Artist { name = "Artist" },
                venue = new Venue { name = "Venue" },
                sets = new Sets
                {
                    set = new List<Set>
                    {
                        new Set
                        {
                            song = new List<Song>
                            {
                                new Song { name = "Song Title" }, 
                                new Song { name = "Another Song Title" }
                            }
                        }
                    }
                }
            };
            
            var service = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist("setlistId");
            
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void GivenSetlistServiceReturnsAnErrorResult_WhenCallingGetSetlist_ThenItReturnsAStatus404()
        {
            var newSetlist = new Setlist
            {
                id = "setlistId",
                eventDate = "01-07-2019",
                artist = new Artist { name = "Artist" },
                venue = new Venue { name = "Venue" },
                sets = new Sets
                {
                    set = new List<Set>
                    {
                        new Set
                        {
                            song = new List<Song>
                            {
                                new Song { name = "Song Title" }, 
                                new Song { name = "Another Song Title" }
                            }
                        }
                    }
                }
            };
            
            var service = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.NotFound, newSetlist);
            var controller = CreateController(service);
            
            var result = await controller.GetSetlist("invalidId");
            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}