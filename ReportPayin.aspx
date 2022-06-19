<%@ Page Language="VB"  maintainScrollPositionOnPostBack = "true" AutoEventWireup="true"  MasterPageFile="~/master_request.Master" CodeFile="ReportPayin.aspx.vb" Inherits="ReportPayin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">

<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
        <div class="card font-mitr">
            <div class="card-body">
                <p class="card-title card-header-th mb-1">
                    [21030] รายงานใบนำฝากเงิน
                </p>
                <hr>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">รูปแบบการแสดงผล</label>
                    <div class="col-lg-10 col-sm-12 form-inline">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" class="form-check">
                            <asp:ListItem class="form-check form-check-input" Selected="True" Value="0">&nbsp;แบบสรุป&nbsp;</asp:ListItem>
                            <asp:ListItem class="form-check form-check-input" Value="1">&nbsp;แสดงรายละเอียด&nbsp;</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">บริษัท</label>
                    <div class="col-lg-5 col-sm-12">
                        <asp:DropDownList ID="ddlCompany" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">วันที่</label>
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
                    <label class="col-lg-2 col-sm-12 col-form-label">เขต</label>
                    <div class="col-lg-4 col-sm-12">
                        <asp:DropDownList ID="ddlArea" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <label class="col-lg-1 col-sm-12 col-form-label">จังหวัด</label>
                    <div class="col-lg-5 col-sm-12">
                        <asp:DropDownList ID="ddlProvince" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True" Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div> 
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">สาขา</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="ddlSBranch" runat="server" cssclass="form-control" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="ddlEBranch" runat="server" cssclass="form-control" Enabled="False">
                            </asp:DropDownList> 
                        </div>
                    </div>
                </div>           
                <div class="row">
                    <label class="col-lg-2 col-sm-12 col-form-label">เลขที่บัญชี</label>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <asp:DropDownList ID="ddlSAccount" runat="server" cssclass="form-control" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-12">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <asp:DropDownList ID="ddlEAccount" runat="server" cssclass="form-control" Enabled="False">
                            </asp:DropDownList> 
                        </div>
                    </div>
                </div> 
                <div class="row mb-3">
                    <div class="col-6">
                        <asp:CheckBox ID="chkParameter" class="col-form-label col-form-label-sm" runat="server" Checked="True" 
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

