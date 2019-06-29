using System;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyPaymentUserControl
    {
        private ModifyPaymentViewModel ViewModel => BindingContext as ModifyPaymentViewModel;

        public ModifyPaymentUserControl ()
		{
			InitializeComponent ();
        }

        private void AmountFieldGotFocus(object sender, FocusEventArgs e)
        {
            AmountEntry.Text = string.Empty;
        }

        private void ChargedAccount_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel?.SelectedItemChangedCommand.Execute(null);
        }

        private void TargetAccount_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel?.SelectedItemChangedCommand.Execute(null);
        }
    }
}