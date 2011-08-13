<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignedRequest.ascx.cs" Inherits="CS_Canvas_AspNetWebForms_WithoutJsSdk.Facebook.SignedRequest" %>
<%-- 
    This user control is used for maintaining the signed_request during post backs. 
    When using Facebook Javascript SDK you don't require this user control as the Javascript SDK
    sets a special Facebook cookie which is always transmitted along with any post backs.
    
    you will also need to manually maintain signed_request for ajax requests.
--%>
<% if (!string.IsNullOrEmpty(Request.Params["signed_request"])) { %>
    <input type="hidden" name="signed_request" value="<%= Request.Params["signed_request"] %>" />
<% } %>