using System;
using ACE.Client.Model.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Client.Test.Client.Model.Extensions
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void IsAlphaTest()
        {
            Assert.IsTrue("abC".IsAlpha());
            Assert.IsFalse("a2C".IsAlpha());
        }

        [TestMethod]
        public void IsIntegerTest()
        {
            Assert.IsTrue("123".IsInteger());
            Assert.IsFalse("2aC".IsInteger());
            Assert.IsFalse("abC".IsInteger());
        }
    }
}
