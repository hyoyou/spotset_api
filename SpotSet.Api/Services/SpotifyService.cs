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
            var spotifyHttpClient = _httpClientFactory.CreateClient(ApiConstants.SpotifyClient);
            var artist = setlistModel?.Artist?.Name;
            var spotifyTracks = new SpotifyTracksModel();
            
            foreach (var track in setlistModel.Tracks)
            {
                var url = ApiConstants.SpotifyQueryArtist + artist + ApiConstants.SpotifyQueryTrack + track.Name +
                          ApiConstants.SpotifyQueryOptions;
                var spotifyResponse = await spotifyHttpClient.GetAsync(url);
                if (spotifyResponse.StatusCode == HttpStatusCode.OK)
                {
                    var spotifyTrack = DeserializeSpotifyTracks(spotifyResponse);
                    spotifyTracks.Add(spotifyTrack.Result);
                }
            }
            
            return spotifyTracks ;
        }
        
        private static async Task<SpotifyTracks> DeserializeSpotifyTracks(HttpResponseMessage response)
        {
            var spotifyTracks = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SpotifyTracks>(spotifyTracks);
        }
    }
}