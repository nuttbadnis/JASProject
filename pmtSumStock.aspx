<%@ Page Language="VB" maintainScrollPositionOnPostBack="true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="pmtSumStock.aspx.vb" Inherits="pmtSumStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">
<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
                <p class="card-title card-header-th mb-1">
                    [10070] รายงานสรุปยอดสินค้าคงคลัง 
                </p>
                <hr>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">บริษัท</label>
                    <div class="col-lg-5 col-sm-12">
                        <asp:DropDownList ID="ddlCompany" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <%-- <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">ประเภทเอกสาร</label>
                    <div class="col-lg-10 col-sm-12 form-inline">
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="rCompany" runat="server" Text="เขต" Checked="True" GroupName="GroupBy" AutoPostBack="True" />
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="rProvince" runat="server" Text="จังหวัด" GroupName="GroupBy" AutoPostBack="True" />
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="rShopCode" runat="server" Text="สำนักงาน" GroupName="GroupBy" AutoPostBack="True" />
                        </div>
                    </div>
                </div> --%>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">สรุปยอดตาม</label>
                    <div class="col-lg-10 col-sm-12 form-inline">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" cssclass="form-check"
                            RepeatDirection="Horizontal" AutoPostBack="True">
                            <asp:ListItem class="form-check form-check-input col-sm-12" Selected="True">&nbsp;เขต&nbsp;</asp:ListItem>
                            <asp:ListItem class="form-check form-check-input col-sm-12">&nbsp;จังหวัด&nbsp;</asp:ListItem>
                            <asp:ListItem class="form-check form-check-input col-sm-12">&nbsp;สำนักงาน&nbsp;</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">เขต</label>
                    <div class="col-lg-4 col-sm-12">
                        <asp:DropDownList ID="ddlSArea" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <label class="col-lg-1 col-sm-12 col-form-label">จังหวัด</label>
                    <div class="col-lg-5 col-sm-12">
                        <asp:DropDownList ID="ddlSProvince" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True" Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">สำนักงาน</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="ddlSBranch" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="ddlEBranch" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">กลุ่มหลักสินค้า</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="ddlSMain" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="ddlEMain" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">กลุ่มย่อยสินค้า</label>
                    <div class="col-lg-10 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="ddlSSub" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-10 offset-lg-2 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="ddlESub" runat="server" cssclass="form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">รหัสสินค้า</label>
                    <div class="col-lg-10 col-sm-12">
                        <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="ddlSProduct" runat="server" cssclass="select2 form-control" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-10 offset-lg-2 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="ddlEProduct" runat="server" cssclass="select2 form-control" AutoPostBack="True" Enabled="False">
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
                        <asp:Button ID="cmdExport" runat="server" Text="Export To Excel" OnClientClick="return chkPreview();" Visible="False"/>
                        <asp:Button ID="cmdPreview" Class="btn btn-success float-right" runat="server" Text="Preview"/>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </div>
</form>
<script>
var $select2 = $('.select2').select2({
    containerCssClass: "wrap",
    theme: "bootstrap",
    selectOnClose: true,
    width: '100%',
    minimumResultsForSearch: Infinity
})
</script>
</asp:Content>