using System.Collections.Generic;
using System.Net;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SpotSetServiceTest
    {
        [Fact]
        public async void GetSetlistReturnsASpotSetDtoWhenCalledWithSetlistIdThatReturnsValidData()
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

         
            var successSetlistService = TestSetup.CreateSpotSetServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var result = await successSetlistService.GetSetlist(newSetlist.Id);
    
            Assert.IsType<SpotSetDto>(result);
            Assert.Equal(newSetlist.Id, result.Id);
            Assert.Equal("07-01-2019", result.EventDate);
            Assert.Equal(newSetlist.Artist.Name, result.Artist);
            Assert.Equal(newSetlist.Venue.Name, result.Venue);
        }
    }
}