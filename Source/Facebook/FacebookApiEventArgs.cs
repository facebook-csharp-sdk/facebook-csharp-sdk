using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Facebook
{
    public class FacebookApiEventArgs : AsyncCompletedEventArgs
    {

        private string m_json;

        public FacebookApiEventArgs(Exception error, bool cancelled, object userState, string json)
            : base(error, cancelled, userState)
        {
            this.m_json = json;
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
