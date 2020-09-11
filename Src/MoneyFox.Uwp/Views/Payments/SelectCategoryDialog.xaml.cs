using CommonServiceLocator;
using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class SelectCategoryDialog
    {
        private SelectCategoryListViewModel ViewModel => (SelectCategoryListViewModel)DataContext;

        public SelectCategoryDialog()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<SelectCategoryListViewModel>();
        }

        protected override void OnBringIntoViewRequested(BringIntoViewRequestedEventArgs e) => ViewModel.AppearingCommand.Execute(null);
    }
}
