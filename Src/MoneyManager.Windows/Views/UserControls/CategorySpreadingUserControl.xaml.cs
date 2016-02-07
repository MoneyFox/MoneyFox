using System;
using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
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