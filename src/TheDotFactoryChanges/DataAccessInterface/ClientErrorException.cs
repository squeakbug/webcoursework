using System;

namespace DataAccessInterface
{
    public class ClientErrorException : ApplicationException
    {
        public ClientErrorException()
        { }
        public ClientErrorException(string message)
            : base(message) { }

        public ClientErrorException(string message, Exception inner)
            : base(message, inner) { }
    }
}
