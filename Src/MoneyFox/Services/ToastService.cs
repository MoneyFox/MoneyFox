using Plugin.Toast;
using Plugin.Toasts;
using System.Threading.Tasks;

namespace MoneyFox.Services
{
    public class ToastService : IToastService
    {
        private readonly IToastNotificator toastNotificator;

        public ToastService(IToastNotificator toastNotificator)
        {
            this.toastNotificator = toastNotificator;
        }

        public async Task ShowToastAsync(string message, string title = "")
        {
            var options = new NotificationOptions()
            {
                Title = title,
                Description = message
            };

            await toastNotificator.Notify(options);
        }
    }
}
