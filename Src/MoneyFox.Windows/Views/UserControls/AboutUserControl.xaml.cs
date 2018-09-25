using System;
using Windows.UI.Xaml;
using Microsoft.Services.Store.Engagement;

namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class AboutUserControl
    {
        public AboutUserControl()
        {
            InitializeComponent();

            if (StoreServicesFeedbackLauncher.IsSupported())
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
