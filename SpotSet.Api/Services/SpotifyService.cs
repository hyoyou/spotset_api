using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotSet.Api.Constants;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public class SpotifyService : ISpotifyService
    {
        private IHttpClientFactory _httpClientFactory;
        
        public SpotifyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<SpotifyTracksModel> SpotifyRequest(SetlistDto setlistModel)
        {
            var spotifyHttpClient = CreateHttpClient();
            
            return await CreateSpotifyTracksModel(setlistModel, spotifyHttpClient);
        }

        private HttpClient CreateHttpClient()
        {
            var spotifyHttpClient = _httpClientFactory.CreateClient(ApiConstants.SpotifyClient);
            
            return spotifyHttpClient;
        }

        private static async Task<SpotifyTracksModel> CreateSpotifyTracksModel(SetlistDto setlistModel, HttpClient spotifyHttpClient)
        {
            var artist = setlistModel?.Artist?.Name;
            var spotifyTracks = new SpotifyTracksModel();

            foreach (var track in setlistModel.Tracks)
            {
                var spotifyResponse = await SendRequest(spotifyHttpClient, artist, track);
                if (spotifyResponse.StatusCode == HttpStatusCode.OK)
                {
                    var spotifyTrack = DeserializeSpotifyTracks(spotifyResponse);
                    spotifyTracks.Add(spotifyTrack.Result);
                }
            }

            return spotifyTracks;
        }

        private static async Task<HttpResponseMessage> SendRequest(HttpClient spotifyHttpClient, string artist, Song track)
        {
            var url = ApiConstants.SpotifyQueryArtist + artist + ApiConstants.SpotifyQueryTrack + track.Name +
                      ApiConstants.SpotifyQueryOptions;
            var spotifyResponse = await spotifyHttpClient.GetAsync(url);
            
            return spotifyResponse;
        }

        private static async Task<SpotifyTracks> DeserializeSpotifyTracks(HttpResponseMessage response)
        {
            var spotifyTracks = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<SpotifyTracks>(spotifyTracks);
        }
    }
}