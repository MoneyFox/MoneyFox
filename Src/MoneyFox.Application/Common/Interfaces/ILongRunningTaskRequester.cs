namespace MoneyFox.Application.Common.Interfaces
{
    public interface ILongRunningTaskRequester
    {
        int RequestLongRunning();

        void EndLongRunning(int taskId);
    }
}
