using EmployeePortal.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace EmployeePortal.Services.Services
{
    public class EMailService : IEmailService
    {
        private readonly ILogger<EMailService> _logger;
        private const string _emailSender = "mail.engage360@gmail.com";
        private const string _senderName = "Engage360-Support";
        private const string _emailSecret = "zfcw scmy rtks tqcf";

        public EMailService(ILogger<EMailService> logger)
        {
            _logger = logger;
        }

        public bool SendEmailUsingGmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(_emailSender, _senderName);
            var toAddress = new MailAddress(toEmail);
            const string emailSecret = _emailSecret;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, emailSecret)
            };
            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while sending an email.");
                return false;
            }
        }
    }
}
