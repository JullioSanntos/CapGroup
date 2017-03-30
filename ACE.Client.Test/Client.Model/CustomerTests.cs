using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Client.Model;
using ACE.Client.Model.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Client.Test.Client.Model
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void InstantiationTest()
        {
            var target = new Customer();
            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void ClientModelCreateMockCustomersTest()
        {
            var target = new Customer();
            var customersCache = new CustomersCache();
            customersCache.Cache = new List<Customer>();
            customersCache.Cache.Add(new Customer() {CustomerId = 1234, FirstName = "John", LastName = "Doe"});
            customersCache.Cache.Add(new Customer() {CustomerId = 5678, FirstName = "Mary", LastName = "Doe"});
            IEnumerable<Customer> actual;
            actual = Customer.FindCustomers("1234", customersCache.Cache);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count());
            actual = Customer.FindCustomers("Mar", customersCache.Cache);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count());
            actual = Customer.FindCustomers("D", customersCache.Cache);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count());
            actual = Customer.FindCustomers("Doe 1234", customersCache.Cache);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count());
            actual = Customer.FindCustomers("J M", customersCache.Cache);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count());
        }
    }
}
