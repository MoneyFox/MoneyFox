using System;
using Xamarin.Essentials;

namespace MoneyFox.Business.Adapter
{
    public interface IConnectivtyAdapter
    {
        bool IsConnected { get; }
    }

    public class ConnectivtyAdapter : IConnectivtyAdapter
    {
        public bool IsConnected
        {
            get
            {
                try
                {
                    return Connectivity.NetworkAccess == NetworkAccess.Internet;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
