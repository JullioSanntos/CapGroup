using System;
using System.Windows.Navigation;
using ACE.Client.Model.Common;

namespace ACE.Client.Infrastructure
{
    public abstract class BindableNavigationalObject : BindableObject, INavigation
    {
        public string ViewId { get; set; }
        public abstract string Header { get; }
        public virtual void OnNavigatedFrom(object context) { }
        public virtual void OnNavigatedTo(object context) { }
    }
}
