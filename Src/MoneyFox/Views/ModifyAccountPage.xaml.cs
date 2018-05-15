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
                Icon = Icon = "IconSave.png"
            };

            ToolbarItems.Add(saveAccountItem);
		    Title = ViewModel.Title;
		}
    }
}