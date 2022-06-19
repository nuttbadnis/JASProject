<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReportSumStockSN.aspx.vb" Inherits="ReportSumStockSN" %>

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
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans+Thai:wght@400;500;600;700&display=swap" rel="stylesheet">
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
</style>
<body>
    <form id="form1" runat="server">
    <div  style="text-align: center">
        <asp:Label ID="lblShop" runat="server" Font-Bold="True"></asp:Label><br />
        <asp:Label ID="lblItem" runat="server" Font-Bold="True"></asp:Label><br />
        <br />
        <div ID="lblNoSN" runat="server" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr text-center">
            <h5><i class="mdi mdi-alert-circle icon-md"></i> ไม่พบข้อมูลที่ค้นหา</h5>
        </div>
        <asp:GridView ID="GridView1" runat="server" align="center" AutoGenerateColumns="False"
           cssclass="table table-sm table-striped"
            CellPadding="4">
            <RowStyle BackColor="White" ForeColor="#330099" />
            <Columns>
                <asp:BoundField DataField="RowID" HeaderText="ลำดับที่" />
                <asp:BoundField DataField="maingroup" HeaderText="กลุ่มหลัก" />
                <asp:BoundField DataField="subgroup" HeaderText="คลังย่อย" />
                <asp:BoundField DataField="SN" HeaderText="Serial Number" />
            </Columns>
            <FooterStyle BackColor="RoyalBlue" ForeColor="White" Font-Bold="True" />
            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
            <HeaderStyle CssClass="table-primary" />
        </asp:GridView>
        &nbsp;&nbsp;</div>
    </form>
</body>
</html>
