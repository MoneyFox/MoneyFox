using MoneyFox.Application.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyFox.Uwp.Commands
{
    public sealed class AsyncCommand : IAsyncCommand
    {
        private readonly Func<bool>? canExecute;
        private readonly Func<Task> execute;

        private bool isExecuting;

        public AsyncCommand(
            Func<Task> execute,
            Func<bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute() => !isExecuting && (canExecute?.Invoke() ?? true);

        public async Task ExecuteAsync()
        {
            if(CanExecute())
            {
                try
                {
                    isExecuting = true;
                    await execute();
                }
                finally
                {
                    isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }


        bool ICommand.CanExecute(object parameter) => CanExecute();

        void ICommand.Execute(object parameter) => ExecuteAsync().FireAndForgetSafeAsync();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public sealed class AsyncCommand<T> : IAsyncCommand<T>
    {
        private readonly Func<T, bool>? canExecute;
        private readonly Func<T, Task> execute;

        private bool isExecuting;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(T parameter) => !isExecuting && (canExecute?.Invoke(parameter) ?? true);

        public async Task ExecuteAsync(T parameter)
        {
            if(CanExecute(parameter))
            {
                try
                {
                    isExecuting = true;
                    await execute(parameter);
                }
                finally
                {
                    isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }


        bool ICommand.CanExecute(object parameter) => CanExecute((T)parameter);

        void ICommand.Execute(object parameter) => ExecuteAsync((T)parameter).FireAndForgetSafeAsync();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}