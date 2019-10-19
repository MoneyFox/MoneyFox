using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Views
{
    public partial class EditCategoryPage
    {
        private EditCategoryViewModel ViewModel => BindingContext as EditCategoryViewModel;

        public EditCategoryPage(int categoryId)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditCategoryVm;

            ViewModel.CategoryId = categoryId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafe();
        }
    }
}
