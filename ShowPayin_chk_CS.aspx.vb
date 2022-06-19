Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Threading
Imports System.Net.IPAddress
Partial Class ShowPayin_chk_CS
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim fDate As DateTime
    Dim strConn As String = WebConfigurationManager.ConnectionStrings("RMSDATConnectionString").ConnectionString.ToString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("StringQuery") IsNot Nothing Then
            ShowGrid()
            'SetColumnWidth()
            ShowHeader()
        Else
            Response.Redirect("~/Payin_Update.aspx")
        End If
    End Sub
    Private Sub ShowGrid()
        Try
            Dim DT As New DataTable
            fDate = "2001-01-01 00:00:00.000"
            Dim strSql As String = Session("StringQuery")
            DT = C.GetDataTable(strSql, True)
            Me.GridView1.DataSource = DT
            GridView1.DataBind()
            If DT.Rows.Count = 0 Then LblNo1.Visible = True
            fDate = Now().ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        Finally
            Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            Log.LogReport("22030", "รายงานค้างนำฝาก", Session("Parameter"), vIPAddress, Session("email"), Session("rdate"), fDate)
        End Try
    End Sub
    Private Sub ShowHeader()
        Dim vDate As String
        Dim currThead As Globalization.CultureInfo
        currThead = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US")
        vDate = Format(Date.Now, "dd/MM/yyyy")
        If Mid(vDate, 7, 4) > 2500 Then
            vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) - 543).ToString
        End If
        Thread.CurrentThread.CurrentCulture = currThead
        LblHeader.Text = " วันที่เรียกดูรายงาน " + vDate + " เวลา " + Format(Now, "HH:mm") + "      " + "  ข้อมูลอัพเดตที่ Server ทุกเที่ยงคืน "

        Try
            Dim vHeader() As String
            If Session("Header") IsNot Nothing Then
                vHeader = Split(Session("Header").ToString, "|")
                LblParam1.Text = vHeader(0).ToString
                LblParam2.Text = vHeader(1).ToString
                LblParam3.Text = vHeader(2).ToString
                LblParam4.Text = vHeader(3).ToString
                LblParam5.Text = vHeader(4).ToString
                LblParam5.Text = vHeader(5).ToString
            End If
        Catch ex As Exception

        Finally
            If LblParam1.Text.Trim.Length = 0 Then LblParam1.Visible = False
            If LblParam2.Text.Trim.Length = 0 Then LblParam2.Visible = False
            If LblParam3.Text.Trim.Length = 0 Then LblParam3.Visible = False
            If LblParam4.Text.Trim.Length = 0 Then LblParam4.Visible = False
        End Try
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Receipt As Double = e.Row.Cells(5).Text
            Dim Payin As Double = e.Row.Cells(6).Text
            Dim cs_in_sys As Double = e.Row.Cells(7).Text

            Receipt = String.Format(e.Row.Cells(5).Text, "{0:00,00.00}")
            e.Row.Cells(5).Text = Format(Receipt, "#,##0.00")
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right

            Payin = String.Format(e.Row.Cells(6).Text, "{0:00,00.00}")
            e.Row.Cells(6).Text = Format(Payin, "#,##0.00")
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right


            cs_in_sys = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(cs_in_sys, "#,##0.00")
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
        End If
    End Sub

End Class
