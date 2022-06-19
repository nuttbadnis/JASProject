<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowrptXreadLink.aspx.vb" Inherits="ShowrptXreadLink" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Report Summary X-Read</title>
</head>
<body style="font-size: 10pt; font-family: 'Microsoft Sans Serif'">
    <form id="form1" runat="server">
    <div>
        <div>
            <table width="100%">
                <tr>
                    <td align="center" style="height: 22px">
                        <asp:Label ID="LblParam1" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="RoyalBlue"
                            Width="100%"></asp:Label></td>
                </tr>
                <tr>
                    <td align="center">
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
                                <asp:BoundField DataField="Area" HeaderText="เขต" />
                                <asp:BoundField DataField="BranchName" HeaderText="รหัสสำนักงาน" />
                                <asp:TemplateField HeaderText="วันที่">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" id="DateDoc" text='<%#Bind("DateDoc") %>'>HyperLink</asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Doc" HeaderText="ใบกำกับภาษี/ใบลดหนี้" />
                                <asp:BoundField DataField="Sales" HeaderText="ผู้รับเงิน" />
                                <asp:BoundField DataField="Status" HeaderText="สถานะเอกสาร" />
                                
                                <asp:BoundField DataField="CS" DataFormatString="{0:#,#0.00}" HeaderText="เงินสด">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CR" DataFormatString="{0:#,#0.00}" HeaderText="เครดิตการ์ด">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CQ" DataFormatString="{0:#,#0.00}" HeaderText="เช็ค">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TR" DataFormatString="{0:#,#0.00}" HeaderText="เงินโอน">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="WH" DataFormatString="{0:#,#0.00}" HeaderText="หัก ณ ที่จ่าย">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AO" DataFormatString="{0:#,#0.00}" HeaderText="อื่น ๆ">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="A_Type" DataFormatString="{0:#,#0.00}" HeaderText="จำนวนเงินชำระ">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AD" DataFormatString="{0:#,#0.00}" HeaderText="หักชำระ">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AI" DataFormatString="{0:#,#0.00}" HeaderText="เพิ่มชำระ">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FR" DataFormatString="{0:#,#0.00}" HeaderText="ปัดเศษลง(ขึ้น)">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <AlternatingRowStyle BackColor="#DAF8FC" />
                            <FooterStyle Font-Bold="True" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 757px">
                        <asp:Label ID="LblNo1" runat="server" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                            Font-Size="10pt" ForeColor="Red" Text="** ไม่พบข้อมูลที่ค้นหา **" Visible="False"></asp:Label></td>
                </tr>
            </table>
        </div>
    
    </div>
    </form>
</body>
</html>
