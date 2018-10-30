using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Business.ViewModels;
using MoneyFox.Business.ViewModels.DesignTime;
using win = Windows.UI;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Microsoft.Toolkit.Uwp.Notifications;
using System;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     View to display an list of accounts.
    /// </summary>
    public sealed partial class AccountListView
    {
        private AppServiceConnection liveTileService;
        /// <summary>
        ///     Initialize View.
        /// </summary>
        public AccountListView()
        {
            InitializeComponent();

            if (DesignMode.DesignModeEnabled)
            {
                ViewModel = new DesignTimeAccountListViewModel();
            } 
         
        }

        private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;
			flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var account = element.DataContext as AccountViewModel;
            if (account == null)
            {
                return;
            }

            (DataContext as AccountListViewModel)?.EditAccountCommand.Execute(account);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            //this has to be called before the dialog service since otherwise the datacontext is reseted and the account will be null
            var element = (FrameworkElement)sender;
            var account = element.DataContext as AccountViewModel;
            if (account == null)
            {
                return;
            }

            (DataContext as AccountListViewModel)?.DeleteAccountCommand.Execute(account);
        }
		private async void AddToStartMenu_ClickAsync(object sender, RoutedEventArgs e)
		{
			if (this.liveTileService == null)
			{
				this.liveTileService = new AppServiceConnection();
				this.liveTileService.AppServiceName = "LiveTiles";
			}
			var element = (FrameworkElement)sender;
			var account = element.DataContext as AccountViewModel;
			if (account == null)
			{
				return;
			}
			var name = account.Account;
			int id = name.Data.Id;
			var sendvalues = new ValueSet();
			sendvalues.Add("action", "create");
			sendvalues.Add("accountid", id.ToString());
			var te = await liveTileService.SendMessageAsync(sendvalues);
		}


	}
}