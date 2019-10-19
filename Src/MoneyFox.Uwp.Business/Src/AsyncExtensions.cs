using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MoneyFox.Uwp.Business
{
    public static class AsyncExtensions
    {
        public static void Await(this IAsyncAction operation)
        {
            try
            {
                Task task = operation.AsTask();
                task.Wait();
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }

        public static TResult Await<TResult>(this IAsyncOperation<TResult> operation)
        {
            try
            {
                Task<TResult> task = operation.AsTask();
                task.Wait();

                return task.Result;
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }
    }
}
