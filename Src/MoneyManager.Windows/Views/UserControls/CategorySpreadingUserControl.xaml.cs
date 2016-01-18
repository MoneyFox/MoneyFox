using System;

namespace MoneyManager.Windows.Views.UserControls
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