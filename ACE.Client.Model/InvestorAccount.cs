using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ACE.Client.Model.Common;
using ACE.Client.Model.Helpers;
using ACE.Client.Common;

namespace ACE.Client.Model
{
    public class InvestorAccount : ModelEntity<long, InvestorAccount>
    {

        private void Customer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FullName)) RaisePropertyChanged(nameof(FullName));
        }

        public override long Key => Number;

        [DataTemplate(group:GenConstants.GroupDetail, order:1, type: GenConstants.TypeColumn)]
        public long Number
        {
            get { return Get<long>(); }
            set { Set(value); }
        }

        [DataTemplate(group: GenConstants.GroupDetail, order:3, type:GenConstants.TypeColumn)]
        public decimal CurrAcctVal
        {
            get { return Get<decimal>(); }
            set { Set(value); }
        }

        public Customer Customer
        {
            get { return Get<Customer>(); }
            set {
                Set(value, true, false);
                if (this.Customer != null) this.Customer.PropertyChanged += Customer_PropertyChanged;
            }
        }

        [DataTemplate(group: GenConstants.GroupDetail, order: 2, type: GenConstants.TypeColumn)]
        public string FullName => Customer.FullName;


        #region Mocks

        private static ICache<InvestorAccount> _investorsAccountCache;
        internal static ICache<InvestorAccount> InvestorsAccountCache
        {
            get { return _investorsAccountCache ?? (_investorsAccountCache = new InvestorsAccountCache(0)); }
            set { _investorsAccountCache = value; }
        }

        internal static void SetupCache(int numberOfMocks)
        {
            var singleCache = new InvestorsAccountCache(numberOfMocks);
            InvestorsAccountCache = singleCache;
        }
        #endregion Mocks
    }
}
