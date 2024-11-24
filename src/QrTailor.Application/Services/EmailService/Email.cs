using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Application.Services.EmailService
{
    public class Email
    {
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
