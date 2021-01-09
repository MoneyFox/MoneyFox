using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.Activation
{
    internal interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }

    internal abstract class ActivationHandler<T> : IActivationHandler
                            where T : class
    {
        protected abstract Task HandleInternalAsync(T args);

        public async Task HandleAsync(object args) => await HandleInternalAsync(args as T);

        public bool CanHandle(object args) => args is T && CanHandleInternal(args as T);

        protected virtual bool CanHandleInternal(T args) => true;
    }
}
