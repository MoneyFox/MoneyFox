using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.iOS
{
    public class DialogService : IDialogService
    {
        private IProgressDialog currentProgressDialog;

        /// <inheritdoc />
        public Task ShowMessage(string title, string message)
        {
            // We have to hide the loading dialog first, otherwise it get's stuck
            HideLoadingDialog();

            UserDialogs.Instance.Alert(message, title);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task ShowConfirmMessage(string title, string message, Action positivAction, string positiveButtonText = null, string negativeButtonText = null, Action negativAction = null)
        {
            HideLoadingDialog();

            var confirmConfig = new ConfirmConfig
            {
                Title = title,
                Message = message
            };

            // Change the text of confirm buttons only if they are not null
            if (!string.IsNullOrEmpty(positiveButtonText))
            {
                confirmConfig.OkText = positiveButtonText;
            }

            if (!string.IsNullOrEmpty(negativeButtonText))
            {
                confirmConfig.CancelText = negativeButtonText;
            }

            // true if user touches positive button
            var isPositiveAction = await UserDialogs.Instance.ConfirmAsync(confirmConfig);

            if (isPositiveAction)
            {
                positivAction();
            } else
            {
                negativAction?.Invoke();
            }
        }

        /// <inheritdoc />
        public async Task<bool> ShowConfirmMessage(string title, string message, string positiveButtonText = null, string negativeButtonText = null)
        {
            // We have to hide the loading dialog first, otherwise it get's stuck
            HideLoadingDialog();

            var confirmConfig = new ConfirmConfig
            {
                Title = title,
                Message = message
            };


            if (!string.IsNullOrEmpty(positiveButtonText))
            {
                confirmConfig.OkText = positiveButtonText;
            }

            if (!string.IsNullOrEmpty(negativeButtonText))
            {
                confirmConfig.CancelText = negativeButtonText;
            }

            var action = await UserDialogs.Instance.ConfirmAsync(confirmConfig);

            return action;
        }

        /// <inheritdoc />
        public void ShowLoadingDialog(string message = null)
        {
            currentProgressDialog = UserDialogs.Instance.Loading(message);
        }

        /// <inheritdoc />
        public void HideLoadingDialog()
        {
            currentProgressDialog?.Dispose();
        }
    }
}
