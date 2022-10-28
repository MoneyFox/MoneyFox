namespace MoneyFox.Core.Common.Helpers;

using System;

public class SystemDateHelper : ISystemDateHelper
{
    public DateTime Today => DateTime.Today;
    public DateTime Now => DateTime.Now;
}
