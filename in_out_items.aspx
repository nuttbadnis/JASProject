<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/master_request.Master" CodeFile="in_out_items.aspx.vb" Inherits="in_out_items" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FormContent" runat="server">
<script type="text/javascript">
var windowObjectReference = null; // global variable

function openRequestedPopup(url, windowName) {
  if(windowObjectReference == null || windowObjectReference.closed) {
    windowObjectReference = window.open(url, windowName, "popup");
  } else {
    windowObjectReference.focus();
  };
}
</script>
<form id="form1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
            <div class="card font-mitr">
                <div class="card-body">
                    <p class="card-title card-header-th mb-1">[10041] รายงาน in/out อุปกรณ์</p>
                    <hr>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">วันที่</label>
                        <div class="col-lg-4 col-sm-12">
                            <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                            </div>
                            <input ID="tbxSDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                            <asp:TextBox ID="tbxSDate" visible="true" Name="tbxSDate" runat="server" cssclass="form-control d-none" />
                            </div>
                        </div>
                        <div class="col-lg-4 col-sm-12">
                            <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                            <div class="input-group-text bg-primary text-white">ถึง</div>
                            </div>
                            <input ID="tbxEDate" type="date" class="form-control" onchange="getcalendar(this);"/>
                            <asp:TextBox ID="tbxEDate" visible="true" Name="tbxEDate" runat="server" cssclass="form-control d-none" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">บริษัท</label>
                        <div class="col-lg-5 col-sm-12">
                            <asp:DropDownList ID="COMPANY_1" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                        <label class="col-lg-1 col-sm-12 col-form-label">เขต</label>
                        <div class="col-lg-4 col-sm-12">
                            <asp:DropDownList ID="RO_1" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row d-none">
                        <label class="col-lg-2 col-sm-12 col-form-label">คลัสเตอร์</label>
                        <div class="col-lg-5 col-sm-12">
                            <asp:DropDownList ID="CLUSTER_1" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">จังหวัด</label>
                        <div class="col-lg-4 col-sm-12">
                            <asp:DropDownList ID="PROVINCE_1" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">สำนักงาน</label>
                        <div class="col-lg-10 col-sm-12">
                            <div class="input-group input-group-sm mb-2">
                                <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">จาก</div>
                                </div>
                                <asp:DropDownList ID="SHOP_1" runat="server" cssclass="form-control" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-10 offset-lg-2 col-sm-12">
                            <div class="input-group input-group-sm mb-2">
                                <div class="input-group-prepend">
                                <div class="input-group-text bg-primary text-white">ถึง</div>
                                </div>
                                <asp:DropDownList ID="SHOP_2" runat="server" cssclass="form-control" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <asp:CheckBox ID="SHOP_ACTIVE_1" class="col-form-label col-form-label-sm" runat="server" Checked="True" 
                            Text="&nbsp;แสดงเฉพาะสำนักงานที่ยังไม่ได้ปิด" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">กลุ่มหลักอุปกรณ์</label>
                        <div class="col-lg-6 col-sm-12">
                            <asp:DropDownList ID="MAIN_GROUP_1" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">กลุ่มย่อยอุปกรณ์</label>
                        <div class="col-lg-6 col-sm-12">
                             <asp:DropDownList ID="SUB_GROUP_1" runat="server" cssclass="form-control form-control-sm" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-lg-2 col-sm-12 col-form-label">รหัสอุปกรณ์</label>
                        <div class="col-lg-10 col-sm-12">
                            <div class="input-group input-group-sm mb-2">
                                <div class="input-group-prepend">
                                    <div class="input-group-text bg-primary text-white">จาก</div>
                                </div>
                                <asp:DropDownList ID="ITEM_1" runat="server" cssclass="form-control select2" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-10 offset-lg-2 col-sm-12">
                            <div class="input-group input-group-sm mb-2">
                                <div class="input-group-prepend">
                                    <div class="input-group-text bg-primary text-white">ถึง</div>
                                </div>
                                <asp:DropDownList ID="ITEM_2" runat="server" cssclass="form-control select2" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-12">
                        <asp:LinkButton ID="SEARCH_1" class="btn btn-success float-right" runat="server"
                         onclick="SEARCH_1_Click" Text="Preview"/></asp:LinkButton>
                            <%-- <asp:Button ID="SEARCH_1" class="btn btn-success float-right" runat="server"
                             Text="Preview" onclick="SEARCH_1_Click" /> --%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- </div>      --%>
        <!-- ERROR MESSAGE -->
        <div id="ERROR_DIV" runat="server" class="row"></div>
    </div>
    <a  ID="preview_test" runat="server" onserverclick="SEARCH_1_Click" class="btn btn-success" >TEST</a>
    <p><a id="aa"
 href="http://www.spreadfirefox.com/"
 target="PromoteFirefoxWindow"
 onclick="openRequestedPopup(this.href, this.target); return false;"
 title="This link will create a new window or will re-use an already opened one"
>Promote Firefox adoption</a></p>
</form>
<script>
<%-- function test(){
    
    var url = "./in_out_items_rpt.aspx?p_sDate=2021-12-01&p_eDate=2021-12-01&p_company=03&p_ro=30&p_cluster=&p_province=VTE&p_shop_1=ALL&p_shop_2=ALL&p_mGroup=ALL&p_sGroup=ALL&p_item_1=ALL&p_item_2=ALL&p_shop_active=TRUE"
    window.open(url,"_blank");
} --%>
$('#preview').click(function () {
    //var redirectWindow = 
    //var url = "./in_out_items_rpt.aspx?p_sDate=2021-12-01&p_eDate=2021-12-01&p_company=03&p_ro=30&p_cluster=&p_province=VTE&p_shop_1=ALL&p_shop_2=ALL&p_mGroup=ALL&p_sGroup=ALL&p_item_1=ALL&p_item_2=ALL&p_shop_active=TRUE"

    //window.open(''+url+'', '_blank', 'toolbar=yes, location=yes, status=yes, menubar=yes, scrollbars=yes');
    window.open('http://google.com','Page','height=500,width=500');
    redirectWindow.location;
});
var $select2 = $('.select2').select2({
    containerCssClass: "wrap",
    theme: "bootstrap",
    selectOnClose: true,
    width: '100%',
    minimumResultsForSearch: Infinity
})
</script>
</asp:Content>



