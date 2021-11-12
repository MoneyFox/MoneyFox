using Microsoft.Services.Store.Engagement;
using MoneyFox.Uwp.ViewModels.About;
using System;
using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.AboutVm;

            if(StoreServicesFeedbackLauncher.IsSupported())
            {
                FeedbackButton.Visibility = Visibility.Visible;
            }
        }

        private AboutViewModel ViewModel => (AboutViewModel)DataContext;

        private async void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            var launcher = StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
    }
}