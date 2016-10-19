using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Shared.Authentication;
using Moq;

namespace MoneyFox.Shared.Tests.Authentication
{
    [TestClass]
    public class PasswordStorageTests
    {
        [TestMethod]
        public void SavePassword_String_SavedPassword()
        {
            const string input = "This is a password";
            var resultPassword = string.Empty;
            var resultKey = string.Empty;

            var mockSetup = new Mock<IProtectedData>();
            mockSetup.Setup(x => x.Protect(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string key, string password) =>
                {
                    resultKey = key;
                    resultPassword = password;
                });

            new PasswordStorage(mockSetup.Object).SavePassword(input);

            Assert.AreEqual("password", resultKey);
            Assert.AreEqual(input, resultPassword);
        }

        [TestMethod]
        public void SavePassword_SpecialCharacter_SavedPassword()
        {
            const string input = "+\"*%&(()=";
            var resultPassword = string.Empty;
            var resultKey = string.Empty;

            var mockSetup = new Mock<IProtectedData>();
            mockSetup.Setup(x => x.Protect(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string key, string password) =>
                {
                    resultKey = key;
                    resultPassword = password;
                });

            new PasswordStorage(mockSetup.Object).SavePassword(input);

            Assert.AreEqual("password", resultKey);
            Assert.AreEqual(input, resultPassword);
        }

        [TestMethod]
        public void LoadPassword_ReturnSavedPassword()
        {
            const string expectedPassword = "fooo";
            var mockSetup = new Mock<IProtectedData>();
            mockSetup.Setup(x => x.Unprotect(It.Is<string>(y => y == "password"))).Returns(expectedPassword);

            Assert.AreEqual(expectedPassword, new PasswordStorage(mockSetup.Object).LoadPassword());
        }

        [TestMethod]
        public void RemovePassword_RemoveMethodWasCalled()
        {
            var called = false;
            var mockSetup = new Mock<IProtectedData>();
            mockSetup.Setup(x => x.Remove(It.Is<string>(y => y == "password"))).Callback(() => called = true);

            new PasswordStorage(mockSetup.Object).RemovePassword();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void RemovePassword_WrongKey_ExceptionCatched()
        {
            var called = false;
            var mockSetup = new Mock<IProtectedData>();
            mockSetup.Setup(x => x.Remove(It.IsAny<string>())).Callback(() =>
            {
                called = true;
                throw new COMException();
            });

            new PasswordStorage(mockSetup.Object).RemovePassword();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ValidatePassword_ValidPassword_CorrectlyValidated()
        {
            const string password = "password";

            var mockSetup = new Mock<IProtectedData>();
            mockSetup.Setup(x => x.Unprotect(It.Is<string>(y => y == password))).Returns(password);

            Assert.IsTrue(new PasswordStorage(mockSetup.Object).ValidatePassword(password));
        }

        [TestMethod]
        public void ValidatePassword_InvalidValidPassword_CorrectlyValidated()
        {
            const string password = "password";
            const string passwordPassed = "abc";

            var mockSetup = new Mock<IProtectedData>();
            mockSetup.Setup(x => x.Unprotect(It.Is<string>(y => y == password))).Returns(password);

            Assert.IsFalse(new PasswordStorage(mockSetup.Object).ValidatePassword(passwordPassed));
        }
    }
}