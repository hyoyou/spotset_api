using System;

namespace SpotSet.Api.Exceptions
{
    public class SpotifyNotFoundException : Exception
    {
        public SpotifyNotFoundException()
        {
        }

        public SpotifyNotFoundException(string message)
            : base(message)
        {
        }

        public SpotifyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}