// --------------------------------
// <copyright file="FacebookMediaObject.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

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
