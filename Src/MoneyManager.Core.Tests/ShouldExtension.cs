using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoneyManager.Core.Tests
{
    public static class ShouldExtension
    {
        public static void ShouldBe<T>(this T self, T other)
        {
            Assert.AreEqual(other, self);
        }

        public static void ShouldNotBe<T>(this T self, T other)
        {
            Assert.AreNotEqual(other, self);
        }

        public static void ShouldBeSame<T>(this T self, T other)
        {
            Assert.AreSame(other, self);
        }
        public static void ShouldBeNotSame<T>(this T self, T other)
        {
            Assert.AreNotSame(other, self);
        }
    }
}
