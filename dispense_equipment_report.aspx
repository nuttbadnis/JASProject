<%@ Page Language="VB" AutoEventWireup="false" CodeFile="dispense_equipment_report.aspx.vb" Inherits="dispense_equipment_report" %>

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
                                    <span class="badge badge-outline-primary">[10081] รายงานการจ่ายอุปกรณ์ที่มีการระบุ Serial Number ออกจากระบบ POS</span>
                                </small>
                                <small class="text-muted mb-1">
                                    <asp:Label ID="comp_param" class="badge badge-outline-danger" runat="server"></asp:Label>
                                </small>
                            </div>
                        </div>
                        <div class="d-flex border-md-right align-items-start flex-grow-1 p-3 justify-content-end">
                            <div class="d-flex flex-column align-items-end">
                            <small class="text-muted">
                                <asp:Label ID="lblServerTime" runat="server"></asp:Label>
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
                        <div id="display_table">
                            <asp:GridView ID="GridView1" cssclass="table table-sm" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="Doc_date" HeaderText="วันที่เอกสาร" DataFormatString="{0:dd-MM-yyyy}" >
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RO" HeaderText="เขต" />
                                    <asp:BoundField DataField="CLUSTER" HeaderText="ครัสเตอร์" >
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROVINCE_NAME" HeaderText="จังหวัด" />
                                    <asp:BoundField DataField="SHOP_CODE" HeaderText="รหัสสำนักงาน" />
                                    <asp:BoundField DataField="SHOP_NAME" HeaderText="ชื่อสำนักงาน" />
                                    <asp:BoundField DataField="Doc_Type" HeaderText="ประเภทเอกสาร" />
                                    <asp:BoundField DataField="Doc_Code" HeaderText="เลขที่เอกสาร" />
                                    <asp:BoundField DataField="Withdrawal_Type" HeaderText="ประเภทการเบิก" />
                                    <asp:BoundField DataField="Withdrawal_Type_Name" HeaderText="ชื่อประเภทการเบิก" />
                                    <asp:BoundField DataField="Referent" HeaderText="ข้อมูลอ้างอิง" />
                                    <asp:BoundField DataField="Item_Code" HeaderText="รหัสอุปกรณ์" />
                                    <asp:BoundField DataField="Item_Name" HeaderText="ชื่ออุปกรณ์" />
                                    <asp:BoundField DataField="Serial" HeaderText="หมายเลขซีเรียล" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div ID="lblNoData" runat="server" Visible="False" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr text-center">
                            <h5><i class="mdi mdi-alert-circle icon-md"></i> ไม่พบข้อมูลที่ค้นหา</h5>
                        </div>
                        <div ID="lblError_view" runat="server" Visible="False" class="alert alert-danger alert-dismissible fade show lh-md mt-3 font-mitr text-center">
                            <asp:Label ID="lblError" runat="server" Text="Label" Visible="False"></asp:Label>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
