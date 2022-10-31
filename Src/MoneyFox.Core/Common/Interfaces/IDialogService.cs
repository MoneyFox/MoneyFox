namespace MoneyFox.Core.Common.Interfaces;

using System.Threading.Tasks;

public interface IDialogService
{
    /// <summary>
    ///     Shows a dialog with title and message. Contains only an OK button.
    /// </summary>
    /// <param name="title">Title to display.</param>
    /// <param name="message">Text to display.</param>
    Task ShowMessageAsync(string title, string message);

    /// <summary>
    ///     Show a dialog with two buttons with customizable Texts. Returns the answer.
    /// </summary>
    /// <param name="title">Title for the dialog.</param>
    /// <param name="message">Message for the dialog.</param>
    /// <param name="positiveButtonText">Text for the yes button.</param>
    /// <param name="negativeButtonText">Text for the no button.</param>
    Task<bool> ShowConfirmMessageAsync(string title, string message, string? positiveButtonText = null, string? negativeButtonText = null);

    /// <summary>
    ///     Shows a loading Dialog.
    /// </summary>
    /// <param name="message">Message to display.</param>
    Task ShowLoadingDialogAsync(string? message = null);

    /// <summary>
    ///     Hides the previously opened Loading Dialog.
    /// </summary>
    Task HideLoadingDialogAsync();
}
