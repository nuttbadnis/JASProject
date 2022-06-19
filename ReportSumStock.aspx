<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReportSumStock.aspx.vb" Inherits="ReportSumStock" %>

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
                                    <asp:Label ID="report_name" class="badge badge-outline-primary" runat="server"></asp:Label>
                                </small>
                                <small class="text-muted mb-1">
                                    <asp:Label ID="comp_param" class="badge badge-outline-danger" runat="server"></asp:Label>
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
                            <asp:Label ID="ShowHead" class="badge badge-pill badge-success mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="ro_param" class="badge badge-pill bg-facebook text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="prov_param" class="badge badge-pill badge-warning mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="branch_param" class="badge badge-pill badge-danger mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="main_param" class="badge badge-pill bg-linkedin text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="sub_param" class="badge badge-pill bg-dribbble text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="product_param" class="badge badge-pill bg-pinterest text-light mb-1 mr-1" runat="server"></asp:Label>                        
                        </div>
                        <div id="display_table">
                            <asp:GridView ID="GridView1" cssclass="table table-sm table-striped" runat="server" Width="100%"
                                ShowFooter="True" AutoGenerateColumns="False">         
                                <Columns>   
                                    <asp:BoundField DataField="Area" HeaderText="เขต" />
                                    <asp:BoundField DataField="Province" HeaderText="จังหวัด"  />
                                    <asp:BoundField DataField="ShopCode" HeaderText="สำนักงาน"  />
                                    <asp:BoundField DataField="ItemCode" HeaderText="รหัสสินค้า" />
                                    <asp:BoundField ItemStyle-CssClass="min-px-300" DataField="ItemName" HeaderText="ชื่อสินค้า" />
                                    <asp:BoundField DataField="WH" HeaderText="คลังหลัก" />
                                    <asp:BoundField ItemStyle-CssClass="min-px-100" DataField="FG" HeaderText="คลังย่อย FG" />
                                    <asp:BoundField ItemStyle-CssClass="min-px-100" DataField="NG" HeaderText="คลังย่อย NG" />
                                    <asp:BoundField ItemStyle-CssClass="min-px-100" DataField="OG" HeaderText="คลังย่อย OG" />
                                    <asp:BoundField ItemStyle-CssClass="min-px-100" DataField="RG" HeaderText="คลังย่อย RG" />
                                    <asp:BoundField ItemStyle-CssClass="min-px-100" DataField="TS" HeaderText="คลังย่อย TS" />
                                    <asp:BoundField ItemStyle-CssClass="min-px-100" DataField="WJ" HeaderText="คลังย่อย WJ" />
                                    <asp:BoundField ItemStyle-CssClass="min-px-100" DataField="WP" HeaderText="คลังย่อย WP" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server"
                                                Target="_blank"><i class="mdi mdi-link icon-md"></i></asp:HyperLink>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            SERIAL
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="RoyalBlue" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="table-primary" />
                            </asp:GridView>
                        </div>
                        <div ID="lblNodata" runat="server" Visible="False" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr text-center">
                            <h5><i class="mdi mdi-alert-circle icon-md"></i> ไม่พบข้อมูลที่ค้นหา</h5>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 grid-margin">
                    <div id="detail_serial" runat="server" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr">
                        <div class="row">
                            <div class="col-12">
                                <span class="badge badge-danger fs-1">หมายเหตุ : คลังหลักสินค้า ประกอบด้วย 5 คลังย่อย ดังต่อไปนี้</span>
                            </div>
                            <div class="col-12">
                                <ul class="list-unstyled">
                                    <li><u>- FG คลังย่อยสินค้าเพื่อขาย</li>
                                    <li><u>- NG คลังย่อยสินค้าชำรุด</li>
                                    <li><u>- OG คลังย่อยสินค้าที่ไม่ได้อยู่ที่สำนักงาน</li>
                                    <li><u>- RG คลังย่อยสินค้าจอง</li>
                                    <li><u>- TS คลังย่อยสินค้าระหว่างทาง</li>
                                </ul>
                            </div>
                        </div>
                        <button type="button" class="close" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>        
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