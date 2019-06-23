using System;
using System.Threading.Tasks;
using NLog;

namespace MoneyFox.Presentation.Utilities
{
    public static class TaskUtilities
    {
        private static Logger logger = LogManager.GetLogger("TaskLogger");

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public static async void FireAndForgetSafeAsync(this Task task)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}