using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.ViewModels.Categories;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
    {
        public AsyncCommand SaveCommand { get; } = null!;

        public AsyncCommand CancelCommand { get; } = null!;

        public RelayCommand DeleteCommand { get; } = null!;

        public CategoryViewModel SelectedCategory { get; } = null!;

        public bool IsEdit { get; }
    }
}
