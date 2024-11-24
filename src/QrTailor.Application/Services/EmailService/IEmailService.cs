using QrTailor.Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Application.Services.EmailService
{
    public interface IEmailService
    {
        IRequestResult MailGonderOld(Email email);
        IRequestResult MailGonder(Email email);
    }
}
