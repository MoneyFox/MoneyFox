using Microsoft.AppCenter.Crashes;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

#nullable enable
namespace MoneyFox.Uwp
{
    public class DialogService : IDialogService
    {
        private const int DELAY_MS = 5;
        private LoadingDialog? loadingDialog;

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts. Returns the answer.
        /// </summary>
        /// <param name="title">Title for the dialog.</param>
        /// <param name="message">Text for the dialog.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        public async Task<bool> ShowConfirmMessageAsync(string title,
            string message,
            string? positiveButtonText = null,
            string? negativeButtonText = null)
        {
            CloseAllOpenDialogs();
            var dialog = new ContentDialog {Title = title, Content = message};
            dialog.PrimaryButtonText = positiveButtonText ?? Strings.YesLabel;
            dialog.SecondaryButtonText = negativeButtonText ?? Strings.NoLabel;
            dialog.RequestedTheme = ThemeSelectorService.Theme;

            var result = await dialog.ShowAsync();

            return result == ContentDialogResult.Primary;
        }

        /// <summary>
        ///     Shows a dialog with title and message. Contains only an OK button.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Text to display.</param>
        public async Task ShowMessageAsync(string title, string message)
        {
            CloseAllOpenDialogs();

            var dialog = new ContentDialog {Title = title, Content = message};
            dialog.PrimaryButtonText = Strings.OkLabel;
            dialog.RequestedTheme = ThemeSelectorService.Theme;

            await dialog.ShowAsync();
        }

        /// <summary>
        ///     Shows a loading Dialog.
        /// </summary>
        public async Task ShowLoadingDialogAsync(string? message = null)
        {
            // Be sure no other dialog is open.
            CloseAllOpenDialogs();

            loadingDialog = new LoadingDialog {Text = message ?? Strings.LoadingLabel};
            loadingDialog.RequestedTheme = ThemeSelectorService.Theme;

            var coreWindow = CoreApplication.MainView;

            // Dispatcher needed to run on UI Thread
            var dispatcher = coreWindow.CoreWindow.Dispatcher;

            // RunAsync all of the UI info.
            await dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                async () =>
                {
                    try
                    {
                        await loadingDialog.ShowAsync();
                    }
                    catch(Exception ex)
                    {
                        Crashes.TrackError(
                            ex,
                            new Dictionary<string, string> {{"Message", "Loading Dialog couldn't be opened."}});
                    }
                });

            // we have to add a delay here for the UI to be able to redraw in certain conditions.
            await Task.Delay(DELAY_MS);
        }

        /// <summary>
        ///     Hides the previously opened Loading Dialog.
        /// </summary>
        public Task HideLoadingDialogAsync()
        {
            loadingDialog?.Hide();
            return Task.CompletedTask;
        }

        private static void CloseAllOpenDialogs()
        {
            IReadOnlyList<Popup>? openedpopups = VisualTreeHelper.GetOpenPopups(Window.Current);
            foreach(var popup in openedpopups)
            {
                if(popup.Child is ContentDialog dialog)
                {
                    dialog.Hide();
                }
            }
        }
    }
}