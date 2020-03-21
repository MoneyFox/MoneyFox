using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.Utilities;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.DesignTime;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    /// View to display an list of accounts.
    /// </summary>
    public sealed partial class AccountListView
    {
        public override string Header => Strings.AccountsTitle;

        /// <summary>
        /// Initialize View.
        /// </summary>
        public AccountListView()
        {
            InitializeComponent();

            if(DesignMode.DesignModeEnabled)
            {
                DataContext = new DesignTimeAccountListViewModel();
            }
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;

            if(!(element.DataContext is AccountViewModel account))
                return;

            (DataContext as AccountListViewModel)?.EditAccountCommand.Execute(account);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            //this has to be called before the dialog service since otherwise the data context is reseted and the account will be null
            var element = (FrameworkElement) sender;

            if(!(element.DataContext is AccountViewModel account))
                return;

            (DataContext as AccountListViewModel)?.DeleteAccountCommand.ExecuteAsync(account).FireAndForgetSafeAsync();
        }
    }
}
