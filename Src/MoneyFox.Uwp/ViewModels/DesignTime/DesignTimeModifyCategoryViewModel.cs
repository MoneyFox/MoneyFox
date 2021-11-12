using GalaSoft.MvvmLight.Command;
using MoneyFox.Uwp.Commands;
using MoneyFox.Uwp.ViewModels.Categories;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
    {
        public RelayCommand DeleteCommand { get; } = null!;

        public bool IsEdit { get; }
        public AsyncCommand SaveCommand { get; } = null!;

        public AsyncCommand CancelCommand { get; } = null!;

        public CategoryViewModel SelectedCategory { get; } = null!;
    }
}