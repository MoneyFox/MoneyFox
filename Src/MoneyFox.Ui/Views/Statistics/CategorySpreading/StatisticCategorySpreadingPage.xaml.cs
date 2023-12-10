namespace MoneyFox.Ui.Views.Statistics.CategorySpreading;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticCategorySpreadingPage : IBindablePage
{
    public StatisticCategorySpreadingPage()
    {
        InitializeComponent();
    }

    private StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
