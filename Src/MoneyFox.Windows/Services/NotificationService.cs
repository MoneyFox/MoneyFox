using System.Threading.Tasks;
using Windows.UI.Notifications;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Windows.Services {
    public class NotificationService : INotificationService {
        public Task SendBasicNotification(string title, string body) {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(title));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(body));
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastXml));
            return Task.CompletedTask;
        }
    }
}