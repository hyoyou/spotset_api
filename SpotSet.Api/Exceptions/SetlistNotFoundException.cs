using System;
using System.Net;

namespace SpotSet.Api.Exceptions
{
    public class SetlistNotFoundException : Exception
    {
        public SetlistNotFoundException()
        {
        }

        public SetlistNotFoundException(string message)
            : base(message)
        {
        }

        public SetlistNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}