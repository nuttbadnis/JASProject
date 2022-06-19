Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Partial Class rptTransferDetailSN
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim doc As String = Request.QueryString("doc")
        Dim item As String = Request.QueryString("item")
        Dim seq As String = Request.QueryString("seq")
        lblDoc.Text = "เลขที่ใบโอนออก: " & doc
        lblItem.Text = "รหัสสินค้า: " & item
        Dim strSql As String
        Dim DT As New DataTable
        strSql = "select f07 'seq', f08 'SN' from r01105 where f05='" & doc & "' and f06 = '" & seq & "' order by f07 "
        DT = C.GetDataTable(strSql)
        If DT.Rows.Count > 0 Then
            lblNoSN.Visible = False
            GridView1.DataSource = DT
            GridView1.DataBind()
        Else
            lblNoSN.Visible = True
        End If

    End Sub
End Class
