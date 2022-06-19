<%@ Page Language="VB" maintainScrollPositionOnPostBack = "true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="search_productprice.aspx.vb" Inherits="SpacialPayin_search_productprice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">

<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
                <p class="card-title card-header-th mb-1">
                    [15030] ค้นหาข้อมูลราคาสินค้า
                </p>
                <hr>
                <div class="row grid-margin stretch-card">
                    <div class="card">
                    <div class="card-body dashboard-tabs p-0">
                        <ul class="nav nav-tabs" role="tablist">
                            <li class="nav-item">
                            <a class="nav-link active" href="search_productprice.aspx">ต้นหาจากประเภทสินค้า</a>
                            </li>
                            <li class="nav-item">
                            <a class="nav-link" href="search_productprice_detail.aspx">ค้นหาจากรหัสสินค้า</a>
                            </li>
                        </ul>
                        <div class="tab-content py-0 px-0">
                            <div class="tab-pane fade show active">
                                <div class="d-flex flex-wrap justify-content-xl-between">
                                    <div class="col-12 mt-3">
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">บริษัท</label>
                                        <div class="col-lg-5 col-sm-12">
                                            <asp:DropDownList ID="ddlCompany" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">ช่วงประเภทสินค้า</label>
                                        <div class="col-lg-5 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">จาก</div>
                                                </div>
                                                <asp:DropDownList ID="SCatagory" runat="server" cssclass="form-control" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-5 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">ถึง</div>
                                                </div>
                                                <asp:DropDownList ID="ECatagory" runat="server" cssclass="form-control" AutoPostBack="True">
                                                </asp:DropDownList> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">ช่วงกลุ่มหลัก</label>
                                        <div class="col-lg-5 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">จาก</div>
                                                </div>
                                                <asp:DropDownList ID="SGroup" runat="server" cssclass="form-control" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-5 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">ถึง</div>
                                                </div>
                                                <asp:DropDownList ID="EGroup" runat="server" cssclass="form-control" AutoPostBack="True">
                                                </asp:DropDownList> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">ช่วงกลุ่มย่อย</label>
                                        <div class="col-lg-5 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">จาก</div>
                                                </div>
                                                <asp:DropDownList ID="SsubGroup" runat="server" cssclass="form-control" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-5 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">ถึง</div>
                                                </div>
                                                <asp:DropDownList ID="EsubGroup" runat="server" cssclass="form-control" AutoPostBack="True">
                                                </asp:DropDownList> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-sm-12 col-form-label">ช่วงรหัสสินค้า</label>
                                        <div class="col-lg-5 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">จาก</div>
                                                </div>
                                                <asp:DropDownList ID="SProduct" runat="server" cssclass="form-control" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-5 col-sm-12">
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text bg-primary text-white">ถึง</div>
                                                </div>
                                                <asp:DropDownList ID="EProduct" runat="server" cssclass="form-control" AutoPostBack="True">
                                                </asp:DropDownList> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-6 col-sm-12">
                                            <asp:CheckBox ID="chkParameter" class="col-form-label col-form-label-sm" runat="server" Checked="True" 
                                            Text="&nbsp;เก็บค่าการเลือกพารามิเตอร์ในเครื่องของคุณ" />
                                        </div>
                                        <div class="col-6 col-sm-12">
                                            <asp:Button ID="cmdPreview" Class="btn btn-success float-right" runat="server" Text="Preview"/>
                                        </div>
                                    </div>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade">
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </div>
</form>
</asp:Content>

