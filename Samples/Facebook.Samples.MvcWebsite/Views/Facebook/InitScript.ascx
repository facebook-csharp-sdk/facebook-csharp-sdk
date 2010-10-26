<%@ Control Language="C#" Inherits="Facebook.Web.Mvc.ViewUserControl<dynamic>" %>
<!-- Facebook -->
<div id="fb-root">
</div>
<script type="text/javascript" src="http://connect.facebook.net/en_US/all.js"></script>
<% if (false)
   { %>
<script src="../../Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<% } %>
<script type="text/javascript">
    FB.init({ appId: '<%=Model.AppId %>',
        status: true, cookie: <%=Model.CookieSupport.ToString().ToLower() %>, xfbml: true
    });

    // fetch the status on load
    FB.getLoginStatus(function(response) {
        if (response.session) {
            // user is logged in
        } else {
            // user is not logged in
        }
    });

    FB.Event.subscribe('auth.login', function(response) {
       if (response.session) {
            if (response.perms) {
                // user is logged in and granted some permissions.
                // perms is a comma separated list of granted permissions
            } else {
                // user is logged in, but did not grant any permissions
            }
            window.location = '/profile';
        } else {
            // user is not logged in
        }
    });

    FB.Event.subscribe('auth.logout', function(response) {
            window.location = "/";
    });
</script>
