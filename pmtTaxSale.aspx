<%@ Page Language="VB"  maintainScrollPositionOnPostBack = "true" AutoEventWireup="true"  MasterPageFile="~/master_request.Master" CodeFile="pmtTaxSale.aspx.vb" Inherits="pmtTaxSale"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">
<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
                <p class="card-title card-header-th mb-1">
                    [22041] รายงานภาษีขาย 
                </p>
                <hr>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">บริษัท</label>
                    <div class="col-lg-5 col-sm-12">
                        <asp:DropDownList ID="cboCompany" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">ประจำเดือน</label>
                    <div class="col-lg-4 col-sm-12">
                        <asp:DropDownList ID="cboMonth" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <label class="col-lg-2 col-sm-12 col-form-label">ประจำปี</label>
                    <div class="col-lg-4 col-sm-12">
                        <asp:DropDownList ID="cboYear" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">เขตธุรกิจ</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="cboSArea" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="cboEArea" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">จังหวัด</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="cboSProvince" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="cboEProvince" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">สำนักงาน</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="cboSBranch" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="cboEBranch" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-6">
                        <asp:CheckBox ID="chkParameter" class="col-form-label col-form-label-sm d-none" runat="server" Checked="True" 
                        Text="&nbsp;จำพารามิเตอร์ที่เลือกไว้ในเครื่องของคุณ" />
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