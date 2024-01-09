namespace MoneyFox.Ui.Common.Services;

using CommunityToolkit.Maui.Views;
using Core.Common.Interfaces;
using Resources.Strings;

public class DialogService : IDialogService
{
    private LoadingIndicatorPopup? loadingDialog;

    public async Task ShowLoadingDialogAsync(string? message = null)
    {
        if (loadingDialog != null)
        {
            await HideLoadingDialogAsync();
        }

        loadingDialog = new();
        Application.Current!.MainPage!.ShowPopup(loadingDialog);
    }

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
            await Task.CompletedTask;
        }
        catch (IndexOutOfRangeException)
        {
            // catch and swallow out of range exceptions when dismissing dialogs.
        }
    }

    public Task ShowMessageAsync(string title, string message)
    {
        return Application.Current!.MainPage!.DisplayAlert(title: title, message: message, cancel: Translations.OkLabel);
    }

    public async Task<bool> ShowConfirmMessageAsync(string title, string message, string? positiveButtonText = null, string? negativeButtonText = null)
    {
        await HideLoadingDialogAsync();

        return await Application.Current!.MainPage!.DisplayAlert(
            title: title,
            message: message,
            accept: positiveButtonText ?? Translations.YesLabel,
            cancel: negativeButtonText ?? Translations.NoLabel);
    }
}
