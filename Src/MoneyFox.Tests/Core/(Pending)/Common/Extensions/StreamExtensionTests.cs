namespace MoneyFox.Tests.Core._Pending_.Common.Extensions
{

    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using MoneyFox.Core._Pending_.Common.Extensions;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class StreamExtensionTests
    {
        [Fact]
        public void ReadToEnd()
        {
            // Arrange
            byte[] bytes =
            {
                12,
                22,
                25,
                23
            };

            var stream = new MemoryStream(bytes);

            // Act
            var resultBytes = stream.ReadToEnd();
            stream.Close();

            // Assert
            Assert.Equal(expected: bytes, actual: resultBytes);
        }
    }

}
