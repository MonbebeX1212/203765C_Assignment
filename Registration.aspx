<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="_203765C_Assignment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Form</title>

    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=pw_tb.ClientID %>').value;

            if (str.length < 12) {
                document.getElementById("pwchecker").innerHTML = "Password length must be at least 12 characters";
                document.getElementById("pwchecker").style.color = "Red";
                return ("too short")
            }
            else if (str.search(/[0-9]/)==-1) {
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

        function creditvalidate() {
            var creditstr = document.getElementById('<%=credit_tb.ClientID %>').value;

            if (creditstr.length < 16) {
                document.getElementById("creditchecker").innerHTML = "Credit card length must be 16 digits";
                document.getElementById("creditchecker").style.color = "Red";
                return ("too short")
            }

            document.getElementById("creditchecker").innerHTML = "Valid!";
            document.getElementById("creditchecker").style.color = "Green";

        }

        function namevalidate() {
            var firststr = document.getElementById('<%=first_tb.ClientID %>').value;
            
            
            if (firststr.search(/[0-9]/) != -1) {

                document.getElementById("namechecker").innerHTML = "Name must not include numbers!";
                document.getElementById("namechecker").style.color = "Red";
                return ("No numbers!")
            }

            else if (firststr.search(/[^a-zA-Z0-9]/) != -1) {

                document.getElementById("namechecker").innerHTML = "Name must not include special character!";
                document.getElementById("namechecker").style.color = "Red";
                return ("No special characters!")
            }

            else {
                document.getElementById("namechecker").innerHTML = "Valid!";
                document.getElementById("namechecker").style.color = "Green";
            }

            
        }

        function lastvalidate() {
            var laststr = document.getElementById('<%=last_tb.ClientID %>').value;


            if (laststr.search(/[0-9]/) != -1) {

                document.getElementById("lastchecker").innerHTML = "Name must not include numbers!";
                document.getElementById("lastchecker").style.color = "Red";
                return ("No numbers!")
            }

            else if (laststr.search(/[^a-zA-Z0-9]/) != -1) {

                document.getElementById("lastchecker").innerHTML = "Name must not include special character!";
                document.getElementById("lastchecker").style.color = "Red";
                return ("No special characters!")
            }

            else {
                document.getElementById("lastchecker").innerHTML = "Valid!";
                document.getElementById("lastchecker").style.color = "Green";
            }


        }

        function datevalidate() {
            var datestr = document.getElementById('<%=date_tb.ClientID %>').value;

            if (datestr.search(/[0-9]/) == -1) {
                document.getElementById("datechecker").innerHTML = "Only numbers are allowed";
                document.getElementById("datechecker").style.color = "Red";
                return ("Only numbers!");
            }

            else {
                document.getElementById("datechecker").innerHTML = "Valid!";
                document.getElementById("datechecker").style.color = "Green";
            }
        }


       



    </script>


    <style type="text/css">
        .auto-style1 {
            margin-left: 49px;
        }
        .auto-style2 {
            margin-left: 55px;
        }
        .auto-style3 {
            margin-left: 44px;
        }
        .auto-style4 {
            margin-left: 59px;
        }
        .auto-style5 {
            margin-left: 25px;
        }
        .auto-style6 {
            margin-left: 34px;
        }
        .auto-style7 {
            margin-left: 250px;
        }
        .auto-style8 {
            margin-left: 222px;
        }
        .auto-style9 {
            margin-left: 91px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <br />
            First Name:<asp:TextBox ID="first_tb" runat="server" CssClass="auto-style1" Width="416px" onkeyup ="javascript:namevalidate()"></asp:TextBox>
            <asp:Label ID="namechecker" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            Last Name:<asp:TextBox ID="last_tb" runat="server" CssClass="auto-style2" Width="418px" onkeyup ="javascript:lastvalidate()"></asp:TextBox>
            <asp:Label ID="lastchecker" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            Credit Card:<asp:TextBox ID="credit_tb" runat="server" CssClass="auto-style3" Width="419px" onkeyup ="javascript:creditvalidate()"></asp:TextBox>
            <asp:Label ID="creditchecker" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            Email address:<asp:TextBox ID="email_tb" runat="server" CssClass="auto-style5" Width="420px" onkeyup =" javascript:emailvalidate()"></asp:TextBox>
            <asp:Label ID="emailchecker" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            Password:<asp:TextBox ID="pw_tb" runat="server" CssClass="auto-style4" Width="425px" TextMode="Password" onkeyup ="javascript:validate()"></asp:TextBox>
            <asp:Label ID="pwchecker" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            <asp:Button ID="checkbtn" runat="server" CssClass="auto-style7" OnClick="checkbtn_Click" Text="Check" Width="209px" />
            <br />
            <br />
            Date of Birth:<asp:TextBox ID="date_tb" runat="server" CssClass="auto-style6" Width="424px" onkeyup ="javascript:datevalidate()"></asp:TextBox>
            <asp:Label ID="datechecker" runat="server" Text=" "></asp:Label>
            <br />
            <br />
            Photo:<asp:FileUpload ID="photoUpload" runat="server" CssClass="auto-style9" />
            <br />
            <br />

            <asp:Image ID="Photo" runat="server" />
            <br />
            <br />
            <asp:Button ID="submitbtn" runat="server" CssClass="auto-style8" OnClick="submitbtn_Click" Text="Submit" Width="269px" />
            <br />
            <br />
            <br />
        </div>
        <p>
            &nbsp;</p>
    </form>
</body>
</html>
