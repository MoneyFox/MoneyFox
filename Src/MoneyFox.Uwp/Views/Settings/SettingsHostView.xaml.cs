using Microsoft.UI.Xaml.Controls;
using System;

#nullable enable
namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class SettingsHostView
    {
        public override bool ShowHeader => false;

        public SettingsHostView()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => ContentFrame.Navigate(typeof(SettingsView));

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            Type page = args.InvokedItemContainer.Tag.ToString() == "settings"
                ? typeof(SettingsView)
                : typeof(AboutView);

            ContentFrame.Navigate(page);
        }
    }
}
