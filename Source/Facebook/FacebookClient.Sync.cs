﻿//-----------------------------------------------------------------------
// <copyright file="FacebookClient.Sync.cs" company="The Outercurve Foundation">
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
    using System.IO;
    using System.Net;

    public partial class FacebookClient
    {
        /// <summary>
        /// Makes a request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <returns>The json result with headers.</returns>
        protected virtual object Api(HttpMethod httpMethod, string path, object parameters, Type resultType)
        {
            Stream input;
            bool containsEtag;
            IList<int> batchEtags;
            var httpHelper = PrepareRequest(httpMethod, path, parameters, resultType, out input, out containsEtag, out  batchEtags);

            if (input != null)
            {
                try
                {
                    using (var stream = httpHelper.OpenWrite())
                    {
                        // write input to requestStream
                        var buffer = new byte[BufferSize];
                        while (true)
                        {
                            int bytesRead = input.Read(buffer, 0, buffer.Length);
                            input.Flush();
                            if (bytesRead <= 0) break;
                            stream.Write(buffer, 0, bytesRead);
                            stream.Flush();
                        }
                    }
                }
                catch (WebExceptionWrapper ex)
                {
                    if (ex.GetResponse() == null) throw;
                }
                finally
                {
                    input.Dispose();
                }
            }

            Stream responseStream = null;
            object result = null;
            bool read = false;
            try
            {
                responseStream = httpHelper.OpenRead();
                read = true;
            }
            catch (WebExceptionWrapper ex)
            {
                var response = ex.GetResponse();
                if (response == null) throw;
                if (response.StatusCode == HttpStatusCode.NotModified)
                {
                    var jsonObject = new JsonObject();
                    var headers = new JsonObject();

                    foreach (var headerName in response.Headers.AllKeys)
                        headers[headerName] = response.Headers[headerName];

                    jsonObject["headers"] = headers;
                    result = jsonObject;
                }
                else
                {
                    responseStream = httpHelper.OpenRead();
                    read = true;
                }
            }
            finally
            {
                if (read)
                {
                    string responseString;
                    using (var stream = responseStream)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            responseString = reader.ReadToEnd();
                        }
                    }

                    result = ProcessResponse(httpHelper, responseString, resultType, containsEtag, batchEtags);
                }
            }

            return result;
        }

        public virtual object Get(string path)
        {
            return Get(path, null);
        }

        public virtual object Get(object parameters)
        {
            return Get(null, parameters);
        }

        public virtual object Get(string path, object parameters)
        {
            return Api(HttpMethod.Get, path, parameters, null);
        }

        public virtual object Post(object parameters)
        {
            return Post(null, parameters);
        }

        public virtual object Post(string path, object parameters)
        {
            return Api(HttpMethod.Post, path, parameters, null);
        }

        public virtual object Delete(string path)
        {
            return Delete(path, null);
        }

        public virtual object Delete(string path, object parameters)
        {
            return Api(HttpMethod.Delete, path, parameters, null);
        }
    }
}