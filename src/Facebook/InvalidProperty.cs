using System;
using System.Dynamic;

namespace Facebook
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class InvalidProperty : DynamicObject
    {
        static InvalidProperty instance = new InvalidProperty();

        public static InvalidProperty Instance
        {
            get { return instance; }
        }

        private InvalidProperty() { }

        public int Count
        {
            get
            {
                return 0;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = InvalidProperty.Instance;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = InvalidProperty.Instance;
            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static implicit operator string(InvalidProperty invalidProperty)
        {
            return string.Empty;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator bool(InvalidProperty invalidProperty)
        {
            return default(bool);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator int(InvalidProperty invalidProperty)
        {
            return default(int);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator double(InvalidProperty invalidProperty)
        {
            return default(double);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator long(InvalidProperty invalidProperty)
        {
            return default(long);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "invalidProperty")]
        public static explicit operator DateTime(InvalidProperty invalidProperty)
        {
            return default(DateTime);
        }

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

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
