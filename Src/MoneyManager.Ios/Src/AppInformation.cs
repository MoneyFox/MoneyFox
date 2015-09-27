using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Ios
{
    public class AppInformation : IAppInformation
    {
        public string GetVersion { get; }
    }
}