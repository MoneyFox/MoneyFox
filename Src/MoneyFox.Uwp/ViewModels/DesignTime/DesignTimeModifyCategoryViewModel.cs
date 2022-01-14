using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.ViewModels.Categories;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.DesignTime
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