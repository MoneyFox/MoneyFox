using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyFox.Windows.Views.Dialogs
{
    public sealed partial class SelectDateRangeDialog
    {
        public SelectDateRangeDialog()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<SelectDateRangeDialogViewModel>();
        }
    }
}