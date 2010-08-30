// --------------------------------
// <copyright file="FacebookMapper.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Api
{
    public static class FacebookMapper
    {
        public static TModel MapToModel<TModel>(dynamic value)
            where TModel : class, new()
        {
            TModel schemaObject = new TModel();
            var properties = typeof(TModel).GetProperties();
            IDictionary<string, object> valueDict = (IDictionary<string, object>)value;
            foreach (var prop in properties)
            {
                if (valueDict.ContainsKey(FromProperName(prop.Name)))
                {
                    prop.SetValue(schemaObject, valueDict[prop.Name], null);
                }
            }
            return schemaObject;
        }

        public static TModel Map<TSchema, TModel>(dynamic value)
            where TSchema : class, new()
            where TModel : class, new()
        {
            TSchema shemaObject = Map<TSchema>(value);
            var typeMap = AutoMapper.Mapper.FindTypeMapFor<TSchema, TModel>();
            if (typeMap == null)
            {

            }
            TModel modelObject = AutoMapper.Mapper.Map<TSchema, TModel>(shemaObject);
            return modelObject; 
        }

        public static TSchema Map<TSchema>(dynamic value)
            where TSchema : class, new()
        {
            TSchema schemaObject = new TSchema();
            var properties = typeof(TSchema).GetProperties();
            IDictionary<string, object> valueDict = (IDictionary<string, object>)value;
            foreach (var prop in properties)
            {
                if (valueDict.ContainsKey(prop.Name))
                {
                    prop.SetValue(schemaObject, valueDict[prop.Name], null);
                }
            }
            return schemaObject;
        }

        public static string FromProperName(string properName, string parentName = null)
        {
            throw new NotImplementedException();
        }

        public static string ToProperName(string name, string parentName = null)
        {
            name = CleanName(name);
            string[] words = name.Split('_');
            StringBuilder finalName = new StringBuilder();
            foreach (var word in words)
            {
                char[] chars = word.ToCharArray();

                for (int i = 0; i < chars.Length; i++)
                {
                    string letter = chars[i].ToString().ToLower();
                    if (i == 0)
                        letter = letter.ToUpper();
                    finalName.Append(letter);
                }
            }
            var result = finalName.ToString();
            if (parentName == null)
            {
                if (result.EndsWith("s") && !result.EndsWith("us"))
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }
            if (result == parentName)
            {
                result = "Value";
            }
            return result;
        }

        public static string CleanName(string name)
        {
            // Check to make sure the word isnt some weird multiproperty??
            if (name.Contains("|"))
            {
                var w = name.Split('|');
                name = w[1].Replace("]", string.Empty).Trim();
            }
            return name;
        }


    }
}
