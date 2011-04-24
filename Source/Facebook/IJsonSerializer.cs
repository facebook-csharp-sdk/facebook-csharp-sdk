// --------------------------------
// <copyright file="IJsonSerializer.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;

    public interface IJsonSerializer
    {
        string SerializeObject(object obj);
        object DeserializeObject(string json, Type type);
        T DeserializeObject<T>(string json);
        object DeserializeObject(string json);
    }
}
