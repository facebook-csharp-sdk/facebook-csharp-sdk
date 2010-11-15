// --------------------------------
// <copyright file="JsonSerializer.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Facebook
{
    internal static class JsonSerializer
    {
        private static readonly Regex _stripXmlnsRegex = new Regex(@"(xmlns:?[^=]*=[""][^""]*[""])",
#if SILVERLIGHT
 RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
#else
 RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
#endif

        private static JsonSerializerSettings SerializerSettings
        {
            get
            {
                var isoDate = new IsoDateTimeConverter();
                isoDate.DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";
                var settings = new JsonSerializerSettings();
                settings.Converters = settings.Converters ?? new List<JsonConverter>();
                settings.Converters.Add(isoDate);
                settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                settings.NullValueHandling = NullValueHandling.Include;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.TypeNameHandling = TypeNameHandling.None;
                settings.ConstructorHandling = ConstructorHandling.Default;
                return settings;
            }
        }


        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.None, SerializerSettings);
        }

        public static object DeserializeObject(Stream stream)
        {
            Contract.Requires(stream != null);

            object result;
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                result = DeserializeObject(json);
            }
            return result;
        }

        public static object DeserializeObject(string json)
        {
            return DeserializeObject(json, null);
        }

        public static object DeserializeObject(string json, Type type)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else if (json.Trim().StartsWith("<?xml", StringComparison.Ordinal))
            {
                return ConvertXml(json);
            }
            else
            {
                object obj;
                
                try
                {
                    obj = JsonConvert.DeserializeObject(json, type, SerializerSettings);
                }
                catch (JsonSerializationException ex)
                {
                    throw new System.Runtime.Serialization.SerializationException(ex.Message, ex);
                }

                // If the object is a JToken we want to
                // convert it to dynamic, it if is any
                // other type we just return it.
                if (obj is JToken)
                {
                    return ConvertJTokenToDictionary((JToken)obj);
                }
                else
                {
                    return obj;
                }
            }
        }

        private static object ConvertJTokenToDictionary(JToken token)
        {
            if (token == null)
            {
                return null;
            }
            else if (token is JValue)
            {
                return ((JValue)token).Value;
            }
            else if (token is JObject)
            {
                var jsonObject = new JsonObject();
                var jsonDict = (IDictionary<string, object>)jsonObject;
                (from childToken in ((JToken)token) where childToken is JProperty select childToken as JProperty).ToList().ForEach(property =>
                {
                    jsonDict.Add(property.Name, ConvertJTokenToDictionary(property.Value));
                });
                return jsonObject;
            }
            else if (token is JContainer)
            {
                var jsonArray = new JsonArray();
                var jsonColl = (ICollection<object>)jsonArray;
                foreach (JToken arrayItem in (JContainer)token)
                {
                    jsonColl.Add(ConvertJTokenToDictionary(arrayItem));
                }
                return jsonArray;
            }
            throw new ArgumentException(string.Format("Unknown token type '{0}'", token.GetType()), "token");
        }

        private static object ConvertXml(string xml)
        {
            Contract.Requires(!String.IsNullOrEmpty(xml));

            xml = _stripXmlnsRegex.Replace(xml, string.Empty);

            XDocument doc = XDocument.Parse(xml);
            if (doc != null && doc.Root != null)
            {
                return ConvertXElementToDictionary(doc.Root);
            }
            return null;
        }

        private static object ConvertXElementToDictionary(XElement element)
        {
            if (element == null)
            {
                return null;
            }
            else if (element.HasElements)
            {
                JsonObject jsonObject = new JsonObject();
                var jsonDict = (IDictionary<string, object>)jsonObject;
                foreach (var child in element.Elements())
                {
                    jsonDict.Add(child.Name.ToString(), ConvertXElementToDictionary(child));
                }
                return jsonObject;
            }
            else
            {
                return element.Value;
            }
        }

    }
}
