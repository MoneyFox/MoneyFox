using Microsoft.Services.Store.Engagement;
using System;
using Windows.UI.Xaml;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();

            if(StoreServicesFeedbackLauncher.IsSupported())
                FeedbackButton.Visibility = Visibility.Visible;
        }

        private async void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            StoreServicesFeedbackLauncher launcher = StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
    }
}
