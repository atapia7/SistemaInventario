using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Utilidades
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret { get; set; }
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
            SendGridSecret = _config.GetValue<string>("Sendgrid:SecretKey");
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //var client = new SendGridClient(SendGridSecret);
            //var from = new EmailAddress("michaeltapialmidon07@gmail.com");
            //var to=new EmailAddress(email);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            //return client.SendEmailAsync(msg);
            throw new NotImplementedException();
        }
    }
}
