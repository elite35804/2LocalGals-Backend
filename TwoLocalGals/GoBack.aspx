<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoBack.aspx.cs" Inherits="TwoLocalGals.GoBack" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GoBack</title>
</head>
<body>
    <script language="javascript" type="text/javascript">
        window.onload = function () {
            window.history.go(-2);
        }
    </script>
</body>
</html>
