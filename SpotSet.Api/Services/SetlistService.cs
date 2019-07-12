using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotSet.Api.Constants;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public class SetlistService : ISetlistService
    {
        private static IHttpClientFactory _httpFactory;

        public SetlistService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<SpotSetDto> GetSetlist(string setlistId)
        {
            var setlistModel = await SetlistRequest(setlistId);
            if (setlistModel == null) return null;
            // TODO: raise specific error
            var spotifyModel = await SpotifyRequest(setlistModel);
            var tracksDto = MapSongToTrackUri(setlistModel.Sets.Set, spotifyModel.SpotifyTracks);
            var setlistDto = new SpotSetDto
            {
                Id = setlistModel.Id,
                EventDate = FormatEventDate(setlistModel.EventDate),
                Artist = setlistModel.Artist.Name,
                Venue = setlistModel.Venue.Name,
                Tracks = tracksDto
            };
            return setlistDto;
        }

        public async Task<SetlistDto> SetlistRequest(string setlistId)
        {
            var uri = $"setlist/{setlistId}";
            
            var httpClient = _httpFactory.CreateClient(HttpConstants.SetlistClient);
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await DeserializeSetlist(response);
            }
            //TODO: return exception
            return null;
        }
        
        private static async Task<SetlistDto> DeserializeSetlist(HttpResponseMessage response)
        {
            var setlist = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SetlistDto>(setlist);
        }
        
        public async Task<SpotifyTracksDto> SpotifyRequest(SetlistDto setlistmodel)
        {
            var spotifyHttpClient = _httpFactory.CreateClient(HttpConstants.SpotifyClient);
            var artist = setlistmodel?.Artist?.Name;
            var spotifyTracks = new SpotifyTracksDto();
            
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

                return spotifyTracks;
            }

            return null;
        }
        
        private static async Task<SpotifyTracks> DeserializeSpotifyTracks(HttpResponseMessage response)
        {
            var spotifyTracks = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SpotifyTracks>(spotifyTracks);
        }

        private List<TracksDto> MapSongToTrackUri(ICollection<Set> sets, ICollection<SpotifyTracks> spotifyModel)
        {
            var mappedTracks = new List<TracksDto>();

            foreach (var set in sets)
            {
                foreach (var song in set.Song)
                {
                    var tracksDto = new TracksDto
                    {
                        Name = song.Name,
                        TrackUri = MatchTrackUri(song.Name, spotifyModel)
                    };
                    
                    mappedTracks.Add(tracksDto);   
                }
            }

            return mappedTracks;
        }

        private string MatchTrackUri(string name, ICollection<SpotifyTracks> spotifyModel)
        {
            foreach (var track in spotifyModel)
            {
                var items = track.Tracks?.Items;
                if (items == null || !items.Exists(item => item.Name.ToLower().Contains(name.ToLower()))) continue;
                {
                    var trackMatch = items.First(item => item.Name.ToLower().Contains(name.ToLower()));
                    return trackMatch?.Uri;
                }
            }

            return null;
        }
        
        private string FormatEventDate(string setlistModelEventDate)
        {
            return DateTime.ParseExact(setlistModelEventDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                .ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
        }
    }
}