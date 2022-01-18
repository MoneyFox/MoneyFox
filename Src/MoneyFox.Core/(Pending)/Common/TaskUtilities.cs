using NLog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MoneyFox.Core._Pending_.Common
{
    public static class TaskUtilities
    {
        private static readonly Logger Logger = LogManager.GetLogger("TaskLogger");

        [SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "Wanted")]
        public static async void FireAndForgetSafeAsync(this Task task)
        {
            try
            {
                await task;
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}