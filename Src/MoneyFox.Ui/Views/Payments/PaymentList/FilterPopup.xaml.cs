namespace MoneyFox.Ui.Views.Payments.PaymentList;

public partial class FilterPopup
{
    public FilterPopup(SelectFilterPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
