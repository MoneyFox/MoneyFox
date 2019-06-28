using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
	public partial class AddAccountPage
	{
		public AddAccountPage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddAccountVm;

            var saveAccountItem = new ToolbarItem
            {
                Command = new Command(async () => await (BindingContext as AddAccountViewModel)?.SaveCommand.ExecuteAsync()),
                Text = Strings.SaveAccountLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                IconImageSource = "ic_save.png"
            };

            ToolbarItems.Add(saveAccountItem);
        }
	}
}