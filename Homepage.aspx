<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="_203765C_Assignment.Homepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Homepage</title>

</head>
<body>

    <form id="form1" runat="server">
        
        <div>
            
            <asp:Label ID="Message" runat="server" EnableViewState="False"> </asp:Label>
            <br />
            <br />
            <br />
            <asp:Label ID="List" runat="server" EnableViewState="False" > List of Images uploaded </asp:Label>
            <%--<asp:Image ID="Photo" runat="server" />--%>
            <br />
            <br />
            <asp:Repeater ID="RepeaterImages" runat="server">
                <ItemTemplate>
                    <asp:Image ID="Image" runat="server" ImageUrl='<%# Container.DataItem %>' ControlStyle-Height="100" ControlStyle-Width="100" Width="242px" />
                </ItemTemplate>
            </asp:Repeater>
            <br />
            <br />
            <br />
            <asp:Button ID="logoutbtn" runat="server" OnClick="LogoutMe" Visible="false" Text="Logout" />
        </div>
    </form>
</body>
</html>
