using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterface
{
    public class NotAuthorizedException : ClientErrorException
    {
        public NotAuthorizedException()
        { }
        public NotAuthorizedException(string message)
            : base(message) { }

        public NotAuthorizedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
