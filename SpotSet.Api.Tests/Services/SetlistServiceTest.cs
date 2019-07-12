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
        public async void SetlistRequestReturnsASetlistModelWhenCalledWithSetlistId()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Sets = new Sets { Set = new List<Set>() }
            };
            
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            
            var result = await successSetlistService.SetlistRequest(newSetlist.Id);
    
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
            
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlistWithMissingData);
            
            var result = await successSetlistService.SetlistRequest(newSetlistWithMissingData.Id);
    
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
            var errorSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.NotFound);
            
            var result = await errorSetlistService.SetlistRequest("invalidId");

            Assert.Null(result);
        }

        [Fact]
        public async void SpotifyRequestReturnsASpotifyModelWhenCalledWithSetlistModel()
        {
            var song1 = new Song
            {
                Name = "Song Title"
            };
            
            var song2 = new Song
            {
                Name = "Another Song Title"
            };
            
            var setWithTwoSongs = new Set
            {
                Song = new List<Song>
                {
                    song1, song2
                }
            };

            var newSets = new Sets
            {
                Set = new List<Set>
                {
                    setWithTwoSongs
                }
            };

            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Sets = newSets
            };
            
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var result = await successSetlistService.SpotifyRequest(newSetlist);

            Assert.IsType<SpotifyTracksModel>(result);
        }

        [Fact]
        public async void SpotifyRequestReturnsNullWhenCalledWithAnInvalidSetlistModel()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = null,
                Venue = null,
                Sets = new Sets { Set = new List<Set>() }
            };
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var result = await successSetlistService.SpotifyRequest(newSetlist);
            
            Assert.Null(result);
        }

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

         
            var successSetlistService = TestSetup.CreateSetlistServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var result = await successSetlistService.GetSetlist(newSetlist.Id);
    
            Assert.IsType<SpotSetDto>(result);
            Assert.Equal(newSetlist.Id, result.Id);
            Assert.Equal("07-01-2019", result.EventDate);
            Assert.Equal(newSetlist.Artist.Name, result.Artist);
            Assert.Equal(newSetlist.Venue.Name, result.Venue);
        }
    }
}