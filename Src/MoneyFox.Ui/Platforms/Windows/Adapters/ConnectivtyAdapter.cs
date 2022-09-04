namespace MoneyFox.Ui.Platforms.Windows.Adapters;

using System.Net.NetworkInformation;
using JetBrains.Annotations;
using MoneyFox.Core.Interfaces;

/// <inheritdoc />
[UsedImplicitly]
public class ConnectivityAdapter : IConnectivityAdapter
{
    /// <inheritdoc />
    public bool IsConnected => NetworkInterface.GetIsNetworkAvailable();
}
