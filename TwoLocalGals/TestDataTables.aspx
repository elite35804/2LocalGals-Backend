<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestDataTables.aspx.cs"
    Inherits="TwoLocalGals.TestDataTables" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Data Tables</title>
    <style type="text/css" title="currentStyle">
        @import "/DataTables/css/jquery.dataTables.css";
    </style>
    <script type="text/javascript" language="javascript" src="/DataTables/js/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="/DataTables/js/jquery.dataTables.js"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $('#example').dataTable({
                "bStateSave": true
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table runat="server" cellpadding="0" cellspacing="0" CssClass="display"  ID="faewf">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>
                    Test Cell
                </asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow CssClass="gradeX">
                <asp:TableCell>
                    Blah
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>


        <table cellpadding="0" cellspacing="0" border="0" class="display" id="example">
            <thead>
                <tr>
                    <th>
                        Date
                    </th>
                    <th>
                        Customer
                    </th>
                    <th>
                        Contractors
                    </th>
                    <th>
                        Total
                    </th>
                    <th>
                        Amount Owed
                    </th>
                    <th>
                        Payment Type
                    </th>
                    <th>
                        Paid
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr class="gradeX">
                    <td>
                        <a href="http://www.google.com">09/14/86</a>
                    </td>
                    <td>
                        <a href="http://www.google.com">Dustin Marks</a>
                    </td>
                    <td>
                        <a href="http://www.google.com">Bill</a>, <a href="http://www.google.com">Ted</a>
                    </td>
                    <td>
                        $140
                    </td>
                    <td>
                        $75
                    </td>
                    <td>
                        <asp:Button ID="BlahButton" runat="server" Text="Charge Card" />
                    </td>
                    <td>
                        Paid
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
