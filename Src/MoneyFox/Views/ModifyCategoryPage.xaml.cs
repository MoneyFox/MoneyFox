using MoneyFox.Foundation.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyCategoryPage
	{
		public ModifyCategoryPage ()
		{
			InitializeComponent ();

		    var saveCategoryItem = new ToolbarItem
		    {
		        Command = new Command(() => ViewModel.SaveCommand.Execute()),
		        Text = Strings.SaveCategoryLabel,
		        Priority = 0,
		        Order = ToolbarItemOrder.Primary,
		        Icon = Icon = "IconSave.png"
		    };

		    ToolbarItems.Add(saveCategoryItem);
		    Title = ViewModel.Title;
		}
	}
}