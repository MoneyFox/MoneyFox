using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

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
        {
            ContentFrame.Navigate(typeof(SettingsView));
            SettingsView.SelectedItem = SettingsView.MenuItems[0];
        }

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if(args.InvokedItemContainer.Tag.ToString() == "settings")
            {
                ContentFrame.Navigate(typeof(SettingsView),
                                      null,
                                      new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
            else
            {
                ContentFrame.Navigate(typeof(AboutView),
                                      null,
                                      new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
            }
        }
    }
}
