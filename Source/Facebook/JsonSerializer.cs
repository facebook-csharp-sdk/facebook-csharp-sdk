// --------------------------------
// <copyright file="JsonSerializer.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;

    /// <summary>
    /// Represents the json serializer class.
    /// </summary>
    public class JsonSerializer
    {
        /// <summary>
        /// The current instance of the json serializer.
        /// </summary>
        private static readonly JsonSerializer Instance = new JsonSerializer();

        /// <summary>
        /// Gets the current json serializer.
        /// </summary>
        public static IJsonSerializer Current
        {
            get { return Instance.InnerCurrent; }
        }

        /// <summary>
        /// Sets the json serializer.
        /// </summary>
        /// <param name="jsonSerializer"></param>
        public static void SetJsonSerializer(IJsonSerializer jsonSerializer)
        {
            Instance.InnerSetApplication(jsonSerializer);
        }

        /// <summary>
        /// Sets the json serializer.
        /// </summary>
        /// <param name="getJsonSerializer"></param>
        public static void SetJsonSerializer(Func<IJsonSerializer> getJsonSerializer)
        {
            Instance.InnerSetApplication(getJsonSerializer);
        }

        private IJsonSerializer _current = new SimpleJsonSerializer();

        /// <summary>
        /// The inner current json serializer.
        /// </summary>
        public IJsonSerializer InnerCurrent
        {
            get { return _current; }
        }

        /// <summary>
        /// Sets the inner application.
        /// </summary>
        /// <param name="jsonSerializer">The json serializer.</param>
        public void InnerSetApplication(IJsonSerializer jsonSerializer)
        {
            _current = jsonSerializer ?? new SimpleJsonSerializer();
        }

        /// <summary>
        /// Sets the inner application.
        /// </summary>
        /// <param name="getJsonSerializer">The json serializer.</param>
        public void InnerSetApplication(Func<IJsonSerializer> getJsonSerializer)
        {
            if (getJsonSerializer == null)
                throw new ArgumentNullException("getJsonSerializer");

            InnerSetApplication(getJsonSerializer());
        }

        private class SimpleJsonSerializer : IJsonSerializer
        {
            public string SerializeObject(object obj)
            {
                return SimpleJson.SimpleJson.SerializeObject(obj);
            }

            public object DeserializeObject(string json, Type type)
            {
                return SimpleJson.SimpleJson.DeserializeObject(json, type);
            }

            public T DeserializeObject<T>(string json)
            {
                return SimpleJson.SimpleJson.DeserializeObject<T>(json);
            }

            public object DeserializeObject(string json)
            {
                return SimpleJson.SimpleJson.DeserializeObject(json);
            }
        }
    }
}
