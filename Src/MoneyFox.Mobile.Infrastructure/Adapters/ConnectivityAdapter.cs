using MoneyFox.Application.Common.Adapters;
using NLog;
using Xamarin.Essentials;

namespace MoneyFox.Mobile.Infrastructure.Adapters
{
    /// <inheritdoc />
    public class ConnectivityAdapter : IConnectivityAdapter
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public bool IsConnected
        {
            get
            {
                try
                {
                    return Connectivity.NetworkAccess == NetworkAccess.Internet;
                }
                catch(PermissionException ex)
                {
                    logger.Error(ex, "Permission denied on check for connection");
                    return false;
                }
            }
        }
    }
}