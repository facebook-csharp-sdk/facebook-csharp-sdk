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
using System.Diagnostics.Contracts;
#if !NET35 && !WINDOWS_PHONE
using System.Dynamic;
#endif
using System.Globalization;
using System.Linq;

namespace Facebook
{
    /// <summary>
    /// Represents a JSON object.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class JsonObject :
#if !NET35 && !WINDOWS_PHONE
 DynamicObject, IDynamicMetaObjectProvider,
#endif
 IDictionary<string, object>
    {

        private Dictionary<string, object> members;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonObject"/> class.
        /// </summary>
        public JsonObject()
        {
            members = new Dictionary<string, object>();
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(members != null);
        }

        public void Add(string key, object value)
        {
            this.members.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return this.members.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.members.Keys; }
        }

        public bool Remove(string key)
        {
            return this.members.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.members.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return this.members.Values; }
        }

        public object this[string key]
        {
            get
            {
                return this.members[key];
            }
            set
            {
                this.members[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this.members.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.members.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.members.ContainsKey(item.Key) && this.members[item.Key] == item.Value;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.members.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.members.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.members.GetEnumerator();
        }


#if !NET35 && !WINDOWS_PHONE
        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Type returns the <see cref="T:System.String"/> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param>
        /// <param name="result">The result of the type conversion operation.</param>
        /// <returns>
        /// Alwasy returns true.
        /// </returns>
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

        /// <summary>
        /// Provides the implementation for operations that delete an object member. This method is not intended for use in C# or Visual Basic.
        /// </summary>
        /// <param name="binder">Provides information about the deletion.</param>
        /// <returns>
        /// Alwasy returns true.
        /// </returns>
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            // <pex>
            if (binder == (DeleteMemberBinder)null)
                throw new ArgumentNullException("binder");
            // </pex>
            return members.Remove(binder.Name);
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the DynamicObject class, <paramref name="indexes"/> is equal to 3.</param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>
        /// Alwasy returns true.
        /// </returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1)
            {
                result = ((IDictionary<string, object>)this)[(string)indexes[0]];
                return true;
            }
            else
            {
                result = (object)null;
                return true;
            }
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result"/>.</param>
        /// <returns>
        /// Alwasy returns true.
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object value;
            if (members.TryGetValue(binder.Name, out value))
            {
                result = value;
                return true;
            }
            else
            {
                result = (object)null;
                return true;
            }
        }

        /// <summary>
        /// Provides the implementation for operations that set a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that access objects by a specified index.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, <paramref name="indexes"/> is equal to 3.</param>
        /// <param name="value">The value to set to the object that has the specified index. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, <paramref name="value"/> is equal to 10.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.
        /// </returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes.Length == 1)
            {
                ((IDictionary<string, object>)this)[(string)indexes[0]] = value;
                return true;
            }

            return base.TrySetIndex(binder, indexes, value);
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, the <paramref name="value"/> is "Test".</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // <pex>
            if (binder == (SetMemberBinder)null)
                throw new ArgumentNullException("binder");
            // </pex>
            members[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// Returns the enumeration of all dynamic member names.
        /// </summary>
        /// <returns>
        /// A sequence that contains dynamic member names.
        /// </returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.members.Keys.AsEnumerable();
        }

#endif

        private sealed class DictionaryEnumerator : IDictionaryEnumerator
        {

            private IEnumerator<KeyValuePair<string, object>> _enumerator;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<string, object>> enumerator)
            {
                _enumerator = enumerator;
            }

            [ContractInvariantMethod]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            private void InvarientObject()
            {
                Contract.Invariant(_enumerator != null);
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