using System.Collections.Generic;
using System.Net;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SetlistServiceTest
    {
        [Fact]
        public async void GetSetlistReturnsASetlistModelWhenCalledWithSetlistId()
        {
            var newSetlist = new Setlist
            {
                id = "setlistId",
                eventDate = "01-07-2019",
                artist = new Artist { name = "Artist" },
                venue = new Venue { name = "Venue" },
                sets = new Sets { set = new List<Set>() }
            };
            
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            
            var result = await successSetlistService.GetSetlist(newSetlist.id);
            
            Assert.IsType<Setlist>(result);
            Assert.Equal(newSetlist.id, result.id);
            Assert.Equal(newSetlist.eventDate, result.eventDate);
            Assert.Equal(newSetlist.artist.name, result.artist.name);
            Assert.Equal(newSetlist.venue.name, result.venue.name);
            Assert.Equal(newSetlist.sets.set, result.sets.set);
        }
        
        [Fact]
        public async void GetSetlistReturnsASetlistModelWhenCalledWithSetlistIdWhichHasMissingData()
        {
            var newSetlist = new Setlist
            {
                id = "setlistId",
                eventDate = "01-07-2019",
                artist = null,
                venue = null,
                sets = new Sets { set = new List<Set>() }
            };
            
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            
            var result = await successSetlistService.GetSetlist(newSetlist.id);
            
            Assert.IsType<Setlist>(result);
            Assert.Equal(newSetlist.id, result.id);
            Assert.Equal(newSetlist.eventDate, result.eventDate);
            Assert.Null(result.artist);
            Assert.Null(result.venue);
            Assert.Equal(newSetlist.sets.set, result.sets.set);
        }

        [Fact]
        public async void GetSetlistReturnsNullWhenCalledWithSetlistIdWhichResultsInError()
        {
            var errorSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.NotFound);
            
            var result = await errorSetlistService.GetSetlist("invalidId");

            Assert.Null(result);
        }
    }
}