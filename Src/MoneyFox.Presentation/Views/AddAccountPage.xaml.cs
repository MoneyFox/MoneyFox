using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAccountPage
	{
		public AddAccountPage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddAccountVm;

            var saveAccountItem = new ToolbarItem
            {
                Command = new Command(() => (BindingContext as AddAccountViewModel)?.SaveCommand.Execute(null)),
                Text = Strings.SaveAccountLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                Icon = "ic_save.png"
            };

            ToolbarItems.Add(saveAccountItem);
        }
	}
}