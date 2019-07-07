using System.Collections.Generic;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Mocks;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SetlistServiceTest
    {
        [Fact]
        public async void GetSetlistReturnsASetlistModelWhenCalledWithSetlistId()
        {
            var id = "setlistId";
            var eventDate = "01-07-2019";
            var artistData = new Artist { name = "Artist" };
            var venueData = new Venue { name = "Venue" };
            var setsData = new Sets { set = new List<Set>() };
            
            var newSetlist = TestSetup.CreateSetlist(id, eventDate, artistData, venueData, setsData);
            var successSetlistService = TestSetup.CreateSuccessSetlistServiceWithMocks(newSetlist);
            
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
            var id = "setlistId";
            var eventDate = "01-07-2019";
            var artistData = new Artist { name = null };
            var venueData = new Venue { name = null };
            var setsData = new Sets { set = null };
            
            var newSetlist = TestSetup.CreateSetlist(id, eventDate, artistData, venueData, setsData);
            var successSetlistService = TestSetup.CreateSuccessSetlistServiceWithMocks(newSetlist);
            
            var result = await successSetlistService.GetSetlist(newSetlist.id);
            
            Assert.IsType<Setlist>(result);
            Assert.Equal(newSetlist.id, result.id);
            Assert.Equal(newSetlist.eventDate, result.eventDate);
            Assert.Null(result.artist.name);
            Assert.Null(result.venue.name);
            Assert.Null(result.sets.set);
        }

        [Fact]
        public async void GetSetlistReturnsNullWhenCalledWithSetlistIdWhichResultsInError()
        {
            var errorSetlistService = TestSetup.CreateErrorSetlistServiceWithMocks();

            var result = await errorSetlistService.GetSetlist("invalidId");

            Assert.Null(result);
        }
        
        [Fact]
        public async void GetSetlistAddsTrackUrisToTheSongModelWhenCalledWithSetlistId()
        {
            var id = "setlistId";
            var eventDate = "01-07-2019";
            var artistData = new Artist { name = "Artist" };
            var venueData = new Venue { name = "Venue" };
            var setsData = new Sets { set = new List<Set>() };
            
            var newSetlist = TestSetup.CreateSetlist(id, eventDate, artistData, venueData, setsData);
            var successSetlistService = TestSetup.CreateSuccessSetlistServiceWithMocks(newSetlist);
            
            var result = await successSetlistService.GetSetlist(newSetlist.id);
            
            Assert.IsType<Setlist>(result);
            Assert.Equal(newSetlist.id, result.id);
            Assert.Equal(newSetlist.eventDate, result.eventDate);
            Assert.Equal(newSetlist.artist.name, result.artist.name);
            Assert.Equal(newSetlist.venue.name, result.venue.name);
            Assert.Equal(newSetlist.sets.set, result.sets.set);
        }
    }
}