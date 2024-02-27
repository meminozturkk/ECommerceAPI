using ECommerceAPI.Application.Abstraction.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{

    namespace ETicaretAPI.Infrastructure.Services
    {

        public class MailService : IMailService
        {
            readonly IConfiguration _configuration;

            public MailService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
            {
                await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
            }

            public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
            {
                MailMessage mail = new();
                mail.IsBodyHtml = isBodyHtml;
                foreach (var to in tos)
                    mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.From = new(_configuration["Mail:Username"], "Muhammet Emin Öztürk", System.Text.Encoding.UTF8);

                SmtpClient smtp = new();
                smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
                smtp.Port = 587;
                
                smtp.Host = _configuration["Mail:Host"];
                await smtp.SendMailAsync(mail);
            }

            public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
            {
                StringBuilder mail = new();
                mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"");
                mail.Append(_configuration["AngularClientUrl"]);
                mail.Append("/update-password/");
                mail.Append(userId);
                mail.Append("/");
                mail.Append(resetToken);
                mail.Append("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br> OZTURK |E-Ticaret");


                await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
            }
        }
    }
}
