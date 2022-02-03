using CommunityToolkit.Mvvm.Input;
using MoneyFox.Win.ViewModels.Categories;

namespace MoneyFox.Win.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
    {
        public AsyncRelayCommand SaveCommand { get; } = null!;

        public AsyncRelayCommand CancelCommand { get; } = null!;

        public RelayCommand DeleteCommand { get; } = null!;

        public CategoryViewModel SelectedCategory { get; } = null!;

        public bool IsEdit { get; }
    }
}