using System;
using System.Diagnostics.CodeAnalysis;
using Should;
using Xunit;

namespace MoneyFox.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class OperationResultTests
    {
        [Fact]
        public void Succeeded_SuccessTrue()
        {
            // Arrange
            // Act
            var result = OperationResult.Succeeded();

            // Assert
            result.Success.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("This is a message")]
        public void Failed_String_SuccessFalse(string message)
        {
            // Arrange
            // Act
            var result = OperationResult.Failed(message);

            // Assert
            result.Success.ShouldBeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData("This is a message")]
        public void Failed_String_MessageSet(string message)
        {
            // Arrange
            // Act
            var result = OperationResult.Failed(message);

            // Assert
            result.Message.ShouldEqual(message);
        }

        [Fact]
        public void Failed_Exception_SuccessFalse()
        {
            // Arrange
            // Act
            var result = OperationResult.Failed(new Exception());

            // Assert
            result.Success.ShouldBeFalse();
        }

        [Fact]
        public void Failed_Exception_MessageSet()
        {
            // Arrange
            // Act
            var result = OperationResult.Failed(new Exception());

            // Assert
            result.Message.ShouldEqual(new Exception().ToString());
        }

        [Fact]
        public void Failed_ExceptionNull_SuccessFalse()
        {
            // Arrange
            // Act
            var result = OperationResult.Failed();

            // Assert
            result.Success.ShouldBeFalse();
        }

        [Fact]
        public void Failed_ExceptionNull_MessageSet()
        {
            // Arrange
            // Act
            var result = OperationResult.Failed();

            // Assert
            result.Message.ShouldBeEmpty();
        }

    }
}