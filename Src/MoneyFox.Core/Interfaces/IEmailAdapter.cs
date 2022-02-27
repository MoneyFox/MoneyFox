namespace MoneyFox.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailAdapter
    {
        Task SendEmailAsync(string subject, string body, List<string> recipients);

        Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths);
    }
}