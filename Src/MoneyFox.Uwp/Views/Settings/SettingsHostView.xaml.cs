using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

#nullable enable
namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class SettingsHostView
    {
        public SettingsHostView()
        {
            InitializeComponent();
        }

        public override bool ShowHeader => false;

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(SettingsView));
            SettingsView.SelectedItem = SettingsView.MenuItems[0];
        }

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if(args.InvokedItemContainer.Tag.ToString() == "settings")
            {
                ContentFrame.Navigate(
                    typeof(SettingsView),
                    null,
                    new SlideNavigationTransitionInfo {Effect = SlideNavigationTransitionEffect.FromRight});
            }
            else
            {
                ContentFrame.Navigate(
                    typeof(AboutView),
                    null,
                    new SlideNavigationTransitionInfo {Effect = SlideNavigationTransitionEffect.FromLeft});
            }
        }
    }
}