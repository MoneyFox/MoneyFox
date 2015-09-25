using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Ios
{
    public class AppInformation : IAppInformation
    {
        public string GetVersion { get; }
    }
}