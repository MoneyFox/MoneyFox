using MoneyFox.Business.ViewModels;
using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountListPage : MvxContentPage<AccountListViewModel>
	{
		public AccountListPage ()
		{
			InitializeComponent ();
		}
	}
}