namespace Pustok.Business.ExternalServices.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(string to, string subject, string name, string text);
    }
}
