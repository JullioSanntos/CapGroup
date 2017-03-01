using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Model
{
    public class Customer : EntityBase<string, Customer>
    {
        public override string Key => CustomerId.ToString().PadLeft(6,'0');


        public string FirstName
        {
            get { return Get<string>(); }
            set { Set(value); RaisePropertyChanged(nameof(FullName));}
        }


        public string LastName
        {
            get { return Get<string>(); }
            set { Set(value); RaisePropertyChanged(nameof(FullName)); }
        }


        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public long CustomerId
        {
            get { return Get<long>(); }
            set { Set(value); }
        }

        public string PhoneNumber
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


        public string TaxId
        {
            get { return Get<string>(); }
            set { Set(value); RaisePropertyChanged(nameof(TaxIdFmt)); }
        }

        public string TaxIdFmt => $"{TaxId.Substring(0, 3)}-{TaxId.Substring(3, 2)}-{TaxId.Substring(5, 4)}";

    }
}
