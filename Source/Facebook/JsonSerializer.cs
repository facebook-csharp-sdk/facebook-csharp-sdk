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
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace Facebook
{
    internal static class JsonSerializer
    {
        
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
                return JsonConvert.DeserializeObject(json, type);
            }
        }

        private static object ConvertXml(string xml)
        {
            Contract.Requires(!String.IsNullOrEmpty(xml));

            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);
            return DeserializeObject(json);
        }

    }
}
