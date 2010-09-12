using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;

namespace Facebook
{
    /// <summary>
    /// A dynamic object representing a property that was not found on the
    /// dynamic object that returns this type.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class InvalidProperty : DynamicObject, IEnumerable, ICollection
    {
        static InvalidProperty instance = new InvalidProperty();

        /// <summary>
        /// The singlton instance of this object.
        /// </summary>
        public static InvalidProperty Instance
        {
            get { return instance; }
        }

        private InvalidProperty() { }

        /// <summary>
        /// Gets an object with the specified property name.
        /// </summary>
        /// <param name="binder">The member binder.</param>
        /// <param name="result">Always set to the singleton instance of InvalidProperty.</param>
        /// <returns>True.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = InvalidProperty.Instance;
            return true;
        }

        /// <summary>
        /// Gets the object as a specified index.
        /// </summary>
        /// <param name="binder">The member binder.</param>
        /// <param name="indexes">The indexes.</param>
        /// <param name="result">Always set to the singleton instance of InvalidProperty.</param>
        /// <returns>True.</returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = InvalidProperty.Instance;
            return true;
        }

        /// <summary>
        /// Converts the invalid property to a string.
        /// </summary>
        /// <param name="invalidProperty">The instance of invalid property to convert.</param>
        /// <returns>An empty string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static implicit operator string(InvalidProperty invalidProperty)
        {
            return string.Empty;
        }

        /// <summary>
        /// Converts the invalid property to a bool.
        /// </summary>
        /// <param name="invalidProperty">The instance of invalid property to convert.</param>
        /// <returns>False</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator bool(InvalidProperty invalidProperty)
        {
            return false;
        }

        /// <summary>
        /// Converts the invalid property to a integer.
        /// </summary>
        /// <param name="invalidProperty">The instance of invalid property to convert.</param>
        /// <returns>0</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator int(InvalidProperty invalidProperty)
        {
            return 0;
        }

        /// <summary>
        /// Converts the invalid property to a double.
        /// </summary>
        /// <param name="invalidProperty">The instance of invalid property to convert.</param>
        /// <returns>0</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator double(InvalidProperty invalidProperty)
        {
            return 0;
        }

        /// <summary>
        /// Converts the invalid property to a long.
        /// </summary>
        /// <param name="invalidProperty">The instance of invalid property to convert.</param>
        /// <returns>0</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator long(InvalidProperty invalidProperty)
        {
            return 0;
        }

        /// <summary>
        /// Converts the invalid property to a DateTime.
        /// </summary>
        /// <param name="invalidProperty">The instance of invalid property to convert.</param>
        /// <returns>DateTime.MinValue</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator DateTime(InvalidProperty invalidProperty)
        {
            return DateTime.MinValue;
        }


        /// <summary>
        /// Compares the equality of an object to this object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if equal, otherwise false.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            else if (obj is InvalidProperty)
            {
                return true;
            }
            else if (obj is string)
            {
                return String.IsNullOrEmpty((string)obj);
            }
            else if (obj is int)
            {
                return (int)obj == default(int);
            }
            else if (obj is double)
            {
                return (double)obj == default(double);
            }
            else if (obj is long)
            {
                return (long)obj == default(long);
            }
            else if (obj is bool)
            {
                return (bool)obj == default(bool);
            }
            else if (obj is DateTime)
            {
                return (DateTime)obj == default(DateTime);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Always returns 0.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Always returns an empty string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Empty;
        }

        #region Implentation of ICollection

        int ICollection.Count
        {
            get
            {
                return 0;
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

        #region Implementation of IEnumerable

        public IEnumerator GetEnumerator()
        {
            return (new object[0]).GetEnumerator();
        }

        #endregion
    }
}
