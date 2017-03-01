using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Client.Model;

namespace ACE.Client.ViewsViewModels
{
    public class ShellViewModel : BindableObject
    {
        public ClientModel Model => ClientModel.Singleton;
    }
}
