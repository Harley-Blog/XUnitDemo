using System.Threading.Tasks;

namespace XUnitDemo.Infrastucture.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string from, string subject, string body);
    }
}
