using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MoneyFox.Uwp.Src
{
    [SuppressMessage("Blocker Code Smell", "S4462:Calls to \"async\" methods should not be blocking", Justification = "<Pending>")]
    public static class AsyncExtensions
    {
        public static void Await(this IAsyncAction operation)
        {
            try
            {
                Task task = operation.AsTask();
                task.Wait();
            }
            catch(AggregateException exception)
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
            catch(AggregateException exception)
            {
                throw exception.InnerException;
            }
        }
    }
}
