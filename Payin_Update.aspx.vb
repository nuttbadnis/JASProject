Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Payin_Update
    Inherits System.Web.UI.Page
    Dim Log As New Cls_LogReport
    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            LoadCompany()
            LoadArea()
            LoadProvince("")
            LoadBranch("")
            '////////// Cookie ///////
            Try

                cboCompany.SelectedValue = Request.Cookies("PayinUpdate")("Company")
                LoadArea()
                cboSArea.SelectedValue = Request.Cookies("PayinUpdate")("AreaFrom")
                cboEArea.SelectedValue = Request.Cookies("PayinUpdate")("AreaTo")

                LoadProvince("Where")
                cboSProvince.SelectedValue = Request.Cookies("PayinUpdate")("ProvinceFrom")
                cboEProvince.SelectedValue = Request.Cookies("PayinUpdate")("ProvinceTo")

                LoadBranch("Where")
                cboSBranch.SelectedValue = Request.Cookies("PayinUpdate")("BranchFrom")
                cboEBranch.SelectedValue = Request.Cookies("PayinUpdate")("BranchTo")

                If Request.Cookies("PayinUpdate")("GroupBy") = "OptSum" Then
                    OptSum.Checked = True
                ElseIf Request.Cookies("PayinUpdate")("GroupBy") = "OptChk" Then
                    OptChk.Checked = True
                End If

                If Request.Cookies("PayinUpdate")("Type") = "Optcq" Then
                    Optcq.Checked = True
                ElseIf Request.Cookies("PayinUpdate")("Type") = "Optcs" Then
                    Optcs.Checked = True
                End If
            Catch ex As Exception
                Response.Cookies("PayinUpdate").Expires = DateTime.MaxValue
                Response.Cookies("PayinUpdate")("Login") = Session("Uemail").ToString
                Response.Cookies("PayinUpdate")("Company") = ""
                Response.Cookies("PayinUpdate")("AreaFrom") = ""
                Response.Cookies("PayinUpdate")("AreaTo") = ""
                Response.Cookies("PayinUpdate")("ProvinceFrom") = ""
                Response.Cookies("PayinUpdate")("ProvinceTo") = ""
                Response.Cookies("PayinUpdate")("BranchFrom") = ""
                Response.Cookies("PayinUpdate")("BranchTo") = ""
                Response.Cookies("PayinUpdate")("GroupBy") = ""
                Response.Cookies("PayinUpdate")("Type") = ""
            End Try
            '////////////////////////
        End If
    End Sub

    Public Sub SetDropDownList(ByRef ddl As DropDownList, ByVal CmdText As String, ByVal TextField As String, ByVal ValueField As String, Optional ByVal Row0 As String = Nothing)
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
            ddl.DataBind()
        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        End Try
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

    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        C.SetDropDownList(cboCompany, vSql, "ComName", "ComCode")
    End Sub

    Private Sub LoadArea()
        'Dim strSql As String = "select distinct f02 as AreaCode,f02 + ' :: ' + f03  as AreaName from m02300 order by f02"
        Dim strSql As String = "select distinct o.f03 as AreaCode,mo.f02 + ' :: ' + mo.f03 as AreaName from m00860 ma, m00030 o, m02300 mo where o.f03=mo.f02 and ma.f03 = o.f02 and ma.f02='" + Session("Uemail") + "'"
        SetDropDownList(cboSArea, strSql, "AreaName", "AreaCode", "")
        SetDropDownList(cboEArea, strSql, "AreaName", "AreaCode", "")
        If cboSArea.Items.Count = 1 Then
            strSql = "select distinct o.f03 as AreaCode,mo.f02 + ' :: ' + mo.f03 as AreaName from m00860 ma, m00030 o, m02300 mo where o.f03=mo.f02 and ma.f03 = 'all' and ma.f02='" + Session("Uemail") + "'"
            SetDropDownList(cboSArea, strSql, "AreaName", "AreaCode", "")
            SetDropDownList(cboEArea, strSql, "AreaName", "AreaCode", "")
        End If
    End Sub

    Private Sub LoadProvince(ByVal Wherecause As String)
        Try
            Dim strSql As String
            Dim strW As String = ""
            If Wherecause <> "" Then
                If cboSArea.SelectedIndex <> 0 Then
                    strW = "o.f03 >= '" & cboSArea.SelectedValue.ToString & "'"
                End If
                If cboEArea.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "o.f03 <= '" & cboEArea.SelectedValue.ToString & "'"
                End If
                Wherecause = strW
            End If

            'strSql = "select distinct m02.f06 as ProvinceCode, m02.f06 + ' :: ' + m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12 " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by m02.f06 "
            strSql = "select distinct m02.f06 as ProvinceCode, m02.f06 + ' :: ' + m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12 and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by m02.f06 "
            SetDropDownList2(cboSProvince, cboEProvince, strSql, "ProvinceName", "ProvinceCode", "")
            If cboSProvince.Items.Count = 1 Then
                strSql = "select distinct m02.f06 as ProvinceCode, m02.f06 + ' :: ' + m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and m02.f02 = o.f12 and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by m02.f06 "
                SetDropDownList2(cboSProvince, cboEProvince, strSql, "ProvinceName", "ProvinceCode", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LoadBranch(ByVal Wherecause As String)
        Try
            Dim strSql As String
            If Wherecause <> "" Then
                Dim strW As String = ""
                If cboSArea.SelectedIndex <> 0 Then
                    strW = "o.f03 >= '" & cboSArea.SelectedValue.ToString & "'"
                End If
                If cboEArea.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "o.f03 <= '" & cboEArea.SelectedValue.ToString & "'"
                End If

                If cboSProvince.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "m02.f06 >= '" & cboSProvince.SelectedValue.ToString & "'"
                End If
                If cboEProvince.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "m02.f06 <= '" & cboEProvince.SelectedValue.ToString & "'"
                End If
                Wherecause = strW
            End If

            'strSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + o.f04 as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and o.f20 is null " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
            strSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
            SetDropDownList2(cboSBranch, cboEBranch, strSql, "BranchName", "BranchCode", "")
            If cboSBranch.Items.Count = 1 Then
                strSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
                SetDropDownList2(cboSBranch, cboEBranch, strSql, "BranchName", "BranchCode", "")
            End If
        Catch ex As Exception

        End Try
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
        If chkParameter.Checked = True Then
            Response.Cookies("PayinUpdate").Expires = DateTime.MaxValue
            Response.Cookies("PayinUpdate")("Login") = Session("Uemail").ToString
            Response.Cookies("PayinUpdate")("Company") = cboCompany.SelectedValue
            Response.Cookies("PayinUpdate")("AreaFrom") = cboSArea.SelectedValue
            Response.Cookies("PayinUpdate")("AreaTo") = cboEArea.SelectedValue
            Response.Cookies("PayinUpdate")("ProvinceFrom") = cboSProvince.SelectedValue
            Response.Cookies("PayinUpdate")("ProvinceTo") = cboEProvince.SelectedValue
            Response.Cookies("PayinUpdate")("BranchFrom") = cboSBranch.SelectedValue
            Response.Cookies("PayinUpdate")("BranchTo") = cboEBranch.SelectedValue

            If OptSum.Checked = True Then
                Response.Cookies("PayinUpdate")("GroupBy") = OptSum.Checked
            ElseIf OptChk.Checked = True Then
                Response.Cookies("PayinUpdate")("GroupBy") = OptChk.Checked

            End If

            If Optcq.Checked = True Then
                Response.Cookies("PayinUpdate")("Type") = Optcq.Checked

            ElseIf Optcs.Checked = True Then
                Response.Cookies("PayinUpdate")("Type") = Optcs.Checked

            End If
        Else
            Response.Cookies("PayinUpdate").Expires = DateTime.MaxValue
            Response.Cookies("PayinUpdate")("Login") = Session("Uemail").ToString
            Response.Cookies("PayinUpdate")("Company") = ""
            Response.Cookies("PayinUpdate")("AreaFrom") = ""
            Response.Cookies("PayinUpdate")("AreaTo") = ""
            Response.Cookies("PayinUpdate")("ProvinceFrom") = ""
            Response.Cookies("PayinUpdate")("ProvinceTo") = ""
            Response.Cookies("PayinUpdate")("BranchFrom") = ""
            Response.Cookies("PayinUpdate")("BranchTo") = ""
            Response.Cookies("PayinUpdate")("GroupBy") = ""
            Response.Cookies("PayinUpdate")("Type") = ""
        End If
        Dim strGroup As String = ""
        Dim strGroup2 As String = ""
        Dim strOrder As String = ""
        Dim strOrder2 As String = ""
        Dim vHeader As String = ""
        Dim vComName As String = cboCompany.SelectedItem.ToString
        Dim vSProvince As String = cboSProvince.SelectedItem.ToString
        Dim vEProvince As String = cboEProvince.SelectedItem.ToString
        Dim vSBranch As String = cboSBranch.SelectedItem.ToString
        Dim vEBranch As String = cboEBranch.SelectedItem.ToString
        Dim vSArea As String = cboSArea.SelectedIndex.ToString
        Dim vEArea As String = cboEArea.SelectedItem.ToString
        Dim vSum As Boolean = OptSum.Checked
        Dim vChk As Boolean = OptChk.Checked
        Dim vCQ As Boolean = Optcq.Checked
        Dim vCS As Boolean = Optcs.Checked
        Dim sqlCQ As String = ""
        Dim sqlCS As String = ""
        Dim sqlChkCQ As String = ""
        Dim sb As New StringBuilder
        Dim sb2 As New StringBuilder
        
        Try
           
            Dim rDate As DateTime = Now()
            Dim Parameter As String = "Area F:" + cboSArea.SelectedValue.ToString + " T:" + cboEArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString
            Session("Parameter") = Parameter
            Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
            '---------------------------------แบบสรุป-----------------------
            '----------------------Sum (CQ)-----------------------
            '------------------------------------------------
            If vSum = True Then
                If vCQ = True Then
                    ''sqlCQ = " select company_code,area_code,province_code,branch_code,sum(round(receipt_cq,2)) as receipt_cq,sum(round(payin_cq,2)) as payin_cq,sum(round(cq_in_sys,2)) cq_in_sys from tt_cq_sum"
                    '' sqlCQ += " where sys_date = REPLACE(convert(varchar(10),getdate(),21),'-','/') and "
                    'sqlCQ = " select CompanyReceipt,b.f03 f03,c.f06 f06,CodeShop,ReceiptDate,ChequeNo,sum(Amount) Amount"
                    'sqlCQ += " from TempArrearsPaymentCQ a, m00030 b,m02020 c where a.CodeShop = b.f02"
                    'sqlCQ += " and b.f12 =c.f02 and"
                    sqlCQ = "  select CompanyReceipt,b.f03 f03,c.f06 f06,CodeShop,sum(Amount) Amount"
                    sqlCQ += " from reconcile01.dbo.TempArrearsPaymentCQ a, rmsdat01.dbo.m00030 b,rmsdat01.dbo.m02020 c where a.CodeShop = b.f02 and b.f12 =c.f02 and"
                    strOrder = " order by CompanyReceipt,b.f03,c.f06,CodeShop"
                    ' strGroup = " Group by ChequeNo, CompanyReceipt,b.f03,c.f06,CodeShop,ReceiptDate"
                    strGroup = " Group by CompanyReceipt,b.f03,c.f06,CodeShop"
                    sb.Append(sqlCQ)
                    '-------Company----------
                    'If Not (cboCompany.SelectedIndex = 0 Or cboCompany.Items.Count = 0) Then
                    sb.Append(" CompanyReceipt='" + cboCompany.SelectedValue.ToString + "'")
                    vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                    vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                    vHeader += "  " + vComName + "  "
                    'Else
                    '    vHeader += "ทุกบริษัท" + "  "
                    'End If
                    vHeader += "|[22030] รายงานค้างนำฝาก |"
                    '---------Area--------------
                    If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                        sb.Append(" and b.f03 >='" + cboSArea.SelectedValue.ToString + "' ")
                        vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                        vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                        vHeader += "จากเขต" + vSArea + "  "
                    Else
                        vHeader += "ทุกเขตธุรกิจ" + "  "
                    End If
                    If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                        sb.Append(" and b.f03 <='" + cboEArea.SelectedValue.ToString + "' ")
                        vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                        vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                        vHeader += "ถึงเขต" + vEArea + "  "
                    Else
                        vHeader += "ทุกเขตธุรกิจ" + "  "
                    End If
                    vHeader += "|"
                    '-------------Province-----------
                    If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                        sb.Append(" and c.f06 >='" + cboSProvince.SelectedValue.ToString + "' ")
                        vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                        vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                        vHeader += "จากจังหวัด" + vSProvince + "  "
                    Else
                        vHeader += "ทุกจังหวัด" + "  "
                    End If
                    If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                        sb.Append(" and c.f06 <='" + cboEProvince.SelectedValue.ToString + "' ")
                        vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                        vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                        vHeader += "ถึงจังหวัด" + vEProvince + "  "
                    Else
                        vHeader += "ทุกจังหวัด" + "  "
                    End If
                    vHeader += "|"
                    '-------------Branch------------

                    If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                        sb.Append(" and CodeShop >='" + cboSBranch.SelectedValue.ToString + "'")
                        vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                        vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                        vHeader += " จากสาขา " + vSBranch + "  "
                    Else
                        vHeader += "ทุกสาขา" + "  "
                    End If
                    If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                        sb.Append(" and CodeShop <='" + cboEBranch.SelectedValue.ToString + "'")
                        vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                        vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                        vHeader += " ถึงสาขา " + vEBranch + "  "
                    Else
                        vHeader += "ทุกสาขา" + "  "
                    End If


                    sb.Append(vbCr + strGroup + vbCr + strOrder)
                    Session("StringQuery") = sb.ToString
                    Session("Header") = vHeader

                    ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('ShowPayin_sum_CQ.aspx?','_blank');self.focus();", True)

                End If
                '---------------------Sum (CS)-----------------------------
                '----------------------------------------------
                '-------------------------------------------------
                If vCS = True Then

                    sqlCS = " select  company_code,area_code,province_code,branch_code,sum(receipt_cs) receipt_cs,sum(payin_cs) payin_cs,sum(cs_in_sys) cs_in_sys"
                    sqlCS += " from reconcile.dbo.TT_CS_CHK "
                    sqlCS += " where sys_date = REPLACE(convert(varchar(10),getdate(),21),'-','/') and "
                    strOrder2 = " order by company_code,area_code,province_code,branch_code"
                    strGroup2 = " group by company_code,area_code,province_code,branch_code"
                    sb2.Append(sqlCS)
                    '-------Company----------
                    'If Not (cboCompany.SelectedIndex = 0 Or cboCompany.Items.Count = 0) Then
                    sb2.Append(" company_code='" + cboCompany.SelectedValue.ToString + "'")
                    vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                    vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                    vHeader += "   " + vComName + "  "
                    'Else
                    '    vHeader += "ทุกบริษัท" + "  "
                    'End If
                    vHeader += "|[22030] รายงานค้างนำฝาก |"
                    '---------Area--------------
                    If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                        sb2.Append(" and area_code >='" + cboSArea.SelectedValue.ToString + "' ")
                        vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                        vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                        vHeader += "จากเขต" + vSArea + "  "
                    Else
                        vHeader += "ทุกเขตธุรกิจ" + "  "
                    End If
                    If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                        sb2.Append(" and area_code <='" + cboEArea.SelectedValue.ToString + "' ")
                        vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                        vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                        vHeader += "ถึงเขต" + vEArea + "  "
                    Else
                        vHeader += "ทุกเขตธุรกิจ" + "  "
                    End If
                    vHeader += "|"
                    '-------------Province-----------
                    If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                        sb2.Append(" and province_code >='" + cboSProvince.SelectedValue.ToString + "' ")
                        vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                        vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                        vHeader += "จากจังหวัด" + vSProvince + "  "
                    Else
                        vHeader += "ทุกจังหวัด" + "  "
                    End If
                    If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                        sb2.Append(" and province_code <='" + cboEProvince.SelectedValue.ToString + "' ")
                        vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                        vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                        vHeader += "ถึงจังหวัด" + vEProvince + "  "
                    Else
                        vHeader += "ทุกจังหวัด" + "  "
                    End If
                    vHeader += "|"
                    '-------------Branch------------

                    If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                        sb2.Append(" and branch_code >='" + cboSBranch.SelectedValue.ToString + "'")
                        vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                        vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                        vHeader += " จากสาขา " + vSBranch + "  "
                    Else
                        vHeader += "ทุกสาขา" + "  "
                    End If
                    If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                        sb2.Append(" and branch_code <='" + cboEBranch.SelectedValue.ToString + "'")
                        vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                        vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                        vHeader += " ถึงสาขา " + vEBranch + "  "
                    Else
                        vHeader += "ทุกสาขา" + "  "
                    End If


                    sb2.Append(vbCr + strGroup2 + vbCr + strOrder2)
                    Session("StringQuery2") = sb2.ToString
                    Session("Header") = vHeader

                    ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('ShowPayin_sum_CS.aspx?','_blank');self.focus();", True)
                End If
            End If

            '--------------------แบบตรวจสอบ-------------------
            '-------------------CHK (CQ)---------------------------
            '--------------------------------------------------
            If vChk = True Then
                If vCQ = True Then
                    sqlChkCQ = " select CompanyReceipt,b.f03 f03,c.f06 f06,CodeShop,ReceiptDate,ChequeNo,Amount"
                    sqlChkCQ += " from reconcile01.dbo.TempArrearsPaymentCQ a, rmsdat01.dbo.m00030 b,rmsdat01.dbo.m02020 c where a.CodeShop = b.f02"
                    sqlChkCQ += " and b.f12 =c.f02 and"
                    strOrder = " order by CompanyReceipt,b.f03,c.f06,CodeShop "

                    sb.Append(sqlChkCQ)
                    '-------Company----------
                    'If Not (cboCompany.SelectedIndex = 0 Or cboCompany.Items.Count = 0) Then
                    sb.Append(" CompanyReceipt='" + cboCompany.SelectedValue.ToString + "'")
                    vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                    vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                    vHeader += "   " + vComName + "  "
                    'Else
                    '    vHeader += "ทุกบริษัท" + "  "
                    'End If
                    vHeader += "|[22030] รายงานค้างนำฝาก |"
                    '---------Area--------------
                    If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                        sb.Append(" and b.f03 >='" + cboSArea.SelectedValue.ToString + "' ")
                        vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                        vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                        vHeader += "จากเขต" + vSArea + "  "
                    Else
                        vHeader += "ทุกเขตธุรกิจ" + "  "
                    End If
                    If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                        sb.Append(" and b.f03 <='" + cboEArea.SelectedValue.ToString + "' ")
                        vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                        vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                        vHeader += "ถึงเขต" + vEArea + "  "
                    Else
                        vHeader += "ทุกเขตธุรกิจ" + "  "
                    End If
                    vHeader += "|"
                    '-------------Province-----------
                    If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                        sb.Append(" and c.f06 >='" + cboSProvince.SelectedValue.ToString + "' ")
                        vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                        vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                        vHeader += "จากจังหวัด" + vSProvince + "  "
                    Else
                        vHeader += "ทุกจังหวัด" + "  "
                    End If
                    If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                        sb.Append(" and c.f06 <='" + cboEProvince.SelectedValue.ToString + "' ")
                        vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                        vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                        vHeader += "ถึงจังหวัด" + vEProvince + "  "
                    Else
                        vHeader += "ทุกจังหวัด" + "  "
                    End If
                    vHeader += "|"
                    '-------------Branch------------

                    If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                        sb.Append(" and CodeShop >='" + cboSBranch.SelectedValue.ToString + "'")
                        vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                        vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                        vHeader += " จากสาขา " + vSBranch + "  "
                    Else
                        vHeader += "ทุกสาขา" + "  "
                    End If
                    If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                        sb.Append(" and CodeShop <='" + cboEBranch.SelectedValue.ToString + "'")
                        vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                        vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                        vHeader += " ถึงสาขา " + vEBranch + "  "
                    Else
                        vHeader += "ทุกสาขา" + "  "
                    End If


                    sb.Append(vbCr + strOrder)
                    Session("StringQuery") = sb.ToString
                    Session("Header") = vHeader

                    ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('ShowPayin_chk_CQ.aspx?','_blank');self.focus();", True)
                End If
                '------------------------------CHK (CS)-----------------------
                '-----------------------------------------------
                '-----------------------------------------------
                If vCS = True Then
                    sqlChkCQ = " select company_code,area_code,province_code,branch_code,receipt_date,receipt_cs,payin_cs,cs_in_sys "
                    sqlChkCQ += " from reconcile.dbo.tt_cs_chk"
                    sqlChkCQ += " where sys_date=replace(convert(varchar(10),getdate(),21),'-','/') and"
                    strOrder = " order by company_code,area_code,province_code,branch_code "

                    sb.Append(sqlChkCQ)
                    '-------Company----------
                    'If Not (cboCompany.SelectedIndex = 0 Or cboCompany.Items.Count = 0) Then
                    sb.Append(" company_code='" + cboCompany.SelectedValue.ToString + "'")
                    vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
                    vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                    vHeader += "   " + vComName + "  "
                    'Else
                    '    vHeader += "ทุกบริษัท" + "  "
                    'End If
                    vHeader += "|[22030] รายงานค้างนำฝาก |"
                    '---------Area--------------
                    If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                        sb.Append(" and area_code >='" + cboSArea.SelectedValue.ToString + "' ")
                        vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                        vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                        vHeader += "จากเขต" + vSArea + "  "
                    Else
                        vHeader += "ทุกเขตธุรกิจ" + "  "
                    End If
                    If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                        sb.Append(" and area_code <='" + cboEArea.SelectedValue.ToString + "' ")
                        vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                        vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                        vHeader += "ถึงเขต" + vEArea + "  "
                    Else
                        vHeader += "ทุกเขตธุรกิจ" + "  "
                    End If
                    vHeader += "|"
                    '-------------Province-----------
                    If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                        sb.Append(" and province_code >='" + cboSProvince.SelectedValue.ToString + "' ")
                        vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                        vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                        vHeader += "จากจังหวัด" + vSProvince + "  "
                    Else
                        vHeader += "ทุกจังหวัด" + "  "
                    End If
                    If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                        sb.Append(" and province_code <='" + cboEProvince.SelectedValue.ToString + "' ")
                        vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                        vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                        vHeader += "ถึงจังหวัด" + vEProvince + "  "
                    Else
                        vHeader += "ทุกจังหวัด" + "  "
                    End If
                    vHeader += "|"
                    '-------------Branch------------

                    If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                        sb.Append(" and branch_code >='" + cboSBranch.SelectedValue.ToString + "'")
                        vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                        vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                        vHeader += " จากสาขา " + vSBranch + "  "
                    Else
                        vHeader += "ทุกสาขา" + "  "
                    End If
                    If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                        sb.Append(" and branch_code <='" + cboEBranch.SelectedValue.ToString + "'")
                        vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                        vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                        vHeader += " ถึงสาขา " + vEBranch + "  "
                    Else
                        vHeader += "ทุกสาขา" + "  "
                    End If


                    sb.Append(vbCr + strOrder)
                    Session("StringQuery") = sb.ToString
                    Session("Header") = vHeader

                    ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('ShowPayin_chk_CS.aspx?','_blank');self.focus();", True)
                End If
            End If
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Page.GetType, "Err", "alert('" + ex.Message + "');", True)
        End Try
    End Sub
    
End Class
