using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Ios
{
    public  class RoamingSettings : IRoamingSettings
    {
        public void AddOrUpdateValue(string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public TValueType GetValueOrDefault<TValueType>(string key, TValueType defaultValue)
        {
            throw new System.NotImplementedException();
        }
    }
}