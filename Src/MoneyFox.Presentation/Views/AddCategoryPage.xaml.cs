using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Ui.Shared.Utilities;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class AddCategoryPage
    {
        private AddCategoryViewModel ViewModel => BindingContext as AddCategoryViewModel;

        public AddCategoryPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AddCategoryVm;

            var saveItem = new ToolbarItem
            {
                Command = new Command(async () => await ViewModel.SaveCommand.ExecuteAsync()),
                Text = Strings.SaveLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await ViewModel.CancelCommand.ExecuteAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(saveItem);
            ToolbarItems.Add(cancelItem);

            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
