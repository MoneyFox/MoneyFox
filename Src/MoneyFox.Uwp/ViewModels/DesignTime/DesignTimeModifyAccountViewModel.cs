using GalaSoft.MvvmLight.Command;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeModifyAccountViewModel
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
        public RelayCommand SaveCommand { get; }

        /// <inheritdoc />
        public RelayCommand DeleteCommand { get; }

        /// <inheritdoc />
        public RelayCommand CancelCommand { get; }
    }
}
