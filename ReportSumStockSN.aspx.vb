Imports System.Data
Partial Class ReportSumStockSN
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim shop As String = Request.QueryString("shop")
        Dim item As String = Request.QueryString("item")

        lblShop.Text = "รหัสสาขา: " & shop
        lblItem.Text = "รหัสสินค้า: " & item
        Dim strSql As String
        Dim DT As New DataTable
        strSql = "select ROW_NUMBER() OVER(ORDER BY f02,F11,f05,f11,f12) AS 'RowID', F05 'item', F10 'maingroup', F11 'subgroup', F12 'SN'  " + vbCr
        strSql += "from R01220 " + vbCr
        strSql += "where F02 = '" & shop & "' and F05='" & item & "' and f13='I' "
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
