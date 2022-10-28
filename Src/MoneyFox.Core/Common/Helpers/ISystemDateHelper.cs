namespace MoneyFox.Core.Common.Helpers;

using System;

public interface ISystemDateHelper
{
    DateTime Today { get; }
    DateTime Now { get; }
}
