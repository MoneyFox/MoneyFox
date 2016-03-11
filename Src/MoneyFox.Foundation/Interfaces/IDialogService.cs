using System;
using System.Threading.Tasks;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IDialogService
    {
        /// <summary>
        ///     Shows a dialog with title and message. Contains only an OK button.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Text to display.</param>
        Task ShowMessage(string title, string message);

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts and who takes actions.
        /// </summary>
        /// <param name="title">Title for the dialog.</param>
        /// <param name="message">Message for the dialog.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        /// <param name="positivAction">Action who shall be executed on the positive button click.</param>
        /// <param name="negativAction">Action who shall be executed on the negative button click.</param>
        Task ShowConfirmMessage(string title, string message, Action positivAction, string positiveButtonText = null,
            string negativeButtonText = null, Action negativAction = null);

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts. Returns the answer.
        /// </summary>
        /// <param name="title">Title for the dialog.</param>
        /// <param name="message">Message for the dialog.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        Task<bool> ShowConfirmMessage(string title, string message, string positiveButtonText = null,
            string negativeButtonText = null);
    }
}