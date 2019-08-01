using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LGThingApi
{
    public static class LGExceptions
    {
        /// <summary>
        /// Trying to POST when user is not logged in or credentials are invalid
        /// </summary>
        internal class NotLoggedInException : Exception
        {
            public NotLoggedInException():base()
            {
            }

            public NotLoggedInException(string message) : base(message)
            {
            }

            public NotLoggedInException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NotLoggedInException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        /// <summary>
        /// Not connected to device
        /// </summary>
        internal class NotConnectedException : Exception
        {
            public NotConnectedException() : base()
            {
            }

            public NotConnectedException(string message) : base(message)
            {
            }

            public NotConnectedException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NotConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        /// <summary>
        /// Other API related low level exceptions
        /// </summary>
        internal class ApiException : Exception
        {
            public ApiException() : base()
            {
            }

            public ApiException(string message) : base(message)
            {
            }

            public ApiException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        } //TO-DO remove
    }
}
