﻿// --------------------------------
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
                var jToken = obj as JToken;
                if (jToken != null)
                {
                    return ConvertJTokenToDictionary(jToken);
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

            var jValue = token as JValue;
            if (jValue != null)
            {
                return jValue.Value;
            }

            var jContainer = token as JArray;
            if (jContainer != null)
            {
                var jsonList = new JsonArray();
                foreach (JToken arrayItem in jContainer)
                {
                    jsonList.Add(ConvertJTokenToDictionary(arrayItem));
                }
                return jsonList;
            }

            var jsonObject = new JsonObject();
            var jsonDict = (IDictionary<string, object>)jsonObject;
            (from childToken in token where childToken is JProperty select childToken as JProperty).ToList().ForEach(property =>
            {
                jsonDict.Add(property.Name, ConvertJTokenToDictionary(property.Value));
            });
            return jsonObject;
        }

    }
}