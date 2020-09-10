using CommonServiceLocator;
using MoneyFox.Uwp.ViewModels;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class SelectCategoryDialog
    {
        public SelectCategoryDialog()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<SelectCategoryListViewModel>();
        }
    }
}
