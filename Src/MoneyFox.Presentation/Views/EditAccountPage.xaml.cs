using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditAccountPage
    {
        private EditAccountViewModel ViewModel => BindingContext as EditAccountViewModel;

        public EditAccountPage(int accountId)
		{
			InitializeComponent ();

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.SaveCommand.Execute(null)),
                Text = Strings.SaveAccountLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                Icon = "ic_save.png"
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.DeleteCommand.Execute(null)),
                Text = Strings.DeleteLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Secondary
            });

            ViewModel.AccountId = accountId;
        }
	}
}