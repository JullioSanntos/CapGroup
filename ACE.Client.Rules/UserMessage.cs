using ACE.Client.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Rules
{
    public class UserMessage : IUserMessage
    {
        public static UserMessage InvalidValue => new UserMessage("Invalid value");
        public static UserMessage NoNegativeValue => new UserMessage("Value can not be negative");

        public MessageSeverity Severity { get; private set; }
        public string Message { get; private set; }
        public UserMessage(string message, MessageSeverity severity = MessageSeverity.AttentionRequired)
        {
            this.Message = message;
            this.Severity = severity;
        }
    }
}
