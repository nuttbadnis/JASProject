<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RemainRouterReport.aspx.vb" Inherits="RemainRouterReport" %>
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
                                    <span class="badge badge-outline-primary">[10096] รายงานการโอนสินค้าระหว่างสำนักงานแบบระบุรหัสสินค้า</span>
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
                            <asp:Label ID="report_param" class="badge badge-pill bg-facebook text-light mb-1 mr-1" runat="server"></asp:Label>
                            <%-- <asp:Label ID="prov_param" class="badge badge-pill badge-danger mb-1 mr-1" runat="server"></asp:Label> --%>
                            <asp:Label ID="shop_param" class="badge badge-pill badge-warning mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="product_param" class="badge badge-pill bg-linkedin text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="date_param" class="badge badge-pill bg-dribbble text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="other_param" class="badge badge-pill bg-pinterest text-light mb-1 mr-1" runat="server"></asp:Label>                        
                        </div>
                        <div id="display_table">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-sm table-striped" AutoGenerateColumns="False"
                            BorderWidth="1px" PageSize="50" ShowFooter="True">
                                <Columns>
                                <asp:BoundField DataField="area" HeaderText="เขต" />
                                <asp:BoundField DataField="itemcode" HeaderText="รหัสสินค้า" />
                                <asp:BoundField DataField="itemname" ItemStyle-Width="50%" ItemStyle-Wrap="false" HeaderText="ชื่อสินค้า" />
                                <asp:BoundField DataField="ขาย" HeaderText="จำนวนรายการที่ออกใบเสร็จ" DataFormatString="{0:#,#0}" />
                                <asp:BoundField DataField="เบิก" HeaderText="จำนวนสินค้าที่เบิก/รับคืน" DataFormatString="{0:#,#0}" />
                                </Columns>
                                <FooterStyle CssClass="bg-warning" />
                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="table-primary" />
                            </asp:GridView>

                            <asp:GridView ID="GridView2" runat="server" CssClass="table table-sm" AutoGenerateColumns="False"
                            BorderWidth="1px" PageSize="50" ShowFooter="True">
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <Columns>
                                    <asp:BoundField DataField="area" HeaderText="เขต" />
                                    <asp:BoundField DataField="cluster" HeaderText="Cluster" />
                                    <asp:BoundField DataField="itemcode" HeaderText="รหัสสินค้า" />
                                    <asp:BoundField DataField="itemname" ItemStyle-Width="50%" ItemStyle-Wrap="false" HeaderText="ชื่อสินค้า" />
                                    <asp:BoundField DataField="ขาย" HeaderText="จำนวนสินค้าที่ขาย" DataFormatString="{0:#,#0}" />
                                    <asp:BoundField DataField="เบิก" HeaderText="จำนวนสินค้าที่เบิก/รับคืน" DataFormatString="{0:#,#0}" />
                                </Columns>
                                <FooterStyle CssClass="bg-warning" />
                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="table-primary" />
                            </asp:GridView>
                
                            <asp:GridView ID="GridView3" runat="server" CssClass="table table-sm" BorderWidth="1px"
                            AutoGenerateColumns="False" PageSize="50" ShowFooter="True">
                                <FooterStyle CssClass="bg-warning" />
                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="table-primary" />
                                <Columns>
                                    <asp:BoundField DataField="area" HeaderText="เขต" />
                                    <asp:BoundField DataField="cluster" HeaderText="Cluster" />
                                    <asp:BoundField DataField="Province" HeaderText="จังหวัด" />
                                    <asp:BoundField DataField="itemcode" HeaderText="รหัสสินค้า" />
                                    <asp:BoundField DataField="itemname" ItemStyle-Width="50%" ItemStyle-Wrap="false" HeaderText="ชื่อสินค้า" />
                                    <asp:BoundField DataField="ขาย" HeaderText="จำนวนสินค้าที่ขาย" DataFormatString="{0:#,#0}" />
                                    <asp:BoundField DataField="เบิก" HeaderText="จำนวนสินค้าที่เบิก" DataFormatString="{0:#,#0}" />
                                </Columns>
                            </asp:GridView>

                            <asp:GridView ID="GridView4" runat="server" CssClass="table table-sm" BorderWidth="1px" 
                            AutoGenerateColumns="False" PageSize="50" ShowFooter="True" EnableModelValidation="True">
                                <FooterStyle CssClass="bg-warning" />
                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="table-primary" />
                                <Columns>
                                    <asp:BoundField DataField="ItemCode" HeaderText="รหัสสินค้า" />
                                    <asp:BoundField DataField="ItemName" ItemStyle-Width="50%" ItemStyle-Wrap="false" HeaderText="ชื่อสินค้า" />
                                    <asp:BoundField DataField="area" HeaderText="เขต" />
                                    <asp:BoundField DataField="Cluster" HeaderText="Cluster" />
                                    <asp:BoundField DataField="Province" HeaderText="จังหวัด" />
                                    <asp:BoundField DataField="CodeShop" HeaderText="รหัสสำนักงาน" />
                                    <asp:BoundField DataField="Doccode" HeaderText="เลขที่เอกสาร" />
                                    <asp:BoundField DataField="DocDate" HeaderText="วันที่ออกใบเสร็จ" />
                                    <asp:BoundField DataField="Account" HeaderText="บัญชีผู้เช่า (Account)" />
                                    <asp:BoundField DataField="CustomerName" ItemStyle-Width="30%" ItemStyle-Wrap="false" HeaderText="ชื่อผู้เช่า" />
                                    <asp:BoundField DataField="Status" HeaderText="สถานะเอกสาร" />
                                    <asp:BoundField DataField="QTY" HeaderText="จำนวนสินค้าที่ขาย" DataFormatString="{0:#,#0}" />
                                    <asp:BoundField DataField="Amount" HeaderText="จำนวนเงิน" DataFormatString="{0:#,#0.00}" />
                                    <asp:BoundField DataField="Item2" HeaderText="รหัสสินค้า" />
                                    <asp:BoundField DataField="Doccode2" HeaderText="เลขที่ใบเบิก" />
                                    <asp:BoundField DataField="DocDate2" HeaderText="วันที่เอกสาร" />
                                    <asp:BoundField DataField="QTY2" HeaderText="จำนวนสินค้าที่เบิก/รับคืน" DataFormatString="{0:#,#0}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div ID="lblNodata" runat="server" Visible="False" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr text-center">
                            <h5><i class="mdi mdi-alert-circle icon-md"></i> ไม่พบข้อมูลยอดรับชำระ</h5>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 grid-margin">
                    <div class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr">
                        <div class="row">
                            <div class="col-lg-1 col-12">
                                <span class="badge badge-danger fs-1">หมายเหตุ</span>
                            </div>
                            <div class="col-lg-11 col-12">
                                <ul class="list-unstyled">
                                    <li><u>เอกสารที่ขึ้นต้นด้วย ART</u> : เป็นเอกสารรับคืนสินค้า</li>
                                    <li><u>เอกสารที่ขึ้นต้นด้วย DCN</u> : เป็นเอกสารใบลดหนี้</li>
                                    <li>ซึ่งเอกสารทั้ง 2 เอกสารนี้จะแสดงผลเป็นติดลบเนื่องจากเป็นการรับสินค้าคืนเข้ามาในระบบ</li>
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
    direction: 'horizontal'
  });
};
 setupExample1();
</script>
</html>
