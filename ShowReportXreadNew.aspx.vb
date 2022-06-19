Imports System.Data
Imports System.Data.OleDb
Imports System.Web.UI
Imports System.Threading
Imports System.HttpStyleUriParser
Imports System.IO
Imports System.Net.IPAddress
Partial Class ShowReportXreadNew
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim Com As String
    Dim SArea As String
    Dim EArea As String
    Dim SProv As String
    Dim EProv As String
    Dim SBranch As String
    Dim EBranch As String
    Dim SDate As String
    Dim EDate As String
    Dim fDate As DateTime

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("StringQuery") IsNot Nothing Then
            ShowGrid()
            ShowHeader()
        Else
            Response.Redirect("~/X-Read_Edit.aspx")
        End If
        'Com = Request.QueryString("com")
        'SArea = Request.QueryString("sarea")
        'EArea = Request.QueryString("earea")
        'SProv = Request.QueryString("sprov")
        'EProv = Request.QueryString("eprov")
        'SBranch = Request.QueryString("sbranch")
        'EBranch = Request.QueryString("ebranch")
        'SDate = Request.QueryString("sdate")
        'EDate = Request.QueryString("edate")
    End Sub
    Private Sub ShowGrid()
        Try
            fDate = "2001-01-01 00:00:00.000"

            'Dim DT As New DataTable
            'Dim strSql As String = Session("StringQuery")
            Dim DT As DataTable = Session("StringQuery")
            'Response.write("aa"+Session("StringQuery").ToString)
            'DT = C.GetDataTableRepweb(strSql, True)

            Me.GridView1.DataSource = DT
            GridView1.DataBind()
            If DT.Rows.Count = 0 Then LblNo1.Visible = True

            fDate = Now().ToString("yyyy-MM-dd HH:mm:ss.000")  ') ', New Globalization.CultureInfo("en-US"))

        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        Finally
            Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            'Log.LogReport("21010", "รายงาน X-Read", Session("Parameter"), vIPAddress, Session("email"), Session("rdate"), fDate)
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
                'response.write(Session("Header").ToString)
                report_name.Text = vHeader(1).ToString
                comp_param.Text = vHeader(0).ToString
                ro_param.Text = vHeader(2).ToString
                prov_param.Text = vHeader(3).ToString
                shop_param.Text = vHeader(4).ToString 
                date_param.Text = vHeader(5).ToString
                report_param.Text = vHeader(6).ToString
            End If
        Catch ex As Exception

        Finally
            If comp_param.Text.Trim.Length = 0 Then comp_param.Visible = False
            If ro_param.Text.Trim.Length = 0 Then ro_param.Visible = False
            If prov_param.Text.Trim.Length = 0 Then prov_param.Visible = False
            If shop_param.Text.Trim.Length = 0 Then shop_param.Visible = False
            If date_param.Text.Trim.Length = 0 Then date_param.Visible = False
            If report_param.Text.Trim.Length = 0 Then report_param.Visible = False
        End Try
    End Sub

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
    Private sumAmount As Decimal = 0.0

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Com = Request.QueryString("com")
            SArea = Request.QueryString("sarea")
            EArea = Request.QueryString("earea")
            SProv = Request.QueryString("sprov")
            EProv = Request.QueryString("eprov")
            SBranch = Request.QueryString("sbranch")
            EBranch = Request.QueryString("ebranch")
            SDate = Request.QueryString("sdate")
            EDate = Request.QueryString("edate")

            CType(e.Row.FindControl("lblSumAmount"), Label).Text = Format(e.Row.Cells(13).Text - e.Row.Cells(16).Text + e.Row.Cells(14).Text + e.Row.Cells(15).Text, "#,0") 'CDbl(CType(e.Row.FindControl("HyperLink1"), HyperLink).Text)

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
            sumAmount += CDec(CType(e.Row.FindControl("lblSumAmount"), Label).Text)


            Dim DCS As Double = e.Row.Cells(7).Text
            DCS = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(DCS, "#,##0")

            Dim DCR As Double = e.Row.Cells(8).Text
            DCR = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(DCR, "#,0")

            Dim DCQ As Double = e.Row.Cells(9).Text
            DCQ = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(DCQ, "#,0")


            Dim DTR As Double = e.Row.Cells(10).Text
            DTR = String.Format(e.Row.Cells(10).Text, "{0:00,00.00}")
            e.Row.Cells(10).Text = Format(DTR, "#,0")

            Dim DWH As Double = e.Row.Cells(11).Text
            DWH = String.Format(e.Row.Cells(11).Text, "{0:00,00.00}")
            e.Row.Cells(11).Text = Format(DWH, "#,0")

            Dim DAO As Double = e.Row.Cells(12).Text
            DAO = String.Format(e.Row.Cells(12).Text, "{0:00,00.00}")
            e.Row.Cells(12).Text = Format(DAO, "#,0")

            Dim DA_Type As Double = e.Row.Cells(13).Text
            DA_Type = String.Format(e.Row.Cells(13).Text, "{0:00,00.00}")
            e.Row.Cells(13).Text = Format(DA_Type, "#,0")

            Dim DAD As Double = e.Row.Cells(14).Text
            DAD = String.Format(e.Row.Cells(14).Text, "{0:00,00.00}")
            e.Row.Cells(14).Text = Format(DAD, "#,0")

            Dim DAI As Double = e.Row.Cells(15).Text
            DAI = String.Format(e.Row.Cells(15).Text, "{0:00,00.00}")
            e.Row.Cells(15).Text = Format(DAI, "#,0")


            Dim DFR As Double = e.Row.Cells(16).Text
            DFR = String.Format(e.Row.Cells(16).Text, "{0:00,00.00}")
            e.Row.Cells(16).Text = Format(DFR, "#,0")
            Dim btn As HyperLink = e.Row.FindControl("DateDoc")

            Dim Shop As String = e.Row.Cells(4).Text
            Shop = Shop.Substring(7, 5)


            btn.Attributes("href") = "ShowrptPayinXread.aspx?com=" + Com + "&date=" + CType(e.Row.FindControl("DateDoc"), HyperLink).Text + "&sarea=" + SArea + "&earea=" + EArea + "&sprov=" + SProv + "&eprov=" + EProv + "&sbranch=" + Shop + "&ebranch=" + Shop
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "ยอดรวมทั้งหมด"

            e.Row.Cells(7).Text = sumCS.ToString
            e.Row.Cells(8).Text = sumCR.ToString
            e.Row.Cells(9).Text = sumCQ.ToString
            e.Row.Cells(10).Text = sumTR.ToString
            e.Row.Cells(11).Text = sumWH.ToString
            e.Row.Cells(12).Text = sumAO.ToString
            e.Row.Cells(13).Text = sumA_Type.ToString
            e.Row.Cells(14).Text = sumAD.ToString
            e.Row.Cells(15).Text = sumAI.ToString
            e.Row.Cells(16).Text = sumFR.ToString
            e.Row.Cells(17).Text = sumAmount.ToString


            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(12).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(13).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(14).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(15).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(16).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(17).HorizontalAlign = HorizontalAlign.Right

            Dim DCS As Double = e.Row.Cells(7).Text
            DCS = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(DCS, "#,0")

            Dim DCR As Double = e.Row.Cells(8).Text
            DCR = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(DCR, "#,0")

            Dim DCQ As Double = e.Row.Cells(9).Text
            DCQ = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(DCQ, "#,0")


            Dim DTR As Double = e.Row.Cells(10).Text
            DTR = String.Format(e.Row.Cells(10).Text, "{0:00,00.00}")
            e.Row.Cells(10).Text = Format(DTR, "#,0")

            Dim DWH As Double = e.Row.Cells(11).Text
            DWH = String.Format(e.Row.Cells(11).Text, "{0:00,00.00}")
            e.Row.Cells(11).Text = Format(DWH, "#,0")

            Dim DAO As Double = e.Row.Cells(12).Text
            DAO = String.Format(e.Row.Cells(12).Text, "{0:00,00.00}")
            e.Row.Cells(12).Text = Format(DAO, "#,0")

            Dim DA_Type As Double = e.Row.Cells(13).Text
            DA_Type = String.Format(e.Row.Cells(13).Text, "{0:00,00.00}")
            e.Row.Cells(13).Text = Format(DA_Type, "#,0")

            Dim DAD As Double = e.Row.Cells(14).Text
            DAD = String.Format(e.Row.Cells(14).Text, "{0:00,00.00}")
            e.Row.Cells(14).Text = Format(DAD, "#,0")

            Dim DAI As Double = e.Row.Cells(15).Text
            DAI = String.Format(e.Row.Cells(15).Text, "{0:00,00.00}")
            e.Row.Cells(15).Text = Format(DAI, "#,0")

            Dim DFR As Double = e.Row.Cells(16).Text
            DFR = String.Format(e.Row.Cells(16).Text, "{0:00,00.00}")
            e.Row.Cells(16).Text = Format(DFR, "#,0")

            Dim DSUM As Double = e.Row.Cells(17).Text
            DSUM = String.Format(e.Row.Cells(17).Text, "{0:00,00.00}")
            e.Row.Cells(17).Text = Format(DSUM, "#,0")

        End If
    End Sub

End Class
