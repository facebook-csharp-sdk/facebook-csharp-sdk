// --------------------------------
// <copyright file="JsonObject.cs" company="Nikhil Kothari">
//     Copyright (c) 2010 Nikhil Kothari
// </copyright>
// <author>Nikhil Kothari (http://www.nikhilk.net)</author>
// <license>Included in this library with permission. Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://github.com/NikhilK/dynamicrest</website>
// ---------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Diagnostics.Contracts;

namespace Facebook
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class JsonObject : DynamicObject, IDictionary<string, object>, IDictionary, IDynamicMetaObjectProvider
    {

        private Dictionary<string, object> _members;

        public JsonObject()
        {
            _members = new Dictionary<string, object>();
        }

        public JsonObject(params object[] nameValuePairs)
            : this()
        {
            if (nameValuePairs != null)
            {
                if (nameValuePairs.Length % 2 != 0)
                {
                    throw new ArgumentException("Mismatch in name/value pairs.");
                }

                for (int i = 0; i < nameValuePairs.Length; i += 2)
                {
                    if (!(nameValuePairs[i] is string))
                    {
                        throw new ArgumentException("Name parameters must be strings.");
                    }

                    _members[(string)nameValuePairs[i]] = nameValuePairs[i + 1];
                }
            }
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(_members != null);
        }

        // This property returns the number of elements
        // in the inner dictionary.
        public int Count
        {
            get
            {
                return this._members.Count;
            }
        }

        public object this[string key]
        {
            get
            {
                return ((IDictionary<string, object>)this)[key];
            }
            set
            {
                ((IDictionary<string, object>)this)[key] = value;
            }
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, object>)this).ContainsKey(key);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            // <pex>
            if (binder == (ConvertBinder)null)
                throw new ArgumentNullException("binder");
            // </pex>
            Type targetType = binder.Type;

            if ((targetType == typeof(IEnumerable)) ||
                (targetType == typeof(IEnumerable<KeyValuePair<string, object>>)) ||
                (targetType == typeof(IDictionary<string, object>)) ||
                (targetType == typeof(IDictionary)))
            {
                result = this;
                return true;
            }

            return base.TryConvert(binder, out result);
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            // <pex>
            if (binder == (DeleteMemberBinder)null)
                throw new ArgumentNullException("binder");
            // </pex>
            return _members.Remove(binder.Name);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            // <pex>
            if (indexes[0L] != (object)null)
                throw new ArgumentException("indexes[0L] != (object)null", "indexes");
            if (indexes == (object[])null)
                throw new ArgumentNullException("indexes");
            // </pex>
            if (indexes.Length == 1)
            {
                result = ((IDictionary<string, object>)this)[(string)indexes[0]];
                return true;
            }
            else
            {
#if !SILVERLIGHT && TRACE
                Trace.TraceWarning("This instance of JsonArray does not contain a value at this index.");
#endif
                result = InvalidProperty.Instance;
                return true;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // <pex>
            if (binder == (GetMemberBinder)null)
                throw new ArgumentNullException("binder");
            // </pex>
            object value;
            if (_members.TryGetValue(binder.Name, out value))
            {
                result = value;
                return true;
            }
            else
            {
#if !SILVERLIGHT && TRACE
                Trace.TraceWarning(string.Format("This instance of JsonObject does not contain the property {0}.", binder.Name));
#endif
                result = InvalidProperty.Instance;
                return true;
            }
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            // <pex>
            if (indexes[0L] != (object)null)
                throw new ArgumentException("indexes[0L] != (object)null", "indexes");
            if (indexes == (object[])null)
                throw new ArgumentNullException("indexes");
            // </pex>
            if (indexes.Length == 1)
            {
                ((IDictionary<string, object>)this)[(string)indexes[0]] = value;
                return true;
            }

            return base.TrySetIndex(binder, indexes, value);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // <pex>
            if (binder == (SetMemberBinder)null)
                throw new ArgumentNullException("binder");
            // </pex>
            _members[binder.Name] = value;
            return true;
        }

        #region Implementation of IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DictionaryEnumerator(_members.GetEnumerator());
        }
        #endregion

        #region Implementation of IEnumerable<KeyValuePair<string, object>>
        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return _members.GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection
        int ICollection.Count
        {
            get
            {
                return _members.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Implementation of ICollection<KeyValuePair<string, object>>
        int ICollection<KeyValuePair<string, object>>.Count
        {
            get
            {
                return _members.Count;
            }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            ((IDictionary<string, object>)_members).Add(item);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            _members.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)_members).Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((IDictionary<string, object>)_members).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)_members).Remove(item);
        }
        #endregion

        #region Implementation of IDictionary
        bool IDictionary.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return _members.Keys;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return ((IDictionary<string, object>)this)[(string)key];
            }
            set
            {
                ((IDictionary<string, object>)this)[(string)key] = value;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return _members.Values;
            }
        }

        void IDictionary.Add(object key, object value)
        {
            _members.Add((string)key, value);
        }

        void IDictionary.Clear()
        {
            _members.Clear();
        }

        bool IDictionary.Contains(object key)
        {
            return _members.ContainsKey((string)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return (IDictionaryEnumerator)((IEnumerable)this).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            _members.Remove((string)key);
        }
        #endregion

        #region Implementation of IDictionary<string, object>
        ICollection<string> IDictionary<string, object>.Keys
        {
            get
            {
                return _members.Keys;
            }
        }

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                object value = null;
                if (_members.TryGetValue(key, out value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                _members[key] = value;
            }
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get
            {
                return _members.Values;
            }
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            _members.Add(key, value);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return _members.ContainsKey(key);
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return _members.Remove(key);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return _members.TryGetValue(key, out value);
        }
        #endregion

        #region IDynamicMetaObjectProvider

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this._members.Keys.AsEnumerable();
        }

        #endregion

        private sealed class DictionaryEnumerator : IDictionaryEnumerator
        {

            private IEnumerator<KeyValuePair<string, object>> _enumerator;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<string, object>> enumerator)
            {
                _enumerator = enumerator;
            }

            public object Current
            {
                get
                {
                    return Entry;
                }
            }

            public DictionaryEntry Entry
            {
                get
                {
                    return new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value);
                }
            }

            public object Key
            {
                get
                {
                    return _enumerator.Current.Key;
                }
            }

            public object Value
            {
                get
                {
                    return _enumerator.Current.Value;
                }
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
    }
}