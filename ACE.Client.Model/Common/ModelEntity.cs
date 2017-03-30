using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace ACE.Client.Model.Common
{
    /// <summary>
    /// EntityBase abstract class. Wraps an main entity 
    /// </summary>
    /// <typeparam name="TKey">Type of Entity's unique identifier</typeparam>
    /// <typeparam name="TEntity">Entity's type</typeparam>
    public class ModelEntity<TKey, TEntity> : BindableObject, IValid, IPersist, IModelEntity where TEntity : class
    {

        /// <summary>
        /// Gets Entity's Key
        /// </summary>
        public virtual TKey Key { get; }

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

        private ObservableCollection<IUserMessage> _userMessages;


        public ObservableCollection<IUserMessage> UserMessages
        {
            get { return _userMessages ?? (_userMessages = new ObservableCollection<IUserMessage>()); }
        }

        /// <summary>
        /// Gets Entity's Key
        /// </summary>
        object IModelEntity.Key
        {
            get
            {
                return this.Key;
            }
        }

        /// <summary>
        /// Gets Entity's instance
        /// </summary>
        object IModelEntity.Value
        {
            get { return this.Value; }
        }

        public bool IsValid => UserMessages?.Any(m => m.Severity == MessageSeverity.InputError || m.Severity == MessageSeverity.SystemError) != true;

        #region IPersist
        public event CompletedEventHandler SaveCompleted;
        public event CompletedEventHandler CancelCompleted;

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            this.ResetIsDirty();
        }

        public void Cancel()
        {
            this.Undo();
        }
        #endregion IPersist
    }
}
