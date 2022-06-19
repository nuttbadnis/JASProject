Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Threading
Imports System.Net.IPAddress
Partial Class ShowReportPayin
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim show As String
    Dim fDate As DateTime
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("StringQuery") IsNot Nothing Then
            show = Request.QueryString("show")


            ShowGrid()
            ShowHeader()
        Else
            Response.Redirect("~/ReportPayin.aspx")
        End If
    End Sub
    Private Sub ShowGrid()
        Try
            Dim DT As New DataTable
            fDate = "2001-01-01 00:00:00.000"
            Dim strSql As String = Session("StringQuery")
            DT = C.GetDataTableRepweb(strSql, True)
            If show = 1 Then
                GridView1.Columns(4).Visible = True
                GridView1.Columns(5).Visible = True
                GridView1.Columns(11).Visible = True
            End If
            Me.GridView1.DataSource = DT
            GridView1.DataBind()
            If DT.Rows.Count = 0 Then
                LblNo1.Visible = True
                Label1.Visible = False
            End If


            fDate = Now().ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))

        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        Finally
            Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            'Log.LogReport("21030", "รายงานใบนำฝากเงิน", Session("Parameter"), vIPAddress, Session("email"), Session("rdate"), fDate)
        End Try
    End Sub
    Private Sub ShowHeader()
        Dim vDate As String

        vDate = Format(Date.Now, "dd/MM/yyyy")
        If Mid(vDate, 7, 4) > 2500 Then
            vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) - 543).ToString
        End If
        LblHeader.Text = "ข้อมูลที่ Server ณ. วันที่ " + vDate + " เวลา " + Format(Now, "HH:mm")

        Try
            Dim vHeader() As String
            If Session("Header") IsNot Nothing Then
                vHeader = Split(Session("Header").ToString, "|")
                LblParam1.Text = vHeader(0).ToString
                LblParam2.Text = vHeader(1).ToString
                LblParam3.Text = vHeader(3).ToString
                LblParam4.Text = vHeader(2).ToString
                LblParam5.Text = vHeader(4).ToString
            End If
        Catch ex As Exception

        Finally
            If LblParam1.Text.Trim.Length = 0 Then LblParam1.Visible = False
            If LblParam2.Text.Trim.Length = 0 Then LblParam2.Visible = False
            If LblParam3.Text.Trim.Length = 0 Then LblParam3.Visible = False
            If LblParam4.Text.Trim.Length = 0 Then LblParam4.Visible = False
            If LblParam5.Text.Trim.Length = 0 Then LblParam5.Visible = False
        End Try
    End Sub
    Private sumCS As Decimal = 0.0
    Private sumCharge As Decimal = 0.0
    Private sumCQ As Decimal = 0.0
    Private sumAmount As Decimal = 0.0
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            sumAmount += CDec(DataBinder.Eval(e.Row.DataItem, "Sum"))
            sumCS += CDec(DataBinder.Eval(e.Row.DataItem, "CS"))
            sumCharge += CDec(DataBinder.Eval(e.Row.DataItem, "Charge"))
            sumCQ += CDec(DataBinder.Eval(e.Row.DataItem, "CQ"))

            Dim CS As Double = e.Row.Cells(7).Text
            CS = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(CS, "#,##0.00")

            Dim Charge As Double = e.Row.Cells(8).Text
            Charge = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(Charge, "#,##0.00")

            Dim CQ As Double = e.Row.Cells(9).Text
            CQ = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(CQ, "#,##0.00")

            Dim Sum As Double = e.Row.Cells(10).Text
            Sum = String.Format(e.Row.Cells(10).Text, "{0:00,00.00}")
            e.Row.Cells(10).Text = Format(Sum, "#,##0.00")


            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(12).HorizontalAlign = HorizontalAlign.Right

            'e.Row.Cells(6).Width = 150
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "รวม::"

            e.Row.Cells(7).Text = sumCS.ToString
            e.Row.Cells(8).Text = sumCharge.ToString
            e.Row.Cells(9).Text = sumCQ.ToString
            e.Row.Cells(10).Text = sumAmount.ToString


            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right

            Dim Sum_CS As Double = e.Row.Cells(7).Text
            Sum_CS = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(Sum_CS, "#,##0.00")

            Dim Sum_Charge As Double = e.Row.Cells(8).Text
            Sum_Charge = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(Sum_Charge, "#,##0.00")

            Dim Sum_CQ As Double = e.Row.Cells(9).Text
            Sum_CQ = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(Sum_CQ, "#,##0.00")

            Dim Sum_Amount As Double = e.Row.Cells(10).Text
            Sum_Amount = String.Format(e.Row.Cells(10).Text, "{0:00,00.00}")
            e.Row.Cells(10).Text = Format(Sum_Amount, "#,##0.00")

           

        End If
    End Sub
End Class
