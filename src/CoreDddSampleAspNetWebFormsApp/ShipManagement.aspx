<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipManagement.aspx.cs" Inherits="CoreDddSampleAspNetWebFormsApp.ShipManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="ShipNameTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:TextBox ID="TonnageTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="CreateNewShipButton" runat="server" Text="Create new ship" OnClick="CreateNewShipButton_Click" />
            <br />
            Last ship id generated: <asp:Label ID="LastGeneratedShipIdLabel" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
