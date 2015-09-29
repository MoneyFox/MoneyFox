using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Windows.Dialogs
{
    public sealed partial class CategoryDialog
    {
        public CategoryDialog(Category category = null)
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<CategoryDialogViewModel>();

            if (category != null)
            {
                ((CategoryDialogViewModel) DataContext).IsEdit = true;
                ((CategoryDialogViewModel) DataContext).Selected = category;
            }
        }
    }
}