// --------------------------------
// <copyright file="JsonSerializer.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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


        public static string SerializeObject(object value)
        {
            string json;
            using (JsonWriter writer = new JsonWriter())
            {
                writer.WriteValue(value);
                json = writer.Json;
            }
            return json;
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
                object value;
                using (JsonReader reader = new JsonReader(json))
                {
                    value = reader.ReadValue();
                }
                return value;
            }
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
