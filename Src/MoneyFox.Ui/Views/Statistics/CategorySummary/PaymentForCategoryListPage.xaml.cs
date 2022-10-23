namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using ViewModels.Statistics;

public partial class PaymentForCategoryListPage : ContentPage
{
    public PaymentForCategoryListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<PaymentForCategoryListViewModel>();
    }
}



