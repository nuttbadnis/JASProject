<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pmtSummary.aspx.vb" Inherits="pmtSummary" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>รายงานการนำส่งเงินสิ้นวัน</title>
    <LINK href="Calendar/bis.css" type="text/css" rel="StyleSheet">
	<SCRIPT language="JavaScript" src="Calendar/popcalendar.js" type="text/javascript">
	    function TABLE1_onclick()
	    {}
    </SCRIPT>    
    <style type="text/css">
        .bodyText
        {
	        background: white;
	        font-family: Tahoma, Verdana, Arial, sans-serif;
	        font-size:13px;
	        font-weight:bold;
	        color:#000099;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function checkDate()
         {
             if (document.getElementById("<%=cboSBranch.ClientID %>").selectedIndex==0)
            {alert('โปรดเลือกสำนักงานค่ะ');return false;}
            
            var txtSDate=document.getElementById("<%=SDate.ClientID %>");
           
           
                                        
                if (txtSDate.value==''  || txtSDate.value.length!=10)
                {alert('โปรดระบุช่วงวันที่');return false;}
                else
                {
                   
                    return true;
                }                    
                
        }
            
    </script> 
</head>
<body>
    <form id="form1" runat="server" class="bodyText">
    <div>
     <br /> <br />
        <table style="width: 720px">
            
            <tr>
                <td>
                </td>
                <td align ="center" >
                    <asp:Label ID="Label1" runat="server" Text="[21040] รายงานการนำส่งเงินสิ้นวัน" Width="432px" Font-Bold="True" Font-Size="Medium"></asp:Label>&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                </td>
            </tr>
         </table>
        <br />
        <table align="left" style="width: 720px">             
            <tr>
                <td colspan="4" align="right">
                    <asp:CheckBox ID="chkParameter" runat="server" Checked="True" Font-Bold="True" Font-Names="Tahoma"
                        Font-Size="8pt" ForeColor="#0000C0" Text="จำพารามิเตอร์ที่เลือกไว้ในเครื่องของคุณ" /></td>
            </tr>
            <tr>
                <td style="width: 19px"></td>
                <td style="width: 98px; height: 34px;">
                    ช่วงวันที่ :</td>
                <td style="width: 543px; height: 34px;">
                                    <asp:TextBox ID="SDate" runat="server" onclick="popUpCalendar(this, this,'dd/mm/yyyy');return false;" onKeyPress="return PreTextChange();" Width="162px" CssClass="Nothing"></asp:TextBox></td>
                <td style="width: 267px; height: 34px">
                                    </td>
            </tr> 
            <tr>
                <td style="width: 19px"></td>
                <td style="width: 98px; height: 40px;">
                    เขตธุรกิจ :</td>
                <td style="width: 543px; height: 40px;">
                    &nbsp;<asp:DropDownList ID="cboSArea" runat="server" Width="168px" AutoPostBack="True">
                                    </asp:DropDownList></td>
                <td style="height: 40px; width: 267px;">
                    </td>
            </tr>
            <tr>
                <td style="width: 19px"></td>
                <td style="width: 98px; height: 40px;">
                    จังหวัด :</td>
                <td style="width: 543px; height: 40px;">
                                    <asp:DropDownList ID="cboSProvince" runat="server" Width="168px" AutoPostBack="True">
                                    </asp:DropDownList></td>
                <td style="height: 40px; width: 267px;">
                                    </td>
            </tr> 
            <tr>
                <td style="width: 19px"></td>
                <td style="width: 98px; height: 40px;">
                    สำนักงาน :</td>
                <td style="width: 543px; height: 40px;">
                                    <asp:DropDownList ID="cboSBranch" runat="server" Width="376px">
                                    </asp:DropDownList></td>
                <td style="height: 40px; width: 267px;">
                                    </td>
            </tr> 
            
            
            <tr>
                <td style="width: 19px"></td>
                <td style="width: 98px;">
                </td>
                <td style="width: 543px;" align="center">
                    <asp:Button ID="cmdPreview" runat="server" Text="Preview" Width="104px" OnClientClick ="return checkDate();"/></td>
                <td style="width: 267px">
                </td>
            </tr>
            <tr>
                <td style="width: 19px; height: 35px;"></td>
                <td style="height: 35px;" colspan="2" valign="bottom">
                    <asp:LinkButton ID="LinkButton1" runat="server">วัตถุประสงค์ของรายงาน</asp:LinkButton>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:LinkButton ID="LinkButton2" runat="server">วิธีการใช้งาน</asp:LinkButton></td>
                <td style="width: 267px; height: 35px;">
                </td>
            </tr>                                                               
        </table>    
    </div>
    </form>
</body>
</html>