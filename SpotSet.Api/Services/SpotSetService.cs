using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public class SpotSetService : ISpotSetService
    {
        private static IHttpClientFactory _httpClientFactory;
        private static ISetlistFmService _setlistFmService;
        private static ISpotifyService _spotifyService;

        public SpotSetService(IHttpClientFactory httpClientFactory, ISetlistFmService setlistFmService, ISpotifyService spotifyService)
        {
            _httpClientFactory = httpClientFactory;
            _setlistFmService = setlistFmService;
            _spotifyService = spotifyService;
        }

        public async Task<SpotSetDto> GetSetlist(string setlistId)
        {
            var setlistModel = await _setlistFmService.SetlistRequest(setlistId);
            if (setlistModel == null) return null;

            var spotifyModel = await _spotifyService.SpotifyRequest(setlistModel);
            var tracksDto = MapSongToTrackUri(setlistModel.Sets.Set, spotifyModel.SpotifyTracks);
            var setlistDto = new SpotSetDto
            {
                Id = setlistModel.Id,
                EventDate = setlistModel.EventDate,
                Artist = setlistModel.Artist.Name,
                Venue = setlistModel.Venue.Name,
                Tracks = tracksDto
            };
            return setlistDto;
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
    }
}