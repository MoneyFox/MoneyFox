using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class AddAccountPage
    {
        private AddAccountViewModel ViewModel => BindingContext as AddAccountViewModel;

        public AddAccountPage()
        {
            InitializeComponent();

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Close()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
            {
                Command = new Command(async () => await ViewModel.SaveCommand.ExecuteAsync()),
                Text = Strings.SaveLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);

            BindingContext = ViewModelLocator.AddAccountVm;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private async Task Close()
        {
            await Navigation.PopModalAsync();
        }
    }
}
