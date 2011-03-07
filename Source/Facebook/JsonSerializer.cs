namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics.Contracts;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Converters;

    public class JsonSerializer
    {
        private static JsonSerializer instance = new JsonSerializer();

        public static IJsonSerializer Current
        {
            get
            {
                Contract.Ensures(Contract.Result<IJsonSerializer>() != null);
                return instance.InnerCurrent;
            }
        }

        public static void SetJsonSerializer(IJsonSerializer jsonSerializer)
        {
            Contract.Requires(jsonSerializer != null);
            instance.InnerSetApplication(jsonSerializer);
        }

        public static void SetJsonSerializer(Func<IJsonSerializer> getJsonSerializer)
        {
            Contract.Requires(getJsonSerializer != null);
            instance.InnerSetApplication(getJsonSerializer);
        }

        private IJsonSerializer current = new JsonNetSerializer();


        public IJsonSerializer InnerCurrent
        {
            get
            {
                Contract.Ensures(Contract.Result<IJsonSerializer>() != null);
                return this.current;
            }
        }

        public void InnerSetApplication(IJsonSerializer jsonSerializer)
        {
            Contract.Requires(jsonSerializer != null);
            this.current = jsonSerializer;
        }

        public void InnerSetApplication(Func<IJsonSerializer> getJsonSerializer)
        {
            Contract.Requires(getJsonSerializer != null);
            this.current = getJsonSerializer();
        }

        private class JsonNetSerializer : IJsonSerializer
        {
            private JsonSerializerSettings m_serializerSettings;

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

                this.m_serializerSettings = settings;
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
                return JsonConvert.SerializeObject(obj, Formatting.None, this.m_serializerSettings);
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
                        obj = JsonConvert.DeserializeObject(json, type, m_serializerSettings);
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
                    obj = JsonConvert.DeserializeObject(json, this.m_serializerSettings);
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
                (from childToken in token where childToken is JProperty select childToken as JProperty).ToList().ForEach(property =>
                {
                    jsonDict.Add(property.Name, ConvertJTokenToDictionary(property.Value));
                });
                return jsonObject;
            }
        }


    }
}
