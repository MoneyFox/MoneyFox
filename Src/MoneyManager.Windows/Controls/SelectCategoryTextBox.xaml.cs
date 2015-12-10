using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Core.ViewModels.Controls;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class SelectCategoryTextBox
    {
        public SelectCategoryTextBox()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SelectCategoryTextBoxViewModel>();
        }
    }
}