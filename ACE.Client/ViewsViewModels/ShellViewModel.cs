using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Client.Infrastructure;
using ACE.Client.Model;
using ACE.Client.Model.Common;

namespace ACE.Client.ViewsViewModels
{
    public class ShellViewModel : BindableObject
    {
        public ClientModel Model {get {return ClientModel.Singleton;} }

        public ShellViewModel()
        {
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedCustomer") NavigationServices.NavigateTo(nameof(CustomersView), Model.SelectedCustomer);
        }

    }
}
