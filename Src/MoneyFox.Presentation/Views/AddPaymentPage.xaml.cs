using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Parameters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPaymentPage
	{
		public AddPaymentPage (ModifyPaymentParameter parameter)
		{
			InitializeComponent ();

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => (BindingContext as AddPaymentViewModel)?.SaveCommand.Execute()),
                Text = Strings.SavePaymentLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                Icon = "ic_save.png"
            });

            ((AddPaymentViewModel) BindingContext).PassedParameter = parameter;
        }
    }
}