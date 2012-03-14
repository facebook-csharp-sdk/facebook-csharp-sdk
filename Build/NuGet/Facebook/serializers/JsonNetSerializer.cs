//-----------------------------------------------------------------------
// <copyright file="JsonNetSErializer.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

namespace Facebook.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;

    /// <remarks>
    /// Install-Package Newtonsoft.Json
    /// It is recommended to set the the default json serializers during application startup.
    /// var jsonNetSerializer = new JsonNetSerializer();
    /// FacebookClient.SetDefaultJsonSerializers(jsonNetSerializer.SerializeObject, jsonNetSerializer.DeserializeObject);
    /// </remarks>
    public class JsonNetSerializer
    {
        private JsonSerializerSettings _serializerSettings;

        public JsonNetSerializer()
        {
            // Standard settings
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

            _serializerSettings = settings;
        }

        /// <summary>
        /// Serializes the object to json string.
        /// </summary>
        /// <param name="obj">
        /// The value.
        /// </param>
        /// <returns>
        /// The json string.
        /// </returns>
        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, this._serializerSettings);
        }


        /// <summary>
        /// Deserializes the json string.
        /// </summary>
        /// <param name="json">
        /// The json string.
        /// </param>
        /// <param name="type">
        /// The type of object.
        /// </param>
        /// <returns>
        /// The object.
        /// </returns>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        /// Occurs when deserialization fails.
        /// </exception>
        public object DeserializeObject(string json, Type type)
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
                    obj = JsonConvert.DeserializeObject(json, type, _serializerSettings);
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

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public T DeserializeObject<T>(string json)
        {
            return (T)DeserializeObject(json, typeof(T));
        }

        /// <summary>
        /// Deserialize the json string to object.
        /// </summary>
        /// <param name="json">
        /// The json string.
        /// </param>
        /// <returns>
        /// The object.
        /// </returns>
        public object DeserializeObject(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            object obj;

            try
            {
                obj = JsonConvert.DeserializeObject(json, this._serializerSettings);
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

        /// <summary>
        /// Converts the <see cref="JToken"/> to <see cref="object"/>
        /// </summary>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <returns>
        /// Returns the object.
        /// </returns>
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
            (from childToken in token where childToken is JProperty select childToken as JProperty).ToList().ForEach(property => jsonDict.Add(property.Name, ConvertJTokenToDictionary(property.Value)));
            return jsonObject;
        }
    }
}