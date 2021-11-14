using JetBrains.Annotations;
using MoneyFox.Application.Common.Adapters;
using System.Net.NetworkInformation;

namespace MoneyFox.Desktop.Infrastructure.Adapters
{
    /// <inheritdoc />
    [UsedImplicitly]
    public class ConnectivityAdapter : IConnectivityAdapter
    {
        /// <inheritdoc />
        public bool IsConnected => NetworkInterface.GetIsNetworkAvailable();
    }
}