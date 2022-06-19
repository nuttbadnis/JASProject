Imports System.Data
Imports System.Web
Imports System.Web.UI
Imports System.Threading
Imports System.Net.IPAddress
Partial Class X_ReadSum
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim CF As New Cls_RequestFlow
    Dim vSql As String
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Add Class Radio
            GroupShop.InputAttributes.Add("class", "form-check-input")
            GroupDate.InputAttributes.Add("class", "form-check-input")
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
    Public Sub SetDropDownList2(ByRef ddl As DropDownList, ByRef ddl2 As DropDownList, ByVal CmdText As String, ByVal TextField As String, ByVal ValueField As String, Optional ByVal Row0 As String = Nothing)
        Try
            Dim DT As DataTable = C.GetDataTableRepweb(CmdText)
            If Not Row0 Is Nothing Then
                Dim DR As DataRow = DT.NewRow
                DR(TextField) = Row0
                DT.Rows.InsertAt(DR, 0)
            End If
            ddl.DataSource = DT
            ddl.DataTextField = TextField
            ddl.DataValueField = ValueField
            ddl2.DataSource = DT
            ddl2.DataTextField = TextField
            ddl2.DataValueField = ValueField
            ddl.DataBind()
            ddl2.DataBind()
        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        End Try
    End Sub

    Public Function CDateText(ByVal TDate As String) As String
        Dim TDate2 As String
        Try
            TDate2 = CStr(TDate)
            If TDate = "  /  /    " Or TDate = "__/__/____" Or Trim(TDate) = "" Then
                CDateText = ""

            Else
                CDateText = Mid(TDate, 7, 4) + "/" + Mid(TDate, 4, 2) + "/" + Mid(TDate, 1, 2)
            End If
        Catch ex As Exception
            CDateText = TDate
        End Try
    End Function

    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        C.SetDropDownList(cboCompany, vSql, "ComName", "ComCode")
    End Sub

    Private Sub LoadArea()
        vSql = "select distinct o.f03 as AreaCode,mo.f02 + ' :: ' + mo.f03 as AreaName from m00860 ma, m00030 o, m02300 mo where o.f03=mo.f02 and ma.f03 = o.f02 and ma.f02='" + Session("Uemail") + "'"
        C.SetDropDownList(cboArea, vSql, "AreaName", "AreaCode", "ดูข้อมูลทั้งหมด")

        If cboArea.Items.Count = 1 Then
            vSql = "select distinct o.f03 as AreaCode,mo.f02 + ' :: ' + mo.f03 as AreaName from m00860 ma, m00030 o, m02300 mo where o.f03=mo.f02 and ma.f03 = 'all' and ma.f02='" + Session("Uemail") + "'"
            C.SetDropDownList(cboArea, vSql, "AreaName", "AreaCode", "ดูข้อมูลทั้งหมด")
        End If
    End Sub
    Private Sub LoadProvince(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            If cboArea.SelectedIndex <> 0 Then
                cboSProvince.Enabled = True
                cboEProvince.Enabled = True
                strW = "  o.f03 = '" & cboArea.SelectedValue.ToString & "'"
            End If
            
            Wherecause = strW

        End If

        vSql = "select distinct CAST(m02.f02 as varchar) as ProvinceCode, m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12 and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + ""
        C.SetDropDownList(cboSProvince, vSql, "ProvinceName", "ProvinceCode", "ดูข้อมูลทั้งหมด", cboEProvince)
        If cboSProvince.Items.Count = 1 Then
            vSql = "select distinct CAST(m02.f02 as varchar) as ProvinceCode,m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and m02.f02 = o.f12 and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + ""
            C.SetDropDownList(cboSProvince, vSql, "ProvinceName", "ProvinceCode", "ดูข้อมูลทั้งหมด", cboEProvince)
        End If
    End Sub
    Private Sub LoadBranch(ByVal Wherecause As String)
        Try
            Dim strSql As String
            If Wherecause <> "" Then
                Dim strW As String = ""
                If cboArea.SelectedIndex <> 0 Then
                    strW = "o.f03 = '" & cboArea.SelectedValue.ToString & "'"
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

            strSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
            SetDropDownList2(cboSBranch, cboEBranch, strSql, "BranchName", "BranchCode", "")
            If cboSBranch.Items.Count = 1 Then
                strSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
                SetDropDownList2(cboSBranch, cboEBranch, strSql, "BranchName", "BranchCode", "")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub cboArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboArea.SelectedIndexChanged
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

        ShowRpt()
    End Sub
    Private Sub ShowRpt()
        Dim strSelect As String = ""
        Dim strGroup As String = ""
        Dim strOrder As String = ""
        Dim vHeader As String = ""
        Dim vComName As String = cboCompany.SelectedItem.ToString
        Dim vArea As String = cboArea.SelectedItem.ToString

        Dim vSProvince As String = cboSProvince.SelectedItem.ToString
        Dim vEProvince As String = cboEProvince.SelectedItem.ToString
        Dim vSBranch As String = cboSBranch.SelectedItem.ToString
        Dim vEBranch As String = cboEBranch.SelectedItem.ToString
        Dim sb As New StringBuilder
        Try
            If GroupShop.Checked = true Then
                strSelect = " select bb.area,bb.codeshop,bb.BranchName,sum(bb.Cdoc) Cdoc,sum(bb.cs) CS,sum(bb.cr) CR,sum(bb.cq) "
                strSelect += " CQ,sum(bb.tr) TR,sum(bb.wh) WH,sum(bb.ao) AO,sum(bb.a_type) A_TYPE,sum(bb.ad) AD,"
                strSelect += " sum(bb.ai) AI,sum(bb.fr) FR ,(sum(bb.BillPayment) - sum(bb.DCNPV)) 'BillPayment'"
                strSelect += " ,(sum(bb.OTC) - sum(bb.DCNOV)) 'OTC'"
                strSelect += " ,(sum(bb.SRCPV)- sum(bb.SCNRV)) 'SRCPV' from ("
                '%%%%%%%%%%%%%%%%%%%%%%%%%% Bill %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '######################################################################
                strSelect += " select c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then r.f09 else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then r.f09 else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09) else 0.00 end)) 'FR',"
                strSelect += " (sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'BillPayment',0 'DCNPV',0 'OTC',0 'SRCPV',0 'SCNRV',0 'DCNOV' "
                strSelect += " from R16060 r with(nolock) , r17510 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where(s.f12 = a.f02 And C.f02 = s.f03 And s.f02 = b.f02)"
                strSelect += " and b.f05 =r.f05 and b.f04 in ('DRCPV','DRCPN') and b.f11 ='1'"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
                End If

                '----------- Province -----------

                If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                    strSelect += "  and a.f02>='" + cboSProvince.SelectedValue.ToString + "' "

                    vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                    vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                    vHeader += "จากจังหวัด" + vSProvince + "  "

                End If
                If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                    strSelect += "  and a.f02<='" + cboEProvince.SelectedValue.ToString + "' "
                    vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                    vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                    vHeader += "ถึงจังหวัด" + vEProvince + "  "

                End If
                '----------- Branch -----------

                If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                    strSelect += "  and b.f02>='" + cboSBranch.SelectedValue.ToString + "'"
                    vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                    vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                    vHeader += " จากสาขา " + vSBranch + "  "

                End If
                If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                    strSelect += "  and b.f02<='" + cboEBranch.SelectedValue.ToString + "'"
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
                strSelect += " group by b.f02,c.f03,b.f06,s.f04,s.f17 "
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%%%%%% ลดหนี้ Bill %%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " select c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName, convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then (r.f09*-1)  else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then  (r.f09*-1) else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then (r.f09*-1)  else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then  (r.f09*-1)  else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then  (r.f09*-1)  else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then  (r.f09*-1) else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then  (r.f09*-1)  else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then  (case when r.f07 in ('AI','AD','FR') then 0.00 else (r.f09*-1) end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then  (r.f09*-1)  else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then r.f09  else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then r.f09 else 0.00 end)) 'FR',"
                strSelect += " 0 'BillPayment',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'DCNPV',0 'OTC',0 'SRCPV',0 'SCNRV',0 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r17510 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where  s.f12=a.f02 and c.f02 =s.f03 and s.f02 =b.f02 and b.f05 =r.f05 and b.f04 in('DCNPV','DCNPN') and b.f11='1' "
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06 <='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                strSelect += " group by b.f02,c.f03,b.f06,s.f04,s.f17 "
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '########################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%%%%%%%% OTC %%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '########################################################
                strSelect += " select c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName, convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then r.f09 else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then r.f09 else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09) else 0.00 end)) 'FR',"
                strSelect += " 0 'BillPayment',0 'DCNPV',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'OTC',0 'SRCPV',0 'SCNRV',0 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r17520 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where  s.f12=a.f02 and c.f02 =s.f03 and s.f02 =b.f02 and b.f05 =r.f05  and b.f11 ='1'"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                strSelect += " group by b.f02,c.f03,b.f06 ,s.f04,s.f17"
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '########################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%%%%%%% ขายสด %%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '########################################################
                strSelect += " select  c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName, convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then r.f09 else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then r.f09 else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09) else 0.00 end)) 'FR',"
                strSelect += "  0 'BillPayment',0 'DCNPV',0 'OTC',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'SRCPV',0 'SCNRV',0 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r11060 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where  s.f12=a.f02 and c.f02 =s.f03 and s.f02 =b.f02 and b.f05 =r.f05  and b.f63 ='1'"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                strSelect += " group by  b.f02,c.f03,b.f06,s.f04,s.f17 "
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%% ลดหนี้ OTC %%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " select  c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName, convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then (r.f09*-1) else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then (r.f09*-1) else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then (r.f09*-1) else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then (r.f09*-1) else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then (r.f09*-1) else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then (r.f09*-1) else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then (r.f09*-1) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else (r.f09*-1) end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then (r.f09*-1) else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then r.f09 else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09*-1) else 0.00 end)) 'FR',"
                strSelect += "  0 'BillPayment',0 'DCNPV',0 'OTC',0 'SRCPV',0 'SCNRV',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'DCNOV' "
                strSelect += " from R16060 r with(nolock) , r11090 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where(s.f12 = a.f02 And C.f02 = s.f03 And s.f02 = b.f02 And b.f05 = r.f05)  and b.f35 = '1'"
                strSelect += " and b.f04 in ('DCNOV','DCNON')"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"

                strSelect += " group by  b.f02,c.f03,b.f06,s.f04,s.f17"
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%% ลดหนี้ขายสด %%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " select  c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName, convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then (r.f09*-1) else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then (r.f09*-1) else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then (r.f09*-1) else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then (r.f09*-1) else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then (r.f09*-1) else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then (r.f09*-1) else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then (r.f09*-1) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else (r.f09*-1) end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then (r.f09*-1) else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then r.f09 else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09*-1) else 0.00 end)) 'FR',"
                strSelect += "  0 'BillPayment',0 'DCNPV',0 'OTC',0 'SRCPV',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'SCNRV',0 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r11090 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where(s.f12 = a.f02 And C.f02 = s.f03 And s.f02 = b.f02 And b.f05 = r.f05) and b.f35 = '1'"
                strSelect += " and b.f04 in ('SCNRV','SCNRN','OCNRV','OCNRN')"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"

                strSelect += " group by  b.f02,c.f03,b.f06,s.f04,s.f17"
                strSelect += " ) bb group by bb.area,bb.codeshop,bb.BranchName"
                strSelect += " order by bb.codeshop"
            ElseIf GroupDate.Checked = true Then
                strSelect = " select bb.area,bb.codeshop,bb.BranchName,bb.datedoc,sum(bb.Cdoc) Cdoc,sum(bb.cs) CS,sum(bb.cr) CR,sum(bb.cq) "
                strSelect += " CQ,sum(bb.tr) TR,sum(bb.wh) WH,sum(bb.ao) AO,sum(bb.a_type) A_TYPE,sum(bb.ad) AD,"
                strSelect += " sum(bb.ai) AI,sum(bb.fr) FR , (sum(bb.BillPayment) - sum(bb.DCNPV)) 'BillPayment'"
                strSelect += " ,(sum(bb.OTC) - sum(bb.DCNOV)) 'OTC'"
                strSelect += " ,(sum(bb.SRCPV)- sum(bb.SCNRV)) 'SRCPV' from ("
                '%%%%%%%%%%%%%%%%%%%%%%%%%% Bill %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '######################################################################
                strSelect += " select c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then r.f09 else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then r.f09 else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09) else 0.00 end)) 'FR',"
                strSelect += " (sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'BillPayment',0 'DCNPV',0 'OTC',0 'SRCPV',0 'SCNRV',0 'DCNOV' "
                strSelect += " from R16060 r with(nolock) , r17510 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where(s.f12 = a.f02 And C.f02 = s.f03 And s.f02 = b.f02)"
                strSelect += " and b.f05 =r.f05 and b.f04 in ('DRCPV','DRCPN') and b.f11 ='1'"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                strSelect += " group by b.f02,c.f03,b.f06,s.f04,s.f17 "
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%%%%%% ลดหนี้ Bill %%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " select c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc', (case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then (r.f09*-1)  else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then  (r.f09*-1) else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then (r.f09*-1)  else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then  (r.f09*-1)  else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then  (r.f09*-1)  else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then  (r.f09*-1) else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then  (r.f09*-1)  else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then  (case when r.f07 in ('AI','AD','FR') then 0.00 else (r.f09*-1) end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then  (r.f09*-1)  else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then r.f09  else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then r.f09 else 0.00 end)) 'FR',"
                strSelect += " 0 'BillPayment',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'DCNPV',0 'OTC',0 'SRCPV',0 'SCNRV',0 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r17510 b with(nolock),m00030 s with(nolock), m02020 a with(nolock) ,m02300 c with(nolock) "
                strSelect += " where  s.f12=a.f02 and c.f02 =s.f03 and s.f02 =b.f02 and b.f05 =r.f05 and b.f04 in('DCNPV','DCNPN') and b.f11 = '1' "
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                strSelect += " group by b.f02,c.f03,b.f06,s.f04,s.f17 "
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '########################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%%%%%%%% OTC %%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '########################################################
                strSelect += " select c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName, convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then r.f09 else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then r.f09 else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09) else 0.00 end)) 'FR',"
                strSelect += " 0 'BillPayment',0 'DCNPV',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'OTC',0 'SRCPV',0 'SCNRV',0 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r17520 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where  s.f12=a.f02 and c.f02 =s.f03 and s.f02 =b.f02 and b.f05 =r.f05  and b.f11 ='1'"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                strSelect += " group by b.f02,c.f03,b.f06,s.f04,s.f17 "
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '########################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%%%%%%% ขายสด %%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '########################################################
                strSelect += " select  c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc', (case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName,convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then r.f09 else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then r.f09 else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then r.f09 else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then r.f09 else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then r.f09 else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then r.f09 else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then r.f09 else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else r.f09 end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then r.f09 else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then (r.f09*-1) else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09) else 0.00 end)) 'FR',"
                strSelect += " 0 'BillPayment',0 'DCNPV',0 'OTC',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'SRCPV',0 'SCNRV',0 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r11060 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where  s.f12=a.f02 and c.f02 =s.f03 and s.f02 =b.f02 and b.f05 =r.f05  and b.f63 ='1'"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                strSelect += " group by  b.f02,c.f03,b.f06,s.f04,s.f17 "
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%% ลดหนี้ OTC %%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " select  c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName, convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then (r.f09*-1) else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then (r.f09*-1) else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then (r.f09*-1) else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then (r.f09*-1) else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then (r.f09*-1) else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then (r.f09*-1) else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then (r.f09*-1) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else (r.f09*-1) end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then (r.f09*-1) else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then r.f09 else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09*-1) else 0.00 end)) 'FR',"
                strSelect += " 0 'BillPayment',0 'DCNPV',0 'OTC',0 'SRCPV',0 'SCNRV',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r11090 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where(s.f12 = a.f02 And C.f02 = s.f03 And s.f02 = b.f02 And b.f05 = r.f05) and b.f35 = '1' "
                strSelect += " and b.f04 in ('DCNOV','DCNON')"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"

                strSelect += " group by  b.f02,c.f03,b.f06,s.f04,s.f17"
                '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " Union"
                '%%%%%%%%%%%%%%%%% ลดหนี้ขายสด %%%%%%%%%%%%%%%%%%%%%%%%
                '############################################################
                strSelect += " select  c.f03 'area',b.f02 'codeshop',count(distinct(b.f05)) 'Cdoc',(case s.f04 when 'สำนักงานใหญ่' then s.f17 else s.f04 end ) as BranchName, convert(varchar(10),b.f06,103) 'DateDoc',"
                strSelect += " (sum(case r.f07 when'CS' then (r.f09*-1) else 0.00 end)) 'CS',"
                strSelect += " ( sum(case r.f07 when'CR' then (r.f09*-1) else 0.00 end)) 'CR',"
                strSelect += " ( sum(case r.f07 when'CQ' then (r.f09*-1) else 0.00 end)) 'CQ',"
                strSelect += " ( sum(case r.f07 when'TR' then (r.f09*-1) else 0.00 end)) 'TR',"
                strSelect += " ( sum(case r.f07 when'WH' then (r.f09*-1) else 0.00 end))  'WH',"
                strSelect += " ( sum(case r.f07 when'AO' then (r.f09*-1) else 0.00 end)) 'AO', "
                'strSelect += " ( sum(case when  r.f07 <>'FR' then (r.f09*-1) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case when  r.f07 <>'FR' then (case when r.f07 in ('AI','AD','FR') then 0.00 else (r.f09*-1) end) else 0.00 end))'A_TYPE',"
                strSelect += " ( sum(case r.f07 when'AD' then (r.f09*-1) else 0.00 end)) 'AD',"
                strSelect += " ( sum(case r.f07 when'AI' then r.f09 else 0.00 end)) 'AI',"
                strSelect += " ( sum(case r.f07 when'FR' then (r.f09*-1) else 0.00 end)) 'FR',"
                strSelect += "  0 'BillPayment',0 'DCNPV',0 'OTC',0 'SRCPV',( sum(case when  r.f07 <>'FR' then (case when r.f07 = 'AI' then (r.f09*-1) else r.f09 end) else 0.00 end)) 'SCNRV',0 'DCNOV'"
                strSelect += " from R16060 r with(nolock) , r11090 b with(nolock),m00030 s with(nolock), m02020 a with(nolock),m02300 c with(nolock) "
                strSelect += " where(s.f12 = a.f02 And C.f02 = s.f03 And s.f02 = b.f02 And b.f05 = r.f05) and b.f35 = '1' "
                strSelect += " and b.f04 in ('SCNRV','SCNRN','OCNRV','OCNRN')"
                '---------------Company-------------'
                'If cboCompany.SelectedIndex <> 0 Then
                strSelect += "  and b.f01 = '" + cboCompany.SelectedValue.ToString + "' "
                vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "| [21015] รายงาน Summary X-Read |"
                '----------- Area -----------

                If Not (cboArea.SelectedIndex = 0 Or cboArea.Items.Count = 0) Then
                    strSelect += "  and s.f03 ='" + cboArea.SelectedValue.ToString + "'"
                    vArea = Replace(cboArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(cboArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += " เขต " + vArea + "  "
                Else
                    vHeader += "เขตทั้งหมด"
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
                    strSelect += "  and b.f06>='" + SDate.Text + "' "
                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If

                If EDate.Text.Length <> 0 Then
                    strSelect += "  and b.f06<='" + EDate.Text + "'"
                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"

                strSelect += " group by  b.f02,c.f03,b.f06,s.f04,s.f17"
                strSelect += " ) bb group by bb.area,bb.codeshop,bb.BranchName,bb.datedoc"
                strSelect += " order by bb.codeshop,bb.datedoc"
            End If

            Dim rDate As DateTime = Now()
            'Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            Dim Parameter As String = "Area:" + cboArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Date F:" + SDate.Text + " T:" + EDate.Text
            Session("Parameter") = Parameter
            Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
            'Log.LogReport("21015", "รายงาน Summary X-Read", "Area:" + cboArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Date F:" + SDate.Text + " T:" + EDate.Text, vIPAddress, Session("Uemail"))

            Session("StringQuery") = strSelect.ToString
            Session("Header") = vHeader
            Dim radio_value as String = ""
            If GroupDate.Checked = true then
                radio_value = "D"
            Else
                radio_value = "S"
            End If
            ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('Showrpt_XreadSum.aspx?group=" + radio_value + "&sdate=" + SDate.Text + "&edate=" + EDate.Text + "&com=" + cboCompany.SelectedValue + "&area=" + cboArea.SelectedValue + "&sprov=" + cboSProvince.SelectedValue + "&eprov=" + cboEProvince.SelectedValue + "&sbranch=" + cboSBranch.SelectedValue + "&ebranch=" + cboEBranch.SelectedValue + "','_blank');self.focus();", True)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ReadCookies()
        If Request.Cookies("POS_X-ReadSum")("Check") = "true" Then
            cboCompany.SelectedValue = Request.Cookies("POS_X-ReadSum")("Company")
            cboArea.SelectedValue = Request.Cookies("POS_X-ReadSum")("Area")
            LoadProvince("Where")
            LoadBranch("Where")
            cboSProvince.SelectedValue = Request.Cookies("POS_X-ReadSum")("ProvinceFrom")
            cboEProvince.SelectedValue = Request.Cookies("POS_X-ReadSum")("ProvinceTo")
            LoadBranch("Where")
            cboSBranch.SelectedValue = Request.Cookies("POS_X-ReadSum")("BranchFrom")
            cboEBranch.SelectedValue = Request.Cookies("POS_X-ReadSum")("BranchTo")
            If Request.Cookies("POS_X-ReadSum")("Type") = "date" Then
                GroupDate.Checked = true
            Else
                GroupShop.Checked = true
            End If
            CheckCookies.Checked = True
        ElseIf Request.Cookies("POS_X-ReadSum")("Check") = "false" Then
            CheckCookies.Checked = False
        End If

    End Sub

    Private Sub WriteCookies(ByVal check As String)
        If check = "true" Then
            Response.Cookies("POS_X-ReadSum").Expires = DateTime.Now.AddDays(15)
            Response.Cookies("POS_X-ReadSum")("Login") = Session("Uemail").ToString
            Response.Cookies("POS_X-ReadSum")("Company") = cboCompany.SelectedValue
            Response.Cookies("POS_X-ReadSum")("Area") = cboArea.SelectedValue
            Response.Cookies("POS_X-ReadSum")("ProvinceFrom") = cboSProvince.SelectedValue
            Response.Cookies("POS_X-ReadSum")("ProvinceTo") = cboEProvince.SelectedValue
            Response.Cookies("POS_X-ReadSum")("BranchFrom") = cboSBranch.SelectedValue
            Response.Cookies("POS_X-ReadSum")("BranchTo") = cboEBranch.SelectedValue
            If GroupDate.Checked = True Then
                Response.Cookies("POS_X-ReadSum")("Type") = "date"
            Else
                Response.Cookies("POS_X-ReadSum")("Type") = "shop"
            End If
            Response.Cookies("POS_X-ReadSum")("DateTime") = Now()
            Response.Cookies("POS_X-ReadSum")("Check") = "true"
        Else
            Response.Cookies("POS_X-ReadSum").Expires = DateTime.Now.AddDays(15)
            Response.Cookies("POS_X-ReadSum")("Login") = Session("Uemail").ToString
            Response.Cookies("POS_X-ReadSum")("Company") = ""
            Response.Cookies("POS_X-ReadSum")("Area") = ""
            Response.Cookies("POS_X-ReadSum")("ProvinceFrom") = ""
            Response.Cookies("POS_X-ReadSum")("ProvinceTo") = ""
            Response.Cookies("POS_X-ReadSum")("BranchFrom") = ""
            Response.Cookies("POS_X-ReadSum")("BranchTo") = ""
            Response.Cookies("POS_X-ReadSum")("Type") = ""
            Response.Cookies("POS_X-ReadSum")("DateTime") = Now()
            If check = "e" Then
                Response.Cookies("POS_X-ReadSum")("Check") = "true"
            Else
                Response.Cookies("POS_X-ReadSum")("Check") = "false"
            End If
        End If
    End Sub
End Class
