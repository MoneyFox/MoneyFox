namespace MoneyManager.Windows.Views.Dialogs
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