<%@ Page Language="VB" AutoEventWireup="false" CodeFile="showshopdetail.aspx.vb" Inherits="showshopdetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>�ʴ���������´�ӹѡ�ҹ��С���ҤҢ��</title>
</head>
<body style="font-family:Tahoma; font-size:9pt;">
    <form id="form1" runat="server">
            <center>
                <table>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="200px">
                                <Columns>
                                    <asp:BoundField DataField="F07" HeaderText="��������´�ӹѡ�ҹ"/>
                                </Columns>
                                <RowStyle BorderColor="#FF8000" />
                                <HeaderStyle BackColor="LightGreen" />
                            </asp:GridView> 
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label id="LblNo1" runat="server" Font-Bold="True" Font-Size="10pt" Text="** ��辺�����ŷ����� **" Visible="False" ForeColor="Red"></asp:Label></td>
                    </tr>
                </table>
            </center>
    </form>
</body>
</html>
