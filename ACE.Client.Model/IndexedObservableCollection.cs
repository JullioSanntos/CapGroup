// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexedObservableCollection.cs" company="FMR LLC">
//   Copyright (C) FMR LLC.  All Rights Reserved.
//   Fidelity Confidential Information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ACE.Client.Model
{
    /// <summary>
    /// <see cref="T:IndexedObservableCollection" /> provides faster indexed access to items in the collection 
    /// and implements INotifyCollectionChanged. WIP. />
    /// </summary>
    /// <typeparam name="TKey">The key's type of the element to add</typeparam>
    /// <typeparam name="TValue">The type of the elemenent being stored</typeparam>
    public class IndexedObservableCollection<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged where TValue : class
    {
        #region fields
        /// <summary>
        /// Exception message
        /// </summary>
        private const string KeyWasNotFound = "key was not found.";
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedObservableCollection&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        public IndexedObservableCollection()
        {
            this.InternalCollection = new ObservableCollection<TValue>();
            this.InternalCollection.CollectionChanged += this.InternalCollectionChanged;
            this.ObservableValueCollection = new ReadOnlyObservableCollection<TValue>(this.InternalCollection);
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when ObservableValueCollection changes
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion

        #region Properties
        #region public properties
        /// <summary>
        /// Gets or sets ObservableValueCollection which provides the bindable collection for this type
        /// </summary>
        public ReadOnlyObservableCollection<TValue> ObservableValueCollection { get; set; }

        #endregion

        #region protected, private, internal properties
        /// <summary>
        /// Gets or sets InternalCollection which provides the main storage content for this type
        /// </summary>
        protected ObservableCollection<TValue> InternalCollection { get; set; }
        #endregion
        #endregion

        #region methods
        #region public methods
        /// <summary>Inserts an element into the <see cref="T:IndexedObservableCollection" /> at the specified index.</summary>
        /// <param name="key">The object to be used for faster access to the item</param>
        /// <param name="value">The object to insert. The value can be null for reference types.</param>
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);

            this.InternalCollection.Add(value);
        }

        /// <summary>
        /// Adds an Entity to this collection
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        public void Add(EntityBase<TKey, TValue> entity)
        {
            this.Add(entity.Key, entity.Value);
        }

        /// <summary>Inserts an element into the <see cref="T:IndexedObservableCollection" /> at the specified index.</summary>
        /// <param name="key">The object to be used for faster access to the item</param>
        /// <param name="value">The object to insert. The value can be null for reference types.</param>
        public void Update(TKey key, TValue value)
        {
            if (!this.ContainsKey(key))
            {
                throw new ArgumentException(key.ToString(), KeyWasNotFound);
            }

            var previousValue = base[key];
            var observCollIndex = this.InternalCollection.IndexOf(previousValue);
            this.InternalCollection[observCollIndex] = value;
            base[key] = value;
        }

        /// <summary>
        /// Remove value associated to key
        /// </summary>
        /// <param name="key">key associated to the object to be removed. An exception will be thrown if key is not found</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false</returns>
        public bool TryRemove(TKey key)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }

            var value = this[key];
            var itemIndex = this.InternalCollection.IndexOf(value);
            if (itemIndex < 0)
            {
                return false;
            }

            this.Remove(key, itemIndex);
            return true;
        }

        /// <summary>
        /// Remove value associated to key
        /// </summary>
        /// <param name="entity">Enity to be removed. An exception will be thrown if key is not found</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false</returns>
        public bool TryRemove(EntityBase<TKey, TValue> entity)
        {
            return this.TryRemove(entity.Key);
        }

        /// <summary>
        /// Remove object associated with key
        /// </summary>
        /// <param name="key">object's index key</param>
        public new void Remove(TKey key)
        {
            var value = this[key];
            var itemIndex = this.InternalCollection.IndexOf(value);
            this.Remove(key, itemIndex);
        }

        /// <summary>
        /// Remove value associated to key
        /// </summary>
        /// <param name="key">key associated to the object to be Added or Updated</param>
        /// <param name="value">value object to be Added or Updated</param>
        /// <returns>true if the element is added; otherwise, false</returns>
        public bool AddOrUpdate(TKey key, TValue value)
        {
            bool valueAdded = false;
            if (this.ContainsKey(key))
            {
                this.Update(key, value);
            }
            else
            {
                this.Add(key, value);
                valueAdded = true;
            }

            return valueAdded;
        }

        /// <summary>
        /// Remove value associated to key
        /// </summary>
        /// <param name="entity">key associated to the object to be Added or Updated</param>
        /// <returns>true if the element is added; otherwise, false</returns>
        public bool AddOrUpdate(EntityBase<TKey, TValue> entity)
        {
            return this.AddOrUpdate(entity.Key, entity.Value);
        }

        /// <summary>
        /// Resets this collection to zero items
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            this.InternalCollection.Clear();
        }

        /// <summary>
        /// Returns enumerating list
        /// </summary>
        /// <returns>values' enumerator</returns>
        public new IEnumerator<TValue> GetEnumerator()
        {
            return this.ObservableValueCollection.GetEnumerator();
        }
        #endregion

        #region protected, private and internal methods

        /// <summary>
        /// Remove object from both base Dictionary and internal Observal collection
        /// </summary>
        /// <param name="key">key associated to the object to be removed in base Dictionay</param>
        /// <param name="itemIndex">index location associated with object to be removed from internal ObservableCollection</param>
        protected void Remove(TKey key, int itemIndex)
        {
            base.Remove(key);
            this.InternalCollection.RemoveAt(itemIndex);
        }

        /// <summary>
        /// re-thrown collection changed event
        /// </summary>
        /// <param name="sender">this object</param>
        /// <param name="e">collection changed arguments</param>
        private void InternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, e);
            }
        }
        #endregion
        #endregion
    }
}
