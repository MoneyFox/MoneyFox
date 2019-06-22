using System.Linq;
using Windows.UI.Xaml.Controls;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public AppShell()
        {
            InitializeComponent();
            NavView.SelectedItem = NavView.MenuItems.First();
        }

        public Frame MainFrame => ContentFrame;

        private void NavView_ItemInvoked(NavigationView navigationView, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ViewModel.ShowSettingsCommand.Execute(null);
            }
            else
            {
                if (args.InvokedItem == null) return;
            }

            if (args.InvokedItem.Equals(Strings.AccountsTitle))
            {
                ViewModel.ShowAccountListCommand.Execute(null);
            } 
            else if (args.InvokedItem.Equals(Strings.CategoriesTitle))
            {
                ViewModel.ShowCategoryListCommand.Execute(null);
            }
            else if (args.InvokedItem.Equals(Strings.StatisticsTitle))
            {
                ViewModel.ShowStatisticSelectorCommand.Execute(null);
            } 
            else if (args.InvokedItem.Equals(Strings.BackupTitle))
            {
                ViewModel.ShowBackupViewCommand.Execute(null);
            } 
            else if (args.InvokedItem.Equals(Strings.SettingsTitle))
            {
                ViewModel.ShowSettingsCommand.Execute(null);
            } 
            else if (args.InvokedItem.Equals(Strings.AboutTitle))
            {
                ViewModel.ShowAboutCommand.Execute(null);
            }
        }
    }
}
