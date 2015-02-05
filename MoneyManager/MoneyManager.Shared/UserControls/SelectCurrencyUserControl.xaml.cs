#region

using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Views;

#endregion

namespace MoneyManager.UserControls
{
    public sealed partial class SelectCurrencyUserControl
    {
        public SelectCurrencyUserControl()
        {
            InitializeComponent();
        }

        private async void LoadCountries(object sender, RoutedEventArgs e)
        {
            if (LicenseHelper.IsFeaturepackLicensed)
            {
                await ServiceLocator.Current.GetInstance<SelectCurrencyViewModel>().LoadCountries();
            }
            else
            {
                var dialog = new MessageDialog(Translation.GetTranslation("FeatureNotLicensedMessage"),
                    Translation.GetTranslation("FeatureNotLicensedTitle"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("RedirectLabel"), GoToPurchase));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("BackLabel"), NavigateBack));
                await dialog.ShowAsync();
            }
        }

        private void GoToPurchase(IUICommand command)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(LicenseView));
        }

        private void NavigateBack(IUICommand command)
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}