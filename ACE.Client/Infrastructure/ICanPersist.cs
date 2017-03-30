using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Infrastructure
{
    /// <summary>
    /// Apply to ViewModels that can persist the Entity that it handles
    /// </summary>
    public interface ICanPersist
    {
        bool? CanSave { get; }
        bool? CanCancel { get; }

        DelegateCommand SaveCommand { get; }
        DelegateCommand CancelCommand { get; }

        void Save();

        void Cancel();
    }
}
