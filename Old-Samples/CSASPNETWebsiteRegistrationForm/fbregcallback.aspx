<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fbregcallback.aspx.cs" Inherits="CSASPNETWebsiteRegistrationForm.fbregcallback" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
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
