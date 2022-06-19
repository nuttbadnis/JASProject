Imports System.Data

Partial Class showshopdetail
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Doc As String = Request.QueryString("doc")
        Dim strSql As String
        Dim DT As New DataTable

        strSql = "SELECT F07 FROM R11162 WHERE F05='" + Doc + "'  order by f07"
        DT = C.GetDataTableRepweb(strSql)
        If DT.Rows.Count = 0 Then
            LblNo1.Visible = True
        End If
        GridView1.DataSource = DT
        GridView1.DataBind()
    End Sub
End Class
