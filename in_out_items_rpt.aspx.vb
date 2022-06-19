Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports System.Web.UI

Partial Class in_out_items_rpt
    Inherits System.Web.UI.Page

    ' load class
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Session("ioiHeader") Is Nothing Then
        '    lblHeader.Text = "ข้อมูลอุปกรณ์ในระบบ POS"
        'Else
        '    lblHeader.Text = Session("ioiHeader")
        'End If

        ' เก็บค่า parameter ที่ส่งมาจากหน้า in_out_items_rpt
        Dim p_sDate As String = Request.QueryString("p_sDate")
        Dim p_eDate As String = Request.QueryString("p_eDate")
        Dim p_company As String = Request.QueryString("p_company")
        Dim p_ro As String = Request.QueryString("p_ro")
        Dim p_cluster As String = Request.QueryString("p_cluster")
        Dim p_province As String = Request.QueryString("p_province")
        Dim p_shop1 As String = Request.QueryString("p_shop_1")
        Dim p_shop2 As String = Request.QueryString("p_shop_2")
        Dim p_mGroup As String = Request.QueryString("p_mGroup")
        Dim p_sGroup As String = Request.QueryString("p_sGroup")
        Dim p_item1 As String = Request.QueryString("p_item_1")
        Dim p_item2 As String = Request.QueryString("p_item_2")
        Dim p_shop_active As String = Request.QueryString("p_shop_active")


        Dim Detail_Header As String = ""
        Detail_Header += "<span class='badge badge-primary m-1'>จากวันที่&nbsp; - &nbsp;ถึงวันที่ &nbsp; : &nbsp;" + p_sDate + "&nbsp; - &nbsp;" + p_eDate + "</span>"
        Detail_Header += "<span class='badge badge-warning m-1'>บริษํท &nbsp; : &nbsp;" + p_company + "</span>"
        Detail_Header += "<span class='badge badge-danger m-1'>เขต &nbsp; : &nbsp;" + p_ro + "</span>"
        Detail_Header += "<span class='badge badge-success m-1'>คลัสเตอร์ &nbsp; : &nbsp;" + p_cluster + "</span>"
        Detail_Header += "<span class='badge badge-secondary m-1'>จังหวัด &nbsp; : &nbsp;" + p_province + "</span>"
        Detail_Header += "<span class='badge badge-info m-1'>จากสำนักงาน&nbsp; - &nbsp;ถึงสำนักงาน &nbsp; : &nbsp;" + p_shop1 + "&nbsp; - &nbsp;" + p_shop2 + "</span>"
        Detail_Header += "<span class='badge badge-warning m-1'>กลุ่มหลักอุปกรณ์ &nbsp; : &nbsp;" + p_mGroup + "</span>"
        Detail_Header += "<span class='badge badge-dark m-1'>กลุ่มย่อยอุปกรณ์ &nbsp; : &nbsp;" + p_sGroup + "</span>"
        Detail_Header += "<span class='badge badge-info m-1'>จากรหัสอุปกรณ์ &nbsp; - >ถึงรหัสอุปกรณ์ &nbsp; : &nbsp;" + p_item1 + "&nbsp; - &nbsp;" + p_item2 + "</span>"
        Detail_Header += "<span class='badge badge-dark m-1'>สถานะ shop &nbsp; : &nbsp;" + p_shop_active + "</span>"

        lblHeader.Text = Detail_Header

        ' สร้าง Query
        Dim sql As String = ""

        ' ตัวแปรสำหรับแสดงข้อมูล
        Dim data_1 As DataTable = New DataTable()

        ' mem shop กับ อุปกรณ์
        Dim m_shop As String = ""
        Dim m_item As String = ""
        Dim table_shop As Table = New Table()
        Dim table_item As Table = New Table()

        ' เก็บค่ายอดรวม
        Dim m_balance As Integer = 0


        ' sql = sql + "" + vbCrLf
        sql = sql + "select '" + p_company + "' COMPANY " + vbCrLf
        sql = sql + ", SHOP.F03 RO " + vbCrLf
        sql = sql + ", PROV.Y04 CLUSTER " + vbCrLf
        sql = sql + ", PROV.F03 PROVINCE " + vbCrLf
        sql = sql + ", SHOP.F02 SHOP " + vbCrLf
        sql = sql + ", ITEM.F08 ITEM_GROUP " + vbCrLf
        sql = sql + ", ITEM.F09 ITEM_SUB_GROUP " + vbCrLf
        sql = sql + ", ITEM.f02 ITEM " + vbCrLf
        sql = sql + ", cast('" + p_sDate + "' as date) DATE_TIME " + vbCrLf
        sql = sql + ", '' DOC_GROUP " + vbCrLf
        sql = sql + ", 'ยอดยกมา' DOC " + vbCrLf
        sql = sql + ", '' DOC_TYPE " + vbCrLf
        sql = sql + ", '' REFERENT " + vbCrLf
        sql = sql + ", cast(sum(isnull(TCN.F18,0)) as int) RECEIVE_ITEM " + vbCrLf
        sql = sql + ", 0 PAY_ITEM " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "from M00030 SHOP with(nolock) " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "join M02020 PROV with(nolock) " + vbCrLf
        sql = sql + "on SHOP.f12 = PROV.f02 " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "join M00210 ITEM with(nolock) " + vbCrLf
        sql = sql + "on 1 = 1 " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "left join R01170 TCN with(nolock) " + vbCrLf
        sql = sql + "on SHOP.f02 = TCN.f02 " + vbCrLf
        sql = sql + "and TCN.F04 = ITEM.f02 " + vbCrLf
        sql = sql + " " + vbCrLf
        ' เงื่อนไข
        sql = sql + "where isnull(TCN.f03, dateadd(day, -1,'" + p_sDate + "')) < '" + p_sDate + "' " + vbCrLf
        sql = sql + "and ITEM.F01 = '" + p_company + "' " + vbCrLf
        sql = sql + "and ITEM.F07 <> 'SRV' " + vbCrLf
        sql = sql + "and isnull(ITEM.F43, dateadd(day, 1,'" + p_sDate + "')) > '" + p_sDate + "' -- ยกเลิกใช้งาน " + vbCrLf
        sql = sql + "and TCN.f01 = '" + p_company + "' " + vbCrLf

        If (p_ro <> "ALL") Then
            sql = sql + "and SHOP.f03 = '" + p_ro + "' " + vbCrLf
        End If

        If (p_cluster <> "ALL") Then
            sql = sql + "and PROV.y04 = '" + p_cluster + "' " + vbCrLf
        End If

        If (p_province <> "ALL") Then
            sql = sql + "and PROV.f06 = '" + p_province + "' " + vbCrLf
        End If

        If (p_shop1 <> "ALL") Then
            sql = sql + "and SHOP.f02 >= '" + p_shop1 + "' " + vbCrLf
        End If

        If (p_shop2 <> "ALL") Then
            sql = sql + "and SHOP.f02 <= '" + p_shop2 + "' " + vbCrLf
        End If

        If (p_mGroup <> "ALL") Then
            sql = sql + "and ITEM.f08 = '" + p_mGroup + "' " + vbCrLf
        End If

        If (p_sGroup <> "ALL") Then
            sql = sql + "and ITEM.f09 = '" + p_sGroup + "' " + vbCrLf
        End If

        If (p_item1 <> "ALL") Then
            sql = sql + "and ITEM.f02 >= '" + p_item1 + "' " + vbCrLf
        End If

        If (p_item2 <> "ALL") Then
            sql = sql + "and ITEM.f02 <= '" + p_item2 + "' " + vbCrLf
        End If

        If (p_shop_active = "TRUE") Then
            sql = sql + "and isnull(SHOP.f20,cast(getdate()+1 as date)) > cast(getdate() as date) " + vbCrLf
        End If

        sql = sql + " " + vbCrLf
        sql = sql + "group by TCN.F01, SHOP.F03, PROV.Y04, PROV.F03, SHOP.F02, ITEM.F08, ITEM.F09, ITEM.f02 " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "having cast(sum(isnull(TCN.F18,0)) as int) <> 0 " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + " union " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "select TCN.f01 COMPANY " + vbCrLf
        sql = sql + ", SHOP.F03 RO " + vbCrLf
        sql = sql + ", PROV.Y04 CLUSTER " + vbCrLf
        sql = sql + ", PROV.F03 PROVINCE " + vbCrLf
        sql = sql + ", SHOP.F02 SHOP " + vbCrLf
        sql = sql + ", ITEM.F08 ITEM_GROUP " + vbCrLf
        sql = sql + ", ITEM.F09 ITEM_SUB_GROUP " + vbCrLf
        sql = sql + ", TCN.F04 ITEM " + vbCrLf
        sql = sql + ", TCN.F03 DATE_TIME " + vbCrLf
        sql = sql + ", DOC.DOC_GROUP DOC_GROUP " + vbCrLf
        sql = sql + ", TCN.F13 DOC " + vbCrLf
        sql = sql + ", DOC.DOC_TYPE DOC_TYPE " + vbCrLf
        sql = sql + ", DOC.REFERENT REFERENT " + vbCrLf
        sql = sql + ", case when TCN.f18 > 0 then TCN.f18 else 0 end RECEIVE_ITEM " + vbCrLf
        sql = sql + ", case when TCN.f18 < 0 then TCN.f18 else 0 end PAY_ITEM " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "from ( " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** OTC */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, F35 REFERENT " + vbCrLf
        sql = sql + "    from R17520 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** ขายสด */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, F11 REFERENT " + vbCrLf
        sql = sql + "    from r11060 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** ลดหนี้ */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, F44 REFERENT " + vbCrLf
        sql = sql + "    from r11090 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** เบิก */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, F11 DOC_TYPE, F19 REFERENT " + vbCrLf
        sql = sql + "    from r01090 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** รับคืน */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, F18 REFERENT " + vbCrLf
        sql = sql + "    from r01010 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** ปรับปรุงเพิ่ม */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, '' REFERENT " + vbCrLf
        sql = sql + "    from r01030 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** ปรับปรุงลด */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, '' REFERENT " + vbCrLf
        sql = sql + "    from r01040 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** โอนออก */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, '' REFERENT " + vbCrLf
        sql = sql + "    from r01100 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** รับโอน */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, '' REFERENT " + vbCrLf
        sql = sql + "    from r01110 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    union all " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "    /** OPMC */ " + vbCrLf
        sql = sql + "    select F01 COMPANY, F02 SHOP, F06 DATE_TIME, F04 DOC_GROUP, F05 DOC, '' DOC_TYPE, '' REFERENT " + vbCrLf
        sql = sql + "    from r01210 with(nolock) " + vbCrLf
        sql = sql + "    where F06 >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "    and F06 <= '" + p_eDate + "' " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + ") DOC " + vbCrLf
        sql = sql + "join r01170 TCN with(nolock) " + vbCrLf
        sql = sql + "on DOC.SHOP = TCN.F02 " + vbCrLf
        sql = sql + "and DOC.DOC = TCN.F13 " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "join M00030 SHOP with(nolock) " + vbCrLf
        sql = sql + "on TCN.f02 = SHOP.f02 " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "join M02020 PROV with(nolock) " + vbCrLf
        sql = sql + "on SHOP.f12 = PROV.f02 " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "join M00210 ITEM with(nolock) " + vbCrLf
        sql = sql + "on TCN.F04 = ITEM.f02 " + vbCrLf
        sql = sql + " " + vbCrLf
        sql = sql + "where TCN.f10 <> 'TS' " + vbCrLf
        sql = sql + "and cast(TCN.f03 as date) >= '" + p_sDate + "' " + vbCrLf
        sql = sql + "and cast(TCN.f03 as date) <= '" + p_eDate + "' " + vbCrLf
        sql = sql + "and ITEM.F01 = '" + p_company + "' " + vbCrLf
        sql = sql + "and ITEM.F07 <> 'SRV' " + vbCrLf
        sql = sql + "and isnull(ITEM.F43, dateadd(day, 1,'" + p_sDate + "')) > '" + p_sDate + "' -- ยกเลิกใช้งาน " + vbCrLf
        sql = sql + "and TCN.f01 = '" + p_company + "' " + vbCrLf

        If (p_ro <> "ALL") Then
            sql = sql + "and SHOP.f03 = '" + p_ro + "' " + vbCrLf
        End If

        If (p_cluster <> "ALL") Then
            sql = sql + "and PROV.y04 = '" + p_cluster + "' " + vbCrLf
        End If

        If (p_province <> "ALL") Then
            sql = sql + "and PROV.f06 = '" + p_province + "' " + vbCrLf
        End If

        If (p_shop1 <> "ALL") Then
            sql = sql + "and SHOP.f02 >= '" + p_shop1 + "' " + vbCrLf
        End If

        If (p_shop2 <> "ALL") Then
            sql = sql + "and SHOP.f02 <= '" + p_shop2 + "' " + vbCrLf
        End If

        If (p_mGroup <> "ALL") Then
            sql = sql + "and ITEM.f08 = '" + p_mGroup + "' " + vbCrLf
        End If

        If (p_sGroup <> "ALL") Then
            sql = sql + "and ITEM.f09 = '" + p_sGroup + "' " + vbCrLf
        End If

        If (p_item1 <> "ALL") Then
            sql = sql + "and ITEM.f02 >= '" + p_item1 + "' " + vbCrLf
        End If

        If (p_item2 <> "ALL") Then
            sql = sql + "and ITEM.f02 <= '" + p_item2 + "' " + vbCrLf
        End If

        If (p_shop_active = "TRUE") Then
            sql = sql + "and isnull(SHOP.f20,cast(getdate()+1 as date)) > cast(getdate() as date) " + vbCrLf
        End If

        sql = sql + " " + vbCrLf
        sql = sql + "order by COMPANY, RO, CLUSTER, PROVINCE, SHOP, ITEM_GROUP, ITEM_SUB_GROUP, ITEM, DATE_TIME " + vbCrLf

        ' นำเข้าข้อมูลสู่ DataTable
        Try
            data_1 = C.GetDataTable(sql)
            'gvw_showDate_1.DataSource = data_1
            'gvw_showDate_1.DataBind()

        Catch ex As Exception
            ERROR_DIV.InnerHtml() = "<div class='col-3'>ERROR when get data to table: </div>"
            ERROR_DIV.InnerHtml() = "<div class='col-12'>" + ex.Message + "</div>"
        End Try

        For Each row As DataRow In data_1.Rows

            ' เมื่อมี shop เข้ามาใหม่
            If row.Item("Shop").ToString() <> m_shop Then

                ' ข้อมูล shop
                Dim shopName_data As DataTable = New DataTable()

                shopName_data = C.GetDataTable("select top 1 f02 + ' : ' + f04 as SHOP_NAME from M00030 where f02 = '" + row.Item("Shop").ToString() + "'")

                Dim t_row As TableRow = New TableRow()
                Dim cell As TableCell = New TableCell()

                ' สร้าง cell
                cell.Controls.Add(New LiteralControl(shopName_data.Rows(0).Item("SHOP_NAME").ToString()))
                ' กำหนด Span
                cell.ColumnSpan() = 16
                ' นำ cell ใส่ row
                t_row.Cells.Add(cell)
                ' เพิ่ม class ใน tr
                t_row.Attributes.Add("class", "bg-warning text-center")
                ' นำ row ใส่ tbe_show
                tbe_show.Rows.Add(t_row)


                ' เปลียนค่าจดจำ shop
                m_shop = row.Item("Shop").ToString()

            End If

            ' เมื่อเปลี่ยน item
            If row.Item("ITEM").ToString() <> m_item Then

                ' ข้อมูล shop
                Dim item_data As DataTable = New DataTable()

                item_data = C.GetDataTable("select top 1 f02 +' : '+F05 as ITEM_NAME from M00210 with(nolock) where f02 = '" + row.Item("ITEM").ToString() + "'")

                Dim t_row As TableRow = New TableRow()
                Dim cell As TableCell = New TableCell()

                ' สร้าง cell
                cell.Controls.Add(New LiteralControl(item_data.Rows(0).Item("ITEM_NAME").ToString()))
                ' กำหนด Span
                cell.ColumnSpan() = 16
                ' นำ cell ใส่ row
                t_row.Cells.Add(cell)
                ' เพิ่ม class ใน tr
                t_row.Attributes.Add("class", "bg-success text-white")
                ' นำ row ใส่ tbe_show
                tbe_show.Rows.Add(t_row)

                ' เปลียนค่าจดจำ item
                m_item = row.Item("ITEM").ToString()


                ' เตรียม table header
                Dim t_row_h As TableHeaderRow = New TableHeaderRow()
                Dim cell_h1 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h2 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h3 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h4 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h5 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h6 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h7 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h8 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h9 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h10 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h11 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h12 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h13 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h14 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h15 As TableHeaderCell = New TableHeaderCell()
                Dim cell_h16 As TableHeaderCell = New TableHeaderCell()

                cell_h1.Controls.Add(New LiteralControl("หัวบริษัท"))
                cell_h2.Controls.Add(New LiteralControl("เขต"))
                cell_h3.Controls.Add(New LiteralControl("ครัสเตอร์"))
                cell_h4.Controls.Add(New LiteralControl("จังหวัด"))
                cell_h5.Controls.Add(New LiteralControl("สำนักงาน"))
                cell_h6.Controls.Add(New LiteralControl("กล่มหลักอุปกรณ์"))
                cell_h7.Controls.Add(New LiteralControl("กลุ่มย่อยอุปกรณ์"))
                cell_h8.Controls.Add(New LiteralControl("รหัสอุปกรณ์"))
                cell_h9.Controls.Add(New LiteralControl("วันที่เวลาเอกสาร"))
                cell_h10.Controls.Add(New LiteralControl("กลุ่มเอกสาร"))
                cell_h11.Controls.Add(New LiteralControl("เอกสาร"))
                cell_h12.Controls.Add(New LiteralControl("ประเภทเอกสาร"))
                cell_h13.Controls.Add(New LiteralControl("อ้างอิง"))
                cell_h14.Controls.Add(New LiteralControl("รับเข้า"))
                cell_h15.Controls.Add(New LiteralControl("จ่ายออก"))
                cell_h16.Controls.Add(New LiteralControl("ยอดรวม"))

                t_row_h.Cells.Add(cell_h1)
                t_row_h.Cells.Add(cell_h2)
                t_row_h.Cells.Add(cell_h3)
                t_row_h.Cells.Add(cell_h4)
                t_row_h.Cells.Add(cell_h5)
                t_row_h.Cells.Add(cell_h6)
                t_row_h.Cells.Add(cell_h7)
                t_row_h.Cells.Add(cell_h8)
                t_row_h.Cells.Add(cell_h9)
                t_row_h.Cells.Add(cell_h10)
                t_row_h.Cells.Add(cell_h11)
                t_row_h.Cells.Add(cell_h12)
                t_row_h.Cells.Add(cell_h13)
                t_row_h.Cells.Add(cell_h14)
                t_row_h.Cells.Add(cell_h15)
                t_row_h.Cells.Add(cell_h16)
                ' เพิ่ม class ใน tr
                t_row_h.Attributes.Add("class", "bg-dark text-white text-center")

                tbe_show.Rows.Add(t_row_h)

                ' นับค่ายอดรวมใหม่เมื่อเปลี่ยนอุปกรณ์
                m_balance = 0

            End If

            ' เตรียมข้อมูลลง ตาราง
            Dim t_row_d As TableRow = New TableRow()
            Dim cell_d1 As TableCell = New TableCell()
            Dim cell_d2 As TableCell = New TableCell()
            Dim cell_d3 As TableCell = New TableCell()
            Dim cell_d4 As TableCell = New TableCell()
            Dim cell_d5 As TableCell = New TableCell()
            Dim cell_d6 As TableCell = New TableCell()
            Dim cell_d7 As TableCell = New TableCell()
            Dim cell_d8 As TableCell = New TableCell()
            Dim cell_d9 As TableCell = New TableCell()
            Dim cell_d10 As TableCell = New TableCell()
            Dim cell_d11 As TableCell = New TableCell()
            Dim cell_d12 As TableCell = New TableCell()
            Dim cell_d13 As TableCell = New TableCell()
            Dim cell_d14 As TableCell = New TableCell()
            Dim cell_d15 As TableCell = New TableCell()
            Dim cell_d16 As TableCell = New TableCell()

            cell_d1.Controls.Add(New LiteralControl(row.Item("COMPANY").ToString()))
            cell_d2.Controls.Add(New LiteralControl(row.Item("RO").ToString()))
            cell_d3.Controls.Add(New LiteralControl(row.Item("CLUSTER").ToString()))
            cell_d4.Controls.Add(New LiteralControl(row.Item("PROVINCE").ToString()))
            cell_d5.Controls.Add(New LiteralControl(row.Item("SHOP").ToString()))
            cell_d6.Controls.Add(New LiteralControl(row.Item("ITEM_GROUP").ToString()))
            cell_d7.Controls.Add(New LiteralControl(row.Item("ITEM_SUB_GROUP").ToString()))
            cell_d8.Controls.Add(New LiteralControl(row.Item("ITEM").ToString()))
            cell_d9.Controls.Add(New LiteralControl(row.Item("DATE_TIME").ToString()))
            cell_d10.Controls.Add(New LiteralControl(row.Item("DOC_GROUP").ToString()))
            cell_d11.Controls.Add(New LiteralControl(row.Item("DOC").ToString()))
            cell_d12.Controls.Add(New LiteralControl(row.Item("DOC_TYPE").ToString()))
            cell_d13.Controls.Add(New LiteralControl(row.Item("REFERENT").ToString()))
            cell_d14.Controls.Add(New LiteralControl(Int(row.Item("RECEIVE_ITEM").ToString())))
            cell_d15.Controls.Add(New LiteralControl(Int(row.Item("PAY_ITEM").ToString())))

            ' รวมยอด 
            m_balance = m_balance + Int(row.Item("RECEIVE_ITEM").ToString) + Int(row.Item("PAY_ITEM").ToString())

            cell_d16.Controls.Add(New LiteralControl(m_balance.ToString()))

            t_row_d.Cells.Add(cell_d1)
            t_row_d.Cells.Add(cell_d2)
            t_row_d.Cells.Add(cell_d3)
            t_row_d.Cells.Add(cell_d4)
            t_row_d.Cells.Add(cell_d5)
            t_row_d.Cells.Add(cell_d6)
            t_row_d.Cells.Add(cell_d7)
            t_row_d.Cells.Add(cell_d8)
            t_row_d.Cells.Add(cell_d9)
            t_row_d.Cells.Add(cell_d10)
            t_row_d.Cells.Add(cell_d11)
            t_row_d.Cells.Add(cell_d12)
            t_row_d.Cells.Add(cell_d13)
            t_row_d.Cells.Add(cell_d14)
            t_row_d.Cells.Add(cell_d15)
            t_row_d.Cells.Add(cell_d16)

            tbe_show.Rows.Add(t_row_d)


        Next row


    End Sub

End Class
