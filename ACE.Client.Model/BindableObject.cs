using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ACE.Client.Model
{
    public abstract class BindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected IDictionary<string, object> memberVariables = new Dictionary<string, object>();

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

            object existingValue;
            if (memberVariables.TryGetValue(propertyName, out existingValue))
            {
                return (T)existingValue; //this was a member variable we knew about
            }
            else
            {
                //this wasn't a member variable we knew about
                //store the default for subsequent retrievals, and then return it                
                memberVariables[propertyName] = defaultValue;
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
        protected virtual T Set<T>(T newValue, bool raiseINPC, Expression<Func<T>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyName = expression.Member.Name;

            return Set(newValue, raiseINPC, propertyName);
        }

        /// <summary>
        /// Sets the storage to the new value and optionally raises <seealso cref="PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="newValue">The new value</param>        
        /// <param name="propertyName">The name of the <see cref="INotifyPropertyChanged"/> property being changed</param>
        /// <param name="raiseINPC">Raise <seealso cref="PropertyChanged"/> in the event the value is actually modified</param>
        /// <returns>The updated value</returns>
        protected virtual T Set<T>(T newValue, bool raiseINPC = true, [CallerMemberName] string propertyName = null)
        {
            object existingValue;
            if (memberVariables.TryGetValue(propertyName, out existingValue) == false)
            {
                existingValue = default(T);
            }

            var valueUpdated = false;

            if (Equals(existingValue, newValue) == false)
            {
                memberVariables[propertyName] = newValue;
                valueUpdated = true;
            }
            //Check if the membervariables does not have this key, then add value with the existingvalue.
            //This is required for properties where the value is set as default value, it should be added to the membervaribles.
            else if (!memberVariables.ContainsKey(propertyName))
            {
                memberVariables[propertyName] = existingValue;
            }

            if (valueUpdated && raiseINPC)
            {
                RaisePropertyChanged(propertyName);
            }

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
    }
}

