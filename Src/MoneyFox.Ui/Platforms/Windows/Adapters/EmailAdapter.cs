namespace MoneyFox.Ui.Platforms.Windows.Adapters;

using JetBrains.Annotations;
using MoneyFox.Core.Interfaces;

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
