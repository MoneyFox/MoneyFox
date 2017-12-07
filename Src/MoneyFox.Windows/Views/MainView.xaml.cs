using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyFox.Business.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Attributes;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxPagePresentation]
    [MvxViewFor(typeof(MenuViewModel))]
    public sealed partial class MainView
    {
        public MainView()
        {
            this.InitializeComponent();

            CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
        }
        
        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitle.Margin = new Thickness(CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset + 12, 8, 0, 0);
        }

        public Frame MainFrame => ContentFrame;

        private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                await ((MenuViewModel) ViewModel).ShowSettingsCommand.ExecuteAsync();
            } 
            else
            {
                switch (args.InvokedItem)
                {
                    case "Accounts":
                        await ((MenuViewModel) ViewModel).ShowAccountListCommand.ExecuteAsync();
                        break;

                    case "Statistics":
                        await ((MenuViewModel) ViewModel).ShowStatisticSelectorCommand.ExecuteAsync();
                        break;

                    case "Categories":
                        await ((MenuViewModel) ViewModel).ShowCategoryListCommand.ExecuteAsync();
                        break;

                    case "Backup":
                        await ((MenuViewModel) ViewModel).ShowBackupViewCommand.ExecuteAsync();
                        break;

                    case "Settings":
                        await ((MenuViewModel) ViewModel).ShowSettingsCommand.ExecuteAsync();
                        break;

                    case "About":
                        await ((MenuViewModel) ViewModel).ShowAboutCommand.ExecuteAsync();
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
