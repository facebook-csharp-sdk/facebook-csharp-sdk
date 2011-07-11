// --------------------------------
// <copyright file="JsonSerializer.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System.Diagnostics.Contracts;

namespace Facebook
{
    using System;

    public class JsonSerializer
    {
        private static readonly JsonSerializer _instance = new JsonSerializer();

        public static IJsonSerializer Current
        {
            get
            {
                Contract.Ensures(Contract.Result<IJsonSerializer>() != null);
                return _instance.InnerCurrent;
            }
        }

        public static void SetJsonSerializer(IJsonSerializer jsonSerializer)
        {
            _instance.InnerSetApplication(jsonSerializer);
        }

        public static void SetJsonSerializer(Func<IJsonSerializer> getJsonSerializer)
        {
            Contract.Requires(getJsonSerializer != null);
            _instance.InnerSetApplication(getJsonSerializer);
        }

        private IJsonSerializer _current = new SimpleJsonSerializer();

        public IJsonSerializer InnerCurrent
        {
            get
            {
                Contract.Ensures(Contract.Result<IJsonSerializer>() != null);
                return _current;
            }
        }

        public void InnerSetApplication(IJsonSerializer jsonSerializer)
        {
            _current = jsonSerializer ?? new SimpleJsonSerializer();
        }

        public void InnerSetApplication(Func<IJsonSerializer> getJsonSerializer)
        {
            Contract.Requires(getJsonSerializer != null);
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
