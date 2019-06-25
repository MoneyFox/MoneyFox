using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Utilities;
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
			InitializeComponent();
            BindingContext = ViewModelLocator.EditAccountVm;

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(async () => await ViewModel.SaveCommand.ExecuteAsync()),
                Text = Strings.SaveAccountLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                IconImageSource = "ic_save.png"
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(async () => await ViewModel.DeleteCommand.ExecuteAsync()),
                Text = Strings.DeleteLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Secondary
            });

            ViewModel.AccountId = accountId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
	}
}