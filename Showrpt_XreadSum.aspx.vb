Imports System.Web
Imports System.Web.UI
Imports System.Data
Imports System.Threading
Imports System.Net.IPAddress

Partial Class Showrpt_XreadSum
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim Group As String
    Dim Com As String
    Dim Area As String
    Dim SDate As String
    Dim EDate As String
    Dim fDate As DateTime

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("StringQuery") IsNot Nothing Then
            Group = Request.QueryString("group")
            Com = Request.QueryString("com")
            Area = Request.QueryString("area")
            SDate = Request.QueryString("sdate")
            EDate = Request.QueryString("edate")
            ShowGrid()
            ShowHeader()
        Else
            Response.Redirect("~/X_readSum.aspx")
        End If
    End Sub
    Private Sub ShowGrid()
        Try
            Dim DT As New DataTable

            fDate = "2001-01-01 00:00:00.000"
            Dim strSql As String = Session("StringQuery")
            
            DT = C.GetDataTableRepweb(strSql, True)
            If Group = "S" Then
                GridView1.Columns(3).Visible = False
                'GridView1.Columns(4).Visible = True
            Else
                GridView1.Columns(3).Visible = True
                'GridView1.Columns(4).Visible = False
            End If
            Me.GridView1.DataSource = DT
            GridView1.DataBind()

            If DT.Rows.Count = 0 Then LblNo1.Visible = True
            fDate = Now().ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
            
        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        Finally
            Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            'Log.LogReport("21015", "รายงาน Summary X-Read", Session("Parameter"), vIPAddress, Session("email"), Session("rdate"), fDate)
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
                report_name.Text = vHeader(1).ToString
                comp_param.Text = vHeader(0).ToString
                ro_param.Text = vHeader(2).ToString
                prov_param.Text = vHeader(3).ToString
                shop_param.Text = vHeader(4).ToString 
                date_param.Text = vHeader(5).ToString
            End If
        Catch ex As Exception

        Finally
            ' If LblParam1.Text.Trim.Length = 0 Then LblParam1.Visible = False
            ' If LblParam2.Text.Trim.Length = 0 Then LblParam2.Visible = False
            ' If LblParam3.Text.Trim.Length = 0 Then LblParam3.Visible = False
            ' If LblParam4.Text.Trim.Length = 0 Then LblParam4.Visible = False
            ' If LblParam5.Text.Trim.Length = 0 Then LblParam5.Visible = False
        End Try
    End Sub

    Private sumDoc As Integer = 0
    Private sumCS As Decimal = 0.0
    Private sumCQ As Decimal = 0.0
    Private sumCR As Decimal = 0.0
    Private sumTR As Decimal = 0.0
    Private sumWH As Decimal = 0.0
    Private sumAI As Decimal = 0.0
    Private sumAD As Decimal = 0.0
    Private sumAO As Decimal = 0.0
    Private sumFR As Decimal = 0.0
    Private sumA_Type As Decimal = 0.0
    Private sumBill As Decimal = 0.0
    Private sumOTC As Decimal = 0.0
    Private sumSrcpv As Decimal = 0.0


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'If Group = "S" Then
            sumDoc += CDec(DataBinder.Eval(e.Row.DataItem, "CDoc"))
            'End If
            sumCS += CDec(DataBinder.Eval(e.Row.DataItem, "CS"))
            sumCQ += CDec(DataBinder.Eval(e.Row.DataItem, "CQ"))
            sumCR += CDec(DataBinder.Eval(e.Row.DataItem, "CR"))
            sumTR += CDec(DataBinder.Eval(e.Row.DataItem, "TR"))
            sumWH += CDec(DataBinder.Eval(e.Row.DataItem, "WH"))
            sumAI += CDec(DataBinder.Eval(e.Row.DataItem, "AI"))
            sumAD += CDec(DataBinder.Eval(e.Row.DataItem, "AD"))
            sumAO += CDec(DataBinder.Eval(e.Row.DataItem, "AO"))
            sumFR += CDec(DataBinder.Eval(e.Row.DataItem, "FR"))
            sumA_Type += CDec(DataBinder.Eval(e.Row.DataItem, "A_Type"))
            sumBill += CDec(DataBinder.Eval(e.Row.DataItem, "BillPayment"))
            sumOTC += CDec(DataBinder.Eval(e.Row.DataItem, "OTC"))
            sumSrcpv += CDec(DataBinder.Eval(e.Row.DataItem, "SRCPV"))

            Dim DCS As Double = e.Row.Cells(4 + 1).Text
            DCS = String.Format(e.Row.Cells(4 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(4 + 1).Text = Format(DCS, "#,##0")

            Dim DCR As Double = e.Row.Cells(5 + 1).Text
            DCR = String.Format(e.Row.Cells(5 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(5 + 1).Text = Format(DCR, "#,##0.00")

            Dim DCQ As Double = e.Row.Cells(6 + 1).Text
            DCQ = String.Format(e.Row.Cells(6 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(6 + 1).Text = Format(DCQ, "#,##0.00")


            Dim DTR As Double = e.Row.Cells(7 + 1).Text
            DTR = String.Format(e.Row.Cells(7 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(7 + 1).Text = Format(DTR, "#,##0.00")

            Dim DWH As Double = e.Row.Cells(8 + 1).Text
            DWH = String.Format(e.Row.Cells(8 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(8 + 1).Text = Format(DWH, "#,##0.00")

            Dim DAO As Double = e.Row.Cells(9 + 1).Text
            DAO = String.Format(e.Row.Cells(9 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(9 + 1).Text = Format(DAO, "#,##0.00")

            Dim DA_Type As Double = e.Row.Cells(10 + 1).Text
            DA_Type = String.Format(e.Row.Cells(10 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(10 + 1).Text = Format(DA_Type, "#,##0.00")

            Dim DAD As Double = e.Row.Cells(11 + 1).Text
            DAD = String.Format(e.Row.Cells(11 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(11 + 1).Text = Format(DAD, "#,##0.00")

            Dim DAI As Double = e.Row.Cells(12 + 1).Text
            DAI = String.Format(e.Row.Cells(12 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(12 + 1).Text = Format(DAI, "#,##0.00")


            Dim DFR As Double = e.Row.Cells(13 + 1).Text
            DFR = String.Format(e.Row.Cells(13 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(13 + 1).Text = Format(DFR, "#,##0.00")

            Dim DBill As Double = e.Row.Cells(14 + 1).Text
            DBill = String.Format(e.Row.Cells(14 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(14 + 1).Text = Format(DBill, "#,##0.00")

            Dim DOTC As Double = e.Row.Cells(15 + 1).Text
            DOTC = String.Format(e.Row.Cells(15 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(15 + 1).Text = Format(DOTC, "#,##0.00")

            Dim DSRCPV As Double = e.Row.Cells(16 + 1).Text
            DSRCPV = String.Format(e.Row.Cells(16 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(16 + 1).Text = Format(DSRCPV, "#,##0.00")

            Dim btn As HyperLink = e.Row.FindControl("CodeShop")
            If Group = "S" Then
                btn.Attributes("href") = "ShowrptXreadLink.aspx?com=" + Com + "&sdate=" + SDate + "&edate=" + EDate + "&area=" + Area + "&branch=" + CType(e.Row.FindControl("CodeShop"), HyperLink).Text

            Else
                Dim conDate As String
                conDate = Mid(e.Row.Cells(3).Text, 7, 4) + "/" + Mid(e.Row.Cells(3).Text, 4, 2) + "/" + Mid(e.Row.Cells(3).Text, 1, 2)
                btn.Attributes("href") = "ShowrptXreadLink.aspx?com=" + Com + "&sdate=" + conDate + "&edate=" + conDate + "&area=" + Area + "&branch=" + CType(e.Row.FindControl("CodeShop"), HyperLink).Text
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(2).Text = "รวมทั้งหมด"

            e.Row.Cells(3 + 1).Text = sumDoc.ToString
            e.Row.Cells(4 + 1).Text = sumCS.ToString
            e.Row.Cells(5 + 1).Text = sumCR.ToString
            e.Row.Cells(6 + 1).Text = sumCQ.ToString
            e.Row.Cells(7 + 1).Text = sumTR.ToString
            e.Row.Cells(8 + 1).Text = sumWH.ToString
            e.Row.Cells(9 + 1).Text = sumAO.ToString
            e.Row.Cells(10 + 1).Text = sumA_Type.ToString
            e.Row.Cells(11 + 1).Text = sumAD.ToString
            e.Row.Cells(12 + 1).Text = sumAI.ToString
            e.Row.Cells(13 + 1).Text = sumFR.ToString
            e.Row.Cells(14 + 1).Text = sumBill.ToString
            e.Row.Cells(15 + 1).Text = sumOTC.ToString
            e.Row.Cells(16 + 1).Text = sumSrcpv.ToString

            e.Row.Cells(3 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(4 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(5 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(6 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(7 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(10 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(11 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(12 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(13 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(14 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(15 + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(16 + 1).HorizontalAlign = HorizontalAlign.Right

            Dim DCDoc As Double = e.Row.Cells(3 + 1).Text
            DCDoc = String.Format(e.Row.Cells(3 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(3 + 1).Text = Format(DCDoc, "#,##0")

            Dim DCS As Double = e.Row.Cells(4 + 1).Text
            DCS = String.Format(e.Row.Cells(4 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(4 + 1).Text = Format(DCS, "#,##0")

            Dim DCR As Double = e.Row.Cells(5 + 1).Text
            DCR = String.Format(e.Row.Cells(5 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(5 + 1).Text = Format(DCR, "#,##0.00")

            Dim DCQ As Double = e.Row.Cells(6 + 1).Text
            DCQ = String.Format(e.Row.Cells(6 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(6 + 1).Text = Format(DCQ, "#,##0.00")


            Dim DTR As Double = e.Row.Cells(7 + 1).Text
            DTR = String.Format(e.Row.Cells(7 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(7 + 1).Text = Format(DTR, "#,##0.00")

            Dim DWH As Double = e.Row.Cells(8 + 1).Text
            DWH = String.Format(e.Row.Cells(8 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(8 + 1).Text = Format(DWH, "#,##0.00")

            Dim DAO As Double = e.Row.Cells(9 + 1).Text
            DAO = String.Format(e.Row.Cells(9 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(9 + 1).Text = Format(DAO, "#,##0.00")

            Dim DA_Type As Double = e.Row.Cells(10 + 1).Text
            DA_Type = String.Format(e.Row.Cells(10 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(10 + 1).Text = Format(DA_Type, "#,##0.00")

            Dim DAD As Double = e.Row.Cells(11 + 1).Text
            DAD = String.Format(e.Row.Cells(11 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(11 + 1).Text = Format(DAD, "#,##0.00")

            Dim DAI As Double = e.Row.Cells(12 + 1).Text
            DAI = String.Format(e.Row.Cells(12 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(12 + 1).Text = Format(DAI, "#,##0.00")


            Dim DFR As Double = e.Row.Cells(13 + 1).Text
            DFR = String.Format(e.Row.Cells(13 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(13 + 1).Text = Format(DFR, "#,##0.00")

            Dim DBill As Double = e.Row.Cells(14 + 1).Text
            DBill = String.Format(e.Row.Cells(14 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(14 + 1).Text = Format(DBill, "#,##0.00")

            Dim DOTC As Double = e.Row.Cells(15 + 1).Text
            DOTC = String.Format(e.Row.Cells(15 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(15 + 1).Text = Format(DOTC, "#,##0.00")

            Dim DSRCPV As Double = e.Row.Cells(16 + 1).Text
            DSRCPV = String.Format(e.Row.Cells(16 + 1).Text, "{0:00,00.00}")
            e.Row.Cells(16 + 1).Text = Format(DSRCPV, "#,##0.00")

        End If
    End Sub
End Class
