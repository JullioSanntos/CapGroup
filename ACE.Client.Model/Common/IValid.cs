using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Model.Common
{
    public interface IValid
    {
        ObservableCollection<IUserMessage> UserMessages { get; }

        bool IsValid { get; }
    }
}
