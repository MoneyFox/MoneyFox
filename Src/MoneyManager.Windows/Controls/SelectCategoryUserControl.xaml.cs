using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class SelectCategoryUserControl
    {
        public SelectCategoryUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SelectCategoryViewModel>();
        }
    }
}