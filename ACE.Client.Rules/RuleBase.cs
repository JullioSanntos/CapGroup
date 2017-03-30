using ACE.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Rules
{
    public class RuleBase 
    {
        public Func<ClientModel, bool> Expression { get; private set; }
        public RuleBase(Func<ClientModel, bool> booleanExpression) 
        {
            this.Expression = booleanExpression;
        }

        public DateTime EffDate { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
