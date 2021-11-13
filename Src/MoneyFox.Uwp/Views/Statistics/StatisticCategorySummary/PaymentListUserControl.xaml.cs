using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;
using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics.StatisticCategorySummary
{
    public partial class PaymentListUserControl
    {
        public PaymentListUserControl()
        {
            InitializeComponent();

            DataContextChanged += PaymentListUserControl_DataContextChanged;
        }

        private void PaymentListUserControl_DataContextChanged(FrameworkElement sender,
            DataContextChangedEventArgs args)
        {
            if(args.NewValue is CategoryOverviewViewModel model)
            {
                ViewModel = model;
                Bindings.Update();
            }
        }

        public CategoryOverviewViewModel? ViewModel { get; set; }
    }
}