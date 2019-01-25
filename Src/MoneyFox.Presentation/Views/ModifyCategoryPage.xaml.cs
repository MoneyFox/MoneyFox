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

		    ToolbarItems.Add(new ToolbarItem
		    {
		        Command = new Command(() => ViewModel.SaveCommand.Execute()),
		        Text = Strings.SaveCategoryLabel,
		        Priority = 0,
		        Order = ToolbarItemOrder.Primary,
		        Icon = "ic_save.png"
            });
        }

	    protected override void OnAppearing()
	    {
	        Title = ViewModel.Title;

	        if (Device.RuntimePlatform == Device.Android)
	        {
	            DeleteCategoryButton.IsVisible = false;

	            if (ViewModel.IsEdit)
	            {
	                ToolbarItems.Add(new ToolbarItem
	                {
	                    Command = new Command(() => ViewModel.DeleteCommand.Execute()),
	                    Text = Strings.DeleteLabel,
	                    Priority = 1,
	                    Order = ToolbarItemOrder.Secondary
	                });
	            }
	        }

            base.OnAppearing();
	    }
	}
}