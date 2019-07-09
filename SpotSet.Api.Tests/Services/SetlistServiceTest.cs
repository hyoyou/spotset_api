using System.Collections.Generic;
using System.Net;
using Moq;
using SpotSet.Api.Models;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SetlistServiceTest
    {
        [Fact]
        public async void SetlistRequestReturnsASetlistModelWhenCalledWithSetlistId()
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
            
            var result = await successSetlistService.SetlistRequest(newSetlist.id);
    
            Assert.IsType<Setlist>(result);
            Assert.Equal(newSetlist.id, result.id);
            Assert.Equal(newSetlist.eventDate, result.eventDate);
            Assert.Equal(newSetlist.artist.name, result.artist.name);
            Assert.Equal(newSetlist.venue.name, result.venue.name);
        }
        
        [Fact]
        public async void SetlistRequestReturnsASetlistModelWhenCalledWithSetlistIdWhichHasMissingData()
        {
            var newSetlistWithMissingData = new Setlist
            {
                id = "setlistId",
                eventDate = "01-07-2019",
                artist = null,
                venue = null,
                sets = new Sets { set = new List<Set>() }
            };
            
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlistWithMissingData);
            
            var result = await successSetlistService.SetlistRequest(newSetlistWithMissingData.id);
    
            Assert.IsType<Setlist>(result);
            Assert.Equal(newSetlistWithMissingData.id, result.id);
            Assert.Equal(newSetlistWithMissingData.eventDate, result.eventDate);
            Assert.Null(result.artist);
            Assert.Null(result.venue);
            Assert.Equal(newSetlistWithMissingData.sets.set, result.sets.set);
        }

        [Fact]
        public async void SetlistRequestReturnsNullWhenCalledWithSetlistIdWhichResultsInError()
        {
            var errorSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.NotFound);
            
            var result = await errorSetlistService.SetlistRequest("invalidId");

            Assert.Null(result);
        }

        [Fact]
        public async void SpotifyRequestReturnsASpotifyModelWhenCalledWithSetlistModel()
        {
            var song1 = new Song
            {
                name = "Song Title"
            };
            
            var song2 = new Song
            {
                name = "Another Song Title"
            };
            
            var setWithTwoSongs = new Set
            {
                song = new List<Song>
                {
                    song1, song2
                }
            };

            var newSets = new Sets
            {
                set = new List<Set>
                {
                    setWithTwoSongs
                }
            };

            var newSetlist = new Setlist
            {
                id = "setlistId",
                eventDate = "01-07-2019",
                artist = new Artist { name = "Artist" },
                venue = new Venue { name = "Venue" },
                sets = newSets
            };
            
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var result = await successSetlistService.SpotifyRequest(newSetlist);

            Assert.IsType<SpotifyTracksDto>(result);
        }

        [Fact]
        public async void SpotifyRequestReturnsNullWhenCalledWithAnInvalidSetlistModel()
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
            var result = await successSetlistService.SpotifyRequest(newSetlist);
            
            Assert.Null(result);
        }

        [Fact]
        public async void GetSetlistReturnsASetlistDtoWhenCalledWithSetlistIdThatReturnsValidData()
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

         
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var result = await successSetlistService.GetSetlist(newSetlist.id);
    
            Assert.IsType<SetlistDto>(result);
            Assert.Equal(newSetlist.id, result.id);
            Assert.Equal("07-01-2019", result.eventDate);
            Assert.Equal(newSetlist.artist.name, result.artist);
            Assert.Equal(newSetlist.venue.name, result.venue);
        }
    }
}