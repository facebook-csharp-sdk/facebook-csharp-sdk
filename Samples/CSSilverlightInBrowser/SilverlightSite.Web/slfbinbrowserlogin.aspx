<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Facebook" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
    protected string AccessToken { get; set; }
    protected string ErrorDescription { get; set; }

    private void Page_Load(object sender, EventArgs e)
    {
        var url = HttpContext.Current.Request.Url;
        FacebookOAuthResult authResult;

        if (FacebookOAuthResult.TryParse(url, out authResult))
        {
            if (authResult.IsSuccess)
            {
                var oauth = new FacebookOAuthClient
                                {
                                    ClientId = "{appid}",
                                    ClientSecret = "{app secret}",
                                    RedirectUri = new Uri("http://localhost/fbslinbrowser/slfbinbrowserlogin.aspx")
                                };
                var result = (IDictionary<string, object>)oauth.ExchangeCodeForAccessToken(authResult.Code, null);
                this.AccessToken = result["access_token"].ToString();
            }
            else
            {
                this.ErrorDescription = authResult.ErrorDescription;
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
        else
        {
            Response.Redirect("~/");
        }
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    </form>
    <script type="text/javascript">
        window.opener.LoginComplete('<%: this.AccessToken %>', '<%: this.ErrorDescription %>');
    </script>
</body>
</html>
