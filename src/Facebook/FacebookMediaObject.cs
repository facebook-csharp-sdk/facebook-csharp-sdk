
namespace Facebook
{
    /// <summary>
    /// Represents a media object such as a photo or video.
    /// </summary>
    public class FacebookMediaObject
    {
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
        public void SetValue(byte[] value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value of the media object.
        /// </summary>
        /// <returns></returns>
        public byte[] GetValue()
        {
            return _value;
        }
    }
}
