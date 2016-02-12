using System;
using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
{
    public sealed partial class CashFlowUserControl : IDisposable
    {
        public CashFlowUserControl()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            CashFlowPlotView.Model = null;
        }
    }
}