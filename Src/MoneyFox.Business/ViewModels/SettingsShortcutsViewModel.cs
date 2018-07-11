using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;

// ReSharper disable ExplicitCallerInfoArgument

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Provides the information for the TileSettingsView
    /// </summary>
    public class SettingsShortcutsViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;
        private readonly ITileManager tileManager;

        /// <summary>
        ///     Creates a SettingsShortcutsViewModel object
        /// </summary>
        public SettingsShortcutsViewModel(ISettingsManager settingsManager, ITileManager tileManager)
        {
            this.settingsManager = settingsManager;
            this.tileManager = tileManager;
        }

        public bool ShowInfoOnMainTile
        {
            get => settingsManager.ShowCashFlowOnMainTile;
            set
            {
                settingsManager.ShowCashFlowOnMainTile = value;
                RaisePropertyChanged();
            }
        }

        public string Title => Strings.TileTitle;
        public string Prompt => Strings.TileSettingPrompt;
        /// <summary>
        ///     Creates a Expense Shortcut
        /// </summary>
        public MvxCommand CreateSpendingShortcutCommand => new MvxCommand(CreateSpendingShortcut);

        /// <summary>
        ///     Creates an Income Shortcut
        /// </summary>
        public MvxCommand CreateIncomeShortcutCommand => new MvxCommand(CreateIncomeShortcut);

        /// <summary>
        ///     Creates an Transfer Shortcut
        /// </summary>
        public MvxCommand CreateTransferShortcutCommand => new MvxCommand(CreateTransferShortcut);

        /// <summary>
        ///     Indicates if there exists a spending shortcut
        /// </summary>
        public bool IsSpendingShortcutPinned => tileManager.Exists(TyleType.Expense);

        /// <summary>
        ///     Indicates if there is a income shortcut
        /// </summary>
        public bool IsIncomeShortcutPinned => tileManager.Exists(TyleType.Income);

        /// <summary>
        ///     Indicates if there is a transfer shortcut
        /// </summary>
        public bool IsTransferShortcutPinned => tileManager.Exists(TyleType.Transfer);

        /// <summary>
        ///     Removes the existing Expense Shortcut
        /// </summary>
        public MvxCommand RemoveSpendingShortcutCommand => new MvxCommand(RemoveSpendingShortcut);

        /// <summary>
        ///     Removes the existing Income Shortcut
        /// </summary>
        public MvxCommand RemoveIncomeShortcutCommand => new MvxCommand(RemoveIncomeShortcut);

        /// <summary>
        ///     Removes the existing Transfer Shortcut
        /// </summary>
        public MvxCommand RemoveTransferShortcutCommand => new MvxCommand(RemoveTransferShortcut);

        private void CreateSpendingShortcut()
        {
            tileManager.CreateTile(TyleType.Expense);
            RaisePropertyChanged(nameof(IsSpendingShortcutPinned));
        }

        private void CreateIncomeShortcut()
        {
            tileManager.CreateTile(TyleType.Income);
            RaisePropertyChanged(nameof(IsIncomeShortcutPinned));
        }

        private void CreateTransferShortcut()
        {
            tileManager.CreateTile(TyleType.Transfer);
            RaisePropertyChanged(nameof(IsTransferShortcutPinned));
        }

        private void RemoveSpendingShortcut()
        {
            tileManager.RemoveTile(TyleType.Expense);
            RaisePropertyChanged(nameof(IsSpendingShortcutPinned));
        }

        private void RemoveIncomeShortcut()
        {
            tileManager.RemoveTile(TyleType.Income);
            RaisePropertyChanged(nameof(IsIncomeShortcutPinned));
        }

        private void RemoveTransferShortcut()
        {
            tileManager.RemoveTile(TyleType.Transfer);
            RaisePropertyChanged(nameof(IsTransferShortcutPinned));
        }
    }
}