Imports System.Data
Imports System.Threading
Partial Class ShowrptPayinXread
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim DocDate As String
    Dim Com As String
    Dim SArea As String
    Dim EArea As String
    Dim SProv As String
    Dim EProv As String
    Dim SBranch As String
    Dim EBranch As String
    Private sumCS As Decimal = 0.0
    Private sumCharge As Decimal = 0.0
    Private sumCQ As Decimal = 0.0
    Private sumAmount As Decimal = 0.0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Com = Request.QueryString("com")
        DocDate = Request.QueryString("date")
        SArea = Request.QueryString("sarea")
        EArea = Request.QueryString("earea")
        SProv = Request.QueryString("sprov")
        EProv = Request.QueryString("eprov")
        SBranch = Request.QueryString("sbranch")
        EBranch = Request.QueryString("ebranch")
        ShowReport()
    End Sub

    Private Sub ShowReport()
        Dim strSelect As String = ""
        Dim strGroup As String = ""
        Dim strOrder As String = ""
        Dim vHeader As String = ""
        Dim vComName As String
        Dim vSArea As String
        Dim vEArea As String
        Dim vSProvince As String
        Dim vEProvince As String
        Dim vSBranch As String
        Dim vEBranch As String
        Dim DT As New DataTable
        Dim DTCom As New DataTable

        If Com <> "" Then
            Dim SqlCom As String = "select distinct f01 as ComCode,f02 as ComName  from m00010 Where f01='" + Com + "'"
            DTCom = C.GetDataTableRepweb(SqlCom)
            vComName = DTCom.Rows(0).Item("ComName")
        End If
        Dim DTArea As New DataTable
        If SArea <> "" Then
            Dim SqlArea As String = "select distinct mo.f02 as AreaCode,mo.f03 as AreaName from  m02300 mo where mo.f02='" + SArea + "'"
            DTArea = C.GetDataTableRepweb(SqlArea)
            vSArea = DTArea.Rows(0).Item("AreaName")

        End If
        If EArea <> "" Then
            Dim SqlArea As String = "select distinct mo.f02 as AreaCode,mo.f03 as AreaName from  m02300 mo where mo.f02='" + EArea + "'"
            DTArea = C.GetDataTableRepweb(SqlArea)
            vEArea = DTArea.Rows(0).Item("AreaName")

        End If

        Dim DTBranch As New DataTable

        If SBranch <> "" Then
            Dim SqlSBranch As String = " select distinct f02 as BranchCode, (case f04 when 'สำนักงานใหญ่' then f17 else f04 end ) as BranchName from m00030 where f02='" + SBranch + "'"
            DTBranch = C.GetDataTableRepweb(SqlSBranch)
            vSBranch = DTBranch.Rows(0).Item("BranchName")
        End If
        If EBranch <> "" Then
            Dim SqlEBranch As String = " select distinct f02 as BranchCode, (case f04 when 'สำนักงานใหญ่' then f17 else f04 end ) as BranchName from m00030 where f02='" + EBranch + "'"
            DTBranch = C.GetDataTableRepweb(SqlEBranch)
            vEBranch = DTBranch.Rows(0).Item("BranchName")
        End If
        Try

            strSelect = " select m.f01,m.f02 'Branch',m.f05 'DocNo',convert(varchar(10),m.f06,103) 'DocDate',m.f12 'Account'"
            strSelect += " ,convert(varchar(10),m.f13,103) 'PayinDate',m.f15 'Note'"
            strSelect += " ,sum(d.f09) 'CS',sum(d.f14) 'Charge',sum(d.f12) 'CQ',m.f17+m.f18 'Sum'"
            strSelect += " from r16030 m,r16031 d,m00030 a,m02020 b"
            strSelect += " where m.f01=d.f01 and m.f02=d.f02 and m.f05=d.f05 and a.f12=b.f02 and a.f02=m.f02 and a.f02=d.f02 and m.f19='1'"

            '-----------Company----------

            If Com <> "" Then
                strSelect += " and m.f01 = '" + Com + "' "
                vHeader = vComName
            End If
            vHeader += "|"
            '----------- Area -----------

            If SArea <> "" And EArea <> "" Then
                strSelect += " and a.f03>='" + SArea + "' and a.f03<='" + EArea + "'"
                vHeader += "จาก" + vSArea + " ถึง " + vEArea
            Else
                vHeader += "ทุกเขตธุรกิจ"
            End If

            '-----------Province-----------
            If SProv <> "" And EProv <> "" Then
                strSelect += "  and b.f02 >='" + SProv + "' and  b.f02 <='" + EProv + "' "
                vHeader += "จากจังหวัด" + vSProvince + "  ถึงจังหวัด " + vEProvince
            Else
                vHeader += "ทุกจังหวัด" + "  "
            End If


            '----------- Branch -----------

            If SBranch <> "" And EBranch <> "" Then
                strSelect += " and a.f02 >='" + SBranch + "' and a.f02 <='" + EBranch + "'"
                vHeader += " จากสาขา " + vSBranch + " ถึงสาขา " + vEBranch
            Else
                vHeader += "ทุกสาขา" + "  "
            End If

            vHeader += "|"
            '----------- Date -----------

            If DocDate <> "" Then
                strSelect += " and convert(varchar(10),m.f06,103)='" + DocDate + "' "

                vHeader += " ตั้งแต่ วันที่ " + DocDate + "ถึงวันที่ " + DocDate
            End If
            'vHeader += "|"

            vHeader += "|"

            strSelect += "  group by m.f01,m.f02,m.f05,m.f06,m.f12,m.f13,m.f15,m.f17,m.f18"
            strSelect += "  Order by m.f01,m.f02,m.f05,m.f06,m.f12,m.f13"

            'Response.Write(strSelect)
            Session("StringQuery") = strSelect
            Session("Header") = vHeader
            DT = C.GetDataTableRepweb(strSelect)
            GridView1.DataSource = DT
            GridView1.DataBind()

            If DT.Rows.Count = 0 Then LblNo1.Visible = True

            Dim vDate As String
            vDate = Format(Date.Now, "dd/MM/yyyy")
            If Mid(vDate, 7, 4) > 2500 Then
                vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) - 543).ToString
            End If
            LblHeader.Text = "ข้อมูลที่ Server ณ. วันที่ " + vDate + " เวลา " + Format(Now, "HH:mm")

            Try
                Dim vHeaders() As String
                If vHeader IsNot Nothing Then
                    vHeaders = Split(vHeader, "|")
                    LblParam1.Text = vHeaders(0).ToString
                    LblParam2.Text = vHeaders(1).ToString
                    LblParam3.Text = vHeaders(2).ToString
                    LblParam4.Text = vHeaders(3).ToString
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
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            sumAmount += CDec(DataBinder.Eval(e.Row.DataItem, "Sum"))
            sumCS += CDec(DataBinder.Eval(e.Row.DataItem, "CS"))
            sumCharge += CDec(DataBinder.Eval(e.Row.DataItem, "Charge"))
            sumCQ += CDec(DataBinder.Eval(e.Row.DataItem, "CQ"))

            Dim CS As Double = e.Row.Cells(6).Text
            CS = String.Format(e.Row.Cells(6).Text, "{0:00,00.00}")
            e.Row.Cells(6).Text = Format(CS, "#,##0.00")

            Dim Charge As Double = e.Row.Cells(7).Text
            Charge = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(Charge, "#,##0.00")

            Dim CQ As Double = e.Row.Cells(8).Text
            CQ = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(CQ, "#,##0.00")

            Dim Sum As Double = e.Row.Cells(9).Text
            Sum = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(Sum, "#,##0.00")


            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(5).Width = 250

            Dim btn As HyperLink = e.Row.FindControl("PayinDate")
            btn.Attributes("href") = "http://posweb.triplet.co.th/PayInSlip/ShowDataLink.aspx?e=" + Session("email") + "&com=" + Com + "&date=" + CType(e.Row.FindControl("PayinDate"), HyperLink).Text + "&branch=" + e.Row.Cells(0).Text
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(5).Text = "รวม::"

            e.Row.Cells(6).Text = sumCS.ToString
            e.Row.Cells(7).Text = sumCharge.ToString
            e.Row.Cells(8).Text = sumCQ.ToString
            e.Row.Cells(9).Text = sumAmount.ToString


            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right

            Dim Sum_CS As Double = e.Row.Cells(6).Text
            Sum_CS = String.Format(e.Row.Cells(6).Text, "{0:00,00.00}")
            e.Row.Cells(6).Text = Format(Sum_CS, "#,##0.00")

            Dim Sum_Charge As Double = e.Row.Cells(7).Text
            Sum_Charge = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(Sum_Charge, "#,##0.00")

            Dim Sum_CQ As Double = e.Row.Cells(8).Text
            Sum_CQ = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(Sum_CQ, "#,##0.00")

            Dim Sum_Amount As Double = e.Row.Cells(9).Text
            Sum_Amount = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(Sum_Amount, "#,##0.00")



        End If
    End Sub
End Class
