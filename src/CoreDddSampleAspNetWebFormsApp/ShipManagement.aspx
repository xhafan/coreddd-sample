<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipManagement.aspx.cs" Inherits="CoreDddSampleAspNetWebFormsApp.ShipManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Ship name:
            <asp:TextBox ID="CreateShipNameTextBox" runat="server"></asp:TextBox>
            <br />
            Tonnage:
            <asp:TextBox ID="CreateTonnageTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="CreateNewShipButton" runat="server" Text="Create new ship" OnClick="CreateNewShipButton_Click" />
            <br />
            Last ship id generated: <asp:Label ID="LastGeneratedShipIdLabel" runat="server"></asp:Label>

            <hr />
            
            Ship id:
            <asp:TextBox ID="UpdateShipIdTextBox" runat="server"></asp:TextBox>
            <br />
            Ship name:
            <asp:TextBox ID="UpdateShipNameTextBox" runat="server"></asp:TextBox>
            <br />
            Tonnage:
            <asp:TextBox ID="UpdateTonnageTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="UpdateShipButton" runat="server" Text="Update ship" OnClick="UpdateShipButton_OnClick" />
            <br />
        </div>
    </form>

</body>
</html>
