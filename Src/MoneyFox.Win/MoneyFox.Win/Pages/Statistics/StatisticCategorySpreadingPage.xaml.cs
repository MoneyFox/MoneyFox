namespace MoneyFox.Win.Pages.Statistics;

using Core.Resources;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using ViewModels.Statistics;

public sealed partial class StatisticCategorySpreadingPage
{
    public StatisticCategorySpreadingPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.StatisticCategorySpreadingVm;
    }

    public override bool ShowHeader => false;

    public StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)DataContext;

    public override string Header => Strings.CategorySpreadingTitle;

    private void OpenFilterFlyout(object sender, RoutedEventArgs e)
    {
        FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
    }
}
