using System.Threading.Tasks;
using XUnitDemo.Infrastucture.Interface;

namespace XUnitDemo.Infrastucture
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string from, string subject, string body)
        {
            //发送邮件逻辑
            await Task.CompletedTask;
        }
    }
}
