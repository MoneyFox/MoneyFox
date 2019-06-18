using System;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyPaymentUserControl
	{
		public ModifyPaymentUserControl ()
		{
			InitializeComponent ();
            
            ResetIcon.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => (BindingContext as ModifyPaymentViewModel)?.ResetCategoryCommand.Execute())
            });
        }

        private void AmountFieldGotFocus(object sender, FocusEventArgs e)
        {
            AmountEntry.Text = string.Empty;
        }

        private void ChargedAccount_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            (BindingContext as ModifyPaymentViewModel)?.SelectedItemChangedCommand.Execute();
        }

        private void TargetAccount_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            (BindingContext as ModifyPaymentViewModel)?.SelectedItemChangedCommand.Execute();
        }
    }
}