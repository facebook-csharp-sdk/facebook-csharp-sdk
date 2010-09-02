// --------------------------------
// <copyright file="DynamicDictionary.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;

namespace Facebook
{
    public class DynamicDictionary : DynamicObject, IDynamicMetaObjectProvider, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, INotifyPropertyChanged
    {
        // NOTE: This object is the same as System.Dynamic.ExpandoObject save for the fact
        // that it does not throw a RuntimeBinderException if you try to access a property 
        // that is not in the object. If the .Net Framework were to unseal ExpandoObject
        // we would simply derive from that and override TryGetMember().

        // The inner dictionary.
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        #region DynamicObject

        public void Add(string key, object value)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            Contract.EndContractBlock();

            this.dictionary.Add(key, value);
            this.NotifyPropertyChanged(key);
        }

        [ContractInvariantMethod] 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(dictionary != null);
        }

        // This property returns the number of elements
        // in the inner dictionary.
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder == null)
            {
                throw new ArgumentNullException("binder");
            }

            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.

            var exists = this.dictionary.TryGetValue(name, out result);
            if (exists)
            {
                return true;
            }

#if !SILVERLIGHT && TRACE
            if (!exists)
            {
                Trace.WriteLine(string.Format("This instance of DynamicDicationary does not contain the property {0}.", name));
            }
#endif
            // set to empty so it doesnt throw an exception when a member is accessed that is not there
            result = InvalidProperty.Instance;

            return true;

        }


        /// <summary>
        /// If you try to set a value of a property that is
        /// not defined in the class, this method is called.
        /// </summary>
        /// <param name="binder">The dynamic binder.</param>
        /// <param name="value">The member value.</param>
        /// <returns>Always returns true.</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder == null)
            {
                throw new ArgumentNullException("binder");
            }

            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            this.dictionary[binder.Name.ToLower()] = value;
            this.NotifyPropertyChanged(binder.Name.ToLower());

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }

        #endregion

        #region IDynamicMetaObjectProvider

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.dictionary.Keys.AsEnumerable();
        }

        #endregion

        #region IDictionary<string, object>

        public bool ContainsKey(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            return this.dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            var result = this.dictionary.Remove(key);
            if (result)
            {
                this.NotifyPropertyChanged(key);
            }
            return result;
        }

        public bool TryGetValue(string key, out object value)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            return this.dictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return this.dictionary.Values; }
        }

        public object this[string key]
        {
            get
            {
                if (String.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }
                return this.dictionary[key];
            }
            set
            {
                if (String.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }
                this.dictionary[key] = value;
                this.NotifyPropertyChanged(key);
            }
        }



        public void Add(KeyValuePair<string, object> item)
        {
            if (String.IsNullOrEmpty(item.Key))
            {
                throw new ArgumentNullException("key");
            }
            this.dictionary.Add(item.Key, item.Value);
            this.NotifyPropertyChanged(item.Key);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            if (String.IsNullOrEmpty(item.Key))
            {
                throw new ArgumentNullException("key");
            }
            return this.dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (this.dictionary.Keys.Count < arrayIndex || arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            this.dictionary.ToList().CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            if (String.IsNullOrEmpty(item.Key))
            {
                throw new ArgumentNullException("key");
            }
            var result = this.dictionary.Remove(item.Key);
            if (result)
            {
                this.NotifyPropertyChanged(item.Key);
            }
            return result;
        }

        #region IEnumerable<KeyValuePair<string, object>>

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.dictionary.AsEnumerable().GetEnumerator();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}