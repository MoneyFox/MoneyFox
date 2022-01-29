using CommunityToolkit.Mvvm.Input;
using MoneyFox.Win.ViewModels.Accounts;

namespace MoneyFox.Win.ViewModels.DesignTime
{
    public class DesignTimeModifyAccountViewModel
    {
        /// <inheritdoc />
        public bool IsEdit { get; }

        /// <inheritdoc />
        public string Title { get; } = "";

        /// <inheritdoc />
        public string AmountString { get; } = "";

        /// <inheritdoc />
        public AccountViewModel SelectedAccount { get; } = null!;

        /// <inheritdoc />
        public RelayCommand SaveCommand { get; } = null!;

        /// <inheritdoc />
        public RelayCommand DeleteCommand { get; } = null!;

        /// <inheritdoc />
        public RelayCommand CancelCommand { get; } = null!;
    }
}