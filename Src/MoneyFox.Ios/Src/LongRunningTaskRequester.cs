using MoneyFox.Application.Common.Interfaces;
using System;
using UIKit;

namespace MoneyFox.iOS.Src
{
    public class LongRunningTaskRequester : ILongRunningTaskRequester
    {
        public int RequestLongRunning() => Convert.ToInt32(UIApplication.SharedApplication.BeginBackgroundTask(() => { }));

        public void EndLongRunning(int taskId) => UIApplication.SharedApplication.EndBackgroundTask(taskId);
    }
}