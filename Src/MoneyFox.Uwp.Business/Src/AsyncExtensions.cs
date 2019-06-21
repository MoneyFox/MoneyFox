using System;
using Windows.Foundation;

namespace MoneyFox.Uwp.Business
{
    public static class AsyncExtensions
    {
        public static void Await(this IAsyncAction operation)
        {
            try
            {
                var task = operation.AsTask();
                task.Wait();
            } catch (AggregateException exception)
            {
                // TODO - this possibly oversimplifies the problem report
                throw exception.InnerException;
            }
        }

        public static TResult Await<TResult>(this IAsyncOperation<TResult> operation)
        {
            try
            {
                var task = operation.AsTask();
                task.Wait();
                return task.Result;
            } catch (AggregateException exception)
            {
                // TODO - this possibly oversimplifies the problem report
                throw exception.InnerException;
            }
        }
    }
}
