using System;
using System.Collections.Generic;
using Xunit;

namespace TestFoundation
{
    public static class ShouldExtension
    {
        public static void ShouldBe<T>(this T self, T other)
        {
            Assert.Equal(other, self);
        }

        public static void ShouldNotBe<T>(this T self, T other)
        {
            Assert.NotEqual(other, self);
        }

        public static void ShouldBeNull(this object self)
        {
            Assert.Null(self);
        }

        public static void ShouldNotBeNull(this object self)
        {
            Assertno(self);
        }

        public static void ShouldBeSameAs(this object self, object other)
        {
            Assert.AreSame(other, self);
        }

        public static void ShouldNotBeSameAs(this object self, object other)
        {
            Assert.AreSame(other, self);
        }

        public static void ShouldBeTrue(this bool self)
        {
            Assert.IsTrue(self);
        }

        public static void ShouldBeTrue(this bool self, string message)
        {
            Assert.IsTrue(self, message);
        }

        public static void ShouldBeFalse(this bool self)
        {
            Assert.IsFalse(self);
        }

        public static void ShouldBeFalse(this bool self, string message)
        {
            Assert.IsFalse(self, message);
        }

        public static void ShouldBeGreaterThan<T>(this T self, T other)
            where T : IComparable<T>
        {
            Assert.IsTrue(self.CompareTo(other) > 0);
        }

        public static void ShouldBeGreaterThan<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) > 0);
        }

        public static void ShouldBeGreaterThanOrEqualTo<T>(this T self, T other)
            where T : IComparable<T>
        {
            Assert.IsTrue(self.CompareTo(other) >= 0);
        }

        public static void ShouldBeGreaterThanOrEqualTo<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) >= 0);
        }

        public static void ShouldBeLessThan<T>(this T self, T other)
            where T : IComparable<T>
        {
            Assert.IsTrue(self.CompareTo(other) < 0);
        }

        public static void ShouldBeLessThan<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) < 0);
        }

        public static void ShouldBeLessThanOrEqualTo<T>(this T self, T other)
            where T : IComparable<T>
        {
            Assert.IsTrue(self.CompareTo(other) <= 0);
        }

        public static void ShouldBeLessThanOrEqualTo<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) <= 0);
        }

        public static void ShouldBeInstanceOf(this object self, Type type)
        {
            Assert.IsInstanceOfType(self, type);
        }

        public static void ShouldNotBeInstanceOf(this object self, Type type)
        {
            Assert.IsNotInstanceOfType(self, type);
        }
    }
}