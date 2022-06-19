<%@ Page Language="VB" AutoEventWireup="false" CodeFile="in_out_items_rpt.aspx.vb" Inherits="in_out_items_rpt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

<%--    <link rel="stylesheet" href="css/bootstrap%204/css/bootstrap.min.css" />
    
    <script src="/css/bootstrap%204/js/jquery-3.1.1.slim.min.js"></script>
    <script src="/css/bootstrap%204/js/bootstrap.min.js"></script>--%>

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/js/bootstrap.bundle.min.js"></script>
    
    <title>in_out_items_rpt</title>
</head>
    <style>
    @import url('https://fonts.googleapis.com/css?family=Kanit:400,600');
    @import url('https://fonts.googleapis.com/css?family=Mitr:400,600');
    @font-face{
      font-family: 'TH Sarabun New';
      src: url('Font/THSarabunNew.ttf');
      font-weight: normal;
      font-style: normal;
    }

    body{
        font-size:14px;
        font-family: 'kanit', sans-serif;
    }
    .badge{
        font-size:12px;
        font-weight:lighter;
    }
    .font-mitr{
		font-family:'mitr';
	}
</style>
<body>
    <form id="form1" runat="server">
    <div class="container mt-2">
        <div class="text-center font-mitr">
            <h5><label>[10110] รายงานการขายและเบิกสินค้าค้างจ่าย</label></h5>
        </div>
        <div class="card border-warning pl-3 pr-3" style="border-width: 3px;">
            <div class="card-header text-dark bg-white border-warning pl-0 pr-0" style="border-width: 2px;">
                รายละเอียดข้อมูลเอกสาร
            </div>
            <div class="card-body p-2">
                <asp:Label ID="lblHeader" runat="server" Text="Label"></asp:Label> 
            </div>
        </div>
        <BR>
        <div>   
            <div>
                <asp:Table ID="tbe_show" class="table table-sm" runat="server" GridLines="Both" Font-Size="12px">
                </asp:Table>
            </div>
        </div>
        <BR>
        <div>
        
            <asp:GridView ID="gvw_showDate_1" runat="server" >
            </asp:GridView>
        
        </div>
        <BR>
        <!-- ERROR MESSAGE -->
        <div id="ERROR_DIV" runat="server" class="row">
                
        </div>
    </div> <!-- container -->
    </form>
</body>
</html>
