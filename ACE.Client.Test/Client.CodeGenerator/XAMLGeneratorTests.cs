using System;
using ACE.Client.Model;
using ACE.Client.Model.CodeGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Client.Test.Client.CodeGenerator
{
    [TestClass]
    public class XAMLGeneratorTests
    {
        [TestMethod]
        public void XAMLGeneratorBasicRunTest()
        {
            var target = new XAMLGenerator();
            var model = new ClientModel();
            target.Generate(model);
            Assert.IsTrue(true);
        }
    }
}
