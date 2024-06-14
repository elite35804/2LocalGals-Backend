<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TwoLocalGals.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2 Local Gals - Portal Login</title>
    <link href="/Styles/LoginM.css" media="screen and (max-width: 800px)" rel="stylesheet" type="text/css" />
    <link href="/Styles/Login.css" media="screen and (min-width: 800px)" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=600" />
</head>
<body>
    <script language="javascript" type="text/javascript">
    window.onload = function () {
        document.getElementById('<%=Username.ClientID %>').focus();
    }
    </script>
    <form id="form" runat="server">
    <div class="LoginBox">
        <table class="LoginTable">
            <tr>
                <td>
                    <a href="http://2localgals.com"><img src="/2LG_Logo.jpg" alt="None" style="width:350px;" runat="server" /></a>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="ErrorLabel" CssClass="Error" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="LoginHeader">
                    Email Address / Username
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="Username" CssClass="LoginTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="LoginHeader">
                    Password
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="Password" CssClass="LoginTextBox" TextMode="Password" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Keep me logged in
                    <asp:CheckBox ID="RememberMe" Checked="true" runat="server" /> | <asp:LinkButton ID="ForgotButton" Text="Forgot Password?" OnCommand="Forgot_Click" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="LogOnButton" OnClick="Logon_Click" Text="Log On" runat="server" CssClass="LogonButton" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <!-- Google Tag Manager -->
    <noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-TQLF5BS"
    height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
    <script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
    new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
    j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
    '//www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
    })(window,document,'script','dataLayer','GTM-TQLF5BS');</script>
    <!-- End Google Tag Manager -->
</body>
</html>
