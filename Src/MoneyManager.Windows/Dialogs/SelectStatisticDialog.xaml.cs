using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Dialogs
{
    public sealed partial class SelectStatisticDialog
    {
        public SelectStatisticDialog()
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<SelectStatisticDialogViewModel>();
        }
    }
}