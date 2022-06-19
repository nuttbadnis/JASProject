<%@ Page Language="VB" MaintainScrollPositionOnPostBack = "true" AutoEventWireup="true" CodeFile="cheque_BB.aspx.vb" Inherits="WebApplication2.cheque_BB" %>

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
</style>
<body>
    <form id="form1" runat="server">
    <div class="container-scroller">
    <!-- partial:partials/_navbar.html -->
		<nav class="navbar col-lg-12 col-12 p-0 fixed-top d-flex flex-row">
				<div class="navbar-brand-wrapper d-flex justify-content-center">
			<div class="navbar-brand-inner-wrapper d-flex justify-content-between align-items-center w-100 ml-3">  
			<a class="navbar-brand brand-logo" href="default.aspx">
				<img src="App_Inc/majestic/images/3BB_logo_popup.png" style="height: 40px;" alt="logo"/>
				<span class="text-warp" style="line-height: 20px;font-size: 18px;font-weight: bold;color: #004280;display: inline-block;vertical-align: middle;">
				POSWEB<br><small><b>FOR BEST</b></small></span>        
				<a class="navbar-brand brand-logo-mini" href="default.aspx">
				<img src="App_Inc/majestic/images/3BB_logo_popup.png" alt="logo" style="max-width: 150%;height: 20px;"/></a>
			<button class="navbar-toggler navbar-toggler align-self-center" type="button" data-toggle="minimize">
				<span class="mdi mdi-sort-variant"></span>
			</button>
			</div>  
		</div>
		<div class="navbar-menu-wrapper d-flex align-items-center justify-content-end">
			<ul class="navbar-nav mr-lg-4 w-100">
			</ul>
			<ul class="navbar-nav navbar-nav-right">
			<li class="nav-item dropdown mr-4">
				<a class="nav-link count-indicator dropdown-toggle d-flex align-items-center justify-content-center notification-dropdown" id="notificationDropdown" href="#" data-toggle="dropdown">
				<i class="mdi mdi-bell mx-0"></i>
				<span class="count"></span>
				</a>
				<div class="dropdown-menu dropdown-menu-right navbar-dropdown" aria-labelledby="notificationDropdown">
				<p class="mb-0 font-weight-normal float-left dropdown-header">Notifications</p>
				<a class="dropdown-item">
					<div class="item-thumbnail">
						<div class="item-icon bg-success">
							<i class="mdi mdi-information mx-0"></i>
						</div>
					</div>
					<div class="item-content">
					<h6 class="font-weight-normal">None Notifications</h6>
					</div>
				</a>
				</div>
			</li>
			<li class="nav-item nav-profile dropdown">
				<a class="nav-link dropdown-toggle" href="#" data-toggle="dropdown" id="profileDropdown">
				<img src="App_Inc/majestic/images/faces/face5.jpg" alt="profile"/>
				<span id="profile_name" runat="server" class="nav-profile-name"></span>
				</a>
				<div class="dropdown-menu dropdown-menu-right navbar-dropdown" aria-labelledby="profileDropdown">
				<%-- <a class="dropdown-item">
					<i class="mdi mdi-settings text-primary"></i>
					Settings
				</a> --%>
				<a class="dropdown-item">
					<i class="mdi mdi-logout text-primary"></i>
					Logout
				</a>
				</div>
			</li>
			</ul>
			<button class="navbar-toggler navbar-toggler-right d-lg-none align-self-center" type="button" data-toggle="offcanvas">
			<span class="mdi mdi-menu"></span>
			</button>
		</div>
		</nav>
		<!-- partial -->
		<div class="container-fluid page-body-wrapper">
		<!-- partial:partials/_sidebar.html -->
		<nav class="sidebar sidebar-offcanvas font-mitr" id="sidebar">
			<ul class="nav">
			<li class="nav-item d-none">
				<a class="nav-link" href="default.aspx">
				<i class="mdi mdi-home menu-icon"></i>
				<span class="menu-title">Dashboard</span>
				</a>
			</li>
			<li class="nav-item d-none">
				<a class="nav-link" href="announcement.aspx">
				<i class="mdi mdi-newspaper menu-icon"></i>
				<span class="menu-title">Announcement</span>
				</a>
          	</li>
			<li class="nav-item active d-none">
				<a class="nav-link" data-toggle="collapse" href="#report">
				<i class="mdi mdi-file-document menu-icon"></i>
				<span class="menu-title">Reports</span>
				<i class="menu-arrow"></i>
				</a>
				<div class="collapse" id="report">
				<ul class="nav flex-column sub-menu">
					<li class="nav-item"> <a class="nav-link" href="RemainRouter.aspx">[10110] รายงานการขาย</br>และเบิกสินค้าค้างจ่าย</a></li>
				</ul>
				</div>
			</li>
            <li class="nav-item active">
                <a class="nav-link" href="RemainRouter.aspx">
                <i class="mdi mdi-file-document menu-icon"></i>
                <span class="menu-title text-wrap">[10110] รายงานการขาย และเบิกสินค้าค้างจ่าย</span>
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="pmtTransferDetail.aspx">
                <i class="mdi mdi-file-document menu-icon"></i>
                <span class="menu-title text-wrap">[10096] รายงานการโอนสินค้า ระหว่างสำนักงานแบบระบุรหัสสินค้า</span>
                </a>
            </li>
			</ul>
		</nav>
		<!-- partial -->
		<div class="main-panel">
			<div class="content-wrapper">
			<div class="row">
				<div class="col-md-12 grid-margin">
				<div class="d-flex justify-content-between flex-wrap">
					<div class="d-flex align-items-end flex-wrap">
					<div class="d-flex">
						<i class="mdi mdi-home text-muted hover-cursor"></i>
						<p class="text-primary mb-0 hover-cursor">&nbsp;/&nbsp;Report</p>
					</div>
					</div>
				</div>
				</div>
			</div>

			<div class="row">
				<div class="col-md-12 grid-margin stretch-card">
				<div class="card font-mitr">
					<div class="card-body">
					<p class="card-title card-header-th mb-1">
						[22020] รายงานรับเช็ค
					</p>
					<hr>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">วันที่เอกสาร</label>
                        <div class="col-lg-4 col-sm-12">
                            <div class="input-group mb-2">
                                <div class="input-group-prepend">
                                    <div class="input-group-text bg-primary text-white">จาก</div>
                                </div>
                                <input ID="startdate" type="date" class="form-control" onchange="getcalendar(this);"/>
                                <asp:TextBox ID="startdate" visible="true" Name="SDate" runat="server" class="form-control d-none" />
                            </div>
                        </div>
                        <div class="col-lg-4 col-sm-12">
                            <div class="input-group mb-2">
                                <div class="input-group-prepend">
                                    <div class="input-group-text bg-primary text-white">ถึง</div>
                                </div>
                                <input ID="enddate" type="date" class="form-control" onchange="getcalendar(this);"/>
                                <asp:TextBox ID="enddate" visible="true" Name="EDate" runat="server" class="form-control d-none" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">บริษัท</label>
                        <div class="col-lg-5 col-sm-12">
                            <asp:DropDownList ID="company" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">ช่วงเขตการขาย</label>
                        <div class="col-lg-10 col-sm-12">
                            <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="area" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                            </div>
                        </div>
                        <label class="col-lg-2 col-sm-12 col-form-label"></label>
                        <div class="col-lg-10 col-sm-12">
                            <div class="input-group mb-2">
                            <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="area1" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                            </div>
                        </div>
                    </div>                   
    <div>
        &nbsp; &nbsp;&nbsp; &nbsp;
        <table style="width: 728px; height: 368px; font-weight: bold; font-size: small; color: #000099; font-family: Arial; left: 0px; position: relative; top: 0px;" title="รายงานรับเช็ค">
            <tr>
                <td style="height: 43px; width: 104px;">
                    ช่วงจังหวัด :</td>
                <td style="height: 43px; width: 291px;">
                    จาก :
                    <asp:DropDownList ID="prov" runat="server" AutoPostBack="True" Width="180px">
                    </asp:DropDownList></td>
                <td style="height: 43px; width: 300px;">
                    ถึง :
                    <asp:DropDownList ID="prov1" runat="server" AutoPostBack="True" Width="180px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 104px; height: 45px">
                    ช่วงสาขา :</td>
                <td style="width: 291px; height: 45px">
                    จาก :
                    <asp:DropDownList ID="shop" runat="server" AutoPostBack="True" Width="258px">
                    </asp:DropDownList></td>
                <td style="width: 300px; height: 45px">
                    ถึง :
                    <asp:DropDownList ID="shop1" runat="server" AutoPostBack="True" Width="258px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 300px; height: 41px">
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" Visible="False" />&nbsp;
                    <asp:DropDownList ID="cboFormat" runat="server" Visible="False">
                        <asp:ListItem>--โปรดเลือก Format--</asp:ListItem>
                        <asp:ListItem>PDF</asp:ListItem>
                        <asp:ListItem>Excel</asp:ListItem>
                        <asp:ListItem>Excel (Data Only)</asp:ListItem>
                        <asp:ListItem>Word</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
        </table>
    
    </div>
                    <div class="row mb-3">
                        <div class="col-6">
                            <asp:CheckBox ID="chkParameter" class="col-form-label col-form-label-sm" runat="server" Checked="True" 
                            Text="&nbsp;จำพารามิเตอร์ที่เลือกไว้ในเครื่องของคุณ" />
                        </div>
                        <div class="col-6">
                            <asp:Button ID="Clear" Class="btn btn-success float-right" runat="server" Text="Clear" />
                            <asp:Button ID="OK" Class="btn btn-success float-right" runat="server" OnClientClick="return chkPreview();" Text="Preview"/>
                        </div>
                    </div>
				</div>
				</div>
			</div>
                <div class="alert alert-danger alert-dismissible fade show lh-md font-mitr">
                    <span class="badge badge-danger fs-1">หมายเหตุ</span>
                    <u>รายงานที่ลงท้ายด้วย (C) จะแสดงในรูปแบบ Crystal Report รายงานใดที่ไม่มีต่อท้าย จะแสดงในรูปแบบ Web</u>       
                </div>
			</div>
			<!-- content-wrapper ends -->
			<!-- partial:partials/_footer.html -->
			<footer class="footer">
			<div class="d-sm-flex justify-content-center justify-content-sm-between">
				<span class="text-muted text-center text-sm-left d-block d-sm-inline-block">Copyright ? 2021 by <a href="mailto:support_pos@jasmine.com">support_pos@jasmine.com</a></span>
				<%-- <span class="float-none float-sm-right d-block mt-1 mt-sm-0 text-center">Hand-crafted & made with <i class="mdi mdi-heart text-danger"></i></span> --%>
			</div>
			</footer>
			<!-- partial -->
		</div>
		<!-- main-panel ends -->
		</div>
		<!-- page-body-wrapper ends -->
  	</div>

    </div>
    <!-- container-scroller -->

    <!-- plugins:js -->
    <script src="App_Inc/majestic/vendors/base/vendor.bundle.base.js"></script>
    <!-- endinject -->
    <!-- Plugin js for this page-->
    <script src="App_Inc/majestic/vendors/chart.js/Chart.min.js"></script>
    <script src="App_Inc/majestic/vendors/datatables.net/jquery.dataTables.js"></script>
    <script src="App_Inc/majestic/vendors/datatables.net-bs4/dataTables.bootstrap4.js"></script>
    <!-- End plugin js for this page-->
    <!-- inject:js -->
    <script src="App_Inc/majestic/js/off-canvas.js"></script>
    <script src="App_Inc/majestic/js/hoverable-collapse.js"></script>
    <script src="App_Inc/majestic/js/template.js"></script>
    <!-- endinject -->
    <!-- Custom js for this page-->
    <script src="App_Inc/majestic/js/dashboard.js"></script>
    <script src="App_Inc/majestic/js/data-table.js"></script>
    <script src="App_Inc/majestic/js/jquery.dataTables.js"></script>
    <script src="App_Inc/majestic/js/dataTables.bootstrap4.js"></script>
    <!-- End custom js for this page-->
    </form>
</body>
<script>
    function getcalendar(x){
        var id = x.id;
        var value = x.value;
        console.log("calendar running...");
        console.log("id : "+ id);
        console.log("value : "+ value);
        $("input[name="+id+"]").val(value);
    }
</script>
</html>
