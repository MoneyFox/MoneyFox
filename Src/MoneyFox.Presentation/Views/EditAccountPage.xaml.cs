using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class EditAccountPage
    {
        private EditAccountViewModel ViewModel => BindingContext as EditAccountViewModel;

        public EditAccountPage(int accountId)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditAccountVm;

            var saveItem = new ToolbarItem
            {
                Command = new Command(async () => await ViewModel.SaveCommand.ExecuteAsync()),
                Text = Strings.SaveLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(saveItem);

            ViewModel.AccountId = accountId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
