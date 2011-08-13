<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationCallback.aspx.cs" Inherits="CS_AspNetWebForms_RegistrationForm.Facebook.RegistrationCallback" %>

<!DOCTYPE html>
<html xmlns:fb="https://www.facebook.com/2008/fbml">
<head>
    <title>Facebook C# SDK ASP.NET WebForms with Facebook Registration Form</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Facebook C# SDK ASP.NET WebForms with Facebook Registration Form</h1>
        <p>This sample demonstrates the use of Facebook C# SDK along with Facebook Registration Form</p>
        
        <table>
            <thead>
                <tr>
                    <th colspan="2">Registration Data</th>
                </tr>
            </thead>
            <tr>
                <td>Name: </td>
                <td><%: this.RegistrationData.name %></td>
            </tr>
            <tr>
                <td>Email:</td>
                <td><%: this.RegistrationData.email %></td>
            </tr>
            <tr>
                <td>Location:</td>
                <td><%: this.RegistrationData.location.name %></td>
            </tr>
            <tr>
                <td>Gender:</td>
                <td><%: this.RegistrationData.gender %></td>
            </tr>
            <tr>
                <td>Birthday</td>
                <td><%: this.RegistrationData.birthday %></td>
            </tr>
            <tr>
                <td>Phone</td>
                <td><%: this.RegistrationData.phone %></td>
            </tr>
        </table>

    </form>
</body>
</html>
