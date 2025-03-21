using EmployeePortal.Core.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Logging;

namespace EmployeePortal.Services.Services
{
    public class EMailService : IEmailService
    {
        private readonly ILogger<EMailService> _logger;
        private const string _emailSender = "rkfunworld69@gmail.com";
        private const string _senderName = "Ranjeet Karmakar";
        private const string _formPassword = "gcot mexr dzny ctxl";

        public EMailService(ILogger<EMailService> logger)
        {
            _logger = logger;
        }

        public bool SendEmailUsingGmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(_emailSender, _senderName);
            var toAddress = new MailAddress(toEmail);
            const string fromPassword = _formPassword;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
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
