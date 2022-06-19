<%@ Page Language="VB" maintainScrollPositionOnPostBack = "true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="dispense_equipment.aspx.vb" Inherits="dispense_equipment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">

<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
                <p class="card-title card-header-th mb-1">
                    [10081] รายงานการจ่ายอุปกรณ์ที่มีการระบุ Serial Number ออกจากระบบ POS
                </p>
                <hr>
                <div class="row grid-margin stretch-card">
                    <div class="card">
                    <div class="card-body dashboard-tabs p-0">
                        <ul class="nav nav-tabs" role="tablist">
                            <li class="nav-item">
                            <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overview" aria-selected="true">ต้นหาจากประเภทสินค้า</a>
                            </li>
                            <li class="nav-item">
                            <a class="nav-link" id="sales-tab" data-toggle="tab" href="#sales" aria-selected="false">ค้นหาจากรหัสสินค้า</a>
                            </li>
                        </ul>
                        <div class="tab-content py-0 px-0">
                            <div class="tab-pane fade show active" id="overview">
                                <div class="d-flex flex-wrap justify-content-xl-between">
                                    <div class="col-12 mt-3">
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">วันที่</label>
                                        <div class="col-lg-4 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">จาก</div>
                                                </div>
                                                <input ID="SDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                                                <asp:TextBox ID="SDate" visible="true" Name="tbxSDate" runat="server" cssclass="form-control d-none" />
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">ถึง</div>
                                                </div>
                                                <input ID="EDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                                                <asp:TextBox ID="EDate" visible="true" Name="tbxEDate" runat="server" cssclass="form-control d-none" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">บริษัท</label>
                                        <div class="col-lg-10 col-sm-12">
                                            <asp:DropDownList ID="ddlCompany" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label d-none">ครัสเตอร์</label>
                                        <div class="col-lg-5 col-sm-12 d-none">
                                            <asp:DropDownList ID="ddlCluster" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                        <label class="col-lg-2 col-sm-12 col-form-label">เขต</label>
                                        <div class="col-lg-4 col-sm-12">
                                            <asp:DropDownList ID="ddlRO" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                        <label class="col-lg-2 col-sm-12 col-form-label">จังหวัด</label>
                                        <div class="col-lg-4 col-sm-12">
                                            <asp:DropDownList ID="ddlProvince" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">สำนักงาน</label>
                                        <div class="col-lg-10 col-sm-12">
                                            <asp:DropDownList ID="ddlShop" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">กลุ่มหลักอุปกรณ์</label>
                                        <div class="col-lg-10 col-sm-12">
                                            <asp:DropDownList ID="ddlMainItem" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">กลุ่มย่อยอุปกรณ์</label>
                                        <div class="col-lg-10 col-sm-12">
                                            <asp:DropDownList ID="ddlSubItem" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">อุปกรณ์</label>
                                        <div class="col-lg-10 col-sm-12">
                                            <asp:DropDownList ID="ddlItem" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-6 col-sm-12">
                                            <asp:Button ID="btnSearch" Class="btn btn-success float-right" runat="server" Text="ค้นหา"/>
                                        </div>
                                    </div>                           
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" id="sales">
                                <div class="d-flex flex-wrap justify-content-xl-between">
                                <div class="col-md-12 grid-margin mt-3">
                                    <div class="form-group">
                                        <label>ค้นหาจาก Serial</label>
                                        <asp:TextBox ID="txtSerial" runat="server" Class="form-control"></asp:TextBox>
                                    </div>
                                        <asp:Button ID="btnSearchSerial" runat="server" Class="btn btn-success float-right" Text="ค้นหา Serial" />              
                                        <asp:Label ID="lblError" runat="server" Text="Label" Visible="false"></asp:Label>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
                <div class="alert alert-danger alert-dismissible fade show lh-md font-mitr">
                    <span class="badge badge-danger fs-1">หมายเหตุ</span>
                    <u>** รายงานนี้จะแสดงข้อมูลที่มีการจ่ายออกจากระบบ POS ผ่านทาง OTC, ใบเบิก และใบปรับปรุงลด</u>       
                </div>
            </div>
        </div>
        </div>
    </div>
</form>
</asp:Content>
