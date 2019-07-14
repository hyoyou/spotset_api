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
        
        public async Task<SpotifyTracksModel> SpotifyRequest(SetlistDto setlistmodel)
        {
            var spotifyHttpClient = _httpClientFactory.CreateClient(HttpConstants.SpotifyClient);
            var artist = setlistmodel?.Artist?.Name;
            var spotifyTracks = new SpotifyTracksModel();
            
            foreach (var set in setlistmodel?.Sets?.Set)
            {
                foreach (var song in set.Song)
                {
                    var spotifyResponse = await spotifyHttpClient.GetAsync(
                        $"search?query=artist%3A{artist}+track%3A{song.Name}&type=track&offset=0&limit=1");
                    if (spotifyResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var spotifyTrack = DeserializeSpotifyTracks(spotifyResponse);
                        spotifyTracks.Add(spotifyTrack.Result);
                    }
                }
            }
            
            return spotifyTracks;
        }
        
        private static async Task<SpotifyTracks> DeserializeSpotifyTracks(HttpResponseMessage response)
        {
            var spotifyTracks = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SpotifyTracks>(spotifyTracks);
        }
    }
}