Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Partial Class pmtSumStock
    Inherits System.Web.UI.Page
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
                ' ddlCompany.SelectedValue = Request.Cookies("SumStock")("Company")
                 LoadArea()
                ' ddlSArea.SelectedValue = Request.Cookies("SumStock")("AreaFrom")

                 LoadProvince("Where")
                ' 'ddlSProvince.SelectedValue = Request.Cookies("SumStock")("ProvinceFrom")

                 LoadBranch("Where")
                ' 'ddlSBranch.SelectedValue = Request.Cookies("SumStock")("BranchFrom")
                ' 'ddlEBranch.SelectedValue = Request.Cookies("SumStock")("BranchTo")

                 LoadMainProduct()
                ' ddlSMain.SelectedValue = Request.Cookies("SumStock")("MainFrom")
                ' ddlEMain.SelectedValue = Request.Cookies("SumStock")("MainTo")


                 LoadSubProduct("Where")
                ' ddlSSub.SelectedValue = Request.Cookies("SumStock")("SSubFrom")
                ' ddlESub.SelectedValue = Request.Cookies("SumStock")("SSubTo")


                 LoadProduct("Where")
                ' ddlSProduct.SelectedValue = Request.Cookies("SumStock")("ProductFrom")
                ' ddlEProduct.SelectedValue = Request.Cookies("SumStock")("ProductTo")


                ' If Request.Cookies("SumStock")("Groupsum") = "rboArea" Then
                '     RadioButtonList1.SelectedIndex = 0
                ' ElseIf Request.Cookies("SumStock")("Groupsum") = "rboProvince" Then
                '     RadioButtonList1.SelectedIndex = 1
                ' ElseIf Request.Cookies("SumStock")("Groupsum") = "rboShop" Then
                '     RadioButtonList1.SelectedIndex = 2
                ' End If
               
            response.write("try")
            Catch ex As Exception
            response.write("end catch")
                ' Response.Cookies("SumStock").Expires = DateTime.MaxValue
                ' Response.Cookies("SumStock")("Login") = Session("Uemail").ToString
                ' Response.Cookies("SumStock")("Company") = ""
                ' Response.Cookies("SumStock")("AreaFrom") = ""
                ' Response.Cookies("SumStock")("ProvinceFrom") = ""
                ' Response.Cookies("SumStock")("BranchFrom") = ""
                ' Response.Cookies("SumStock")("BranchTo") = ""
                ' Response.Cookies("SumStock")("MainFrom") = ""
                ' Response.Cookies("SumStock")("MainTo") = ""
                ' Response.Cookies("SumStock")("SSubFrom") = ""
                ' Response.Cookies("SumStock")("SSubTo") = ""
                ' Response.Cookies("SumStock")("ProductFrom") = ""
                ' Response.Cookies("SumStock")("ProductTo") = ""
                ' Response.Cookies("SumStock")("Groupsum") = ""
           
            End Try
            '////////////////////////
        End If
    End Sub

    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        C.SetDropDownList(ddlCompany, vSql, "ComName", "ComCode")
    End Sub
    
    Private Sub LoadArea()
        vSql = CF.rSqlDDArea(Session("Uemail"),0)

        C.SetDropDownList(ddlSArea, vSql, "AreaName", "AreaCode", "---ทั้งหมด---")
        ' ถ้าสำนักงานเป็น ALL
        If ddlSArea.Items.Count = 1 Then
            vSql = CF.rSqlDDArea(Session("Uemail"),1)
            C.SetDropDownList(ddlSArea, vSql, "AreaName", "AreaCode", "---ทั้งหมด---")
        End If
        
        'Response.write(vSql)
    End Sub

    Private Sub LoadProvince(ByVal Wherecause As String)

        Dim strW As String = ""
        If Wherecause <> "" Then
            If ddlSArea.SelectedIndex <> 0 Then
                'ddlSProvince.Enabled = True
                strW = " o.f03 = '" & ddlSArea.SelectedValue.ToString & "'"
            End If
            Wherecause = strW
        End If
  
        vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(ddlSProvince, vSql, "ProvinceName", "ProvinceCode", "---ทั้งหมด---")
        ' ถ้าสำนักงานเป็น ALL
        If ddlSProvince.Items.Count = 1 Then
            vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(ddlSProvince, vSql, "ProvinceName", "ProvinceCode", "---ทั้งหมด---")
        End If
        'Response.Write(vSql)
    End Sub

    Private Sub LoadBranch(ByVal Wherecause As String)
        If Wherecause <> "" Then
            Dim strW As String = ""
            If ddlSArea.SelectedIndex <> 0 Then
                strW = "o.f03 = '" & ddlSArea.SelectedValue.ToString & "'"
            End If

            If ddlSProvince.SelectedIndex <> 0 Then
                ddlSBranch.Enabled = True
                ddlEBranch.Enabled = True
                If strW <> "" Then strW = strW + " and "
                strW = strW + "pv.f06 = '" & ddlSProvince.SelectedValue.ToString & "'"
            End If
            Wherecause = strW
        End If
        vSql = CF.rSqlDDShop(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(ddlSBranch, vSql, "ShopName", "ShopCode", "---ทั้งหมด---",ddlEBranch)
        If ddlSBranch.Items.Count = 1 Then
            vSql = CF.rSqlDDShop(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(ddlSBranch, vSql, "ShopName", "ShopCode", "---ทั้งหมด---",ddlEBranch)
        End If

    End Sub

    Private Sub LoadMainProduct()
        vSql = CF.rSqlDDMainProduct(ddlCompany.SelectedValue.ToString)
        C.SetDropDownList(ddlSMain, vSql, "GroupName", "GroupCode", "---ทั้งหมด---",ddlEMain)

        If ddlSMain.Items.Count = 1 Then
            vSql = CF.rSqlDDMainProduct(ddlCompany.SelectedValue.ToString)
            C.SetDropDownList(ddlSMain, vSql, "GroupName", "GroupCode", "---ทั้งหมด---",ddlEMain)
        End If
        ddlSMain.Enabled = True
        ddlEMain.Enabled = True
    End Sub
    
    Private Sub LoadSubProduct(ByVal WhereCause As String)
        Dim strW As String = ""
        If WhereCause <> "" Then
            If ddlSMain.SelectedIndex <> 0 Then
                strW = "sg.f02 >= '" & ddlSMain.SelectedValue.ToString & "'"
                ddlSSub.Enabled = True
                ddlESub.Enabled = True
            End If
            If ddlEMain.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "sg.f02 <= '" & ddlEMain.SelectedValue.ToString & "'"
            End If
            WhereCause = strW
        End If

        vSql = CF.rSqlDDSubProduct(ddlCompany.SelectedValue.ToString,Wherecause)
        C.SetDropDownList(ddlSSub, vSql, "SubGroupName", "SubGroupCode", "---ทั้งหมด---",ddlESub)
    End Sub

    Private Sub LoadProduct(ByVal WhereCause As String)
        Dim strSql As String
        If WhereCause <> "" Then
            Dim strW As String = ""
            If ddlSMain.SelectedIndex <> 0 Then
                strW = "sg.f02 >= '" & ddlSMain.SelectedValue.ToString & "'"
            End If
            If ddlEMain.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "sg.f02 <= '" & ddlEMain.SelectedValue.ToString & "'"
            End If

            If ddlSSub.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "sg.f03 >= '" & ddlSSub.SelectedValue.ToString & "'"
            End If
            If ddlESub.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "sg.f03 <= '" & ddlESub.SelectedValue.ToString & "'"
            End If
            WhereCause = strW
        End If
        vSql = CF.rSqlDDProduct(ddlCompany.SelectedValue.ToString,Wherecause)
        C.SetDropDownList(ddlSProduct, vSql, "StockName", "StockCode", "---ทั้งหมด---", ddlEProduct)

        ddlSProduct.Enabled = True
        ddlEProduct.Enabled = True
        'ddlSProduct.Width = 200
        ddlSProduct.Attributes.Add("STYLE", "word-wrap:break-word;left:0")
        Response.Write(vSql)
    End Sub

    Protected Sub ddlSArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSArea.SelectedIndexChanged
        If RadioButtonList1.Items(0).Selected Then
            ddlSProvince.Enabled = False
            ddlSBranch.Enabled = False
            ddlEBranch.Enabled = False
        Else
            LoadProvince("Where")
            LoadBranch("Where")
        End If
    End Sub

    Protected Sub ddlSProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSProvince.SelectedIndexChanged
        If RadioButtonList1.Items(1).Selected Then
            ddlSBranch.Enabled = False
            ddlEBranch.Enabled = False
        Else
            LoadBranch("Where")
        End If
    End Sub

    Protected Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        LoadMainProduct()
        LoadSubProduct("")
        LoadProduct("")
        ddlSMain.Enabled = True
        ddlEMain.Enabled = True
        ddlSProduct.Enabled = True
        ddlEProduct.Enabled = True
    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        If RadioButtonList1.Items(0).Selected Then
            ddlSProvince.Enabled = False

            ddlSBranch.Enabled = False
            ddlEBranch.Enabled = False

            LoadProvince("Where")
            LoadBranch("Where")

        End If
        If RadioButtonList1.Items(1).Selected Then
            ddlSProvince.Enabled = True

            ddlSBranch.Enabled = False
            ddlEBranch.Enabled = False

            LoadProvince("Where")
            LoadBranch("Where")

        End If
        If RadioButtonList1.Items(2).Selected Then
            ddlSProvince.Enabled = True

            ddlSBranch.Enabled = True
            ddlEBranch.Enabled = True

            LoadBranch("Where")
        End If
    End Sub

    Protected Sub ddlSMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSMain.SelectedIndexChanged
        LoadSubProduct("Where")
        LoadProduct("Where")
    End Sub

    Protected Sub ddlEMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEMain.SelectedIndexChanged
        LoadSubProduct("Where")
        LoadProduct("Where")
    End Sub

    Protected Sub ddlSSub_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSSub.SelectedIndexChanged
        LoadProduct("Where")
    End Sub

    Protected Sub ddlESub_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlESub.SelectedIndexChanged
        LoadProduct("Where")
    End Sub

    Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        ' If chkParameter.Checked = True Then
        '     Response.Cookies("SumStock").Expires = DateTime.MaxValue
        '     Response.Cookies("SumStock")("Login") = Session("Uemail").ToString
        '     Response.Cookies("SumStock")("Company") = ddlCompany.SelectedValue
        '     Response.Cookies("SumStock")("AreaFrom") = ddlSArea.SelectedValue
        '     Response.Cookies("SumStock")("BranchFrom") = ddlSBranch.SelectedValue
        '     Response.Cookies("SumStock")("BranchTo") = ddlEBranch.SelectedValue
        '     Response.Cookies("SumStock")("MainFrom") = ddlSMain.SelectedValue
        '     Response.Cookies("SumStock")("MainTo") = ddlEMain.SelectedValue
        '     Response.Cookies("SumStock")("SSubFrom") = ddlSSub.SelectedValue
        '     Response.Cookies("SumStock")("SSubTo") = ddlESub.SelectedValue
        '     Response.Cookies("SumStock")("ProductFrom") = ddlSProduct.SelectedValue
        '     Response.Cookies("SumStock")("ProductTo") = ddlEProduct.SelectedValue
        '     Response.Cookies("SumStock")("Groupsum") = RadioButtonList1.SelectedIndex
        ' Else
        '     Response.Cookies("SumStock").Expires = DateTime.MaxValue
        '     Response.Cookies("SumStock")("Login") = Session("Uemail").ToString
        '     Response.Cookies("SumStock")("Company") = ""
        '     Response.Cookies("SumStock")("AreaFrom") = ""
        '     Response.Cookies("SumStock")("BranchFrom") = ""
        '     Response.Cookies("SumStock")("BranchTo") = ""
        '     Response.Cookies("SumStock")("MainFrom") = ""
        '     Response.Cookies("SumStock")("MainTo") = ""
        '     Response.Cookies("SumStock")("SSubFrom") = ""
        '     Response.Cookies("SumStock")("SSubTo") = ""
        '     Response.Cookies("SumStock")("ProductFrom") = ""
        '     Response.Cookies("SumStock")("ProductTo") = ""
        '     Response.Cookies("SumStock")("Groupsum") = ""
        ' End If
       

        Dim SqlPreview As String
        SqlPreview = "com=" + ddlCompany.SelectedValue.ToString
        SqlPreview += DetailReport()
        If RadioButtonList1.Items(0).Selected Then
            SqlPreview += "&option=a"
        ElseIf RadioButtonList1.Items(1).Selected Then
            SqlPreview += "&option=p"
        Else
            SqlPreview += "&option=b"
        End If

        SendHeader()

        ClientScript.RegisterStartupScript(Page.GetType, "previewreport", "window.open('ReportSumStock.aspx?" + SqlPreview + "','_blank');", True)
    End Sub
    Private Function DetailReport() As String
        Dim sb As New StringBuilder
        sb.Remove(0, sb.Length)
        If RadioButtonList1.Items(0).Selected Then
            sb.Append("&sarea=" + IIf(ddlSArea.SelectedIndex = 0, "00", ddlSArea.SelectedValue.ToString))
            sb.Append("&sprovince=" + IIf(ddlSProvince.SelectedIndex = 0, "00", ddlSProvince.SelectedValue.ToString))
            sb.Append("&sbranch=" + IIf(ddlSBranch.SelectedIndex = 0, "00", ddlSBranch.SelectedValue.ToString))
            sb.Append("&ebranch=" + IIf(ddlEBranch.SelectedIndex = 0, "ZZ", ddlEBranch.SelectedValue.ToString))
        ElseIf RadioButtonList1.Items(1).Selected Then
            sb.Append("&sarea=" + IIf(ddlSArea.SelectedIndex = 0, "00", ddlSArea.SelectedValue.ToString))

            sb.Append("&sprovince=" + IIf(ddlSProvince.SelectedIndex = 0, "00", ddlSProvince.SelectedValue.ToString))

            sb.Append("&sbranch=" + IIf(ddlSBranch.SelectedIndex = 0, "00", ddlSBranch.SelectedValue.ToString))
            sb.Append("&ebranch=" + IIf(ddlEBranch.SelectedIndex = 0, "ZZ", ddlEBranch.SelectedValue.ToString))
        ElseIf RadioButtonList1.Items(2).Selected Then
            sb.Append("&sarea=" + IIf(ddlSArea.SelectedIndex = 0, "00", ddlSArea.SelectedValue.ToString))

            sb.Append("&sprovince=" + IIf(ddlSProvince.SelectedIndex = 0, "00", ddlSProvince.SelectedValue.ToString))

            sb.Append("&sbranch=" + IIf(ddlSBranch.SelectedIndex = 0, "00", ddlSBranch.SelectedValue.ToString))
            sb.Append("&ebranch=" + IIf(ddlEBranch.SelectedIndex = 0, "ZZ", ddlEBranch.SelectedValue.ToString))
        Else
            sb.Append("&sarea=" + IIf(ddlSArea.SelectedIndex = 0, "00", ddlSArea.SelectedValue.ToString))

            sb.Append("&sprovince=" + IIf(ddlSProvince.SelectedIndex = 0, "00", ddlSProvince.SelectedValue.ToString))

            sb.Append("&sbranch=" + IIf(ddlSBranch.SelectedIndex = 0, "00", ddlSBranch.SelectedValue.ToString))
            sb.Append("&ebranch=" + IIf(ddlEBranch.SelectedIndex = 0, "ZZ", ddlEBranch.SelectedValue.ToString))
        End If
        sb.Append("&smain=" + IIf(ddlSMain.SelectedIndex = 0, "00", ddlSMain.SelectedValue.ToString))
        sb.Append("&emain=" + IIf(ddlEMain.SelectedIndex = 0, "ZZ", ddlEMain.SelectedValue.ToString))
        sb.Append("&ssub=" + IIf(ddlSSub.SelectedIndex = 0, "00", ddlSSub.SelectedValue.ToString))
        sb.Append("&esub=" + IIf(ddlESub.SelectedIndex = 0, "ZZ", ddlESub.SelectedValue.ToString))
        sb.Append("&sproduct=" + IIf(ddlSProduct.SelectedIndex = 0, "00", ddlSProduct.SelectedValue.ToString))
        sb.Append("&eproduct=" + IIf(ddlEProduct.SelectedIndex = 0, "ZZ", ddlEProduct.SelectedValue.ToString))

        Return sb.ToString

    End Function
    Private Sub SendHeader()
        Dim vHeader As String = ""
        Dim vComName As String = ""
        Dim vAreaName As String = ""
        Dim SprovinceName As String = ""
        Dim EprovinceName As String = ""
        Dim SbranchName As String = ""
        Dim EbranchName As String = ""
        Dim SGroupName As String = ""
        Dim EGroupName As String = ""
        Dim SSubGroupName As String = ""
        Dim ESubGroupName As String = ""
        Dim SStockName As String = ""
        Dim EStockName As String = ""
        'If ddlCompany.SelectedIndex <> 0 Then
        vComName = Replace(ddlCompany.SelectedItem.ToString, " :: ", "/")
        vComName = Mid(vComName, InStr(Replace(ddlCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
        vHeader = vComName.ToString
        'End If
        vHeader += "|"

        If Not (ddlSArea.SelectedIndex = 0 Or ddlSArea.Items.Count = 0) Then
            vAreaName = Replace(ddlSArea.SelectedItem.ToString, " :: ", "/")
            vAreaName = Mid(vAreaName, InStr(Replace(ddlSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vAreaName))

            vHeader += " เขตธุรกิจที่ " + ddlSArea.SelectedValue.ToString

        Else
            vHeader += " เขตธุรกิจ ทั้งหมด "
        End If

        vHeader += "|"
        If Not (ddlSProvince.SelectedIndex = 0 Or ddlSProvince.Items.Count = 0) Then
            SprovinceName = Replace(ddlSProvince.SelectedItem.ToString, " :: ", "/")
            SprovinceName = Mid(SprovinceName, InStr(Replace(ddlSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SprovinceName))

            vHeader += " จังหวัด " + ddlSProvince.SelectedValue.ToString
        Else
            vHeader += " จังหวัด ทั้งหมด "
        End If

        vHeader += "|"
        If Not (ddlSBranch.SelectedIndex = 0 Or ddlSBranch.Items.Count = 0) Then
            SbranchName = Replace(ddlSBranch.SelectedItem.ToString, " :: ", "/")
            SbranchName = Mid(SbranchName, InStr(Replace(ddlSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SbranchName))

            vHeader += " จากสำนักงาน " + ddlSBranch.SelectedValue.ToString
        Else
            vHeader += " จากสำนักงาน ทั้งหมด "
        End If
        vHeader += "|"
        If Not (ddlEBranch.SelectedIndex = 0 Or ddlEBranch.Items.Count = 0) Then
            EbranchName = Replace(ddlEBranch.SelectedItem.ToString, " :: ", "/")
            EbranchName = Mid(EbranchName, InStr(Replace(ddlEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(EbranchName))

            vHeader += " ถึงสำนักงาน " + ddlEBranch.SelectedValue.ToString()
        Else
            vHeader += " ถึงสำนักงาน ทั้งหมด "
        End If
        vHeader += "|"
        If Not (ddlSMain.SelectedIndex = 0 Or ddlSMain.Items.Count = 0) Then
            SGroupName = Replace(ddlSMain.SelectedItem.ToString, " :: ", "/")
            SGroupName = Mid(SGroupName, InStr(Replace(ddlSMain.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SGroupName))
            vHeader += " จากกลุ่มหลัก " + ddlSMain.SelectedValue.ToString
        End If
        vHeader += "|"
        If Not (ddlEMain.SelectedIndex = 0 Or ddlEMain.Items.Count = 0) Then
            EGroupName = Replace(ddlEMain.SelectedItem.ToString, " :: ", "/")
            EGroupName = Mid(EGroupName, InStr(Replace(ddlEMain.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(EGroupName))
            vHeader += " ถึงกลุ่มหลัก " + ddlEMain.SelectedValue.ToString
        End If
        vHeader += "|"
        If Not (ddlSSub.SelectedIndex = 0 Or ddlSSub.Items.Count = 0) Then
            SSubGroupName = Replace(ddlSSub.SelectedItem.ToString, " :: ", "/")
            SSubGroupName = Mid(SSubGroupName, InStr(Replace(ddlSSub.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(SSubGroupName))

            vHeader += " จากกลุ่มย่อย " + ddlSSub.SelectedValue.ToString
        Else
            vHeader += " จากกลุ่มย่อย ทั้งหมด "
        End If
        vHeader += "|"
        If Not (ddlESub.SelectedIndex = 0 Or ddlESub.Items.Count = 0) Then
            ESubGroupName = Replace(ddlESub.SelectedItem.ToString, " :: ", "/")
            ESubGroupName = Mid(ESubGroupName, InStr(Replace(ddlESub.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(ESubGroupName))

            vHeader += " ถึงกลุ่มย่อย " + ddlESub.SelectedValue.ToString
        Else
            vHeader += " ถึงกลุ่มย่อย ทั้งหมด "
        End If
        vHeader += "|"
        If Not (ddlSProduct.SelectedIndex = 0 Or ddlSProduct.Items.Count = 0) Then

            vHeader += " จากรหัสสินค้า " + ddlSProduct.SelectedValue.ToString
        Else
            vHeader += " จากรหัสสินค้า ทั้งหมด "
        End If
        vHeader += "|"
        If Not (ddlEProduct.SelectedIndex = 0 Or ddlEProduct.Items.Count = 0) Then

            vHeader += " ถึงรหัสสินค้า " + ddlEProduct.SelectedValue.ToString
        Else
            vHeader += " ถึงรหัสสินค้า ทั้งหมด "
        End If
        vHeader += "|"

        Session("HeaderSale") = vHeader
    End Sub

    Protected Sub cmdExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExport.Click
        Dim SqlPreview As String
        SqlPreview = "com=" + ddlCompany.SelectedValue.ToString
        SqlPreview += DetailReport()


        If RadioButtonList1.Items(0).Selected Then
            SqlPreview += "&option=a"
        ElseIf RadioButtonList1.Items(1).Selected Then
            SqlPreview += "&option=p"
        Else
            SqlPreview += "&option=b"
        End If

        SendHeader()

        ClientScript.RegisterStartupScript(Page.GetType, "previewreport", "window.open('ReportSumStock.aspx?" + SqlPreview + "&exp=true','_blank');", True)
    End Sub

End Class


