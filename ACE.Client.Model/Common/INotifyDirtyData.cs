using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Model.Common
{
    public interface INotifyDirtyData
    {
        bool IsDirty { get; set; }
        bool GetIsDirty(string propertyName);
    }
}
