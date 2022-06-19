Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports System.Web.UI
'Imports System.Globalization

Partial Class pmtTransferDetail
    Inherits System.Web.UI.Page

    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("email") = "weraphon.r"
        'Session("email") = "varut.v"
        ' If Session("email") Is Nothing Then
        '     Response.Redirect("~/Chksession.aspx")
        '     ClientScript.RegisterStartupScript(Page.GetType, "open", "window.close()", True)
        ' End If
        ' Add Class Radio
            rdbOpen.InputAttributes.Add("class", "form-check-input")
            rdbClose.InputAttributes.Add("class", "form-check-input")
            rdbAll.InputAttributes.Add("class", "form-check-input")
        ' Check PostBack
            If SDate.Text <> "" then
                ClientScript.RegisterStartupScript(Page.GetType, "GetSDate", "$('#SDate').val('"+SDate.Text+"');", True)
            End If
            If EDate.Text <> "" then
                ClientScript.RegisterStartupScript(Page.GetType, "GetEDate", "$('#EDate').val('"+EDate.Text+"');", True)
            End If
        If Not Page.IsPostBack Then
            'Dim vDate As String

            ' Dim currThead As Globalization.CultureInfo
            ' currThead = Thread.CurrentThread.CurrentCulture
            ' Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US")
            'Default ให้แสดงวันที่ปัจจุบัน
            'vDate = Format(Now, "dd/MM/yyyy")
            'If Mid(vDate, 7, 4) < 2500 Then
            'vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) + 543).ToString
            'End If
            'SDate.Text = vDate
            'EDate.Text = vDate

            ' Thread.CurrentThread.CurrentCulture = currThead

            LoadArea()
            LoadProvince("")
            LoadBranch("")
            LoadBranch2("")
            LoadGroup()
            LoadSubGroup("")
            LoadItem("")

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

    Private Sub LoadArea()
        vSql = CF.rSqlDDArea(Session("Uemail"),0)
        C.SetDropDownList(cboArea, vSql, "AreaName", "AreaCode", "ดูข้อมูลทั้งหมด")
        ' ถ้าสำนักงานเป็น ALL
        If cboArea.Items.Count = 1 Then
            vSql = CF.rSqlDDArea(Session("Uemail"),1)
            C.SetDropDownList(cboArea, vSql, "AreaName", "AreaCode", "ดูข้อมูลทั้งหมด")
        End If
    End Sub

    Private Sub LoadProvince(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If cboArea.SelectedIndex <> 0 Then
                 cboProvince.Enabled = True
                 cboSBranch.Enabled = True
                 cboEBranch.Enabled = True
                strW = strW + " o.f03 = '" & cboArea.SelectedValue.ToString & "' "
            End If
            Wherecause = strW
        End If
        
        vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(cboProvince, vSql, "ProvinceName", "ProvinceCode", "ดูข้อมูลทั้งหมด")
        ' ถ้าสำนักงานเป็น ALL
        If cboProvince.Items.Count = 1 Then
            vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(cboProvince, vSql, "ProvinceName", "ProvinceCode", "ดูข้อมูลทั้งหมด")
        End If
    End Sub

    Private Sub LoadBranch(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If cboArea.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & cboArea.SelectedValue.ToString & "' "
            End If

            ' where province
            If cboProvince.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " pv.F06 = '" & cboProvince.SelectedValue.ToString & "'"
            End If

            Wherecause = strW
        End If

        vSql = CF.rSqlDDShop(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(cboSBranch, vSql, "ShopName", "ShopCode", "ดูข้อมูลทั้งหมด",cboEBranch)

        ' ถ้าสำนักงานเป็น ALL
        If cboSBranch.Items.Count = 1 Then
            vSql = CF.rSqlDDShop(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(cboSBranch, vSql, "ShopName", "ShopCode", "ดูข้อมูลทั้งหมด",cboEBranch)
        End If

    End Sub

    Private Sub LoadBranch2(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If cboArea.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & cboArea.SelectedValue.ToString & "' "
            End If

            ' where province
            If cboProvince.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " pv.F06 = '" & cboProvince.SelectedValue.ToString & "'"
            End If

            Wherecause = strW
        End If

        vSql = CF.rSqlDDShop(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(cboSBranch2, vSql, "ShopName", "ShopCode", "ดูข้อมูลทั้งหมด",cboEBranch2)

        ' ถ้าสำนักงานเป็น ALL
        If cboSBranch2.Items.Count = 1 Then
            vSql = CF.rSqlDDShop(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(cboSBranch2, vSql, "ShopName", "ShopCode", "ดูข้อมูลทั้งหมด",cboEBranch2)
        End If

    End Sub

    Protected Sub cboSArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboArea.SelectedIndexChanged
        If cboArea.SelectedIndex = 0 Then
            LoadProvince("")
            LoadBranch("")
            LoadBranch2("")
        Else
            LoadProvince("Where")
            LoadBranch("Where")
            LoadBranch2("Where")
        End If
    End Sub

    Protected Sub cboSProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProvince.SelectedIndexChanged
        If cboProvince.SelectedIndex = 0 Then
            LoadBranch("")
            LoadBranch2("")
        Else
            LoadBranch("Where")
            LoadBranch2("Where")
        End If
    End Sub

    Private Sub LoadGroup()
        Try
            Dim strSql As String

            strSql = "select distinct f02 as GroupCode ,f02+ ' :: '+f03 as GroupName from m00170 where f01='05' order by f02"
            SetDropDownList(SGroup, strSql, "GroupName", "GroupCode", "--กลุ่มหลักสินค้าทั้งหมด--")

        Catch ex As Exception

        End Try

    End Sub
    Private Sub LoadSubGroup(ByVal Wherecause As String)
        Try
            Dim strSql As String
            Dim strW As String = ""
            If Wherecause <> "" Then
                If SGroup.SelectedIndex <> 0 Then
                    strW = "f02 = '" & SGroup.SelectedValue.ToString & "'"
                End If
                Wherecause = strW
            End If
            strSql = "select distinct f03 as subGroupCode,f03+' :: '+ f04 as subGroupName from m00180 where f01='05'" + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by f03"
            SetDropDownList(SsubGroup, strSql, "subGroupName", "subGroupCode", "--กลุ่มย่อยสินค้าทั้งหมด--")

        Catch ex As Exception

        End Try

    End Sub
    Private Sub LoadItem(ByVal Wherecause As String)
        Try
            Dim strSql As String
            If Wherecause <> "" Then
                Dim strW As String = ""

                If SGroup.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "f08 = '" & SGroup.SelectedValue.ToString & "'"
                End If

                If SsubGroup.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "f09 = '" & SsubGroup.SelectedValue.ToString & "'"
                End If

                Wherecause = strW
            End If
            ' strSql = "select f02 as ItemCode,f02+' :: '+ f05 as ItemName from m00210 where f01='05' and f06 = 'SK01' and (f41 is null or cast(f41 as date) > cast(getdate() as date)) " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by f02"
			strSql = "select f02 as ItemCode,f02+' :: '+ f05 as ItemName from m00210 where f06 = 'SK01' and (f41 is null or cast(f41 as date) > cast(getdate() as date)) " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by f02"
            SetDropDownList2(SItem, EItem, strSql, "ItemName", "ItemCode", "--รหัสสินค้าทั้งหมด--")
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub SGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SGroup.SelectedIndexChanged
        If SGroup.SelectedIndex = 0 Then
            LoadSubGroup("")
            LoadItem("")
        Else
            LoadSubGroup("Where")
            LoadItem("Where")
        End If
    End Sub

    Protected Sub SsubGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SsubGroup.SelectedIndexChanged
        If SsubGroup.SelectedIndex = 0 Then
            LoadItem("")
        Else
            LoadItem("Where")
        End If
    End Sub

    Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click

        Dim url As String

        ' Dim currThead As Globalization.CultureInfo
        ' currThead = Thread.CurrentThread.CurrentCulture
        ' Dim culture As Globalization.CultureInfo = New Globalization.CultureInfo("en-US")
        'Dim date1 As Date = Convert.ToDateTime(SDate.Text, culture)
        'Dim date2 As Date = Convert.ToDateTime(EDate.Text, culture)

        'Dim mFromDate As String = date1.ToString("yyyy-MM-dd")
        'Dim mToDate As String = date2.ToString("yyyy-MM-dd")
        Dim mFromDate As String = SDate.Text
        Dim mToDate As String = EDate.Text
        Dim mArea As String = ""
        Dim mProv As String = ""
        Dim mFromBranch As String = ""
        Dim mToBranch As String = ""
        Dim mFromBranch2 As String = ""
        Dim mToBranch2 As String = ""
        Dim mFromGroup As String = ""
        Dim mToGroup As String = ""
        Dim mFromSubGroup As String = ""
        Dim mToSubGroup As String = ""
        Dim mFromItem As String = ""
        Dim mToItem As String = ""
        Dim mStatus As String = ""
        Dim mLogistic As Boolean = cbxLg.Checked

        'If (checkParameter() = 0) Then
        '    Return
        'End If

        If (cboArea.SelectedIndex = 0) Then
            mArea = "ALL"
        Else
            mArea = cboArea.SelectedValue.ToString
        End If

        If (cboProvince.SelectedIndex = 0) Then
            mProv = "ALL"
        Else
            mProv = cboProvince.SelectedValue.ToString
        End If

        If (cboSBranch.SelectedIndex = 0) Then
            mFromBranch = "ALL"
        Else
            mFromBranch = cboSBranch.SelectedValue.ToString
        End If

        If (cboEBranch.SelectedIndex = 0) Then
            mToBranch = "ALL"
        Else
            mToBranch = cboEBranch.SelectedValue.ToString
        End If

        If (cboSBranch2.SelectedIndex = 0) Then
            mFromBranch2 = "ALL"
        Else
            mFromBranch2 = cboSBranch2.SelectedValue.ToString
        End If

        If (cboEBranch2.SelectedIndex = 0) Then
            mToBranch2 = "ALL"
        Else
            mToBranch2 = cboEBranch2.SelectedValue.ToString
        End If

        If (SGroup.SelectedIndex = 0) Then
            mFromGroup = "ALL"
        Else
            mFromGroup = SGroup.SelectedValue.ToString
        End If

        If (SsubGroup.SelectedIndex = 0) Then
            mFromSubGroup = "ALL"
        Else
            mFromSubGroup = SsubGroup.SelectedValue.ToString
        End If

        If (SItem.SelectedIndex = 0) Then
            mFromItem = "ALL"
        Else
            mFromItem = SItem.SelectedValue.ToString
        End If

        If (EItem.SelectedIndex = 0) Then
            mToItem = "ALL"
        Else
            mToItem = EItem.SelectedValue.ToString
        End If

        If (rdbOpen.Checked = True) Then
            mStatus = "open"
        ElseIf (rdbClose.Checked = True) Then
            mStatus = "close"
        ElseIf (rdbAll.Checked = True) Then
            mStatus = "all"
        End If

        url = "rptTransferDetail.aspx?mFromDate=" & mFromDate _
               & "&mToDate=" & mToDate _
                & "&mArea=" & mArea _
                  & "&mProv=" & mProv _
                    & "&mFromBranch=" & mFromBranch _
                    & "&mToBranch=" & mToBranch _
                    & "&mFromBranch2=" & mFromBranch2 _
                    & "&mToBranch2=" & mToBranch2 _
                      & "&mFromGroup=" & mFromGroup _
                        & "&mFromSubGroup=" & mFromSubGroup _
                          & "&mFromItem=" & mFromItem _
                          & "&mToItem=" & mToItem _
                            & "&mStatus=" & mStatus _
                              & "&mLogistic=" & mLogistic

        ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('" + url + "');", True)

    End Sub

    Function checkParameter() As Integer

        If (cboArea.SelectedIndex = 0) Then
            ClientScript.RegisterStartupScript(Page.GetType, "PopupScript", "alert('กรุณาเลือกเขตที่ต้องการดูรายงาน');", True)
            Return 0
        End If

        If (cboProvince.SelectedIndex = 0) Then
            ClientScript.RegisterStartupScript(Page.GetType, "PopupScript", "alert('กรุณาเลือกจังหวัดที่ต้องการดูรายงาน');", True)
            Return 0
        End If

            Return 1

    End Function

    Protected Sub cbxLg_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLg.CheckedChanged
        If cboProvince.SelectedIndex = 0 Then
            LoadBranch("")
            LoadBranch2("")
        Else
            LoadBranch("Where")
            LoadBranch2("Where")
        End If
    End Sub
End Class
