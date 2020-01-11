using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class AddAccountPage
    {
        private AddAccountViewModel ViewModel => BindingContext as AddAccountViewModel;

        public AddAccountPage()
        {
            InitializeComponent();

            var saveItem = new ToolbarItem
            {
                Command = new Command(async () => await ViewModel.SaveCommand.ExecuteAsync()),
                Text = Strings.SaveLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(saveItem);

            BindingContext = ViewModelLocator.AddAccountVm;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
