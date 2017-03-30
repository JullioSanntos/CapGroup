using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ACE.Client.Model.Common
{
    public abstract class BindableObject : INotifyPropertyChanged, INotifyDirtyData
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected IDictionary<string, PropertyLog> membersActivities = new Dictionary<string, PropertyLog>();

        public bool IsDirty {
            get {
                var result = membersActivities.Values.Any(a => a.IsDirty == true);
                return result;
            }
            set { }
        }
            

        /// <summary>
        /// Gets the value for <paramref name="propertyName"/>, or the default for the type, if this is the first retrieval.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="propertyName"/></typeparam>
        /// <param name="propertyName">The name of the <see cref="INotifyPropertyChanged"/> property to retrieve the backing value for</param>
        /// <returns>The value for <paramref name="propertyName"/></returns>
        protected T Get<T>(T defaultValue = default(T), [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            { throw new ArgumentException("propertyName"); }

            PropertyLog existingLog;
            if (membersActivities.TryGetValue(propertyName, out existingLog))
            {
                return (T)(existingLog.PropertyValue); //this was a member variable we knew about
            }
            else
            {
                //this wasn't a member variable we knew about
                //store the default for subsequent retrievals, and then return it  
                (existingLog = new PropertyLog(propertyName)).PropertyValues.Push(defaultValue);
                membersActivities.Add(propertyName, existingLog);
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the value represented by <paramref name="propertyExpr"/>, or the default, if this is the first retrieval.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="propertyExpr"/></typeparam>
        /// <param name="propertyExpr">The expression representing the name of the <see cref="INotifyPropertyChanged"/> property to retrieve the backing value for.</param>
        /// <returns>the value for <paramref name="propertyExpr"/></returns>
        protected T Get<T>(Expression<Func<T>> propertyExpr, T defaultValue = default(T))
        {
            var expression = (MemberExpression)propertyExpr.Body;
            string propertyName = expression.Member.Name;

            return Get<T>(defaultValue, propertyName);
        }

        /// <summary>
        /// Sets the storage to the new value, and optionally raises <see cref="PropertyChanged"/>
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="newValue">The new value</param>
        /// <param name="raiseINPC">Raise <seealso cref="PropertyChanged"/> in the event the value is actually modified</param>
        /// <param name="propertyExpression">An expression which represents the name of the <see cref="INotifyPropertyChanged"/> property being changed</param>
        /// <returns>The updated value</returns>
        protected virtual T Set<T>(T newValue, bool raiseINPC, bool setIsDirty, Expression<Func<T>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyName = expression.Member.Name;

            return Set(newValue, raiseINPC, setIsDirty, propertyName);
        }

        /// <summary>
        /// Sets the storage to the new value and optionally raises <seealso cref="PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="newValue">The new value</param>        
        /// <param name="propertyName">The name of the <see cref="INotifyPropertyChanged"/> property being changed</param>
        /// <param name="raiseINPC">Raise <seealso cref="PropertyChanged"/> in the event the value is actually modified</param>
        /// <returns>The updated value</returns>
        protected virtual T Set<T>(T newValue, bool raiseINPC = true, bool setIsDirty = true, [CallerMemberName] string propertyName = null)
        {
            PropertyLog propertyLog;
            if (membersActivities.TryGetValue(propertyName, out propertyLog) == false)
            {
                propertyLog = new PropertyLog(propertyName);
                propertyLog.PropertyValues.Push(default(T));
                membersActivities.Add(propertyName, propertyLog);
            }

            var valueUpdated = false;

            if (Equals(propertyLog.PropertyValue, newValue) == false)
            {
                var propLog = membersActivities[propertyName];
                propLog.PropertyValues.Push(newValue);
                valueUpdated = true;
                if (!setIsDirty) ResetIsDirty(propLog);
            }
            //Check if the membervariables does not have this key, then add value with the existingvalue.
            //This is required for properties where the value is set as default value, it should be added to the membervaribles.
            else if (!membersActivities.ContainsKey(propertyName))
            {
                membersActivities.Add(propertyName, propertyLog);
            }

            if (valueUpdated && raiseINPC)
            {
                RaisePropertyChanged(propertyName);
            }

            if (setIsDirty) RaisePropertyChanged(nameof(IsDirty));

            return newValue;
        }

        /// <summary>
        /// Explicitly raises the <seealso cref="PropertyChanged"/> event
        /// </summary>
        /// <typeparam name="T">The type of property raising the <seealso cref="PropertyChanged"/> event</typeparam>
        /// <param name="propertyExpr">The expression representing the <see cref="INotifyPropertyChanged"/> property</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpr)
        {
            var expression = (MemberExpression)propertyExpr.Body;
            string propertyName = expression.Member.Name;

            RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Explicitly raises the <seealso cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName">The name of the <see cref="INotifyPropertyChanged"/> property that changed</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handlers = PropertyChanged;
            if (handlers != null)
            {
                handlers(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void CleanUpMemberVariables(IDictionary<string, object> memberVariables)
        {
            foreach (var kvp in memberVariables)
            {
                RecursivelyCleanup(kvp.Value);
            }

            memberVariables.Clear();
            memberVariables = null;
        }

        private void RecursivelyCleanup(object parent)
        {
            if (parent is IDisposable)
            {
                (parent as IDisposable).Dispose();
                return;
            }

            var enumerableParent = parent as IEnumerable;
            if (enumerableParent == null)
            { return; }

            foreach (var child in enumerableParent)
            {
                RecursivelyCleanup(child);
            }

            if (parent is ICollection<object> && !(parent as ICollection<object>).IsReadOnly)
            {
                (parent as ICollection<object>).Clear();
            }
        }

        public void Undo()
        {
            membersActivities.ToList().ForEach(pl => Undo(pl.Value) );
            RaisePropertyChanged(nameof(IsDirty));
        }

        /// Reverts back to the initial value
        public void Undo(string propertyName)
        {
            if (membersActivities?.Any(pl => pl.Key == propertyName) != true) return;
            var propLog = membersActivities[propertyName];
            Undo(propLog);
            RaisePropertyChanged(propertyName);
            RaisePropertyChanged(nameof(IsDirty));
        }

        /// <summary>
        /// Reverts back to the initial value
        /// </summary>
        private void Undo(PropertyLog propLog)
        {
            if (propLog.PropertyValues.Count > 1)
            {
                var initialValue = propLog.InitialValue;
                propLog.PropertyValues.Clear();
                propLog.PropertyValues.Push(initialValue);
                RaisePropertyChanged(propLog.PropertyName);
            }
        }

        public bool GetIsDirty(string propertyName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clear all pending changes by adopting the latest value as the original one.
        /// </summary>
        public void ResetIsDirty()
        {
            membersActivities.Values.ToList().ForEach(pl => ResetIsDirty(pl));
            RaisePropertyChanged(nameof(IsDirty));
        }

        /// <summary>
        /// Clear all pending changes by adopting the latest value as the original one.
        /// </summary>
        public void ResetIsDirty(string propertyName)
        {
            var propLog = membersActivities[propertyName];
            ResetIsDirty(propLog);
            RaisePropertyChanged(nameof(IsDirty));
        }

        private void ResetIsDirty(PropertyLog propertyLog)
        {
            //save the latest Value
            var currValue = propertyLog.PropertyValue;
            //clear the stack
            propertyLog.PropertyValues.Clear();
            //save the latest value as initial
            propertyLog.PropertyValues.Push(currValue);
        }

        protected class PropertyLog
        {
            public PropertyLog(string propertyName)
            {
                this.PropertyName = propertyName;
            }

            public string PropertyName { get; set; }

            private Stack<object> _propertyValue;
            public Stack<object> PropertyValues { get { return _propertyValue ?? (_propertyValue = new Stack<object>()); } }
            public object PropertyValue { get { return PropertyValues.Peek(); } }
            public object InitialValue { get { return PropertyValues.ToArray()[PropertyValues.Count() - 1]; } }
            public bool IsDirty { get { return PropertyValues.Count > 1; } }
        }
    }
}

