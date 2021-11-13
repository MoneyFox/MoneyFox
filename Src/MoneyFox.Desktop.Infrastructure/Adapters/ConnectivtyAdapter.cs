using MoneyFox.Application.Common.Adapters;
using NLog;
using System.Net.NetworkInformation;

namespace MoneyFox.Desktop.Infrastructure.Adapters
{
    /// <inheritdoc />
    public class ConnectivityAdapter : IConnectivityAdapter
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public bool IsConnected => NetworkInterface.GetIsNetworkAvailable();
    }
}