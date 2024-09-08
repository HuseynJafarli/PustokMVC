using Microsoft.Extensions.Configuration;
using Pustok.Business.ExternalServices.Interfaces;
using System.Net.Mail;
using System.Net;

namespace Pustok.Business.ExternalServices.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;


        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMailAsync(string to, string subject, string name, string text)
        {
            string mail = _configuration.GetSection("EmailService:Mail").Value;
            string password = _configuration.GetSection("EmailService:Password").Value;
            string bodyOfMail = GetBody(name, text);

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            await client.SendMailAsync(new MailMessage(mail, to, subject, bodyOfMail) { IsBodyHtml = true });
        }


        private string GetBody(string name, string text)
        {

            string body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            color: #333;
            margin: 20px;
        }}
        .content {{
            padding: 20px;
            background-color: #ffffff;
            border: 1px solid #ddd;
        }}
    </style>
</head>
<body>
    <div class='content'>
        <h2>Hello {name},</h2>
        <p>{text}</p>
    </div>
</body>
</html>
";
            return body;
        }
    }
}

