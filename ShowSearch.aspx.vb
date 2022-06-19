Imports System.Data
Imports System.Threading
Partial Class ShowSearch
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim company As String = Request.QueryString("company")
        Dim SCatagory As String = Request.QueryString("SCatagory")
        Dim ECatagory As String = Request.QueryString("ECatagory")
        Dim SGroup As String = Request.QueryString("SGroup")
        Dim EGroup As String = Request.QueryString("EGroup")
        Dim SsubGroup As String = Request.QueryString("SsubGroup")
        Dim EsubGroup As String = Request.QueryString("EsubGroup")
        Dim SProduct As String = Request.QueryString("SProduct")
        Dim EProduct As String = Request.QueryString("EProduct")

        ShowHeader()
        Dim strSql As String
        Dim DT As New DataTable
        strSql = " SELECT b.F11 'ITEMCODE',d.F05 'ITEMNAME',b.F17 'COST',"
        strSql += " CONVERT(VARCHAR(10),a.F12,103) as  'STARTDATE',CONVERT(VARCHAR(10),a.F14,103) as 'ENDDATE',"
        strSql += " a.F05 'DOC'"
        strSql += " FROM R11160 a left join R11161 b on a.F05=b.F05 inner join M00210 d on b.F11=d.F03"
        strSql += " WHERE  a.F24='1' "
        strSql += " and a.F01='" + company + "'"
        strSql += " and d.f07>='" + SCatagory + "' and d.f07<='" + ECatagory + "'"
        strSql += " and d.f08>='" + SGroup + "' and d.f08<='" + EGroup + "' "
        strSql += " and d.f09>='" + SsubGroup + "' and d.f09<='" + EsubGroup + "'"
        strSql += " and d.f03>='" + SProduct + "' and d.f03<='" + EProduct + "'"
        strSql += " GROUP BY a.F05,b.F11,d.F05,b.F17,a.F12,a.F14"
        strSql += " ORDER BY b.F11,d.F05,a.F05"
        DT = C.GetDataTableRepweb(strSql)
        If DT.Rows.Count = 0 Then
            LblNo1.Visible = True
        End If
        GridView1.DataSource = DT
        GridView1.DataBind()
    End Sub
    Private Sub ShowHeader()
        Dim vDate As String
        vDate = Format(Date.Now, "dd/MM/yyyy")
        LblHeader.Text = "ข้อมูลที่ Server ณ. วันที่ " + vDate + " เวลา " + Format(Now, "HH:mm")

        Try
            Dim vHeader() As String
            If Session("Header") IsNot Nothing Then
                vHeader = Split(Session("Header").ToString, "|")
                report_param.Text = vHeader(2).ToString
                shop_param.Text = vHeader(3).ToString
                product_param.Text = vHeader(4).ToString
                date_param.Text = vHeader(1).ToString
            End If
        Catch ex As Exception

        Finally
            ' If LblParam1.Text.Trim.Length = 0 Then LblParam1.Visible = False
            ' If LblParam2.Text.Trim.Length = 0 Then LblParam2.Visible = False
            ' If LblParam3.Text.Trim.Length = 0 Then LblParam3.Visible = False
        End Try
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim shop As HyperLink = e.Row.FindControl("Shop")
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
