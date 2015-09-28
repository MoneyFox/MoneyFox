using System.Threading.Tasks;
using Android.App;
using Java.Util;
using Microsoft.Live;

namespace MoneyManager.Droid.Services
{
    public class OneDriveClient
    {
        private const string CLIENTID = "XXXXXXX";
        private readonly string[] requestedScopes = {"wl.signin", "wl.skydrive_update", "wl.offline_access"};
        private readonly Activity activity;

        public OneDriveClient(Activity activity)
        {
            this.activity = activity;
        }

        public async Task<LiveConnectSession> LogonAsync()
        {
            var scopes = new ArrayList();
            scopes.AddAll(requestedScopes);

            var client = new LiveAuthClient(activity, CLIENTID);
            var loginAuthListener = new OneDriveAuthListener();
            client.Login(activity, scopes, loginAuthListener);
            return await loginAuthListener.Task;
        }
    }
}