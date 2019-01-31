using System;
using Windows.UI.Xaml;
using Microsoft.Services.Store.Engagement;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    ///     About View
    /// </summary>
    public sealed partial class AboutView
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public AboutView()
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