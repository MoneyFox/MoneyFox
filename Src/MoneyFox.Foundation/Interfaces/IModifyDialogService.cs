using System.Threading.Tasks;

namespace MoneyFox.Foundation.Interfaces
{
    /// <summary>
    ///     Shows a Dialog to select the operation on edit.
    ///     On Android this is used as context menu.
    /// </summary>
    public interface IModifyDialogService
    {

        /// <summary>
        ///     Creates a Dialog to select between edit and delete
        /// </summary>
        Task<ModifyOperation> ShowEditSelectionDialog();
    }
}
