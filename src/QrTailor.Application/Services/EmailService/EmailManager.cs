using QrTailor.Domain.Entities;
using QrTailor.Infrastructure.Errors.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QrTailor.Infrastructure.Results;

namespace QrTailor.Application.Services.EmailService
{
    public class EmailManager : IEmailService
    {

        public IRequestResult MailGonderOld(Email email)
        {
            //MAIL GONDERME KODLARI
            SmtpClient sc = new SmtpClient();
            sc.Port = ApplicationSettings.MailPort;
            sc.Host = ApplicationSettings.MailHost; // LOCALDE posta gonderme hatasi verdi.
            sc.EnableSsl = ApplicationSettings.MailEnableSsl;

            sc.Credentials = new NetworkCredential(ApplicationSettings.MailFrom, ApplicationSettings.MailPassword);

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(ApplicationSettings.MailFrom, ApplicationSettings.AppName);

            mail.To.Add(email.ToMail);

            //mail.CC.Add("ozkanardil@gmail.com");

            mail.Subject = email.Subject;
            mail.IsBodyHtml = true;

            //mail.Body = changeWebAppAddress(ApplicationSettings.AppUrl2, email.Body);

            //mail.Attachments.Add(new Attachment(@"C:\Rapor.xlsx"));
            //mail.Attachments.Add(new Attachment(@"C:\Sonuc.pptx"));

            try
            {
                sc.Send(mail);
                return new SuccessRequestResult();
            }
            catch (Exception ex)
            {
                return new ErrorRequestResult();
            }
        }

        public IRequestResult MailGonder(Email email)
        {
            try
            {
                var message = new MailMessage {
                 From = new MailAddress(ApplicationSettings.MailFrom),
                 To = { email.ToMail },
                 Subject = email.Subject,
                 Body = email.Body,
                 DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                 IsBodyHtml = true
                };
                using(SmtpClient sc = new SmtpClient(ApplicationSettings.MailHost)) { 
                    sc.Credentials = new NetworkCredential(ApplicationSettings.MailFrom, ApplicationSettings.MailPassword);
                    sc.Port = ApplicationSettings.MailPort;
                    sc.EnableSsl = ApplicationSettings.MailEnableSsl;
                    sc.Send(message);
                }
                return new SuccessRequestResult();
            }
            catch (Exception ex)
            {

                return new ErrorRequestResult();
            }
        }

        private string changeWebAppAddress(string webAppAddress, string rawText)
        {
            string MailBody = rawText.Replace("xxxxxx", webAppAddress);
            return MailBody;
        }

    }
}
