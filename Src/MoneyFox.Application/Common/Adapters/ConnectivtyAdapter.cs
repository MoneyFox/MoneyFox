using Microsoft.AppCenter.Crashes;
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
                    Crashes.TrackError(ex);

                    return false;
                }
            }
        }
    }
}
