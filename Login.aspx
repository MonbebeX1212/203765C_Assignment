<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_203765C_Assignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

    <script src="https://www.google.com/recaptcha/api.js?render=6Lc8AkUeAAAAAODfAAqbAPMG2zYAbD0EPV3pZ67x"></script>

    <style type="text/css">
        .auto-style1 {
            margin-left: 40px;
        }
        .auto-style2 {
            margin-left: 15px;
        }
        .auto-style3 {
            margin-left: 2px;
        }
        .auto-style4 {
            margin-left: 29px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        &nbsp;<asp:Label ID="loginemail" runat="server" Text="Email: "></asp:Label>
            <asp:TextBox ID="loginemail_tb" runat="server" CssClass="auto-style1" Width="240px"></asp:TextBox>
            <br />
            <br />
            <br />
            <asp:Label ID="loginpw" runat="server" Text="Paasword: "></asp:Label>
            <asp:TextBox ID="loginpw_tb" runat="server" CssClass="auto-style2" Width="236px" TextMode="Password" ></asp:TextBox>
            <br />
            <br />
            <br />
            <asp:Button ID="loginbtn" runat="server" CssClass="auto-style3" onClick="LoginMe"  Text="Login" />
            <asp:Button ID="forgotbtn" runat="server" CssClass="auto-style4" OnClick="forgotbtn_Click" Text="Forgot password" Width="148px" />
            <br />
            <br />

         

           <%-- <div class="g-recaptcha" data-sitekey="6Lc8AkUeAAAAAODfAAqbAPMG2zYAbD0EPV3pZ67x"></div>--%>

            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
            <br />
            <asp:Label ID="error" runat="server" EnableViewState="false" Text=" "></asp:Label>
            <br />
            <asp:Label ID="lockmsg" runat="server" Text=" "></asp:Label>
            <br />
          
            <br />
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lc8AkUeAAAAAODfAAqbAPMG2zYAbD0EPV3pZ67x', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
