using System;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels.Statistics;

namespace MoneyManager.Windows.Controls.StatisticControls
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
