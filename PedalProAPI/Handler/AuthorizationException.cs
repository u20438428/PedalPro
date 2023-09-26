using System;

namespace PedalProAPI.Handler
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string message) : base(message)
        {
        }
    }
}
