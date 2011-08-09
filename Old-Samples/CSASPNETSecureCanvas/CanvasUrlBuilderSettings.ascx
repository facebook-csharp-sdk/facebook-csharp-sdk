<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CanvasUrlBuilderSettings.ascx.cs" Inherits="CSASPNETSecureCanvas.CanvasUrlBuilderSettings" %>

<table>
    <tr>
        <td>Canvas Page Application Path</td>
        <td><%: CanvasUrlBuilder.CanvasPageApplicationPath %></td>
    </tr>
    <tr>
        <td>Canvas Page</td>
        <td><%: CanvasUrlBuilder.CanvasPage %></td>
    </tr>
    <tr>
        <td>Canvas Url</td>
        <td><%:CanvasUrlBuilder.CanvasUrl %></td>
    </tr>
    <tr>
        <td>Secure Canvas Url</td>
        <td><%:CanvasUrlBuilder.SecureCanvasUrl %></td>
    </tr>
    <tr>
        <td>Current Canvas Url</td>
        <td><%: CanvasUrlBuilder.CurrentCanvasUrl %></td>
    </tr>
    <tr>
        <td>Current Canvas Page</td>
        <td><%: CanvasUrlBuilder.CurrentCanvasPage %></td>
    </tr>
    <tr>
        <td>Current Canvas Path and Query</td>
        <td><%: CanvasUrlBuilder.CurrentCanvasPathAndQuery %></td>
    </tr>
    <tr>
        <td>Is Secure Connection</td>
        <td><%: CanvasUrlBuilder.IsSecureConnection %></td>
    </tr>
    <tr>
        <td>Use Facebook Beta</td>
        <td><%: CanvasUrlBuilder.UseFacebookBeta %></td>
    </tr>
    <tr>
        <td>ResolveCanvasUrl sample</td>
        <td><%: this.ResolveCanvasUrl("~/default.aspx") %></td>
    </tr>
    <tr>
        <td>ResolveCanvasPageUrl sample</td>
        <td><%: this.ResolveCanvasPageUrl("~/default.aspx") %></td>
    </tr>
</table>

<br />

<p>
    <a target="_top" href="<%: this.ResolveCanvasPageUrl("~/") %>">Home (<%: this.ResolveCanvasPageUrl("~/") %>)</a>
</p>

<p>
    <a target="_blank" href="<%: this.PicUrlWebClient %>"><%: this.PicUrlWebClient %></a><br />
    <a target="_blank" href="<%: this.PicUrlApp %>"><%: this.PicUrlApp %></a>
</p>