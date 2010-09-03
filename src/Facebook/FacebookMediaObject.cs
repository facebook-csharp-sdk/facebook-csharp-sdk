using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook
{
    public class FacebookMediaObject
    {
        private byte[] _value;

        public string ContentType { get; set; }
        public string FileName { get; set; }

        public void SetValue(byte[] value)
        {
            _value = value;
        }

        public byte[] GetValue()
        {
            return _value;
        }
    }
}
