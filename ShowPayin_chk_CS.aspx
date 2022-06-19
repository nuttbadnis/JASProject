<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowPayin_chk_CS.aspx.vb" Inherits="ShowPayin_chk_CS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Show Report ค้างนำฝากเงินสดแบบตรวจสอบ</title>
    <style type="text/css">
        body
        {
	        background: white;
	        font-family: Microsoft San Sarif;
	        font-size:13px;            
        }
        /* GridView */
        .Gridfont
        {
	        font-family: Microsoft San Sarif;
	        font-size:13px;
	        color:Black;
        }

        .SelectedRowStyle
        {
            background-color: Yellow;
            font-family: Microsoft San Sarif;
        }
        .HeaderStyle
        {
            background-color: #53A0D5;
            color:White;
            font-family: Microsoft San Sarif;
            font-size:13px;	
        }
        .FooterStyle
        {
            background-color: #53A0D5;
            color:White;
            font-family: Microsoft San Sarif;
            font-size:13px;	
        }
        .PagerStyle
        {
            background-color: #53A0D5;
            color:White;
            font-family: Microsoft San Sarif;
            font-size:13px;	
            font-weight:bold;
        }        
    </style>    
</head>
<body>
    <form id="form1" runat="server">
    <div>       
        <A NAME="top"></A> 
        <A NAME="gridview1"></A>
        <table width="100%">
            <tr>
                <td align="center" style="width: 757px; height: 17px;">
                    <asp:Label ID="LblParam1" runat="server" Font-Bold="True" Width="100%" Font-Size="Medium" ForeColor="RoyalBlue"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" style="width: 757px">
                    <asp:Label ID="LblParam2" runat="server" Font-Bold="True" Width="100%" ForeColor="RoyalBlue"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" style="width: 757px">
                    <asp:Label ID="LblParam3" runat="server" Font-Bold="True" Width="100%" ForeColor="RoyalBlue"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" style="width: 757px; height: 17px;">
                    <asp:Label ID="LblParam4" runat="server" Font-Bold="True" Width="100%" ForeColor="RoyalBlue"></asp:Label></td>
            </tr>  
            <tr>
                <td align="center" style="width: 757px; height: 17px;">
                    <asp:Label ID="LblParam5" runat="server" Font-Bold="True" Width="100%" Font-Size="Small" ForeColor="RoyalBlue"></asp:Label></td>
            </tr>                                    
            <tr>
                <td align="center" style="width: 757px; height: 13px;"><asp:Label ID="LblHeader" runat="server" Font-Bold="True" Font-Size="Small" Width="760px" ForeColor="RoyalBlue"></asp:Label></td>
            </tr>
            <tr>
                <td style="height: 18px; width: 757px;"></td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td colspan="2" rowspan="3" width="80%" align="left">
                    <asp:Label ID="LblGv1" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="ActiveCaption"
                        Text="รายงานเงินสดค้างนำฝากแบบตรวจสอบ" Width="376px"></asp:Label></td>
                            <td rowspan="3" width="20%" align="right">
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>                   
            <tr>
                <td align="center">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ForeColor="Black" AlternatingRowStyle-BackColor="#DAF8FC" Width="80%" >
                        <HeaderStyle CssClass="HeaderStyle"/>
                        <Columns>
                            <asp:BoundField DataField="company_code" HeaderText="บริษัท" />
                            <asp:BoundField DataField="area_code" HeaderText="เขตธุรกิจ" />
                            <asp:BoundField DataField="province_code" HeaderText="จังหวัด" />
                            <asp:BoundField DataField="branch_code" HeaderText="สำนักงาน" />
                            <asp:BoundField DataField="receipt_date" HeaderText="วันที่รับชำระ" />
                            <asp:BoundField DataField="receipt_cs" HeaderText="เงินสดที่รับเข้ามา" />
                            <asp:BoundField DataField="payin_cs" HeaderText="จำนวนเงินสดที่นำฝาก" />
                            <asp:BoundField DataField="cs_in_sys" HeaderText="จำนวนเงินสดคงเหลือยังไม่นำฝาก" />                         
                        </Columns>
                        <AlternatingRowStyle BackColor="#DAF8FC" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 757px">
                    <asp:Label ID="LblNo1" runat="server" Font-Bold="True" Text="** ไม่พบข้อมูลที่ค้นหา **" Visible="False" ForeColor="Red"></asp:Label><br />
                    <br />
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="   *** ข้อมูลค้างนำฝากสะสมตั้งแต่วันที่ 01/11/2009 ***"></asp:Label></td>
            </tr>            
        </table>
       
        <br />
        <br />
        <br />
        <table width="100%">
            <tr>
                <td rowspan="3" width="40%">
                    &nbsp;
                </td>
                <td colspan="2" rowspan="3" width="60%" align="right">
                    <a href="#top" id="aTop3" runat="server" style="cursor:hand">Go To Top Page</a>
                </td>
            </tr>
            <tr>
            </tr>
        </table>        
    </div>
    </form>
</body>
</html>



