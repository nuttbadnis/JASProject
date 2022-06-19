Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Web.UI
Imports System.Threading
Imports System.Net.IPAddress

Partial Class SpacialPayin_search_productprice
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        
        If Me.txtSearch.Text.Trim <> "" Then
            RefreshGrid()
        Else
            RefreshGrid(False)
        End If
    End Sub
    Private Sub RefreshGrid(Optional ByVal WhereCause As Boolean = True, Optional ByVal vID As Boolean = False)
        Dim strSql As String
        Dim DT As New DataTable
  
        strSql = " SELECT b.F11 'ITEMCODE',d.F05 'ITEMNAME',b.F17 'COST',"
        strSql += " CONVERT(VARCHAR(10),a.F12,103) as  'STARTDATE',CONVERT(VARCHAR(10),a.F14,103) as 'ENDDATE',"
        strSql += " a.F05 'DOC'"

        If WhereCause = True Then
            If vID = False Then
                strSql += " FROM R11160 a left join R11161 b on a.F05=b.F05 inner join M00210 d on b.F11=d.F03"
                strSql += " WHERE  a.F24='1' "
                strSql += " and d.f03 like '%" + txtSearch.Text + "%'"
            Else
                strSql += " FROM R11160 a left join R11161 b on a.F05=b.F05 inner join M00210 d on b.F11=d.F03"
                strSql += " WHERE  a.F24='1' "
                strSql += " and d.f03 like '%" + txtSearch.Text + "%'"
            End If
        Else
            strSql += " FROM R11160 a left join R11161 b on a.F05=b.F05 inner join M00210 d on b.F11=d.F03"
            strSql += " WHERE  a.F24='1' "
        End If
        strSql += " GROUP BY a.F05,b.F11,d.F05,b.F17,a.F12,a.F14"
        strSql += " ORDER BY b.F11,d.F05,a.F05"
        DT = C.GetDataTableRepweb(strSql)
        GridView1.DataSource = DT
        GridView1.DataBind()
        If DT.Rows.Count = 0 Then
            LblNo1.Visible = True
        Else
            GridView1.Visible = True
            LblNo1.Visible = False
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim shop As HyperLink = e.Row.FindControl("Shop")
            shop.Target = "_blank"
            Dim strSql As String
            Dim DT As New DataTable

            strSql = " SELECT F05,F07 FROM R11162 WHERE F05='" + e.Row.Cells(5).Text + "' GROUP BY F05,F07"
            DT = C.GetDataTableRepweb(strSql)
            If DT.Rows.Count <> 0 Then

                If DT.Rows(0).Item("F07") = "" Then
                    shop.Text = "ทุกสำนักงาน"
                Else
                    shop.Text = "บางสำนักงาน"
                    shop.Attributes("href") = "showshopdetail.aspx?doc=" + e.Row.Cells(5).Text
                End If
            End If
        End If
    End Sub
End Class
