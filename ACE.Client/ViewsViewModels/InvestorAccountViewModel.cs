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
    public class InvestorAccountViewModel : ViewModelBase
    {
     
        //TODO: Will be instantiated through IOC container. 
        //TODO: Only XAML should be allowed to refer to static property "Singleton" for testing reasons.
        public ClientModel Model => ClientModel.Singleton;

        public InvestorAccount EditingInvestorAccount
        {
            get { return Get<InvestorAccount>(); }
            set
            {
                if (EditingInvestorAccount != null) return; // set it just once, on the first time.
                Set(value);
                RaisePropertyChanged(nameof(Header));
                RaisePropertyChanged(nameof(EditingEntity));
            }
        }

        #region ViewModelBase
        public override string Header => $"Inv: {EditingInvestorAccount?.Key.ToString()}";
        public override BindableObject EditingEntity => this.EditingInvestorAccount;
        #endregion ViewModelBase

        #region Navigation
        public override void OnNavigatedTo(object context)
        {
            base.OnNavigatedTo(context);
            EditingInvestorAccount =  context as InvestorAccount;
        }
        #endregion Navigation
    }
}
