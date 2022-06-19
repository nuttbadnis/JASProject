Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Partial Class RemainRouterReport
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Private NumSale As Double = 0
    Private NumTake As Double = 0
    Private TotalAmount As Double = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim Company As String = Request.QueryString("Com")
            Dim FromDate As String = Request.QueryString("FromDate")
            Dim ToDate As String = Request.QueryString("ToDate")
            Dim FromArea As String = Request.QueryString("FromArea")
            Dim ToArea As String = Request.QueryString("ToArea")
            Dim FromProvince As String = Request.QueryString("FromProvince")
            Dim ToProvince As String = Request.QueryString("ToProvince")
            Dim FromCluster As String = Request.QueryString("FromCluster")
            Dim ToCluster As String = Request.QueryString("ToCluster")
            Dim FromBranch As String = Request.QueryString("FromBranch")
            Dim ToBranch As String = Request.QueryString("ToBranch")
            Dim FromItem As String = Request.QueryString("FromItem")
            Dim ToItem As String = Request.QueryString("ToItem")
            Dim GroupBy As String = Request.QueryString("GroupBy")

            Dim strSql As String = ""
            Dim DT As DataTable
            ShowHeader()
            If GroupBy = "area" Then
                
                strSql = "select area, itemcode, itemname, sum(QTY)'ขาย', sum(QTY2)'เบิก' " + vbCr
                strSql += "from ( " + vbCr
                strSql += "/* OTC */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '') Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , i1.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province'  " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , h.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),h.f06,111) 'DocDate' " + vbCr
                strSql += "    , case when h.f11 ='A' then 'ยกเลิกใบเสร็จ'else '' end 'Status' " + vbCr
                strSql += "    , case when h.f11 ='A' then 0 else c.f11  end 'QTY' " + vbCr
                strSql += "    , case when h.f11 ='A' then 0 else d.f26  end 'Amount' " + vbCr
                strSql += "    , h.f35 'Account' " + vbCr
                strSql += "    , h.F36 + ' ' + h.F37 + ' ' + h.f38 'CustomerName' " + vbCr
                strSql += "    from r17520 h with(nolock) -- เอกสาร OTC " + vbCr
                strSql += "    join r17521 d with(nolock) -- เอกสารรายการ " + vbCr
                strSql += "    on h.f05 =d.f05 -- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join r17524 c with(nolock) -- เอกสารรายการอุปกรณ์ " + vbCr
                strSql += "    on h.f05 =c.f05	-- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join m00030 b with(nolock) -- ข้อมูลสำนักงาน " + vbCr
                strSql += "    on b.f02 = h.f02 -- สำนักงานตรงกัน " + vbCr
                strSql += "    join m02020 m with(nolock) -- ข้อมูลจังหวัด " + vbCr
                strSql += "    on m.f02 =b.f12	-- เลขที่จังหวัดจรงกัน " + vbCr
                strSql += "    join m00210 i1 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on c.f08 = i1.f02 -- อุปกรณ์เอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where(C.f06 = d.f06) --ลำดับรายการตรงกับลำดับอุปกรณ์ " + vbCr
                strSql += "    and c.f08 >= '" + FromItem + "' " + vbCr
                strSql += "    and c.f08 <= '" + ToItem + "' " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and m.y04 >= '" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>= '" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>= '" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and h.f06 >='" + FromDate + "' -- วันที่เริ่มต้นในการค้นหาเอกสาร " + vbCr
                strSql += "    and h.f06 <='" + ToDate + "' -- วันที่สุดท้ายในการค้นหาเอกสาร " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += "Left Join " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select rd.f25 'Item2' -- อุปกรณ์ที่เบิก " + vbCr
                strSql += "    , i2.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , r.f05 'Doccode2' -- เอกสารที่เบิก " + vbCr
                strSql += "    , r.f23 'ref_doc' -- เอกสารอ้างอิง " + vbCr
                strSql += "    , convert(varchar(10),r.f06,111) 'DocDate2' -- วันทีเบิกตามเอกสาร " + vbCr
                strSql += "    , case when r.f04 in ('AISRN','AISSN','AISSV') then rd.f10 else (rd.f10*-1)  end 'QTY2' -- จำนวน " + vbCr
                strSql += "    from r01090 r with(nolock) -- เอกสารใบเบิก " + vbCr
                strSql += "    join r01091 rd with(nolock) -- รายการใบเบิก " + vbCr
                strSql += "    on r.f05 =rd.f05 -- เอกสารต้องเป็นตัวเดียวกัน " + vbCr
                strSql += "    join m00210 i2 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on rd.f25 = i2.f02 -- อุปกรณ์ในเอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where r.f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "    and rd.f24 <> 0 -- ตัดหมายเหตุรายการออก " + vbCr
                strSql += "    and r.F05 not in ( -- ตัดเอกสารที่มีการรับคืนออก " + vbCr
                strSql += "        Select f10 --อ้างอิงรับคืนเอกสารไหน " + vbCr
                strSql += "        from R01090 with(nolock) " + vbCr
                strSql += "        where f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "        and F04 = 'ARTNN' -- ดึงเฉพาะรับคืนอ้างใบเบิก " + vbCr
                strSql += "    ) " + vbCr
                strSql += "    and r.f06 >='" + FromDate + "' -- วันที่เริ้มต้นการค้นหา " + vbCr
                strSql += " " + vbCr
                strSql += ")) match_doc " + vbCr
                strSql += "on main_doc.Doccode = match_doc.ref_doc -- เอกสารขายตรงกับเอกสารอ้างอิงของใบเบิก " + vbCr
                strSql += "and main_doc.subgroup = match_doc.subgroup -- จับกลุ่มย่อยสินค้าประเภทเดียวกัน " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ลดหนี้ OTC */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '')Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster'" + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , db.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),db.f06,111) 'Docdate' " + vbCr
                strSql += "    , 'ลดหนี้ใบเสร็จ' status " + vbCr
                strSql += "    , c.f11*-1 'QTY' " + vbCr
                strSql += "    , d.f26*-1 'Amount' " + vbCr
                strSql += "    , '' 'Item2' " + vbCr
                strSql += "    , '' 'Doccode2' " + vbCr
                strSql += "    , '' 'Docdate2' " + vbCr
                strSql += "    , 0 'Qty2' " + vbCr
                strSql += "    , db.f44 'Account' " + vbCr
                strSql += "    , db.F53 + ' ' + db.F54 + ' ' + db.f55 'CustomerName' " + vbCr
                strSql += "    from r17520 h with(nolock) inner join r17521 d with(nolock) on h.f05 =d.f05 " + vbCr
                strSql += "    left join r17524 c with(nolock) on h.f05=c.f05 " + vbCr
                strSql += "    left join m00030 b with(nolock) on b.f02 =h.f02 " + vbCr
                strSql += "    left join m02020 m with(nolock) on m.f02 =b.f12 " + vbCr
                strSql += "    left join r11090 db with(nolock) on db.f10 =h.f05 " + vbCr
                strSql += "    where(db.f02 = b.f02) " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and c.f08 >='" + FromItem + "' and c.f08 <='" + ToItem + "' " + vbCr
                strSql += "    and m.y04 >='" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>='" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>='" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and db.f06 >='" + FromDate + "' " + vbCr
                strSql += "    and db.f06 <='" + ToDate + "' " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ขายสด */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '') Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select isnull(C.f08, d.F09) 'ItemCode' " + vbCr
                strSql += "    , isnull(c.f09, d.F13) 'ItemName' " + vbCr
                strSql += "    , i1.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , h.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),h.f06,111)'DocDate' " + vbCr
                strSql += "    , case when h.f63 ='A' then 'ยกเลิกใบเสร็จ'else '' end 'Status' " + vbCr
                strSql += "    , case when h.f63 ='A' then 0 else isNULL(c.f11, d.F24) end 'QTY' " + vbCr
                strSql += "    , case when h.f63 ='A' then 0 else d.f25 end 'Amount' " + vbCr
                strSql += "    , h.f11 'Account' " + vbCr
                strSql += "    , h.f45 'CustomerName' " + vbCr
                strSql += "    from r11060 h with(nolock) -- เอกสาร OTC " + vbCr
                strSql += "    join r11061 d with(nolock) -- เอกสารรายการ " + vbCr
                strSql += "    on h.f05 =d.f05 -- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    left join r11068 c with(nolock) -- เอกสารรายการอุปกรณ์ " + vbCr
                strSql += "    on h.f05 =c.f05	-- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join m00030 b with(nolock) -- ข้อมูลสำนักงาน " + vbCr
                strSql += "    on b.f02 = h.f02 -- สำนักงานตรงกัน " + vbCr
                strSql += "    join m02020 m with(nolock) -- ข้อมูลจังหวัด " + vbCr
                strSql += "    on m.f02 =b.f12	-- เลขที่จังหวัดจรงกัน " + vbCr
                strSql += "    join m00210 i1 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on isnull(c.f08, d.f09) = i1.f02 -- อุปกรณ์เอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where isnull(C.f06, d.f06) = d.f06 --ลำดับรายการตรงกับลำดับอุปกรณ์) " + vbCr
                strSql += "    and c.f08 >= '" + FromItem + "' " + vbCr
                strSql += "    and c.f08 <= '" + ToItem + "' " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and m.y04 >= '" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>= '" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>= '" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and convert(varchar(10),h.f06,111) >='" + FromDate + "' -- วันที่เริ่มต้นในการค้นหาเอกสาร " + vbCr
                strSql += "    and convert(varchar(10),h.f06,111) <='" + ToDate + "' -- วันที่สุดท้ายในการค้นหาเอกสาร " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += "Left Join " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select rd.f25 'Item2' -- อุปกรณ์ที่เบิก " + vbCr
                strSql += "    , i2.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , r.f05 'Doccode2' -- เอกสารที่เบิก " + vbCr
                strSql += "    , r.f23 'ref_doc' -- เอกสารอ้างอิง " + vbCr
                strSql += "    , convert(varchar(10),r.f06,111) 'DocDate2' -- วันทีเบิกตามเอกสาร " + vbCr
                strSql += "    , case when r.f04 in ('AISRN','AISSN','AISSV') then rd.f10 else (rd.f10*-1)  end 'QTY2' -- จำนวน " + vbCr
                strSql += "    from r01090 r with(nolock) -- เอกสารใบเบิก " + vbCr
                strSql += "    join r01091 rd with(nolock) -- รายการใบเบิก " + vbCr
                strSql += "    on r.f05 =rd.f05 -- เอกสารต้องเป็นตัวเดียวกัน " + vbCr
                strSql += "    join m00210 i2 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on rd.f25 = i2.f02 -- อุปกรณ์ในเอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where r.f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "    and rd.f24 <> 0 -- ตัดหมายเหตุรายการออก " + vbCr
                strSql += "    and r.F05 not in ( -- ตัดเอกสารที่มีการรับคืนออก " + vbCr
                strSql += "    Select f10 --อ้างอิงรับคืนเอกสารไหน " + vbCr
                strSql += "        from R01090 with(nolock) " + vbCr
                strSql += "        where f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "        and F04 = 'ARTNN' -- ดึงเฉพาะรับคืนอ้างใบเบิก " + vbCr
                strSql += "    ) " + vbCr
                strSql += "    and r.f06 >= '" + FromDate + "' -- วันที่เริ้มต้นการค้นหา " + vbCr
                strSql += " " + vbCr
                strSql += ") )match_doc " + vbCr
                strSql += "on main_doc.Doccode = match_doc.ref_doc -- เอกสารขายตรงกับเอกสารอ้างอิงของใบเบิก " + vbCr
                strSql += "and main_doc.subgroup = match_doc.subgroup -- จับกลุ่มย่อยสินค้าประเภทเดียวกัน " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ลดหนี้ขายสด */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '')Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , db.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),db.f06,111) 'Docdate' " + vbCr
                strSql += "    , 'ลดหนี้ใบเสร็จ' status " + vbCr
                strSql += "    , c.f11 * -1 'QTY' " + vbCr
                strSql += "    , d.f26 * -1 'Amount' " + vbCr
                strSql += "    , '' 'Item2' " + vbCr
                strSql += "    , '' 'Doccode2' " + vbCr
                strSql += "    , '' 'Docdate2' " + vbCr
                strSql += "    , 0 'Qty2' " + vbCr
                strSql += "    , db.f44 'Account' " + vbCr
                strSql += "    , db.F53 + ' ' + db.F54 + ' ' + db.f55 'CustomerName' " + vbCr
                strSql += "    from r11060 h with(nolock) " + vbCr
                strSql += "    join r11061 d with(nolock) on h.f05 = d.f05 " + vbCr
                strSql += "    left join r11068 c with(nolock) on h.f05 = c.f05	" + vbCr
                strSql += "    join m00030 b with(nolock) on b.f02 =h.f02 " + vbCr
                strSql += "    join m02020 m with(nolock) on m.f02 =b.f12 " + vbCr
                strSql += "    join r11090 db with(nolock) on db.f10 =h.f05 " + vbCr
                strSql += "    where(db.f02 = b.f02) " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and c.f08 >='" + FromItem + "' and c.f08 <='" + ToItem + "' " + vbCr
                strSql += "    and m.y04 >='" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>='" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>='" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and db.f06 >= '" + FromDate + "' " + vbCr
                strSql += "    and db.f06 <= '" + ToDate + "' " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += " " + vbCr

                strSql += ") sar " + vbCr
                strSql += "group by area, cluster, Province, itemcode, itemname  " + vbCr
                strSql += "order by area, Cluster, itemcode, itemname " + vbCr

                'Response.Write(strSql)
                DT = C.GetDataTableRepweb(strSql)
                GridView1.DataSource = DT
                GridView1.DataBind()
            ElseIf GroupBy = "cluster" Then

                strSql = "select area,cluster,itemcode,itemname,sum(QTY)'ขาย',sum(QTY2)'เบิก' " + vbCr
                strSql += "from ( " + vbCr
                strSql += "/* OTC */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '') Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , i1.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province'  " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , h.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),h.f06,111) 'DocDate' " + vbCr
                strSql += "    , case when h.f11 ='A' then 'ยกเลิกใบเสร็จ'else '' end 'Status' " + vbCr
                strSql += "    , case when h.f11 ='A' then 0 else c.f11  end 'QTY' " + vbCr
                strSql += "    , case when h.f11 ='A' then 0 else d.f26  end 'Amount' " + vbCr
                strSql += "    , h.f35 'Account' " + vbCr
                strSql += "    , h.F36 + ' ' + h.F37 + ' ' + h.f38 'CustomerName' " + vbCr
                strSql += "    from r17520 h with(nolock) -- เอกสาร OTC " + vbCr
                strSql += "    join r17521 d with(nolock) -- เอกสารรายการ " + vbCr
                strSql += "    on h.f05 =d.f05 -- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join r17524 c with(nolock) -- เอกสารรายการอุปกรณ์ " + vbCr
                strSql += "    on h.f05 =c.f05	-- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join m00030 b with(nolock) -- ข้อมูลสำนักงาน " + vbCr
                strSql += "    on b.f02 = h.f02 -- สำนักงานตรงกัน " + vbCr
                strSql += "    join m02020 m with(nolock) -- ข้อมูลจังหวัด " + vbCr
                strSql += "    on m.f02 =b.f12	-- เลขที่จังหวัดจรงกัน " + vbCr
                strSql += "    join m00210 i1 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on c.f08 = i1.f02 -- อุปกรณ์เอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where(C.f06 = d.f06) --ลำดับรายการตรงกับลำดับอุปกรณ์)" + vbCr
                strSql += "    and c.f08 >= '" + FromItem + "' " + vbCr
                strSql += "    and c.f08 <= '" + ToItem + "' " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and m.y04 >= '" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>= '" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>= '" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and h.f06 >='" + FromDate + "' -- วันที่เริ่มต้นในการค้นหาเอกสาร " + vbCr
                strSql += "    and h.f06 <='" + ToDate + "' -- วันที่สุดท้ายในการค้นหาเอกสาร " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += "Left Join " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select rd.f25 'Item2' -- อุปกรณ์ที่เบิก " + vbCr
                strSql += "    , i2.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , r.f05 'Doccode2' -- เอกสารที่เบิก " + vbCr
                strSql += "    , r.f23 'ref_doc' -- เอกสารอ้างอิง " + vbCr
                strSql += "    , convert(varchar(10),r.f06,111) 'DocDate2' -- วันทีเบิกตามเอกสาร " + vbCr
                strSql += "    , case when r.f04 in ('AISRN','AISSN','AISSV') then rd.f10 else (rd.f10*-1)  end 'QTY2' -- จำนวน " + vbCr
                strSql += "    from r01090 r with(nolock) -- เอกสารใบเบิก " + vbCr
                strSql += "    join r01091 rd with(nolock) -- รายการใบเบิก " + vbCr
                strSql += "    on r.f05 =rd.f05 -- เอกสารต้องเป็นตัวเดียวกัน " + vbCr
                strSql += "    join m00210 i2 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on rd.f25 = i2.f02 -- อุปกรณ์ในเอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where r.f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "    and rd.f24 <> 0 -- ตัดหมายเหตุรายการออก " + vbCr
                strSql += "    and r.F05 not in ( -- ตัดเอกสารที่มีการรับคืนออก " + vbCr
                strSql += "        Select f10 --อ้างอิงรับคืนเอกสารไหน " + vbCr
                strSql += "        from R01090 with(nolock) " + vbCr
                strSql += "        where f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "        and F04 = 'ARTNN' -- ดึงเฉพาะรับคืนอ้างใบเบิก " + vbCr
                strSql += "    ) " + vbCr
                strSql += "    and r.f06 >='" + FromDate + "' -- วันที่เริ้มต้นการค้นหา " + vbCr
                strSql += " " + vbCr
                strSql += ") match_doc " + vbCr
                strSql += "on main_doc.Doccode = match_doc.ref_doc -- เอกสารขายตรงกับเอกสารอ้างอิงของใบเบิก " + vbCr
                strSql += "and main_doc.subgroup = match_doc.subgroup -- จับกลุ่มย่อยสินค้าประเภทเดียวกัน " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ลดหนี้ OTC */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '')Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster'" + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , db.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),db.f06,111) 'Docdate' " + vbCr
                strSql += "    , 'ลดหนี้ใบเสร็จ' status " + vbCr
                strSql += "    , c.f11*-1 'QTY' " + vbCr
                strSql += "    , d.f26*-1 'Amount' " + vbCr
                strSql += "    , '' 'Item2' " + vbCr
                strSql += "    , '' 'Doccode2' " + vbCr
                strSql += "    , '' 'Docdate2' " + vbCr
                strSql += "    , 0 'Qty2' " + vbCr
                strSql += "    , db.f44 'Account' " + vbCr
                strSql += "    , db.F53 + ' ' + db.F54 + ' ' + db.f55 'CustomerName' " + vbCr
                strSql += "    from r17520 h with(nolock) inner join r17521 d with(nolock) on h.f05 =d.f05 " + vbCr
                strSql += "    left join r17524 c with(nolock) on h.f05=c.f05 " + vbCr
                strSql += "    left join m00030 b with(nolock) on b.f02 =h.f02 " + vbCr
                strSql += "    left join m02020 m with(nolock) on m.f02 =b.f12 " + vbCr
                strSql += "    left join r11090 db with(nolock) on db.f10 =h.f05 " + vbCr
                strSql += "    where(db.f02 = b.f02) " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and c.f08 >='" + FromItem + "' and c.f08 <='" + ToItem + "' " + vbCr
                strSql += "    and m.y04 >='" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>='" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>='" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and db.f06 >='" + FromDate + "' " + vbCr
                strSql += "    and db.f06 <='" + ToDate + "' " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ขายสด */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '') Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select isnull(C.f08, d.F09) 'ItemCode' " + vbCr
                strSql += "    , isnull(c.f09, d.F13) 'ItemName' " + vbCr
                strSql += "    , i1.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , h.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),h.f06,111)'DocDate' " + vbCr
                strSql += "    , case when h.f63 ='A' then 'ยกเลิกใบเสร็จ'else '' end 'Status' " + vbCr
                strSql += "    , case when h.f63 ='A' then 0 else isNULL(c.f11, d.F24) end 'QTY' " + vbCr
                strSql += "    , case when h.f63 ='A' then 0 else d.f25 end 'Amount' " + vbCr
                strSql += "    , h.f11 'Account' " + vbCr
                strSql += "    , h.f45 'CustomerName' " + vbCr
                strSql += "    from r11060 h with(nolock) -- เอกสาร OTC " + vbCr
                strSql += "    join r11061 d with(nolock) -- เอกสารรายการ " + vbCr
                strSql += "    on h.f05 =d.f05 -- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    left join r11068 c with(nolock) -- เอกสารรายการอุปกรณ์ " + vbCr
                strSql += "    on h.f05 =c.f05	-- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join m00030 b with(nolock) -- ข้อมูลสำนักงาน " + vbCr
                strSql += "    on b.f02 = h.f02 -- สำนักงานตรงกัน " + vbCr
                strSql += "    join m02020 m with(nolock) -- ข้อมูลจังหวัด " + vbCr
                strSql += "    on m.f02 =b.f12	-- เลขที่จังหวัดจรงกัน " + vbCr
                strSql += "    join m00210 i1 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on isnull(c.f08, d.f09) = i1.f02 -- อุปกรณ์เอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where isnull(C.f06, d.f06) = d.f06 --ลำดับรายการตรงกับลำดับอุปกรณ์) " + vbCr
                strSql += "    and c.f08 >= '" + FromItem + "' " + vbCr
                strSql += "    and c.f08 <= '" + ToItem + "' " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and m.y04 >= '" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>= '" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>= '" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and h.f06 >='" + FromDate + "' -- วันที่เริ่มต้นในการค้นหาเอกสาร " + vbCr
                strSql += "    and h.f06 <='" + ToDate + "' -- วันที่สุดท้ายในการค้นหาเอกสาร " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += "Left Join " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select rd.f25 'Item2' -- อุปกรณ์ที่เบิก " + vbCr
                strSql += "    , i2.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , r.f05 'Doccode2' -- เอกสารที่เบิก " + vbCr
                strSql += "    , r.f23 'ref_doc' -- เอกสารอ้างอิง " + vbCr
                strSql += "    , convert(varchar(10),r.f06,111) 'DocDate2' -- วันทีเบิกตามเอกสาร " + vbCr
                strSql += "    , case when r.f04 in ('AISRN','AISSN','AISSV') then rd.f10 else (rd.f10*-1)  end 'QTY2' -- จำนวน " + vbCr
                strSql += "    from r01090 r with(nolock) -- เอกสารใบเบิก " + vbCr
                strSql += "    join r01091 rd with(nolock) -- รายการใบเบิก " + vbCr
                strSql += "    on r.f05 =rd.f05 -- เอกสารต้องเป็นตัวเดียวกัน " + vbCr
                strSql += "    join m00210 i2 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on rd.f25 = i2.f02 -- อุปกรณ์ในเอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where r.f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "    and rd.f24 <> 0 -- ตัดหมายเหตุรายการออก " + vbCr
                strSql += "    and r.F05 not in ( -- ตัดเอกสารที่มีการรับคืนออก " + vbCr
                strSql += "    Select f10 --อ้างอิงรับคืนเอกสารไหน " + vbCr
                strSql += "        from R01090 with(nolock) " + vbCr
                strSql += "        where f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "        and F04 = 'ARTNN' -- ดึงเฉพาะรับคืนอ้างใบเบิก " + vbCr
                strSql += "    ) " + vbCr
                strSql += "    and r.f06 >= '" + FromDate + "' -- วันที่เริ้มต้นการค้นหา " + vbCr
                strSql += " " + vbCr
                strSql += ") match_doc " + vbCr
                strSql += "on main_doc.Doccode = match_doc.ref_doc -- เอกสารขายตรงกับเอกสารอ้างอิงของใบเบิก " + vbCr
                strSql += "and main_doc.subgroup = match_doc.subgroup -- จับกลุ่มย่อยสินค้าประเภทเดียวกัน " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ลดหนี้ขายสด */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '')Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , db.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),db.f06,111) 'Docdate' " + vbCr
                strSql += "    , 'ลดหนี้ใบเสร็จ' status " + vbCr
                strSql += "    , c.f11 * -1 'QTY' " + vbCr
                strSql += "    , d.f26 * -1 'Amount' " + vbCr
                strSql += "    , '' 'Item2' " + vbCr
                strSql += "    , '' 'Doccode2' " + vbCr
                strSql += "    , '' 'Docdate2' " + vbCr
                strSql += "    , 0 'Qty2' " + vbCr
                strSql += "    , db.f44 'Account' " + vbCr
                strSql += "    , db.F53 + ' ' + db.F54 + ' ' + db.f55 'CustomerName' " + vbCr
                strSql += "    from r11060 h with(nolock) " + vbCr
                strSql += "    join r11061 d with(nolock) on h.f05 = d.f05 " + vbCr
                strSql += "    left join r11068 c with(nolock) on h.f05 = c.f05	" + vbCr
                strSql += "    join m00030 b with(nolock) on b.f02 =h.f02 " + vbCr
                strSql += "    join m02020 m with(nolock) on m.f02 =b.f12 " + vbCr
                strSql += "    join r11090 db with(nolock) on db.f10 =h.f05 " + vbCr
                strSql += "    where(db.f02 = b.f02) " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and c.f08 >='" + FromItem + "' and c.f08 <='" + ToItem + "' " + vbCr
                strSql += "    and m.y04 >='" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>='" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>='" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and db.f06 >= '" + FromDate + "' " + vbCr
                strSql += "    and db.f06 <= '" + ToDate + "' " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += " " + vbCr

                strSql += ") sar " + vbCr
                strSql += "group by area,cluster,Province, itemcode, itemname  " + vbCr
                strSql += "order by area, Cluster, itemcode, itemname " + vbCr


                DT = C.GetDataTableRepweb(strSql)
                GridView2.DataSource = DT
                GridView2.DataBind()
            ElseIf GroupBy = "province" Then

                strSql = "select area,cluster,Province,itemcode,itemname,sum(QTY)'ขาย',sum(QTY2)'เบิก' " + vbCr
                strSql += "from ( " + vbCr
                strSql += "/* OTC */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '') Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , i1.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province'  " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , h.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),h.f06,111) 'DocDate' " + vbCr
                strSql += "    , case when h.f11 ='A' then 'ยกเลิกใบเสร็จ'else '' end 'Status' " + vbCr
                strSql += "    , case when h.f11 ='A' then 0 else c.f11  end 'QTY' " + vbCr
                strSql += "    , case when h.f11 ='A' then 0 else d.f26  end 'Amount' " + vbCr
                strSql += "    , h.f35 'Account' " + vbCr
                strSql += "    , h.F36 + ' ' + h.F37 + ' ' + h.f38 'CustomerName' " + vbCr
                strSql += "    from r17520 h with(nolock) -- เอกสาร OTC " + vbCr
                strSql += "    join r17521 d with(nolock) -- เอกสารรายการ " + vbCr
                strSql += "    on h.f05 =d.f05 -- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join r17524 c with(nolock) -- เอกสารรายการอุปกรณ์ " + vbCr
                strSql += "    on h.f05 =c.f05	-- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join m00030 b with(nolock) -- ข้อมูลสำนักงาน " + vbCr
                strSql += "    on b.f02 = h.f02 -- สำนักงานตรงกัน " + vbCr
                strSql += "    join m02020 m with(nolock) -- ข้อมูลจังหวัด " + vbCr
                strSql += "    on m.f02 =b.f12	-- เลขที่จังหวัดจรงกัน " + vbCr
                strSql += "    join m00210 i1 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on c.f08 = i1.f02 -- อุปกรณ์เอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where(C.f06 = d.f06) --ลำดับรายการตรงกับลำดับอุปกรณ์)" + vbCr
                strSql += "    and c.f08 >= '" + FromItem + "' " + vbCr
                strSql += "    and c.f08 <= '" + ToItem + "' " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and m.y04 >= '" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>= '" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>= '" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and h.f06 >='" + FromDate + "' -- วันที่เริ่มต้นในการค้นหาเอกสาร " + vbCr
                strSql += "    and h.f06 <='" + ToDate + "' -- วันที่สุดท้ายในการค้นหาเอกสาร " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += "Left Join " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select rd.f25 'Item2' -- อุปกรณ์ที่เบิก " + vbCr
                strSql += "    , i2.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , r.f05 'Doccode2' -- เอกสารที่เบิก " + vbCr
                strSql += "    , r.f23 'ref_doc' -- เอกสารอ้างอิง " + vbCr
                strSql += "    , convert(varchar(10),r.f06,111) 'DocDate2' -- วันทีเบิกตามเอกสาร " + vbCr
                strSql += "    , case when r.f04 in ('AISRN','AISSN','AISSV') then rd.f10 else (rd.f10*-1)  end 'QTY2' -- จำนวน " + vbCr
                strSql += "    from r01090 r with(nolock) -- เอกสารใบเบิก " + vbCr
                strSql += "    join r01091 rd with(nolock) -- รายการใบเบิก " + vbCr
                strSql += "    on r.f05 =rd.f05 -- เอกสารต้องเป็นตัวเดียวกัน " + vbCr
                strSql += "    join m00210 i2 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on rd.f25 = i2.f02 -- อุปกรณ์ในเอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where r.f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "    and rd.f24 <> 0 -- ตัดหมายเหตุรายการออก " + vbCr
                strSql += "    and r.F05 not in ( -- ตัดเอกสารที่มีการรับคืนออก " + vbCr
                strSql += "        Select f10 --อ้างอิงรับคืนเอกสารไหน " + vbCr
                strSql += "        from R01090 with(nolock) " + vbCr
                strSql += "        where f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "        and F04 = 'ARTNN' -- ดึงเฉพาะรับคืนอ้างใบเบิก " + vbCr
                strSql += "    ) " + vbCr
                strSql += "    and r.f06 >='" + FromDate + "' -- วันที่เริ้มต้นการค้นหา " + vbCr
                strSql += " " + vbCr
                strSql += ") match_doc " + vbCr
                strSql += "on main_doc.Doccode = match_doc.ref_doc -- เอกสารขายตรงกับเอกสารอ้างอิงของใบเบิก " + vbCr
                strSql += "and main_doc.subgroup = match_doc.subgroup -- จับกลุ่มย่อยสินค้าประเภทเดียวกัน " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ลดหนี้ OTC */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '')Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster'" + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , db.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),db.f06,111) 'Docdate' " + vbCr
                strSql += "    , 'ลดหนี้ใบเสร็จ' status " + vbCr
                strSql += "    , c.f11*-1 'QTY' " + vbCr
                strSql += "    , d.f26*-1 'Amount' " + vbCr
                strSql += "    , '' 'Item2' " + vbCr
                strSql += "    , '' 'Doccode2' " + vbCr
                strSql += "    , '' 'Docdate2' " + vbCr
                strSql += "    , 0 'Qty2' " + vbCr
                strSql += "    , db.f44 'Account' " + vbCr
                strSql += "    , db.F53 + ' ' + db.F54 + ' ' + db.f55 'CustomerName' " + vbCr
                strSql += "    from r17520 h with(nolock) inner join r17521 d with(nolock) on h.f05 =d.f05 " + vbCr
                strSql += "    left join r17524 c with(nolock) on h.f05=c.f05 " + vbCr
                strSql += "    left join m00030 b with(nolock) on b.f02 =h.f02 " + vbCr
                strSql += "    left join m02020 m with(nolock) on m.f02 =b.f12 " + vbCr
                strSql += "    left join r11090 db with(nolock) on db.f10 =h.f05 " + vbCr
                strSql += "    where(db.f02 = b.f02) " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and c.f08 >='" + FromItem + "' and c.f08 <='" + ToItem + "' " + vbCr
                strSql += "    and m.y04 >='" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>='" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>='" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and db.f06 >='" + FromDate + "' " + vbCr
                strSql += "    and db.f06 <='" + ToDate + "' " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ขายสด */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '') Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select isnull(C.f08, d.F09) 'ItemCode' " + vbCr
                strSql += "    , isnull(c.f09, d.F13) 'ItemName' " + vbCr
                strSql += "    , i1.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , h.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),h.f06,111)'DocDate' " + vbCr
                strSql += "    , case when h.f63 ='A' then 'ยกเลิกใบเสร็จ'else '' end 'Status' " + vbCr
                strSql += "    , case when h.f63 ='A' then 0 else isNULL(c.f11, d.F24) end 'QTY' " + vbCr
                strSql += "    , case when h.f63 ='A' then 0 else d.f25 end 'Amount' " + vbCr
                strSql += "    , h.f11 'Account' " + vbCr
                strSql += "    , h.f45 'CustomerName' " + vbCr
                strSql += "    from r11060 h with(nolock) -- เอกสาร OTC " + vbCr
                strSql += "    join r11061 d with(nolock) -- เอกสารรายการ " + vbCr
                strSql += "    on h.f05 =d.f05 -- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    left join r11068 c with(nolock) -- เอกสารรายการอุปกรณ์ " + vbCr
                strSql += "    on h.f05 =c.f05	-- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join m00030 b with(nolock) -- ข้อมูลสำนักงาน " + vbCr
                strSql += "    on b.f02 = h.f02 -- สำนักงานตรงกัน " + vbCr
                strSql += "    join m02020 m with(nolock) -- ข้อมูลจังหวัด " + vbCr
                strSql += "    on m.f02 =b.f12	-- เลขที่จังหวัดจรงกัน " + vbCr
                strSql += "    join m00210 i1 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on isnull(c.f08, d.f09) = i1.f02 -- อุปกรณ์เอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where isnull(C.f06, d.f06) = d.f06 --ลำดับรายการตรงกับลำดับอุปกรณ์) " + vbCr
                strSql += "    and c.f08 >= '" + FromItem + "' " + vbCr
                strSql += "    and c.f08 <= '" + ToItem + "' " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and m.y04 >= '" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>= '" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>= '" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and h.f06 >='" + FromDate + "' -- วันที่เริ่มต้นในการค้นหาเอกสาร " + vbCr
                strSql += "    and h.f06 <='" + ToDate + "' -- วันที่สุดท้ายในการค้นหาเอกสาร " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += "Left Join " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select rd.f25 'Item2' -- อุปกรณ์ที่เบิก " + vbCr
                strSql += "    , i2.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , r.f05 'Doccode2' -- เอกสารที่เบิก " + vbCr
                strSql += "    , r.f23 'ref_doc' -- เอกสารอ้างอิง " + vbCr
                strSql += "    , convert(varchar(10),r.f06,111) 'DocDate2' -- วันทีเบิกตามเอกสาร " + vbCr
                strSql += "    , case when r.f04 in ('AISRN','AISSN','AISSV') then rd.f10 else (rd.f10*-1)  end 'QTY2' -- จำนวน " + vbCr
                strSql += "    from r01090 r with(nolock) -- เอกสารใบเบิก " + vbCr
                strSql += "    join r01091 rd with(nolock) -- รายการใบเบิก " + vbCr
                strSql += "    on r.f05 =rd.f05 -- เอกสารต้องเป็นตัวเดียวกัน " + vbCr
                strSql += "    join m00210 i2 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on rd.f25 = i2.f02 -- อุปกรณ์ในเอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where r.f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "    and rd.f24 <> 0 -- ตัดหมายเหตุรายการออก " + vbCr
                strSql += "    and r.F05 not in ( -- ตัดเอกสารที่มีการรับคืนออก " + vbCr
                strSql += "    Select f10 --อ้างอิงรับคืนเอกสารไหน " + vbCr
                strSql += "        from R01090 with(nolock) " + vbCr
                strSql += "        where f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "        and F04 = 'ARTNN' -- ดึงเฉพาะรับคืนอ้างใบเบิก " + vbCr
                strSql += "    ) " + vbCr
                strSql += "    and r.f06 >= '" + FromDate + "' -- วันที่เริ้มต้นการค้นหา " + vbCr
                strSql += " " + vbCr
                strSql += ") match_doc " + vbCr
                strSql += "on main_doc.Doccode = match_doc.ref_doc -- เอกสารขายตรงกับเอกสารอ้างอิงของใบเบิก " + vbCr
                strSql += "and main_doc.subgroup = match_doc.subgroup -- จับกลุ่มย่อยสินค้าประเภทเดียวกัน " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ลดหนี้ขายสด */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '')Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , db.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),db.f06,111) 'Docdate' " + vbCr
                strSql += "    , 'ลดหนี้ใบเสร็จ' status " + vbCr
                strSql += "    , c.f11 * -1 'QTY' " + vbCr
                strSql += "    , d.f26 * -1 'Amount' " + vbCr
                strSql += "    , '' 'Item2' " + vbCr
                strSql += "    , '' 'Doccode2' " + vbCr
                strSql += "    , '' 'Docdate2' " + vbCr
                strSql += "    , 0 'Qty2' " + vbCr
                strSql += "    , db.f44 'Account' " + vbCr
                strSql += "    , db.F53 + ' ' + db.F54 + ' ' + db.f55 'CustomerName' " + vbCr
                strSql += "    from r11060 h with(nolock) " + vbCr
                strSql += "    join r11061 d with(nolock) on h.f05 = d.f05 " + vbCr
                strSql += "    left join r11068 c with(nolock) on h.f05 = c.f05	" + vbCr
                strSql += "    join m00030 b with(nolock) on b.f02 =h.f02 " + vbCr
                strSql += "    join m02020 m with(nolock) on m.f02 =b.f12 " + vbCr
                strSql += "    join r11090 db with(nolock) on db.f10 =h.f05 " + vbCr
                strSql += "    where(db.f02 = b.f02) " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and c.f08 >='" + FromItem + "' and c.f08 <='" + ToItem + "' " + vbCr
                strSql += "    and m.y04 >='" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>='" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>='" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and db.f06 >= '" + FromDate + "' " + vbCr
                strSql += "    and db.f06 <= '" + ToDate + "' " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += " " + vbCr

                strSql += ") sar " + vbCr
                strSql += "group by area,cluster, Province, itemcode, itemname  " + vbCr
                strSql += "order by area, Cluster, Province, itemcode, itemname " + vbCr


                DT = C.GetDataTableRepweb(strSql)
                GridView3.DataSource = DT
                GridView3.DataBind()
            ElseIf GroupBy = "doc" Then

                strSql = "/* OTC */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '') Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , i1.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province'  " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , h.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),h.f06,111) 'DocDate' " + vbCr
                strSql += "    , case when h.f11 ='A' then 'ยกเลิกใบเสร็จ'else '' end 'Status' " + vbCr
                strSql += "    , case when h.f11 ='A' then 0 else c.f11  end 'QTY' " + vbCr
                strSql += "    , case when h.f11 ='A' then 0 else d.f26  end 'Amount' " + vbCr
                strSql += "    , h.f35 'Account' " + vbCr
                strSql += "    , h.F36 + ' ' + h.F37 + ' ' + h.f38 'CustomerName' " + vbCr
                strSql += "    from r17520 h with(nolock) -- เอกสาร OTC " + vbCr
                strSql += "    join r17521 d with(nolock) -- เอกสารรายการ " + vbCr
                strSql += "    on h.f05 =d.f05 -- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join r17524 c with(nolock) -- เอกสารรายการอุปกรณ์ " + vbCr
                strSql += "    on h.f05 =c.f05	-- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join m00030 b with(nolock) -- ข้อมูลสำนักงาน " + vbCr
                strSql += "    on b.f02 = h.f02 -- สำนักงานตรงกัน " + vbCr
                strSql += "    join m02020 m with(nolock) -- ข้อมูลจังหวัด " + vbCr
                strSql += "    on m.f02 =b.f12	-- เลขที่จังหวัดจรงกัน " + vbCr
                strSql += "    join m00210 i1 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on c.f08 = i1.f02 -- อุปกรณ์เอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where(C.f06 = d.f06) --ลำดับรายการตรงกับลำดับอุปกรณ์)" + vbCr
                strSql += "    and c.f08 >= '" + FromItem + "' " + vbCr
                strSql += "    and c.f08 <= '" + ToItem + "' " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and m.y04 >= '" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>= '" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>= '" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and h.f06 >='" + FromDate + "' -- วันที่เริ่มต้นในการค้นหาเอกสาร " + vbCr
                strSql += "    and h.f06 <='" + ToDate + "' -- วันที่สุดท้ายในการค้นหาเอกสาร " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += "Left Join " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select rd.f25 'Item2' -- อุปกรณ์ที่เบิก " + vbCr
                strSql += "    , i2.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , r.f05 'Doccode2' -- เอกสารที่เบิก " + vbCr
                strSql += "    , r.f23 'ref_doc' -- เอกสารอ้างอิง " + vbCr
                strSql += "    , convert(varchar(10),r.f06,111) 'DocDate2' -- วันทีเบิกตามเอกสาร " + vbCr
                strSql += "    , case when r.f04 in ('AISRN','AISSN','AISSV') then rd.f10 else (rd.f10*-1)  end 'QTY2' -- จำนวน " + vbCr
                strSql += "    from r01090 r with(nolock) -- เอกสารใบเบิก " + vbCr
                strSql += "    join r01091 rd with(nolock) -- รายการใบเบิก " + vbCr
                strSql += "    on r.f05 =rd.f05 -- เอกสารต้องเป็นตัวเดียวกัน " + vbCr
                strSql += "    join m00210 i2 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on rd.f25 = i2.f02 -- อุปกรณ์ในเอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where r.f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "    and rd.f24 <> 0 -- ตัดหมายเหตุรายการออก " + vbCr
                strSql += "    and r.F05 not in ( -- ตัดเอกสารที่มีการรับคืนออก " + vbCr
                strSql += "        Select f10 --อ้างอิงรับคืนเอกสารไหน " + vbCr
                strSql += "        from R01090 with(nolock) " + vbCr
                strSql += "        where f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "        and F04 = 'ARTNN' -- ดึงเฉพาะรับคืนอ้างใบเบิก " + vbCr
                strSql += "    ) " + vbCr
                strSql += "    and r.f06 >='" + FromDate + "' -- วันที่เริ้มต้นการค้นหา " + vbCr
                strSql += " " + vbCr
                strSql += ") match_doc " + vbCr
                strSql += "on main_doc.Doccode = match_doc.ref_doc -- เอกสารขายตรงกับเอกสารอ้างอิงของใบเบิก " + vbCr
                strSql += "and main_doc.subgroup = match_doc.subgroup -- จับกลุ่มย่อยสินค้าประเภทเดียวกัน " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ลดหนี้ OTC */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '')Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster'" + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , db.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),db.f06,111) 'Docdate' " + vbCr
                strSql += "    , 'ลดหนี้ใบเสร็จ' status " + vbCr
                strSql += "    , c.f11*-1 'QTY' " + vbCr
                strSql += "    , d.f26*-1 'Amount' " + vbCr
                strSql += "    , '' 'Item2' " + vbCr
                strSql += "    , '' 'Doccode2' " + vbCr
                strSql += "    , '' 'Docdate2' " + vbCr
                strSql += "    , 0 'Qty2' " + vbCr
                strSql += "    , db.f44 'Account' " + vbCr
                strSql += "    , db.F53 + ' ' + db.F54 + ' ' + db.f55 'CustomerName' " + vbCr
                strSql += "    from r17520 h with(nolock) inner join r17521 d with(nolock) on h.f05 =d.f05 " + vbCr
                strSql += "    left join r17524 c with(nolock) on h.f05=c.f05 " + vbCr
                strSql += "    left join m00030 b with(nolock) on b.f02 =h.f02 " + vbCr
                strSql += "    left join m02020 m with(nolock) on m.f02 =b.f12 " + vbCr
                strSql += "    left join r11090 db with(nolock) on db.f10 =h.f05 " + vbCr
                strSql += "    where(db.f02 = b.f02) " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and c.f08 >='" + FromItem + "' and c.f08 <='" + ToItem + "' " + vbCr
                strSql += "    and m.y04 >='" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>='" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>='" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and db.f06 >='" + FromDate + "' " + vbCr
                strSql += "    and db.f06 <='" + ToDate + "' " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ขายสด */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '') Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select isnull(C.f08, d.F09) 'ItemCode' " + vbCr
                strSql += "    , isnull(c.f09, d.F13) 'ItemName' " + vbCr
                strSql += "    , i1.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , h.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),h.f06,111)'DocDate' " + vbCr
                strSql += "    , case when h.f63 ='A' then 'ยกเลิกใบเสร็จ'else '' end 'Status' " + vbCr
                strSql += "    , case when h.f63 ='A' then 0 else isNULL(c.f11, d.F24) end 'QTY' " + vbCr
                strSql += "    , case when h.f63 ='A' then 0 else d.f25 end 'Amount' " + vbCr
                strSql += "    , h.f11 'Account' " + vbCr
                strSql += "    , h.f45 'CustomerName' " + vbCr
                strSql += "    from r11060 h with(nolock) -- เอกสาร OTC " + vbCr
                strSql += "    join r11061 d with(nolock) -- เอกสารรายการ " + vbCr
                strSql += "    on h.f05 =d.f05 -- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    left join r11068 c with(nolock) -- เอกสารรายการอุปกรณ์ " + vbCr
                strSql += "    on h.f05 =c.f05	-- เลขที่เอกสารตรงกัน " + vbCr
                strSql += "    join m00030 b with(nolock) -- ข้อมูลสำนักงาน " + vbCr
                strSql += "    on b.f02 = h.f02 -- สำนักงานตรงกัน " + vbCr
                strSql += "    join m02020 m with(nolock) -- ข้อมูลจังหวัด " + vbCr
                strSql += "    on m.f02 =b.f12	-- เลขที่จังหวัดจรงกัน " + vbCr
                strSql += "    join m00210 i1 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on isnull(c.f08, d.f09) = i1.f02 -- อุปกรณ์เอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where isnull(C.f06, d.f06) = d.f06 --ลำดับรายการตรงกับลำดับอุปกรณ์) " + vbCr
                strSql += "    and c.f08 >= '" + FromItem + "' " + vbCr
                strSql += "    and c.f08 <= '" + ToItem + "' " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and m.y04 >= '" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>= '" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>= '" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and h.f06 >='" + FromDate + "' -- วันที่เริ่มต้นในการค้นหาเอกสาร " + vbCr
                strSql += "    and h.f06 <='" + ToDate + "' -- วันที่สุดท้ายในการค้นหาเอกสาร " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += "Left Join " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select rd.f25 'Item2' -- อุปกรณ์ที่เบิก " + vbCr
                strSql += "    , i2.f09 'subgroup' -- กลุ่มย่อยอุปกรณ์ " + vbCr
                strSql += "    , r.f05 'Doccode2' -- เอกสารที่เบิก " + vbCr
                strSql += "    , r.f23 'ref_doc' -- เอกสารอ้างอิง " + vbCr
                strSql += "    , convert(varchar(10),r.f06,111) 'DocDate2' -- วันทีเบิกตามเอกสาร " + vbCr
                strSql += "    , case when r.f04 in ('AISRN','AISSN','AISSV') then rd.f10 else (rd.f10*-1)  end 'QTY2' -- จำนวน " + vbCr
                strSql += "    from r01090 r with(nolock) -- เอกสารใบเบิก " + vbCr
                strSql += "    join r01091 rd with(nolock) -- รายการใบเบิก " + vbCr
                strSql += "    on r.f05 =rd.f05 -- เอกสารต้องเป็นตัวเดียวกัน " + vbCr
                strSql += "    join m00210 i2 with(nolock) -- ข้อมูลอุปกรณ์ " + vbCr
                strSql += "    on rd.f25 = i2.f02 -- อุปกรณ์ในเอกสารตรงกับข้อมูลอุปกรณ์ " + vbCr
                strSql += "    where r.f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "    and rd.f24 <> 0 -- ตัดหมายเหตุรายการออก " + vbCr
                strSql += "    and r.F05 not in ( -- ตัดเอกสารที่มีการรับคืนออก " + vbCr
                strSql += "    Select f10 --อ้างอิงรับคืนเอกสารไหน " + vbCr
                strSql += "        from R01090 with(nolock) " + vbCr
                strSql += "        where f11 in ('ISS27','ISS51','ISS80','ISS45') -- เอาเฉพาะที่เบิกให้ลูกค้า " + vbCr
                strSql += "        and F04 = 'ARTNN' -- ดึงเฉพาะรับคืนอ้างใบเบิก " + vbCr
                strSql += "    ) " + vbCr
                strSql += "    and r.f06 >= '" + FromDate + "' -- วันที่เริ้มต้นการค้นหา " + vbCr
                strSql += " " + vbCr
                strSql += ") match_doc " + vbCr
                strSql += "on main_doc.Doccode = match_doc.ref_doc -- เอกสารขายตรงกับเอกสารอ้างอิงของใบเบิก " + vbCr
                strSql += "and main_doc.subgroup = match_doc.subgroup -- จับกลุ่มย่อยสินค้าประเภทเดียวกัน " + vbCr
                strSql += " " + vbCr
                strSql += "union " + vbCr
                strSql += " " + vbCr
                strSql += "/* ลดหนี้ขายสด */ " + vbCr
                strSql += "Select ItemCode " + vbCr
                strSql += ", ItemName " + vbCr
                strSql += ", area " + vbCr
                strSql += ", Cluster " + vbCr
                strSql += ", Province " + vbCr
                strSql += ", CodeShop " + vbCr
                strSql += ", Doccode " + vbCr
                strSql += ", DocDate " + vbCr
                strSql += ", Status " + vbCr
                strSql += ", QTY " + vbCr
                strSql += ", Amount " + vbCr
                strSql += ", isNULL(Item2, '') Item2 " + vbCr
                strSql += ", isNULL(Doccode2, '')Doccode2 " + vbCr
                strSql += ", isnull(DocDate2, '') DocDate2 " + vbCr
                strSql += ", isNULL(QTY2, 0)QTY2 " + vbCr
                strSql += ", isNULL(Account, '')Account " + vbCr
                strSql += ", isNULL(CustomerName, '')CustomerName " + vbCr
                strSql += "from " + vbCr
                strSql += "( " + vbCr
                strSql += " " + vbCr
                strSql += "    Select C.f08 'ItemCode' " + vbCr
                strSql += "    , c.f09 'ItemName' " + vbCr
                strSql += "    , b.f03 'area' " + vbCr
                strSql += "    , m.y04 'Cluster' " + vbCr
                strSql += "    , m.f06 'Province' " + vbCr
                strSql += "    , b.f02 'CodeShop' " + vbCr
                strSql += "    , db.f05 'Doccode' " + vbCr
                strSql += "    , convert(varchar(10),db.f06,111) 'Docdate' " + vbCr
                strSql += "    , 'ลดหนี้ใบเสร็จ' status " + vbCr
                strSql += "    , c.f11 * -1 'QTY' " + vbCr
                strSql += "    , d.f26 * -1 'Amount' " + vbCr
                strSql += "    , '' 'Item2' " + vbCr
                strSql += "    , '' 'Doccode2' " + vbCr
                strSql += "    , '' 'Docdate2' " + vbCr
                strSql += "    , 0 'Qty2' " + vbCr
                strSql += "    , db.f44 'Account' " + vbCr
                strSql += "    , db.F53 + ' ' + db.F54 + ' ' + db.f55 'CustomerName' " + vbCr
                strSql += "    from r11060 h with(nolock) " + vbCr
                strSql += "    join r11061 d with(nolock) on h.f05 = d.f05 " + vbCr
                strSql += "    left join r11068 c with(nolock) on h.f05 = c.f05	" + vbCr
                strSql += "    join m00030 b with(nolock) on b.f02 =h.f02 " + vbCr
                strSql += "    join m02020 m with(nolock) on m.f02 =b.f12 " + vbCr
                strSql += "    join r11090 db with(nolock) on db.f10 =h.f05 " + vbCr
                strSql += "    where(db.f02 = b.f02) " + vbCr
                strSql += "    and h.f01 = '" + Company + "' " + vbCr
                If FromArea <> "00" Then
                    strSql += "    and b.f03 ='" + FromArea + "'	" + vbCr
                End If
                strSql += "    and c.f08 >='" + FromItem + "' and c.f08 <='" + ToItem + "' " + vbCr
                strSql += "    and m.y04 >='" + FromCluster + "'and m.y04 <='" + ToCluster + "' " + vbCr
                strSql += "    and m.f06>='" + FromProvince + "' and m.f06<='" + ToProvince + "' " + vbCr
                strSql += "    and h.f02>='" + FromBranch + "' and h.f02<='" + ToBranch + "' " + vbCr
                strSql += "    and db.f06 >= '" + FromDate + "' " + vbCr
                strSql += "    and db.f06 <= '" + ToDate + "' " + vbCr
                strSql += " " + vbCr
                strSql += ") main_doc " + vbCr
                strSql += " " + vbCr
                
                strSql += "order by area, Cluster, Province, CodeShop, DocDate, Doccode " + vbCr


                DT = C.GetDataTableRepweb(strSql)
                GridView4.DataSource = DT
                GridView4.DataBind()
            End If

                If GridView1.Rows.Count = 0 And GridView2.Rows.Count = 0 And GridView3.Rows.Count = 0 And GridView4.Rows.Count = 0 Then
                    lblNodata.Visible = True
                End If

            End If
    End Sub

    Private Sub ShowHeader()
        Dim vDate As String
        vDate = Format(Date.Now, "dd/MM/yyyy")
        If Mid(vDate, 7, 4) > 2500 Then
            vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) - 543).ToString
        End If
        lblHeader.Text = "ข้อมูลที่ Server ณ. วันที่ " + vDate + " เวลา " + Format(Now, "HH:mm")
        Try
            Dim vHeader() As String

            If Session("HeaderRemainRouter") IsNot Nothing Then
                vHeader = Split(Session("HeaderRemainRouter").ToString, "|")
                'response.write(Session("HeaderRemainRouter"))
                comp_param.Text = vHeader(0).ToString
                'prov_param.Text = vHeader(1).ToString
                shop_param.Text = vHeader(2).ToString
                product_param.Text = vHeader(3).ToString
                date_param.Text = vHeader(4).ToString
                report_param.Text = vHeader(5).ToString
                other_param.Text = vHeader(6).ToString

                If Request.QueryString("GroupBy") = "area" Then
                    'lblParam1.Text = vHeader(1).ToString + vHeader(4).ToString
                    'lblParam2.Text = vHeader(5).ToString + vHeader(6).ToString
                ElseIf Request.QueryString("GroupBy") = "cluster" Then
                    'lblParam1.Text = vHeader(1).ToString + vHeader(2).ToString
                    'lblParam2.Text = vHeader(4).ToString + vHeader(5).ToString
                    'lblParam3.Text = vHeader(6).ToString
                ElseIf Request.QueryString("GroupBy") = "province" Then
                    'lblParam1.Text = vHeader(1).ToString + vHeader(2).ToString
                    'lblParam2.Text = vHeader(3).ToString + vHeader(4).ToString
                    'lblParam3.Text = vHeader(5).ToString + vHeader(6).ToString
                ElseIf Request.QueryString("GroupBy") = "doc" Then
                    'lblParam1.Text = vHeader(1).ToString + vHeader(2).ToString
                    'lblParam2.Text = vHeader(3).ToString + vHeader(4).ToString
                    'lblParam3.Text = vHeader(5).ToString + vHeader(6).ToString
                End If
            End If
        Catch ex As Exception

        Finally
            'If lblParam1.Text.Trim.Length = 0 Then lblParam1.Visible = False
            'If lblParam2.Text.Trim.Length = 0 Then lblParam2.Visible = False
            'If lblParam3.Text.Trim.Length = 0 Then lblParam3.Visible = False

        End Try
    End Sub

    Function AddCell(ByVal text As String, ByVal width As Integer) As TableCell
        Dim oTableCell As New TableCell()

        oTableCell.Text = text
        oTableCell.ColumnSpan = width
        oTableCell.BorderStyle = BorderStyle.Solid
        oTableCell.BorderWidth = 1
        oTableCell.HorizontalAlign = HorizontalAlign.Center
        oTableCell.Font.Bold = True

        Return oTableCell
    End Function

    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim oGridView As GridView = DirectCast(sender, GridView)
            Dim oGridViewRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            oGridViewRow.Cells.Add(AddCell("ข้อมูลการขาย", 4))
            oGridViewRow.Cells.Add(AddCell("ข้อมูลการเบิกสินค้า", 1))

            oGridView.Controls(0).Controls.AddAt(0, oGridViewRow)
        End If
    End Sub

    Protected Sub GridView2_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView2.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim oGridView As GridView = DirectCast(sender, GridView)
            Dim oGridViewRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            oGridViewRow.Cells.Add(AddCell("ข้อมูลการขาย", 5))
            oGridViewRow.Cells.Add(AddCell("ข้อมูลการเบิกสินค้า", 1))

            oGridView.Controls(0).Controls.AddAt(0, oGridViewRow)
        End If
    End Sub

    Protected Sub GridView3_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView3.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim oGridView As GridView = DirectCast(sender, GridView)
            Dim oGridViewRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            oGridViewRow.Cells.Add(AddCell("ข้อมูลการขาย", 6))
            oGridViewRow.Cells.Add(AddCell("ข้อมูลการเบิกสินค้า", 1))

            oGridView.Controls(0).Controls.AddAt(0, oGridViewRow)
        End If
    End Sub

    Protected Sub GridView4_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView4.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim oGridView As GridView = DirectCast(sender, GridView)
            Dim oGridViewRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            oGridViewRow.Cells.Add(AddCell("ข้อมูลการขาย", 13))
            oGridViewRow.Cells.Add(AddCell("ข้อมูลการเบิกสินค้า", 4))

            oGridView.Controls(0).Controls.AddAt(0, oGridViewRow)
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            NumSale = NumSale + CDbl(e.Row.Cells(3).Text)
            NumTake = NumTake + CDbl(e.Row.Cells(4).Text)
            e.Row.Cells(3).Text = CDbl(e.Row.Cells(3).Text).ToString("#,###")
            e.Row.Cells(4).Text = CDbl(e.Row.Cells(4).Text).ToString("#,###")
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(2).Text = "รวม:"
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).Text = IIf(NumSale = 0, "0", NumSale.ToString("#,###"))
            e.Row.Cells(4).Text = IIf(NumTake = 0, "0", NumTake.ToString("#,###"))
        End If
    End Sub

    Protected Sub GridView2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            NumSale = NumSale + CDbl(e.Row.Cells(4).Text)
            NumTake = NumTake + CDbl(e.Row.Cells(5).Text)
            e.Row.Cells(4).Text = CDbl(e.Row.Cells(4).Text).ToString("#,###")
            e.Row.Cells(5).Text = CDbl(e.Row.Cells(5).Text).ToString("#,###")
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(3).Text = "รวม:"
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(4).Text = IIf(NumSale = 0, "0", NumSale.ToString("#,###"))
            e.Row.Cells(5).Text = IIf(NumTake = 0, "0", NumTake.ToString("#,###"))
        End If
    End Sub

    Protected Sub GridView3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            NumSale = NumSale + CDbl(e.Row.Cells(5).Text)
            NumTake = NumTake + CDbl(e.Row.Cells(6).Text)
            e.Row.Cells(5).Text = CDbl(e.Row.Cells(5).Text).ToString("#,###")
            e.Row.Cells(6).Text = CDbl(e.Row.Cells(6).Text).ToString("#,###")
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(4).Text = "รวม:"
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(5).Text = IIf(NumSale = 0, "0", NumSale.ToString("#,###"))
            e.Row.Cells(6).Text = IIf(NumTake = 0, "0", NumTake.ToString("#,###"))
        End If
    End Sub

    Protected Sub GridView4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            NumSale = NumSale + CDbl(e.Row.Cells(11).Text)
            TotalAmount = TotalAmount + CDbl(e.Row.Cells(12).Text)
            NumTake = NumTake + CDbl(e.Row.Cells(16).Text)
            If e.Row.Cells(0).Text = "&nbsp;" Then
                e.Row.Cells(11).Text = ""
                e.Row.Cells(12).Text = ""
            Else
                e.Row.Cells(11).Text = CDbl(e.Row.Cells(11).Text).ToString("#,###")
                e.Row.Cells(12).Text = CDbl(e.Row.Cells(12).Text).ToString("#,###.00")
            End If
            If e.Row.Cells(13).Text = "&nbsp;" Then
                e.Row.Cells(16).Text = ""
            Else
                e.Row.Cells(16).Text = CDbl(e.Row.Cells(16).Text).ToString("#,###")
            End If
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(10).Text = "รวม:"
            e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(11).Text = IIf(NumSale = 0, "0", NumSale.ToString("#,###"))
            e.Row.Cells(12).Text = IIf(TotalAmount = 0, "0.00", TotalAmount.ToString("#,###.00"))
            e.Row.Cells(16).Text = IIf(NumTake = 0, "0", NumTake.ToString("#,###"))
        End If
    End Sub
End Class
