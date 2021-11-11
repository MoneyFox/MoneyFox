using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyFox.Application.Common.Adapters
{
    public interface IEmailAdapter
    {
        Task SendEmailAsync(string subject, string body, List<string> recipients);

        Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths);
    }
}