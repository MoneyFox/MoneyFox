using System.Threading.Tasks;

namespace MoneyManager.Foundation.Interfaces.Shotcuts
{
    /// <summary>
    ///     Defines a interface to handling Shortcut
    /// </summary>
    public interface IShortcut
    {
        /// <summary>
        ///     Indicates if the shortcut exists
        /// </summary>
        bool IsShortcutExisting { get; }

        /// <summary>
        ///     Creates the Shortcut.
        /// </summary>
        /// <returns>Task to make the method awaitable.</returns>
        Task CreateShortCut();

        /// <summary>
        ///     Removes an existing Shortcut.
        /// </summary>
        /// <returns>Task to make the method awaitable.</returns>
        Task RemoveShortcut();
    }
}