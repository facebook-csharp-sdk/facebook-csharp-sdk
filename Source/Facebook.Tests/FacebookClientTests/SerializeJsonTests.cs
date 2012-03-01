﻿//-----------------------------------------------------------------------
// <copyright file="SerializeJsonTests.cs" company="Thuzi LLC (www.thuzi.com)">
//    Copyright 2011
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

namespace Facebook.Tests.FacebookClient
{
    using System;
    using Facebook;
    using Xunit;

    public class SerializeJsonTests
    {
        [Fact]
        public void SerializeBigNumbersCorrectly()
        {
            string json = SimpleJson.SerializeObject(new { object_id = 10150098461530576 });

            Assert.Equal("{\"object_id\":10150098461530576}", json);
        }
    }
}
