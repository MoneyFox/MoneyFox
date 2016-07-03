using System;
using NUnit.Framework;
using Xunit;
using Assert = NUnit.Framework.Assert;


namespace MoneyFox.Droid.Tests {
    [TestFixture]
    public class TestsSample {

        [SetUp]
        public void Setup() { }


        [TearDown]
        public void Tear() { }

        [Fact]
        public void Pass() {
            Console.WriteLine("test1");
            Assert.True(true);
        }

        [Fact]
        public void Fail() {
            Assert.False(true);
        }

        [Test]
        [Ignore("another time")]
        public void Ignore() {
            Assert.True(false);
        }

        [Test]
        public void Inconclusive() {
            Assert.Inconclusive("Inconclusive");
        }
    }
}