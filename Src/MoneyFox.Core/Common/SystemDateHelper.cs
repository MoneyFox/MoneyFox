namespace MoneyFox.Core.Common;

using System;

public interface ISystemDateHelper
{
    DateTime Today { get; }
    DateTime Now { get; }
}

public class SystemDateHelper : ISystemDateHelper
{
    public DateTime Today => DateTime.Today;
    public DateTime Now => DateTime.Now;
}
