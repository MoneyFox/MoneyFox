using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyFox.Core.Interfaces
{
    public interface IEmailAdapter
    {
        Task SendEmailAsync(string subject, string body, List<string> recipients);

        Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths);
    }
}