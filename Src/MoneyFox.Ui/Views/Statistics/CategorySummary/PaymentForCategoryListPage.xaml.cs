namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

public partial class PaymentForCategoryListPage : ContentPage
{
    public PaymentForCategoryListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<PaymentForCategoryListViewModel>();
    }
}
