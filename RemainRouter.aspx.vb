Imports System.Data
Partial Class RemainRouter
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
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

            LoadCompany()
            LoadArea()
            'LoadCluster("")
            LoadProvince("")
            LoadBranch("")
            LoadItem()

            '////////// Cookie ///////
            Try
                cboCompany.SelectedValue = Request.Cookies("RemainRouter")("Company")

                cboSArea.SelectedValue = Request.Cookies("RemainRouter")("AreaFrom")
                'cboEArea.SelectedValue = Request.Cookies("RemainRouter")("AreaTo")

                LoadProvince("Where")
                cboSProvince.SelectedValue = Request.Cookies("RemainRouter")("ProvinceFrom")
                cboEProvince.SelectedValue = Request.Cookies("RemainRouter")("ProvinceTo")

                'LoadCluster("Where")
                'cboSCluster.SelectedValue = Request.Cookies("RemainRouter")("ClusterFrom")
                'cboECluster.SelectedValue = Request.Cookies("RemainRouter")("ClusterTo")

                LoadBranch("Where")
                cboSBranch.SelectedValue = Request.Cookies("RemainRouter")("BranchFrom")
                cboEBranch.SelectedValue = Request.Cookies("RemainRouter")("BranchTo")

                LoadItem()
                cboSItem.SelectedValue = Request.Cookies("RemainRouter")("ItemFrom")
                cboEItem.SelectedValue = Request.Cookies("RemainRouter")("ItemTo")


                If Request.Cookies("RemainRouter")("GroupBy") = "rbtArea" Then
                    rbtArea.Checked = True
                ElseIf Request.Cookies("RemainRouter")("GroupBy") = "rbtProvince" Then
                    rbtProvince.Checked = True
                    'ElseIf Request.Cookies("RemainRouter")("GroupBy") = "rbtCluster" Then
                    '    rbtCluster.Checked = True
                ElseIf Request.Cookies("RemainRouter")("GroupBy") = "rbtDoc" Then
                    rbtDoc.Checked = True
                End If


            Catch ex As Exception
                Response.Cookies("RemainRouter").Expires = DateTime.MaxValue
                Response.Cookies("RemainRouter")("Login") = Session("Uemail")'.ToString
                Response.Cookies("RemainRouter")("Company") = ""
                Response.Cookies("RemainRouter")("AreaFrom") = ""
                Response.Cookies("RemainRouter")("AreaTo") = ""
                Response.Cookies("RemainRouter")("ProvinceFrom") = ""
                Response.Cookies("RemainRouter")("ProvinceTo") = ""
                'Response.Cookies("RemainRouter")("ClusterFrom") = ""
                'Response.Cookies("RemainRouter")("ClusterTo") = ""
                Response.Cookies("RemainRouter")("BranchFrom") = ""
                Response.Cookies("RemainRouter")("BranchTo") = ""
                Response.Cookies("RemainRouter")("ItemFrom") = ""
                Response.Cookies("RemainRouter")("ItemTo") = ""
                Request.Cookies("RemainRouter")("GroupBy") = ""
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
        C.SetDropDownList(cboCompany, vSql, "ComName", "ComCode")
    End Sub

    Private Sub LoadArea()
        vSql = CF.rSqlDDArea(Session("Uemail"),0)
        C.SetDropDownList(cboSArea, vSql, "AreaName", "AreaCode", "---ทั้งหมด---")
        ' ถ้าสำนักงานเป็น ALL
        If cboSArea.Items.Count = 1 Then
            vSql = CF.rSqlDDArea(Session("Uemail"),1)
            C.SetDropDownList(cboSArea, vSql, "AreaName", "AreaCode", "---ทั้งหมด---")
        End If
        'Response.write(vSql)
    End Sub
    
    Private Sub LoadCluster(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If cboSArea.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & cboSArea.SelectedValue.ToString & "'"
            End If

            Wherecause = strW
        End If
        vSql = CF.rSqlDDCluster_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(cboSCluster, vSql, "ClusterName", "ClusterCode", "--กรุณาเลือกครัสเตอร์--")
        ' ถ้าสำนักงานเป็น ALL
        If cboSCluster.Items.Count = 1 Then
            vSql = CF.rSqlDDCluster_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(cboSCluster, vSql, "ClusterName", "ClusterCode", "--กรุณาเลือกครัสเตอร์--")
        End If
        'Response.Write(vSql)
    End Sub

    Private Sub LoadProvince(ByVal Wherecause As String)
        Dim strW As String = ""
            If Wherecause <> "" Then
                If cboSArea.SelectedIndex <> 0 Then
                    strW = "o.f03 = '" & cboSArea.SelectedValue.ToString & "'"
                End If
                'If cboSCluster.SelectedIndex <> 0 Then
                '    If strW <> "" Then strW = strW + " and "
                '    strW = strW + "m02.y04 >= '" & cboSCluster.SelectedValue.ToString & "'"
                'End If
                'If cboECluster.SelectedIndex <> 0 Then
                '    If strW <> "" Then strW = strW + " and "
                '    strW = strW + "m02.y04 <= '" & cboECluster.SelectedValue.ToString & "'"
                'End If
                Wherecause = strW
            End If
        
        vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(cboSProvince, vSql, "ProvinceName", "ProvinceCode", "--กรุณาเลือกจังหวัด--",cboEProvince)
        ' ถ้าสำนักงานเป็น ALL
        If cboSProvince.Items.Count = 1 Then
            vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(cboSProvince, vSql, "ProvinceName", "ProvinceCode", "--กรุณาเลือกจังหวัด--",cboEProvince)
        End If
        'Response.Write(vSql)
    End Sub

    Private Sub LoadBranch(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If cboSArea.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & cboSArea.SelectedValue.ToString & "' "
            End If

            ' where province
            If cboSProvince.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " pv.F06 = '" & cboSProvince.SelectedValue.ToString & "'"
            End If
            If cboEProvince.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " pv.f06 <= '" & cboEProvince.SelectedValue.ToString & "'"
            End If

            Wherecause = strW
        End If

        vSql = CF.rSqlDDShop(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(cboSBranch, vSql, "ShopName", "ShopCode", "--กรุณาเลือกสำนักงาน--", cboEBranch)

        ' ถ้าสำนักงานเป็น ALL
        If cboSBranch.Items.Count = 1 Then
            vSql = CF.rSqlDDShop(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(cboSBranch, vSql, "ShopName", "ShopCode", "--กรุณาเลือกสำนักงาน--", cboEBranch)
        End If     
    End Sub

    Private Sub LoadItem()
        vSql = "select f02 as ItemCode, f02 + ' :: ' + f05 as ItemName from m00210 where left(f02,4) in ('ISMO','ISWL','ISFX','ISPR','ISGF','ISSI','ISTV','ISAC') and f01 = '" + cboCompany.SelectedValue.ToString + "' order by ItemName"
        C.SetDropDownList(cboSItem, vSql, "ItemName", "ItemCode", "--รหัสสินค้า--", cboEItem)
    End Sub

    Protected Sub cboSArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSArea.SelectedIndexChanged
        'LoadCluster("Where")
        LoadProvince("Where")
        LoadBranch("Where")
    End Sub

    'Protected Sub cboEArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEArea.SelectedIndexChanged
    '    LoadCluster("Where")
    '    LoadProvince("Where")
    'End Sub

    Protected Sub cboSCluster_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSCluster.SelectedIndexChanged
        LoadProvince("Where")
        LoadBranch("Where")
    End Sub

    Protected Sub cboECluster_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboECluster.SelectedIndexChanged
        LoadProvince("Where")
        LoadBranch("Where")
    End Sub
    'เมื่อเปลี่ยนจังหวัดให้โหลดสาขาขึ้นมาใหม่
    Protected Sub cboSProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSProvince.SelectedIndexChanged
        LoadBranch("Where")
    End Sub

    Protected Sub cboEProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEProvince.SelectedIndexChanged
        LoadBranch("Where")
    End Sub

    Private Sub ShowRpt()
        Try
            If chkParameter.Checked = True Then
                Response.Cookies("RemainRouter").Expires = DateTime.MaxValue
                Response.Cookies("RemainRouter")("Login") = Session("Uemail").ToString
                Response.Cookies("RemainRouter")("Company") = cboCompany.SelectedValue
                Response.Cookies("RemainRouter")("AreaFrom") = cboSArea.SelectedValue
                'Response.Cookies("RemainRouter")("AreaTo") = cboEArea.SelectedValue
                Response.Cookies("RemainRouter")("ProvinceFrom") = cboSProvince.SelectedValue
                Response.Cookies("RemainRouter")("ProvinceTo") = cboEProvince.SelectedValue
                'Response.Cookies("RemainRouter")("ClusterFrom") = cboSCluster.SelectedValue
                'Response.Cookies("RemainRouter")("ClusterTo") = cboECluster.SelectedValue
                Response.Cookies("RemainRouter")("BranchFrom") = cboSBranch.SelectedValue
                Response.Cookies("RemainRouter")("BranchTo") = cboEBranch.SelectedValue
                Response.Cookies("RemainRouter")("ItemFrom") = cboSItem.SelectedValue
                Response.Cookies("RemainRouter")("ItemTo") = cboEItem.SelectedValue
                If rbtArea.Checked = True Then
                    Request.Cookies("RemainRouter")("GroupBy") = "rbtArea"
                ElseIf rbtProvince.Checked = True Then
                    Request.Cookies("RemainRouter")("GroupBy") = "rbtProvince"
                    'ElseIf rbtCluster.Checked = True Then
                    '    Request.Cookies("RemainRouter")("GroupBy") = "rbtCluster"
                ElseIf rbtDoc.Checked = True Then
                    Request.Cookies("RemainRouter")("GroupBy") = "rbtDoc"
                End If


            Else

                Response.Cookies("RemainRouter")("Company") = ""
                Response.Cookies("RemainRouter")("AreaFrom") = ""
                Response.Cookies("RemainRouter")("AreaTo") = ""
                Response.Cookies("RemainRouter")("ProvinceFrom") = ""
                Response.Cookies("RemainRouter")("ProvinceTo") = ""
                'Response.Cookies("RemainRouter")("ClusterFrom") = ""
                'Response.Cookies("RemainRouter")("ClusterTo") = ""
                Response.Cookies("RemainRouter")("BranchFrom") = ""
                Response.Cookies("RemainRouter")("BranchTo") = ""
                Response.Cookies("RemainRouter")("ItemFrom") = ""
                Response.Cookies("RemainRouter")("ItemTo") = ""

                Request.Cookies("RemainRouter")("GroupBy") = ""
            End If



            Dim FromDate As String = SDate.Text
            Dim ToDate As String = EDate.Text
            Dim FromArea As String = "00"
            Dim ToArea As String = "ZZ"
            Dim FromProvince As String = "00"
            Dim ToProvince As String = "ZZ"
            'Dim FromCluster As String = "00"
            'Dim ToCluster As String = "ZZ"
            Dim FromBranch As String = "00000"
            Dim ToBranch As String = "ZZZZZ"
            Dim FromItem As String = cboSItem.Items(1).Value.ToString
            Dim ToItem As String = cboEItem.Items(cboEItem.Items.Count - 1).Value.ToString


            If cboSArea.SelectedIndex <> 0 Then
                FromArea = cboSArea.SelectedValue
            End If
            'If cboEArea.SelectedIndex <> 0 Then
            '    ToArea = cboEArea.SelectedValue
            'End If
            If cboSProvince.SelectedIndex <> 0 Then
                FromProvince = cboSProvince.SelectedValue
            End If
            If cboEProvince.SelectedIndex <> 0 Then
                ToProvince = cboEProvince.SelectedValue
            End If
            'If cboSCluster.SelectedIndex <> 0 Then
            '    FromCluster = cboSCluster.SelectedValue
            'End If
            'If cboECluster.SelectedIndex <> 0 Then
            '    ToCluster = cboECluster.SelectedValue
            'End If
            If cboSBranch.SelectedIndex <> 0 Then
                FromBranch = cboSBranch.SelectedValue
            End If
            If cboEBranch.SelectedIndex <> 0 Then
                ToBranch = cboEBranch.SelectedValue
            End If
            If cboSItem.SelectedIndex <> 0 Then
                FromItem = cboSItem.SelectedValue
            End If
            If cboEItem.SelectedIndex <> 0 Then
                ToItem = cboEItem.SelectedValue
            End If

            Dim vHeader As String = ""
            Dim Company As String = ""
            Dim SAreaName As String = ""
            Dim EAreaName As String = ""
            Dim SprovinceName As String = ""
            Dim EprovinceName As String = ""
            'Dim SClusterName As String = ""
            'Dim EClusterName As String = ""
            Dim SBranchName As String = ""
            Dim EBranchName As String = ""
            Dim SItemName As String = ""
            Dim EItemName As String = ""

            Company = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            Company = Mid(Company, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(Company))
            vHeader += Company.ToString
            vHeader += "|"
            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                'SAreaName = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                'SAreaName = Mid(SAreaName, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SAreaName))
                vHeader += "จาก" + SAreaName.ToString + " "
            Else
                vHeader += " จากเขตทั้งหมด "
            End If

            'If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
            '    EAreaName = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
            '    EAreaName = Mid(EAreaName, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(EAreaName))
            '    vHeader += "ถึง" + EAreaName.ToString + " "
            'Else
            '    vHeader += " ถึงเขตทั้งหมด "
            'End If
            'vHeader += "|"
            'If Not (cboSCluster.SelectedIndex = 0 Or cboSCluster.Items.Count = 0) Then
            '    'SClusterName = Replace(cboSCluster.SelectedItem.ToString, " :: ", "/")
            '    'SClusterName = Mid(SClusterName, InStr(Replace(cboSCluster.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SClusterName))
            '    vHeader += "จาก Cluster " + cboSCluster.SelectedValue.ToString + " "
            'Else
            '    vHeader += " จาก Cluster ทั้งหมด "
            'End If

            'If Not (cboECluster.SelectedIndex = 0 Or cboECluster.Items.Count = 0) Then
            '    'EClusterName = Replace(cboECluster.SelectedItem.ToString, " :: ", "/")
            '    'EClusterName = Mid(EClusterName, InStr(Replace(cboECluster.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(EClusterName))
            '    vHeader += "ถึง Cluster " + cboECluster.SelectedValue.ToString + " "
            'Else
            '    vHeader += " ถึง Cluster ทั้งหมด "
            'End If
            vHeader += "|"
            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                SprovinceName = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                SprovinceName = Mid(SprovinceName, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SprovinceName))
                vHeader += " จากจังหวัด" + SprovinceName.ToString + " "
            Else
                vHeader += " จากจังหวัดทั้งหมด "
            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                EprovinceName = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                EprovinceName = Mid(EprovinceName, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(EprovinceName))
                vHeader += " ถึงจังหวัด" + EprovinceName.ToString + " "
            Else
                vHeader += " ถึงจังหวัดทั้งหมด "
            End If
            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                SBranchName = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                SBranchName = Mid(SBranchName, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SBranchName))
                vHeader += " จากสาขา " + SBranchName.ToString + " "
            Else
                vHeader += " จากสาขาทั้งหมด "
            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                EBranchName = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                EBranchName = Mid(EBranchName, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(EBranchName))
                vHeader += " ถึงสาขา " + EBranchName.ToString + " "
            Else
                vHeader += " ถึงสาขาทั้งหมด "
            End If
            vHeader += "|"
            If Not (cboSItem.SelectedIndex = 0 Or cboSItem.Items.Count = 0) Then
                'SItemName = Replace(cboSItem.SelectedItem.ToString, " :: ", "/")
                'SItemName = Mid(SItemName, InStr(Replace(cboSItem.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SItemName))
                vHeader += " จากรหัสสินค้า " + cboSItem.SelectedValue.ToString + " "
            Else
                vHeader += " จากรหัสสินค้าทั้งหมด "
            End If
            If Not (cboEItem.SelectedIndex = 0 Or cboEItem.Items.Count = 0) Then
                'EItemName = Replace(cboEItem.SelectedItem.ToString, " :: ", "/")
                'EItemName = Mid(EItemName, InStr(Replace(cboEItem.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(EItemName))
                vHeader += " ถึงรหัสสินค้า " + cboEItem.SelectedValue.ToString + " "
            Else
                vHeader += " ถึงรหัสสินค้าทั้งหมด "
            End If
            vHeader += "|"
            If SDate.Text.Length <> 0 Then
                vHeader += " จากเอกสารวันที่ " + SDate.Text + " "
            End If
            If EDate.Text.Length <> 0 Then
                vHeader += " ถึงเอกสารวันที่ " + EDate.Text + " "
            End If
            vHeader += "|"
            If rbtArea.Checked = True Then
                vHeader += " Group By เขต "
                'ElseIf rbtCluster.Checked = True Then
                '    vHeader += " Group By Cluster "
            ElseIf rbtProvince.Checked = True Then
                vHeader += " Group By จังหวัด "
            ElseIf rbtDoc.Checked = True Then
                vHeader += " Group By เอกสาร "
            End If
            Session("HeaderRemainRouter") = vHeader

            Dim url As String
            url = "RemainRouterReport.aspx?"
            url += "Com=" + cboCompany.SelectedValue
            url += "&FromDate=" + SDate.Text
            url += "&ToDate=" + EDate.Text
            url += "&FromArea=" + FromArea
            url += "&ToArea=" + ToArea
            url += "&FromProvince=" + FromProvince
            url += "&ToProvince=" + ToProvince
            'url += "&FromCluster=" + FromCluster
            'url += "&ToCluster=" + ToCluster
            url += "&FromBranch=" + FromBranch
            url += "&ToBranch=" + ToBranch
            url += "&FromItem=" + FromItem
            url += "&ToItem=" + ToItem
            If rbtArea.Checked = True Then
                url += "&GroupBy=area"
                'ElseIf rbtCluster.Checked = True Then
                '    url += "&GroupBy=cluster"
            ElseIf rbtProvince.Checked = True Then
                url += "&GroupBy=province"
            ElseIf rbtDoc.Checked = True Then
                url += "&GroupBy=doc"
            End If

            'url += "&exp=false"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "open", "window.open('" + url + "','_blank');", True)

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        'If cboSArea.SelectedValue = "" Then
        '    Response.Write("<script type=""text/javascript"">alert(""โปรดเลือกเขต"");</script")
        '    Exit Sub
        'End If
        ShowRpt()
    End Sub

    Protected Sub rbtArea_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtArea.CheckedChanged
        If rbtArea.Checked = True Then
            'cboSCluster.SelectedIndex = 0
            'cboSCluster.Enabled = False
            'cboECluster.SelectedIndex = 0
            'cboECluster.Enabled = False
            cboSProvince.SelectedIndex = 0
            cboSProvince.Enabled = False
            cboEProvince.SelectedIndex = 0
            cboEProvince.Enabled = False
            cboSBranch.Enabled = False
            cboSBranch.SelectedIndex = 0
            cboEBranch.Enabled = False
            cboEBranch.SelectedIndex = 0
            'ElseIf rbtCluster.Checked = True Then
            '    cboSCluster.Enabled = True
            '    cboECluster.Enabled = True
            '    cboSProvince.SelectedIndex = 0
            '    cboSProvince.Enabled = False
            '    cboEProvince.SelectedIndex = 0
            '    cboEProvince.Enabled = False
            '    cboSBranch.Enabled = False
            '    cboSBranch.SelectedIndex = 0
            '    cboEBranch.Enabled = False
            '    cboEBranch.SelectedIndex = 0
        Else
            'cboSCluster.Enabled = True
            'cboECluster.Enabled = True
            cboSProvince.Enabled = True
            cboEProvince.Enabled = True
            cboSBranch.Enabled = True
            cboEBranch.Enabled = True
        End If
    End Sub

    Protected Sub rbtCluster_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCluster.CheckedChanged
        If rbtArea.Checked = True Then
            cboSCluster.SelectedIndex = 0
            cboSCluster.Enabled = False
            cboECluster.SelectedIndex = 0
            cboECluster.Enabled = False
            cboSProvince.SelectedIndex = 0
            cboSProvince.Enabled = False
            cboEProvince.SelectedIndex = 0
            cboEProvince.Enabled = False
            cboSBranch.Enabled = False
            cboSBranch.SelectedIndex = 0
            cboEBranch.Enabled = False
            cboEBranch.SelectedIndex = 0
        ElseIf rbtCluster.Checked = True Then
            cboSCluster.Enabled = True
            cboECluster.Enabled = True
            cboSProvince.SelectedIndex = 0
            cboSProvince.Enabled = False
            cboEProvince.SelectedIndex = 0
            cboEProvince.Enabled = False
            cboSBranch.Enabled = False
            cboSBranch.SelectedIndex = 0
            cboEBranch.Enabled = False
            cboEBranch.SelectedIndex = 0
        Else
            cboSCluster.Enabled = True
            cboECluster.Enabled = True
            cboSProvince.Enabled = True
            cboEProvince.Enabled = True
            cboSBranch.Enabled = True
            cboEBranch.Enabled = True
        End If
    End Sub

    Protected Sub rbtProvince_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtProvince.CheckedChanged
        If rbtArea.Checked = True Then
            'cboSCluster.SelectedIndex = 0
            'cboSCluster.Enabled = False
            'cboECluster.SelectedIndex = 0
            'cboECluster.Enabled = False
            cboSProvince.SelectedIndex = 0
            cboSProvince.Enabled = False
            cboEProvince.SelectedIndex = 0
            cboEProvince.Enabled = False
            cboSBranch.Enabled = False
            cboSBranch.SelectedIndex = 0
            cboEBranch.Enabled = False
            cboEBranch.SelectedIndex = 0
            'ElseIf rbtCluster.Checked = True Then
            '    cboSCluster.Enabled = True
            '    cboECluster.Enabled = True
            '    cboSProvince.SelectedIndex = 0
            '    cboSProvince.Enabled = False
            '    cboEProvince.SelectedIndex = 0
            '    cboEProvince.Enabled = False
            '    cboSBranch.Enabled = False
            '    cboSBranch.SelectedIndex = 0
            '    cboEBranch.Enabled = False
            '    cboEBranch.SelectedIndex = 0
        Else
            'cboSCluster.Enabled = True
            'cboECluster.Enabled = True
            cboSProvince.Enabled = True
            cboEProvince.Enabled = True
            cboSBranch.Enabled = True
            cboEBranch.Enabled = True
        End If
    End Sub

    Protected Sub rbtDoc_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDoc.CheckedChanged
        If rbtArea.Checked = True Then
            'cboSCluster.SelectedIndex = 0
            'cboSCluster.Enabled = False
            'cboECluster.SelectedIndex = 0
            'cboECluster.Enabled = False
            cboSProvince.SelectedIndex = 0
            cboSProvince.Enabled = False
            cboEProvince.SelectedIndex = 0
            cboEProvince.Enabled = False
            cboSBranch.Enabled = False
            cboSBranch.SelectedIndex = 0
            cboEBranch.Enabled = False
            cboEBranch.SelectedIndex = 0
            'ElseIf rbtCluster.Checked = True Then
            '    cboSCluster.Enabled = True
            '    cboECluster.Enabled = True
            '    cboSProvince.SelectedIndex = 0
            '    cboSProvince.Enabled = False
            '    cboEProvince.SelectedIndex = 0
            '    cboEProvince.Enabled = False
            '    cboSBranch.Enabled = False
            '    cboSBranch.SelectedIndex = 0
            '    cboEBranch.Enabled = False
            '    cboEBranch.SelectedIndex = 0
        Else
            'cboSCluster.Enabled = True
            'cboECluster.Enabled = True
            cboSProvince.Enabled = True
            cboEProvince.Enabled = True
            cboSBranch.Enabled = True
            cboEBranch.Enabled = True
        End If
    End Sub


    Protected Sub cboCompany_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCompany.SelectedIndexChanged
        LoadItem()
    End Sub
End Class
