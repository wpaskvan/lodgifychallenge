using System;
using System.Collections.Generic;
using System.Text;

namespace SuperApp.Core.Exceptions
{
    public class ExternalApiException : Exception
    {
        public int StatusCode { get; private set; }
        public string ResponseMessage { get; private set; }

        public ExternalApiException(int statusCode, string responseMessage, string message)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseMessage = responseMessage;
        }
    }
}
