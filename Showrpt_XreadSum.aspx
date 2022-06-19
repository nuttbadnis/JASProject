<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Showrpt_XreadSum.aspx.vb" Inherits="Showrpt_XreadSum" %>

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
                            <asp:Label ID="ro_param" class="badge badge-pill badge-success mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="prov_param" class="badge badge-pill badge-warning mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="shop_param" class="badge badge-pill badge-danger mb-1 mr-1" runat="server"></asp:Label></br>
                            <asp:Label ID="report_param" class="badge badge-pill badge-primary mb-1 mr-1" runat="server"></asp:Label>
                            <asp:Label ID="date_param" class="badge badge-pill bg-gradient-warning mb-1 mr-1" runat="server"></asp:Label>                           
                        </div>
                        <div id="display_table">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-sm table-striped" AutoGenerateColumns="False" ShowFooter="True" BorderWidth="1px">
                                <Columns>
                                    <asp:BoundField DataField="area" HeaderText="เขต" ItemStyle-Width="300px" ItemStyle-Wrap="false" />
                                    <%--<asp:BoundField DataField="CodeShop" HeaderText="รหัสสำนักงาน" />--%>
                                    <asp:TemplateField HeaderText="รหัสสำนักงาน">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="CodeShop" runat="server" Text='<%#Bind("CodeShop")%>'> </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BranchName" HeaderText="ชื่อสำนักงาน" ItemStyle-Width="40%" ItemStyle-Wrap="false"/>
                                    <asp:BoundField DataField="datedoc" HeaderText="วันที่เอกสาร" Visible="false"/>
                                    <asp:BoundField DataField="Cdoc" HeaderText="จำนวนเอกสาร" Visible="true" DataFormatString="{0:#,#0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CS" DataFormatString="{0:#,#0}" HeaderText="เงินสด">
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
                                    <asp:BoundField DataField="A_Type" DataFormatString="{0:#,#0.00}" HeaderText="รวมเงินรับชำระ">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AD" DataFormatString="{0:#,#0.00}" HeaderText="หักชำระ">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AI" DataFormatString="{0:#,#0.00}" HeaderText="เพิ่มชำระ">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FR" DataFormatString="{0:#,#0.00}" HeaderText="ปัดเศษ">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BillPayment" DataFormatString="{0:#,#0.00}" HeaderText="รวมรับชำระ Bill Payment">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OTC" DataFormatString="{0:#,#0.00}" HeaderText="รวมรับชำระ Onetime Charg">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SRCPV" DataFormatString="{0:#,#0.00}" HeaderText="รวมรับชำระขายสด">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                <FooterStyle cssclass="bg-warning font-weight-bold" />
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
    direction: 'horizontal'
  });
};
 setupExample1();
</script>
</html>
