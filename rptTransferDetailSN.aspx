<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptTransferDetailSN.aspx.vb" Inherits="rptTransferDetailSN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <asp:Label ID="lblDoc" runat="server" Font-Bold="True"></asp:Label><br />
        <asp:Label ID="lblItem" runat="server" Font-Bold="True"></asp:Label><br />
        <br />
        <asp:Label ID="lblNoSN" runat="server" Font-Bold="False" Text="--- ไม่พบ Serial Number ---"
            Visible="False" ForeColor="Red"></asp:Label><br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
            BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" align ="center">
            <RowStyle BackColor="White" ForeColor="#330099" />
            <Columns>
                <asp:BoundField DataField="seq" HeaderText="ลำดับที่" />
                <asp:BoundField DataField="SN" HeaderText="Serial Number" />
            </Columns>
            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
        </asp:GridView>
        &nbsp;</div>
    </form>
</body>
</html>
