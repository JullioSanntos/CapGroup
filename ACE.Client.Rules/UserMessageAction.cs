using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Client.Model;
using ACE.Client.Model.Common;

namespace ACE.Client.Rules
{
    public class UserMessageAction : ActionBase
    {
        public UserMessage originalMessage { get; private set; }
        public UserMessage FinalMessage { get; private set; }
        public UserMessageAction(UserMessage userMessage) 
        {
            originalMessage = userMessage;
        }
        public void FormatMessage(object[] messageArgs = null)
        {
            if (messageArgs == null) FinalMessage = originalMessage;
            var msg = string.Format(originalMessage.Message, messageArgs);
            FinalMessage = new UserMessage(msg, originalMessage.Severity);
            base.Expression = (args) => (ClientModel.Singleton.SelectedCustomer as IValid).UserMessages.Add(FinalMessage);
        }
    }
}
