namespace Facebook
{
    using System;
    using System.ComponentModel;

    public class FacebookApiEventArgs : AsyncCompletedEventArgs
    {
        private string m_json;

        public FacebookApiEventArgs(Exception error, bool cancelled, object userState, string json)
            : base(error, cancelled, userState)
        {
            // check for error coz if its is rest api, json is not null
            if (error == null)
            {
                this.m_json = json;
            }
        }

        public object GetResultData()
        {
            return JsonSerializer.Current.DeserializeObject(this.m_json);
        }

        public T GetResultData<T>()
        {
            return JsonSerializer.Current.DeserializeObject<T>(this.m_json);
        }

    }
}
