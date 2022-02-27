namespace MoneyFox.Core.Tests._Pending_.Common.Extensions
{
    using Core._Pending_.Common.Extensions;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class StreamExtensionTests
    {
        [Fact]
        public void ReadToEnd()
        {
            // Arrange
            byte[] bytes = {12, 22, 25, 23};
            var stream = new MemoryStream(bytes);

            // Act
            byte[] resultBytes = stream.ReadToEnd();
            stream.Close();

            // Assert
            Assert.Equal(bytes, resultBytes);
        }
    }
}