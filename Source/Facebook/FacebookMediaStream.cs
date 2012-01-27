// --------------------------------
// <copyright file="FacebookMediaStream.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.IO;

    /// <summary>
    /// Represents a media stream such as a photo or a video.
    /// </summary>
    public class FacebookMediaStream : IDisposable
    {
        /// <summary>
        /// The value of the media stream.
        /// </summary>
        private Stream _value;

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
        /// Sets the value of the media stream.
        /// </summary>
        /// <param name="value">The media stream value.</param>
        /// <returns>Facebook Media Stream</returns>
        public FacebookMediaStream SetValue(Stream value)
        {
            _value = value;
            return this;
        }

        /// <summary>
        /// Gets the value of the media stream.
        /// </summary>
        /// <returns>The value of the media stream.</returns>
        public Stream GetValue()
        {
            return _value;
        }

        public void Dispose()
        {
            var stream = GetValue();
            if (stream != null)
                stream.Dispose();
        }
    }
}