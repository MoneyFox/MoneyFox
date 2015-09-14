using System.Threading.Tasks;

namespace MoneyManager.Foundation.OperationContracts.Shotcuts
{
    public interface IShortcut
    {
        bool IsShortcutExisting { get; }

        Task CreateShortCut();
        Task RemoveShortcut();
    }
}
