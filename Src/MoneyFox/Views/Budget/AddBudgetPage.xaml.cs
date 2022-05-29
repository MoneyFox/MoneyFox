namespace MoneyFox.Views.Budget
{

    using Xamarin.Forms;

    public partial class AddBudgetPage : ContentPage
    {
        public AddBudgetPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AddBudgetViewModel;
        }

        private void AmountFieldGotFocus(object sender, FocusEventArgs e)
        {
            Dispatcher.BeginInvokeOnMainThread(
                () =>
                {
                    AmountEntry.CursorPosition = 0;
                    AmountEntry.SelectionLength = AmountEntry.Text != null ? AmountEntry.Text.Length : 0;
                });
        }
    }

}
