using System.Diagnostics.CodeAnalysis;
using MoneyFox.ServiceLayer.Authentication;
using MoneyFox.ServiceLayer.Interfaces;
using Moq;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.Authentication
{
    [ExcludeFromCodeCoverage]
    public class PasswordStorageTests
    {
        [Theory]
        [InlineData("MyPassword")]
        [InlineData("/*-+%&")]
        [InlineData("")]
        [InlineData("789456")]
        public void SavePassword_PasswordSaved(string password)
        {
            // Arrange
            var keyPassed = string.Empty;
            var valuePassed = string.Empty;

            const string passwordKey = "password";

            var protectedDataSetup = new Mock<IProtectedData>();
            protectedDataSetup.Setup(x => x.Protect(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string key, string value) =>
                {
                    keyPassed = key;
                    valuePassed = value;
                });

            // Act
            new PasswordStorage(protectedDataSetup.Object).SavePassword(password);

            // Assert
            Assert.Equal(passwordKey, keyPassed);
            Assert.Equal(password, valuePassed);
        }

        [Fact]
        public void LoadPassword_PasswordReturned()
        {
            // Arrange
            const string passwordValue = "MyPassword";

            var protectedDataSetup = new Mock<IProtectedData>();
            protectedDataSetup.Setup(x => x.Unprotect(It.IsAny<string>()))
                .Returns(passwordValue);
            

            // Act
            var result = new PasswordStorage(protectedDataSetup.Object).LoadPassword();

            // Assert
            Assert.Equal(passwordValue, result);
        }

        [Fact]
        public void RemovePassword()
        {
            // Arrange
            var protectedDataSetup = new Mock<IProtectedData>();
            protectedDataSetup.Setup(x => x.Remove(It.IsAny<string>()));
            

            // Act
            new PasswordStorage(protectedDataSetup.Object).RemovePassword();

            // Assert
        }

        [Fact]
        public void RemovePassword_RemoveTwiceForException_ExceptionCatched()
        {
            // Arrange
            var protectedDataSetup = new Mock<IProtectedData>();
            protectedDataSetup.Setup(x => x.Remove(It.IsAny<string>()));


            // Act
            new PasswordStorage(protectedDataSetup.Object).RemovePassword();
            new PasswordStorage(protectedDataSetup.Object).RemovePassword();

            // Assert
        }

        [Theory]
        [InlineData("Password", "Password", true)]
        [InlineData("Foo", "Password", false)]
        [InlineData("12345", "12345", true)]
        [InlineData("12345", "23456", false)]
        [InlineData("/*-+´~?", "/*-+´~?", true)]
        [InlineData("/*-+´~?", "23456", false)]
        [InlineData("", "", true)]
        [InlineData("", " ", false)]
        public void ValidatePassword(string passwordToCompare, string savedPassword, bool expectedResult)
        {
            // Arrange
            var protectedDataSetup = new Mock<IProtectedData>();
            protectedDataSetup.Setup(x => x.Unprotect(It.IsAny<string>()))
                .Returns(savedPassword);

            // Act
            var result = new PasswordStorage(protectedDataSetup.Object).ValidatePassword(passwordToCompare);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
