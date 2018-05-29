using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyFox.Business.ViewModels;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView
    {
        public MainView()
        {
            this.InitializeComponent();

            CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
        }

        public Frame MainFrame => ContentFrame;

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitle.Margin = new Thickness(CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset + 12, 8, 0, 0);
        }

        private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                await ((MainViewModel) ViewModel).ShowSettingsCommand.ExecuteAsync();
            } 
            else
            {
                switch (args.InvokedItem)
                {
                    case "Accounts":
                        await ((MainViewModel)ViewModel).ShowAccountListCommand.ExecuteAsync();
                        break;

                    case "Statistics":
                        await ((MainViewModel)ViewModel).ShowStatisticSelectorCommand.ExecuteAsync();
                        break;

                    case "Categories":
                        await ((MainViewModel)ViewModel).ShowCategoryListCommand.ExecuteAsync();
                        break;

                    case "Backup":
                        await ((MainViewModel)ViewModel).ShowBackupViewCommand.ExecuteAsync();
                        break;

                    case "Settings":
                        await ((MainViewModel)ViewModel).ShowSettingsCommand.ExecuteAsync();
                        break;

                    case "About":
                        await ((MainViewModel)ViewModel).ShowAboutCommand.ExecuteAsync();
                        break;

                }
            }
        }

        /// <summary>
        ///     Adjusts the view for login.
        /// </summary>
        public void SetLoginView()
        {
            NavView.OpenPaneLength = 0;
        }

        /// <summary>
        ///     Adjusts the view for the general usage.
        /// </summary>
        public void SetLoggedInView()
        {
            NavView.OpenPaneLength = 200;
        }
    }
}
