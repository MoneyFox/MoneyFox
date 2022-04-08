namespace MoneyFox.Win.Infrastructure.Adapters;

using System.Net.NetworkInformation;
using Core.Interfaces;
using JetBrains.Annotations;

/// <inheritdoc />
[UsedImplicitly]
public class ConnectivityAdapter : IConnectivityAdapter
{
    /// <inheritdoc />
    public bool IsConnected => NetworkInterface.GetIsNetworkAvailable();
}
