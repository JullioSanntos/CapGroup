using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Model
{
    public class ClientModel : BindableObject
    {
        #region Singleton
        private static ClientModel _singleton;

        public static ClientModel Singleton => _singleton ?? (_singleton = new ClientModel());

        private ClientModel() { }
        #endregion

        #region Search
        public string CustomerSearchingValues
        {
            get { return Get<string>(); }
            set
            {
                Set<string>(value);
                RaisePropertyChanged(nameof(Customers));
            }
        }

        private IndexedObservableCollection<string, Customer> _customers;
        public IndexedObservableCollection<string, Customer> Customers
        {
            get
            {
                if (_customers == null) _customers = CustomersCache;
                return _customers;
            }

        }

        #region mocks
        private IndexedObservableCollection<string, Customer> _customersCache;
        private IndexedObservableCollection<string, Customer> CustomersCache
        {
            get { return _customersCache ?? (_customersCache = CreateMockCustomers(5)); }
        }

        public IndexedObservableCollection<string, Customer> CreateMockCustomers(int numberOfCustomers)
        {
            var randomizer = new Randomizer();
            var customers = new IndexedObservableCollection<string, Customer>();
            var syllableRandomizer = new Syllable();
            for (long i = 0; i < numberOfCustomers; i++)
            {
                var cust = new Customer() {CustomerId = long.Parse(randomizer.Next(Randomizer.NumericValues, 6)) };
                cust.TaxId = randomizer.Next(Randomizer.NumericValues, 9);
                cust.PhoneNumber = randomizer.Next(Randomizer.NumericValues, 10);
                cust.FirstName = randomizer.Next(syllableRandomizer, 2, 4);
                cust.LastName = randomizer.Next(syllableRandomizer, 2, 4);
                customers.AddOrUpdate(cust);
            }
            return customers;
        }
        #endregion

        #endregion

    }
}
