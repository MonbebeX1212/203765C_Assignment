<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="_203765C_Assignment.ForgetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Password Forgotten</title>

    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=newpassword_tb.ClientID %>').value;

            if (str.length < 12) {
                document.getElementById("pwchecker").innerHTML = "Password length must be at least 12 characters";
                document.getElementById("pwchecker").style.color = "Red";
                return ("too short")
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("pwchecker").innerHTML = "Password require at least 1 number ";
                document.getElementById("pwchecker").style.color = "Red";
                return ("no number")
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("pwchecker").innerHTML = "Password must consist at least 1 upper case";
                document.getElementById("pwchecker").style.color = "Red";
                return ("no capital letter");

            }

            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("pwchecker").innerHTML = "Password require at least 1 lower case";
                document.getElementById("pwchecker").style.color = "Red";
                return ("no lower case");

            }

            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("pwchecker").innerHTML = "Password require at least 1 special characters";
                document.getElementById("pwchecker").style.color = "Red";
                return ("no special character");

            }

            document.getElementById("pwchecker").innerHTML = "Excellent";
            document.getElementById("pwchecker").style.color = "Green";
        }

    </script>



    <style type="text/css">
        .auto-style1 {
            margin-left: 131px;
        }
        .auto-style2 {
            margin-left: 178px;
        }
        .auto-style3 {
            margin-left: 59px;
        }
        .auto-style4 {
            margin-left: 14px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Forgot password or need update your password ah? 
            <br />
            Please enter your email and enter your new password below<br />
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Email: "></asp:Label>
            <asp:TextBox ID="recoveremail_tb" runat="server" CssClass="auto-style1" Height="22px" Width="319px"></asp:TextBox>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="recovererror" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            <asp:Label ID="newpassword" runat="server" Text="New Password: "></asp:Label>
            <asp:TextBox ID="newpassword_tb" runat="server" CssClass="auto-style3" Height="22px" TextMode="Password" onkeyup ="javascript:validate()" Width="318px"></asp:TextBox>
            <asp:Label ID="pwchecker" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            <br />
            <asp:Label ID="newpassword2" runat="server" Text="Confirmed Password: "></asp:Label>
            <asp:TextBox ID="newpassword2_tb" runat="server" CssClass="auto-style4" TextMode="Password" Width="317px"></asp:TextBox>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="passworderror" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            <asp:Button ID="proceedbtn" runat="server" CssClass="auto-style2" Text="Proceed" OnClick="proceedbtn_Click" />
        </div>
    </form>
</body>
</html>
