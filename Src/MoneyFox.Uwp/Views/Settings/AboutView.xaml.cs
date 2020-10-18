using CommonServiceLocator;
using Microsoft.Services.Store.Engagement;
using MoneyFox.Ui.Shared.ViewModels.About;
using System;
using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class AboutView
    {
        private readonly AboutViewModel ViewModel = ServiceLocator.Current.GetInstance<AboutViewModel>();

        public AboutView()
        {
            InitializeComponent();

            if(StoreServicesFeedbackLauncher.IsSupported())
            {
                FeedbackButton.Visibility = Visibility.Visible;
            }
        }

        private async void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            var launcher = StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
    }
}
