using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
    {
        public MvxAsyncCommand SaveCommand { get; }
        public MvxAsyncCommand CancelCommand { get; }
        public MvxAsyncCommand DeleteCommand { get; }
        public CategoryViewModel SelectedCategory { get; }

        public bool IsEdit { get; }
    }
}
