using MoneyFox.Shared.Interfaces;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MoneyManager.Windows.Services
{
    public class NotificationService : INotificationService
    {
        public Task SendBasicNotification(string title, string body)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(title));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(body));
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastXml));
            return Task.CompletedTask;
        }
    }
}
