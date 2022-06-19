<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowSearch.aspx.vb" Inherits="ShowSearch" %>

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
    table 
    {
        font-family: 'Noto Sans Thai' !important;
        font-size: 15px;
    }
    #display_table{
        overflow: auto;
        cursor: move;
        cursor: ew-resize;
        margin-top: 10px;
        border-width: 0px 1px 0px 1px;
        border-style: solid;
        border-color: #d7ebfb;
        border-radius: 4px;
    }
</style>
<body>
    <form id="form1" runat="server">
        <div class="content-wrapper container-fluid">
            <div class="row">
                <div class="col-12 grid-margin">
                    <div class="card">
                        <div class="d-flex flex-wrap justify-content-xl-between">
                        <div class="d-none d-xl-flex border-md-right flex-grow-1 align-items-start p-3 item">
                            <i class="mdi mdi-note-text mdi-fw icon-lg mr-3 text-primary" style="font-size: 55px;text-align: center !important;width: 3rem;"></i>
                            <div class="d-flex flex-column align-items-start">
                                <small class="text-muted mb-1">
                                    <span class="badge badge-outline-primary">[15030] รายการประกาศราคาสินค้า</span>
                                </small>
                            </div>
                        </div>
                        <div class="d-flex border-md-right align-items-start flex-grow-1 p-3 justify-content-end">
                            <div class="d-flex flex-column align-items-end">
                            <small class="text-muted">
                                <asp:Label ID="LblHeader" runat="server"></asp:Label>
                            </small>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 grid-margin">
                    <div class="card">
                    <div class="card-body">
                        <div id="display_caption">
                            <asp:Label ID="report_param" class="badge badge-pill bg-facebook text-light mb-1 mr-1" runat="server"></asp:Label>
                            <%-- <asp:Label ID="prov_param" class="badge badge-pill badge-danger mb-1 mr-1" runat="server"></asp:Label> --%>
                            <asp:Label ID="shop_param" class="badge badge-pill badge-warning mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="product_param" class="badge badge-pill bg-linkedin text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="date_param" class="badge badge-pill bg-dribbble text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="other_param" class="badge badge-pill bg-pinterest text-light mb-1 mr-1" runat="server"></asp:Label>                        
                        </div>
                        <div id="display_table">
                            <asp:GridView ID="GridView1" runat="server" cssclass="table table-sm" AlternatingRowStyle-BackColor="#DAF8FC"
                                AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None"
                                BorderWidth="1px" CellPadding="4" Width="100%" Font-Names="Tahoma" Font-Size="9pt">
                                <HeaderStyle BackColor="DarkCyan" CssClass="HeaderStyle" Font-Bold="True" ForeColor="#FFFFCC" />
                                <Columns>
                        
                                    <asp:BoundField DataField="ITEMCODE" HeaderText="รหัสสินค้า" />
                                    <asp:BoundField DataField="ITEMNAME" HeaderText="ชื่อสินค้า">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COST" DataFormatString="{0:#,#0.00}" HeaderText="ราคาขาย">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="STARTDATE" HeaderText="วันที่เริ่มใช้" />
                                    <asp:BoundField DataField="ENDDATE" HeaderText="วันที่สิ้นสุด" />
                                    <asp:BoundField DataField="DOC" HeaderText="เลขที่ใบปรับราคา" />
                                    <asp:TemplateField HeaderText="สำนักงานที่มีผล">
                                        <ItemTemplate>
                                            <asp:HyperLink runat="server" id="Shop"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <AlternatingRowStyle BackColor="#DAF8FC" />
                            </asp:GridView>
                        </div>
                        <div ID="LblNo1" runat="server" Visible="False" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr text-center">
                            <h5><i class="mdi mdi-alert-circle icon-md"></i> ไม่พบข้อมูลที่ค้นหา</h5>
                        </div>        
                    </div>
                    </div>
                </div>
            </div>
        </div>
    </form> 
</body>
<script src="https://unpkg.com/scrollbooster@2/dist/scrollbooster.min.js"></script>
<script>

const setupExample1 = () => {
  new ScrollBooster({
    viewport: document.querySelector("#display_table"),
    scrollMode: 'native',
    direction: 'horizontal',
    textSelection: true
  });
};
 setupExample1();
</script>
</html>
