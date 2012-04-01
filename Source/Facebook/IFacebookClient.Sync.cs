using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook
{
    public partial interface IFacebookClient
    {
		object Get(string path);
		object Get(object parameters);
		object Get(string path, object parameters);
		object Post(object parameters);
		object Post(string path, object parameters);
		object Delete(string path);
		object Delete(string path, object parameters);
    }
}
