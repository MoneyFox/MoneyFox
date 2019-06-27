using MoneyFox.Foundation;
using Should;
using Xunit;

namespace MoneyFox.DataLayer.Tests.Helper
{
    public class DatabasePathHelperTests
    {
        [Theory]
        [InlineData(AppPlatform.Android, @"Documents\moneyfox3.db")]
        [InlineData(AppPlatform.iOS, @"\..\Library\moneyfox3.db")]
        [InlineData(AppPlatform.UWP, "moneyfox3.db")]
        public void GetDbPath_Platform_CorrectPath(AppPlatform platform, string expectedPathSegment)
        {
            // Arrange
            ExecutingPlatform.Current = platform;

            // Act
            var result = DatabasePathHelper.GetDbPath();

            // Assert
            result.ShouldContain(expectedPathSegment);
        }
    }
}
