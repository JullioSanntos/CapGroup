using System;
using System.Collections.Generic;

namespace ACE.Client.Model.Helpers
{
    public class CustomersCache : ICache<Customer>
    {
        private readonly int _numberOfMockCustomer;
        public CustomersCache(int numberOfMockCustomer = 10000)
        {
            this._numberOfMockCustomer = numberOfMockCustomer;
        }

        private List<Customer> _cache;
        public List<Customer> Cache
        {
            get
            {
                var result = _cache ?? (_cache = LoadCache(_numberOfMockCustomer));
                return result;
            }
            set { _cache = value; }
        }

        public List<Customer> LoadCache(object numberOfCustomers)
        {
            InvestorAccount.SetupCache(3 * _numberOfMockCustomer);
            var randomizer = new Randomizer();
            var numberOfAccounts = new Random();
            var customers = new List<Customer>();
            var investorAccountIx = 0;
            var syllableRandomizer = new Syllable();
            for (long i = 0; i < (int)numberOfCustomers; i++)
            {
                var cust = new Customer() { CustomerId = long.Parse(randomizer.Next(Randomizer.NumericValues, 6)) };
                cust.TaxId = randomizer.Next(Randomizer.NumericValues, 9);
                cust.PhoneNumber = randomizer.Next(Randomizer.NumericValues, 10);
                cust.FirstName = randomizer.Next(syllableRandomizer, 2, 4);
                cust.LastName = randomizer.Next(syllableRandomizer, 2, 4);
                for (int j = 0; j < numberOfAccounts.Next(2, 4); j++)
                {
                    if (investorAccountIx > InvestorAccount.InvestorsAccountCache.Cache.Count) break;
                    var invAcct = InvestorAccount.InvestorsAccountCache.Cache[investorAccountIx];
                    invAcct.Customer = cust;
                    cust.InvestorAccounts.AddOrUpdate(invAcct);
                    investorAccountIx++;
                }
                cust.ResetIsDirty();
                customers.Add(cust);
            }
            return customers;
        }
    }
}
