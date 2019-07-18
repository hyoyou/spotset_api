using System.Collections.Generic;
using System.Net;
using SpotSet.Api.Models;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SpotifyServiceTest
    {
        [Fact]
        public async void SpotifyRequestReturnsASpotifyModelWhenCalledWithValidSetlistModel()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Tracks = new List<Song>()
            };

            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            var result = await mockSpotifyService.SpotifyRequest(newSetlist);

            Assert.IsType<SpotifyTracksModel>(result);
        }

        [Fact]
        public async void SpotifyRequestReturnsSpotifyTracksModelWithCorrectNumberOfTracksWhenCalledWithASetlistModel()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Tracks = new List<Song>
                {
                    new Song { Name = "Song Title" }, 
                    new Song { Name = "Another Song Title" }
                }
            };
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            var result = await mockSpotifyService.SpotifyRequest(newSetlist);
            
            Assert.Equal(2, result.SpotifyTracks.Count);
        }
        
        [Fact]
        public async void SpotifyRequestReturnsEmptySpotifyTracksModelWhenCalledWithAnInvalidSetlistModel()
        {
            var newSetlist = new SetlistDto
            {
                Id = null,
                EventDate = null,
                Artist = new Artist { Name = null },
                Venue = new Venue { Name = null },
                Tracks = new List<Song>()
            };

            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            var result = await mockSpotifyService.SpotifyRequest(newSetlist);
            
            Assert.Equal(0, result.SpotifyTracks.Count);
        }
    }
}