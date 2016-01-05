using System;

namespace MoneyManager.Windows.Controls.StatisticControls
{
    public sealed partial class CategorySpreadingUserControl : IDisposable
    {
        public CategorySpreadingUserControl()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            SpreadingPlotView.Model = null;
        }
    }
}