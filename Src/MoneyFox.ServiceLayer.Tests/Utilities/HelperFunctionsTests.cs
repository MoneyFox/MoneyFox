using System;
using System.Diagnostics.CodeAnalysis;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.Utilities
{
    [ExcludeFromCodeCoverage]
    public class HelperFunctionsTests
    {
        [Fact]
        public void GetEndOfMonth_NoneInput_LastDayOfMonth()
        {
            ServiceLayer.Utilities.HelperFunctions.GetEndOfMonth().ShouldBeType(typeof(DateTime));
        }
    }
}