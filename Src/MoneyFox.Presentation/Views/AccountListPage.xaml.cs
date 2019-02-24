using System;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(WrapInNavigationPage = false, Title = "Accounts", Icon = "ic_accounts_black")]
    public partial class AccountListPage
	{
		public AccountListPage ()
		{
			InitializeComponent ();
		    AccountsList.ItemTapped += (sender, args) =>
		    {
		        AccountsList.SelectedItem = null;
		        ViewModel.OpenOverviewCommand.Execute(args.Item);
		    };
		    Title = Strings.AccountsTitle;
		}

	    private async void AddItem_Clicked(object sender, EventArgs e)
	    {
	        await Navigation.PushPopupAsync(new AddAccountAndPaymentPopup { BindingContext = ViewModel.ViewActionViewModel })
                .ConfigureAwait(true);
        }

        private void EditAccount(object sender, EventArgs e)
	    {
            if (!(sender is MenuItem menuItem)) return;

            ViewModel.EditAccountCommand.ExecuteAsync(menuItem.CommandParameter as AccountViewModel);
	    }

	    private void DeleteAccount(object sender, EventArgs e)
	    {
            if (!(sender is MenuItem menuItem)) return;

            ViewModel.DeleteAccountCommand.ExecuteAsync(menuItem.CommandParameter as AccountViewModel);
	    }
    }
}