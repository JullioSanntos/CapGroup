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

        [TestMethod]
        public void ClientModelCreateMockCustomersTest()
        {
            var target = ClientModel.Singleton;
            var actualMockCustomers = target.CreateMockCustomers(10);
            Assert.IsInstanceOfType(actualMockCustomers, typeof(IndexedObservableCollection<string, Customer>));
            Assert.AreEqual(10, actualMockCustomers.Count);
        }


        [TestMethod]
        public void ClientModelCustomersCacheTest()
        {
            var target = ClientModel.Singleton;
            var actualMockCustomers = target.Customers;
            Assert.IsInstanceOfType(actualMockCustomers, typeof(IndexedObservableCollection<string, Customer>));
            Assert.IsTrue(actualMockCustomers.Count > 10000);
        }
    }
}
