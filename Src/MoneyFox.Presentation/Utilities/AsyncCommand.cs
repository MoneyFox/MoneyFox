using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyFox.Presentation.Utilities
{
    public class AsyncCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool isExecuting;
        private readonly Func<object, Task> execute;
        private readonly Func<bool> canExecute;

        public AsyncCommand(
            Func<Task> execute,
            Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute()
        {
            return !isExecuting && (canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute())
            {
                try
                {
                    isExecuting = true;
                    await execute(parameter);
                } finally
                {
                    isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
    }
}
