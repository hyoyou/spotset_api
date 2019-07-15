using System.Collections.Generic;

namespace SpotSet.Api.Models
{
    public class SpotifyTracksModel
    {
        private ICollection<SpotifyTracks> _spotifyTracks;

        public SpotifyTracksModel()
        {
            _spotifyTracks = new List<SpotifyTracks>();
        }

        public ICollection<SpotifyTracks> SpotifyTracks
        {
            get => _spotifyTracks;
            set => _spotifyTracks = value;
        }

        public void Add(SpotifyTracks track) {
            _spotifyTracks.Add(track);
        }
    }
}