Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Net.IPAddress

Partial Class X_Read_Edit
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim CF As New Cls_RequestFlow
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Add Class Radio
            rAllReceipt.InputAttributes.Add("class", "form-check-input")
            rReceipt.InputAttributes.Add("class", "form-check-input")
            rReturnReceipt.InputAttributes.Add("class", "form-check-input")
            rCancleReceipt.InputAttributes.Add("class", "form-check-input")
        ' Check PostBack
            If SDate.Text <> "" then
                ClientScript.RegisterStartupScript(Page.GetType, "GetSDate", "$('#SDate').val('"+SDate.Text+"');", True)
            End If
            If EDate.Text <> "" then
                ClientScript.RegisterStartupScript(Page.GetType, "GetEDate", "$('#EDate').val('"+EDate.Text+"');", True)
            End If
        If Not Page.IsPostBack Then
            LoadCompany()
            LoadArea()
            LoadProvince("")
            LoadBranch("")

            '////////// Cookie ///////
            Try
                ReadCookies()

            Catch ex As Exception

                WriteCookies("e")

            End Try
            '////////////////////////

        End If
    End Sub
    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        C.SetDropDownList(cboCompany, vSql, "ComName", "ComCode")
    End Sub

    ' โหลด RO
    Private Sub LoadArea()
        vSql = CF.rSqlDDArea(Session("Uemail"),0)
        C.SetDropDownList(cboSArea, vSql, "AreaName", "AreaCode", "ดูข้อมูลทั้งหมด", cboEArea)
        ' ถ้าสำนักงานเป็น ALL
        If cboSArea.Items.Count = 1 Then
            vSql = CF.rSqlDDArea(Session("Uemail"),1)
            C.SetDropDownList(cboSArea, vSql, "AreaName", "AreaCode", "ดูข้อมูลทั้งหมด", cboEArea)
        End If
        'Response.Write(vSql)
    End Sub
    Private Sub LoadProvince(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            If cboSArea.SelectedIndex <> 0 Then
                cboSProvince.Enabled = True
                cboEProvince.Enabled = True
                strW = "  o.f03 >= '" & cboSArea.SelectedValue.ToString & "'"
            End If
            If cboEArea.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW += " o.f03 <= '" & cboEArea.SelectedValue.ToString & "'"
            End If
            Wherecause = strW

        End If

        vsql = "select distinct CAST(m02.f02 AS varchar) as ProvinceCode, m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 "
        vsql += "where ma.f03 = o.f02 and m02.f02 = o.f12 and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " "
        C.SetDropDownList(cboSProvince, vSql, "ProvinceName", "ProvinceCode", "ดูข้อมูลทั้งหมด", cboEProvince)
        If cboSProvince.Items.Count = 1 Then
            vSql = "select distinct CAST(m02.f02 AS varchar) as ProvinceCode,m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and m02.f02 = o.f12 and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " "
            C.SetDropDownList(cboSProvince, vSql, "ProvinceName", "ProvinceCode", "ดูข้อมูลทั้งหมด", cboEProvince)
        End If
    End Sub

    Private Sub LoadBranch(ByVal Wherecause As String)
        If Wherecause <> "" Then
            Dim strW As String = ""
            If cboSArea.SelectedIndex <> 0 Then
                strW = "o.f03 >= '" & cboSArea.SelectedValue.ToString & "'"
            End If
            If cboEArea.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW += "o.f03 <= '" & cboEArea.SelectedValue.ToString & "'"
            End If

            If cboSProvince.SelectedIndex <> 0 Then
                cboSBranch.Enabled = True
                cboEBranch.Enabled = True
                If strW <> "" Then strW = strW + " and "
                strW = strW + "m02.f02 >= '" & cboSProvince.SelectedValue.ToString & "'"
            End If
            If cboEProvince.SelectedIndex <> 0 Then
                cboSBranch.Enabled = True
                cboEBranch.Enabled = True
                If strW <> "" Then strW = strW + " and "
                strW = strW + "m02.f02 <= '" & cboEProvince.SelectedValue.ToString & "'"
            End If
            Wherecause = strW
        End If

        vSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
        C.SetDropDownList(cboSBranch, vSql, "BranchName", "BranchCode", "ดูข้อมูลทั้งหมด", cboEBranch)
        If cboSBranch.Items.Count = 1 Then
            vSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
            C.SetDropDownList(cboSBranch, vSql, "BranchName", "BranchCode", "ดูข้อมูลทั้งหมด", cboEBranch)
        End If
    End Sub
    Protected Sub cboSArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSArea.SelectedIndexChanged
        LoadProvince("Where")
        LoadBranch("Where")
    End Sub

    Protected Sub cboEArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEArea.SelectedIndexChanged
        LoadProvince("Where")
        LoadBranch("Where")
    End Sub
    Protected Sub cboSProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSProvince.SelectedIndexChanged    
        LoadBranch("Where")
    End Sub

    Protected Sub cboEProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEProvince.SelectedIndexChanged
        LoadBranch("Where")
    End Sub
  

    Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click

        '////////// Cookie ///////
        If CheckCookies.Checked = True Then
            WriteCookies("true")
        Else
            WriteCookies("false")
        End If
        '//////////////
        If rCancleReceipt.Checked = True Then
            ShowRptCancel()
        Else
            ShowRpt()
        End If


    End Sub
    Private Sub ShowRpt()
        Dim strSelect As String = ""
        Dim strSelect1 As String = ""
        Dim strSelect2 As String = ""
        Dim strSelect3 As String = ""
        Dim strSelect4 As String = ""
        Dim strGroup As String = ""
        Dim strOrder As String = ""
        Dim vHeader As String = ""
        Dim vComName As String = cboCompany.SelectedItem.ToString
        Dim vSArea As String = cboSArea.SelectedItem.ToString
        Dim vEArea As String = cboEArea.SelectedItem.ToString
        Dim vSProvince As String = cboSProvince.SelectedItem.ToString
        Dim vEProvince As String = cboEProvince.SelectedItem.ToString
        Dim vSBranch As String = cboSBranch.SelectedItem.ToString
        Dim vEBranch As String = cboEBranch.SelectedItem.ToString
        Dim sb As New StringBuilder
        Try
            '-------------------------- Bill ---------------------------------
            strSelect = " select  Rec.Area,Rec.BranchCode,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f10 'Sales', case b.f11 when 'A' then 'ยกเลิก' else '' end 'Status',"
            strSelect += " Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR,"
            strSelect += " Substring(b.f07,1,2)+':'+Substring(b.f07,3,2) AS 'TimeDoc'"
            strSelect += " from"
            strSelect += " (select s.f03 'Area',s.f02 'BranchCode',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
            strSelect += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CS' then r.f09 else 0.00 end))end) 'CS',"
            strSelect += " (case b.f11 when 'A' then '0.00' else (sum(case r.f07 when'CR' then r.f09 else 0.00 end))end) 'CR',"
            strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) end)'CQ',"
            strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) end)'TR',"
            strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'WH' then r.f09 else 0.00 end)) end) 'WH',"
            strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AO' then r.f09 else 0.00 end))end) 'AO', "
            strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))end)'A_TYPE',"
            strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AD' then r.f09 else 0.00 end))end) 'AD',"
            strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end))end) 'AI',"
            strSelect += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'FR' then r.f09 else 0.00 end))end) 'FR'"
            strSelect += " from R16060 r with(nolock) , r17510 b with(nolock) ,m00030 s with(nolock) , m02020 a with(nolock) "
            strSelect += " where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and b.f04 in ('DRCPV','DRCPN')"
            '-----------Company----------
            'If cboCompany.SelectedIndex <> 0 Then
            strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' " '-- Company select f01,f02 from m00010
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "| [21010] รายงาน X-Read |"

            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect += "  and s.f03>='" + cboSArea.SelectedValue.ToString + "'"
                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "
            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect += "  and s.f03<='" + cboEArea.SelectedValue.ToString + "'"
                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "
            End If
            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "  '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020
                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If
            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If
            vHeader += "|"
            '----------- Date -----------

            If SDate.Text.Length <> 0 Then
                strSelect += "  and b.f06 >='" + SDate.Text + "' "
                vHeader += " ตั้งแต่ วันที่ " + SDate.Text
            End If

            If EDate.Text.Length <> 0 Then
                strSelect += "  and b.f06 <='" + EDate.Text + "' "
                vHeader += " ถึงวันที่ " + EDate.Text
            End If
            vHeader += "|"
            strSelect += " group by b.f05,b.f11,s.f02,s.f03,s.f04,s.f17   ) Rec  ,r17510 b where  Rec.Doc=b.f05"
            strSelect += " order by Rec.DOC "
            '-------------------------------------------------
            'strSelect += " Union"
            '-------------------------------------------------
            '-------------------------- ลดหนี้ Bill ---------------------------------
            strSelect1 += " select  Rec.Area,Rec.BranchCode,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f10 'Sales', case b.f11 when 'A' then 'ยกเลิก' else '' end 'Status',Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR,Substring(b.f07,1,2)+':'+Substring(b.f07,3,2) AS 'TimeDoc'  from "
            strSelect1 += " (select s.f03 'Area',s.f02 'BranchCode',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CS' then (r.f09*-1) else 0.00 end))end) 'CS',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CR' then (r.f09*-1)else 0.00 end))end) 'CR',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CQ' then (r.f09*-1) else 0.00 end)) end)'CQ',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'TR' then (r.f09*-1) else 0.00 end)) end)'TR',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'WH' then (r.f09*-1) else 0.00 end)) end) 'WH',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AO' then  (r.f09*-1) else 0.00 end))end) 'AO', "
            strSelect1 += " (case b.f11 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else (r.f09*-1) end) else 0.00 end))end)'A_TYPE',"
            'strSelect1 += " (case b.f11 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then (r.f09*-1) else 0.00 end)) end)'A_TYPE',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AD' then (r.f09*-1)else 0.00 end))end) 'AD',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AI' then  (r.f09) else 0.00 end))end) 'AI',"
            strSelect1 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'FR' then (r.f09*-1) else 0.00 end))end) 'FR'"
            strSelect1 += " from R16060 r with(nolock) , r17510 b with(nolock) ,m00030 s with(nolock) , m02020 a with(nolock) "
            strSelect1 += " where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and b.f04 in('DCNPV','DCNPN')"
            '-----------Company----------
            'If cboCompany.SelectedIndex <> 0 Then
            strSelect1 += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' " '-- Company select f01,f02 from m00010
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "| [21010] รายงาน X-Read |"
            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect1 += "  and s.f03>='" + cboSArea.SelectedValue.ToString + "'"
                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "
            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect1 += "  and s.f03<='" + cboEArea.SelectedValue.ToString + "'"
                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "
            End If
            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect1 += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "  '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect1 += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020
                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If
            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect1 += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect1 += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If
            vHeader += "|"
            '----------- Date -----------

            If SDate.Text.Length <> 0 Then
                strSelect1 += "  and b.f06 >='" + SDate.Text + "' "
                vHeader += " ตั้งแต่ วันที่ " + SDate.Text
            End If

            If EDate.Text.Length <> 0 Then
                strSelect1 += "  and b.f06 <='" + EDate.Text + "'"
                vHeader += " ถึงวันที่ " + EDate.Text
            End If
            vHeader += "|"
            strSelect1 += " group by b.f05,b.f11,s.f02,s.f03,s.f04 ,s.f17    ) Rec  ,r17510 b where  Rec.Doc=b.f05 "
            strSelect1 += " order by Rec.DOC "
            '----------------------------------------------
            'strSelect += " Union"
            '----------------------------------------------
            '----------------------------OTC----------------------------
            strSelect2 += " select  Rec.Area,Rec.BranchCode,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f10 'Sales', case b.f11 when 'A' then 'ยกเลิก' else '' end 'Status',Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR,Substring(b.f07,1,2)+':'+Substring(b.f07,3,2) AS 'TimeDoc'  from "
            strSelect2 += " (select s.f03 'Area',s.f02 'BranchCode',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else(sum(case r.f07 when'CS' then r.f09 else 0.00 end))end) 'CS',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else (sum(case r.f07 when'CR' then r.f09 else 0.00 end))end) 'CR',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) end)'CQ',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) end)'TR',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'WH' then r.f09 else 0.00 end)) end) 'WH',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AO' then r.f09 else 0.00 end))end) 'AO', "
            strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))end)'A_TYPE',"
            'strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then r.f09  else 0.00 end)) end)'A_TYPE',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AD' then r.f09 else 0.00 end))end) 'AD',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end))end) 'AI',"
            strSelect2 += " (case b.f11 when 'A' then '0.00' else( sum(case r.f07 when'FR' then r.f09 else 0.00 end))end) 'FR'"
            strSelect2 += " from R16060 r with(nolock) , r17520 b with(nolock) ,m00030 s with(nolock) , m02020 a with(nolock) "
            strSelect2 += " where(s.f12 = a.f02 And s.f02 = b.f02 And b.f05 = r.f05)"
            '-----------Company----------
            'If cboCompany.SelectedIndex <> 0 Then
            strSelect2 += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' " '-- Company select f01,f02 from m00010
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "| [21010] รายงาน X-Read |"
            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect2 += "  and s.f03>='" + cboSArea.SelectedValue.ToString + "'"
                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "
            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect2 += "  and s.f03<='" + cboEArea.SelectedValue.ToString + "'"
                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "
            End If
            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect2 += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "  '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect2 += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020
                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If
            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect2 += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect2 += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If
            vHeader += "|"
            '----------- Date -----------

            If SDate.Text.Length <> 0 Then
                strSelect2 += "  and b.f06 >='" + SDate.Text + "' "
                vHeader += " ตั้งแต่ วันที่ " + SDate.Text
            End If

            If EDate.Text.Length <> 0 Then
                strSelect2 += "  and b.f06 <='" + EDate.Text + "'"
                vHeader += " ถึงวันที่ " + EDate.Text
            End If
            vHeader += "|"
            strSelect2 += " group by b.f05,b.f11,s.f02,s.f03,s.f04 ,s.f17   ) Rec  ,r17520 b where  Rec.Doc=b.f05  "
            strSelect2 += " order by Rec.DOC "
            '--------------------------------------
            'strSelect += " Union"
            '--------------------------------------
            '-------------------------------ขายสด--------------------------
            strSelect3 += " select Rec.Area,Rec.BranchCode,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f64 'Sales',"
            strSelect3 += " case b.f63 when 'A' then 'ยกเลิก' else '' end 'Status',"
            strSelect3 += " Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR,Substring(b.f07,1,2)+':'+Substring(b.f07,3,2) AS 'TimeDoc'  from "
            strSelect3 += " (select s.f03 'Area',s.f02 'BranchCode',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else(sum(case r.f07 when'CS' then r.f09 else 0.00 end))end) 'CS',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else (sum(case r.f07 when'CR' then r.f09 else 0.00 end))end) 'CR',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) end)'CQ',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) end)'TR',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'WH' then r.f09 else 0.00 end)) end) 'WH',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'AO' then r.f09 else 0.00 end))end) 'AO', "
            strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))end)'A_TYPE',"
            'strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case when  r.f07 <>'FR' then r.f09  else 0.00 end)) end)'A_TYPE',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'AD' then r.f09 else 0.00 end))end) 'AD',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end))end) 'AI',"
            strSelect3 += " (case b.f63 when 'A' then '0.00' else( sum(case r.f07 when'FR' then r.f09 else 0.00 end))end) 'FR'"
            strSelect3 += " from R16060 r with(nolock) , r11060 b with(nolock) ,m00030 s with(nolock) , m02020 a with(nolock) "
            strSelect3 += " where(s.f12 = a.f02 And s.f02 = b.f02 And b.f05 = r.f05)"
            '-----------Company----------
            'If cboCompany.SelectedIndex <> 0 Then
            strSelect3 += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' " '-- Company select f01,f02 from m00010
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "| [21010] รายงาน X-Read |"
            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect3 += "  and s.f03>='" + cboSArea.SelectedValue.ToString + "'"
                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "
            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect3 += "  and s.f03<='" + cboEArea.SelectedValue.ToString + "'"
                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "
            End If
            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect3 += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "  '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect3 += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020
                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If
            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect3 += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect3 += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If
            vHeader += "|"
            '----------- Date -----------

            If SDate.Text.Length <> 0 Then
                strSelect3 += "  and b.f06 >='" + SDate.Text + "' "
                vHeader += " ตั้งแต่ วันที่ " + SDate.Text
            End If

            If EDate.Text.Length <> 0 Then
                strSelect3 += "  and b.f06 <='" + EDate.Text + "'"
                vHeader += " ถึงวันที่ " + EDate.Text
            End If
            vHeader += "|"
            strSelect3 += " group by b.f05,b.f63,s.f02,s.f03,s.f04 ,s.f17   ) Rec  ,r11060 b where  Rec.Doc=b.f05   "
            strSelect3 += " order by Rec.DOC "
            '-----------------------------------
            'strSelect += " Union"
            '-------------------------------------
            '----------------ลดหนี้ขายสด  ลดหนี้ OTC------------------------
            strSelect4 += " select  Rec.Area,Rec.BranchCode,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC,b.f36 'Sales', ''as  'Status',"
            strSelect4 += " Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR,Substring(b.f07,1,2)+':'+Substring(b.f07,3,2) AS 'TimeDoc'  from "
            strSelect4 += " (select s.f03 'Area',s.f02 'BranchCode',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'CS' then (r.f09*-1) else 0.00 end))end) 'CS',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'CR' then  (r.f09*-1) else 0.00 end))end) 'CR',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'CQ' then (r.f09*-1) else 0.00 end))end)'CQ',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'TR' then  (r.f09*-1) else 0.00 end))end)'TR',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'WH' then  (r.f09*-1) else 0.00 end))end)  'WH',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'AO' then  (r.f09*-1) else 0.00 end))end) 'AO', "
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else (r.f09*-1) end) else 0.00 end))end)'A_TYPE',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'AD' then (r.f09*-1) else 0.00 end))end) 'AD',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'AI' then  r.f09 else 0.00 end))end) 'AI',"
            strSelect4 += " (case b.f35 when 'A' then '0.00' else(sum(case r.f07 when'FR' then (r.f09*-1) else 0.00 end))end) 'FR'"
            strSelect4 += " from R16060 r with(nolock) , r11090 b with(nolock) ,m00030 s with(nolock) , m02020 a with(nolock) "
            strSelect4 += " where(s.f12 = a.f02 And s.f02 = b.f02 And b.f05 = r.f05)"
            '-----------Company----------
            'If cboCompany.SelectedIndex <> 0 Then
            strSelect4 += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' " '-- Company select f01,f02 from m00010
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "| [21010] รายงาน X-Read |"
            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect4 += "  and s.f03>='" + cboSArea.SelectedValue.ToString + "'"
                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "
            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect4 += "  and s.f03<='" + cboEArea.SelectedValue.ToString + "'"
                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "
            End If
            vHeader += "|"
            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect4 += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "  '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect4 += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020
                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If
            vHeader += "|"
            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect4 += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect4 += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If
            vHeader += "|"
            '----------- Date -----------

            If SDate.Text.Length <> 0 Then
                strSelect4 += "  and b.f06 >='" + SDate.Text + "' "
                vHeader += " ตั้งแต่ วันที่ " + SDate.Text
            End If

            If EDate.Text.Length <> 0 Then
                strSelect4 += "  and b.f06 <='" + EDate.Text + "'"
                vHeader += " ถึงวันที่ " + EDate.Text
            End If
            strSelect4 += " group by b.f05,b.f35,s.f02,s.f03,s.f04 ,s.f17   ) Rec  ,r11090 b where  Rec.Doc=b.f05 "
            strSelect4 += " order by DateDoc,Rec.DOC,Sales"

            vHeader += "|"
            If rReceipt.Checked = True Then
                vHeader += "แสดงแฉพาะเอกสารใบเสร็จรับเงิน"
            ElseIf rReturnReceipt.Checked = True Then
                vHeader += "แสดงแฉพาะเอกสารใบลดหนี้"
            ElseIf rCancleReceipt.Checked = True Then
                vHeader += "แสดงแฉพาะเอกสารยกเลิก"
            ElseIf rAllReceipt.Checked = True Then
                vHeader += "แสดงเอกสารทุกประเภท"
            Else
                vHeader += " "
            End If
            'response.Write(strSelect)
            Dim rDate As DateTime = Now()

            Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            'Log.LogReport("21010", "รายงาน X-Read", "Area F:" + cboSArea.SelectedValue.ToString + " T:" + cboEArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Date F:" + SDate.Text + " T:" + EDate.Text, vIPAddress, Session("Uemail"), rDate, "2001-01-01")
            Dim Parameter As String = "Area F:" + cboSArea.SelectedValue.ToString + " T:" + cboEArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Date F:" + SDate.Text + " T:" + EDate.Text
            Session("Parameter") = Parameter
            'Session("rDate") = Format(rDate, "yyyy-MM-dd HH:mm:ss.000")
            Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
            'C.ExecuteNonQuery("Insert into ReportLog(ReportNumber,ReportName,rDate,Parameter,IPAddress,Login) values('21010','รายงาน X-Read',getdate(),'Area F:" + cboSArea.SelectedValue.ToString + " T:" + cboEArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Date F:" + SDate.Text + " T:" + EDate.Text + "','" + vIPAddress + "','" + Session("Uemail") + "')")


            Dim DT As New DataTable
            Dim objDA As New SqlDataAdapter(strSelect, C.GetConnectionStringRepweb)
            'Try
            '    objDA.SelectCommand.CommandTimeout = 600
            '    objDA.Fill(DT)
            'Catch ex As Exception
            '    Err.Raise(Err.Number, , ex.Message)
            'End Try
            Dim objDA1 As New SqlDataAdapter(strSelect1, C.GetConnectionStringRepweb)
            'Try
            '    objDA1.SelectCommand.CommandTimeout = 600
            '    objDA1.Fill(DT)
            'Catch ex As Exception
            '    Err.Raise(Err.Number, , ex.Message)
            'End Try
            Dim objDA2 As New SqlDataAdapter(strSelect2, C.GetConnectionStringRepweb)
            'Try
            '    objDA2.SelectCommand.CommandTimeout = 600
            '    objDA2.Fill(DT)
            'Catch ex As Exception
            '    Err.Raise(Err.Number, , ex.Message)
            'End Try
            Dim objDA3 As New SqlDataAdapter(strSelect3, C.GetConnectionStringRepweb)
            'Try
            '    objDA3.SelectCommand.CommandTimeout = 600
            '    objDA3.Fill(DT)
            'Catch ex As Exception
            '    Err.Raise(Err.Number, , ex.Message)
            'End Try
            Dim objDA4 As New SqlDataAdapter(strSelect4, C.GetConnectionStringRepweb)
            'Try
            '    objDA4.SelectCommand.CommandTimeout = 600
            '    objDA4.Fill(DT)
            'Catch ex As Exception
            '    Err.Raise(Err.Number, , ex.Message)
            'End Try

            '--------------check redio button and prepare data to print on next page(data page)

            If rReceipt.Checked = True Then 'show receipt data
                Try
                    objDA.SelectCommand.CommandTimeout = 600
                    objDA.Fill(DT)
                    objDA2.SelectCommand.CommandTimeout = 600
                    objDA2.Fill(DT)
                    objDA3.SelectCommand.CommandTimeout = 600
                    objDA3.Fill(DT)
                Catch ex As Exception
                    Err.Raise(Err.Number, , ex.Message)
                End Try
            ElseIf rReturnReceipt.Checked = True Then 'show return receipt data
                Try
                    objDA1.SelectCommand.CommandTimeout = 600
                    objDA1.Fill(DT)
                    objDA4.SelectCommand.CommandTimeout = 600
                    objDA4.Fill(DT)
                Catch ex As Exception
                    Err.Raise(Err.Number, , ex.Message)
                End Try
            Else 'show all receipt data
                Try
                    objDA.SelectCommand.CommandTimeout = 600
                    objDA.Fill(DT)
                    objDA1.SelectCommand.CommandTimeout = 600
                    objDA1.Fill(DT)
                    objDA2.SelectCommand.CommandTimeout = 600
                    objDA2.Fill(DT)
                    objDA3.SelectCommand.CommandTimeout = 600
                    objDA3.Fill(DT)
                    objDA4.SelectCommand.CommandTimeout = 600
                    objDA4.Fill(DT)
                Catch ex As Exception
                    Err.Raise(Err.Number, , ex.Message)
                End Try
            End If

            Session("StringQuery") = DT 'strSelect.ToString
            Session("Header") = vHeader
            Dim url as String = "ShowReportXreadNew.aspx?"
            url += "com=" + cboCompany.SelectedValue + ""
            url += "&sarea=" + cboSArea.SelectedValue + ""
            url += "&earea=" + cboEArea.SelectedValue + ""
            url += "&sprov=" + cboSProvince.SelectedValue + ""
            url += "&eprov=" + cboEProvince.SelectedValue + ""
            url += "&sbranch=" + cboSBranch.SelectedValue + ""
            url += "&ebranch=" + cboEBranch.SelectedValue + ""
            url += "&sdate=" + SDate.Text + ""
            url += "&edate=" + EDate.Text + ""
            ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('"+url+"','_blank','height=300,width=620,left=130,top=150,resizable=1,scrollbars=1');", True)
        Catch ex As Exception

        End Try


    End Sub


    Private Sub ShowRptCancel() ' แสดงเฉพาะเอกสารยกเลิก
        Dim strSelect As String = ""
        'Dim strSelect1 As String = ""
        Dim strSelect2 As String = ""
        Dim strSelect3 As String = ""
        'Dim strSelect4 As String = ""
        Dim strGroup As String = ""
        Dim strOrder As String = ""
        Dim vHeader As String = ""
        Dim vComName As String = cboCompany.SelectedItem.ToString
        Dim vSArea As String = cboSArea.SelectedItem.ToString
        Dim vEArea As String = cboEArea.SelectedItem.ToString
        Dim vSProvince As String = cboSProvince.SelectedItem.ToString
        Dim vEProvince As String = cboEProvince.SelectedItem.ToString
        Dim vSBranch As String = cboSBranch.SelectedItem.ToString
        Dim vEBranch As String = cboEBranch.SelectedItem.ToString
        Dim sb As New StringBuilder
        Try
            '-------------------------- Bill ---------------------------------
            strSelect = " select  Rec.Area,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f10 'Sales', case b.f11 when 'A' then 'ยกเลิก' else '' end 'Status'," + vbCr
            strSelect += " Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR," + vbCr
            strSelect += " Substring(b.f07,1,2)+':'+Substring(b.f07,3,2) AS 'TimeDoc'" + vbCr
            strSelect += " from" + vbCr
            strSelect += " (select s.f03 'Area',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc'," + vbCr
            strSelect += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS'," + vbCr
            strSelect += " (sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR'," + vbCr
            strSelect += " (sum(case r.f07 when'CQ' then r.f09 else 0.00 end))'CQ'," + vbCr
            strSelect += " (sum(case r.f07 when'TR' then r.f09 else 0.00 end))'TR'," + vbCr
            strSelect += " (sum(case r.f07 when'WH' then r.f09 else 0.00 end)) 'WH'," + vbCr
            strSelect += " (sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', " + vbCr
            strSelect += " (sum(case when  r.f07 <>'FR' then r.f09 else 0.00 end))'A_TYPE'," + vbCr
            strSelect += " (sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD'," + vbCr
            strSelect += " (sum(case r.f07 when'AI' then r.f09 else 0.00 end)) 'AI'," + vbCr
            strSelect += " (sum(case r.f07 when'FR' then r.f09 else 0.00 end)) 'FR'" + vbCr
            strSelect += " from R16060 r with(nolock) , r17510 b with(nolock) ,m00030 s with(nolock) , m02020 a with(nolock) " + vbCr
            strSelect += " where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and b.f04 in ('DRCPV','DRCPN') and b.f11='A' " + vbCr
            '-----------Company----------
            'If cboCompany.SelectedIndex <> 0 Then
            strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' " '-- Company select f01,f02 from m00010
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "| [21010] รายงาน X-Read |"
            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect += "  and s.f03>='" + cboSArea.SelectedValue.ToString + "'"
                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "
            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect += "  and s.f03<='" + cboEArea.SelectedValue.ToString + "'"
                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "
            End If
            vHeader += "|"
            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "  '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020
                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If
            vHeader += "|"
            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If
            vHeader += "|"
            '----------- Date -----------

            If SDate.Text.Length <> 0 Then
                strSelect += "  and b.f06 >='" + SDate.Text + "' "
                vHeader += " ตั้งแต่ วันที่ " + SDate.Text
            End If

            If EDate.Text.Length <> 0 Then
                strSelect += "  and b.f06 <='" + EDate.Text + "'"
                vHeader += " ถึงวันที่ " + EDate.Text
            End If
            vHeader += "|"
            strSelect += " group by b.f05,b.f11,s.f03,s.f04,s.f17   ) Rec  ,r17510 b where  Rec.Doc=b.f05"
            strSelect += " order by Rec.DOC "
            '-------------------------------------------------
            'strSelect += " Union"
            '-------------------------------------------------
            '----------------------------OTC----------------------------
            strSelect2 += " select  Rec.Area,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f10 'Sales', case b.f11 when 'A' then 'ยกเลิก' else '' end 'Status',Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR,Substring(b.f07,1,2)+':'+Substring(b.f07,3,2) AS 'TimeDoc'  from " + vbCr
            strSelect2 += " (select s.f03 'Area',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc'," + vbCr
            strSelect2 += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS'," + vbCr
            strSelect2 += " (sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR'," + vbCr
            strSelect2 += " (sum(case r.f07 when'CQ' then r.f09 else 0.00 end))'CQ'," + vbCr
            strSelect2 += " (sum(case r.f07 when'TR' then r.f09 else 0.00 end))'TR'," + vbCr
            strSelect2 += " (sum(case r.f07 when'WH' then r.f09 else 0.00 end)) 'WH'," + vbCr
            strSelect2 += " (sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', " + vbCr
            strSelect2 += " (sum(case when  r.f07 <>'FR' then r.f09  else 0.00 end))'A_TYPE'," + vbCr
            strSelect2 += " (sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD'," + vbCr
            strSelect2 += " (sum(case r.f07 when'AI' then r.f09 else 0.00 end)) 'AI'," + vbCr
            strSelect2 += " (sum(case r.f07 when'FR' then r.f09 else 0.00 end)) 'FR'" + vbCr
            strSelect2 += " from R16060 r with(nolock) , r17520 b with(nolock) ,m00030 s with(nolock) , m02020 a with(nolock) " + vbCr
            strSelect2 += " where(s.f12 = a.f02 And s.f02 = b.f02 And b.f05 = r.f05) and b.f11='A'" + vbCr
            '-----------Company----------
            'If cboCompany.SelectedIndex <> 0 Then
            strSelect2 += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' " '-- Company select f01,f02 from m00010
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "| [21010] รายงาน X-Read |"
            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect2 += "  and s.f03>='" + cboSArea.SelectedValue.ToString + "'"
                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "
            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect2 += "  and s.f03<='" + cboEArea.SelectedValue.ToString + "'"
                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "
            End If
            vHeader += "|"
            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect2 += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "  '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect2 += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020
                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If
            vHeader += "|"
            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect2 += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect2 += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If
            vHeader += "|"
            '----------- Date -----------

            If SDate.Text.Length <> 0 Then
                strSelect2 += "  and b.f06 >='" + SDate.Text + "' "
                vHeader += " ตั้งแต่ วันที่ " + SDate.Text
            End If

            If EDate.Text.Length <> 0 Then
                strSelect2 += "  and b.f06 <='" + EDate.Text + "'"
                vHeader += " ถึงวันที่ " + EDate.Text
            End If
            vHeader += "|"
            strSelect2 += " group by b.f05,b.f11 ,s.f03,s.f04 ,s.f17   ) Rec  ,r17520 b where  Rec.Doc=b.f05  "
            strSelect2 += " order by Rec.DOC "
            '--------------------------------------
            'strSelect += " Union"
            '--------------------------------------
            '-------------------------------ขายสด--------------------------
            strSelect3 += " select Rec.Area,Rec.BranchName,convert(varchar(10),b.f06,103) 'DateDoc',Rec.DOC ,b.f64 'Sales'," + vbCr
            strSelect3 += " case b.f63 when 'A' then 'ยกเลิก' else '' end 'Status'," + vbCr
            strSelect3 += " Rec.CS,Rec.CR,Rec.CQ,Rec.TR,Rec.WH,Rec.AO,Rec.A_TYPE,Rec.AD,Rec.AI,Rec.FR,Substring(b.f07,1,2)+':'+Substring(b.f07,3,2) AS 'TimeDoc'  from " + vbCr
            strSelect3 += " (select s.f03 'Area',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,b.f05 'Doc'," + vbCr
            strSelect3 += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS'," + vbCr
            strSelect3 += " (sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR'," + vbCr
            strSelect3 += " (sum(case r.f07 when'CQ' then r.f09 else 0.00 end))'CQ'," + vbCr
            strSelect3 += " (sum(case r.f07 when'TR' then r.f09 else 0.00 end))'TR'," + vbCr
            strSelect3 += " (sum(case r.f07 when'WH' then r.f09 else 0.00 end)) 'WH'," + vbCr
            strSelect3 += " (sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', " + vbCr
            strSelect3 += " (sum(case when  r.f07 <>'FR' then r.f09  else 0.00 end))'A_TYPE'," + vbCr
            strSelect3 += " (sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD'," + vbCr
            strSelect3 += " (sum(case r.f07 when'AI' then r.f09 else 0.00 end)) 'AI'," + vbCr
            strSelect3 += " (sum(case r.f07 when'FR' then r.f09 else 0.00 end)) 'FR'" + vbCr
            strSelect3 += " from R16060 r with(nolock) , r11060 b with(nolock) ,m00030 s with(nolock) , m02020 a with(nolock) " + vbCr
            strSelect3 += " where(s.f12 = a.f02 And s.f02 = b.f02 And b.f05 = r.f05) and b.f63='A' " + vbCr
            '-----------Company----------
            'If cboCompany.SelectedIndex <> 0 Then
            strSelect3 += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' " '-- Company select f01,f02 from m00010
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "| [21010] รายงาน X-Read |"
            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect3 += "  and s.f03>='" + cboSArea.SelectedValue.ToString + "'"
                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "
            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect3 += "  and s.f03<='" + cboEArea.SelectedValue.ToString + "'"
                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "
            End If
            vHeader += "|"
            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect3 += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "  '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect3 += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020
                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If
            vHeader += "|"
            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect3 += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect3 += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030
                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If
            vHeader += "|"
            '----------- Date -----------

            If SDate.Text.Length <> 0 Then
                strSelect3 += "  and b.f06 >='" + SDate.Text + "' "
                vHeader += " ตั้งแต่ วันที่ " + SDate.Text
            End If

            If EDate.Text.Length <> 0 Then
                strSelect3 += "  and b.f06 <='" + EDate.Text + "'"
                vHeader += " ถึงวันที่ " + EDate.Text
            End If

            strSelect3 += " group by b.f05,b.f63 ,s.f03,s.f04 ,s.f17   ) Rec  ,r11060 b where  Rec.Doc=b.f05   "
            strSelect3 += " order by Rec.DOC "

            vHeader += "|"
            '----------- DocType ---------------

            If rCancleReceipt.Checked = True Then
                vHeader += "แสดงแฉพาะเอกสารยกเลิก"
            Else
                vHeader += " "
            End If

            Dim rDate As DateTime = Now()

            Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            'Log.LogReport("21010", "รายงาน X-Read", "Area F:" + cboSArea.SelectedValue.ToString + " T:" + cboEArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Date F:" + SDate.Text + " T:" + EDate.Text, vIPAddress, Session("Uemail"), rDate, "2001-01-01")
            Dim Parameter As String = "Area F:" + cboSArea.SelectedValue.ToString + " T:" + cboEArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Date F:" + SDate.Text + " T:" + EDate.Text
            Session("Parameter") = Parameter
            'Session("rDate") = Format(rDate, "yyyy-MM-dd HH:mm:ss.000")
            Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
            'C.ExecuteNonQuery("Insert into ReportLog(ReportNumber,ReportName,rDate,Parameter,IPAddress,Login) values('21010','รายงาน X-Read',getdate(),'Area F:" + cboSArea.SelectedValue.ToString + " T:" + cboEArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Date F:" + SDate.Text + " T:" + EDate.Text + "','" + vIPAddress + "','" + Session("Uemail") + "')")


            Dim DT As New DataTable
            Dim objDA As New SqlDataAdapter(strSelect, C.GetConnectionStringRepweb)

            Dim objDA2 As New SqlDataAdapter(strSelect2, C.GetConnectionStringRepweb)

            Dim objDA3 As New SqlDataAdapter(strSelect3, C.GetConnectionStringRepweb)


            Try
                objDA.SelectCommand.CommandTimeout = 600
                objDA.Fill(DT)
                objDA2.SelectCommand.CommandTimeout = 600
                objDA2.Fill(DT)
                objDA3.SelectCommand.CommandTimeout = 600
                objDA3.Fill(DT)
            Catch ex As Exception
                Err.Raise(Err.Number, , ex.Message)
            End Try

            Session("StringQuery") = DT 'strSelect.ToString
            Session("Header") = vHeader
            Dim url as String = "ShowReportXreadNew.aspx?"
            url += "com=" + cboCompany.SelectedValue + ""
            url += "&sarea=" + cboSArea.SelectedValue + ""
            url += "&earea=" + cboEArea.SelectedValue + ""
            url += "&sprov=" + cboSProvince.SelectedValue + ""
            url += "&eprov=" + cboEProvince.SelectedValue + ""
            url += "&sbranch=" + cboSBranch.SelectedValue + ""
            url += "&ebranch=" + cboEBranch.SelectedValue + ""
            url += "&sdate=" + SDate.Text + ""
            url += "&edate=" + EDate.Text + ""
            ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('"+ url +"','_blank','height=300,width=620,left=130,top=150,resizable=1,scrollbars=1');", True)
        Catch ex As Exception

        End Try


    End Sub

    Private Sub ReadCookies()
        If Request.Cookies("POS_X-Read")("Check") = "true" Then
            cboCompany.SelectedValue = Request.Cookies("POS_X-Read")("Company")
            cboSArea.SelectedValue = Request.Cookies("POS_X-Read")("AreaFrom")
            cboEArea.SelectedValue = Request.Cookies("POS_X-Read")("AreaTo")
            LoadProvince("Where")
            LoadBranch("Where")
            cboSProvince.SelectedValue = Request.Cookies("POS_X-Read")("ProvinceFrom")
            cboEProvince.SelectedValue = Request.Cookies("POS_X-Read")("ProvinceTo")
            LoadBranch("Where")
            cboSBranch.SelectedValue = Request.Cookies("POS_X-Read")("BranchFrom")
            cboEBranch.SelectedValue = Request.Cookies("POS_X-Read")("BranchTo")
            CheckCookies.Checked = True
        ElseIf Request.Cookies("POS_X-Read")("Check") = "false" Then
            CheckCookies.Checked = False
        End If
    End Sub

    Private Sub WriteCookies(ByVal check As String)
        If check = "true" Then
            Response.Cookies("POS_X-Read").Expires = DateTime.Now.AddDays(15)
            'Response.Cookies("POS_X-Read")("Login") = Session("Uemail").ToString
            Response.Cookies("POS_X-Read")("Company") = cboCompany.SelectedValue
            Response.Cookies("POS_X-Read")("AreaFrom") = cboSArea.SelectedValue
            Response.Cookies("POS_X-Read")("AreaTo") = cboEArea.SelectedValue
            Response.Cookies("POS_X-Read")("ProvinceFrom") = cboSProvince.SelectedValue
            Response.Cookies("POS_X-Read")("ProvinceTo") = cboEProvince.SelectedValue
            Response.Cookies("POS_X-Read")("BranchFrom") = cboSBranch.SelectedValue
            Response.Cookies("POS_X-Read")("BranchTo") = cboEBranch.SelectedValue
            Response.Cookies("POS_X-Read")("DateTime") = Now()
            Response.Cookies("POS_X-Read")("Check") = "true"
        Else
            Response.Cookies("POS_X-Read").Expires = DateTime.MaxValue
            'Response.Cookies("POS_X-Read")("Login") = Session("Uemail").ToString
            Response.Cookies("POS_X-Read")("Company") = ""
            Response.Cookies("POS_X-Read")("AreaFrom") = ""
            Response.Cookies("POS_X-Read")("AreaTo") = ""
            Response.Cookies("POS_X-Read")("ProvinceFrom") = ""
            Response.Cookies("POS_X-Read")("ProvinceTo") = ""
            Response.Cookies("POS_X-Read")("BranchFrom") = ""
            Response.Cookies("POS_X-Read")("BranchTo") = ""
            Response.Cookies("POS_X-Read")("DateTime") = Now()
            If check = "e" Then
                Response.Cookies("POS_X-Read")("Check") = "true"
            Else
                Response.Cookies("POS_X-Read")("Check") = "false"
            End If

        End If
    End Sub
End Class
