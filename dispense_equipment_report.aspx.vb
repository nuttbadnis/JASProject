Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports System.Web.UI

Partial Class dispense_equipment_report
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Dim form_type As String

    Dim login As String

    Dim s_Date As String
    Dim e_Date As String
    Dim Company As String
    Dim RO As String
    Dim cluster As String
    Dim province As String
    Dim shop As String

    Dim mainItem As String
    Dim subItem As String
    Dim item As String

    Dim serial As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblServerTime.Text = "ข้อมูลที่ Server ณ. วันที่ " + Date.Today.Day.ToString("00") + "/" + Date.Today.Month.ToString("00") + "/" + Date.Today.Year.ToString("0000") + " เวลา " + Now.Hour.ToString("00") + ":" + Now.Minute.ToString("00")

        Dim DT As DataTable

        login = Session("Uemail").ToString
        s_Date = Request.QueryString("s_Date")
        e_Date = Request.QueryString("e_Date")
        Company = Request.QueryString("company")
        RO = Request.QueryString("RO")
        cluster = Request.QueryString("cluster")
        province = Request.QueryString("province")
        shop = Request.QueryString("shop")

        mainItem = Request.QueryString("mainItem")
        subItem = Request.QueryString("subItem")
        item = Request.QueryString("item")

        serial = Request.QueryString("serial")

        form_type = Request.QueryString("form_type")
        
        Dim strSql As String = ""

        If form_type = "where_item" Then
            strSql = where_item()
        ElseIf form_type = "where_serial" Then
            strSql = where_Serial()
        End If

        Try

            DT = C.GetDataTableRepweb(strSql)

            GridView1.DataSource = DT

            GridView1.DataBind()

        Catch ex As Exception
            lblError_view.Visible = True
            lblError.Text = "Page_Load -> get DT: " + ex.Message.ToString
        End Try

        Try
            If (DT.Rows.Count < 1) Or (DT Is Nothing) Then
                lblNoData.Visible = True
            End If
        Catch ex As Exception
            lblError_view.Visible = True
            lblError.Text = "Page_Load -> check DT: " + ex.Message.ToString
        End Try



    End Sub

    Private Function where_item()

        Dim DT As DataTable
        Dim strSql As String

        DT = C.GetDataTableRepweb("select F03 from m00860 with(nolocK) where f03 = 'ALL' and  f02 = '" + Session("email") + "'")

        strSql = "select DOC.Doc_date, SHOP.RO, SHOP.CLUSTER, SHOP.PROVINCE_NAME, SHOP.SHOP_CODE, SHOP.SHOP_NAME, DOC.Doc_Type, DOC.Doc_Code, DOC.Withdrawal_Type, DOC.Withdrawal_Type_Name, DOC.Referent, DOC.Item_Code, DOC.Item_Name, DOC.Serial " + vbCrLf
        strSql += "from( " + vbCrLf
        strSql += "	select shop.f03 RO, province.y04 CLUSTER, province.f03 PROVINCE_NAME, shop.f02 SHOP_CODE, shop.f04 SHOP_NAME " + vbCrLf
        strSql += "	from m00030 shop with(nolock) " + vbCrLf
        strSql += "	join m02020 province with(nolock) " + vbCrLf
        strSql += "	on shop.f12 = province.f02" + vbCrLf
        If DT.Rows.Count < 1 Then
            strSql += "	join m00860 U_POS with(nolock) " + vbCrLf
            strSql += "	on SHOP.f02 = U_POS.f03 " + vbCrLf
            strSql += "	and U_POS.f02 = '" + Session("email") + "' " + vbCrLf
        End If
        strSql += "	where isnull(shop.F20, cast(getdate() + 1 as date)) > cast(getdate() as date) " + vbCrLf
        If RO <> "" Then
            strSql += "	and shop.f03 = '" + RO + "' " + vbCrLf
        End If
        If cluster <> "" Then
            strSql += "	and province.y04 = '" + cluster + "' " + vbCrLf
        End If
        If province <> "" Then
            strSql += "	and province.f03 = '" + province + "' " + vbCrLf
        End If
        If shop <> "" Then
            strSql += "	and shop.f02 = '" + shop + "' " + vbCrLf
        End If
        strSql += ") SHOP " + vbCrLf
        strSql += "join " + vbCrLf
        strSql += "( " + vbCrLf
        strSql += "	/* OTC */ " + vbCrLf
        strSql += "	select main.f06 Doc_date, main.f01 Company, main.f02 Shop_Code, main.f04 Doc_Type, main.f05 Doc_Code, 'OTC' Withdrawal_Type, 'ชำระ OTC' Withdrawal_Type_Name, main.f35 Referent, detail.f07 Detail_Running, detail.f08 Item_Code, detail.f09 Item_Name, detail.f11 Item_Amount, serial.f07 Detail_Running_2, serial.f08 Running, serial.f09 Serial " + vbCrLf
        strSql += "	from r17520 main with(nolock) " + vbCrLf
        strSql += "	join r17524 detail with(nolock) " + vbCrLf
        strSql += "	on main.f05 = detail.f05 " + vbCrLf
        strSql += "	left join r17523 serial with(nolock) " + vbCrLf
        strSql += "	on main.f05 = serial.f05 " + vbCrLf
        strSql += "	and detail.f07 = serial.f07 " + vbCrLf
        strSql += "	join m00210 items with(nolock)" + vbCrLf
        strSql += "	on main.f01 = items.f01" + vbCrLf
        strSql += "	and detail.f08 = items.f02" + vbCrLf
        strSql += "	where main.f11 = '1' -- เอกสารอนุมัติ " + vbCrLf
        strSql += "	and detail.F14 = 1 -- เฉพาะที่เป็นอุปกร์ " + vbCrLf
        strSql += "	and left(detail.f08,2) <> 'IS' -- ตัดค้างจ่ายออก  " + vbCrLf
        strSql += "	and main.f01 = '" + Company + "' " + vbCrLf
        strSql += "	and main.f06 >= cast('" + s_Date + "' as date) " + vbCrLf
        strSql += "	and main.f06 <= cast('" + e_Date + "' as date) " + vbCrLf
        If shop <> "" Then
            strSql += "	and main.f02 = '" + shop + "' " + vbCrLf
        End If
        If mainItem <> "" Then
            strSql += "	and items.f08 = '" + mainItem + "' " + vbCrLf
        End If
        If subItem <> "" Then
            strSql += "	and items.f09 = '" + subItem + "' " + vbCrLf
        End If
        If item <> "" Then
            strSql += "	and items.f02 = '" + item + "' " + vbCrLf
        End If
        strSql += "" + vbCrLf
        strSql += "	union all" + vbCrLf
        strSql += "" + vbCrLf
        strSql += "	/* ใบเบิก */ " + vbCrLf
        strSql += "	select  main.f06 Doc_date, main.f01 Company, main.f02 Shop_Code, main.f04 Doc_Type, main.f05 Doc_Code, main.f11 Withdrawal_Type, wt.f03 Withdrawal_Type_Name, case when main.f11 in ('ISS04','ISS34') then '' else main.f19 end Referent, detail.f06 Detail_Running, detail.f25 Item_Code, detail.f26 Item_Name, detail.f10 Item_Amount, serial.f06 Detail_Running_2, serial.f07 Running, serial.f08 Serial " + vbCrLf
        strSql += "	from r01090 main with(nolock) " + vbCrLf
        strSql += "	join r01091 detail with(nolock) " + vbCrLf
        strSql += "	on main.f05 = detail.f05 " + vbCrLf
        strSql += "	left join r01095 serial with(nolocK) " + vbCrLf
        strSql += "	on main.f05 = serial.f05 " + vbCrLf
        strSql += "	and detail.f06 = serial.f06 " + vbCrLf
        strSql += "	join m00380 wt with(nolock) " + vbCrLf
        strSql += "	on main.f11 = wt.f02 " + vbCrLf
        strSql += "	join m00210 items with(nolock)" + vbCrLf
        strSql += "	on main.f01 = items.f01" + vbCrLf
        strSql += "	and detail.f25 = items.f02" + vbCrLf
        strSql += "	where main.f16 = '1' -- เอกสารอนุมัติ " + vbCrLf
        strSql += "	and detail.f24 = 1 -- รายการอุปกรณ์ " + vbCrLf
        strSql += "	and main.f01 = '" + Company + "' " + vbCrLf
        strSql += "	and main.f06 >= cast('" + s_Date + "' as date) " + vbCrLf
        strSql += "	and main.f06 <= cast('" + e_Date + "' as date) " + vbCrLf
        strSql += "	and main.f04 not in ('ARTNN') " + vbCrLf
        strSql += "	and main.f05 not in ( " + vbCrLf
        strSql += "		select f10 from r01090 where f04 in ('ARTNN') " + vbCrLf
        strSql += "	) " + vbCrLf
        If shop <> "" Then
            strSql += "	and main.f02 = '" + shop + "' " + vbCrLf
        End If
        If mainItem <> "" Then
            strSql += "	and items.f08 = '" + mainItem + "' " + vbCrLf
        End If
        If subItem <> "" Then
            strSql += "	and items.f09 = '" + subItem + "' " + vbCrLf
        End If
        If item <> "" Then
            strSql += "	and items.f02 = '" + item + "' " + vbCrLf
        End If
        strSql += "" + vbCrLf
        strSql += "	union all " + vbCrLf
        strSql += "" + vbCrLf
        strSql += "	/* ใบปรับลด */ " + vbCrLf
        strSql += "	select main.f06 Doc_date, main.f01 Company, main.f02 Shop_Code, main.f04 Doc_Type, main.f05 Doc_Code, detail.f22 Withdrawal_Type, wt.f03 Withdrawal_Type_Name, '' Referent, detail.f06 Detail_Running, detail.f08 Item_Code, detail.f10 Item_Name, detail.f18 Item_Amount, serial.f06 Detail_Running_2, serial.f07 Running, serial.f08 Serial " + vbCrLf
        strSql += "	from R01040 main with(nolock) " + vbCrLf
        strSql += "	join R01041 detail with(nolock) " + vbCrLf
        strSql += "	on main.f05 = detail.f05 " + vbCrLf
        strSql += "	left join r01045 serial with(nolocK) " + vbCrLf
        strSql += "	on main.f05 = serial.f05 " + vbCrLf
        strSql += "	and detail.f06 = serial.f06 " + vbCrLf
        strSql += "	join m00320 wt with(nolock) " + vbCrLf
        strSql += "	on detail.f22 = wt.f02 " + vbCrLf
        strSql += "	join m00210 items with(nolock)" + vbCrLf
        strSql += "	on main.f01 = items.f01" + vbCrLf
        strSql += "	and detail.f08 = items.f02" + vbCrLf
        strSql += "	where main.f15 = '1' -- เอกสารอนุมัติ " + vbCrLf
        strSql += "	and detail.f26 = 1 -- รายการอุปกรณ์ " + vbCrLf
        strSql += "	and main.f01 = '" + Company + "' " + vbCrLf
        strSql += "	and main.f06 >= cast('" + s_Date + "' as date) " + vbCrLf
        strSql += "	and main.f06 <= cast('" + e_Date + "' as date) " + vbCrLf
        If shop <> "" Then
            strSql += "	and main.f02 = '" + shop + "' " + vbCrLf
        End If
        If mainItem <> "" Then
            strSql += "	and items.f08 = '" + mainItem + "' " + vbCrLf
        End If
        If subItem <> "" Then
            strSql += "	and items.f09 = '" + subItem + "' " + vbCrLf
        End If
        If item <> "" Then
            strSql += "	and items.f02 = '" + item + "' " + vbCrLf
        End If
        strSql += "" + vbCrLf
        strSql += ") DOC " + vbCrLf
        strSql += "on SHOP.SHOP_CODE = DOC.Shop_Code " + vbCrLf
        strSql += "order by DOC.Doc_date, SHOP.RO, SHOP.CLUSTER, SHOP.PROVINCE_NAME, SHOP.SHOP_CODE, SHOP.SHOP_NAME, DOC.Doc_Type, DOC.Doc_Code " + vbCrLf
        'Response.Write(strSql)
        Return strSql

    End Function

    Private Function where_Serial()

        Dim strSql As String

        strSql = "select DOC.Doc_date, SHOP.RO, SHOP.CLUSTER, SHOP.PROVINCE_NAME, SHOP.SHOP_CODE, SHOP.SHOP_NAME, DOC.Doc_Type, DOC.Doc_Code, DOC.Withdrawal_Type, DOC.Withdrawal_Type_Name, DOC.Referent, DOC.Item_Code, DOC.Item_Name, DOC.Serial " + vbCrLf
        strSql += "from( " + vbCrLf
        strSql += "	select shop.f03 RO, province.y04 CLUSTER, province.f03 PROVINCE_NAME, shop.f02 SHOP_CODE, shop.f04 SHOP_NAME " + vbCrLf
        strSql += "	from m00030 shop with(nolock) " + vbCrLf
        strSql += "	join m02020 province with(nolock) " + vbCrLf
        strSql += "	on shop.f12 = province.f02" + vbCrLf
        strSql += "	where isnull(shop.F20, cast(getdate() + 1 as date)) > cast(getdate() as date) " + vbCrLf
        strSql += ") SHOP " + vbCrLf
        strSql += "join " + vbCrLf
        strSql += "( " + vbCrLf
        strSql += "	/* OTC */ " + vbCrLf
        strSql += "	select main.f06 Doc_date, main.f01 Company, main.f02 Shop_Code, main.f04 Doc_Type, main.f05 Doc_Code, 'OTC' Withdrawal_Type, 'ชำระ OTC' Withdrawal_Type_Name, main.f35 Referent, detail.f07 Detail_Running, detail.f08 Item_Code, detail.f09 Item_Name, detail.f11 Item_Amount, serial.f07 Detail_Running_2, serial.f08 Running, serial.f09 Serial " + vbCrLf
        strSql += "	from r17520 main with(nolock) " + vbCrLf
        strSql += "	join r17524 detail with(nolock) " + vbCrLf
        strSql += "	on main.f05 = detail.f05 " + vbCrLf
        strSql += "	left join r17523 serial with(nolock) " + vbCrLf
        strSql += "	on main.f05 = serial.f05 " + vbCrLf
        strSql += "	and detail.f07 = serial.f07 " + vbCrLf
        strSql += "	where main.f11 = '1' -- เอกสารอนุมัติ " + vbCrLf
        strSql += "	and detail.F14 = 1 -- เฉพาะที่เป็นอุปกร์ " + vbCrLf
        strSql += "	and left(detail.f08,2) <> 'IS' -- ตัดค้างจ่ายออก  " + vbCrLf
        strSql += "	and serial.f09 = '" + serial + "' " + vbCrLf
        strSql += "" + vbCrLf
        strSql += "	union all" + vbCrLf
        strSql += "" + vbCrLf
        strSql += "	/* ใบเบิก */ " + vbCrLf
        strSql += "	select  main.f06 Doc_date, main.f01 Company, main.f02 Shop_Code, main.f04 Doc_Type, main.f05 Doc_Code, main.f11 Withdrawal_Type, wt.f03 Withdrawal_Type_Name, case when main.f11 in ('ISS04','ISS34') then '' else main.f19 end Referent, detail.f06 Detail_Running, detail.f25 Item_Code, detail.f26 Item_Name, detail.f10 Item_Amount, serial.f06 Detail_Running_2, serial.f07 Running, serial.f08 Serial " + vbCrLf
        strSql += "	from r01090 main with(nolock) " + vbCrLf
        strSql += "	join r01091 detail with(nolock) " + vbCrLf
        strSql += "	on main.f05 = detail.f05 " + vbCrLf
        strSql += "	left join r01095 serial with(nolocK) " + vbCrLf
        strSql += "	on main.f05 = serial.f05 " + vbCrLf
        strSql += "	and detail.f06 = serial.f06 " + vbCrLf
        strSql += "	join m00380 wt with(nolock) " + vbCrLf
        strSql += "	on main.f11 = wt.f02 " + vbCrLf
        strSql += "	where main.f16 = '1' -- เอกสารอนุมัติ " + vbCrLf
        strSql += "	and detail.f24 = 1 -- รายการอุปกรณ์ " + vbCrLf
        strSql += "	and serial.f08 = '" + serial + "' " + vbCrLf
        strSql += "	and main.f04 not in ('ARTNN') " + vbCrLf
        strSql += "	and main.f05 not in ( " + vbCrLf
        strSql += "		select main2.f10 " + vbCrLf
        strSql += "		from r01090 main2 with(nolock) " + vbCrLf
        strSql += "		join r01091 detail2 with(nolock) " + vbCrLf
        strSql += "		on main2.f05 = detail2.f05 " + vbCrLf
        strSql += "		left join r01095 serial2 with(nolocK) " + vbCrLf
        strSql += "		on main2.f05 = serial2.f05 " + vbCrLf
        strSql += "		where serial2.f08 = serial.f08 " + vbCrLf
        strSql += "	) " + vbCrLf
        strSql += "" + vbCrLf
        strSql += "	union all " + vbCrLf
        strSql += "" + vbCrLf
        strSql += "	/* ใบปรับลด */ " + vbCrLf
        strSql += "	select main.f06 Doc_date, main.f01 Company, main.f02 Shop_Code, main.f04 Doc_Type, main.f05 Doc_Code, detail.f22 Withdrawal_Type, wt.f03 Withdrawal_Type_Name, '' Referent, detail.f06 Detail_Running, detail.f08 Item_Code, detail.f10 Item_Name, detail.f18 Item_Amount, serial.f06 Detail_Running_2, serial.f07 Running, serial.f08 Serial " + vbCrLf
        strSql += "	from R01040 main with(nolock) " + vbCrLf
        strSql += "	join R01041 detail with(nolock) " + vbCrLf
        strSql += "	on main.f05 = detail.f05 " + vbCrLf
        strSql += "	left join r01045 serial with(nolocK) " + vbCrLf
        strSql += "	on main.f05 = serial.f05 " + vbCrLf
        strSql += "	and detail.f06 = serial.f06 " + vbCrLf
        strSql += "	join m00320 wt with(nolock) " + vbCrLf
        strSql += "	on detail.f22 = wt.f02 " + vbCrLf
        strSql += "	where main.f15 = '1' -- เอกสารอนุมัติ " + vbCrLf
        strSql += "	and detail.f26 = 1 -- รายการอุปกรณ์ " + vbCrLf
        strSql += "	and serial.f08 = '" + serial + "' " + vbCrLf
        strSql += "" + vbCrLf
        strSql += ") DOC " + vbCrLf
        strSql += "on SHOP.SHOP_CODE = DOC.Shop_Code " + vbCrLf
        strSql += "order by DOC.Doc_date, SHOP.RO, SHOP.CLUSTER, SHOP.PROVINCE_NAME, SHOP.SHOP_CODE, SHOP.SHOP_NAME, DOC.Doc_Type, DOC.Doc_Code " + vbCrLf

        Return strSql

    End Function

End Class
