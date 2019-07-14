using System.Collections.Generic;
using System.Net;
using SpotSet.Api.Models;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SpotifyServiceTest
    {
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
            
            var successSpotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var result = await successSpotifyService.SpotifyRequest(newSetlist);

            Assert.IsType<SpotifyTracksModel>(result);
        }

        [Fact]
        public async void SpotifyRequestReturnsEmptySpotifyTracksModelWhenCalledWithAnInvalidSetlistModel()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = null,
                Venue = null,
                Sets = new Sets { Set = new List<Set>() }
            };
            var successSpotifyService = TestSetup.CreateSpotifyServiceWithMocks(HttpStatusCode.OK, newSetlist);
            var result = await successSpotifyService.SpotifyRequest(newSetlist);
            
            Assert.Equal(0, result.SpotifyTracks.Count);
        }
    }
}