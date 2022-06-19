Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports System.Web.UI

Partial Class in_out_items
    Inherits System.Web.UI.Page

    ' load class
    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Check PostBack
            If tbxSDate.Text <> "" then
                ClientScript.RegisterStartupScript(Page.GetType, "GetSDate", "$('#tbxSDate').val('"+tbxSDate.Text+"');", True)
            End If
            If tbxEDate.Text <> "" then
                ClientScript.RegisterStartupScript(Page.GetType, "GetEDate", "$('#tbxEDate').val('"+tbxEDate.Text+"');", True)
            End If

        If Not Page.IsPostBack Then

            ' โหลดข้อมูลเข้า DropDownList
            ' โหลดส่วนของบริษัท
            LoadCompany()

            '' ตั้งหัวบริษัทไว้ที่ 05
            'COMPANY_1.ClearSelection()
            'COMPANY_1.Items.FindByValue("05").Selected = True

            ' โหลดส่วนของสำนักงาน
            LoadArea()
            LoadCluster("")
            LoadProvince("")
            LoadShop("")

            ' โหลดส่วนของอุปกรณ์
            LoadMAIN_GROUP()
            LoadSUB_GROUP("")
            LoadITEM("")

        End If
    End Sub

    ' โหลดบริษัท
    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        C.SetDropDownList(COMPANY_1, vSql, "ComName", "ComCode")
    End Sub

    ' โหลด RO
    Private Sub LoadArea()
        vSql = CF.rSqlDDArea(Session("Uemail"),0)
        C.SetDropDownList(RO_1, vSql, "AreaName", "AreaCode", "--กรุณาเลือกเขตธุรกิจ--")
        ' ถ้าสำนักงานเป็น ALL
        If RO_1.Items.Count = 1 Then
            vSql = CF.rSqlDDArea(Session("Uemail"),1)
            C.SetDropDownList(RO_1, vSql, "AreaName", "AreaCode", "--กรุณาเลือกเขตธุรกิจ--")
        End If
        Response.Write(vSql)
        'Response.Write(RO_1.Items.Count)
        'RO_1.Items.IndexOf(3)
    End Sub

    ' โหลด CLUSTER
    Private Sub LoadCluster(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If RO_1.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & RO_1.SelectedValue.ToString & "'"
            End If

            Wherecause = strW
        End If
        vSql = CF.rSqlDDCluster_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(CLUSTER_1, vSql, "ClusterName", "ClusterCode", "--กรุณาเลือกครัสเตอร์--")
        ' ถ้าสำนักงานเป็น ALL
        If CLUSTER_1.Items.Count = 1 Then
            vSql = CF.rSqlDDCluster_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(CLUSTER_1, vSql, "ClusterName", "ClusterCode", "--กรุณาเลือกครัสเตอร์--")
        End If
        'Response.Write(vSql)
    End Sub

    ' โหลด จังหวัด
    Private Sub LoadProvince(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If RO_1.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & RO_1.SelectedValue.ToString & "' "
            End If

            ' where cluster
            If CLUSTER_1.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " pv.y04 = '" & CLUSTER_1.SelectedValue.ToString & "' "
            End If

            Wherecause = strW
        End If
        
        vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(PROVINCE_1, vSql, "ProvinceName", "ProvinceCode", "--กรุณาเลือกจังหวัด--")
        ' ถ้าสำนักงานเป็น ALL
        If PROVINCE_1.Items.Count = 1 Then
            vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(PROVINCE_1, vSql, "ProvinceName", "ProvinceCode", "--กรุณาเลือกจังหวัด--")
        End If
        'Response.Write(vSql)
    End Sub

    ' โหลด สำนักงาน
    Private Sub LoadShop(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            ' where ro
            If RO_1.SelectedIndex <> 0 Then
                strW = strW + " o.f03 = '" & RO_1.SelectedValue.ToString & "' "
            End If

            ' where cluster
            If CLUSTER_1.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " pv.y04 = '" & CLUSTER_1.SelectedValue.ToString & "' "
            End If

            ' where province
            If PROVINCE_1.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " pv.F06 = '" & PROVINCE_1.SelectedValue.ToString & "'"
            End If

            ' เมื่อเลือกแสดงเฉพาะสำนักงานที่เปิด
            If SHOP_ACTIVE_1.Checked = True Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " isnull(o.f20,cast(getdate()+1 as date)) > cast(getdate() as date) "
            End If

            Wherecause = strW
        End If

        vSql = CF.rSqlDDShop(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(SHOP_1, vSql, "ShopName", "ShopCode", "--กรุณาเลือกสำนักงาน--",SHOP_2)

        ' ถ้าสำนักงานเป็น ALL
        If SHOP_1.Items.Count = 1 Then
            vSql = CF.rSqlDDShop(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(SHOP_1, vSql, "ShopName", "ShopCode", "--กรุณาเลือกสำนักงาน--",SHOP_2)
        End If
        
    End Sub

    ' โหลดกลุ่มหลักอุปกรณ์
    Private Sub LoadMAIN_GROUP()
        vSql = CF.rSqlDDMainGroup(COMPANY_1.SelectedValue)
        C.SetDropDownList(MAIN_GROUP_1, vSql, "GroupName", "GroupCode", "--กลุ่มหลักสินค้า--")
    End Sub

    ' โหลดกลุ่มย่อยอุปกรณ์
    Private Sub LoadSUB_GROUP(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            If MAIN_GROUP_1.SelectedIndex <> 0 Then
                strW = strW + " M80.f02 = '" & MAIN_GROUP_1.SelectedValue.ToString & "' "
            End If

            Wherecause = strW
        End If

        vSql = CF.rSqlDDSubGroup(COMPANY_1.SelectedValue,Wherecause)
        C.SetDropDownList(SUB_GROUP_1, vSql, "subGroupName", "subGroupCode", "--กลุ่มย่อยสินค้า--")
    End Sub

    ' โหลดอุปกรณ์
    Private Sub LoadITEM(ByVal Wherecause As String)
        If Wherecause <> "" Then
            Dim strW As String = ""

            If MAIN_GROUP_1.SelectedIndex <> 0 Then
                strW = strW + " f08 = '" & MAIN_GROUP_1.SelectedValue.ToString & "'"
            End If

            If SUB_GROUP_1.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + " f09 = '" & SUB_GROUP_1.SelectedValue.ToString & "'"
            End If

            Wherecause = strW
        End If

        vSql = CF.rSqlDDItem(COMPANY_1.SelectedValue,Wherecause)

        C.SetDropDownList(ITEM_1, vSql, "ItemName", "ItemCode", "--รหัสสินค้า  :: ชื่อสินค้า--",ITEM_2)
    End Sub

    ' event เมื่อเลือก RO
    Protected Sub RO_1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RO_1.SelectedIndexChanged
        ' เปลี่ยนค่า ครัสเตอร์ จังหวัด สำนักงาน
        LoadCluster("Where")
        LoadProvince("Where")
        LoadShop("Where")

    End Sub

    ' event เมื่อเลือก CLUSTER
    Protected Sub CLUSTER_1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CLUSTER_1.SelectedIndexChanged
        ' เปลี่ยนค่า จังหวัด สำนักงาน
        LoadProvince("Where")
        LoadShop("Where")

    End Sub

    ' event เมื่อเลือก จังหวัด
    Protected Sub PROVINCE_1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PROVINCE_1.SelectedIndexChanged
        ' เปลี่ยนค่า สำนักงาน
        LoadShop("Where")

    End Sub

    ' event เมื่อเลือกกลุ่มหลัก
    Protected Sub MAIN_GROUP_1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MAIN_GROUP_1.SelectedIndexChanged
        ' เปลี่ยนค่า กลุ่มย่อย อุปกรณ์
        LoadSUB_GROUP("Where")
        LoadITEM("Where")
    End Sub

    Protected Sub SUB_GROUP_1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SUB_GROUP_1.SelectedIndexChanged
        ' เปลี่ยนค่า อุปกรณ์
        LoadITEM("Where")
    End Sub

    ' event เปลี่ยน บริษัท
    Protected Sub COMPANY_1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles COMPANY_1.SelectedIndexChanged
        ' เปลี่ยนค่า กลุ่มหลัก กลุ่มย่อย อุปกรณ์
        LoadMAIN_GROUP()
        LoadSUB_GROUP("Where")
        LoadITEM("Where")
    End Sub

    ' เมื่อกดดึงข้อมูล
    Protected Sub SEARCH_1_Click(ByVal sender As Object, ByVal e As System.EventArgs)' Handles SEARCH_1.Click

        ' ดึงค่าที่จะใช้ในการค้นหาข้อมูล
        ' วันที่ในการดึงข้อมูล
        Dim sDate As String = tbxSDate.Text.ToString()
        Dim eDate As String = tbxEDate.Text.ToString()

        ' สำนักงาน
        Dim company As String = COMPANY_1.SelectedValue.ToString()
        Dim ro As String = RO_1.SelectedValue.ToString()
        Dim cluster As String = CLUSTER_1.SelectedValue.ToString()
        Dim province As String = PROVINCE_1.SelectedValue.ToString()
        Dim shop1 As String = SHOP_1.SelectedValue.ToString()
        Dim shop2 As String = SHOP_2.SelectedValue.ToString()

        ' อุปกรณ์
        Dim mGroup As String = MAIN_GROUP_1.SelectedValue.ToString()
        Dim sGroup As String = SUB_GROUP_1.SelectedValue.ToString()
        Dim item1 As String = ITEM_1.SelectedValue.ToString()
        Dim item2 As String = ITEM_2.SelectedValue.ToString()

        ' แสดงสำนักงาน
        Dim shop_active As String = UCase(SHOP_ACTIVE_1.Checked.ToString)


        'ตรวจสอบข้อมูลวันที่
        If (Not IsDate(sDate) Or Not IsDate(eDate)) Then

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Window", "alert('ข้อมูลวันที่ไม่ถูกต้อง');", True)
            Return

        End If

        'เตรียมส่งข้อมูล
        Dim url As String = "in_out_items_rpt.aspx"
        url = url + "?p_sDate=" + sDate
        url = url + "&p_eDate=" + eDate
        url = url + "&p_company=" + company
        url = url + "&p_ro=" + ro
        url = url + "&p_cluster=" + cluster
        url = url + "&p_province=" + province
        url = url + "&p_shop_1=" + shop1
        url = url + "&p_shop_2=" + shop2
        url = url + "&p_mGroup=" + mGroup
        url = url + "&p_sGroup=" + sGroup
        url = url + "&p_item_1=" + item1
        url = url + "&p_item_2=" + item2
        url = url + "&p_shop_active=" + shop_active

        'เตรียมข้อมูลหัวรายงาน
        createHeader()
        'Response.Redirect(url)
        'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "openModal", "window.open('CertificatePrintViewAll.aspx' ,'_blank');", True)
        'preview.HRef = url
        'ส่งค่า url 
        'SEARCH_1.Click
        'Dim script As String = "window.open('" & url.ToString & "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');"
        'Dim script As String = "window.open('" & url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');"        
        'ClientScript.RegisterStartupScript(Page.GetType(), "openRequestedPopup('www.google.co.th', 'PromoteFirefoxWindow');", True)
        'ClientScript.RegisterStartupScript(Page.GetType, "open", "$('#aa').click();", True)
        'ClientScript.RegisterStartupScript(Page.GetType, "newwin", "test();", True)
        ClientScript.RegisterStartupScript(Page.GetType, "open", "$(document).on('keydown', function(e) {window.open('"+ url +"','_target', 'toolbar=yes, location=yes, status=yes, menubar=yes, scrollbars=yes');});", True)
        'ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('"+ url +"','Page','height=500,width=500');", True)

    End Sub

    ' เมื่อเลือกแสดงเฉพาะสำนักงานที่เปิด
    Protected Sub SHOP_ACTIVE_1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SHOP_ACTIVE_1.CheckedChanged
        ' เลือกแสดงสำนักงาน
        LoadShop("Where")
    End Sub

    Private Sub createHeader()

        Dim vHeader As String = ""

        'vHeader += "เขตธุรกิจที่ 05 จังหวัด LPG จากสำนักงาน LPG53 ถึงสำนักงาน LPG53 "
        'vHeader += "จากกลุ่มหลัก AC ถึงกลุ่มหลัก WL จากกลุ่มย่อย ทั้งหมด ถึงกลุ่มย่อย ทั้งหมด "
        'vHeader += "จากรหัสสินค้า ทั้งหมด ถึงรหัสสินค้า ทั้งหมด "
        'vHeader += "ข้อมูลที่ Server ณ. วันที่ 16/11/2020 เวลา 12:08 "

        vHeader += "ข้อมูลจากวันที่ " + tbxSDate.Text.ToString() + " "
        vHeader += "ถึงวันที่ " + tbxEDate.Text.ToString() + " "

        vHeader += "บริษัท " + IIf(COMPANY_1.SelectedValue.ToString() = "ALL", "ทั้งหมด", COMPANY_1.SelectedItem.ToString()) + " "
        vHeader += "เขต " + IIf(RO_1.SelectedValue.ToString() = "ALL", "ทั้งหมด", RO_1.SelectedItem.ToString()) + " "
        vHeader += "คลัสเตอร์ " + IIf(CLUSTER_1.SelectedValue.ToString() = "ALL", "ทั้งหมด", CLUSTER_1.SelectedItem.ToString()) + " "
        vHeader += "จังหวัด " + IIf(PROVINCE_1.SelectedValue.ToString() = "ALL", "ทั้งหมด", PROVINCE_1.SelectedItem.ToString()) + " "
        vHeader += "จากสำนักงาน " + IIf(SHOP_1.SelectedValue.ToString() = "ALL", "ทั้งหมด", SHOP_1.SelectedValue.ToString()) + " "
        vHeader += "ถึงสำนักงาน " + IIf(SHOP_2.SelectedValue.ToString() = "ALL", "ทั้งหมด", SHOP_2.SelectedValue.ToString()) + " "

        vHeader += "กลุ่มหลัก " + IIf(MAIN_GROUP_1.SelectedValue.ToString() = "ALL", "ทั้งหมด", MAIN_GROUP_1.SelectedValue.ToString()) + " "
        vHeader += "กลุ่มย่อย " + IIf(SUB_GROUP_1.SelectedValue.ToString() = "ALL", "ทั้งหมด", SUB_GROUP_1.SelectedValue.ToString()) + " "
        vHeader += "จากอุปกรณ์ " + IIf(ITEM_1.SelectedValue.ToString() = "ALL", "ทั้งหมด", ITEM_1.SelectedValue.ToString()) + " "
        vHeader += "ถึงอุปกรณ์ " + IIf(ITEM_2.SelectedValue.ToString() = "ALL", "ทั้งหมด", ITEM_2.SelectedValue.ToString()) + " "

        vHeader += "ข้อมูลที่ Server ณ. วันที่ " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm") + " "

        Session("ioiHeader") = vHeader

    End Sub

End Class
