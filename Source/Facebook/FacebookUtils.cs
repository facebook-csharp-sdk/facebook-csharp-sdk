// --------------------------------
// <copyright file="FacebookUtils.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using FluentHttp;

    internal class FacebookUtils
    {
        #region Constants

        public static class Resources
        {
            public const string GetResultDataGenericNotSupportedForBatchRequests = "GetResultData<T> not supported for batch results.";
            public const string InvalidSignedRequest = "Invalid signed request.";
            public const string MediaObjectMustHavePropertiesSetError = "The media object must have a content type, file name, and value set.";
            public const string ParameterMethodValueRequired = "You must specify a value for the method parameter.";
            public const string return_ssl_resources = "return_ssl_resources";
        }

        /// <summary>
        /// The multi-part form prefix characters.
        /// </summary>
        public const string MultiPartFormPrefix = "--";

        /// <summary>
        /// The multi-part form new line characters.
        /// </summary>
        public const string MultiPartNewLine = "\r\n";

        public const string DOMAIN_MAP_API = "api";
        public const string DOMAIN_MAP_API_READ = "api_read";
        public const string DOMAIN_MAP_API_VIDEO = "api_video";
        public const string DOMAIN_MAP_GRAPH = "graph";
        public const string DOMAIN_MAP_GRAPH_VIDEO = "graph_video";
        public const string DOMAIN_MAP_WWW = "www";
        public const string DOMAIN_MAP_APPS = "apps";

        /// <summary>
        /// Domain Maps
        /// </summary>
        public static Dictionary<string, Uri> DomainMaps = new Dictionary<string, Uri> {
            { DOMAIN_MAP_API,       new Uri("https://api.facebook.com/") },
            { DOMAIN_MAP_API_READ,  new Uri("https://api-read.facebook.com/") },
            { DOMAIN_MAP_API_VIDEO, new Uri("https://api-video.facebook.com/") },
            { DOMAIN_MAP_GRAPH,     new Uri("https://graph.facebook.com/") },
            { DOMAIN_MAP_GRAPH_VIDEO,new Uri("https://graph-video.facebook.com/")},
            { DOMAIN_MAP_WWW,       new Uri("http://www.facebook.com/") },
            { DOMAIN_MAP_APPS,      new Uri("http://apps.facebook.com/") }
        };

        /// <summary>
        /// Secure Domain Maps
        /// </summary>
        public static Dictionary<string, Uri> DomainMapsSecure = new Dictionary<string, Uri> {
            { DOMAIN_MAP_API,       new Uri("https://api.facebook.com/") },
            { DOMAIN_MAP_API_READ,  new Uri("https://api-read.facebook.com/") },
            { DOMAIN_MAP_API_VIDEO, new Uri("https://api-video.facebook.com/") },
            { DOMAIN_MAP_GRAPH,     new Uri("https://graph.facebook.com/") },
            { DOMAIN_MAP_GRAPH_VIDEO,new Uri("https://graph-video.facebook.com/")},
            { DOMAIN_MAP_WWW,       new Uri("https://www.facebook.com/") },
            { DOMAIN_MAP_APPS,      new Uri("https://apps.facebook.com/") }
        };

        public static Dictionary<string, Uri> DomainMapsBeta = new Dictionary<string, Uri> {
            { DOMAIN_MAP_API,       new Uri("https://api.beta.facebook.com/") },
            { DOMAIN_MAP_API_READ,  new Uri("https://api-read.beta.facebook.com/") },
            { DOMAIN_MAP_API_VIDEO, new Uri("https://api-video.beta.facebook.com/") },
            { DOMAIN_MAP_GRAPH,     new Uri("https://graph.beta.facebook.com/") },
            { DOMAIN_MAP_GRAPH_VIDEO,new Uri("https://graph-video.beta.facebook.com/")},
            { DOMAIN_MAP_WWW,       new Uri("http://www.beta.facebook.com/") },
            { DOMAIN_MAP_APPS,      new Uri("http://apps.beta.facebook.com/") }
        };

        public static Dictionary<string, Uri> DomainMapsBetaSecure = new Dictionary<string, Uri> {
            { DOMAIN_MAP_API,       new Uri("https://api.beta.facebook.com/") },
            { DOMAIN_MAP_API_READ,  new Uri("https://api-read.beta.facebook.com/") },
            { DOMAIN_MAP_API_VIDEO, new Uri("https://api-video.beta.facebook.com/") },
            { DOMAIN_MAP_GRAPH,     new Uri("https://graph.beta.facebook.com/") },
            { DOMAIN_MAP_GRAPH_VIDEO,new Uri("https://graph-video.beta.facebook.com/")},
            { DOMAIN_MAP_WWW,       new Uri("https://www.beta.facebook.com/") },
            { DOMAIN_MAP_APPS,      new Uri("https://apps.beta.facebook.com/") }
        };

        public static string[] ReadOnlyCalls = new[] {
            "admin.getallocation",
            "admin.getappproperties",
            "admin.getbannedusers",
            "admin.getlivestreamvialink",
            "admin.getmetrics",
            "admin.getrestrictioninfo",
            "application.getpublicinfo",
            "auth.getapppublickey",
            "auth.getsession",
            "auth.getsignedpublicsessiondata",
            "comments.get",
            "connect.getunconnectedfriendscount",
            "dashboard.getactivity",
            "dashboard.getcount",
            "dashboard.getglobalnews",
            "dashboard.getnews",
            "dashboard.multigetcount",
            "dashboard.multigetnews",
            "data.getcookies",
            "events.get",
            "events.getmembers",
            "fbml.getcustomtags",
            "feed.getappfriendstories",
            "feed.getregisteredtemplatebundlebyid",
            "feed.getregisteredtemplatebundles",
            "fql.multiquery",
            "fql.query",
            "friends.arefriends",
            "friends.get",
            "friends.getappusers",
            "friends.getlists",
            "friends.getmutualfriends",
            "gifts.get",
            "groups.get",
            "groups.getmembers",
            "intl.gettranslations",
            "links.get",
            "notes.get",
            "notifications.get",
            "pages.getinfo",
            "pages.isadmin",
            "pages.isappadded",
            "pages.isfan",
            "permissions.checkavailableapiaccess",
            "permissions.checkgrantedapiaccess",
            "photos.get",
            "photos.getalbums",
            "photos.gettags",
            "profile.getinfo",
            "profile.getinfooptions",
            "stream.get",
            "stream.getcomments",
            "stream.getfilters",
            "users.getinfo",
            "users.getloggedinuser",
            "users.getstandardinfo",
            "users.hasapppermission",
            "users.isappuser",
            "users.isverified",
            "video.getuploadlimits" 
        };

        public static Collection<string> DropQueryParameters = new Collection<string> {
            "session",
            "signed_request",
        };

        #endregion

        public static string ConvertToStringForce(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    return "GET";
                case HttpMethod.Delete:
                    return "DELETE";
                case HttpMethod.Post:
                    return "POST";
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the string representation of the specified http method.
        /// </summary>
        /// <param name="httpMethod">
        /// The http method.
        /// </param>
        /// <returns>
        /// The string representation of the http method.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throws error if the http method is not Get,Post or Delete.
        /// </exception>
        public static string ConvertToString(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    return "GET";
                case HttpMethod.Delete:
#if !SILVERLIGHT
                    return "DELETE";
#endif
                case HttpMethod.Post:
                    return "POST";
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Removes the querystring parameters from the path value and adds them
        /// to the parameters dictionary.
        /// </summary>
        /// <param name="path">The path to parse.</param>
        /// <param name="parameters">The dictionary</param>
        /// <returns></returns>
        public static string ParseQueryParametersToDictionary(string path, IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            if (String.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            Uri url;
            if (Uri.TryCreate(path, UriKind.Absolute, out url))
            {
                if (url.Host == "graph.facebook.com" || url.Host == "graph-video.facebook.com")
                {
                    // If the host is graph.facebook.com the user has passed in the full url.
                    // We remove the host part and continue with the parsing.
                    path = String.Concat(url.AbsolutePath, url.Query);
                }
                else
                {
                    // If the url is a valid absolute url we are passing the full url as the 'id'
                    // parameter of the query. For example, if path is something like
                    // http://www.microsoft.com/page.aspx?id=23 it means that we are trying
                    // to make the request https://graph.facebook.com/http://www.microsoft.com/page.aspx%3Fid%3D23
                    // So we are just going to return the path
                    return path;
                }
            }

            // Clean the path, remove leading '/'. 
            // If the path is '/' just return.
            if (path[0] == '/' && path.Length > 1)
            {
                path = path.Substring(1);
            }

            // If the url does not have a host it means we are using a url
            // like /me or /me?fields=first_name,last_name so we want to
            // remove the querystring info and add it to parameters
            if (!String.IsNullOrEmpty(path) && path.Contains('?'))
            {
                var parts = path.Split('?');
                path = parts[0]; // Set the path to only the path portion of the url
                if (parts.Length > 1 && parts[1] != null)
                {
                    // Add the query string values to the parameters dictionary
                    var qs = parts[1];
                    var keyValPairs = qs.Split('&');
                    foreach (var kvp in keyValPairs)
                    {
                        if (!String.IsNullOrEmpty(kvp))
                        {
                            var kv = kvp.Split('=');
                            if (kv.Length == 2 && !String.IsNullOrEmpty(kv[0]))
                            {
                                parameters[HttpHelper.UrlDecode(kv[0])] = HttpHelper.UrlDecode(kv[1]);
                            }
                        }
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Build the URL for given domain alias, path and parameters.
        /// </summary>
        /// <param name="domainMaps">
        /// The domain maps.
        /// </param>
        /// <param name="name">
        /// The name of the domain (from the domain maps).
        /// </param>
        /// <param name="path">
        /// Optional path (without a leading slash).
        /// </param>
        /// <param name="parameters">
        /// Optional query parameters.
        /// </param>
        /// <returns>
        /// The string of the url for the given parameters.
        /// </returns>
        public static Uri GetUrl(IDictionary<string, Uri> domainMaps, string name, string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (!domainMaps.ContainsKey(name))
            {
                throw new ArgumentException("Invalid url name.");
            }

            var uri = new UriBuilder(domainMaps[name]);
            if (!String.IsNullOrEmpty(path))
            {
                if (path[0] == '/')
                {
                    path = path.Length > 1 ? path.Substring(1) : string.Empty;
                }

                if (!string.IsNullOrEmpty(path))
                {
                    uri.Path = HttpHelper.UrlEncode(path);
                }
            }

            if (parameters != null)
            {
                uri.Query = ToJsonQueryString(parameters);
            }

            return uri.Uri;
        }

        /// <summary>
        /// Converts the dictionary to a json formatted query string.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>A json formatted querystring.</returns>
        public static string ToJsonQueryString(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            var sb = new StringBuilder();
            bool isFirst = true;
            foreach (var key in dictionary.Keys)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append("&");
                }

                if (dictionary[key] != null)
                {
                    // Format Object As json And Remove leading and trailing parenthesis
                    string jsonValue = ToJsonString(dictionary[key]);

                    if (!string.IsNullOrEmpty(jsonValue))
                    {
                        var encodedValue = HttpHelper.UrlEncode(jsonValue);
                        sb.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}", key, encodedValue);
                    }
                }
                else
                {
                    sb.Append(key);
                }
            }

            return sb.ToString();
        }

        public static string ToJsonString(object value)
        {
            // Format Object As json And Remove leading and trailing parenthesis
            string jsonValue = JsonSerializer.Current.SerializeObject(value);
            jsonValue = SimpleJson.SimpleJson.EscapeToJavascriptString(jsonValue);
            if (jsonValue.StartsWith("\"", StringComparison.Ordinal))
            {
                jsonValue = jsonValue.Substring(1, jsonValue.Length - 1);
            }

            if (jsonValue.EndsWith("\"", StringComparison.Ordinal))
            {
                jsonValue = jsonValue.Substring(0, jsonValue.Length - 1);
            }

            return jsonValue;
        }

        public static bool IsUsingRestApi(IDictionary<string, Uri> domainMaps, Uri requestUri)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            var map = domainMaps ?? DomainMaps;

            return requestUri.Host == map[DOMAIN_MAP_API].Host ||
                   requestUri.Host == map[DOMAIN_MAP_API_READ].Host ||
                   requestUri.Host == map[DOMAIN_MAP_API_VIDEO].Host;
        }

        public static void CopyStream(Stream input, Stream output, int? bufferSize)
        {
            byte[] buffer = new byte[bufferSize ?? 1024 * 4]; // 4 kb
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
            }
        }

        /// <summary>
        /// Convert the object to dictionary.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the dictionary equivalent of the specified object.
        /// </returns>
        public static IDictionary<string, object> ToDictionary(object parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            if (parameters is JsonObject)
            {
                return (JsonObject)parameters;
            }

            var json = JsonSerializer.Current.SerializeObject(parameters);
            return (IDictionary<string, object>)JsonSerializer.Current.DeserializeObject(json);
        }

        /// <summary>
        /// Merges two dictionaries.
        /// </summary>
        /// <param name="first">Default values, only used if second does not contain a value.</param>
        /// <param name="second">Every value of the merged object is used.</param>
        /// <returns>The merged dictionary</returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            first = first ?? new Dictionary<TKey, TValue>();
            second = second ?? new Dictionary<TKey, TValue>();
            var merged = new Dictionary<TKey, TValue>();

            foreach (var kvp in second)
            {
                merged.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in first)
            {
                if (!merged.ContainsKey(kvp.Key))
                {
                    merged.Add(kvp.Key, kvp.Value);
                }
            }

            return merged;
        }

        /// <summary>
        /// Parse a URL query and fragment parameters into a key-value bundle.
        /// </summary>
        /// <param name="query">
        /// The URL query to parse.
        /// </param>
        /// <returns>
        /// Returns a dictionary of keys and values for the querystring.
        /// </returns>
        public static IDictionary<string, object> ParseUrlQueryString(string query)
        {
            var result = new Dictionary<string, object>();

            // if string is null, empty or whitespace
            if (string.IsNullOrEmpty(query) || query.Trim().Length == 0)
            {
                return result;
            }

            string decoded = HttpHelper.HtmlDecode(query);
            int decodedLength = decoded.Length;
            int namePos = 0;
            bool first = true;

            while (namePos <= decodedLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (int q = namePos; q < decodedLength; q++)
                {
                    if (valuePos == -1 && decoded[q] == '=')
                    {
                        valuePos = q + 1;
                    }
                    else if (decoded[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                if (first)
                {
                    first = false;
                    if (decoded[namePos] == '?')
                    {
                        namePos++;
                    }
                }

                string name, value;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = HttpHelper.UrlDecode(decoded.Substring(namePos, valuePos - namePos - 1));
                }

                if (valueEnd < 0)
                {
                    namePos = -1;
                    valueEnd = decoded.Length;
                }
                else
                {
                    namePos = valueEnd + 1;
                }

                value = HttpHelper.UrlDecode(decoded.Substring(valuePos, valueEnd - valuePos));

                if (!string.IsNullOrEmpty(name))
                {
                    result[name] = value;
                }

                if (namePos == -1)
                {
                    break;
                }
            }

            return result;
        }

#if TPL

        public static void TransferCompletionToTask<T>(System.Threading.Tasks.TaskCompletionSource<T> tcs, System.ComponentModel.AsyncCompletedEventArgs e, Func<T> getResult, Action unregisterHandler)
        {
            if (e.UserState != tcs)
                return;

            try
            {
                unregisterHandler();
            }
            finally
            {
                if (e.Cancelled) tcs.TrySetCanceled();
                else if (e.Error != null) tcs.TrySetException(e.Error);
                else tcs.TrySetResult(getResult());
            }
        }

#endif
    }
}