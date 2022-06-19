<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptTaxSale.aspx.vb" Inherits="rptTaxSale" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>รายงานภาษีขาย</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <table width="100%">
                    <tr>
                        <td align="center" style="height: 22px">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                                ForeColor="RoyalBlue" Font-Size="Medium" Text="[22041] รายงานภาษีขาย"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center" style="height: 22px">
                            <asp:Label ID="LblParam1" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="RoyalBlue"
                                Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center" style="height: 17px">
                            <asp:Label ID="LblParam2" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="RoyalBlue"
                                Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="LblParam3" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="RoyalBlue"
                                Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="LblParam4" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="RoyalBlue"
                                Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="LblParam5" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="RoyalBlue"
                                Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="LblHeader" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="RoyalBlue"
                                Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" AlternatingRowStyle-BackColor="#DAF8FC"
                                AutoGenerateColumns="False" Font-Names="Microsoft Sans Serif" Font-Size="10pt"
                                ForeColor="Black" ShowFooter="True" Width="100%">
                                <HeaderStyle CssClass="HeaderStyle" />
                                <Columns>
                                    <asp:BoundField DataField="RO" HeaderText="เขต" />
                                    <asp:BoundField DataField="CodeShop" HeaderText="รหัสสำนักงาน" />
                                    <asp:BoundField DataField="ShopName" HeaderText="ชื่อสำนักงาน" />
                                    <asp:BoundField DataField="DocDate" HeaderText="วันที่" />
                                    <asp:BoundField DataField="DocNo" HeaderText="เลขที่เอกสาร" />
                                    <asp:BoundField DataField="CusName" HeaderText="ชื่อลูกค้า" />
                                    <asp:BoundField DataField="TaxId" HeaderText="เลขประจำตัวผู้เสียภาษี" />
                                    <asp:BoundField DataField="AmountWithTax" DataFormatString="{0:#,#0.00}" HeaderText="ยอดขายที่ต้องเสียภาษี">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tax" DataFormatString="{0:#,#0.00}" HeaderText="ภาษีขาย">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotalAmount" DataFormatString="{0:#,#0.00}" HeaderText="รวมทั้งสิ้น">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    
                                </Columns>
                                <AlternatingRowStyle BackColor="#DAF8FC" />
                                <FooterStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 757px; height: 18px">
                            <asp:Label ID="LblNo1" runat="server" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                                Font-Size="10pt" ForeColor="Red" Text="** ไม่พบข้อมูลที่ค้นหา **" Visible="False"></asp:Label></td>
                    </tr>
                </table>
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
