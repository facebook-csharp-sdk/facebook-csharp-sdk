<%@ Control Language="C#" Inherits="Facebook.Web.Mvc.ViewUserControl<dynamic>" %>
<fb:profile-pic uid="<%=ViewData["UserId"] %>" facebook-logo="false" size="thumb"> </fb:profile-pic>
<h2>
    <fb:name uid="<%=ViewData["UserId"] %>" useyou="false"></fb:name>
</h2>
<p>
    <a href="#">Settings</a> | 
    <a href="#" onclick="FB.logout()">Logout</a>
</p>
