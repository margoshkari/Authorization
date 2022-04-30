using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        SqlOperations sqlOperations = new SqlOperations();
        string msg = string.Empty;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get(string login, string email, string password)
        {
            if (sqlOperations.isLoginExist(login))
            {
                return "Login exist!";
            }
            else if (sqlOperations.isEmailExist(email))
            {
                return "Email exist!";
            }
            try
            {
                SendPassword(email, password);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return sqlOperations.Register(login, email, password);
        }

        void SendPassword(string email, string password)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "tanksproga@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("Hello", email));
            emailMessage.Subject = "тема";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = password
            };

            using (var client = new SmtpClient())
            {
                client.ConnectAsync("smtp.gmail.com", 465, false);
                client.AuthenticateAsync("tanksproga@gmail.com", "tanks123");
                client.SendAsync(emailMessage);

                client.DisconnectAsync(true);
            }
        }
    }
}
