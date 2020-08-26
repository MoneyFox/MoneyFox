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

        public async Task ShowToastAsync(string title, string text)
        {
            var options = new NotificationOptions()
            {
                Title = title,
                Description = text
            };

            await toastNotificator.Notify(options);
        }
    }
}
