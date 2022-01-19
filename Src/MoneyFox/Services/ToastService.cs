using Acr.UserDialogs;
using JetBrains.Annotations;
using MoneyFox.Core._Pending_.Common.Interfaces;
using System.Threading.Tasks;

namespace MoneyFox.Services
{
    [UsedImplicitly]
    public class ToastService : IToastService
    {
        private readonly IUserDialogs userDialogs;

        public ToastService(IUserDialogs userDialogs)
        {
            this.userDialogs = userDialogs;
        }

        public Task ShowToastAsync(string message, string title = "")
        {
            userDialogs.Toast(new ToastConfig(message));
            return Task.CompletedTask;
        }
    }
}