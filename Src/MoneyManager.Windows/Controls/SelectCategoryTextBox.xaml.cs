using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

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