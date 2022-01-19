using FluentAssertions;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Helpers;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Core.Tests._Pending_.Common.Helper
{
    [ExcludeFromCodeCoverage]
    public class DatabasePathHelperTests
    {
        [Theory]
        [InlineData(AppPlatform.Android, @"moneyfox3.db")]
        [InlineData(AppPlatform.iOS, @"Library")]
        [InlineData(AppPlatform.UWP, "moneyfox3.db")]
        public void GetDbPath_Platform_CorrectPath(AppPlatform platform, string expectedPathSegment)
        {
            // Arrange
            ExecutingPlatform.Current = platform;

            // Act
            string result = DatabasePathHelper.DbPath;

            // Assert
            result.Should().Contain(expectedPathSegment);
        }
    }
}