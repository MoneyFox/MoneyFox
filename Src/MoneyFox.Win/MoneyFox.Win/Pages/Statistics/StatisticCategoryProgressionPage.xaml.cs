namespace MoneyFox.Win.Pages.Statistics;

using Core.Resources;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using ViewModels.Statistics;

public sealed partial class StatisticCategoryProgressionPage
{
    public StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)DataContext;

    public override string Header => Strings.MonthlyCashflowTitle;

    public StatisticCategoryProgressionPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.StatisticCategoryProgressionVm;
    }

    private void OpenFilterFlyout(object sender, RoutedEventArgs e) =>
        FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
}