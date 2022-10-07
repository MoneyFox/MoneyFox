namespace MoneyFox.Win.Infrastructure.Adapters;

using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using JetBrains.Annotations;

[UsedImplicitly]
public class EmailAdapter : IEmailAdapter
{
    public async Task SendEmailAsync(string subject, string body, List<string> recipients)
    {
        await SendEmailAsync(subject: subject, body: body, recipients: recipients, filePaths: new());
    }

    public async Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths)
    {
        await Task.CompletedTask;
    }
}
