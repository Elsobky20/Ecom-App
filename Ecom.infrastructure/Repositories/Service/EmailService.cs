using Ecom.core.DTO;
using Ecom.core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories.Service
{
    class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
          this._configuration = configuration;   
        }
        public async Task SendEmail(EmailDTO emailDTO)
        {

            //SMPT Simple Mail Transfare Protocol 
            MimeMessage Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("My Ecom", _configuration["EmailSetting:From"]));
            Message.Subject = emailDTO.Subject;
            Message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            Message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    //connect
                    await smtp.ConnectAsync(
                           _configuration["EmailSetting:Smtp"],
                           int.Parse(_configuration["EmailSetting:Port"]),
                           true);
                    //login
                    await smtp.AuthenticateAsync(
                        _configuration["EmailSetting:ahmedelsobky630@gmail.com"],
                        _configuration["EmailSetting:xkqnsvjmacxjrinf"]
                        );
                    //send message
                    await smtp.SendAsync(Message);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    //disconnect
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }
                
        }
    }
}
