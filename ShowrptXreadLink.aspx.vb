Imports System.Data
Imports System.Threading
Partial Class ShowrptXreadLink
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Area As String
    Dim DocDate As String
    Dim Com As String
    Dim Branch As String
    Dim SDate As String
    Dim EDate As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Area = Request.QueryString("area")
        Com = Request.QueryString("com")
        Branch = Request.QueryString("branch")
        SDate = Request.QueryString("sdate")
        EDate = Request.QueryString("edate")
        ShowReport()

    End Sub

    Private Sub ShowReport()
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
        If Area <> "" Then
            Dim SqlArea As String = "select distinct mo.f02 as AreaCode,mo.f03 as AreaName from  m02300 mo where mo.f02='" + Area + "'"
            DTArea = C.GetDataTableRepweb(SqlArea)
            vSArea = DTArea.Rows(0).Item("AreaName")
            vEArea = DTArea.Rows(0).Item("AreaName")
        End If
       

        Dim DTBranch As New DataTable

        If Branch <> "" Then
            Dim SqlSBranch As String = " select distinct f02 as BranchCode, (case f04 when 'สำนักงานใหญ่' then f17 else f04 end ) as BranchName from m00030 where f02='" + Branch + "'"
            DTBranch = C.GetDataTableRepweb(SqlSBranch)
            vSBranch = DTBranch.Rows(0).Item("BranchName")
        End If


        Dim strSelect As String = ""
        Dim strGroup As String = ""
        Dim strOrder As String = ""
        Dim vHeader As String = ""

        '-------------------------- Bill ---------------------------------
        strSelect = " select  Rec.Area,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f10 'Sales', case b.f11 when 'A' then 'ยกเลิก' else '' end 'Status',"
        strSelect += " Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR  from "
        strSelect += " (select s.f03 'Area',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
        strSelect += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CS' then r.f09 else 0.00 end))end) 'CS',"
        strSelect += " (case b.f11 when 'A' then '0.00' else (sum(case r.f07 when'CR' then r.f09 else 0.00 end))end) 'CR',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) end)'CQ',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) end)'TR',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'WH' then r.f09 else 0.00 end)) end) 'WH',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AO' then r.f09 else 0.00 end))end) 'AO', "
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then r.f09 else 0.00 end)) end)'A_TYPE',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AD' then r.f09 else 0.00 end))end) 'AD',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AI' then r.f09 else 0.00 end))end) 'AI',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'FR' then r.f09 else 0.00 end))end) 'FR'"
        strSelect += " from R16060 r , r17510 b,m00030 s, m02020 a "
        strSelect += " where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and b.f04 in ('DRCPV','DRCPN')"
        '-----------Company----------
        If Com <> "" Then
            strSelect += "  and b.f01 = '" + Com + "' "
            vHeader = vComName
        End If
        vHeader += "|"
        '----------- Area -----------

        If Area <> "" Then
            strSelect += "  and s.f03='" + Area + "' "
            vHeader += " จากเขต " + vSArea + " ถึงเขต " + vEArea
        End If

      
       
        '----------- Branch -----------
        If Branch <> "" Then
            strSelect += "  and b.f02='" + Branch + "'  "
            vHeader += " จากสาขา " + vSBranch + " ถึงสาขา " + vSBranch
        End If
        
        vHeader += "|"
        '----------- Date -----------

        If SDate <> "" And EDate <> "" Then
            strSelect += "  and replace(convert(varchar(10),b.f06,21),'-','/')>='" + SDate + "' and  replace(convert(varchar(10),b.f06,21),'-','/')<='" + EDate + "'"
            vHeader += " ตั้งแต่ วันที่ " + SDate + " ถึงวันที่ " + EDate
        End If
        vHeader += "|"
        strSelect += " group by b.f05,b.f11 ,s.f03,s.f04,s.f17  ) Rec  ,r17510 b where  Rec.Doc=b.f05"
        '-------------------------------------------------
        strSelect += " Union"
        '-------------------------------------------------
        '-------------------------- ลดหนี้ Bill ---------------------------------
        strSelect += " select  Rec.Area,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f10 'Sales', case b.f11 when 'A' then 'ยกเลิก' else '' end 'Status',Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR  from "
        strSelect += " (select s.f03 'Area',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
        strSelect += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CS' then (r.f09*-1) else 0.00 end))end) 'CS',"
        strSelect += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CR' then (r.f09*-1)else 0.00 end))end) 'CR',"
        strSelect += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CQ' then (r.f09*-1) else 0.00 end)) end)'CQ',"
        strSelect += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'TR' then (r.f09*-1) else 0.00 end)) end)'TR',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'WH' then (r.f09*-1) else 0.00 end)) end) 'WH',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AO' then  (r.f09*-1) else 0.00 end))end) 'AO', "
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then (r.f09*-1) else 0.00 end)) end)'A_TYPE',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AD' then (r.f09*-1)else 0.00 end))end) 'AD',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AI' then  (r.f09*-1) else 0.00 end))end) 'AI',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'FR' then (r.f09*-1) else 0.00 end))end) 'FR'"
        strSelect += " from R16060 r , r17510 b,m00030 s, m02020 a "
        strSelect += " where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and b.f04 in('DCNPV','DCNPN')"
        '-----------Company----------
        If Com <> "" Then
            strSelect += "  and b.f01 = '" + Com + "' "
            vHeader = vComName
        End If
        vHeader += "|"
        '----------- Area -----------

        If Area <> "" Then
            strSelect += "  and s.f03='" + Area + "' "
            vHeader += " จากเขต " + vSArea + " ถึงเขต " + vEArea
        End If



        '----------- Branch -----------
        If Branch <> "" Then
            strSelect += "  and b.f02='" + Branch + "'"
            vHeader += " จากสาขา " + vSBranch + " ถึงสาขา " + vSBranch
        End If

        vHeader += "|"
        '----------- Date -----------

        If SDate <> "" And EDate <> "" Then
            strSelect += "  and replace(convert(varchar(10),b.f06,21),'-','/')>='" + SDate + "' and  replace(convert(varchar(10),b.f06,21),'-','/')<='" + EDate + "'"
            vHeader += " ตั้งแต่ วันที่ " + SDate + " ถึงวันที่ " + EDate
        End If
        vHeader += "|"
        strSelect += " group by b.f05,b.f11 ,s.f03,s.f04,s.f17  ) Rec  ,r17510 b where  Rec.Doc=b.f05 "
        '----------------------------------------------
        strSelect += " Union"
        '----------------------------------------------
        '----------------------------OTC----------------------------
        strSelect += " select  Rec.Area,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f10 'Sales', case b.f11 when 'A' then 'ยกเลิก' else '' end 'Status',Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR  from "
        strSelect += " (select s.f03 'Area',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
        strSelect += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CS' then r.f09 else 0.00 end))end) 'CS',"
        strSelect += " (case b.f11 when 'A' then '0.00' else (sum(case r.f07 when'CR' then r.f09 else 0.00 end))end) 'CR',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) end)'CQ',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) end)'TR',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'WH' then r.f09 else 0.00 end)) end) 'WH',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AO' then r.f09 else 0.00 end))end) 'AO', "
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then r.f09  else 0.00 end)) end)'A_TYPE',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AD' then r.f09 else 0.00 end))end) 'AD',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AI' then r.f09 else 0.00 end))end) 'AI',"
        strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'FR' then r.f09 else 0.00 end))end) 'FR'"
        strSelect += " from R16060 r , r17520 b,m00030 s, m02020 a "
        strSelect += " where(s.f12 = a.f02 And s.f02 = b.f02 And b.f05 = r.f05)"
        '-----------Company----------
        If Com <> "" Then
            strSelect += "  and b.f01 = '" + Com + "' "
            vHeader = vComName
        End If
        vHeader += "|"
        '----------- Area -----------

        If Area <> "" Then
            strSelect += "  and s.f03>'" + Area + "'"
            vHeader += " จากเขต " + vSArea + " ถึงเขต " + vEArea
        End If



        '----------- Branch -----------
        If Branch <> "" Then
            strSelect += "  and b.f02='" + Branch + "'"
            vHeader += " จากสาขา " + vSBranch + " ถึงสาขา " + vSBranch
        End If

        vHeader += "|"
        '----------- Date -----------

        If SDate <> "" And EDate <> "" Then
            strSelect += "  and replace(convert(varchar(10),b.f06,21),'-','/')>='" + SDate + "' and  replace(convert(varchar(10),b.f06,21),'-','/')<='" + EDate + "'"
            vHeader += " ตั้งแต่ วันที่ " + SDate + " ถึงวันที่ " + EDate
        End If
        vHeader += "|"
        strSelect += " group by b.f05,b.f11 ,s.f03,s.f04,s.f17  ) Rec  ,r17520 b where  Rec.Doc=b.f05  "
        '--------------------------------------
        strSelect += " Union"
        '--------------------------------------
        '-------------------------------ขายสด--------------------------
        strSelect += " select  Rec.Area,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f64 'Sales',"
        strSelect += " case b.f63 when 'A' then 'ยกเลิก' else '' end 'Status',"
        strSelect += " Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR  from "
        strSelect += " (select s.f03 'Area',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
        strSelect += " (case b.f63 when 'A' then '0.00' else(sum(case r.f07 when'CS' then r.f09 else 0.00 end))end) 'CS',"
        strSelect += " (case b.f63 when 'A' then '0.00' else (sum(case r.f07 when'CR' then r.f09 else 0.00 end))end) 'CR',"
        strSelect += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) end)'CQ',"
        strSelect += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) end)'TR',"
        strSelect += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'WH' then r.f09 else 0.00 end)) end) 'WH',"
        strSelect += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'AO' then r.f09 else 0.00 end))end) 'AO', "
        strSelect += " (case b.f63 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then r.f09  else 0.00 end)) end)'A_TYPE',"
        strSelect += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'AD' then r.f09 else 0.00 end))end) 'AD',"
        strSelect += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'AI' then r.f09 else 0.00 end))end) 'AI',"
        strSelect += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'FR' then r.f09 else 0.00 end))end) 'FR'"
        strSelect += " from R16060 r , r11060 b,m00030 s, m02020 a "
        strSelect += " where(s.f12 = a.f02 And s.f02 = b.f02 And b.f05 = r.f05)"
        '-----------Company----------
        If Com <> "" Then
            strSelect += "  and b.f01 = '" + Com + "' "
            vHeader = vComName
        End If
        vHeader += "|"
        '----------- Area -----------

        If Area <> "" Then
            strSelect += "  and s.f03='" + Area + "' "
            vHeader += " จากเขต " + vSArea + " ถึงเขต " + vEArea
        End If



        '----------- Branch -----------
        If Branch <> "" Then
            strSelect += "  and b.f02='" + Branch + "'"
            vHeader += " จากสาขา " + vSBranch + " ถึงสาขา " + vSBranch
        End If

        vHeader += "|"
        '----------- Date -----------

        If SDate <> "" And EDate <> "" Then
            strSelect += "  and replace(convert(varchar(10),b.f06,21),'-','/')>='" + SDate + "' and  replace(convert(varchar(10),b.f06,21),'-','/')<='" + EDate + "'"
            vHeader += " ตั้งแต่ วันที่ " + SDate + " ถึงวันที่ " + EDate
        End If
        vHeader += "|"
        strSelect += " group by b.f05,b.f63,s.f03,s.f04,s.f17   ) Rec  ,r11060 b where  Rec.Doc=b.f05   "
        '-----------------------------------
        strSelect += " Union"
        '-------------------------------------
        '----------------ลดหนี้ขายสด  ลดหนี้ OTC------------------------
        strSelect += " select  Rec.Area,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC,b.f36 'Sales', ''as  'Status',"
        strSelect += " Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR  from "
        strSelect += " (select s.f03 'Area',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
        strSelect += " (sum(case r.f07 when'CS' then (r.f09*-1) else 0.00 end)) 'CS',"
        strSelect += " (sum(case r.f07 when'CR' then  (r.f09*-1) else 0.00 end)) 'CR',"
        strSelect += " (sum(case r.f07 when'CQ' then (r.f09*-1) else 0.00 end))'CQ',"
        strSelect += " ( sum(case r.f07 when'TR' then  (r.f09*-1) else 0.00 end))'TR',"
        strSelect += " ( sum(case r.f07 when'WH' then  (r.f09*-1) else 0.00 end))  'WH',"
        strSelect += " ( sum(case r.f07 when'AO' then  (r.f09*-1) else 0.00 end)) 'AO', "
        strSelect += " ( sum(case when  r.f07 <>'FR' then  (r.f09*-1)  else 0.00 end))'A_TYPE',"
        strSelect += " ( sum(case r.f07 when'AD' then (r.f09*-1) else 0.00 end)) 'AD',"
        strSelect += " ( sum(case r.f07 when'AI' then  (r.f09*-1) else 0.00 end)) 'AI',"
        strSelect += " ( sum(case r.f07 when'FR' then (r.f09*-1) else 0.00 end)) 'FR'"
        strSelect += " from R16060 r , r11090 b,m00030 s, m02020 a "
        strSelect += " where(s.f12 = a.f02 And s.f02 = b.f02 And b.f05 = r.f05)"
        '-----------Company----------
        If Com <> "" Then
            strSelect += "  and b.f01 = '" + Com + "' "
            vHeader = vComName
        End If
        vHeader += "|"
        '----------- Area -----------

        If Area <> "" Then
            strSelect += "  and s.f03='" + Area + "' "
            vHeader += " จากเขต " + vSArea + " ถึงเขต " + vEArea
        End If
        '----------- Branch -----------
        If Branch <> "" Then
            strSelect += "  and b.f02='" + Branch + "' "
            vHeader += " จากสาขา " + vSBranch + " ถึงสาขา " + vSBranch
        End If

        vHeader += "|"
        '----------- Date -----------

        If SDate <> "" And EDate <> "" Then
            strSelect += "  and replace(convert(varchar(10),b.f06,21),'-','/')>='" + SDate + "' and  replace(convert(varchar(10),b.f06,21),'-','/')<='" + EDate + "'"
            vHeader += " ตั้งแต่ วันที่ " + SDate + " ถึงวันที่ " + EDate
        End If
        vHeader += "|"
        strSelect += " group by b.f05 ,s.f03,s.f04,s.f17  ) Rec  ,r11090 b where  Rec.Doc=b.f05 "
        strSelect += " order by convert(varchar(10),b.f06,103),Rec.DOC,b.f10 "
        DT = C.GetDataTableRepweb(strSelect)
        GridView1.DataSource = DT
        GridView1.DataBind()

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

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

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


            Dim DCS As Double = e.Row.Cells(6).Text
            DCS = String.Format(e.Row.Cells(6).Text, "{0:00,00.00}")
            e.Row.Cells(6).Text = Format(DCS, "#,##0.00")

            Dim DCR As Double = e.Row.Cells(7).Text
            DCR = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(DCR, "#,##0.00")

            Dim DCQ As Double = e.Row.Cells(8).Text
            DCQ = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(DCQ, "#,##0.00")


            Dim DTR As Double = e.Row.Cells(9).Text
            DTR = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(DTR, "#,##0.00")

            Dim DWH As Double = e.Row.Cells(10).Text
            DWH = String.Format(e.Row.Cells(10).Text, "{0:00,00.00}")
            e.Row.Cells(10).Text = Format(DWH, "#,##0.00")

            Dim DAO As Double = e.Row.Cells(11).Text
            DAO = String.Format(e.Row.Cells(11).Text, "{0:00,00.00}")
            e.Row.Cells(11).Text = Format(DAO, "#,##0.00")

            Dim DA_Type As Double = e.Row.Cells(12).Text
            DA_Type = String.Format(e.Row.Cells(12).Text, "{0:00,00.00}")
            e.Row.Cells(12).Text = Format(DA_Type, "#,##0.00")

            Dim DAD As Double = e.Row.Cells(13).Text
            DAD = String.Format(e.Row.Cells(13).Text, "{0:00,00.00}")
            e.Row.Cells(13).Text = Format(DAD, "#,##0.00")

            Dim DAI As Double = e.Row.Cells(14).Text
            DAI = String.Format(e.Row.Cells(14).Text, "{0:00,00.00}")
            e.Row.Cells(14).Text = Format(DAI, "#,##0.00")


            Dim DFR As Double = e.Row.Cells(15).Text
            DFR = String.Format(e.Row.Cells(15).Text, "{0:00,00.00}")
            e.Row.Cells(15).Text = Format(DFR, "#,##0.00")

            Dim btn As HyperLink = e.Row.FindControl("DateDoc")
            btn.Attributes("href") = "ShowrptPayinLink.aspx?com=" + Com + "&date=" + CType(e.Row.FindControl("DateDoc"), HyperLink).Text + "&area=" + Area + "&branch=" + Branch
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(5).Text = "ยอดรวมทั้งหมด"

            e.Row.Cells(6).Text = sumCS.ToString
            e.Row.Cells(7).Text = sumCR.ToString
            e.Row.Cells(8).Text = sumCQ.ToString
            e.Row.Cells(9).Text = sumTR.ToString
            e.Row.Cells(10).Text = sumWH.ToString
            e.Row.Cells(11).Text = sumAO.ToString
            e.Row.Cells(12).Text = sumA_Type.ToString
            e.Row.Cells(13).Text = sumAD.ToString
            e.Row.Cells(14).Text = sumAI.ToString
            e.Row.Cells(15).Text = sumFR.ToString

            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(12).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(13).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(14).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(15).HorizontalAlign = HorizontalAlign.Right


            Dim DCS As Double = e.Row.Cells(6).Text
            DCS = String.Format(e.Row.Cells(6).Text, "{0:00,00.00}")
            e.Row.Cells(6).Text = Format(DCS, "#,##0.00")

            Dim DCR As Double = e.Row.Cells(7).Text
            DCR = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(DCR, "#,##0.00")

            Dim DCQ As Double = e.Row.Cells(8).Text
            DCQ = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(DCQ, "#,##0.00")


            Dim DTR As Double = e.Row.Cells(9).Text
            DTR = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(DTR, "#,##0.00")

            Dim DWH As Double = e.Row.Cells(10).Text
            DWH = String.Format(e.Row.Cells(10).Text, "{0:00,00.00}")
            e.Row.Cells(10).Text = Format(DWH, "#,##0.00")

            Dim DAO As Double = e.Row.Cells(11).Text
            DAO = String.Format(e.Row.Cells(11).Text, "{0:00,00.00}")
            e.Row.Cells(11).Text = Format(DAO, "#,##0.00")

            Dim DA_Type As Double = e.Row.Cells(12).Text
            DA_Type = String.Format(e.Row.Cells(12).Text, "{0:00,00.00}")
            e.Row.Cells(12).Text = Format(DA_Type, "#,##0.00")

            Dim DAD As Double = e.Row.Cells(13).Text
            DAD = String.Format(e.Row.Cells(13).Text, "{0:00,00.00}")
            e.Row.Cells(13).Text = Format(DAD, "#,##0.00")

            Dim DAI As Double = e.Row.Cells(14).Text
            DAI = String.Format(e.Row.Cells(14).Text, "{0:00,00.00}")
            e.Row.Cells(14).Text = Format(DAI, "#,##0.00")


            Dim DFR As Double = e.Row.Cells(15).Text
            DFR = String.Format(e.Row.Cells(15).Text, "{0:00,00.00}")
            e.Row.Cells(15).Text = Format(DFR, "#,##0.00")


        End If
    End Sub
End Class
