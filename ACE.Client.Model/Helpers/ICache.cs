using System.Collections.Generic;

namespace ACE.Client.Model.Helpers
{
    public interface ICache<T>
    {
        List<T> Cache { get; set; }
        List<T> LoadCache(object loadArguments);
    }
}
