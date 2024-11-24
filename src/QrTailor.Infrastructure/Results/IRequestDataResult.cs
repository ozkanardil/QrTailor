using System;
using System.Collections.Generic;
using System.Text;

namespace QrTailor.Infrastructure.Results
{
    public interface IRequestDataResult<out T> : IRequestResult
    {
        T Data { get; }
    }
}
