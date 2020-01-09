using Microsoft.AppCenter.Crashes;
using NLog;
using Xamarin.Essentials;

namespace MoneyFox.Application.Common.Adapters
{
    /// <summary>
    ///     Provides access to the connectivity state.
    /// </summary>
    public interface IConnectivityAdapter
    {
        /// <summary>
        ///     returns if the device is connected to the internet.
        /// </summary>
        bool IsConnected { get; }
    }

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
                catch (PermissionException ex)
                {
                    logger.Error(ex, "Permission denied on check for connection");
                    return false;
                }
            }
        }
    }
}
