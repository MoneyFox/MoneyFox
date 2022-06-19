namespace MoneyFox.Win.Pages.Statistics.StatisticCategorySummary;

using Microsoft.UI.Xaml;
using ViewModels.Statistics.StatisticCategorySummary;

public partial class PaymentListUserControl
{
    public PaymentListUserControl()
    {
        InitializeComponent();
        DataContextChanged += PaymentListUserControl_DataContextChanged;
    }

    private CategoryOverviewViewModel? ViewModel { get; set; }

    private void PaymentListUserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
    {
        if (args.NewValue is CategoryOverviewViewModel model)
        {
            ViewModel = model;
            Bindings.Update();
        }
    }
}
