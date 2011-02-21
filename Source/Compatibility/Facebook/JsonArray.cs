// --------------------------------
// <copyright file="JsonArray.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

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
