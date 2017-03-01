// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityBase.cs" company="FMR LLC">
//   Copyright (C) FMR LLC.  All Rights Reserved.
//   Fidelity Confidential Information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using IndexedObservableCollectionProj;

namespace ACE.Client.Model
{
    /// <summary>
    /// EntityBase abstract class. Wraps an main entity 
    /// </summary>
    /// <typeparam name="TKey">Type of Entity's unique identifier</typeparam>
    /// <typeparam name="TEntity">Entity's type</typeparam>
    public abstract class EntityBase<TKey, TEntity> : BindableObject, IEntityBase where TEntity : class
    {

        /// <summary>
        /// Gets Entity's Key
        /// </summary>
        public abstract TKey Key { get; }

        /// <summary>
        /// Gets Entity's type
        /// </summary>
        public Type EntityType
        {
            get { return typeof(TEntity); }
        }

        /// <summary>
        /// Gets Entity's instance
        /// </summary>
        public TEntity Value
        {
            get { return this as TEntity; }
        }

        /// <summary>
        /// Gets Entity's Key
        /// </summary>
        object IEntityBase.Key
        {
            get
            {
                return this.Key;
            }
        }

        /// <summary>
        /// Gets Entity's instance
        /// </summary>
        object IEntityBase.Value
        {
            get { return this.Value; }
        }
    }
}
