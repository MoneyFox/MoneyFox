namespace MoneyFox.Ui.Views.Payments.PaymentList;

public partial class FilterPopup
{
    public FilterPopup(SelectFilterPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private SelectFilterPopupViewModel ViewModel => (SelectFilterPopupViewModel)BindingContext;

    private void Button_OnClicked(object sender, EventArgs e)
    {
        if (ViewModel.IsDateRangeValid)
        {
            ViewModel.FilterSelectedCommand.Execute(null);
            Close();
        }
    }
}
