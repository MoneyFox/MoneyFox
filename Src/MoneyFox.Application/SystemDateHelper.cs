using System;

namespace MoneyFox.Application
{
    public class SystemDateHelper : ISystemDateHelper
    {
        public DateTime Today => DateTime.Today;
    }
}
