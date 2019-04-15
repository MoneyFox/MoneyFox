using Windows.UI.Xaml.Controls;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        public Frame MainFrame => ContentFrame;

        private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                await ((MainViewModel) ViewModel).ShowSettingsCommand
                                                 .ExecuteAsync()
                                                 ;
            }
            else
            {
                if (args.InvokedItem == null) return;
            }

            if (args.InvokedItem.Equals(Strings.AccountsTitle))
            {
                await ((MainViewModel) ViewModel).ShowAccountListCommand
                                                 .ExecuteAsync()
                                                 ;
            } 
            else if (args.InvokedItem.Equals(Strings.CategoriesTitle))
            {
                await ((MainViewModel) ViewModel).ShowCategoryListCommand
                                                 .ExecuteAsync()
                                                 ;
            }
            else if (args.InvokedItem.Equals(Strings.StatisticsTitle))
            {
                await ((MainViewModel) ViewModel).ShowStatisticSelectorCommand
                                                 .ExecuteAsync()
                                                 ;
            } 
            else if (args.InvokedItem.Equals(Strings.BackupTitle))
            {
                await ((MainViewModel) ViewModel).ShowBackupViewCommand
                                                 .ExecuteAsync()
                                                 ;
            } 
            else if (args.InvokedItem.Equals(Strings.SettingsTitle))
            {
                await ((MainViewModel) ViewModel).ShowSettingsCommand
                                                 .ExecuteAsync()
                                                 ;
            } 
            else if (args.InvokedItem.Equals(Strings.AboutTitle))
            {
                await ((MainViewModel) ViewModel).ShowAboutCommand
                                                 .ExecuteAsync()
                                                 ;
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
