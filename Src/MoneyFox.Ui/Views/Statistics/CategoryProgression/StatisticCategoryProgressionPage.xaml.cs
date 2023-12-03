namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticCategoryProgressionPage : IBindablePage
{
    public StatisticCategoryProgressionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticCategoryProgressionViewModel>();
    }

    private StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
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
