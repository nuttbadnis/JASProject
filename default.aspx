<%@ Page Language="VB" maintainScrollPositionOnPostBack = "true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">
	<form id="form1" runat="server" enctype="multipart/form-data" class="d-none">
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
					<p class="text-muted mb-0 hover-cursor">&nbsp;/&nbsp;Dashboard&nbsp;/&nbsp;</p>
					<p class="text-primary mb-0 hover-cursor">Analytics</p>
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
			<div class="col-md-12 grid-margin stretch-card">
			<div class="card">
				<div class="card-body">
				<p class="card-title card-header-th mb-1">
					ประกาศ <span class="badge badge-danger align-text-top">new</span>
					<span id="announcement_total" runat="server" class="float-right fs-1">
					</span>
				</p>
				<hr>
					<span id="announcement_list" runat="server"></span>
					<div class="ml-auto col-lg-3 col-12 text-right font-mitr">
						<a href="announcement.aspx" class="btn-icon-text">
						ดูประกาศทั้งหมด <i class="mdi mdi-chevron-double-right"></i>
						</a>
					</div>
				</div>
			</div>
			</div>
			<div class="col-md-5 grid-margin stretch-card d-none">
			<div class="card">
				<div class="card-body">
				<p class="card-title">Total sales</p>
				<h1>$ 28835</h1>
				<h4>Gross sales over the years</h4>
				<p class="text-muted">Today, many people rely on computers to do homework, work, and create or store useful information. Therefore, it is important </p>
				<div id="total-sales-chart-legend"></div>                  
				</div>
				<canvas id="total-sales-chart"></canvas>
			</div>
			</div>
		</div>
		<div class="row">
			<div class="col-md-12 font-mitr stretch-card">
			<div class="card">
				<div class="card-body">
				<p class="card-title card-header-th mb-1">
					รายงาน
				</p>
				<hr>
				<div class="table-responsive">
				<table class="table table-bordered">
					<thead>
						<tr>
							<th colspan="5">Report</th>
						</tr>
					</thead>
					<tbody>
					<tr class="table-danger">
					<td colspan="5">[10000] ระบบสินค้าคงคลัง</td>
					</tr>
					<tr class="table-primary">
					<td><i class="mdi mdi-checkbox-multiple-blank-circle text-danger" style=""></i></td>
					<td colspan="4">[10010] รายงานเอกสารใบเบิก-รับคืน และรับ OPMC</td>
					</tr>
					<tr class="table-warning">
					<td></td>
					<td><i class="mdi mdi-checkbox-multiple-blank-circle text-primary" style=""></i></td>
					<td colspan="3">[10011] รายงานแตก complete set และ adaptor ผ่านเอกสาร OPMC</td>
					</tr>
					<tr class="table-primary">
					<td><i class="mdi mdi-checkbox-multiple-blank-circle text-danger" style=""></i></td>
					<td colspan="4">[10020] รายการ Serial No.ของเอกสารใบรับสินค้าจาก OPMC</td>
					</tr>
					<tr class="table-primary">
					<td><i class="mdi mdi-checkbox-multiple-blank-circle text-danger" style=""></i></td>
					<td colspan="4">[10030] รายงานเปรียบเทียบสต็อกสาขา</td>
					</tr>
					<tr class="table-primary">
					<td><i class="mdi mdi-checkbox-multiple-blank-circle text-danger" style=""></i></td>
					<td colspan="4">[10040] รายงานสินค้าคงคลัง (Stock Card)</td>
					</tr>
					<tr class="table-warning">
					<td></td>
					<td><i class="mdi mdi-checkbox-multiple-blank-circle text-primary" style=""></i></td>
					<td colspan="3">[10041] รายงานin/outอุปกรณ์</td>
					</tr>
				</tbody>
				</table>
					<table id="recent-purchases-listing" class="table d-none">
					<thead>
						<tr>
							<th>Name</th>
							<th>Status report</th>
							<th>Office</th>
							<th>Price</th>
							<th>Date</th>
							<th>Gross amount</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>Jeremy Ortega</td>
							<td>Levelled up</td>
							<td>Catalinaborough</td>
							<td>$790</td>
							<td>06 Jan 2018</td>
							<td>$2274253</td>
						</tr>
						<tr>
							<td>Alvin Fisher</td>
							<td>Ui design completed</td>
							<td>East Mayra</td>
							<td>$23230</td>
							<td>18 Jul 2018</td>
							<td>$83127</td>
						</tr>
						<tr>
							<td>Emily Cunningham</td>
							<td>support</td>
							<td>Makennaton</td>
							<td>$939</td>
							<td>16 Jul 2018</td>
							<td>$29177</td>
						</tr>
						<tr>
							<td>Minnie Farmer</td>
							<td>support</td>
							<td>Agustinaborough</td>
							<td>$30</td>
							<td>30 Apr 2018</td>
							<td>$44617</td>
						</tr>
						<tr>
							<td>Betty Hunt</td>
							<td>Ui design not completed</td>
							<td>Lake Sandrafort</td>
							<td>$571</td>
							<td>25 Jun 2018</td>
							<td>$78952</td>
						</tr>
						<tr>
							<td>Myrtie Lambert</td>
							<td>Ui design completed</td>
							<td>Cassinbury</td>
							<td>$36</td>
							<td>05 Nov 2018</td>
							<td>$36422</td>
						</tr>
						<tr>
							<td>Jacob Kennedy</td>
							<td>New project</td>
							<td>Cletaborough</td>
							<td>$314</td>
							<td>12 Jul 2018</td>
							<td>$34167</td>
						</tr>
						<tr>
							<td>Ernest Wade</td>
							<td>Levelled up</td>
							<td>West Fidelmouth</td>
							<td>$484</td>
							<td>08 Sep 2018</td>
							<td>$50862</td>
						</tr>
					</tbody>
					</table>
				</div>
				</div>
			</div>
			</div>
		</div>
	</form>
</asp:Content>
