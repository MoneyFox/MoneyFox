using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditCategoryPage
    {
        private EditCategoryViewModel ViewModel => BindingContext as EditCategoryViewModel;

        public EditCategoryPage(int categoryId)
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.EditCategoryVm;

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.SaveCommand.ExecuteAsync().FireAndForgetSafeAsync()),
                Text = Strings.SaveCategoryLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                Icon = "ic_save.png"
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.DeleteCommand.ExecuteAsync().FireAndForgetSafeAsync()),
                Text = Strings.DeleteLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Secondary
            });

            ViewModel.CategoryId = categoryId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
	}
}