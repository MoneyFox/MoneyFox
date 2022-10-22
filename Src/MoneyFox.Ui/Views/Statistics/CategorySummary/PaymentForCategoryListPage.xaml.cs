namespace MoneyFox.Ui.Views.Statistics;

using ViewModels.Statistics;

public partial class PaymentForCategoryListPage : ContentPage
{
    public PaymentForCategoryListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<PaymentForCategoryListViewModel>();
    }
}
