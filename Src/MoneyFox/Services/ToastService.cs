using Acr.UserDialogs;
using System.Threading.Tasks;

namespace MoneyFox.Services
{
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
