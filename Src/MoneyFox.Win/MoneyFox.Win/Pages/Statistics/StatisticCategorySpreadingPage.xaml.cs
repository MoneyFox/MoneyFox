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
        DataContext = App.GetViewModel<StatisticCategorySpreadingViewModel>();
    }

    public override bool ShowHeader => false;

    internal StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)DataContext;

    public override string Header => Strings.CategorySpreadingTitle;

    private void OpenFilterFlyout(object sender, RoutedEventArgs e)
    {
        FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
    }
}
