//-----------------------------------------------------------------------
// <copyright file="FacebookClient.Batch.Async.cs" company="The Outercurve Foundation">
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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public partial class FacebookClient
    {
        private const string AtLeastOneBatchParameterRequried = "At least one batch parameter is required";
        private const string OnlyOneAttachmentAllowedPerBatchRequest = "Only one attachement (FacebookMediaObject/FacebookMediaStream) allowed per FacebookBatchParamter.";

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="batchParameters">The list of batch parameters.</param>
        /// <param name="userState">The user state.</param>
        /// <param name="parameters">The parameters.</param>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use BatchTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void BatchAsync(FacebookBatchParameter[] batchParameters, object userState, object parameters)
        {
            var actualParameter = PrepareBatchRequest(batchParameters, parameters);
            PostAsync(null, actualParameter, userState);
        }

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="batchParameters">The list of batch parameters.</param>
        /// <param name="userState">The user state.</param>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use BatchTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void BatchAsync(FacebookBatchParameter[] batchParameters, object userState)
        {
            BatchAsync(batchParameters, userState, null);
        }

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="batchParameters">The list of batch parameters.</param>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use BatchTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void BatchAsync(FacebookBatchParameter[] batchParameters)
        {
            BatchAsync(batchParameters, null);
        }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        internal object PrepareBatchRequest(FacebookBatchParameter[] batchParameters, object parameters)
        {
            if (batchParameters == null)
                throw new ArgumentNullException("batchParameters");
            if (batchParameters.Length == 0)
                throw new ArgumentNullException("batchParameters", AtLeastOneBatchParameterRequried);

            IDictionary<string, object> actualBatchParameter = new Dictionary<string, object>();
            IList<object> flatnedBatchParameters = new List<object>();
            actualBatchParameter["batch"] = flatnedBatchParameters;

            IDictionary<string, FacebookMediaObject> mediaObjects;
            IDictionary<string, FacebookMediaStream> mediaStreams;

            foreach (var batchParameter in batchParameters)
            {
                var data = ToDictionary(batchParameter.Data, out mediaObjects, out mediaStreams);

                if (mediaObjects.Count + mediaStreams.Count > 0)
                    throw new ArgumentException("Attachments (FacebookMediaObject/FacebookMediaStream) are only allowed in FacebookBatchParameter.Parameters");

                if (data == null)
                    data = new Dictionary<string, object>();

                if (!data.ContainsKey("method"))
                {
                    switch (batchParameter.HttpMethod)
                    {
                        case HttpMethod.Get:
                            data["method"] = "GET";
                            break;
                        case HttpMethod.Post:
                            data["method"] = "POST";
                            break;
                        case HttpMethod.Delete:
                            data["method"] = "DELETE";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                IList<string> attachedFiles = new List<string>();

                var pars = ToDictionary(batchParameter.Parameters, out mediaObjects, out mediaStreams) ?? new Dictionary<string, object>();
                bool containsEtag = false;
                string etag = null;
                if (pars.ContainsKey(ETagKey))
                {
                    etag = (string)pars[ETagKey];
                    containsEtag = true;
                    pars.Remove(ETagKey);
                }

                bool hasAttachmentInBatchParameter = false;

                foreach (var attachment in mediaObjects)
                {
                    if (hasAttachmentInBatchParameter)
                        throw new ArgumentException(OnlyOneAttachmentAllowedPerBatchRequest, "batchParameters");
                    if (actualBatchParameter.ContainsKey(attachment.Key))
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Attachment (FacebookMediaObject/FacebookMediaStream) with key '{0}' already exists", attachment.Key));
                    attachedFiles.Add(HttpHelper.UrlEncode(attachment.Key));
                    actualBatchParameter.Add(attachment.Key, attachment.Value);
                    hasAttachmentInBatchParameter = true;
                }

                foreach (var attachment in mediaStreams)
                {
                    if (hasAttachmentInBatchParameter)
                        throw new ArgumentException(OnlyOneAttachmentAllowedPerBatchRequest, "batchParameters");
                    if (actualBatchParameter.ContainsKey(attachment.Key))
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Attachment (FacebookMediaObject/FacebookMediaStream) with key '{0}' already exists", attachment.Key));
                    attachedFiles.Add(HttpHelper.UrlEncode(attachment.Key));
                    actualBatchParameter.Add(attachment.Key, attachment.Value);
                    hasAttachmentInBatchParameter = true;
                }

                if (attachedFiles.Count > 0 && !data.ContainsKey("attached_files"))
                    data["attached_files"] = string.Join(",", attachedFiles.ToArray());

                string path;
                if (!data["method"].ToString().Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    if (!data.ContainsKey("relative_url"))
                    {
                        path = ParseUrlQueryString(batchParameter.Path, pars, false);
                        SerializeParameters(pars);

                        var relativeUrl = new StringBuilder();
                        relativeUrl.Append(path).Append("?");
                        foreach (var kvp in pars)
                            relativeUrl.AppendFormat("{0}={1}&", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode)));
                        if (relativeUrl.Length > 0)
                            relativeUrl.Length--;
                        data["relative_url"] = relativeUrl.ToString();
                    }
                }
                else
                {
                    path = ParseUrlQueryString(batchParameter.Path, pars, false);
                    SerializeParameters(pars);

                    if (!data.ContainsKey("relative_url"))
                    {
                        if (path.Length > 0)
                            data["relative_url"] = path;
                    }

                    if (!data.ContainsKey("body"))
                    {
                        var sb = new StringBuilder();
                        foreach (var kvp in pars)
                            sb.AppendFormat("{0}={1}&", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode)));

                        if (sb.Length > 0)
                        {
                            sb.Length--;
                            data["body"] = sb.ToString();
                        }
                    }
                }

                if (containsEtag)
                    data[ETagKey] = etag;

                flatnedBatchParameters.Add(data);
            }

            var parametersAsDictionary = ToDictionary(parameters, out mediaObjects, out mediaStreams);
            if (parametersAsDictionary != null)
            {
                foreach (var parameter in parametersAsDictionary)
                {
                    actualBatchParameter[parameter.Key] = parameter.Value;
                }
            }

            foreach (var mediaObject in mediaObjects)
            {
                actualBatchParameter[mediaObject.Key] = mediaObject.Value;
            }

            foreach (var mediaStream in mediaStreams)
            {
                actualBatchParameter[mediaStream.Key] = mediaStream.Value;
            }

            return actualBatchParameter;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal object ProcessBatchResponse(object result, IList<int> batchEtags)
        {
            if (result == null)
                return null;

            var list = new JsonArray();
            var resultList = (IList<object>)result;

            int i = 0;
            foreach (var row in resultList)
            {
                if (row == null)
                {
                    // row is null when omit_response_on_success = true
                    list.Add(null);
                }
                else
                {
                    var batchResult = (IDictionary<string, object>)row;
                    var code = Convert.ToInt64(batchResult["code"], CultureInfo.InvariantCulture);

                    object bodyAsJsonObject = null;

                    try
                    {
                        if (batchEtags != null && batchEtags.Contains(i))
                        {
                            var jsonObject = new JsonObject();
                            var originalHeaders = (IList<object>)batchResult["headers"];
                            var headers = new JsonObject();
                            foreach (var originalHeader in originalHeaders)
                            {
                                var dict = (IDictionary<string, object>)originalHeader;
                                headers[(string)dict["name"]] = dict["value"];
                            }
                            jsonObject["headers"] = headers;

                            if (code != 304)
                            {
                                var body = (string)batchResult["body"];
                                jsonObject["body"] = bodyAsJsonObject = ProcessResponse(null, body, null, true, null);
                            }

                            bodyAsJsonObject = jsonObject;
                        }
                        else
                        {
                            var body = (string)batchResult["body"];
                            bodyAsJsonObject = ProcessResponse(null, body, null, false, null);
                        }

                        list.Add(bodyAsJsonObject);
                    }
                    catch (Exception ex)
                    {
                        list.Add(ex);
                    }
                }

                i++;
            }

            return list;
        }
    }
}