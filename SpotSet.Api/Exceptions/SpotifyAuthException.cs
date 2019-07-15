using System;

namespace SpotSet.Api.Exceptions
{
    public class SpotifyAuthException : Exception
    {
        public SpotifyAuthException()
        {
        }

        public SpotifyAuthException(string message)
            : base(message)
        {
        }

        public SpotifyAuthException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}