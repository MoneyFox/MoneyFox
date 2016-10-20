using MoneyFox.Business.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views.Dialogs
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