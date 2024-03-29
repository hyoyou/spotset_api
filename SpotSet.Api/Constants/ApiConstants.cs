namespace SpotSet.Api.Constants
{
    public class ApiConstants
    {
        public const string SetlistClient = "GetSetlistClient";
        public const string SpotifyClient = "GetSpotifyClient";

        public const string SetlistApiKey = "SetlistApiKey";
        public const string SpotifyApiKey = "SpotifyApiKey";
        public const string SpotifyApiSecret = "SpotifyApiSecret";
        
        public const string SetlistUri = "https://api.setlist.fm/rest/1.0/";
        public const string SetlistSearchUri = "setlist/";
        public const string SpotifyAuthUri = "https://accounts.spotify.com/api/token";
        public const string SpotifyUri = "https://api.spotify.com/v1/";
        public const string SpotifyQueryArtist = "search?query=artist%3A";
        public const string SpotifyQueryTrack = "+track%3A";
        public const string SpotifyQueryOptions = "&type=track&offset=0&limit=1";
        public const string ClientUrlLocal = "http://localhost:3000";
        
        public const string XApiKey = "x-api-key";
        public const string Basic = "Basic";
        public const string Bearer = "Bearer";
        public const string ContentType = "Accept";
        public const string AppJson = "application/json";
        public const string GrantType = "grant_type";
        public const string ClientCred = "client_credentials";
    }
}