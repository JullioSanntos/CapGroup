using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =true)]
    public class DataTemplateAttribute : Attribute
    {
        public DataTemplateAttribute(string group, int order, string type = null)
        {
            this.Group = group;
            this.Order = Order;
            this.Type = type;
        }
        public string Group { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
    }
}
