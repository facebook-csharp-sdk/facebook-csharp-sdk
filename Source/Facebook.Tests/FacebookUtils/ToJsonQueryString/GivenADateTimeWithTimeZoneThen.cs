//-----------------------------------------------------------------------
// <copyright file="<file>.cs" company="The Outercurve Foundation">
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

namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using System;
    using Facebook;
    using Xunit;

    public class GivenADateTimeWithTimeZoneThen
    {
#if !SILVERLIGHT

        [Fact(Skip = "in bangkok, thailand the result is +07:00 and thus fails")]
        public void ShouldSerializeItCorrectlyToISO8601DateTimeFormat()
        {
            // We create the datetime this way so that this test passes in all time zones
            var dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(2010, 6, 15, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Utc.Id, "Central Standard Time");
            var result = JsonSerializer.Current.SerializeObject(dateTime);

            // TODO: fix - in bangkok, thailand the result is +07:00 and thus fails
            Assert.Equal("\"2010-06-14T19:00:00-04:00\"", result);
        }
#endif
    }
}