using System;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.UserControls
{
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
