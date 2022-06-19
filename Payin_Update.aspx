<%@ Page Language="VB" maintainScrollPositionOnPostBack="true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="Payin_Update.aspx.vb" Inherits="Payin_Update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">

<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
                <p class="card-title card-header-th mb-1">
                    [22030] รายงานค้างนำฝาก
                </p>
                <hr>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">บริษัท:</label>
                    <div class="col-lg-5 col-sm-12">
                        <asp:DropDownList ID="cboCompany" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">เขตการขาย</label>
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
                            <asp:DropDownList ID="cboSProvince" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="cboEProvince" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">สาขา</label>
                    <div class="col-lg-10 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="cboSBranch" runat="server" cssclass="form-control" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <label class="col-lg-2 col-sm-12 col-form-label"></label>
                    <div class="col-lg-10 col-sm-12">
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
                    <label class="col-lg-2 col-sm-12 col-form-label">รูปแบบ</label>
                    <div class="col-lg-4 col-sm-12 form-inline">
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="OptSum" runat="server" Text="แบบสรุป" Checked="True" GroupName="Sum" />
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="OptChk" runat="server" Text="แบบตรวจสอบ" GroupName="Sum"/>
                        </div>
                    </div>
                    <label class="col-lg-2 col-sm-12 col-form-label">ประเภทเอกสาร</label>
                    <div class="col-lg-4 col-sm-12 form-inline">
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="Optcq" runat="server" Text="Cheque" Checked="True" GroupName="Type"/>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="Optcs" runat="server" Text="Cash" GroupName="Type"/>
                        </div>
                    </div>
                </div>    
                <div class="row mb-3">
                    <div class="col-6">
                        <asp:CheckBox ID="chkParameter" class="col-form-label col-form-label-sm" runat="server" Checked="True" 
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
    <div class="alert alert-danger alert-dismissible fade show lh-md font-mitr">
        <span class="badge badge-danger fs-1">หมายเหตุ</span>
        <u>ข้อมูลที่แสดงในรายงานนี้จะ Update ทุกเที่ยงคืน</u>       
    </div>  
</form>
</asp:Content>
