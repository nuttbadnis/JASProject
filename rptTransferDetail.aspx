<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptTransferDetail.aspx.vb" Inherits="rptTransferDetail" %>

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
                            <asp:Label ID="branch_param" class="badge badge-pill badge-danger mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="group_param" class="badge badge-pill badge-warning mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="subgroup_param" class="badge badge-pill bg-linkedin text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="product_param" class="badge badge-pill bg-dribbble text-light mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="date_param" class="badge badge-pill bg-pinterest text-light mb-1 mr-1" runat="server"></asp:Label>                        
                        </div>
                        <div id="display_table">
                            <asp:GridView ID="gvwData" runat="server" CssClass="table table-sm table-striped" BorderWidth="1px" AutoGenerateColumns="False">
                                <RowStyle BackColor="White" ForeColor="#003399" />
                                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <HeaderStyle cssclass="table-primary fs-light" font-size="14px"/>
                                <Columns>
                                    <asp:BoundField DataField="company" HeaderText="บริษัท" ReadOnly="True" SortExpression="company" />
                                    <asp:BoundField DataField="shop" HeaderText="สำนักงานโอนออก" ReadOnly="True" SortExpression="shop" />
                                    <asp:BoundField DataField="Destination_shop" HeaderText="สำนักงานรับโอน" ReadOnly="True"
                                        SortExpression="Destination_shop" />
                                    <asp:BoundField DataField="doc_date" HeaderText="วันที่โอนอุปกรณ์" ReadOnly="True"
                                        SortExpression="doc_date" HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="doc" HeaderText="เอกสารโอนออกอุปกรณ์" ReadOnly="True"
                                        SortExpression="doc" />
                                    <asp:BoundField DataField="doc_status" HeaderText="สถานะเอกสาร" ReadOnly="True" SortExpression="doc_status" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="item_group" HeaderText="กลุ่มหลักสินค้า" ReadOnly="True"
                                        SortExpression="item_group" />
                                    <asp:BoundField DataField="item_subgroup" HeaderText="กลุ่มย่อยสินค้า" ReadOnly="True"
                                        SortExpression="item_subgroup" />
                                    <asp:BoundField DataField="item" HeaderText="รหัสสินค้า" ReadOnly="True" SortExpression="item" />
                                    <asp:BoundField DataField="item_name" HeaderText="ชื่อสินค้า" ReadOnly="True" SortExpression="item_name" />
                                    <asp:BoundField DataField="item_quantity" HeaderText="จำนวนสินค้า" ReadOnly="True"
                                        SortExpression="item_quantity" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="item_unit" HeaderText="หน่วยของสินค้า" ReadOnly="True"
                                        SortExpression="item_unit" />
                                    <asp:BoundField DataField="warehouse" HeaderText="คลังย่อย" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank"><i class="mdi mdi-cast-connected icon-md"></i></asp:HyperLink>
                                            <asp:Label ID="lblSeq" runat="server" Text='<%# Bind("sequence") %>' Visible="False"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                                SERIAL
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div ID="NoQuery" runat="server" Visible="False" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr text-center">
                            <h5><i class="mdi mdi-alert-circle icon-md"></i> ไม่พบข้อมูลที่ค้นหา</h5>
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
                                    <li><u>รายงานตัวนี้จะแสดงข้อมูลโดยใช้การอ้างอิงจาก ITEM สินค้าโยงไปยังเอกสาร ดังนั้นในตัวเอกสารของโปรแกรม POS อาจมีรายการสินค้าอื่นนอกจาก ITEM ที่เลือกในรายงานอยู่ด้วย</li>
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
