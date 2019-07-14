using System.Collections.Generic;
using System.Net;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SetlistFmServiceTest
    {
        [Fact]
        public async void SetlistRequestReturnsASetlistDtoWhenCalledWithSetlistId()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Sets = new Sets { Set = new List<Set>() }
            };
            
            var successSetlistFmService = TestSetup.CreateSetlistFmServiceWithMocks(HttpStatusCode.OK, newSetlist);
            
            var result = await successSetlistFmService.SetlistRequest(newSetlist.Id);
    
            Assert.IsType<SetlistDto>(result);
            Assert.Equal(newSetlist.Id, result.Id);
            Assert.Equal(newSetlist.EventDate, result.EventDate);
            Assert.Equal(newSetlist.Artist.Name, result.Artist.Name);
            Assert.Equal(newSetlist.Venue.Name, result.Venue.Name);
        }
        
        [Fact]
        public async void SetlistRequestReturnsASetlistModelWhenCalledWithSetlistIdWhichHasMissingData()
        {
            var newSetlistWithMissingData = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = null,
                Venue = null,
                Sets = new Sets { Set = new List<Set>() }
            };
            
            var successSetlistFmService = TestSetup.CreateSetlistFmServiceWithMocks(HttpStatusCode.OK, newSetlistWithMissingData);
            
            var result = await successSetlistFmService.SetlistRequest(newSetlistWithMissingData.Id);
    
            Assert.IsType<SetlistDto>(result);
            Assert.Equal(newSetlistWithMissingData.Id, result.Id);
            Assert.Equal(newSetlistWithMissingData.EventDate, result.EventDate);
            Assert.Null(result.Artist);
            Assert.Null(result.Venue);
            Assert.Equal(newSetlistWithMissingData.Sets.Set, result.Sets.Set);
        }

        [Fact]
        public async void SetlistRequestReturnsNullWhenCalledWithSetlistIdWhichResultsInError()
        {
            var errorSetlistFmService = TestSetup.CreateSetlistFmServiceWithMocks(HttpStatusCode.NotFound);
            
            var result = await errorSetlistFmService.SetlistRequest("invalidId");

            Assert.Null(result);
        }
    }
}