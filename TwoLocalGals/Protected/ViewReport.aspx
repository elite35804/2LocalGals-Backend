<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="Nexus.Protected.ViewReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2 Local Gals Housekeeping</title>
    <link href="~/Styles/Reports.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-2.0.3.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="/JScript.js" language="javascript" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        window.onload = function WindowLoad() {
                var yPos = $.cookie("ReportScrollPos");
                window.scrollTo(0, yPos);
        }
    </script>
</head>
<body>
    <form id="form" runat="server">
    <input type="hidden" value="0" id="hiddenScrollPos" runat="server" />
    <div id="PageDiv" runat="server">
        <div id="ErrorDiv" class="Error" runat="server" />
        <div id="ReportTitleDiv" class="ReportTitle" runat="server" />
        <div id="DateRangeDiv" class="DateRange" runat="server" />
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        $('*').click(function () {
            var yPos = window.pageYOffset || document.documentElement.scrollTop;
            $.cookie("ReportScrollPos", yPos);
        });
    </script>
</body>
</html>
