<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Partners.aspx.cs" Inherits="TwoLocalGals.Partners" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2LG - Partners</title>
    <link href="/Styles/PartnersM.css" media="screen and (max-width: 800px)" rel="stylesheet" type="text/css" />
    <link href="/Styles/Partners.css" media="screen and (min-width: 800px)" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="/favicon.ico" />
    <script src="/JScript.js" language="javascript" type="text/javascript"></script>
    <link href="/chosen/chosen.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-2.0.3.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="/chosen/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery(".chzn-select").chosen();
        });
    </script>
    <meta name="viewport" content="width=600" />
</head>
<body>
<form runat="server" autocomplete="off">
    <div class="Page">
        <img class="Logo" src="/2LG_PortalLogo.jpg" alt="None" />
        <div class="ErrorDiv">
            <asp:Label ID="ErrorLabel" runat="server" />
        </div>
        <div class="Title">
            Partner Referrals
        </div>
        <div class="Category">
            Select Category <asp:DropDownList ID="BusinessType" CssClass="PartnersDropDown" Width="250px" runat="server" OnSelectedIndexChanged="CategoryChanged" AutoPostBack="true" />
        </div>
        <div id="PartnersDiv" class="PartnersDiv" runat="server">
        </div>
    </div>
</form>
</body>
</html>
