using System;
using System.Threading.Tasks;
using NLog;

namespace MoneyFox.Presentation.Utilities
{
    public static class TaskUtilities
    {
        private static readonly Logger Logger = LogManager.GetLogger("TaskLogger");

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
#pragma warning disable S3168 // "async" methods should not return "void"
        public static async void FireAndForgetSafe(this Task task)
#pragma warning restore S3168 // "async" methods should not return "void"
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
