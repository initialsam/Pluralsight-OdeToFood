using Common.Utility.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Infrastructure
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
    public class IdentityEmailSender : IEmailSender
    {
        public EmailSender Sender { get; }

        public IdentityEmailSender(IConfiguration configuration)
        {

            Sender = new EmailSender(new SMTPServerDTO
            {
                SMTP = "smtp.gmail.com",
                SMTPport = 587,
                SenderAccount = configuration["EmailStrings:EmailSenderAccount"],
                SenderMailAddress = configuration["EmailStrings:EmailSenderMailAddress"],
                SenderPassword = configuration["EmailStrings:EmailSenderPassword"],
                SslRequired = true,
                AuthRequired = true
            },
            new List<string>(),  //receivers
            new List<string>()); //bccReceivers
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Sender.ReceiverAddresses = new List<string> { email };
            await Sender.SendMailAsync(subject, htmlMessage);
        }
    }
}
