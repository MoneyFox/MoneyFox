
using System;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels.Statistics;

namespace MoneyManager.Windows.Controls.StatisticControls
{
    public sealed partial class CategorySpreadingUserControl : IDisposable
    {
        public CategorySpreadingUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<StatisticViewModel>();
        }


        public void Dispose()
        {
            SpreadingPlotView.Model = null;
        }
    }
}
