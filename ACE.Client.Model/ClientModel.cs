using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ACE.Client.Model.Common;

namespace ACE.Client.Model
{
    public class ClientModel : BindableObject
    {
        #region Singleton
        private static ClientModel _singleton;

        public  static ClientModel Singleton { 
            get { return _singleton ?? (_singleton = new ClientModel()); }
            private set { _singleton = value; }
        }

        public ClientModel() { Singleton = this; }

        #endregion

        #region Search
        public string CustomerSearchingValues
        {
            get { return Get<string>(); }
            set
            {
                Set<string>(value);
                //MatchingCustomers = Customer.FindCustomers(value, Customer.CustomersCache.Cache).ToList();
                MatchingCustomers = Customer.FindCustomers(value).ToList();
            }
        }

        public List<Customer> MatchingCustomers
        {
            get { return Get<List<Customer>>(); }
            set { Set(value); }
        }

        public Customer SelectedCustomer
        {
            get { return Get<Customer>(); }
            set { Set(value); }
        }

        //private IndexedObservableCollection<string, Customer> _customers;
        //public IndexedObservableCollection<string, Customer> Customers
        //{
        //    get
        //    {
        //        if (_customers == null)
        //        {
        //            _customers = new IndexedObservableCollection<string, Customer>();
        //            Customer.CustomersCache.Cache.ForEach(c => _customers.AddOrUpdate(c));
        //        }
        //        return _customers;
        //    }
        //}

        #endregion

    }
}
