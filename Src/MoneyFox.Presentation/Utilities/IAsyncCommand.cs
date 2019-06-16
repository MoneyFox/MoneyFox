using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyFox.Presentation.Utilities
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
        bool CanExecute();
    }
}
