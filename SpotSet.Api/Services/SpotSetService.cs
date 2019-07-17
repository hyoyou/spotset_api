using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotSet.Api.Constants;
using SpotSet.Api.Exceptions;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public class SpotSetService : ISpotSetService
    {
        private static ISetlistFmService _setlistFmService;
        private static ISpotifyService _spotifyService;

        public SpotSetService(ISetlistFmService setlistFmService, ISpotifyService spotifyService)
        {
            _setlistFmService = setlistFmService;
            _spotifyService = spotifyService;
        }

        public async Task<SpotSetDto> GetSetlist(string setlistId)
        {
            try
            {
                var setlistModel = await _setlistFmService.SetlistRequest(setlistId);
                if (setlistModel == null)
                {
                    var errorMessage = ErrorConstants.SetlistError + setlistId + ErrorConstants.SetlistErrorTryAgain;
                    throw new SetlistNotFoundException(errorMessage);
                }

                var spotifyModel = await _spotifyService.SpotifyRequest(setlistModel);
                if (spotifyModel.SpotifyTracks.Count == 0)
                {
                    throw new SpotifyNotFoundException(ErrorConstants.SpotifyError);
                }

                var tracksDto = MapSongToTrackUri(setlistModel.Tracks, spotifyModel.SpotifyTracks);
                return CreateSpotSetDto(setlistModel, tracksDto);
            }
            catch (SetlistNotFoundException ex)
            {
                throw ex;
            }
            catch (SpotifyNotFoundException ex)
            {
                throw ex;
            }
        }

        private List<TracksDto> MapSongToTrackUri(List<Song> tracks, ICollection<SpotifyTracks> spotifyModel)
        {
            var mappedTracks = new List<TracksDto>();

            foreach (var track in tracks)
            {
                var tracksDto = new TracksDto
                {
                    Name = track.Name,
                    TrackUri = MatchTrackUri(track.Name, spotifyModel)
                };
                    
                mappedTracks.Add(tracksDto);
            }

            return mappedTracks;
        }

        private string MatchTrackUri(string name, ICollection<SpotifyTracks> spotifyModel)
        {
            foreach (var track in spotifyModel)
            {
                var items = track.Tracks?.Items;
                if (isMatch(name, items)) continue;
                {
                    var trackMatch = items.First(item => item.Name.ToLower().Contains(name.ToLower()));
                    return trackMatch?.Uri;
                }
            }

            return null;
        }

        private bool isMatch(string name, List<Item> items)
        {
            return items == null || !items.Exists(item => item.Name.ToLower().Contains(name.ToLower()));
        }

        private static SpotSetDto CreateSpotSetDto(SetlistDto setlistModel, List<TracksDto> tracksDto)
        {
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
    }
}