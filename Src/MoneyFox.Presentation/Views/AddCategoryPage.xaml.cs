using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
	public partial class AddCategoryPage
    {
        private AddCategoryViewModel ViewModel => BindingContext as AddCategoryViewModel;

        public AddCategoryPage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddCategoryVm;

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.SaveCommand.ExecuteAsync().FireAndForgetSafeAsync()),
                Text = Strings.SaveCategoryLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                IconImageSource = "ic_save.png"
            });
        }
	}
}