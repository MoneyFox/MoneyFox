using System.Diagnostics.CodeAnalysis;
using MoneyFox.Application;
using MoneyFox.DataLayer;
using Should;
using Xunit;

namespace MoneyFox.Persistence.Tests.Helper
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
            string result = DatabasePathHelper.GetDbPath();

            // Assert
            result.ShouldContain(expectedPathSegment);
        }
    }
}
