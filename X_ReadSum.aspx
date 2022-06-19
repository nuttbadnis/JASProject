<%@ Page Language="VB" maintainScrollPositionOnPostBack = "true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="X_ReadSum.aspx.vb" Inherits="X_ReadSum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">

<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
                <p class="card-title card-header-th mb-1">
                    [21015] รายงาน Summary X-Read
                </p>
                <hr>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">ช่วงวันที่</label>
                    <div class="col-lg-4 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <input ID="SDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                            <asp:TextBox ID="SDate" visible="true" Name="SDate" runat="server" cssclass="form-control d-none" />
                        </div>
                    </div>
                    <div class="col-lg-4 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <input ID="EDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                            <asp:TextBox ID="EDate" visible="true" Name="EDate" runat="server" cssclass="form-control d-none" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">บริษัท</label>
                    <div class="col-lg-5 col-sm-12">
                        <asp:DropDownList ID="cboCompany" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">เขตธุรกิจ</label>
                    <div class="col-lg-5 col-sm-12">
                        <asp:DropDownList ID="cboArea" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">ช่วงจังหวัด</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="cboSProvince" runat="server" cssclass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="cboEProvince" runat="server" cssclass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">ช่วงสาขา</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="cboSBranch" runat="server" cssclass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="cboEBranch" runat="server" cssclass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">Group by</label>
                    <div class="col-lg-10 col-sm-12 form-inline">
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="GroupShop" runat="server" Text="Shop" Checked="True" GroupName="Group by" AutoPostBack="True" />
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="GroupDate" runat="server" Text="Date" GroupName="Group by" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-6">
                        <asp:CheckBox ID="CheckCookies" class="col-form-label col-form-label-sm" runat="server" Checked="True" 
                        Text="&nbsp;เก็บค่าการเลือกพารามิเตอร์ในเครื่องของคุณ" />
                    </div>
                    <div class="col-6">
                        <asp:Button ID="cmdPreview" Class="btn btn-success float-right" runat="server" Text="Preview"/>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </div>
</form>
</asp:Content>

