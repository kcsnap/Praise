namespace Praise.Api.Logic.Events
{
    public interface IEmailManagerLogic
    {
        Task SendEmail(string toEmail, string subject, string bodyString);
    }
}