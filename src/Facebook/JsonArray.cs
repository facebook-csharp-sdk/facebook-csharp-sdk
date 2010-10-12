// --------------------------------
// <copyright file="JsonArray.cs" company="Nikhil Kothari">
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
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Globalization;

namespace Facebook
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class JsonArray : DynamicObject, ICollection<object>, ICollection
    {

        private List<object> _members = new List<object>();

        public JsonArray() { }

        public JsonArray(object value)
            : this()
        {
            _members.Add(value);
        }

        public JsonArray(object value1, object value2)
            : this()
        {
            _members.Add(value1);
            _members.Add(value2);
        }

        public JsonArray(params object[] value)
            : this()
        {
            Contract.Requires(value != null);

            _members.AddRange(value);
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(_members != null);
        }

        public int Count
        {
            get
            {
                return _members.Count;
            }
        }

        public object this[int index]
        {
            get
            {
                if (_members.Count <= index)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                return _members[index];
            }
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            Type targetType = binder.Type;

            if ((targetType == typeof(IEnumerable)) ||
                (targetType == typeof(IEnumerable<object>)) ||
                (targetType == typeof(ICollection<object>)) ||
                (targetType == typeof(ICollection)))
            {
                result = this;
                return true;
            }

            return base.TryConvert(binder, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (String.Compare(binder.Name, "Add", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 1)
                {
                    _members.Add(args[0]);
                    result = null;
                    return true;
                }
                result = null;
                return false;
            }
            else if (String.Compare(binder.Name, "Insert", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 2)
                {
                    _members.Insert(Convert.ToInt32(args[0], CultureInfo.InvariantCulture), args[1]);
                    result = null;
                    return true;
                }
                result = null;
                return false;
            }
            else if (String.Compare(binder.Name, "IndexOf", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 1)
                {
                    result = _members.IndexOf(args[0]);
                    return true;
                }
                result = null;
                return false;
            }
            else if (String.Compare(binder.Name, "Clear", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 0)
                {
                    _members.Clear();
                    result = null;
                    return true;
                }
                result = null;
                return false;
            }
            else if (String.Compare(binder.Name, "Remove", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 1)
                {
                    result = _members.Remove(args[0]);
                    return true;
                }
                result = null;
                return false;
            }
            else if (String.Compare(binder.Name, "RemoveAt", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 1)
                {
                    _members.RemoveAt(Convert.ToInt32(args[0], CultureInfo.InvariantCulture));
                    result = null;
                    return true;
                }
                result = null;
                return false;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1)
            {
                result = _members[Convert.ToInt32(indexes[0], CultureInfo.InvariantCulture)];
                return true;
            }
            else
            {
#if !SILVERLIGHT && TRACE
                Trace.TraceWarning("This instance of JsonArray does not contain a value at this index.");
#endif
                result = null;
                return true;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (String.Compare("Length", binder.Name, StringComparison.Ordinal) == 0)
            {
                result = _members.Count;
                return true;
            }
            else
            {
#if !SILVERLIGHT && TRACE
                Trace.TraceWarning(String.Format(CultureInfo.InvariantCulture, "This instance of JsonArray does not contain the property {0}.", binder.Name));
#endif
                result = null;
                return true;
            }
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes.Length == 1)
            {
                _members[Convert.ToInt32(indexes[0], CultureInfo.InvariantCulture)] = value;
                return true;
            }

            return base.TrySetIndex(binder, indexes, value);
        }

        #region Implementation of IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _members.GetEnumerator();
        }
        #endregion

        #region Implementation of IEnumerable<object>
        IEnumerator<object> IEnumerable<object>.GetEnumerator()
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

        #region Implementation of ICollection<object>
        int ICollection<object>.Count
        {
            get
            {
                return _members.Count;
            }
        }

        bool ICollection<object>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        void ICollection<object>.Add(object item)
        {
            ((ICollection<object>)_members).Add(item);
        }

        void ICollection<object>.Clear()
        {
            ((ICollection<object>)_members).Clear();
        }

        bool ICollection<object>.Contains(object item)
        {
            return ((ICollection<object>)_members).Contains(item);
        }

        void ICollection<object>.CopyTo(object[] array, int arrayIndex)
        {
            ((ICollection<object>)_members).CopyTo(array, arrayIndex);
        }

        bool ICollection<object>.Remove(object item)
        {
            return ((ICollection<object>)_members).Remove(item);
        }
        #endregion
    }
}