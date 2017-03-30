using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Model.Common
{
    public interface IUserMessage
    {
        string Message { get; }
        MessageSeverity Severity { get; }
    }

    public enum MessageSeverity
    {
        Informational,
        DiscloseToUser,
        AttentionRequired,
        InputError,
        SystemError
    }
}
