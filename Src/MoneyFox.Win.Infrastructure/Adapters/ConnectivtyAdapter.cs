namespace MoneyFox.Win.Infrastructure.Adapters;

using Core.Interfaces;
using JetBrains.Annotations;
using System.Net.NetworkInformation;

/// <inheritdoc />
[UsedImplicitly]
public class ConnectivityAdapter : IConnectivityAdapter
{
    /// <inheritdoc />
    public bool IsConnected => NetworkInterface.GetIsNetworkAvailable();
}