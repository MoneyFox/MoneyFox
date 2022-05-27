namespace MoneyFox.Services
{

    using System;
    using System.Threading.Tasks;
    using CommunityToolkit.Maui.Views;
    using Core.Common.Interfaces;
    using Core.Resources;
    using Views.Dialogs;

    public class DialogService : IDialogService
    {
        private LoadingDialog? loadingDialog;

        /// <inheritdoc />
        public async Task ShowLoadingDialogAsync(string? message = null)
        {
            if (loadingDialog != null)
            {
                await HideLoadingDialogAsync();
            }

            loadingDialog = new LoadingDialog();
            await Application.Current.MainPage.ShowPopupAsync(loadingDialog);
        }

        /// <inheritdoc />
        public async Task HideLoadingDialogAsync()
        {
            if (loadingDialog == null)
            {
                return;
            }

            try
            {
                loadingDialog.Close();
                loadingDialog = null;
            }
            catch (IndexOutOfRangeException)
            {
                // catch and swallow out of range exceptions when dismissing dialogs.
            }
            await Task.CompletedTask;
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            if (loadingDialog != null)
            {
                // Only 1 dialog can be open at a time. Close the Loading dialog sow the message can be displayed.
                await HideLoadingDialogAsync();
            }

            var messageDialog = new MessageDialog(title: title, message: message);
            await Application.Current.MainPage.ShowPopupAsync(messageDialog);
        }

        public async Task<bool> ShowConfirmMessageAsync(string title, string message, string? positiveButtonText = null, string? negativeButtonText = null)
        {
            if (loadingDialog != null)
            {
                // Only 1 dialog can be open at a time. Close the Loading dialog sow the message can be displayed.
                await HideLoadingDialogAsync();
            }

            var confirmDialog = new ConfirmMessageDialog(
                title: title,
                message: message,
                positiveText: positiveButtonText ?? Strings.YesLabel,
                negativeText: negativeButtonText ?? Strings.NoLabel);

            return (bool)await Application.Current.MainPage.ShowPopupAsync(confirmDialog);
        }
    }

}
