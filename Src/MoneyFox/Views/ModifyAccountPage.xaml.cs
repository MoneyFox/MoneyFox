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
                Text = Strings.SaveAccountLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
            };

            ToolbarItems.Add(saveAccountItem);
        }
	}
}