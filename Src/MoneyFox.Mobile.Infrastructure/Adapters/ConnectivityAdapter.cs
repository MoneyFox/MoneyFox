namespace MoneyFox.Mobile.Infrastructure.Adapters
{

    using Core.Interfaces;
    using Serilog;
    using Xamarin.Essentials;

    public class ConnectivityAdapter : IConnectivityAdapter
    {
        public bool IsConnected
        {
            get
            {
                try
                {
                    return Connectivity.NetworkAccess == NetworkAccess.Internet;
                }
                catch (PermissionException ex)
                {
                    Log.Error(exception: ex, messageTemplate: "Permission denied on check for connection");

                    return false;
                }
            }
        }
    }

}
