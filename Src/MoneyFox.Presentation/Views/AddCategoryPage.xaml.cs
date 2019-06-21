using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCategoryPage
    {
        private AddCategoryViewModel ViewModel => BindingContext as AddCategoryViewModel;

        public AddCategoryPage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddCategoryVm;

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.SaveCommand.Execute(null)),
                Text = Strings.SaveCategoryLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                Icon = "ic_save.png"
            });
        }
	}
}