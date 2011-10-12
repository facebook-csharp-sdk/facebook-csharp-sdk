using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using CS_AspNetMvc3_WithoutJsSdk.Models;
using Facebook;

namespace CS_AspNetMvc3_WithoutJsSdk.Controllers
{
    public class AccountController : Controller
    {
        private const string ExtendedPermissions = "user_about_me,publish_stream,offline_access";

        //
        // GET: /Account/LogOn

        public ActionResult LogOn(string returnUrl)
        {
            var oauthClient = new FacebookOAuthClient(FacebookApplication.Current) { RedirectUri = GetFacebookRedirectUri() };


            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index", "Facebook");
            }

            dynamic parameters = new ExpandoObject();
            parameters.scope = ExtendedPermissions;

            // add csrf_token to prevent cross site forger attacks
            // pass returnUrl as state, so the callback know which page to redirect when the oauth login is successful
            // to make the querystring ?state=value safe, encode the value of state using Base64UrlEncode.
            var state = new { csrf_token = CalculateMD5Hash(Guid.NewGuid().ToString()), return_url = returnUrl };
            parameters.state = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Current.SerializeObject(state)));
            SetFacebookCsrfToken(state.csrf_token);
            ViewBag.FacebookLoginUrl = oauthClient.GetLoginUrl(parameters).AbsoluteUri;

            return View();
        }

        public ActionResult FacebookCallback()
        {
            var oauthClient = new FacebookOAuthClient(FacebookApplication.Current) { RedirectUri = GetFacebookRedirectUri() };

            FacebookOAuthResult oAuthResult;
            if (oauthClient.TryParseResult(Request.Url, out oAuthResult))
            {
                if (oAuthResult.IsSuccess)
                {
                    if (!string.IsNullOrWhiteSpace(oAuthResult.Code))
                    {
                        string returnUrl = null;
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(oAuthResult.State))
                            {
                                dynamic state = JsonSerializer.Current.DeserializeObject(Encoding.UTF8.GetString(Base64UrlDecode(oAuthResult.State)));
                                if (!state.ContainsKey("csrf_token") || !ValidateFacebookCsrfToken(state.csrf_token))
                                {
                                    // someone tried to hack the site.
                                    return RedirectToAction("Index", "Home");
                                }

                                if (state.ContainsKey("return_url") && !string.IsNullOrWhiteSpace(state.return_url))
                                {
                                    returnUrl = Encoding.UTF8.GetString(Base64UrlDecode(oAuthResult.State));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // catch incase user puts his own state, 
                            // Base64UrlDecode might throw exception if the value is not properly encoded.
                            return RedirectToAction("Index", "Home");
                        }

                        try
                        {
                            var result = (IDictionary<string, object>)oauthClient.ExchangeCodeForAccessToken(oAuthResult.Code);

                            ProcessSuccesfulFacebookCallback(result);

                            // prevent open redirection attacks. make sure the returnUrl is trusted before redirecting to it
                            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Facebook");
                            }

                        }
                        catch (FacebookApiException)
                        {
                            // catch incase the user entered dummy code or the code expired.
                        }
                    }

                    return Redirect("~/");
                }

                return View("FacebookCallbackError", oAuthResult);
            }

            return Redirect("~/");
        }

        private void ProcessSuccesfulFacebookCallback(IDictionary<string, object> result)
        {
            string accessToken = (string)result["access_token"];

            // incase the expires on is not present, it means we have offline_access permission
            DateTime expiresOn = result.ContainsKey("expires") ? DateTime.UtcNow.AddSeconds(Convert.ToDouble(result["expires"])) : DateTime.MaxValue;

            var fb = new FacebookClient(accessToken);
            dynamic me = fb.Get("me?fields=id,name");
            string id = me.id;
            string name = me.name;

            // do your custom logic to store the user here
            InMemoryUserStore.AddOrUpdate(new FacebookUser { AccessToken = accessToken, FacebookId = id, Name = name });

            // set the forms auth cookie
            FormsAuthentication.SetAuthCookie(id, false);
        }

        private void SetFacebookCsrfToken(string csrfToken)
        {
            Session["fb_csrf_token"] = csrfToken;
        }

        private bool ValidateFacebookCsrfToken(string csrfToken)
        {
            var result = Session["fb_csrf_token"] != null && (string)Session["fb_csrf_token"] == csrfToken;
            Session["fb_csrf_token"] = null;
            return result;
        }

        private Uri GetFacebookRedirectUri()
        {
            return new Uri(Url.Action("FacebookCallback", "Account", null, Request.Url.Scheme));
        }

        #region Base64 Url Decoding/Encoding

        /// <summary>
        /// Base64 Url decode.
        /// </summary>
        /// <param name="base64UrlSafeString">
        /// The base 64 url safe string.
        /// </param>
        /// <returns>
        /// The base 64 url decoded string.
        /// </returns>
        private static byte[] Base64UrlDecode(string base64UrlSafeString)
        {
            if (string.IsNullOrEmpty(base64UrlSafeString))
                throw new ArgumentNullException("base64UrlSafeString");

            base64UrlSafeString =
                base64UrlSafeString.PadRight(base64UrlSafeString.Length + (4 - base64UrlSafeString.Length % 4) % 4, '=');
            base64UrlSafeString = base64UrlSafeString.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(base64UrlSafeString);
        }

        /// <summary>
        /// Base64 url encode.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// Base64 url encoded string.
        /// </returns>
        private static string Base64UrlEncode(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            return Convert.ToBase64String(input).Replace("=", String.Empty).Replace('+', '-').Replace('/', '_');
        }

        #endregion

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
