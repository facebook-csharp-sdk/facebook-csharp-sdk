// --------------------------------
// <copyright file="JsonSerializer.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Facebook.Utilities
{
    internal class JsonSerializer
    {

        public static string SerializeObject(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
        }

        public static dynamic DeserializeObject(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            Contract.EndContractBlock();

            dynamic result;
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                result = DeserializeObject(json);
            }
            return result;
        }

        public static dynamic DeserializeObject(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else if (json.Trim().StartsWith("<?xml"))
            {
                return ConvertXml(json);
            }
            else
            {
                object obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

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

        private static dynamic ConvertXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException("xml");
            }
            Contract.EndContractBlock();

            XDocument doc = XDocument.Parse(xml);
            if (doc != null && doc.Root != null)
            {
                return ConvertXElementToDictionary(doc.Root);
            }
            return null;
        }

        private static dynamic ConvertJTokenToDictionary(JToken token)
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
                DynamicDictionary expando = new DynamicDictionary();
                (from childToken in ((JToken)token) where childToken is JProperty select childToken as JProperty).ToList().ForEach(property =>
                {
                    expando.Add(property.Name, ConvertJTokenToDictionary(property.Value));
                });
                return expando;
            }
            else if (token is JContainer)
            {
                Collection<dynamic> collection = new Collection<dynamic>();
                foreach (JToken arrayItem in (JContainer)token)
                {
                    collection.Add(ConvertJTokenToDictionary(arrayItem));
                }
                return collection;
            }
            throw new ArgumentException(string.Format("Unknown token type '{0}'", token.GetType()), "token");
        }

        private static dynamic ConvertXElementToDictionary(XElement element)
        {
            if (element == null)
            {
                return null;
            }
            else if (element.HasElements)
            {
                DynamicDictionary expando = new DynamicDictionary();
                foreach (var child in element.Elements())
                {
                    expando.Add(child.Name.ToString(), ConvertXElementToDictionary(child));
                }
                return expando;
            }
            else
            {
                return element.Value;
            }
        }

    }
}
