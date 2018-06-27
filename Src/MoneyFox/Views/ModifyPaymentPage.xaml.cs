using MoneyFox.Foundation.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyPaymentPage
	{
		public ModifyPaymentPage ()
		{
			InitializeComponent ();

		    ToolbarItems.Add(new ToolbarItem
		    {
		        Command = new Command(() => ViewModel.SaveCommand.Execute()),
		        Text = Strings.SavePaymentLabel,
		        Priority = 0,
		        Order = ToolbarItemOrder.Primary,
		        Icon = "IconSave.png"
		    });

		    ResetIcon.GestureRecognizers.Add(new TapGestureRecognizer
		    {
                Command = new Command(() => ViewModel.ResetCategoryCommand.Execute())
		    });
        }

	    protected override void OnAppearing()
	    {
	        Title = ViewModel.Title;
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

            base.OnAppearing();
	    }

        private void AmountFieldGotFocus(object sender, FocusEventArgs e)
	    {
	        AmountEntry.Text = string.Empty;
	    }
	}
}