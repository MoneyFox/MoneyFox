using Should;
using Xunit;

namespace MoneyFox.Persistence.Tests
{
    public class ContextAdapterTests
    {
        [Fact]
        public void InitializedCorrectly()
        {
            // Arrange
            // Act
            var adapter = new ContextAdapter();

            // Assert
            adapter.Context.ShouldNotBeNull();
        }
    }
}
