namespace SpotSet.Api.Constants
{
    public class ErrorConstants
    {
        public const string SetlistError = "No results found for setlist with an ID of ";
        public const string SetlistErrorTryAgain = ". Please try your search again.";
        public const string SpotifyError = "There was an error fetching track details for the requested setlist. Please make sure it is not an empty setlist.";
        public const string SpotifyAuthError = "There was an error authenticating the app.";
    }
}