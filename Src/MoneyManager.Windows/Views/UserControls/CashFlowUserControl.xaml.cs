using System;
using MoneyManager.Core.ViewModels.Statistics;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
{
    public sealed partial class CashFlowUserControl : IDisposable
    {
        public CashFlowUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<StatisticViewModel>();
        }

        public void Dispose()
        {
            CashFlowPlotView.Model = null;
        }
    }
}