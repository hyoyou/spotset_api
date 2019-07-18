using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
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
                Tracks = new List<Song>
                {
                    new Song { Name = "Song Title" }
                }
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
        
        [Fact]
        public async void SpotifyRequestReturnsASpotifyTracksModelWithTrackInformationWhenCalledWithValidSetlistModel()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Tracks = new List<Song>
                {
                    new Song { Name = "Song Title" }
                }
            };

            var testSpotifyTracks =
                "{\"tracks\": {\"items\": [{\"name\": \"Song Title\", \"uri\": \"spotify:track:uri1\"}]}}";
            JObject parsedSpotifyTracks = JObject.Parse(testSpotifyTracks);
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSpotifyTracks);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            var result = await mockSpotifyService.SpotifyRequest(newSetlist);

            Assert.IsType<SpotifyTracksModel>(result);
            Assert.Equal("Song Title", result.SpotifyTracks.First().Tracks.Items[0].Name);
            Assert.Equal("spotify:track:uri1", result.SpotifyTracks.First().Tracks.Items[0].Uri);
        }
        
        [Fact]
        public async void SpotifyRequestReturnsASpotifyTracksModelWithMissingTrackInformationWhenCalledWithValidSetlistModel()
        {
            var newSetlist = new SetlistDto
            {
                Id = "setlistId",
                EventDate = "01-07-2019",
                Artist = new Artist { Name = "Artist" },
                Venue = new Venue { Name = "Venue" },
                Tracks = new List<Song>
                {
                    new Song { Name = "Song Title" }
                }
            };

            var testSpotifyTracks =
                "{\"tracks\": {\"items\": [{\"name\": \"Song Title\"}]}}";
            JObject parsedSpotifyTracks = JObject.Parse(testSpotifyTracks);
            
            var mockHttpClientFactoryforSpotifyService = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSpotifyTracks);
            var mockSpotifyService = new SpotifyService(mockHttpClientFactoryforSpotifyService);
            var result = await mockSpotifyService.SpotifyRequest(newSetlist);

            Assert.IsType<SpotifyTracksModel>(result);
            Assert.Equal("Song Title", result.SpotifyTracks.First().Tracks.Items[0].Name);
            Assert.Null(result.SpotifyTracks.First().Tracks.Items[0].Uri);
        }
    }
}