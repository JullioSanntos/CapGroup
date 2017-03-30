using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NavigationInstancesAttribute : Attribute
    {
        public int MaximumInstances { get; private set; }

        public NavigationInstancesAttribute(int maximunInstances)
        {
            this.MaximumInstances = maximunInstances;
        }
    }
}
