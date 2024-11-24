using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Application.Features.Auth.Models
{
    public class VerifyEmailCommandRequest
    {
        public int code { get; set; }
    }
}
