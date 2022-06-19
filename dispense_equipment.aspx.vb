Imports System.Data
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports System.Web.UI
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Text
Imports System.Net


Partial Class dispense_equipment
    Inherits System.Web.UI.Page

    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session("email") = "varut.v"
        ' user for test
        'Session("email") = "muthita.c"

        If Not Page.IsPostBack Then
            SDate.Text = Date.Today.ToString("yyyy-MM-dd")
            EDate.Text = Date.Today.ToString("yyyy-MM-dd")

            LoadCompany()
            LoadArea("")
            LoadCluster("")
            LoadProvince("")
            LoadShop("")

            LoadMainItem()
            LoadSubItem("")
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

    Private Function getSqlShop() As String

        Dim strSql As String

        'strSql = "select distinct RO.f02 AreaCode " + vbCrLf
        'strSql += ", RO.f03 AreaName " + vbCrLf
        'strSql += ", PROVINCE.y04 Cluster " + vbCrLf
        'strSql += ", PROVINCE.f06 ProvinceCode " + vbCrLf
        'strSql += ", PROVINCE.f03 ProvinceName " + vbCrLf
        'strSql += ", SHOP.f02 ShopCode " + vbCrLf
        'strSql += ", SHOP.f04 ShopName " + vbCrLf

        strSql = "from m02300 RO with(nolock) " + vbCrLf
        strSql += "join m00030 SHOP with(nolock) " + vbCrLf
        strSql += "on SHOP.f03 = RO.f02 " + vbCrLf
        'strSql += "and isnull(SHOP.f20, cast(dateadd(day,1,getdate()) as date)) > cast(getdate() as date) -- เฉพาะสำนักงานที่เปิด" + vbCrLf
        strSql += "join m02020 PROVINCE with(nolock) " + vbCrLf
        strSql += "on SHOP.F12 = PROVINCE.f02 " + vbCrLf

        'strSql += "order by AreaCode, Cluster, ProvinceName, ShopCode " + vbCrLf

        Return strSql

    End Function

    Private Function getSqlItem() As String
        Dim strSql As String

        'strSql = "select i.f08 MAIN_GROUP " + vbCrLf
        'strSql += ", mg.f03 MAIN_GROUP_NAME " + vbCrLf
        'strSql += ", i.f09 SUB_GROUP " + vbCrLf
        'strSql += ", sg.f04 SUB_GROUP_NAME " + vbCrLf
        'strSql += ", i.f02 ITEM " + vbCrLf
        'strSql += ", i.f05 ITEM_NAME " + vbCrLf
        strSql = "from m00210 i with(nolock) " + vbCrLf
        strSql += "join M00160 g with(nolock) " + vbCrLf
        strSql += "on i.f01 = g.f01 and i.f07 = g.f02 " + vbCrLf
        strSql += "join M00170 mg with(nolock) " + vbCrLf
        strSql += "on i.f01 = mg.f01 and i.f08 = mg.f02 " + vbCrLf
        strSql += "join M00180 sg with(nolock) " + vbCrLf
        strSql += "on i.f01 = sg.f01 and i.f08 = sg.f02 and i.f09 = sg.f03 " + vbCrLf
        strSql += "join C09050 ic with(nolock) " + vbCrLf
        strSql += "on i.f06 = ic.f01 " + vbCrLf
        strSql += "where cast(isnull(i.F26, getdate()+1) as date) > cast(getdate() as date) -- ตัด ยกเลิกการขาย " + vbCrLf
        strSql += "and cast(isnull(i.F41, getdate()+1) as date) > cast(getdate() as date) -- ตัด ยกเลิกเคลื่อนไหว " + vbCrLf
        strSql += "and cast(isnull(i.F43, getdate()+1) as date) > cast(getdate() as date) -- ตัด ยกเลิกการใช้งาน " + vbCrLf
        strSql += "and i.f06 in ('SK01') -- สินค้าสำเร็จรูป " + vbCrLf
        strSql += "and left(i.f02,2) not in ('RT') -- ตัดสินค้า RT ออก " + vbCrLf

        Return strSql
    End Function

    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        Try
            C.SetDropDownListCompany(ddlCompany, vSql, "ComName", "ComCode")

        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message.ToString
        End Try

    End Sub

    Private Sub LoadArea(ByVal Wherecause As String)

        Try
            vSql = CF.rSqlDDArea(Session("Uemail"),0)
            C.SetDropDownList(ddlRO, vSql, "AreaName", "AreaCode", "ดูข้อมูลทั้งหมด")
            ' ถ้าสำนักงานเป็น ALL
            If ddlRO.Items.Count = 1 Then
                vSql = CF.rSqlDDArea(Session("Uemail"),1)
                C.SetDropDownList(ddlRO, vSql, "AreaName", "AreaCode", "ดูข้อมูลทั้งหมด")
            End If
            'ให้เลือกได้เฉพาะที่มีสำนักงานเปิดอยู่
            'strSql += "where isnull(SHOP.f20, cast(dateadd(day,1,getdate()) as date)) > cast(getdate() as date) " + vbCrLf
            'strSql += "order by AreaCode " + vbCrLf

        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "LoadArea: " + ex.Message.ToString
        End Try

    End Sub

    Private Sub LoadCluster(ByVal Wherecause As String)
        Try
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If ddlRO.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & ddlRO.SelectedValue.ToString & "'"
            End If

            Wherecause = strW
        End If
        vSql = CF.rSqlDDCluster_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(ddlCluster, vSql, "ClusterName", "ClusterCode", "--กรุณาเลือกครัสเตอร์--")
        ' ถ้าสำนักงานเป็น ALL
        If ddlCluster.Items.Count = 1 Then
            vSql = CF.rSqlDDCluster_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(ddlCluster, vSql, "ClusterName", "ClusterCode", "--กรุณาเลือกครัสเตอร์--")
        End If
            'ให้เลือกได้เฉพาะที่มีสำนักงานเปิดอยู่
            'strSql += "where isnull(SHOP.f20, cast(dateadd(day,1,getdate()) as date)) > cast(getdate() as date) " + vbCrLf

        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "LoadCluster: " + ex.Message.ToString
        End Try

    End Sub

    Private Sub LoadProvince(ByVal Wherecause As String)

        Try
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If ddlRO.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & ddlRO.SelectedValue.ToString & "' "
            End If

            ' where cluster
            If ddlCluster.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " pv.y04 = '" & ddlCluster.SelectedValue.ToString & "' "
            End If

            Wherecause = strW
        End If
        
        vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(ddlProvince, vSql, "ProvinceName", "ProvinceCode", "--กรุณาเลือกจังหวัด--")
        ' ถ้าสำนักงานเป็น ALL
        If ddlProvince.Items.Count = 1 Then
            vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(ddlProvince, vSql, "ProvinceName", "ProvinceCode", "--กรุณาเลือกจังหวัด--")
        End If

            'ให้เลือกได้เฉพาะที่มีสำนักงานเปิดอยู่
            'strSql += "where isnull(SHOP.f20, cast(dateadd(day,1,getdate()) as date)) > cast(getdate() as date) " + vbCrLf

        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "LoadProvince: " + ex.Message.ToString
        End Try
    End Sub

    Private Sub LoadShop(ByVal Wherecause As String)

        Dim strSql As String
        Dim DT As DataTable

        Try

            strSql = "select distinct SHOP.f02 ShopCode " + vbCrLf
            strSql += ", SHOP.f02 + ' :: ' +SHOP.f04 ShopName " + vbCrLf

            DT = C.GetDataTableRepweb("select F03 from m00860 with(nolocK) where f03 = 'ALL' and  f02 = '" + Session("email") + "'")

            If DT.Rows.Count > 0 Then
                strSql += getSqlShop()
            Else
                strSql += getSqlShop()
                ' strSql += "join m00860 U_POS with(nolock) " + vbCrLf
                ' strSql += "on SHOP.f02 = U_POS.f03 " + vbCrLf
                ' strSql += "and U_POS.f02 = '" + Session("email") + "' " + vbCrLf
            End If

            'ให้เลือกได้เฉพาะที่มีสำนักงานเปิดอยู่
            strSql += "where isnull(SHOP.f20, cast(dateadd(day,1,getdate()) as date)) > cast(getdate() as date) " + vbCrLf

            ' เมื่อมีการเลือก RO
            If Wherecause <> "" And ddlRO.SelectedIndex.ToString <> "0" Then
                strSql += "and RO.f02 = '" + ddlRO.SelectedValue.ToString + "'" + vbCrLf
            End If

            ' เมื่อมีการเลือก cluster
            If Wherecause <> "" And ddlCluster.SelectedIndex.ToString <> "0" Then
                strSql += "and PROVINCE.y04 = '" + ddlCluster.SelectedValue.ToString + "'" + vbCrLf
            End If

            ' เมื่อมีการเลือกจังหวัด
            If Wherecause <> "" And ddlProvince.SelectedIndex.ToString <> "0" Then
                strSql += "and PROVINCE.f06 = '" + ddlProvince.SelectedValue.ToString + "'" + vbCrLf
            End If

            strSql += "order by ShopCode " + vbCrLf

            SetDropDownList(ddlShop, strSql, "ShopName", "ShopCode", "ดูข้อมูลทั้งหมด")

        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "LoadShop: " + ex.Message.ToString
        End Try
    End Sub

    Private Sub LoadMainItem()
        Dim strSql As String

        Try
            strSql = "select i.f08 MAIN_GROUP " + vbCrLf
            strSql += ", i.f08 + ' :: ' + mg.f03 MAIN_GROUP_NAME  " + vbCrLf

            strSql += getSqlItem()

            strSql += "and i.f01 in ('" + ddlCompany.SelectedValue.ToString + "')" + vbCrLf

            strSql += "group by i.f08, mg.f03 " + vbCrLf
            strSql += "order by i.f08, mg.f03 " + vbCrLf

            SetDropDownList(ddlMainItem, strSql, "MAIN_GROUP_NAME", "MAIN_GROUP", "ดูข้อมูลทั้งหมด")
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "LoadMainItem: " + ex.Message.ToString
        End Try

    End Sub

    Private Sub LoadSubItem(ByVal Wherecause As String)
        Dim strSql As String

        Try
            strSql = "select i.f09 SUB_GROUP " + vbCrLf
            strSql += ", i.f09 + ' :: ' + sg.f04 SUB_GROUP_NAME " + vbCrLf

            strSql += getSqlItem()

            strSql += "and i.f01 in ('" + ddlCompany.SelectedValue.ToString + "')" + vbCrLf

            If ddlMainItem.SelectedIndex.ToString <> 0 Then
                strSql += "and i.f08 in ('" + ddlMainItem.SelectedValue.ToString + "')" + vbCrLf
            End If

            strSql += "group by i.f09, sg.f04 " + vbCrLf
            strSql += "order by i.f09, sg.f04 " + vbCrLf

            SetDropDownList(ddlSubItem, strSql, "SUB_GROUP_NAME", "SUB_GROUP", "ดูข้อมูลทั้งหมด")
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "LoadSubItem: " + ex.Message.ToString
        End Try

    End Sub

    Private Sub LoadItem(ByVal Wherecause As String)
        Dim strSql As String

        Try
            strSql = "select i.f02 ITEM " + vbCrLf
            strSql += ", i.f02 + ' :: ' + i.f05 ITEM_NAME " + vbCrLf

            strSql += getSqlItem()

            strSql += "and i.f01 in ('" + ddlCompany.SelectedValue.ToString + "')" + vbCrLf

            If ddlMainItem.SelectedIndex.ToString <> 0 Then
                strSql += "and i.f08 in ('" + ddlMainItem.SelectedValue.ToString + "')" + vbCrLf
            End If

            If ddlSubItem.SelectedIndex.ToString <> 0 Then
                strSql += "and i.f09 in ('" + ddlSubItem.SelectedValue.ToString + "')" + vbCrLf
            End If

            strSql += "group by i.f02, i.f05 " + vbCrLf
            strSql += "order by i.f02, i.f05 " + vbCrLf


            SetDropDownList(ddlItem, strSql, "ITEM_NAME", "ITEM", "ดูข้อมูลทั้งหมด")
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "LoadItem: " + ex.Message.ToString
        End Try

    End Sub

    Protected Sub ddlRO_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRO.SelectedIndexChanged
        LoadCluster("Where")
        LoadProvince("Where")
        LoadShop("Where")
    End Sub

    Protected Sub ddlCluster_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCluster.SelectedIndexChanged
        LoadProvince("Where")
        LoadShop("Where")
    End Sub

    Protected Sub ddlProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvince.SelectedIndexChanged
        LoadShop("Where")
    End Sub

    Protected Sub ddlShop_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShop.SelectedIndexChanged
        LoadMainItem()
    End Sub

    Protected Sub ddlMainItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMainItem.SelectedIndexChanged
        LoadSubItem("Where")
        LoadItem("Where")
    End Sub

    Protected Sub ddlSubItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubItem.SelectedIndexChanged
        LoadItem("Where")
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim form_type As String = "where_item"

        Dim s_Date As String
        Dim e_Date As String
        Dim company As String
        Dim RO As String
        Dim cluster As String
        Dim province As String
        Dim shop As String

        Dim mainItem As String
        Dim subItem As String
        Dim item As String

        Dim sendParameter As String

        Try

            s_Date = SDate.Text.Trim
            e_Date = EDate.Text.Trim
            company = ddlCompany.SelectedValue.ToString
            RO = ddlRO.SelectedValue.ToString
            cluster = ddlCluster.SelectedValue.ToString
            province = ddlProvince.SelectedValue.ToString
            shop = ddlShop.SelectedValue.ToString

            mainItem = ddlMainItem.SelectedValue.ToString
            subItem = ddlSubItem.SelectedValue.ToString
            item = ddlItem.SelectedValue.ToString


            sendParameter = "s_Date=" + s_Date + "&" + "e_Date=" + e_Date + "&" + "company=" + company + "&" + "RO=" + RO + "&" + "cluster=" + cluster + "&" + "province=" + province + "&" + "shop=" + shop + "&" + "mainItem=" + mainItem + "&" + "subItem=" + subItem + "&" + "item=" + item + "&" + "form_type=" + form_type

            ClientScript.RegisterStartupScript(Page.GetType, "open", "var win=window.open('dispense_equipment_report.aspx?" + sendParameter + "','_blank');win.focus();", True)


        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "btnSearch_Click: " + ex.Message.ToString
        End Try

    End Sub

    Protected Sub btnSearchSerial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchSerial.Click

        Dim form_type As String = "where_serial"

        Dim serial As String

        Dim sendParameter As String

        Try

            serial = txtSerial.Text.ToString

            sendParameter = "serial=" + serial + "&" + "form_type=" + form_type

            ClientScript.RegisterStartupScript(Page.GetType, "open", "var win=window.open('dispense_equipment_report.aspx?" + sendParameter + "','_blank');win.focus();", True)

        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "btnSearchSerial_Click: " + ex.Message.ToString
        End Try

    End Sub


End Class
