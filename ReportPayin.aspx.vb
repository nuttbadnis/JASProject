Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Web.UI
Imports System.Threading
Imports System.Net.IPAddress

Partial Class ReportPayin
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim CF As New Cls_RequestFlow
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Check PostBack
            If SDate.Text <> "" then
                ClientScript.RegisterStartupScript(Page.GetType, "GetSDate", "$('#SDate').val('"+SDate.Text+"');", True)
            End If
            If EDate.Text <> "" then
                ClientScript.RegisterStartupScript(Page.GetType, "GetEDate", "$('#EDate').val('"+EDate.Text+"');", True)
            End If
        If Not Page.IsPostBack Then
            ' Dim vDate As String
            ' Dim currThead As Globalization.CultureInfo
            ' currThead = Thread.CurrentThread.CurrentCulture
            ' Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US")
            ' vDate = Format(Now, "dd/MM/yyyy")
            ' If Mid(vDate, 7, 4) > 2500 Then
            '     vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) - 543).ToString
            ' End If
            ' SDate.Text = vDate
            ' EDate.Text = vDate
            ' Thread.CurrentThread.CurrentCulture = currThead
            LoadCompany()
            LoadArea()
            LoadProvince("")
            LoadBranch("")
            LoadAccount("")

            '////////// Cookie ///////
            Try
                ddlCompany.SelectedValue = Request.Cookies("ReportPayin")("Company")
                LoadArea()
                ddlArea.SelectedValue = Request.Cookies("ReportPayin")("AreaFrom")

                LoadProvince("Where")
                ddlProvince.SelectedValue = Request.Cookies("ReportPayin")("ProvinceFrom")

                LoadBranch("Where")
                ddlSBranch.SelectedValue = Request.Cookies("ReportPayin")("BranchFrom")
                ddlEBranch.SelectedValue = Request.Cookies("ReportPayin")("BranchTo")

                LoadAccount("Where")
                ddlSAccount.SelectedValue = Request.Cookies("ReportPayin")("AccountFrom")
                ddlEAccount.SelectedValue = Request.Cookies("ReportPayin")("AccountTo")


                If Request.Cookies("ReportPayin")("Groupsum") = "rboSumary" Then
                    RadioButtonList1.SelectedIndex = 0
                ElseIf Request.Cookies("ReportPayin")("Groupsum") = "rboDetail" Then
                    RadioButtonList1.SelectedIndex = 1
               
                End If


            Catch ex As Exception
                Response.Cookies("ReportPayin").Expires = DateTime.MaxValue
                Response.Cookies("ReportPayin")("Login") = Session("Uemail").ToString
                Response.Cookies("ReportPayin")("Company") = ""
                Response.Cookies("ReportPayin")("AreaFrom") = ""
                Response.Cookies("ReportPayin")("ProvinceFrom") = ""
                Response.Cookies("ReportPayin")("BranchFrom") = ""
                Response.Cookies("ReportPayin")("BranchTo") = ""
                Response.Cookies("ReportPayin")("AccountFrom") = ""
                Response.Cookies("ReportPayin")("AccountTo") = ""
                Response.Cookies("ReportPayin")("Groupsum") = ""

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

    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        C.SetDropDownList(ddlCompany, vSql, "ComName", "ComCode")
    End Sub

    Private Sub LoadArea()
        Try
            Dim strSql As String = "select distinct o.f03 as AreaCode,mo.f02 + ' :: ' + mo.f03 as AreaName from m00860 ma, m00030 o, m02300 mo where o.f03=mo.f02 and ma.f03 = o.f02 and ma.f02='" + Session("Uemail") + "'"
            C.SetDropDownList(ddlArea, strSql, "AreaName", "AreaCode", "---ดูข้อมูลทั้งหมด---")

            If ddlArea.Items.Count = 1 Then
                strSql = "select distinct o.f03 as AreaCode,mo.f02 + ' :: ' + mo.f03 as AreaName from m00860 ma, m00030 o, m02300 mo where o.f03=mo.f02 and ma.f03 = 'all' and ma.f02='" + Session("Uemail") + "'"
                C.SetDropDownList(ddlArea, strSql, "AreaName", "AreaCode", "---ดูข้อมูลทั้งหมด---")
            End If

        Catch ex As Exception
        End Try

    End Sub

    Private Sub LoadProvince(ByVal Wherecause As String)
        Try
            Dim strSql As String
            Dim strW As String = ""
            If Wherecause <> "" Then
                If ddlArea.SelectedIndex <> 0 Then
                    ddlProvince.Enabled = True
                   

                    strW = "o.f03 = '" & ddlArea.SelectedValue.ToString & "'"
                End If
                Wherecause = strW

            End If

            strSql = "select distinct m02.f06 as ProvinceCode, m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12 and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by m02.f06 "
            C.SetDropDownList(ddlProvince, strSql, "ProvinceName", "ProvinceCode", "---ทั้งหมด---")
            If ddlProvince.Items.Count = 1 Then

                strSql = "select distinct m02.f06 as ProvinceCode,m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and m02.f02 = o.f12 and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by m02.f06 "
                C.SetDropDownList(ddlProvince, strSql, "ProvinceName", "ProvinceCode", "---ทั้งหมด---")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadBranch(ByVal Wherecause As String)
        Try
            Dim strSql As String
            If Wherecause <> "" Then
                Dim strW As String = ""
                If ddlArea.SelectedIndex <> 0 Then
                    strW = "o.f03 = '" & ddlArea.SelectedValue.ToString & "'"
                End If

                If ddlProvince.SelectedIndex <> 0 Then
                    ddlSBranch.Enabled = True
                    ddlEBranch.Enabled = True
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "m02.f06 = '" & ddlProvince.SelectedValue.ToString & "'"
                End If
               
                Wherecause = strW
            End If


            strSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
            SetDropDownList2(ddlSBranch, ddlEBranch, strSql, "BranchName", "BranchCode", "")
            If ddlSBranch.Items.Count = 1 Then
                strSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("Uemail") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
                SetDropDownList2(ddlSBranch, ddlEBranch, strSql, "BranchName", "BranchCode", "")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadAccount(ByVal Wherecause As String)
        Try
            Dim strSql As String
            If Wherecause <> "" Then
                Dim strW As String = ""
                'If ddlCompany.SelectedIndex <> 0 Then
                ddlSAccount.Enabled = True
                ddlEAccount.Enabled = True
                strW = "o.f01 = '" & ddlCompany.SelectedValue.ToString & "'"
                'End If

                Wherecause = strW
            End If


            strSql = "select distinct o.f02 as AccountCode,o.f02 + ' :: ' + o.f03 as AccountName from m06020 o, m00010 c where o.f01 = c.f01  " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
            SetDropDownList2(ddlSAccount, ddlEAccount, strSql, "AccountName", "AccountCode", "")
            If ddlSBranch.Items.Count = 1 Then
                strSql = "select distinct o.f02 as AccountCode,o.f02 + ' :: ' + o.f03 as AccountName from m06020 o, m00010 c where o.f01 = c.f01  " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
                SetDropDownList2(ddlSAccount, ddlEAccount, strSql, "AccountName", "AccountCode", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArea.SelectedIndexChanged
        LoadProvince("Where")
        LoadBranch("Where")
    End Sub

    Protected Sub ddlProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvince.SelectedIndexChanged
        LoadBranch("Where")
    End Sub
    Protected Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        LoadAccount("Where")
    End Sub

    Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        Preview()
    End Sub
    Private Sub Preview()
        Dim strSelect As String = ""
        Dim strGroup As String = ""
        Dim strOrder As String = ""
        Dim vHeader As String = ""
        Dim vComName As String = ddlCompany.SelectedItem.ToString
        Dim vProvince As String = ddlProvince.SelectedItem.ToString
        Dim vSBranch As String = ddlSBranch.SelectedItem.ToString
        Dim vEBranch As String = ddlEBranch.SelectedItem.ToString
        Dim vArea As String = ddlArea.SelectedItem.ToString
        Dim vSAccount As String = ddlSAccount.SelectedItem.ToString
        Dim vEAccount As String = ddlEAccount.SelectedItem.ToString
        Dim rdShow As String = RadioButtonList1.SelectedValue
        Dim sb As New StringBuilder

        If chkParameter.Checked = True Then
            Response.Cookies("ReportPayin")("Login") = Session("Uemail").ToString
            Response.Cookies("ReportPayin")("Company") = ddlCompany.SelectedValue
            Response.Cookies("ReportPayin")("AreaFrom") = ddlArea.SelectedValue
            Response.Cookies("ReportPayin")("ProvinceFrom") = ddlProvince.SelectedValue
            Response.Cookies("ReportPayin")("BranchFrom") = ddlSBranch.SelectedValue
            Response.Cookies("ReportPayin")("BranchTo") = ddlEBranch.SelectedValue
            Request.Cookies("ReportPayin")("AccountFrom") = ddlSAccount.SelectedValue
            Request.Cookies("ReportPayin")("AccountTo") = ddlEAccount.SelectedValue
            Response.Cookies("ReportPayin")("Groupsum") = RadioButtonList1.SelectedIndex
        Else

            Response.Cookies("ReportPayin").Expires = DateTime.MaxValue
            Response.Cookies("ReportPayin")("Login") = Session("Uemail").ToString
            Response.Cookies("ReportPayin")("Company") = ""
            Response.Cookies("ReportPayin")("AreaFrom") = ""
            Response.Cookies("ReportPayin")("ProvinceFrom") = ""
            Response.Cookies("ReportPayin")("BranchFrom") = ""
            Response.Cookies("ReportPayin")("BranchTo") = ""
            Request.Cookies("ReportPayin")("AccountFrom") = ""
            Request.Cookies("ReportPayin")("AccountTo") = ""
            Response.Cookies("ReportPayin")("Groupsum") = ""
        End If
        Try
            If RadioButtonList1.SelectedValue = 0 Then
                sb.Remove(0, sb.Length)
                strSelect = " select m.f01,m.f02 'Branch',m.f05 'DocNo',convert(varchar(10),m.f06,103) 'DocDate',m.f12 'Account'"
                'strSelect += " ,case sum(d.f09) when 0 then '' else convert(varchar(10),m.f13,103) end 'PayinDate',m.f15 'Note'"
                strSelect += " ,convert(varchar(10),m.f13,103) 'PayinDate',m.f15 'Note'"
                strSelect += " ,sum(d.f09) 'CS',sum(d.f14) 'Charge',sum(d.f12) 'CQ',m.f17+m.f18 'Sum',convert(varchar(10), m.f06,103)+' '+'เวลา'+' '+ substring(m.f07,1,2)+':'+ substring(m.f07,3,4)+' '+'น.' 'DateRecord' ,convert(varchar(10), m.y01,103)+' '+'เวลา'+' '+ convert(varchar(5),m.y01,108)+' '+'น.' 'DateSystem', '' 'CreatedBy'"
                strGroup = " group by m.f01,m.f02,m.f05,m.f06,m.f12,m.f13,m.f15,m.f17,m.f18,m.f07,m.y01 "
                strOrder = " Order by m.f01,m.f02,m.f05,m.f06,m.f12,m.f13"

                sb.Append(strSelect)
                sb.Append(vbCr + " from r16030 m,r16031 d,m00030 a,m02020 b" + vbCr)
                sb.Append(" where m.f01=d.f01 and m.f02=d.f02 and m.f05=d.f05 and a.f12=b.f02 and a.f02=m.f02 and a.f02=d.f02 and m.f19='1'")

                '-----------Company----------

                'If ddlCompany.SelectedIndex <> 0 Then
                sb.Append(" and m.f01 = '" + ddlCompany.SelectedValue.ToString + "' ")   '-- Company select f01,f02 from m00010
                vComName = Replace(ddlCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(ddlCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "|[21030] รายงานใบนำฝากเงิน |"
                '----------- Area -----------

                If Not (ddlArea.SelectedIndex = 0 Or ddlArea.Items.Count = 0) Then
                    sb.Append(" and a.f03='" + ddlArea.SelectedValue.ToString + "'")

                    vArea = Replace(ddlArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(ddlArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += vArea
                Else
                    vHeader += "ทุกเขตธุรกิจ" + "  "
                End If

                '----------- Province -----------

                If Not (ddlProvince.SelectedIndex = 0 Or ddlProvince.Items.Count = 0) Then
                    sb.Append(" and b.f06 ='" + ddlProvince.SelectedValue.ToString + "' ")   '-- Province select f02,f03 from m02020

                    vProvince = Replace(ddlProvince.SelectedItem.ToString, " :: ", "/")
                    vProvince = Mid(vProvince, InStr(Replace(ddlProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vProvince))
                    vHeader += "จังหวัด" + vProvince + "  "
                Else
                    vHeader += "ทุกจังหวัด" + "  "
                End If


                '----------- Branch -----------

                If Not (ddlSBranch.SelectedIndex = 0 Or ddlSBranch.Items.Count = 0) Then
                    sb.Append(" and a.f02>='" + ddlSBranch.SelectedValue.ToString + "'")  '-- Office select f02,04 from m00030

                    vSBranch = Replace(ddlSBranch.SelectedItem.ToString, " :: ", "/")
                    vSBranch = Mid(vSBranch, InStr(Replace(ddlSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                    vHeader += " จากสาขา " + vSBranch + "  "

                End If
                If Not (ddlEBranch.SelectedIndex = 0 Or ddlEBranch.Items.Count = 0) Then
                    sb.Append(" and a.f02<='" + ddlEBranch.SelectedValue.ToString + "'")  '-- Office select f02,04 from m00030

                    vEBranch = Replace(ddlEBranch.SelectedItem.ToString, " :: ", "/")
                    vEBranch = Mid(vEBranch, InStr(Replace(ddlEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                    vHeader += " ถึงสาขา " + vEBranch + "  "

                End If


                vHeader += "|"
                '----------- Date -----------

                If SDate.Text.Length <> 0 Then
                    sb.Append(" and replace(convert(varchar(10),m.f06,21),'-','/')>='" + CDateText(SDate.Text) + "' ")

                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If
                'vHeader += "|"
                If EDate.Text.Length <> 0 Then
                    sb.Append(" and replace(convert(varchar(10),m.f06,21),'-','/')<='" + CDateText(EDate.Text) + "'")

                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                '----------------Account------------
                If Not (ddlSAccount.SelectedIndex = 0 Or ddlSAccount.Items.Count = 0) Then
                    sb.Append(" and m.f12>='" + ddlSAccount.SelectedValue.ToString + "'")  '-- Office select f02,04 from m00030

                    vSAccount = Replace(ddlSAccount.SelectedValue.ToString, " :: ", "/")
                    vSAccount = Mid(vSAccount, InStr(Replace(ddlSAccount.SelectedValue.ToString, " :: ", "/"), "/") + 1, Len(vSAccount))
                    vHeader += " จากเลขที่บัญชี  " + vSAccount + "  "

                End If
                If Not (ddlEAccount.SelectedIndex = 0 Or ddlEAccount.Items.Count = 0) Then
                    sb.Append(" and m.f12<='" + ddlEAccount.SelectedValue.ToString + "'")  '-- Office select f02,04 from m00030

                    vEAccount = Replace(ddlEAccount.SelectedValue.ToString, " :: ", "/")
                    vEAccount = Mid(vEAccount, InStr(Replace(ddlEAccount.SelectedValue.ToString, " :: ", "/"), "/") + 1, Len(vEAccount))
                    vHeader += " ถึงเลขที่บัญชี  " + vEAccount + "  "

                End If
                vHeader += "|"
                '----------------------query2----------------------
            Else
                sb.Remove(0, sb.Length)
                strSelect = " select m.f01,m.f02 'Branch',m.f05 'DocNo',convert(varchar(10),m.f06,103) 'DocDate',m.f12 'Account'"
                'strSelect = " ,convert(varchar(10),m.f13,103) 'SubPay'"
                strSelect += " ,case d.f06 when 'CQ' then convert(varchar(10),d.f11,103) else convert(varchar(10),d.f08,103) end 'SubPay'"
                'strSelect += " ,case d.f06 when 'CQ' then '' else convert(varchar(10),m.f13,103) end 'PayinDate',m.f15 'Note'"
                strSelect += " ,convert(varchar(10),m.f13,103) 'PayinDate',m.f15 'Note'"
                strSelect += " ,sum(d.f09) 'CS',sum(d.f14) 'Charge',sum(d.f12) 'CQ',d.f09 + d.f14 + d.f12 'Sum' ,d.f10 'ChequeNo',convert(varchar(10), m.f06,103)+' '+'เวลา'+' '+ substring(m.f07,1,2)+':'+ substring(m.f07,3,4)+' '+'น.' 'DateRecord' ,convert(varchar(10), m.y01,103)+' '+'เวลา'+' '+ convert(varchar(5),m.y01,108)+' '+'น.' 'DateSystem' "
                'strSelect += " , m.f10 'CreatedBy'"
                strSelect += " , isnull(emp.F06,'') + ' ' + isnull(emp.F07,'') 'CreatedBy'"
                strGroup = " group by m.f01,m.f02,m.f05,m.f06,m.f12,d.f08,d.f11,m.f13,m.f15,d.f09,d.f14,d.f12,d.f10,m.f07,d.f06,m.y01,emp.F06,emp.F07 "
                strOrder = " Order by m.f01,m.f02,m.f05,m.f06,m.f12,m.f13"

                sb.Append(strSelect)
                sb.Append(vbCr + " from r16030 m inner join r16031 d on m.f01 = d.f01 and m.f02 = d.f02 and m.f05 = d.f05 inner join m00030 a on a.f02 = m.f02 and a.f02 = d.f02 inner join m02020 b on a.f12 = b.f02 " + vbCr)
                sb.Append(" left outer join m00040 emp on m.f10 = emp.F03 " + vbCr)
                sb.Append(" where m.f19 = '1'")

                'sb.Append(vbCr + " from r16030 m,r16031 d,m00030 a,m02020 b" + vbCr)
                'sb.Append(" where m.f01=d.f01 and m.f02=d.f02 and m.f05=d.f05 and a.f12=b.f02 and a.f02=m.f02 and a.f02=d.f02 and m.f19='1'")

                '-----------Company----------

                'If ddlCompany.SelectedIndex <> 0 Then
                sb.Append(" and m.f01 = '" + ddlCompany.SelectedValue.ToString + "' ")   '-- Company select f01,f02 from m00010
                vComName = Replace(ddlCompany.SelectedItem.ToString, " :: ", "/")
                vComName = Mid(vComName, InStr(Replace(ddlCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
                vHeader = vComName
                'End If
                vHeader += "|"
                '----------- Area -----------

                If Not (ddlArea.SelectedIndex = 0 Or ddlArea.Items.Count = 0) Then
                    sb.Append(" and a.f03='" + ddlArea.SelectedValue.ToString + "'")

                    vArea = Replace(ddlArea.SelectedItem.ToString, " :: ", "/")
                    vArea = Mid(vArea, InStr(Replace(ddlArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vArea))
                    vHeader += vArea
                Else
                    vHeader += "ทุกเขตธุรกิจ" + "  "
                End If

                '----------- Province -----------

                If Not (ddlProvince.SelectedIndex = 0 Or ddlProvince.Items.Count = 0) Then
                    sb.Append(" and b.f06 ='" + ddlProvince.SelectedValue.ToString + "' ")   '-- Province select f02,f03 from m02020

                    vProvince = Replace(ddlProvince.SelectedItem.ToString, " :: ", "/")
                    vProvince = Mid(vProvince, InStr(Replace(ddlProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vProvince))
                    vHeader += "จังหวัด" + vProvince + "  "
                Else
                    vHeader += "ทุกจังหวัด" + "  "
                End If


                '----------- Branch -----------

                If Not (ddlSBranch.SelectedIndex = 0 Or ddlSBranch.Items.Count = 0) Then
                    sb.Append(" and a.f02>='" + ddlSBranch.SelectedValue.ToString + "'")  '-- Office select f02,04 from m00030

                    vSBranch = Replace(ddlSBranch.SelectedItem.ToString, " :: ", "/")
                    vSBranch = Mid(vSBranch, InStr(Replace(ddlSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                    vHeader += " จากสาขา " + vSBranch + "  "

                End If
                If Not (ddlEBranch.SelectedIndex = 0 Or ddlEBranch.Items.Count = 0) Then
                    sb.Append(" and a.f02<='" + ddlEBranch.SelectedValue.ToString + "'")  '-- Office select f02,04 from m00030

                    vEBranch = Replace(ddlEBranch.SelectedItem.ToString, " :: ", "/")
                    vEBranch = Mid(vEBranch, InStr(Replace(ddlEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                    vHeader += " ถึงสาขา " + vEBranch + "  "

                End If


                vHeader += "|"
                '----------- Date -----------

                If SDate.Text.Length <> 0 Then
                    sb.Append(" and replace(convert(varchar(10),m.f06,21),'-','/')>='" + CDateText(SDate.Text) + "' ")

                    vHeader += " ตั้งแต่ วันที่ " + SDate.Text
                End If
                'vHeader += "|"
                If EDate.Text.Length <> 0 Then
                    sb.Append(" and replace(convert(varchar(10),m.f06,21),'-','/')<='" + CDateText(EDate.Text) + "'")

                    vHeader += " ถึงวันที่ " + EDate.Text
                End If
                vHeader += "|"
                '----------------Account------------
                If Not (ddlSAccount.SelectedIndex = 0 Or ddlSAccount.Items.Count = 0) Then
                    sb.Append(" and m.f12>='" + ddlSAccount.SelectedValue.ToString + "'")  '-- Office select f02,04 from m00030

                    vSAccount = Replace(ddlSAccount.SelectedValue.ToString, " :: ", "/")
                    vSAccount = Mid(vSAccount, InStr(Replace(ddlSAccount.SelectedValue.ToString, " :: ", "/"), "/") + 1, Len(vSAccount))
                    vHeader += " จากเลขที่บัญชี  " + vSAccount + "  "

                End If
                If Not (ddlEAccount.SelectedIndex = 0 Or ddlEAccount.Items.Count = 0) Then
                    sb.Append(" and m.f12<='" + ddlEAccount.SelectedValue.ToString + "'")  '-- Office select f02,04 from m00030

                    vEAccount = Replace(ddlEAccount.SelectedValue.ToString, " :: ", "/")
                    vEAccount = Mid(vEAccount, InStr(Replace(ddlEAccount.SelectedValue.ToString, " :: ", "/"), "/") + 1, Len(vEAccount))
                    vHeader += " ถึงเลขที่บัญชี  " + vEAccount + "  "

                End If
                vHeader += "|"
            End If


            sb.Append(vbCr + vbCr + strGroup + strOrder)

            Dim rDate As DateTime = Now()
            'Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            Dim Parameter As String = "Area: " + ddlArea.SelectedValue.ToString + " Province: " + ddlProvince.SelectedValue.ToString + " Shop F:" + ddlSBranch.SelectedValue.ToString + " T:" + ddlEBranch.SelectedValue.ToString + " Date F:" + Log.CDateText(SDate.Text) + " T:" + Log.CDateText(EDate.Text)
            Session("Parameter") = Parameter
            Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
            'Log.LogReport("21030", "รายงานใบนำฝากเงิน", "Area: " + ddlArea.SelectedValue.ToString + " Province: " + ddlProvince.SelectedValue.ToString + " Shop F:" + ddlSBranch.SelectedValue.ToString + " T:" + ddlEBranch.SelectedValue.ToString + " Date F:" + Log.CDateText(SDate.Text) + " T:" + Log.CDateText(EDate.Text), vIPAddress, Session("Uemail"))

            Session("StringQuery") = sb.ToString
            Session("Header") = vHeader

            ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('ShowReportPayin.aspx?show=" + rdShow + "','_blank');self.focus();", True)

        Catch ex As Exception
            ClientScript.RegisterStartupScript(Page.GetType, "Err", "alert('" + ex.Message + "');", True)
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
End Class
