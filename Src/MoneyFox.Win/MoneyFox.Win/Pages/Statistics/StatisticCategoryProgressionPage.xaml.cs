namespace MoneyFox.Win.Pages.Statistics;

using Core.Resources;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using ViewModels.Statistics;

public sealed partial class StatisticCategoryProgressionPage
{
    public StatisticCategoryProgressionPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.StatisticCategoryProgressionVm;
    }

    public StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)DataContext;

    public override string Header => Strings.MonthlyCashflowTitle;

    private void OpenFilterFlyout(object sender, RoutedEventArgs e)
    {
        FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
    }
}
