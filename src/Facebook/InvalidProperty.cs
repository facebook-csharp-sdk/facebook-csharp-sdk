using System;

namespace Facebook
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class InvalidProperty : DynamicDictionary, IEquatable<InvalidProperty>
    {
        static InvalidProperty instance = new InvalidProperty();

        public static InvalidProperty Instance
        {
            get { return instance; }
        }

        private InvalidProperty() { }

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
        public static explicit operator long(InvalidProperty invalidProperty)
        {
            return default(long);
        }

        public bool Equals(InvalidProperty other)
        {
            return true;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
