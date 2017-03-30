using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using ACE.Client.Infrastructure;
using ACE.Client.Model;
using ACE.Client.Model.Common;

namespace ACE.Client.ViewsViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        private void ReactToCustomerChanges(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
            if (e.PropertyName == nameof(Customer.FullName)) RaisePropertyChanged(nameof(Header));
            if (e.PropertyName == nameof(Customer.SelectedInvestorAccount)) NavigationServices.NavigateTo(nameof(InvestorAccountView), EditingCustomer.SelectedInvestorAccount);
        }

        //TODO: Will be instantiated through IOC container. 
        //TODO: Only XAML should be allowed to refer to static property "Singleton" for testing reasons.
        public ClientModel Model => ClientModel.Singleton;

        public Customer EditingCustomer
        {
            get { return Get<Customer>(); }
            private set
            {
                if (EditingCustomer != null) return; // Set this property only once
                Set(value);
                if (this.EditingCustomer != null) this.EditingCustomer.PropertyChanged += ReactToCustomerChanges;
                RaisePropertyChanged(nameof(Header));
                RaisePropertyChanged(nameof(EditingEntity));
            }
        }
        #region ViewModelBase
        public override string Header => $"Cust: {EditingCustomer?.FullName}";
        public override BindableObject EditingEntity => this.EditingCustomer;
        #endregion ViewModelBase


        #region Navigation
        public override void OnNavigatedTo(object context)
        {
            this.EditingCustomer = Model.SelectedCustomer;
        }
        #endregion
    }
}
