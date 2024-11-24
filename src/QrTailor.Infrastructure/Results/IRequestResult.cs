using System;
using System.Collections.Generic;
using System.Text;

namespace QrTailor.Infrastructure.Results
{
    public interface IRequestResult
    {
        bool Success { get; }
        string Message { get; }
    }
}
