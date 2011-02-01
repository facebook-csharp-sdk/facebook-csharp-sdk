Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Diagnostics.CodeAnalysis
Imports System.Diagnostics.Contracts
Imports System.Linq
Imports System.Web
Imports Facebook
Imports Facebook.Web

''' <summary>
''' You will want to create your own base class for your pages.
''' This is an example page you can use as a starting point, but
''' you will probably have additional things in common.
''' </summary>
Partial Public Class CanvasPage
    Inherits System.Web.UI.Page

    Protected requiredAppPermissions As String = "user_about_me"

    Public Sub New()
        fbApp = New FacebookApp()

        authorizer = New CanvasAuthorizer()
        authorizer.Perms = requiredAppPermissions
    End Sub

    Protected fbApp As FacebookApp
    Protected authorizer As CanvasAuthorizer

    ''' <summary>
    ''' Performs a canvas redirect.
    ''' </summary>
    ''' <param name="controller">The controller.</param>
    ''' <param name="url">The URL.</param>
    ''' <returns></returns>
    Public Sub CanvasRedirect(ByVal url As String)

        Contract.Requires(url Is Nothing)

        Dim content As String = CanvasUrlBuilder.GetCanvasRedirectHtml(New Uri(url))

        Response.ContentType = "text/html"
        Response.Write(content)
    End Sub
End Class
