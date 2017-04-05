using System.IO;
using MoneyFox.Business.Extensions;
using Xunit;

namespace MoneyFox.Business.Tests.Extensions
{
    public class StreamExtensionTests
    {
        [Fact]
        public void ReadToEnd()
        {
            // Arrange
            var bytes = new byte[] {12, 22, 25, 23};
            var stream = new MemoryStream(bytes);

            // Act
            var resultBytes = stream.ReadToEnd();
            stream.Close();

            // Assert
            Assert.Equal(bytes, resultBytes);
        }
    }
}
