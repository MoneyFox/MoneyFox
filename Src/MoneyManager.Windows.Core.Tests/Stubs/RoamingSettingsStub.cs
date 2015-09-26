using System;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Windows.Core.Tests.Stubs
{
    public class RoamingSettingsStub : IRoamingSettings
    {
        public void AddOrUpdateValue(string key, object value)
        {
            throw new NotImplementedException();
        }

        public TValueType GetValueOrDefault<TValueType>(string key, TValueType defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}