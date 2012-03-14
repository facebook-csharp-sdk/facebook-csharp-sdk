//-----------------------------------------------------------------------
// <copyright file="FacebookMediaObject.cs" company="The Outercurve Foundation">
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

namespace Facebook
{
    /// <summary>
    /// Represents a media object such as a photo or video.
    /// </summary>
    public class FacebookMediaObject
    {
        /// <summary>
        /// The value of the media object.
        /// </summary>
        private byte[] _value;

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Sets the value of the media object.
        /// </summary>
        /// <param name="value">The media object value.</param>
        /// <returns>Facebook Media Object</returns>
        public FacebookMediaObject SetValue(byte[] value)
        {
            _value = value;
            return this;
        }

        /// <summary>
        /// Gets the value of the media object.
        /// </summary>
        /// <returns>The value of the media object.</returns>
        public byte[] GetValue()
        {
            return _value;
        }
    }
}
