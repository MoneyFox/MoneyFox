using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class EditCategoryPage
    {
        private EditCategoryViewModel ViewModel => BindingContext as EditCategoryViewModel;

        public EditCategoryPage(int categoryId)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditCategoryVm;

            var saveItem = new ToolbarItem
                           {
                               Command = new Command(async() => await ViewModel.SaveCommand.ExecuteAsync()),
                               Text = Strings.SaveLabel,
                               Priority = 0,
                               Order = ToolbarItemOrder.Primary
                           };

            var cancelItem = new ToolbarItem
                             {
                                 Command = new Command(async() => await ViewModel.CancelCommand.ExecuteAsync()),
                                 Text = Strings.CancelLabel,
                                 Priority = -1,
                                 Order = ToolbarItemOrder.Primary
                             };

            ToolbarItems.Add(saveItem);
            ToolbarItems.Add(cancelItem);

            ViewModel.CategoryId = categoryId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
