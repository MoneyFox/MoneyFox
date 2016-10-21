using System.Threading.Tasks;

namespace MoneyFox.Foundation.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        ///     Shows a basic toast notification with a title and a line of text.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Message to display.</param>
        /// <returns></returns>
        Task SendBasicNotification(string title, string message);
    }
}