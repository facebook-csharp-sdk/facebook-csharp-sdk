// --------------------------------
// <copyright file="IFacebookSettings.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
namespace Facebook
{
    public interface IFacebookSettings
    {

        string ApiKey { get; set; }
        string ApiSecret { get; set; }
        string AppId { get; set; }
        bool CookieSupport { get; set; }
        string BaseDomain { get; set; }
        int MaxRetries { get; set; }
        int RetryDelay { get; set; }

    }
}
