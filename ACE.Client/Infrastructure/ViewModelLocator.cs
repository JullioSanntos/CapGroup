using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Client.ViewsViewModels;

namespace ACE.Client.Infrastructure
{
    public class ViewModelLocator
    {
        public CustomersViewModel CustomersViewModel
        {
            get { return new CustomersViewModel(); }
        }
        public InvestorAccountViewModel InvestorAccountViewModel
        {
            get { return new InvestorAccountViewModel(); }
        }
    }
}
