// --------------------------------
// <copyright file="DynamicUtilities.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;

namespace Facebook.Utilities
{
    internal static class DynamicHelper
    {

        /// <summary>
        /// Merges two ExpandoObjects.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static dynamic Merge(dynamic first, dynamic second)
        {
            if (first != null && !(first is IDictionary<string, object>))
            {
                throw new ArgumentException("The argument 'first' must impliment IDictionary<string, object>.");
            }
            if (second != null && !(second is IDictionary<string, object>))
            {
                throw new ArgumentException("The argument 'second' must impliment IDictionary<string, object>.");
            }
            Contract.EndContractBlock();

            first = first ?? new ExpandoObject();
            second = second ?? new ExpandoObject();

            var firstDict = (IDictionary<string, object>)first;
            var secondDict = (IDictionary<string, object>)second;
            var merged = new DynamicDictionary();
            var mergedDict = (IDictionary<string, object>)merged;
            foreach (var property in firstDict)
            {
                mergedDict.Add(property);
            }
            foreach (var property in secondDict)
            {
                mergedDict[property.Key] = secondDict[property.Key];
            }
            return merged;
        }
    }
}
