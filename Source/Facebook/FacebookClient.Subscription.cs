//-----------------------------------------------------------------------
// <copyright file="FacebookClient.Subscription.cs" company="The Outercurve Foundation">
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
    using System.Text;

    public partial class FacebookClient
    {
        private const string SubscriptionXHubSigntureRequestHeaderKey = "X-Hub-Signature";

        private const string SubscriptionHubChallengeKey = "hub.challenge";
        private const string SubscriptionHubVerifyTokenKey = "hub.verify_token";
        private const string SubscriptionHubModeKey = "hub.mode";

        private const string InvalidHttpXHubSignature = "Invalid " + SubscriptionXHubSigntureRequestHeaderKey + " request header";
        private const string InvalidHubChallenge = "Invalid " + SubscriptionHubChallengeKey;
        private const string InvalidVerifyToken = "Invalid " + SubscriptionHubVerifyTokenKey;
        private const string InvalidHubMode = "Invalid " + SubscriptionHubModeKey;

        /// <summary>
        /// Gets or sets the verify_token used in Facebook Realtime updates API.
        /// </summary>
        public virtual string SubscriptionVerifyToken { get; set; }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for HTTP GET.
        /// </summary>
        /// <param name="requestHubMode">The request hub.mode</param>
        /// <param name="requestVerifyToken">The request hub.verify_token</param>
        /// <param name="requestHubChallenge">The request hub.challenge</param>
        /// <param name="verifyToken">Expected verify token.</param>
        public virtual void VerifyGetSubscription(string requestHubMode, string requestVerifyToken, string requestHubChallenge, string verifyToken)
        {
            if (string.IsNullOrEmpty(verifyToken))
                throw new ArgumentNullException("verifyToken");

            if (requestHubMode == "subscribe")
            {
                if (requestVerifyToken == verifyToken)
                {
                    if (string.IsNullOrEmpty(requestHubChallenge))
                    {
                        throw new ArgumentException(InvalidHubChallenge, "requestHubChallenge");
                    }
                }
                else
                {
                    throw new ArgumentException(InvalidVerifyToken, requestVerifyToken);
                }
            }
            else
            {
                throw new ArgumentException(InvalidHubMode, "requestHubMode");
            }
        }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for HTTP GET.
        /// </summary>
        /// <param name="requestHubMode">The request hub.mode</param>
        /// <param name="requestVerifyToken">The request hub.verify_token</param>
        /// <param name="requestHubChallenge">The request hub.challenge</param>
        public virtual void VerifyGetSubscription(string requestHubMode, string requestVerifyToken, string requestHubChallenge)
        {
            VerifyGetSubscription(requestHubMode, requestVerifyToken, requestHubChallenge, SubscriptionVerifyToken);
        }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for HTTP POST.
        /// </summary>
        /// <param name="requestHttpXHubSignature">The request HTTP_X_HUB_SIGNATURE</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="resultType">The result type.</param>
        /// <param name="appSecret">The App secret.</param>
        public virtual object VerifyPostSubscription(string requestHttpXHubSignature, string requestBody, Type resultType, string appSecret)
        {
            // httpXHubSignature looks somewhat like "sha1=4594ae916543cece9de48e3289a5ab568f514b6a"

            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");

            if (!string.IsNullOrEmpty(requestHttpXHubSignature) && requestHttpXHubSignature.StartsWith("sha1="))
            {
                var expectedSha1 = requestHttpXHubSignature.Substring(5);

                if (string.IsNullOrEmpty(expectedSha1))
                {
                    throw new ArgumentException(InvalidHttpXHubSignature, requestHttpXHubSignature);
                }
                else
                {
                    if (string.IsNullOrEmpty(requestBody))
                    {
                        throw new ArgumentException(requestBody, "requestBody");
                    }

                    var sha1 = ComputeHmacSha1Hash(Encoding.UTF8.GetBytes(requestBody), Encoding.UTF8.GetBytes(appSecret));

                    var hashString = new StringBuilder();
                    foreach (var b in sha1)
                    {
                        hashString.Append(b.ToString("x2"));
                    }

                    if (expectedSha1 == hashString.ToString())
                    {
                        return DeserializeJson(requestBody, resultType);
                    }

                    throw new ArgumentException(InvalidHttpXHubSignature, "requestHttpXHubSignature");
                }
            }
            else
            {
                throw new ArgumentException(InvalidHttpXHubSignature, requestHttpXHubSignature);
            }
        }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for HTTP POST.
        /// </summary>
        /// <param name="requestHttpXHubSignature">The request HTTP_X_HUB_SIGNATURE</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="resultType">The result type.</param>
        public object VerifyPostSubscription(string requestHttpXHubSignature, string requestBody, Type resultType)
        {
            return VerifyPostSubscription(requestHttpXHubSignature, requestBody, resultType, AppSecret);
        }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for HTTP POST.
        /// </summary>
        /// <param name="requestHttpXHubSignature">The request HTTP_X_HUB_SIGNATURE</param>
        /// <param name="requestBody">The request body.</param>
        public object VerifyPostSubscription(string requestHttpXHubSignature, string requestBody)
        {
            return VerifyPostSubscription(requestHttpXHubSignature, requestBody, null, AppSecret);
        }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for HTTP POST.
        /// </summary>
        /// <param name="requestHttpXHubSignature">The request HTTP_X_HUB_SIGNATURE</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="appSecret">The App secret.</param>
        public virtual object VerifyPostSubscription(string requestHttpXHubSignature, string requestBody, string appSecret)
        {
            return VerifyPostSubscription(requestHttpXHubSignature, requestBody, null, appSecret);
        }

        private static byte[] ComputeHmacSha1Hash(byte[] data, byte[] key)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (key == null)
                throw new ArgumentNullException("key");

            using (var crypto = new System.Security.Cryptography.HMACSHA1(key))
            {
                return crypto.ComputeHash(data);
            }
        }
    }
}