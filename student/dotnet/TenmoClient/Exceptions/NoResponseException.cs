using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Exceptions
{
    public class NoResponseException : Exception
    {
        public NoResponseException() : base() { }
        public NoResponseException(string message) : base(message) { }
        public NoResponseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
