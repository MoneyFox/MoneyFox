using MoneyFox.Foundation.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyAccountPage
	{
		public ModifyAccountPage ()
		{
			InitializeComponent ();

            var saveAccountItem = new ToolbarItem
            {
                Command = new Command(() => ViewModel.SaveCommand.Execute()),
                Text = Strings.SaveAccountLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                Icon = "IconSave.png"
            };

            ToolbarItems.Add(saveAccountItem);
		}

	    protected override void OnAppearing()
	    {
	        Title = ViewModel.Title;

	        if (Device.RuntimePlatform == Device.Android && ViewModel.IsEdit)
	        {
	            ToolbarItems.Add(new ToolbarItem
	            {
	                Command = new Command(() => ViewModel.DeleteCommand.Execute()),
	                Text = Strings.DeleteLabel,
	                Priority = 1,
	                Order = ToolbarItemOrder.Secondary
	            });

	            DeleteAccountButton.IsVisible = false;
	        }

            base.OnAppearing();
	    }

	    private void AmountFieldGotFocused(object sender, FocusEventArgs e)
	    {
	        AmountEntry.Text = string.Empty;
	    }
	}
}