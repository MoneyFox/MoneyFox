namespace MoneyFox.Views.Budget
{

    using System;
    using Extensions;
    using Xamarin.Forms;

    public partial class ModifyBudgetView
    {
        public ModifyBudgetView()
        {
            InitializeComponent();
        }

        private void AmountFieldGotFocus(object sender, FocusEventArgs e)
        {
            Dispatcher.BeginInvokeOnMainThread(
                () =>
                {
                    AmountEntry.CursorPosition = 0;
                    AmountEntry.SelectionLength = AmountEntry.Text?.Length ?? 0;
                });
        }
    }

}
