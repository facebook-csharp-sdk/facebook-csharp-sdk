namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using FluentHttp;

    public partial class FacebookClient
    {
        private const string AtLeastOneBatchParameterRequried = "At least one batch parameter is required";
        private const string OnlyOneAttachmentAllowedPerBatchRequest = "Only one attachement (FacebookMediaObject/FacebookMediaStream) allowed per FacebookBatchParamter.";

        public virtual void BatchAsync(FacebookBatchParameter[] batchParameters, object userToken)
        {
            throw new NotImplementedException();
        }

        public virtual void BatchAsync(FacebookBatchParameter[] batchParameters)
        {
            var fb = new FacebookClient();

            throw new NotImplementedException();
        }

        private object PrepareBatchRequest(FacebookBatchParameter[] batchParameters)
        {
            if (batchParameters == null)
                throw new ArgumentNullException("batchParameters");
            if (batchParameters.Length == 0)
                throw new ArgumentNullException("batchParameters", AtLeastOneBatchParameterRequried);

            IDictionary<string, object> actualBatchParameter = new Dictionary<string, object>();
            IList<object> flatnedBatchParameters = new List<object>();
            actualBatchParameter["batch"] = flatnedBatchParameters;

            foreach (var batchParameter in batchParameters)
            {
                IDictionary<string, FacebookMediaObject> mediaObjects;
                IDictionary<string, FacebookMediaStream> mediaStreams;

                var data = ToDictionary(batchParameter.Data, out mediaObjects, out mediaStreams);

                if (mediaObjects.Count + mediaStreams.Count > 0)
                    throw new ArgumentException("Attachments (FacebookMediaObject/FacebookMediaStream) are only allowed in FacebookBatchParameter.Parameters");

                if (data == null)
                    data = new Dictionary<string, object>();

                if (!data.ContainsKey("method"))
                    data["method"] = batchParameter.HttpMethod;

                IList<string> attachedFiles = new List<string>();

                var parameters = ToDictionary(batchParameter.Parameters, out mediaObjects, out mediaStreams);

                bool hasAttachmentInBatchParameter = false;

                foreach (var attachment in mediaObjects)
                {
                    if (hasAttachmentInBatchParameter)
                        throw new ArgumentException(OnlyOneAttachmentAllowedPerBatchRequest, "batchParameters");
                    if (actualBatchParameter.ContainsKey(attachment.Key))
                        throw new ArgumentException(string.Format("Attachment (FacebookMediaObject/FacebookMediaStream) with key '{0}' already exists", attachment.Key));
                    attachedFiles.Add(HttpHelper.UrlEncode(attachment.Key));
                    actualBatchParameter.Add(attachment.Key, attachment.Value);
                    hasAttachmentInBatchParameter = true;
                }

                foreach (var attachment in mediaStreams)
                {
                    if (hasAttachmentInBatchParameter)
                        throw new ArgumentException(OnlyOneAttachmentAllowedPerBatchRequest, "batchParameters");
                    if (actualBatchParameter.ContainsKey(attachment.Key))
                        throw new ArgumentException(string.Format("Attachment (FacebookMediaObject/FacebookMediaStream) with key '{0}' already exists", attachment.Key));
                    attachedFiles.Add(HttpHelper.UrlEncode(attachment.Key));
                    actualBatchParameter.Add(attachment.Key, attachment.Value);
                    hasAttachmentInBatchParameter = true;
                }

                if (attachedFiles.Count > 0 && !data.ContainsKey("attached_files"))
                    data["attached_files"] = string.Join(",", attachedFiles);

                string path;
                if (!data["method"].ToString().Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    if (!data.ContainsKey("relative_url"))
                    {
                        path = ParseUrlQueryString(batchParameter.Path, parameters, false);
                        var relativeUrl = new StringBuilder();
                        relativeUrl.Append(path).Append("?");
                        foreach (var kvp in parameters)
                            relativeUrl.AppendFormat("{0}={1}&", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode)));
                        if (relativeUrl.Length > 0)
                            relativeUrl.Length--;
                        data["relative_url"] = relativeUrl;
                    }

                }
                else
                {
                    path = ParseUrlQueryString(batchParameter.Path, parameters, false);
                    if (!data.ContainsKey("relative_url"))
                    {
                        if (path.Length > 0)
                            data["relative_url"] = path;
                    }

                    if (!data.ContainsKey("body"))
                    {
                        var sb = new StringBuilder();
                        foreach (var kvp in parameters)
                            sb.AppendFormat("{0}={1}", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode)));
                        if (sb.Length > 0)
                            data["body"] = sb.ToString();
                    }
                }

                flatnedBatchParameters.Add(data);
            }

            return actualBatchParameter;
        }
    }
}