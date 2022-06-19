<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowrptPayinXread.aspx.vb" Inherits="ShowrptPayinXread" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
  <!-- Required meta tags -->
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <title>POSWEB FOR BEST</title>
  <!-- plugins:css -->
  <link rel="stylesheet" href="App_Inc/majestic/vendors/mdi/css/materialdesignicons.min.css">
  <link rel="stylesheet" href="App_Inc/majestic/vendors/base/vendor.bundle.base.css">
  <!-- endinject -->
  <!-- plugin css for this page -->
  <link rel="stylesheet" href="App_Inc/majestic/vendors/datatables.net-bs4/dataTables.bootstrap4.css">
  <!-- End plugin css for this page -->
  <!-- inject:css -->
  <link rel="stylesheet" href="App_Inc/majestic/css/style.css">
  <!-- endinject -->
  <link rel="shortcut icon" href="App_Inc/majestic/images/favicon.png" />
  <!-- font_th -->
  <link href="https://fonts.googleapis.com/css2?family=Mitr&display=swap" rel="stylesheet">
</head>
<style>
	.card-header-th{
		font-family:'mitr';
		font-size: 1.6rem !important;
	}
	.fs-1{
		font-size:1rem;
	}
	.font-mitr{
		font-family:'mitr';
	}
    .Grid, .Grid th, .Grid td
    {
        border-color: #CCCCCC;
        font-family: Franklin Gothic Book; 
        font-size: 12px;
        font-weight: normal; 
    }
</style>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid mt-2">
        
            <div class="text-center font-mitr">
                <h4><asp:Label ID="LblParam1" runat="server"></asp:Label></h4>
                <h5><asp:Label ID="LblParam2" runat="server"></asp:Label></h5>
                <h5><asp:Label ID="LblParam3" runat="server"></asp:Label></h5>
                <h5><asp:Label ID="LblParam4" runat="server"></asp:Label></h5>
                <h5><asp:Label ID="LblParam5" runat="server"></asp:Label></h5>
                <h5><asp:Label ID="LblHeader" runat="server"></asp:Label></h5>
            </div>

            <div class="table-responsive">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-sm table-striped" Width="100%" ShowFooter="True" >
                <%-- <HeaderStyle CssClass="HeaderStyle"/> --%>
                <Columns>
                    <asp:BoundField DataField="Branch" HeaderText="รหัสสำนักงาน" />
                    <asp:BoundField DataField="DocNo" HeaderText="เลขที่เอกสาร" />
                    <asp:BoundField DataField="Account" HeaderText="เลขที่บัญชี" />
                    <%-- <asp:BoundField DataField="PayinDate" HeaderText="วันที่ฝาก" />--%>
                    <asp:TemplateField HeaderText="วันที่ฝาก">
                        <ItemTemplate>
                            <asp:HyperLink ID="PayinDate" runat="server" Text='<%#Bind("PayinDate") %>'>HyperLink</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SubPay" HeaderText="ยอดรับชำระวันที่" visible="false"/> 
                    <asp:BoundField DataField="Note" HeaderText="รายละเอียด" /> 
                    <asp:BoundField DataField="CS" HeaderText="เงินสดนำฝาก" />  
                    <asp:BoundField DataField="Charge" HeaderText="ค่าธรรมเนียม" />              
                    <asp:BoundField DataField="CQ" HeaderText="เช็คนำฝาก" />                              
                    <asp:BoundField DataField="Sum" HeaderText="รวมทั้งหมด" />                                   

                </Columns>
                <AlternatingRowStyle BackColor="#DAF8FC" />
                <FooterStyle BackColor="SteelBlue" ForeColor="White" />
            </asp:GridView>
            </div>
            
            <div ID="LblNo1" runat="server" Visible="False" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr text-center">
                <h5><i class="mdi mdi-alert-circle icon-md"></i> ไม่พบข้อมูลที่ค้นหา</h5>
            </div>

    </div>
    </form>
</body>
</html>

