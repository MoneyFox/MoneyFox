using Beezy.MvvmCross.Plugins.SecureStorage;
using MoneyManager.Core.Authentication;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Authentication
{
    public class PasswordStorageTests
    {
        [Theory]
        [InlineData("This is a password")]
        [InlineData("+\"*%&(()=")]
        public void SavePassword_StringPassword_SavedPassword(string passwordString)
        {
            var resultPassword = string.Empty;
            var resultKey = string.Empty;

            var mockSetup = new Mock<IMvxProtectedData>();
            mockSetup.Setup(x => x.Protect(It.IsAny<string>(), It.IsAny<string>())).Callback((string key, string password) =>
            {
                resultKey = key;
                resultPassword = password;
            });

            new PasswordStorage(mockSetup.Object).SavePassword(passwordString);

            resultKey.ShouldBe("password");
            resultPassword.ShouldBe(passwordString);
        }

        [Fact]
        public void LoadPassword_ReturnSavedPassword()
        {
            const string expectedPassword = "fooo";
            var mockSetup = new Mock<IMvxProtectedData>();
            mockSetup.Setup(x => x.Unprotect(It.Is<string>(y => y == "password"))).Returns(expectedPassword);

            new PasswordStorage(mockSetup.Object).LoadPassword().ShouldBe(expectedPassword);
        }

        [Fact]
        public void RemovePassword_RemoveMethodWasCalled()
        {
            var called = false;
            var mockSetup = new Mock<IMvxProtectedData>();
            mockSetup.Setup(x => x.Remove(It.Is<string>(y => y == "password"))).Callback(() => called = true);

            new PasswordStorage(mockSetup.Object).RemovePassword();

            called.ShouldBeTrue();
        }

        [Theory]
        [InlineData("fooo", "fooo", true)]
        [InlineData("fooo", "not the same", false)]
        public void ValidatePassword_PassPasswordString_CorrectlyValidated(string password, string stringToCheck, bool result)
        {
            var mockSetup = new Mock<IMvxProtectedData>();
            mockSetup.Setup(x => x.Unprotect(It.Is<string>(y => y == "password"))).Returns(password);

            new PasswordStorage(mockSetup.Object).ValidatePassword(stringToCheck).ShouldBe(result);
        }
    }
}