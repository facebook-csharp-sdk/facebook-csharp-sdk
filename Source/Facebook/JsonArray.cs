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
#if !NET35 && !WINDOWS_PHONE
using System.Dynamic;
#endif
using System.Globalization;

namespace Facebook
{
    /// <summary>
    /// Represents a JSON array.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class JsonArray :
#if !NET35 && !WINDOWS_PHONE
 DynamicObject,
#endif
 IList<object>
    {

        private List<object> members;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArray"/> class.
        /// </summary>
        public JsonArray()
        {
            this.members = new List<object>();
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(members != null);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the entire list.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int IndexOf(object item)
        {
            return this.members.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, object item)
        {
            this.members.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            this.members.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value></value>
        public object this[int index]
        {
            get
            {
                return this.members[index];
            }
            set
            {
                this.members[index] = value;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(object item)
        {
            this.members.Add(item);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.members.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(object item)
        {
            return this.members.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(object[] array, int arrayIndex)
        {
            this.members.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return this.members.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(object item)
        {
            return this.members.Remove(item);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<object> GetEnumerator()
        {
            return this.members.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
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
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
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

        /// <summary>
        /// Provides the implementation for operations that invoke a member. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as calling a method.
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="args">The arguments that are passed to the object member during the invoke operation. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, <paramref name="args"/> is equal to 100.</param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (String.Compare(binder.Name, "Add", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 1)
                {
                    members.Add(args[0]);
                    result = (object)null;
                    return true;
                }
                result = (object)null;
                return false;
            }
            else if (String.Compare(binder.Name, "Insert", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 2)
                {
                    members.Insert(Convert.ToInt32(args[0], CultureInfo.InvariantCulture), args[1]);
                    result = (object)null;
                    return true;
                }
                result = (object)null;
                return false;
            }
            else if (String.Compare(binder.Name, "IndexOf", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 1)
                {
                    result = members.IndexOf(args[0]);
                    return true;
                }
                result = (object)null;
                return false;
            }
            else if (String.Compare(binder.Name, "Clear", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 0)
                {
                    members.Clear();
                    result = (object)null;
                    return true;
                }
                result = (object)null;
                return false;
            }
            else if (String.Compare(binder.Name, "Remove", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 1)
                {
                    result = members.Remove(args[0]);
                    return true;
                }
                result = (object)null;
                return false;
            }
            else if (String.Compare(binder.Name, "RemoveAt", StringComparison.Ordinal) == 0)
            {
                if (args.Length == 1)
                {
                    members.RemoveAt(Convert.ToInt32(args[0], CultureInfo.InvariantCulture));
                    result = (object)null;
                    return true;
                }
                result = (object)null;
                return false;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the DynamicObject class, <paramref name="indexes"/> is equal to 3.</param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1)
            {
                result = members[Convert.ToInt32(indexes[0], CultureInfo.InvariantCulture)];
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
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (String.Compare("Length", binder.Name, StringComparison.Ordinal) == 0)
            {
                result = members.Count;
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
                members[Convert.ToInt32(indexes[0], CultureInfo.InvariantCulture)] = value;
                return true;
            }

            return base.TrySetIndex(binder, indexes, value);
        }
#endif
    }
}