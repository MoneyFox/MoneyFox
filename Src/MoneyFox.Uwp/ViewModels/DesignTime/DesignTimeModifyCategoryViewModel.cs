using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
    {
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public CategoryViewModel SelectedCategory { get; }

        public bool IsEdit { get; }
    }
}
