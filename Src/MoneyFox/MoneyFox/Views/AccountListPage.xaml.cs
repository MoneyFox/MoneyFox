using System;
using MvvmCross.Forms.Views.Attributes;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(WrapInNavigationPage = false, Title = "Accounts", Icon = "ic_accounts")]
    public partial class AccountListPage
	{
		public AccountListPage ()
		{
			InitializeComponent ();
		}

	    private void AddItem_Clicked(object sender, EventArgs e)
	    {
	        throw new NotImplementedException();
	    }

	    private void EditAccount(object sender, EventArgs e)
	    {
	        throw new NotImplementedException();
	    }

	    private void DeleteAccount(object sender, EventArgs e)
	    {
	        throw new NotImplementedException();
	    }
	}
}