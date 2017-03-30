using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ACE.Client.Common;
using ACE.Client.Model.Common;
using ACE.Client.Model.Extensions;
using ACE.Client.Model.Helpers;

[assembly: InternalsVisibleTo("ACE.Client.Test")]

namespace ACE.Client.Model
{
    public class Customer : ModelEntity<string, Customer>
    {
        public Customer()
        {
            this.PropertyChanged += Customer_PropertyChanged;
        }

        private void Customer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FirstName) || (e.PropertyName == nameof(LastName))) RaisePropertyChanged(nameof(FullName));
            if (e.PropertyName == nameof(TaxId) ) RaisePropertyChanged(nameof(TaxIdFmt));
        }

        public override string Key => CustomerId.ToString().PadLeft(6,'0');

        [DataTemplate(group:GenConstants.GroupDetail, order:1, Type = GenConstants.TypePanel)]
        public string FirstName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [DataTemplate(group: GenConstants.GroupDetail, order: 2, Type = GenConstants.TypePanel)]
        public string LastName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [DataTemplate(group: GenConstants.GroupSummary, order: 1, Type = GenConstants.TypePanel)]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public long CustomerId
        {
            get { return Get<long>(); }
            set { Set(value); }
        }

        private IndexedObservableCollection<long, InvestorAccount> _investorAccounts;

        public IndexedObservableCollection<long, InvestorAccount> InvestorAccounts
        {
            get { return _investorAccounts ?? (_investorAccounts = new IndexedObservableCollection<long, InvestorAccount>()); }
            set { _investorAccounts = value; }
        }


        public InvestorAccount SelectedInvestorAccount
        {
            get { return Get<InvestorAccount>(); }
            set { Set(value, true, false);  }
        }

        [DataTemplate(group: GenConstants.GroupSummary, order: 2, Type = GenConstants.TypePanel)]
        public string PhoneNumber
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


        public string TaxId
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


        [DataTemplate(group: GenConstants.GroupDetail, order: 3, Type = GenConstants.TypePanel)]
        //[DataTemplate(group: GenConstants.GroupSummary, order: 3, Type = GenConstants.TypePanel)]
        public string TaxIdFmt => $"{TaxId.Substring(0, 3)}-{TaxId.Substring(3, 2)}-{TaxId.Substring(5, 4)}";


        #region Services

        public static IEnumerable<Customer> FindCustomers(string searchingTokens)
        {
            var result = FindCustomers(searchingTokens, CustomersCache.Cache);
            return result;
        } 

        public static IEnumerable<Customer>  FindCustomers(string searchingTokens, IEnumerable<Customer> customers )
        {
            var tokens = searchingTokens.Split(' ').Where(t => !string.IsNullOrEmpty(t)).Select(t => t.ToUpper()).ToList();
            var numberTokens = tokens.Where(t => t.IsInteger()).ToList();
            var alphaTokens = tokens.Where(t => t.IsAlpha()).ToList();

            var result = customers;
            if (alphaTokens.Any())
                result = result.Where(c => alphaTokens.Any(t => c.FirstName.ToUpper().StartsWith(t) || c.LastName.ToUpper().StartsWith(t)));
            if (numberTokens.Any())
                result = result.Where(c => numberTokens.Any(t => c.CustomerId.ToString().StartsWith(t)));

            return result;
        }
        #endregion

        #region mocks

        private const int _numberOfMocks = 10000;
        private static ICache<Customer> _customersCache;
        internal static ICache<Customer> CustomersCache {
            get { return _customersCache ?? (_customersCache = new CustomersCache(_numberOfMocks)); }
            set { _customersCache = value; }
        }

        internal static void SetupCache(int numberOfMocks)
        {
            CustomersCache = new CustomersCache(numberOfMocks);
        }
        #endregion
    }
}
