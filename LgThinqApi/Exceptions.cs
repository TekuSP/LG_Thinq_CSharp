using System;
using System.Collections.Generic;
using System.Text;

namespace LGThingApi
{
    public static class LGExceptions
    {
        public class NotLoggedInException : Exception { };
        public class NotConnectedException : Exception { };
        public class ApiException : Exception
        {
            public ApiException(string errorMessage) : base(errorMessage)
            {

            }
        }
    }
}
