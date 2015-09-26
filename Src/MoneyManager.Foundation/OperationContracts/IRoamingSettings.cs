namespace MoneyManager.Foundation.OperationContracts
{
    public interface IRoamingSettings
    {
        void AddOrUpdateValue(string key, object value);
        TValueType GetValueOrDefault<TValueType>(string key, TValueType defaultValue);
    }
}