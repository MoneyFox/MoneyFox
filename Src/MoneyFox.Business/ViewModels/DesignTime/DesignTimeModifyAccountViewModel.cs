using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeModifyAccountViewModel : BaseViewModel, IModifyAccountViewModel
    {
        /// <inheritdoc />
        public bool IsEdit { get; }
        
        /// <inheritdoc />
        public string Title { get; }

        /// <inheritdoc />
        public string AmountString { get; }

        /// <inheritdoc />
        public AccountViewModel SelectedAccount { get; }

        /// <inheritdoc />
        public MvxAsyncCommand SaveCommand { get; }

        /// <inheritdoc />
        public MvxAsyncCommand DeleteCommand { get; }

        /// <inheritdoc />
        public MvxAsyncCommand CancelCommand { get; }
    }
}
