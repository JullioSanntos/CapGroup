using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Rules
{
    public class RuleActionBinding
    {
        public string RuleId { get; set; }
        public RuleBase Rule { get; set; }
        public bool? BooleanOperator {get; set;}
        public ActionBase Action { get; set; }
        public object[] Arguments { get; set; }

        public RuleActionBinding() { }
        public RuleActionBinding(RuleBase rule, bool? booleanOperator, ActionBase action, string ruleId = null)
        {
            this.Rule = rule;
            this.BooleanOperator = booleanOperator;
            this.Action = action;
            this.RuleId = ruleId ?? this.GetHashCode().ToString();
        }
    }
}
