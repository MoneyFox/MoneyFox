using System.Threading.Tasks;

namespace MoneyFox.Application.Common.Interfaces
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows a loading Dialog.
        /// </summary>
        /// <param name="message">Message to display.</param>
        Task ShowLoadingDialogAsync(string? message = null);

        /// <summary>
        /// Hides the previously opened Loading Dialog.
        /// </summary>
        Task HideLoadingDialogAsync();
    }
}
