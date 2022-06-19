<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReportCheque_BB.aspx.vb" Inherits="ReportCheq_BB" %>


    

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>รายงานรับเช็ค</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="expExcel" runat="server" Text="Export To Excel" Visible="False" />
    <div runat="server" id="div_ChequeBB">
        <table>
            <tr>
                <td align="center"></td><td align="center">
                    <asp:Label ID="lblCompany" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="13pt"></asp:Label></td><td align="center"></td>
            </tr>
            <tr>
                <td align="center"></td><td align="center">
                    <asp:Label ID="Label2" runat="server" Text="[22020] รายงานรับเช็ค" Font-Bold="True" Font-Names="Arial" Font-Size="13pt"></asp:Label></td><td align="center"></td>
            </tr>
            <tr>
                <td align="center"></td><td align="center">
                    <asp:Label ID="lblParam1" runat="server" Font-Size="11pt"></asp:Label></td><td align="center"></td>
            </tr><tr>
                <td align="center"></td><td align="center">
                    <asp:Label ID="lblParam2" runat="server" Font-Size="11pt"></asp:Label></td><td align="center"></td>
            </tr><tr>
                <td align="center"></td><td align="center">
                    <asp:Label ID="lblParam3" runat="server" Font-Size="11pt"></asp:Label></td><td align="center"></td>
            </tr><tr>
                <td align="center"></td><td align="center">
                    <asp:Label ID="lblParam4" runat="server" Font-Size="11pt"></asp:Label></td><td align="center"></td>
            </tr><tr>
                <td align="center"></td><td align="center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="11pt"></asp:Label></td><td align="center"></td>
            </tr><tr>
                <td align="center"></td><td align="center"></td><td align="center"></td>
            </tr>
            <tr>
                <td></td><td>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" Font-Names="Arial" Font-Size="9pt" ShowFooter="True">
                        <Columns>
                            <asp:BoundField HeaderText="ลำดับ" />
                            <asp:BoundField HeaderText="เขต" DataField="RO" />
                            <asp:BoundField HeaderText="Cluster" DataField="Cluster" />
                            <asp:BoundField HeaderText="รหัสสำนักงาน" DataField="CodeShop" />
                            <asp:BoundField HeaderText="วันที่เอกสาร" DataField="DateDoc" />
                            <asp:BoundField DataField="cc" HeaderText="บริษัทรับเช็ค" />
                            <asp:BoundField HeaderText="เลขที่ใบเสร็จรับเงิน" DataField="DOC" />
                            <asp:BoundField DataField="status" />
                            <asp:BoundField DataField="CQ" HeaderText="เลขที่เช็ค" />
                            <asp:BoundField DataField="CQDate" HeaderText="วันที่เช็ค" />
                            <asp:BoundField DataField="Bank" HeaderText="ธนาคาร" />
                            <asp:BoundField DataField="BankS" HeaderText="สาขาธนาคาร" />
                            <asp:BoundField DataField="CusName" HeaderText="ชื่อลูกค้า" />
                            <asp:BoundField DataField="Paid" HeaderText="นำฝากแล้ว" />
                            <asp:BoundField HeaderText="ยังไม่นำฝาก" DataField="UnPaid" />
                        </Columns>
                        <RowStyle BackColor="White" ForeColor="#330099" VerticalAlign="Top" />
                        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                    </asp:GridView>
                </td><td></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
