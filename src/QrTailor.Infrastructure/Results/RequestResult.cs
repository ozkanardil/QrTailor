using System;
using System.Collections.Generic;
using System.Text;

namespace QrTailor.Infrastructure.Results
{
    public class RequestResult : IRequestResult
    {
        public RequestResult(bool success, string message) : this(success)
        {
            Message = message;
        }

        public RequestResult(bool success)
        {
            Success = success;
        }
        public bool Success { get; }
        public string Message { get; }
    }
}
