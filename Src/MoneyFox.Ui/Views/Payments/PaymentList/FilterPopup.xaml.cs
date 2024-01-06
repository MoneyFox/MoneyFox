namespace MoneyFox.Ui.Views.Payments.PaymentList;

public partial class FilterPopup
{
    public FilterPopup()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectFilterDialogViewModel>();
    }

    public FilterPopup(PaymentListFilterChangedMessage message)
    {
        InitializeComponent();
        ViewModel.Initialize(message);
    }

    private SelectFilterDialogViewModel ViewModel => (SelectFilterDialogViewModel)BindingContext;

    private void Button_OnClicked(object sender, EventArgs e)
    {
        if (ViewModel.IsDateRangeValid)
        {
            ViewModel.FilterSelectedCommand.Execute(null);
            Close();
        }
    }
}
