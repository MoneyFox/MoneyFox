using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Droid.Src
{
    public class LongRunningTaskRequester : ILongRunningTaskRequester
    {
        public void EndLongRunning(int taskId)
        {
            // Do nothing on Android
        }

        public int RequestLongRunning() => 0;
    }
}