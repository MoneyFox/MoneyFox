namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using CommunityToolkit.Maui.Views;

public partial class StatisticCategoryProgressionPage : ContentPage
{
    public StatisticCategoryProgressionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticCategoryProgressionViewModel>();
    }

    private StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
