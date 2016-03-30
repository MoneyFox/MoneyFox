using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.Interfaces.Shotcuts;
using MoneyFox.Core.SettingAccess;

// ReSharper disable ExplicitCallerInfoArgument

namespace MoneyFox.Core.ViewModels
{
    /// <summary>
    ///     Provides the information for the TileSettingsView
    /// </summary>
    public class SettingsShortcutsViewModel : ViewModelBase
    {
        private readonly IIncomeShortcut incomeShortcut;
        private readonly ISpendingShortcut spendingShortcut;
        private readonly ITransferShortcut transferShortcut;

        /// <summary>
        ///     Creates a SettingsShortcutsViewModel object
        /// </summary>
        public SettingsShortcutsViewModel(ISpendingShortcut spendingShortcut, IIncomeShortcut incomeShortcut,
            ITransferShortcut transferShortcut)
        {
            this.spendingShortcut = spendingShortcut;
            this.incomeShortcut = incomeShortcut;
            this.transferShortcut = transferShortcut;
        }

        public bool ShowInfoOnMainTile
        {
            get { return Settings.ShowCashFlowOnMainTile; }
            set
            {
                Settings.ShowCashFlowOnMainTile = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Creates a Expense Shortcut
        /// </summary>
        public RelayCommand CreateSpendingShortcutCommand => new RelayCommand(CreateSpendingShortcut);

        /// <summary>
        ///     Creates an Income Shortcut
        /// </summary>
        public RelayCommand CreateIncomeShortcutCommand => new RelayCommand(CreateIncomeShortcut);

        /// <summary>
        ///     Creates an Transfer Shortcut
        /// </summary>
        public RelayCommand CreateTransferShortcutCommand => new RelayCommand(CreateTransferShortcut);

        /// <summary>
        ///     Indicates if there exists a spending shortcut
        /// </summary>
        public bool IsSpendingShortcutPinned => spendingShortcut.IsShortcutExisting;

        /// <summary>
        ///     Indicates if there is a income shortcut
        /// </summary>
        public bool IsIncomeShortcutPinned => incomeShortcut.IsShortcutExisting;

        /// <summary>
        ///     Indicates if there is a transfer shortcut
        /// </summary>
        public bool IsTransferShortcutPinned => transferShortcut.IsShortcutExisting;

        /// <summary>
        ///     Removes the existing Expense Shortcut
        /// </summary>
        public RelayCommand RemoveSpendingShortcutCommand => new RelayCommand(RemoveSpendingShortcut);

        /// <summary>
        ///     Removes the existing Income Shortcut
        /// </summary>
        public RelayCommand RemoveIncomeShortcutCommand => new RelayCommand(RemoveIncomeShortcut);

        /// <summary>
        ///     Removes the existing Transfer Shortcut
        /// </summary>
        public RelayCommand RemoveTransferShortcutCommand => new RelayCommand(RemoveTransferShortcut);

        private async void CreateSpendingShortcut()
        {
            await spendingShortcut.CreateShortCut();
            RaisePropertyChanged(nameof(IsSpendingShortcutPinned));
        }

        private async void CreateIncomeShortcut()
        {
            await incomeShortcut.CreateShortCut();
            RaisePropertyChanged(nameof(IsIncomeShortcutPinned));
        }

        private async void CreateTransferShortcut()
        {
            await transferShortcut.CreateShortCut();
            RaisePropertyChanged(nameof(IsTransferShortcutPinned));
        }

        private async void RemoveSpendingShortcut()
        {
            await spendingShortcut.RemoveShortcut();
            RaisePropertyChanged(nameof(IsSpendingShortcutPinned));
        }

        private async void RemoveIncomeShortcut()
        {
            await incomeShortcut.RemoveShortcut();
            RaisePropertyChanged(nameof(IsIncomeShortcutPinned));
        }

        private async void RemoveTransferShortcut()
        {
            await transferShortcut.RemoveShortcut();
            RaisePropertyChanged(nameof(IsTransferShortcutPinned));
        }
    }
}