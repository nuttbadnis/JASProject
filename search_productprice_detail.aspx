<%@ Page Language="VB" maintainScrollPositionOnPostBack = "true" MasterPageFile="~/master_request.Master" AutoEventWireup="true" CodeFile="search_productprice_detail.aspx.vb" Inherits="SpacialPayin_search_productprice" %>

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
                            <a class="nav-link" id="overview-tab" href="search_productprice.aspx">ต้นหาจากประเภทสินค้า</a>
                            </li>
                            <li class="nav-item">
                            <a class="nav-link active" id="sales-tab" href="search_productprice_detail.aspx">ค้นหาจากรหัสสินค้า</a>
                            </li>
                        </ul>
                        <div class="tab-content py-0 px-0">
                            <div class="tab-pane fade">
                            </div>
                            <div class="tab-pane fade show active">
                                <div class="d-flex flex-wrap justify-content-xl-between">
                                    <div class="col-md-12 grid-margin mt-3">
                                        <div class="form-group">
                                            <label>ค้นหาจากรหัสสินค้า</label>
                                            <asp:TextBox ID="txtSearch" runat="server" Class="form-control"></asp:TextBox>
                                        </div>
                                        <asp:Button ID="cmdSearch" runat="server" Class="btn btn-success float-right" Text="ค้นหา" />
                                    </div>
                                    <div class="col-md-12 grid-margin mt-3">
                                        <asp:GridView ID="GridView1" runat="server" class="mt-3" AlternatingRowStyle-BackColor="#DAF8FC"
                                            AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None"
                                            BorderWidth="1px" CellPadding="4" Font-Names="Tahoma" Font-Size="9pt" Visible="False"
                                            Width="100%">
                                            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                            <Columns>
                                                <asp:BoundField DataField="ITEMCODE" HeaderText="รหัสสินค้า" />
                                                <asp:BoundField DataField="ITEMNAME" HeaderText="ชื่อสินค้า">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="COST" DataFormatString="{0:#,#0.00}" HeaderText="ราคาขาย">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="STARTDATE" HeaderText="วันที่เริ่มใช้" />
                                                <asp:BoundField DataField="ENDDATE" HeaderText="วันที่สิ้นสุด" />
                                                <asp:BoundField DataField="DOC" HeaderText="เลขที่ใบปรับราคา" />
                                                <asp:TemplateField HeaderText="สำนักงานที่มีผล">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="Shop" runat="server"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle BackColor="White" ForeColor="#330099" />
                                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="DarkCyan" CssClass="HeaderStyle" Font-Bold="True" ForeColor="#FFFFCC" />
                                            <AlternatingRowStyle BackColor="#DAF8FC" />
                                        </asp:GridView>
                                        <asp:Label ID="LblNo1" runat="server" class="alert alert-danger d-flex align-items-center"
                                            Text="** ไม่พบข้อมูลที่ค้นหา **" Visible="False"></asp:Label>
                                    </div>
                                </div>
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

