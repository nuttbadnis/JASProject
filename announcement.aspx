<%@ Page Language="VB" AutoEventWireup="false" CodeFile="announcement.aspx.vb" Inherits="announcement" %>

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
	.dataTable{
		table-layout: fixed;
	}
	.my-table th::before {
    content: '';
    display: block;
    min-width: 100px;
}
</style>
<body>
	<form id="form1" runat="server" enctype="multipart/form-data">
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
			<%-- <li class="nav-item nav-search d-none d-lg-block w-100">
				<div class="input-group">
				<div class="input-group-prepend">
					<span class="input-group-text" id="search">
					<i class="mdi mdi-magnify"></i>
					</span>
				</div>
				<input type="text" class="form-control" placeholder="Search now" aria-label="search" aria-describedby="search">
				</div>
			</li> --%>
			</ul>
			<ul class="navbar-nav navbar-nav-right">
			<%-- <li class="nav-item dropdown mr-1">
				<a class="nav-link count-indicator dropdown-toggle d-flex justify-content-center align-items-center" id="messageDropdown" href="#" data-toggle="dropdown">
				<i class="mdi mdi-message-text mx-0"></i>
				<span class="count"></span>
				</a>
				<div class="dropdown-menu dropdown-menu-right navbar-dropdown" aria-labelledby="messageDropdown">
				<p class="mb-0 font-weight-normal float-left dropdown-header">Messages</p>
				<a class="dropdown-item">
					<div class="item-thumbnail">
						<img src="App_Inc/majestic/images/faces/face4.jpg" alt="image" class="profile-pic">
					</div>
					<div class="item-content flex-grow">
					<h6 class="ellipsis font-weight-normal">David Grey
					</h6>
					<p class="font-weight-light small-text text-muted mb-0">
						The meeting is cancelled
					</p>
					</div>
				</a>
				<a class="dropdown-item">
					<div class="item-thumbnail">
						<img src="App_Inc/majestic/images/faces/face2.jpg" alt="image" class="profile-pic">
					</div>
					<div class="item-content flex-grow">
					<h6 class="ellipsis font-weight-normal">Tim Cook
					</h6>
					<p class="font-weight-light small-text text-muted mb-0">
						New product launch
					</p>
					</div>
				</a>
				<a class="dropdown-item">
					<div class="item-thumbnail">
						<img src="App_Inc/majestic/images/faces/face3.jpg" alt="image" class="profile-pic">
					</div>
					<div class="item-content flex-grow">
					<h6 class="ellipsis font-weight-normal"> Johnson
					</h6>
					<p class="font-weight-light small-text text-muted mb-0">
						Upcoming board meeting
					</p>
					</div>
				</a>
				</div>
			</li> --%>
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
				<%-- <a class="dropdown-item">
					<div class="item-thumbnail">
					<div class="item-icon bg-success">
						<i class="mdi mdi-information mx-0"></i>
					</div>
					</div>
					<div class="item-content">
					<h6 class="font-weight-normal">Application Error</h6>
					<p class="font-weight-light small-text mb-0 text-muted">
						Just now
					</p>
					</div>
				</a>
				<a class="dropdown-item">
					<div class="item-thumbnail">
					<div class="item-icon bg-warning">
						<i class="mdi mdi-settings mx-0"></i>
					</div>
					</div>
					<div class="item-content">
					<h6 class="font-weight-normal">Settings</h6>
					<p class="font-weight-light small-text mb-0 text-muted">
						Private message
					</p>
					</div>
				</a>
				<a class="dropdown-item">
					<div class="item-thumbnail">
					<div class="item-icon bg-info">
						<i class="mdi mdi-account-box mx-0"></i>
					</div>
					</div>
					<div class="item-content">
					<h6 class="font-weight-normal">New user registration</h6>
					<p class="font-weight-light small-text mb-0 text-muted">
						2 days ago
					</p>
					</div>
				</a> --%>
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
		<nav class="sidebar sidebar-offcanvas" id="sidebar">
			<ul class="nav">
			<li class="nav-item">
				<a class="nav-link" href="default.aspx">
				<i class="mdi mdi-home menu-icon"></i>
				<span class="menu-title">Dashboard</span>
				</a>
			</li>
			<li class="nav-item active">
				<a class="nav-link" href="announcement.aspx">
				<i class="mdi mdi-newspaper menu-icon"></i>
				<span class="menu-title">Announcement</span>
				</a>
          	</li>
			<li class="nav-item">
				<a class="nav-link" data-toggle="collapse" href="#report">
				<i class="mdi mdi-file-document menu-icon"></i>
				<span class="menu-title">Reports</span>
				<i class="menu-arrow"></i>
				</a>
				<div class="collapse" id="report">
				<ul class="nav flex-column sub-menu">
					<li class="nav-item"> <a class="nav-link" href="RemainRouter.aspx">[10110] รายงานการขายและเบิกสินค้าค้างจ่าย</a></li>
				</ul>
				</div>
			</li>
			<%-- <li class="nav-item">
				<a class="nav-link" data-toggle="collapse" href="#ui-basic" aria-expanded="false" aria-controls="ui-basic">
				<i class="mdi mdi-circle-outline menu-icon"></i>
				<span class="menu-title">UI Elements</span>
				<i class="menu-arrow"></i>
				</a>
				<div class="collapse" id="ui-basic">
				<ul class="nav flex-column sub-menu">
					<li class="nav-item"> <a class="nav-link" href="App_Inc/majestic/pages/ui-features/buttons.html">Buttons</a></li>
					<li class="nav-item"> <a class="nav-link" href="App_Inc/majestic/pages/ui-features/typography.html">Typography</a></li>
				</ul>
				</div>
			</li>
			<li class="nav-item">
				<a class="nav-link" href="App_Inc/majestic/pages/forms/basic_elements.html">
				<i class="mdi mdi-view-headline menu-icon"></i>
				<span class="menu-title">Form elements</span>
				</a>
			</li>
			<li class="nav-item">
				<a class="nav-link" href="App_Inc/majestic/pages/charts/chartjs.html">
				<i class="mdi mdi-chart-pie menu-icon"></i>
				<span class="menu-title">Charts</span>
				</a>
			</li>
			<li class="nav-item">
				<a class="nav-link" href="App_Inc/majestic/pages/tables/basic-table.html">
				<i class="mdi mdi-grid-large menu-icon"></i>
				<span class="menu-title">Tables</span>
				</a>
			</li>
			<li class="nav-item">
				<a class="nav-link" href="App_Inc/majestic/pages/icons/mdi.html">
				<i class="mdi mdi-emoticon menu-icon"></i>
				<span class="menu-title">Icons</span>
				</a>
			</li>
			<li class="nav-item">
				<a class="nav-link" data-toggle="collapse" href="#auth" aria-expanded="false" aria-controls="auth">
				<i class="mdi mdi-account menu-icon"></i>
				<span class="menu-title">User Pages</span>
				<i class="menu-arrow"></i>
				</a>
				<div class="collapse" id="auth">
				<ul class="nav flex-column sub-menu">
					<li class="nav-item"> <a class="nav-link" href="App_Inc/majestic/pages/samples/login.html"> Login </a></li>
					<li class="nav-item"> <a class="nav-link" href="App_Inc/majestic/pages/samples/login-2.html"> Login 2 </a></li>
					<li class="nav-item"> <a class="nav-link" href="App_Inc/majestic/pages/samples/register.html"> Register </a></li>
					<li class="nav-item"> <a class="nav-link" href="App_Inc/majestic/pages/samples/register-2.html"> Register 2 </a></li>
					<li class="nav-item"> <a class="nav-link" href="App_Inc/majestic/pages/samples/lock-screen.html"> Lockscreen </a></li>
				</ul>
				</div>
			</li>
			<li class="nav-item">
				<a class="nav-link" href="App_Inc/majestic/documentation/documentation.html">
				<i class="mdi mdi-file-document-box-outline menu-icon"></i>
				<span class="menu-title">Documentation</span>
				</a>
			</li> --%>
			</ul>
		</nav>
		<!-- partial -->
		<div class="main-panel">
			<div class="content-wrapper">
			<div class="row">
				<div class="col-md-12 grid-margin">
				<div class="d-flex justify-content-between flex-wrap">
					<div class="d-flex align-items-end flex-wrap">
					<div class="mr-md-3 mr-xl-5 d-none">
						<h2>Welcome back,</h2>
						<p class="mb-md-0">Your analytics dashboard template.</p>
					</div>
					<div class="d-flex">
						<i class="mdi mdi-home text-muted hover-cursor"></i>
						<p class="text-primary mb-0 hover-cursor">&nbsp;/&nbsp;Announcement</p>
					</div>
					</div>
					<%-- <div class="d-flex justify-content-between align-items-end flex-wrap">
					<button type="button" class="btn btn-light bg-white btn-icon mr-3 d-none d-md-block ">
						<i class="mdi mdi-download text-muted"></i>
					</button>
					<button type="button" class="btn btn-light bg-white btn-icon mr-3 mt-2 mt-xl-0">
						<i class="mdi mdi-clock-outline text-muted"></i>
					</button>
					<button type="button" class="btn btn-light bg-white btn-icon mr-3 mt-2 mt-xl-0">
						<i class="mdi mdi-plus text-muted"></i>
					</button>
					<button class="btn btn-primary mt-2 mt-xl-0">Download report</button>
					</div> --%>
				</div>
				</div>
			</div>
			<div class="row">
				<div class="col-md-12 grid-margin stretch-card d-none">
				<div class="card">
					<div class="card-body dashboard-tabs p-0">
					<ul class="nav nav-tabs px-4" role="tablist">
						<li class="nav-item">
						<a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overview" role="tab" aria-controls="overview" aria-selected="true">Overview</a>
						</li>
						<li class="nav-item">
						<a class="nav-link" id="sales-tab" data-toggle="tab" href="#sales" role="tab" aria-controls="sales" aria-selected="false">Sales</a>
						</li>
						<li class="nav-item">
						<a class="nav-link" id="purchases-tab" data-toggle="tab" href="#purchases" role="tab" aria-controls="purchases" aria-selected="false">Purchases</a>
						</li>
					</ul>
					<div class="tab-content py-0 px-0">
						<div class="tab-pane fade show active" id="overview" role="tabpanel" aria-labelledby="overview-tab">
						<div class="d-flex flex-wrap justify-content-xl-between">
							<div class="d-none d-xl-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-calendar-heart icon-lg mr-3 text-primary"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Start date</small>
								<div class="dropdown">
								<a class="btn btn-secondary dropdown-toggle p-0 bg-transparent border-0 text-dark shadow-none font-weight-medium" href="#" role="button" id="dropdownMenuLinkA" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
									<h5 class="mb-0 d-inline-block">26 Jul 2018</h5>
								</a>
								<div class="dropdown-menu" aria-labelledby="dropdownMenuLinkA">
									<a class="dropdown-item" href="#">12 Aug 2018</a>
									<a class="dropdown-item" href="#">22 Sep 2018</a>
									<a class="dropdown-item" href="#">21 Oct 2018</a>
								</div>
								</div>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-currency-usd mr-3 icon-lg text-danger"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Revenue</small>
								<h5 class="mr-2 mb-0">$577545</h5>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-eye mr-3 icon-lg text-success"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Total views</small>
								<h5 class="mr-2 mb-0">9833550</h5>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-download mr-3 icon-lg text-warning"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Downloads</small>
								<h5 class="mr-2 mb-0">2233783</h5>
							</div>
							</div>
							<div class="d-flex py-3 border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-flag mr-3 icon-lg text-danger"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Flagged</small>
								<h5 class="mr-2 mb-0">3497843</h5>
							</div>
							</div>
						</div>
						</div>
						<div class="tab-pane fade" id="sales" role="tabpanel" aria-labelledby="sales-tab">
						<div class="d-flex flex-wrap justify-content-xl-between">
							<div class="d-none d-xl-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-calendar-heart icon-lg mr-3 text-primary"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Start date</small>
								<div class="dropdown">
								<a class="btn btn-secondary dropdown-toggle p-0 bg-transparent border-0 text-dark shadow-none font-weight-medium" href="#" role="button" id="dropdownMenuLinkA" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
									<h5 class="mb-0 d-inline-block">26 Jul 2018</h5>
								</a>
								<div class="dropdown-menu" aria-labelledby="dropdownMenuLinkA">
									<a class="dropdown-item" href="#">12 Aug 2018</a>
									<a class="dropdown-item" href="#">22 Sep 2018</a>
									<a class="dropdown-item" href="#">21 Oct 2018</a>
								</div>
								</div>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-download mr-3 icon-lg text-warning"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Downloads</small>
								<h5 class="mr-2 mb-0">2233783</h5>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-eye mr-3 icon-lg text-success"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Total views</small>
								<h5 class="mr-2 mb-0">9833550</h5>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-currency-usd mr-3 icon-lg text-danger"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Revenue</small>
								<h5 class="mr-2 mb-0">$577545</h5>
							</div>
							</div>
							<div class="d-flex py-3 border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-flag mr-3 icon-lg text-danger"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Flagged</small>
								<h5 class="mr-2 mb-0">3497843</h5>
							</div>
							</div>
						</div>
						</div>
						<div class="tab-pane fade" id="purchases" role="tabpanel" aria-labelledby="purchases-tab">
						<div class="d-flex flex-wrap justify-content-xl-between">
							<div class="d-none d-xl-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-calendar-heart icon-lg mr-3 text-primary"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Start date</small>
								<div class="dropdown">
								<a class="btn btn-secondary dropdown-toggle p-0 bg-transparent border-0 text-dark shadow-none font-weight-medium" href="#" role="button" id="dropdownMenuLinkA" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
									<h5 class="mb-0 d-inline-block">26 Jul 2018</h5>
								</a>
								<div class="dropdown-menu" aria-labelledby="dropdownMenuLinkA">
									<a class="dropdown-item" href="#">12 Aug 2018</a>
									<a class="dropdown-item" href="#">22 Sep 2018</a>
									<a class="dropdown-item" href="#">21 Oct 2018</a>
								</div>
								</div>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-currency-usd mr-3 icon-lg text-danger"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Revenue</small>
								<h5 class="mr-2 mb-0">$577545</h5>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-eye mr-3 icon-lg text-success"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Total views</small>
								<h5 class="mr-2 mb-0">9833550</h5>
							</div>
							</div>
							<div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-download mr-3 icon-lg text-warning"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Downloads</small>
								<h5 class="mr-2 mb-0">2233783</h5>
							</div>
							</div>
							<div class="d-flex py-3 border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
							<i class="mdi mdi-flag mr-3 icon-lg text-danger"></i>
							<div class="d-flex flex-column justify-content-around">
								<small class="mb-1 text-muted">Flagged</small>
								<h5 class="mr-2 mb-0">3497843</h5>
							</div>
							</div>
						</div>
						</div>
					</div>
					</div>
				</div>
				</div>
			</div>
			<div class="row">
				<div class="col-md-12 grid-margin stretch-card">
				<div class="card">
					<div class="card-body">
					<p class="card-title card-header-th mb-1">
						ประกาศ <span class="badge badge-danger align-text-top">new</span>
						<span id="announcement_total" runat="server" class="float-right fs-1">
						</span>
					</p>
					<hr>
					<table id="news_table" class="table table-sm" style="width:100%">
						<thead>
							<tr>
								<th width="100%">test</th>
							</tr>
						</thead>
						<tbody>
							<span id="announcement_list" runat="server"></span>
						<tbody>
					 </table>
				</div>
				</div>
			</div>
			</div>
			<!-- content-wrapper ends -->
			<!-- partial:partials/_footer.html -->
			<footer class="footer">
			<div class="d-sm-flex justify-content-center justify-content-sm-between">
				<span class="text-muted text-center text-sm-left d-block d-sm-inline-block">Copyright © 2021 by <a href="mailto:support_pos@jasmine.com">support_pos@jasmine.com</a></span>
				<%-- <span class="float-none float-sm-right d-block mt-1 mt-sm-0 text-center">Hand-crafted & made with <i class="mdi mdi-heart text-danger"></i></span> --%>
			</div>
			</footer>
			<!-- partial -->
		</div>
		<!-- main-panel ends -->
		</div>
		<!-- page-body-wrapper ends -->
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
	$(document).ready(function() {
		$('#news_table').DataTable({
			"autoWidth": false,
			"ordering": false,
			"language": {
      			"emptyTable": "ไม่มีข้อมูลข่าวสาร",
				"class": "text-center"
    		}
			<%-- "scrollX": true --%>
			
		});
		$('#news_table').addClass("NewClassName").removeClass('dataTable');
	});
</script>

</html>
