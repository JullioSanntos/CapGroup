using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Client.Model;

namespace ACE.Client.Rules
{
    public partial class RulesEngine
    {
        public ClientModel Model { get; private set; }
        public void Initialize(ClientModel model) {
            Model = model;

            RuleActions.Add(Rule1.RuleId,Rule1);
        }
        public void PauseEngine() { }
        public void PauseRule() { }
        public void StartRule() { }

        public void InvokeRule(string ruleId)
        {
            var ruleAction = RuleActions[ruleId];
            if (ruleAction.Rule.EffDate == null && ruleAction.Rule.EffDate <= DateTime.Now)
            {
                if (ruleAction.Rule.Expression(Model) == ruleAction.BooleanOperator) ruleAction.Action.Expression(ruleAction.Arguments);
            }
        }

        #region Rules
        public static RuleActionBinding Rule1
        {
            get
            {
                //
                var rule1 = new RuleBase((Model) => Model.SelectedCustomer?.SelectedInvestorAccount?.CurrAcctVal < 0);
                var rule1 = new RuleBase((Model) => Model.SelectedCustomer?.SelectedInvestorAccount?.CurrAcctVal < 0);
                var act1 = new UserMessageAction(UserMessage.NoNegativeValue);
                var ruleAction1 = new RuleActionBinding(rule1, true, act1, "Rule1");
                return ruleAction1;
            }
        }

        private Dictionary<string, RuleActionBinding> _ruleActions;
        public Dictionary<string, RuleActionBinding> RuleActions
        {
            get { return _ruleActions ?? (_ruleActions = new Dictionary<string, RuleActionBinding>()); }
        }
        #endregion Rules
    }
}
