using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCategoryPage 
	{
		public AddCategoryPage ()
		{
			InitializeComponent ();

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => (BindingContext as AddCategoryViewModel)?.SaveCommand.Execute()),
                Text = Strings.SaveCategoryLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                Icon = "ic_save.png"
            });
        }
	}
}