Imports System.Data
Imports System.Threading
Partial Class rptTaxSale
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim fDate As DateTime
    Dim No As New Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("StringQuery") IsNot Nothing Then
            ShowGrid()
            ShowHeader()
        Else
            Response.Redirect("~/pmtTaxSummary.aspx")
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
            Log.LogReport("22040", "รายงานภาษีขาย", Session("Parameter"), vIPAddress, Session("email"), Session("rdate"), fDate)
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
        LblHeader.Text = "ข้อมูลที่ Server ณ. วันที่ " + vDate + " เวลา " + Format(Now, "HH:mm")

        Try
            Dim vHeader() As String
            If Session("Header") IsNot Nothing Then
                vHeader = Split(Session("Header").ToString, "|")
                LblParam1.Text = vHeader(0).ToString
                LblParam2.Text = vHeader(1).ToString
                LblParam3.Text = vHeader(2).ToString
                LblParam4.Text = vHeader(3).ToString
                'LblParam4.Text = vHeader(4).ToString
                'LblParam5.Text = vHeader(5).ToString

            End If
        Catch ex As Exception

        Finally
            If LblParam1.Text.Trim.Length = 0 Then LblParam1.Visible = False
            If LblParam2.Text.Trim.Length = 0 Then LblParam2.Visible = False
            If LblParam3.Text.Trim.Length = 0 Then LblParam3.Visible = False
            If LblParam4.Text.Trim.Length = 0 Then LblParam4.Visible = False
            'If LblParam5.Text.Trim.Length = 0 Then LblParam5.Visible = False
        End Try
    End Sub

    Private sumAmountWithTax As Decimal = 0.0
    Private sumTax As Decimal = 0.0
    Private sumTotalAmount As Decimal = 0.0


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Dim runid As Label = e.Row.FindControl("No")

            'Dim a As Integer = e.Row.RowIndex + 1
            ''Dim DT As New DataTable
            ''Dim strSql As String = Session("StringQuery")

            ''If No <= DT.Rows.Count - 1 Then
            ''No += 1
            ''runid.Text = No
            ''End If
            'runid.Text = a

            sumAmountWithTax += CDec(DataBinder.Eval(e.Row.DataItem, "AmountWithTax"))
            sumTax += CDec(DataBinder.Eval(e.Row.DataItem, "Tax"))
            sumTotalAmount += CDec(DataBinder.Eval(e.Row.DataItem, "TotalAmount"))


            Dim AmountWithTax As Double = e.Row.Cells(7).Text
            AmountWithTax = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(AmountWithTax, "#,##0.00")

            Dim Tax As Double = e.Row.Cells(8).Text
            Tax = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(Tax, "#,##0.00")

            Dim TotalAmount As Double = e.Row.Cells(9).Text
            TotalAmount = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(TotalAmount, "#,##0.00")



        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "รวมทั้งสิ้น"
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right

            e.Row.Cells(7).Text = sumAmountWithTax.ToString
            e.Row.Cells(8).Text = sumTax.ToString
            e.Row.Cells(9).Text = sumTotalAmount.ToString


            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right



            Dim AmountWithTax As Double = e.Row.Cells(7).Text
            AmountWithTax = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(AmountWithTax, "#,##0.00")

            Dim Tax As Double = e.Row.Cells(8).Text
            Tax = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(Tax, "#,##0.00")

            Dim TotalAmount As Double = e.Row.Cells(9).Text
            TotalAmount = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(TotalAmount, "#,##0.00")


        End If
    End Sub
End Class
