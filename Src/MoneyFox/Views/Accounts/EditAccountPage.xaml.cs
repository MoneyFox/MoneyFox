using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Accounts;
using Xamarin.Forms;

namespace MoneyFox.Views.Accounts
{
    public partial class EditAccountPage
    {
        private EditAccountViewModel ViewModel => (EditAccountViewModel) BindingContext;

        private int accountId;

        public EditAccountPage(int accountId)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditAccountViewModel;
            this.accountId = accountId;

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Navigation.PopModalAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
            {
                Command = new Command(() => ViewModel.SaveCommand.Execute(null)),
                Text = Strings.SaveLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);
        }

        protected override async void OnAppearing() => await ViewModel.InitializeAsync(accountId);
    }
}
