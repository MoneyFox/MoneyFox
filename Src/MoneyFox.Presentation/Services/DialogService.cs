using System;
using System.Threading.Tasks;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Interfaces;
using XF.Material.Forms.UI.Dialogs;

namespace MoneyFox.Presentation.Services
{
    public class DialogService : IDialogService 
    {
        private IMaterialModalPage dialog;

        public async Task ShowMessage(string title, string message) {
            await MaterialDialog.Instance.AlertAsync(message: message,
                                                     title: title,
                                                     acknowledgementText: Strings.OkLabel);
        }

        public async Task<bool> ShowConfirmMessage(string title, string message, string positiveButtonText = null, string negativeButtonText = null) {
            var answer = await MaterialDialog.Instance.ConfirmAsync(message: message,
                                                       title: title,
                                                       confirmingText: positiveButtonText ?? Strings.YesLabel,
                                                       dismissiveText: negativeButtonText ?? Strings.NoLabel);

            return answer ?? false;
        }

        public async Task ShowLoadingDialog(string message = null) {
            if (dialog != null) {
                await HideLoadingDialog();
            }
            dialog = await MaterialDialog.Instance.LoadingDialogAsync(message: message ?? Strings.LoadingLabel);
        }

        public async Task HideLoadingDialog() {
            if (dialog != null) {
                await dialog.DismissAsync();
                dialog = null;
            }
        }
    }
}
