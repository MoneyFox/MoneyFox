using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Dialogs
{
    public sealed partial class SelectDateRangeDialog
    {
        public SelectDateRangeDialog()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SelectDateRangeDialogViewModel>();
        }
    }
}