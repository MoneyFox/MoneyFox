using MoneyFox.Foundation;
using Should;
using Xunit;

namespace MoneyFox.DataLayer.Tests.Helper
{
    public class DatabasePathHelperTests
    {
        [Theory]
        [InlineData(AppPlatform.Android, @"C:\Users\padruttn\Documents\moneyfox3.db")]
        [InlineData(AppPlatform.iOS, @"C:\Users\padruttn\Documents\..\Library\moneyfox3.db")]
        [InlineData(AppPlatform.UWP, "moneyfox3.db")]
        public void GetDbPath_Platform_CorrectPath(AppPlatform platform, string expectedResult)
        {
            // Arrange
            ExecutingPlatform.Current = platform;

            // Act
            var result = DatabasePathHelper.GetDbPath();

            // Assert
            result.ShouldEqual(expectedResult);
        }
    }
}
