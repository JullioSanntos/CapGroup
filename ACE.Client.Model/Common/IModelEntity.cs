
namespace ACE.Client.Model.Common
{
    /// <summary>
    /// IEntityBase interface
    /// </summary>
    public interface IModelEntity
    {
        /// <summary>
        /// Gets IEntityBase's Key
        /// </summary>
        object Key { get; }

        /// <summary>
        /// Gets IEntityBase's Value
        /// </summary>
        object Value { get; }
    }
}
