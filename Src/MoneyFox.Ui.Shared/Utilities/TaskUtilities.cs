using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NLog;

namespace MoneyFox.Presentation.Utilities
{
    public static class TaskUtilities
    {
        private static readonly Logger Logger = LogManager.GetLogger("TaskLogger");

        [SuppressMessage("Minor Code Smell", "S4261:Methods should be named according to their synchronicities", Justification = "Wanted")]
        [SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "Wanted")]
        [SuppressMessage("Minor Code Smell", "S2221:\"Exception\" should not be caught when not required by called methods",
                         Justification = "Catch all exception")]
        public static async void FireAndForgetSafeAsync(this Task task)
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
