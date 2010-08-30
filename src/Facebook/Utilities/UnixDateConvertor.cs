// --------------------------------
// <copyright file="UnixDateConvertor.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Facebook.Utilities
{
    internal class UnixDateConvertor : DateTimeConverterBase
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1"), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var isNullableType = objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>);
            // Type t = isNullableType ? Nullable.GetUnderlyingType(objectType) : objectType;

            if (reader.TokenType == JsonToken.Null)
            {
                if (!isNullableType)
                    throw new JsonReaderException(string.Format("Cannot convert null value to {0}.", objectType));

                return null;
            }

            // if (reader.TokenType != JsonToken.StartConstructor || string.Compare(reader.Value.ToString(), "Date", StringComparison.Ordinal) != 0)
            //    throw new Exception(string.Format("Unexpected token or value when parsing date. Token: {0}, Value: {1}", reader.TokenType, reader.Value));

            // reader.Read();

            if (reader.TokenType != JsonToken.Integer)
                throw new JsonReaderException(string.Format("Unexpected token parsing date. Expected Integer, got {0}.", reader.TokenType));

            DateTime d = UnixDateTime.FromUnixTime(reader.Value.ToString());

            // reader.Read();

            // if (reader.TokenType != JsonToken.EndConstructor)
            //    throw new Exception(string.Format("Unexpected token parsing date. Expected EndConstructor, got {0}.", reader.TokenType));

            return d;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            string ticks;

            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                DateTime utcDateTime = dateTime.ToUniversalTime();
                ticks = UnixDateTime.ToUnixTime(utcDateTime);
            }
            else
            {
                throw new JsonWriterException("Expected date object value.");
            }

            writer.WriteStartConstructor("Integer");
            writer.WriteValue(ticks);
            writer.WriteEndConstructor();
        }

    }
}
