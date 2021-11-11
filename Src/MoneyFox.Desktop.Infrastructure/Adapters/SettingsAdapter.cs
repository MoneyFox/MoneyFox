using MoneyFox.Application.Common.Adapters;
using NLog;
using System;

namespace MoneyFox.Desktop.Infrastructure.Adapters
{
    public class SettingsAdapter : ISettingsAdapter
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public bool GetValue(string key, bool defaultValue) => throw new NotImplementedException();

        public string GetValue(string key, string defaultValue) => throw new NotImplementedException();

        public int GetValue(string key, int defaultValue) => throw new NotImplementedException();

        public void AddOrUpdate(string key, bool value) => throw new NotImplementedException();

        public void AddOrUpdate(string key, string value) => throw new NotImplementedException();

        public void AddOrUpdate(string key, int value) => throw new NotImplementedException();

        public void Remove(string key) => throw new NotImplementedException();
    }
}
