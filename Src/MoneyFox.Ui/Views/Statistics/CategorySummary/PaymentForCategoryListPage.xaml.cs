namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using Common.Navigation;

public partial class PaymentForCategoryListPage: IBindablePage
{
    public PaymentForCategoryListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<PaymentForCategoryListViewModel>();
    }

    private PaymentForCategoryListViewModel ViewModel => (PaymentForCategoryListViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
