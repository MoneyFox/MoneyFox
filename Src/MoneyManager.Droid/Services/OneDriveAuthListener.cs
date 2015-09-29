using System.Threading.Tasks;
using Java.Lang;
using Microsoft.Live;
using Exception = System.Exception;

namespace MoneyManager.Droid.Services
{
    public class OneDriveAuthListener : Object, ILiveAuthListener
    {
        private readonly TaskCompletionSource<LiveConnectSession> source =
            new TaskCompletionSource<LiveConnectSession>();

        public Task<LiveConnectSession> Task => source.Task;

        public void OnAuthComplete(LiveStatus status, LiveConnectSession session, Object userState)
        {
            try
            {
                source.SetResult(session);
            }
            catch (Exception ex)
            {
                source.TrySetException(ex);
            }
        }

        public void OnAuthError(LiveAuthException exception, Object userState)
        {
            source.TrySetException(exception);
        }
    }
}