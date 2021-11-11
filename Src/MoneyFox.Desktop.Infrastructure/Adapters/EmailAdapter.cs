using MoneyFox.Application.Common.Adapters;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyFox.Desktop.Infrastructure.Adapters
{
    public class EmailAdapter : IEmailAdapter
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public async Task SendEmailAsync(string subject, string body, List<string> recipients) => throw new NotImplementedException();

        public async Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths) => throw new NotImplementedException();
    }
}
