using MoneyFox.Business.Helpers;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels
{
    public interface ISettingsShortcutsViewModel
    {
        bool ShowInfoOnMainTile { get; set; }

        /// <summary>
        ///     Creates a Expense Shortcut
        /// </summary>
        MvxCommand CreateSpendingShortcutCommand { get; }

        /// <summary>
        ///     Creates an Income Shortcut
        /// </summary>
        MvxCommand CreateIncomeShortcutCommand { get; }

        /// <summary>
        ///     Creates an Transfer Shortcut
        /// </summary>
        MvxCommand CreateTransferShortcutCommand { get; }

        /// <summary>
        ///     Indicates if there exists a spending shortcut
        /// </summary>
        bool IsSpendingShortcutPinned { get; }

        /// <summary>
        ///     Indicates if there is a income shortcut
        /// </summary>
        bool IsIncomeShortcutPinned { get; }

        /// <summary>
        ///     Indicates if there is a transfer shortcut
        /// </summary>
        bool IsTransferShortcutPinned { get; }

        /// <summary>
        ///     Removes the existing Expense Shortcut
        /// </summary>
        MvxCommand RemoveSpendingShortcutCommand { get; }

        /// <summary>
        ///     Removes the existing Income Shortcut
        /// </summary>
        MvxCommand RemoveIncomeShortcutCommand { get; }

        /// <summary>
        ///     Removes the existing Transfer Shortcut
        /// </summary>
        MvxCommand RemoveTransferShortcutCommand { get; }

        /// <summary>
        ///     Provides Access to the LocalizedResources for the current language
        /// </summary>
        LocalizedResources Resources { get; }
    }
}