namespace SpotSet.Api.Constants
{
    public class HttpConstants
    {
        public const string SetlistClient = "GetSetlistClient";
        public const string SpotifyClient = "GetSpotifyClient";
        public const string SetlistUri = "https://api.setlist.fm/rest/1.0/";
        public const string SpotifyAuthUri = "https://accounts.spotify.com/api/token";
        public const string SpotifyUserAuthUri = "https://accounts.spotify.com/authorize";
        public const string SpotifyRedirectUri = "https://localhost:5001/callback";
        public const string SpotifyUri = "https://api.spotify.com/v1/";
        public const string SpotifyUserScopes = "playlist-modify-private playlist-modify-public";
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