using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;

namespace ACE.Client.Model.Common
{
    public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);
    public interface IPersist
    {
        bool IsDirty {get;}
        void Load();
        void Save();
        void Cancel();

        event CompletedEventHandler SaveCompleted;
        event CompletedEventHandler CancelCompleted;
    }
}
