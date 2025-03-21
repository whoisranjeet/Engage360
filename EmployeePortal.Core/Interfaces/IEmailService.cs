namespace EmployeePortal.Core.Interfaces
{
    public interface IEmailService
    {
        bool SendEmailUsingGmail(string toEmail, string subject, string body);
    }
}
