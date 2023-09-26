using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string to, string subject, string body)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

        using (var client = new SmtpClient())
        {
            var credentials = new NetworkCredential
            {
                UserName = emailSettings["SmtpUsername"],
                Password = emailSettings["SmtpPassword"]
            };

            client.Credentials = credentials;
            client.Host = emailSettings["SmtpServer"];
            client.Port = int.Parse(emailSettings["SmtpPort"]);
            client.EnableSsl = bool.Parse(emailSettings["UseSsl"]);

            using (var emailMessage = new MailMessage())
            {
                emailMessage.From = new MailAddress(emailSettings["SmtpUsername"]);
                emailMessage.To.Add(to);
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                client.Send(emailMessage);
            }
        }
    }
}