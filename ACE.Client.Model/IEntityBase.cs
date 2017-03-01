// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityBase.cs" company="FMR LLC">
//   Copyright (C) FMR LLC.  All Rights Reserved.
//   Fidelity Confidential Information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace IndexedObservableCollectionProj
{
    /// <summary>
    /// IEntityBase interface
    /// </summary>
    public interface IEntityBase
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
