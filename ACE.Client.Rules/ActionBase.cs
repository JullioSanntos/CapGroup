using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Rules
{
    public class ActionBase
    {
        public Action<object[]> Expression { get; set; }
        public ActionBase() { }
        public ActionBase(Action<object[]> actionExpression)
        {
            this.Expression = actionExpression;
        }
    }
}
