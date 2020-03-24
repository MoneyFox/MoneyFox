using MoneyFox.Application.Common.Interfaces;
using NLog;
using System;
using UIKit;

namespace MoneyFox.iOS.Src
{
    public class LongRunningTaskRequester : ILongRunningTaskRequester
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public int RequestLongRunning()
        {
            var taskId = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            logger.Info("Long Running Task for id {0} requested.", taskId);
            return Convert.ToInt32(taskId);
        }

        public void EndLongRunning(int taskId)
        {
            UIApplication.SharedApplication.EndBackgroundTask(taskId);
            logger.Info("Long Running Task with id {0} completed.", taskId);
        }
    }
}