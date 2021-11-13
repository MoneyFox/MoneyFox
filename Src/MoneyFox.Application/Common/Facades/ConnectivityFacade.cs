namespace MoneyFox.Application.Common.Facades
{
    public interface IConnectivityFacade
    {
        /// <summary>
        /// returns if the device is connected to the internet.
        /// </summary>
        bool IsConnected { get; }
    }

    public class ConnectivityFacade : IConnectivityFacade
    {
        private readonly IConnectivityFacade connectivityFacade;

        public ConnectivityFacade(IConnectivityFacade connectivityFacade)
        {
            this.connectivityFacade = connectivityFacade;
        }

        public bool IsConnected => connectivityFacade.IsConnected;
    }
}
