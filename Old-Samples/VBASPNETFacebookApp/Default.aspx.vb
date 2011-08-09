Option Strict Off
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Diagnostics.CodeAnalysis
Imports System.Diagnostics.Contracts
Imports System.Linq
Imports System.Web
Imports Facebook
Imports Facebook.Web

Partial Class _Default
    Inherits CanvasPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (authorizer.Authorize()) Then
            LoggedIn()
        End If

    End Sub
    Private Sub LoggedIn()
        Dim myInfo As Object = fbApp.Get("me")
        lblName.Text = myInfo.name
        pnlHello.Visible = True

    End Sub
End Class
