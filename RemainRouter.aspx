<%@ Page Language="VB" maintainScrollPositionOnPostBack = "true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="RemainRouter.aspx.vb" Inherits="RemainRouter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">

<form id="form1" runat="server">

    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
            <p class="card-title card-header-th mb-1">
                [10110] รายงานการขายและเบิกสินค้าค้างจ่าย
            </p>
            <hr>
            <div class="row">
                <label class="col-lg-2 col-sm-12 col-form-label">วันที่เอกสาร</label>
                <div class="col-lg-4 col-sm-12">
                    <div class="input-group input-group-sm mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">จาก</div>
                        </div>
                        <input ID="SDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                        <asp:TextBox ID="SDate" visible="true" Name="SDate" runat="server" class="form-control d-none" />
                    </div>
                </div>
                <div class="col-lg-4 col-sm-12">
                    <div class="input-group input-group-sm mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">ถึง</div>
                        </div>
                        <input ID="EDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                        <asp:TextBox ID="EDate" visible="true" Name="EDate" runat="server" class="form-control d-none" />
                    </div>
                </div>
            </div>
            <div class="row">
                <label class="col-lg-2 col-sm-12 col-form-label">Group By</label>
                <div class="col-lg-10 col-sm-12 form-inline">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="rbtArea" runat="server" Text="เขต" Checked="True" GroupName="GroupBy" AutoPostBack="True" />
                    </div>
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="rbtProvince" runat="server" Text="จังหวัด" GroupName="GroupBy" AutoPostBack="True" />
                    </div>
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="rbtDoc" runat="server" Text="เอกสาร" GroupName="GroupBy" AutoPostBack="True" />
                    </div>
                    <div class="form-check form-check-inline invisible">
                        <asp:RadioButton ID="rbtCluster" runat="server" Text="Cluster" GroupName="GroupBy" AutoPostBack="True" />
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
                <label class="col-lg-2 col-sm-12 col-form-label">เขต</label>
                <div class="col-lg-5 col-sm-12">
                    <asp:DropDownList ID="cboSArea" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row d-none">
                <label class="col-lg-2 col-sm-12 col-form-label">Cluster</label>
                <div class="col-lg-5 col-sm-12">
                    <div class="input-group input-group-sm mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">จาก</div>
                        </div>
                        <asp:DropDownList ID="cboSCluster" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-lg-5 col-sm-12">
                    <div class="input-group input-group-sm mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">ถึง</div>
                        </div>
                        <asp:DropDownList ID="cboECluster" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
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
                <div class="col-lg-10 col-sm-12">
                    <div class="input-group input-group-sm mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">จาก</div>
                        </div>
                        <asp:DropDownList ID="cboSBranch" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
                <label class="col-lg-2 col-sm-12 col-form-label"></label>
                <div class="col-lg-10 col-sm-12">
                    <div class="input-group input-group-sm mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">ถึง</div>
                        </div>
                        <asp:DropDownList ID="cboEBranch" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <label class="col-lg-2 col-sm-12 col-form-label">รหัสสินค้า</label>
                <div class="col-lg-10 col-sm-12">
                    <div class="input-group input-group-sm mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">จาก</div>
                        </div>
                        <asp:DropDownList ID="cboSItem" runat="server" cssclass="form-control" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <label class="col-lg-2 col-sm-12 col-form-label"></label>
                <div class="col-lg-10 col-sm-12">
                    <div class="input-group input-group-sm mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">ถึง</div>
                        </div>
                        <asp:DropDownList ID="cboEItem" runat="server" cssclass="form-control" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row mb-3">
                <div class="col-6">
                    <asp:CheckBox ID="chkParameter" class="col-form-label col-form-label-sm" runat="server" Checked="True" 
                    Text="&nbsp;จำพารามิเตอร์ที่เลือกไว้ในเครื่องของคุณ" />
                </div>
                <div class="col-6">
                    <asp:Button ID="cmdPreview" Class="btn btn-success float-right" runat="server" OnClientClick="return chkPreview();" Text="Preview"/>
                </div>
            </div>
        </div>
        </div>
    </div>
</form>
</asp:Content>

