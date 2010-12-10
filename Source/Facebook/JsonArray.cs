using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook
{
    public class JsonArray : List<object>
    {

        public override string ToString()
        {
            return JsonSerializer.SerializeObject(this);
        }
      
    }
}
