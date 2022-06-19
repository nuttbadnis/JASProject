<%@ Page Language="VB" maintainScrollPositionOnPostBack = "true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="pmtTransferDetail.aspx.vb" Inherits="pmtTransferDetail" %>


<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">

<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
                <p class="card-title card-header-th mb-1">
                    [10096] รายงานการโอนสินค้าระหว่างสำนักงานแบบระบุรหัสสินค้า
                </p>
                <hr>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">วันที่โอนออกอุปกรณ์</label>
                    <div class="col-lg-4 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <input ID="SDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                            <asp:TextBox ID="SDate" visible="true" Name="SDate" runat="server" cssclass="form-control" />
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
                    <label class="col-lg-2 col-sm-12 col-form-label">เขตธุรกิจ</label>
                    <div class="col-lg-4 col-sm-12">
                        <asp:DropDownList ID="cboArea" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <label class="col-lg-2 col-sm-12 col-form-label">จังหวัด</label>
                    <div class="col-lg-4 col-sm-12">
                        <asp:DropDownList ID="cboProvince" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row d-none">
                    <div class="col-12">
                        <asp:CheckBox ID="cbxLg" class="col-form-label col-form-label-sm" runat="server" Checked="True" 
                        Text="&nbsp;ดูเฉพาะคลัง logistic" />
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">สาขาที่โอนออกสินค้า</label>
                    <div class="col-lg-10 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="cboSBranch" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-10 offset-lg-2 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="cboEBranch" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">สาขาที่รับออกสินค้า</label>
                    <div class="col-lg-10 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="cboSBranch2" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-10 offset-lg-2 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="cboEBranch2" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">กลุ่มหลัก</label>
                    <div class="col-lg-10 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="SGroup" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <label class="col-lg-2 col-sm-12 col-form-label">กลุ่มย่อย</label>
                    <div class="col-lg-10 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="SsubGroup" runat="server" cssclass="form-control" AutoPostBack="True">
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
                            <asp:DropDownList ID="SItem" runat="server" cssclass="form-control select2">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-10 offset-lg-2 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="EItem" runat="server" cssclass="form-control select2">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">สถานะใบโอนออก</label>
                    <div class="col-lg-10 col-sm-12 form-inline">
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="rdbOpen" runat="server" Text="รอรับโอนอุปกรณ์" Checked="True" GroupName="rdbStatus" AutoPostBack="True" />
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="rdbClose" runat="server" Text="รับโอนอุปกรณ์แล้ว" GroupName="rdbStatus" AutoPostBack="True" />
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="rdbAll" runat="server" Text="ดูสถานะทั้งหมด" GroupName="rdbStatus" AutoPostBack="True" />
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-12">
                        <asp:Button ID="cmdPreview" Class="btn btn-success float-right" runat="server" Text="Preview"/>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </div>
    <div class="alert alert-danger alert-dismissible fade show lh-md font-mitr">
        <span class="badge badge-danger fs-1">หมายเหตุ</span>
        <u>ในส่วนกลุ่มหลัก, กลุ่มย่อย, รหัสสินค้า หากไม่เลือกกลุ่มหรืออุปกรณ์ โปรแกรมจะดึงข้อมูลอุปกรณ์ทั้งหมด</u>       
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

