using System;
using ACE.Client.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Client.Test.Client.Model
{
    [TestClass]
    public class ClientModelTests
    {
        [TestMethod]
        public void InstantiationTest()
        {
            var target = ClientModel.Singleton;
            Assert.IsNotNull(target);
        }
    }
}
