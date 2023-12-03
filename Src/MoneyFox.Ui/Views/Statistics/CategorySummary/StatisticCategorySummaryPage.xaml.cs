namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticCategorySummaryPage : IBindablePage
{
    public StatisticCategorySummaryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticCategorySummaryViewModel>();
    }

    private StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.LoadedCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
