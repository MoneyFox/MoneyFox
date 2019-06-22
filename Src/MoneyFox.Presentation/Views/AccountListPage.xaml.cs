using System;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.Interfaces;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountListPage
    {
        private IAccountListViewModel ViewModel => BindingContext as AccountListViewModel;

        public AccountListPage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AccountListVm;

		    AccountsList.ItemTapped += (sender, args) =>
		    {
		        AccountsList.SelectedItem = null;
                ViewModel?.OpenOverviewCommand.Execute(args.Item);
		    };
        }

        protected override void OnAppearing()
        {
            ViewModel?.LoadDataCommand.Execute(null);
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
	    {
	        await Navigation.PushPopupAsync(new AddAccountAndPaymentPopup { BindingContext = ViewModel?.ViewActionViewModel });
        }

        private void EditAccount(object sender, EventArgs e)
	    {
            if (!(sender is MenuItem menuItem)) return;

            ViewModel?.EditAccountCommand.Execute(menuItem.CommandParameter as AccountViewModel);
	    }

	    private void DeleteAccount(object sender, EventArgs e)
	    {
            if (!(sender is MenuItem menuItem)) return;

            ViewModel?.DeleteAccountCommand.Execute(menuItem.CommandParameter as AccountViewModel);
	    }
    }
}