using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms;
using XF.Material.Forms.Resources;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace MoneyFox.Presentation.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowMessageAsync(string title, string message)
        {
            if(loadingDialog != null)
                await HideLoadingDialogAsync();

            await MaterialDialog.Instance.AlertAsync(message, title, Strings.OkLabel, GetAlertDialogConfiguration());
        }

        public async Task<bool> ShowConfirmMessageAsync(string title,
                                                        string message,
                                                        string positiveButtonText = null,
                                                        string negativeButtonText = null)
        {
            if(loadingDialog != null)
                await HideLoadingDialogAsync();

            bool? wasConfirmed = await MaterialDialog.Instance
                                                     .ConfirmAsync(message,
                                                                   title,
                                                                   Strings.YesLabel,
                                                                   Strings.NoLabel,
                                                                   GetAlertDialogConfiguration());

            return wasConfirmed ?? false;
        }

        private IMaterialModalPage loadingDialog;

        /// <inheritdoc/>
        public async Task ShowLoadingDialogAsync(string message = null)
        {
            if(loadingDialog != null)
                await HideLoadingDialogAsync();

            loadingDialog = await MaterialDialog.Instance
                                                .LoadingDialogAsync(message ?? Strings.LoadingLabel, GetLoadingDialogConfiguration());
        }

        /// <inheritdoc/>
        public async Task HideLoadingDialogAsync()
        {
            await loadingDialog.DismissAsync();
        }

        private static MaterialAlertDialogConfiguration GetAlertDialogConfiguration()
        {
            return new MaterialAlertDialogConfiguration
                   {
                       BackgroundColor = (Color) Xamarin.Forms.Application.Current.Resources["DialogBackgroundColor"],
                       TitleTextColor = Material.GetResource<Color>(MaterialConstants.Color.ON_PRIMARY),
                       MessageTextColor = Material.GetResource<Color>(MaterialConstants.Color.ON_PRIMARY).MultiplyAlpha(0.8),
                       TintColor = Material.GetResource<Color>(MaterialConstants.Color.ON_PRIMARY),
                       CornerRadius = 8,
                       ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
                       ButtonAllCaps = false
                   };
        }

        private static MaterialLoadingDialogConfiguration GetLoadingDialogConfiguration()
        {
            return new MaterialLoadingDialogConfiguration
                   {
                       BackgroundColor = (Color) Xamarin.Forms.Application.Current.Resources["DialogBackgroundColor"],
                       MessageTextColor = Material.GetResource<Color>(MaterialConstants.Color.ON_PRIMARY).MultiplyAlpha(0.8),
                       TintColor = Material.GetResource<Color>(MaterialConstants.Color.ON_PRIMARY),
                       CornerRadius = 8,
                       ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32)
                   };
        }
    }
}
