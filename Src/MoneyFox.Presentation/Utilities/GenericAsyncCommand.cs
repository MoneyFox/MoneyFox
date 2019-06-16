using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyFox.Presentation.Utilities
{
    public class AsyncCommand<T> : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool isExecuting;
        private readonly Func<T, Task> execute;
        private readonly Func<bool> canExecute;

        public AsyncCommand(
            Func<T, Task> execute,
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
                    await execute((T)parameter);
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
            var val = parameter;

            if (parameter != null
                && parameter.GetType() != typeof(T))
            {
                if (parameter is IConvertible)
                {
                    val = Convert.ChangeType(parameter, typeof(T), null);
                }
            }

            if (CanExecute())
            {
                if (val == null)
                {

                    if (typeof(T).IsValueType)
                    {
                        await ExecuteAsync(default(T));
                    } else
                    {
                        // ReSharper disable ExpressionIsAlwaysNull
                        await ExecuteAsync((T)val);
                        // ReSharper restore ExpressionIsAlwaysNull
                    }
                } else
                {
                    await ExecuteAsync((T)val);
                }
            }
        }
    }
}
