Imports System.Data.OleDb
Imports System.Web.UI
Imports System.HttpStyleUriParser
Imports System.IO
Imports System.Net.IPAddress

Partial Class ReportCheq_BB
    Inherits System.Web.UI.Page
    Public Ar As New System.Configuration.AppSettingsReader
    Dim C As New Cls_Data
    Dim Log As New Cls_LogReport
    Dim fDate As DateTime
    Dim TotalPaid As Double = 0
    Dim TotalUnPaid As Double = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            fDate = "2001-01-01 00:00:00.000"
            Dim company As String = Request.QueryString("company")
            Dim shop As String = Request.QueryString("shop")
            Dim shop1 As String = Request.QueryString("shop1")
            Dim prov As String = Request.QueryString("prov")
            Dim prov1 As String = Request.QueryString("prov1")
            Dim area As String = Request.QueryString("area")
            Dim area1 As String = Request.QueryString("area1")

            'Dim startdate As DateTime = Request.QueryString("startdate")
            Dim startdate As String = Request.QueryString("startdate")
            Dim vStrDate As String = C.CDateText(startdate)
            'Dim enddate As DateTime = Request.QueryString("enddate")
            Dim enddate As String = Request.QueryString("enddate")
            Dim vEndDate As String = C.CDateText(enddate)

            'pVal.Value = company
            'objRpt.FileName = HttpContext.Current.Server.MapPath("รายงานรับเช็ค.rpt")
            'objRpt.ParameterFields("mCompany").CurrentValues.Add(pVal)

            'pVal = New CrystalDecisions.Shared.ParameterDiscreteValue   'ล้างค่าเพื่อรับค่าใหม่
            'pVal.Value = area
            'objRpt.ParameterFields("mFromkhet").CurrentValues.Add(pVal)

            'pVal = New CrystalDecisions.Shared.ParameterDiscreteValue   'ล้างค่าเพื่อรับค่าใหม่
            'pVal.Value = area1
            'objRpt.ParameterFields("mTokhet").CurrentValues.Add(pVal)

            'objRpt.SetParameterValue("pTitle", "รายงานรับเช็ค")
            'objRpt.SetParameterValue("pLocalCountry", "TH")
            'objRpt.SetParameterValue("pHeaderType", "1.00")
            'objRpt.SetParameterValue("pHeaderLanguage", "T")
            'objRpt.SetParameterValue("pCopyName", " ")
            'objRpt.SetParameterValue("pAdrCol10", "1")
            'objRpt.SetParameterValue("pAdrCol11", "1")
            '' objRpt.SetParameterValue("mCompanyname", "บริษัท ทีทีแอนด์ที จำกัด (มหาชน)")

            'pVal = New CrystalDecisions.Shared.ParameterDiscreteValue   'ล้างค่าเพื่อรับค่าใหม่
            'pVal.Value = shop
            'objRpt.ParameterFields("mFromBranch").CurrentValues.Add(pVal)

            'pVal = New CrystalDecisions.Shared.ParameterDiscreteValue   'ล้างค่าเพื่อรับค่าใหม่
            'pVal.Value = shop1
            'objRpt.ParameterFields("mToBranch").CurrentValues.Add(pVal)

            'pVal = New CrystalDecisions.Shared.ParameterDiscreteValue   'ล้างค่าเพื่อรับค่าใหม่
            'pVal.Value = vStrDate
            'objRpt.ParameterFields("mFromDate").CurrentValues.Add(pVal)

            'pVal = New CrystalDecisions.Shared.ParameterDiscreteValue   'ล้างค่าเพื่อรับค่าใหม่
            'pVal.Value = vEndDate
            'objRpt.ParameterFields("mToDate").CurrentValues.Add(pVal)

            'Dim server As String = Ar.GetValue("server", GetType(String))
            'Dim DBName As String = Ar.GetValue("DBName", GetType(String))
            'Dim UserName As String = Ar.GetValue("UserName", GetType(String))
            'Dim Password As String = Ar.GetValue("Password", GetType(String))
            'objRpt.SetDatabaseLogon(UserName, Password, server, DBName)

            'Session("objExpRpt") = objRpt
            'Dim oStream As MemoryStream
            'If Session("ExportRpt") = "no" Then
            '    With rptview
            '        .ReportSource = objRpt
            '    End With
            'ElseIf Session("Format") = 1 Then
            '    Response.Redirect("~/ShowExportReport.aspx?name=ReportPDF")
            'ElseIf Session("Format") = 2 Then
            '    oStream = DirectCast(objRpt.ExportToStream(CrystalDecisions.[Shared].ExportFormatType.Excel), MemoryStream)
            '    objRpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel)
            '    Response.Clear()
            '    Response.Buffer = True
            '    Response.ContentType = "application/vnd.ms-excel"
            '    Response.AddHeader("content-disposition", "attachment;filename=Report.xls")
            '    Response.BinaryWrite(oStream.ToArray)
            '    Response.End()
            'ElseIf Session("Format") = 3 Then
            '    oStream = DirectCast(objRpt.ExportToStream(CrystalDecisions.[Shared].ExportFormatType.ExcelRecord), MemoryStream)
            '    objRpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelRecord)
            '    Response.Clear()
            '    Response.Buffer = True
            '    Response.ContentType = "application/vnd.ms-excel"
            '    Response.AddHeader("content-disposition", "attachment;filename=Report_.xls")
            '    Response.BinaryWrite(oStream.ToArray)
            '    Response.End()
            'ElseIf Session("Format") = 4 Then
            '    oStream = DirectCast(objRpt.ExportToStream(CrystalDecisions.[Shared].ExportFormatType.WordForWindows), MemoryStream)
            '    objRpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows)
            '    Response.Clear()
            '    Response.Buffer = True
            '    Response.ContentType = "application/ms-word"
            '    Response.AddHeader("content-disposition", "attachment;filename=Report.doc")
            '    Response.BinaryWrite(oStream.ToArray)
            '    Response.End()
            'End If

            ShowHeader()
            Dim strSql As String
            Dim DT As DataTable
            strSql = "/*Bill*/" + vbCr
            strSql += "select  Rec.RO,Rec.Cluster,Rec.CodeShop,convert(varchar(10),b.f06,103) 'DateDoc',Rec.cc,Rec.DOC,case b.f11 when 'A' then 'ยกเลิก' else '' end status ,Rec.CQ,Rec.CQDate,Rec.Bank,Rec.BankS,Rec.CusName,Rec.Paid,Rec.UnPaid,convert(varchar(10),b.f06,111) 'sDate' " + vbCr
            strSql += "from " + vbCr
            strSql += "(select s.f03 'RO',a.y04 'Cluster',r.f02 'CodeShop',b.f05 'Doc',r.f11 'CQ' ,convert(varchar(10),r.f12,103) 'CQDate',c.f03 'Bank',r.f31 'BankS',b.f37 'CusName', " + vbCr
            strSql += "(case b.f11 when 'A' then 0.00 else (case r.f33 when '1' then (case b.f04 when 'DCNPV' then r.f09*(-1) else r.f09 end) else 0.00 end )end) Paid, " + vbCr
            strSql += "(case b.f11 when 'A' then 0.00 else (case r.f33 when '0' then (case b.f04 when 'DCNPV' then r.f09*(-1) else r.f09 end) else 0.00 end) end) UnPaid ,r.f32 cc " + vbCr
            strSql += "from R16060 r with(nolock), r17510 b with(nolock),m00030 s with(nolock), m02020 a with(nolock), m00140 c with(nolock) " + vbCr
            strSql += "where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and r.f02=s.f02  " + vbCr
            strSql += "and r.f07 ='CQ'  and c.f02 =r.f13 " + vbCr
            strSql += "and b.f06 >= '" + vStrDate.ToString + "' and b.f06 <= '" + vEndDate.ToString + "' " + vbCr
            strSql += "and b.f01='" + company.ToString + "' /*บริษัท*/" + vbCr
            strSql += "and s.f03 >= '" + area.ToString + "' and s.f03 <= '" + area1.ToString + "' /*เขต*/" + vbCr
            strSql += "and a.f06 >= '" + prov.ToString + "' and  a.f06 <= '" + prov1.ToString + "' /*ลำดับจังหวัด*/" + vbCr
            strSql += "and b.f02>='" + shop.ToString + "' and b.f02<='" + shop1.ToString + "'  /*รหัสสำนักงาน*/" + vbCr
            strSql += "group by s.f03,a.y04,r.f02,b.f05,r.f11,r.f12,c.f03,r.f31,b.f37,r.f09,r.f33,b.f11,r.f32,b.f04 )   Rec  ,r17510 b with(nolock)  where  Rec.Doc=b.f05 " + vbCr
            strSql += "union " + vbCr
            strSql += "/*--Bill-- กรณีที่เลือกบริษัทรับเช็คไม่ตรงกับเอกสารที่ออก*/" + vbCr
            strSql += "select  Rec.RO,Rec.Cluster,Rec.CodeShop,convert(varchar(10),b.f06,103) 'DateDoc',Rec.cc,Rec.DOC,case b.f11 when 'A' then 'ยกเลิก' else '' end status ,Rec.CQ,Rec.CQDate,Rec.Bank,Rec.BankS,Rec.CusName,Rec.Paid,Rec.UnPaid,convert(varchar(10),b.f06,111) 'sDate' " + vbCr
            strSql += "from " + vbCr
            strSql += "(select s.f03 'RO',a.y04 'Cluster',r.f02 'CodeShop',b.f05 'Doc',r.f11 'CQ' ,convert(varchar(10),r.f12,103) 'CQDate',c.f03 'Bank',r.f31 'BankS',b.f37 'CusName', " + vbCr
            strSql += "(case b.f11 when 'A' then 0.00 else (case r.f33 when '1' then (case b.f04 when 'DCNPV' then r.f09*(-1) else r.f09 end) else 0.00 end )end) Paid, " + vbCr
            strSql += "(case b.f11 when 'A' then 0.00 else (case r.f33 when '0' then (case b.f04 when 'DCNPV' then r.f09*(-1) else r.f09 end) else 0.00 end) end) UnPaid ,r.f32 cc " + vbCr
            strSql += "from R16060 r with(nolock) , r17510 b with(nolock),m00030 s with(nolock), m02020 a with(nolock), m00140 c with(nolock) " + vbCr
            strSql += "where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and r.f02=s.f02  " + vbCr
            strSql += "and r.f07 ='CQ'  and c.f02 =r.f13 " + vbCr
            strSql += "and b.f06 >= '" + vStrDate.ToString + "' and b.f06 <= '" + vEndDate.ToString + "' " + vbCr
            strSql += "and r.f32='" + company.ToString + "' and r.f01<>'" + company.ToString + "' /*บริษัท*/" + vbCr
            strSql += "and s.f03 >= '" + area.ToString + "' and s.f03 <= '" + area1.ToString + "' /*เขต*/" + vbCr
            strSql += "and a.f06 >= '" + prov.ToString + "' and  a.f06 <= '" + prov1.ToString + "' /*ลำดับจังหวัด*/" + vbCr
            strSql += "and b.f02>='" + shop.ToString + "' and b.f02<='" + shop1.ToString + "'  /*รหัสสำนักงาน*/" + vbCr
            strSql += "group by s.f03,a.y04,r.f02,b.f05,r.f11,r.f12,c.f03,r.f31,b.f37,r.f09,r.f33,b.f11,r.f32,b.f04 )   Rec  ,r17510 b with(nolock)  where  Rec.Doc=b.f05 " + vbCr
            strSql += "union " + vbCr
            strSql += "/*OTC*/" + vbCr
            strSql += "select  Rec.RO,Rec.Cluster,Rec.CodeShop,convert(varchar(10),b.f06,103) 'DateDoc',Rec.cc,Rec.DOC,case b.f11 when 'A' then 'ยกเลิก' else '' end status ,Rec.CQ,Rec.CQDate,Rec.Bank,Rec.BankS,Rec.CusName,Rec.Paid,Rec.UnPaid,convert(varchar(10),b.f06,111) 'sDate' " + vbCr
            strSql += "from " + vbCr
            strSql += "(select s.f03 'RO',a.y04 'Cluster',r.f02 'CodeShop',b.f05 'Doc',r.f11 'CQ' ,convert(varchar(10),r.f12,103) 'CQDate',c.f03 'Bank',r.f31 'BankS',b.f37 'CusName', " + vbCr
            strSql += "(case b.f11 when 'A' then 0.00 else (case r.f33 when '1' then r.f09 else 0.00 end )end) Paid, " + vbCr
            strSql += "(case b.f11 when 'A' then 0.00 else (case r.f33 when '0' then r.f09 else 0.00 end) end) UnPaid ,r.f32 cc " + vbCr
            strSql += "from R16060 r with(nolock) , r17520 b with(nolock),m00030 s with(nolock), m02020 a with(nolock), m00140 c with(nolock) " + vbCr
            strSql += "where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and r.f02=s.f02  " + vbCr
            strSql += "and r.f07 ='CQ'  and c.f02 =r.f13 " + vbCr
            strSql += "and b.f06 >= '" + vStrDate.ToString + "' and b.f06 <= '" + vEndDate.ToString + "' " + vbCr
            strSql += "and b.f01='" + company.ToString + "' /*บริษัท*/" + vbCr
            strSql += "and s.f03 >= '" + area.ToString + "' and s.f03 <= '" + area1.ToString + "' /*เขต*/ " + vbCr
            strSql += "and a.f06 >= '" + prov.ToString + "' and  a.f06 <= '" + prov1.ToString + "' /*ลำดับจังหวัด*/ " + vbCr
            strSql += "and b.f02>='" + shop.ToString + "' and b.f02<='" + shop1.ToString + "'  /*รหัสสำนักงาน*/ " + vbCr
            strSql += "group by s.f03,a.y04,r.f02,b.f05,r.f11,r.f12,c.f03,r.f31,b.f37,r.f09,r.f33,b.f11,r.f32 )   Rec  ,r17520 b with(nolock)  where  Rec.Doc=b.f05 " + vbCr
            strSql += "union " + vbCr
            strSql += "/*--OTC-- กรณีที่เลือกบริษัทรับเช็คไม่ตรงกับเอกสารที่ออก*/" + vbCr
            strSql += "select  Rec.RO,Rec.Cluster,Rec.CodeShop,convert(varchar(10),b.f06,103) 'DateDoc',Rec.cc,Rec.DOC,case b.f11 when 'A' then 'ยกเลิก' else '' end status ,Rec.CQ,Rec.CQDate,Rec.Bank,Rec.BankS,Rec.CusName,Rec.Paid,Rec.UnPaid,convert(varchar(10),b.f06,111) 'sDate' " + vbCr
            strSql += "from " + vbCr
            strSql += "(select s.f03 'RO',a.y04 'Cluster',r.f02 'CodeShop',b.f05 'Doc',r.f11 'CQ' ,convert(varchar(10),r.f12,103) 'CQDate',c.f03 'Bank',r.f31 'BankS',b.f37 'CusName', " + vbCr
            strSql += "(case b.f11 when 'A' then 0.00 else (case r.f33 when '1' then r.f09 else 0.00 end )end) Paid, " + vbCr
            strSql += "(case b.f11 when 'A' then 0.00 else (case r.f33 when '0' then r.f09 else 0.00 end) end) UnPaid ,r.f32 cc " + vbCr
            strSql += "from R16060 r with(nolock) , r17520 b with(nolock),m00030 s with(nolock), m02020 a with(nolock), m00140 c with(nolock) " + vbCr
            strSql += "where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and r.f02=s.f02  " + vbCr
            strSql += "and r.f07 ='CQ'  and c.f02 =r.f13 " + vbCr
            strSql += "and b.f06 >='" + vStrDate.ToString + "' and b.f06 <='" + vEndDate.ToString + "' " + vbCr
            strSql += "and r.f32='" + company.ToString + "' and r.f01<>'" + company.ToString + "' /*บริษัท*/" + vbCr
            strSql += "and s.f03 >= '" + area.ToString + "' and s.f03 <= '" + area1.ToString + "' /*เขต*/ " + vbCr
            strSql += "and a.f06 >= '" + prov.ToString + "' and  a.f06 <= '" + prov1.ToString + "' /*ลำดับจังหวัด*/ " + vbCr
            strSql += "and b.f02>='" + shop.ToString + "' and b.f02<='" + shop1.ToString + "'  /*รหัสสำนักงาน*/ " + vbCr
            strSql += "group by s.f03,a.y04,r.f02,b.f05,r.f11,r.f12,c.f03,r.f31,b.f37,r.f09,r.f33,b.f11,r.f32 )   Rec  ,r17520 b with(nolock)  where  Rec.Doc=b.f05 " + vbCr
            strSql += "union " + vbCr
            strSql += "/*ขายสด*/" + vbCr
            strSql += "select  Rec.RO,Rec.Cluster,Rec.CodeShop,convert(varchar(10),b.f06,103) 'DateDoc',Rec.cc,Rec.DOC,case b.f11 when 'A' then 'ยกเลิก' else '' end status ,Rec.CQ,Rec.CQDate,Rec.Bank,Rec.BankS,Rec.CusName,Rec.Paid,Rec.UnPaid,convert(varchar(10),b.f06,111) 'sDate' " + vbCr
            strSql += "from " + vbCr
            strSql += "(select s.f03 'RO',a.y04 'Cluster',r.f02 'CodeShop',b.f05 'Doc',r.f11 'CQ' ,convert(varchar(10),r.f12,103) 'CQDate',c.f03 'Bank',r.f31 'BankS',b.f45 'CusName', " + vbCr
            strSql += "(case b.f63 when 'A' then 0.00 else (case r.f33 when '1' then r.f09 else 0.00 end )end) Paid, " + vbCr
            strSql += "(case b.f63 when 'A' then 0.00 else (case r.f33 when '0' then r.f09 else 0.00 end) end) UnPaid ,r.f32 cc " + vbCr
            strSql += "from R16060 r with(nolock), r11060 b with(nolock),m00030 s with(nolock), m02020 a with(nolock), m00140 c with(nolock) " + vbCr
            strSql += "where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and r.f02=s.f02  " + vbCr
            strSql += "and r.f07 ='CQ'  and c.f02 =r.f13 " + vbCr
            strSql += "and b.f06 >='" + vStrDate.ToString + "' and b.f06 <='" + vEndDate.ToString + "' " + vbCr
            strSql += "and b.f01='" + company.ToString + "' /*บริษัท*/ " + vbCr
            strSql += "and s.f03 >= '" + area.ToString + "' and s.f03 <= '" + area1.ToString + "' /*เขต*/ " + vbCr
            strSql += "and a.f06 >= '" + prov.ToString + "' and  a.f06 <= '" + prov1.ToString + "' /*ลำดับจังหวัด*/ " + vbCr
            strSql += "and b.f02>='" + shop.ToString + "' and b.f02<='" + shop1.ToString + "'  /*รหัสสำนักงาน*/" + vbCr
            strSql += "group by s.f03,a.y04,r.f02,b.f05,r.f11,r.f12,c.f03,r.f31,b.f45,r.f09,r.f33,b.f63,r.f32 )   Rec  ,r11060 b with(nolock)  where  Rec.Doc=b.f05 " + vbCr
            strSql += "union " + vbCr
            strSql += "/*--ขายสด-- กรณีที่เลือกบริษัทรับเช็คไม่ตรงกับเอกสารที่ออก*/" + vbCr
            strSql += "select  Rec.RO,Rec.Cluster,Rec.CodeShop,convert(varchar(10),b.f06,103) 'DateDoc',Rec.cc,Rec.DOC,case b.f11 when 'A' then 'ยกเลิก' else '' end status ,Rec.CQ,Rec.CQDate,Rec.Bank,Rec.BankS,Rec.CusName,Rec.Paid,Rec.UnPaid,convert(varchar(10),b.f06,111) 'sDate' " + vbCr
            strSql += "from " + vbCr
            strSql += "(select s.f03 'RO',a.y04 'Cluster',r.f02 'CodeShop',b.f05 'Doc',r.f11 'CQ' ,convert(varchar(10),r.f12,103) 'CQDate',c.f03 'Bank',r.f31 'BankS',b.f45 'CusName', " + vbCr
            strSql += "(case b.f63 when 'A' then 0.00 else (case r.f33 when '1' then r.f09 else 0.00 end )end) Paid, " + vbCr
            strSql += "(case b.f63 when 'A' then 0.00 else (case r.f33 when '0' then r.f09 else 0.00 end) end) UnPaid ,r.f32 cc " + vbCr
            strSql += "from R16060 r with(nolock), r11060 b with(nolock),m00030 s with(nolock), m02020 a with(nolock), m00140 c with(nolock) " + vbCr
            strSql += "where  s.f12=a.f02 and s.f02 =b.f02 and b.f05 =r.f05 and r.f02=s.f02  " + vbCr
            strSql += "and r.f07 ='CQ'  and c.f02 =r.f13 " + vbCr
            strSql += "and b.f06 >='" + vStrDate.ToString + "' and b.f06 <='" + vEndDate.ToString + "' " + vbCr
            strSql += "and r.f32='" + company.ToString + "' and r.f01<>'" + company.ToString + "' /*บริษัท*/ " + vbCr
            strSql += "and s.f03 >= '" + area.ToString + "' and s.f03 <= '" + area1.ToString + "' /*เขต*/ " + vbCr
            strSql += "and a.f06 >= '" + prov.ToString + "' and  a.f06 <= '" + prov1.ToString + "' /*ลำดับจังหวัด*/ " + vbCr
            strSql += "and b.f02>='" + shop.ToString + "' and b.f02<='" + shop1.ToString + "'  /*รหัสสำนักงาน*/" + vbCr
            strSql += "group by s.f03,a.y04,r.f02,b.f05,r.f11,r.f12,c.f03,r.f31,b.f45,r.f09,r.f33,b.f63,r.f32 )   Rec  ,r11060 b with(nolock)  where  Rec.Doc=b.f05 " + vbCr
            strSql += "order by sDate,cc,Doc "
            'strSql += "" + vbCr
            'strSql += "" + vbCr

            DT = C.GetDataTableRepweb(strSql)
            GridView1.DataSource = DT
            GridView1.DataBind()

            fDate = Now().ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))

            'If Session("ExportChequeBB") IsNot Nothing Then
            '    If Session("ExportChequeBB") = "yes" Then
            '        '------------ export excel ----------------
            '        PrepareGridViewForExport(Me.GridView1)


            '        Response.Clear()
            '        'Response.Buffer = True
            '        Response.BufferOutput = True
            '        Response.Charset = "windows-874"
            '        Response.ContentEncoding = Encoding.UTF7
            '        Response.AddHeader("content-disposition", "attachment; filename=ReportSale.xls")
            '        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
            '        Response.ContentType = "application/vnd.ms-excel"

            '        Dim tw As New System.IO.StringWriter()
            '        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            '        Dim frm As HtmlForm = New HtmlForm()

            '        Me.Controls.Add(frm)
            '        frm.Controls.Add(div_ChequeBB) '//<---------write div
            '        frm.Attributes("runat") = "server"
            '        frm.RenderControl(hw)
            '        'GridView1.RenderControl(hw)
            '        Me.EnableViewState = False
            '        Response.Write(tw.ToString())
            '        Response.End()
            '        '------------ export excel ----------------
            '        div_ChequeBB.Visible = False
            '    End If
            'End If

        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        Finally
            Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            Log.LogReport("22020", "รายงานรับเช็ค", Session("Parameter"), vIPAddress, Session("email"), Session("rdate"), fDate)
        End Try
        
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

            If Session("rptCheque_BB") IsNot Nothing Then
                vHeader = Split(Session("rptCheque_BB").ToString, "|")
                lblParam1.Text = vHeader(0).ToString + vHeader(1).ToString + vHeader(2).ToString + vHeader(3).ToString
                lblParam2.Text = vHeader(4).ToString + vHeader(5).ToString + vHeader(6).ToString + vHeader(7).ToString
                lblCompany.Text = vHeader(8).ToString ' + vHeader(5).ToString + vHeader(8).ToString
            End If
        Catch ex As Exception

        Finally
            If lblParam1.Text.Trim.Length = 0 Then lblParam1.Visible = False
            If lblParam2.Text.Trim.Length = 0 Then lblParam2.Visible = False
            If lblParam3.Text.Trim.Length = 0 Then lblParam3.Visible = False

        End Try

    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = (e.Row.RowIndex + 1).ToString
            e.Row.Cells(8).Attributes.Add("class", "text")
            
            Dim Paid As Double = e.Row.Cells(13).Text
            Paid = String.Format(e.Row.Cells(13).Text, "{0:00,00.00}")
            e.Row.Cells(13).Text = Format(Paid, "#,##0.00")
            e.Row.Cells(13).HorizontalAlign = HorizontalAlign.Right
            Dim UnPaid As Double = e.Row.Cells(14).Text
            UnPaid = String.Format(e.Row.Cells(14).Text, "{0:00,00.00}")
            e.Row.Cells(14).Text = Format(UnPaid, "#,##0.00")
            e.Row.Cells(14).HorizontalAlign = HorizontalAlign.Right

            TotalPaid += CDbl(DataBinder.Eval(e.Row.DataItem, "Paid"))
            TotalUnPaid += CDbl(DataBinder.Eval(e.Row.DataItem, "UnPaid"))
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(12).Text = "รวมทั้งหมด:"
            e.Row.Cells(13).Text = TotalPaid.ToString("#,##0.00") 'ตรงนี้ก็เอาผลรวมที่ได้มาแสดงใน Footer ครับ
            e.Row.Cells(13).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(14).Text = TotalUnPaid.ToString("#,##0.00")
            e.Row.Cells(14).HorizontalAlign = HorizontalAlign.Right
        End If
    End Sub

    Protected Sub PrepareGridViewForExport(ByVal gv As Control)
        Dim lb As New LinkButton()
        Dim l As New Literal()
        Dim name As String = String.Empty

        For i As Integer = 0 To gv.Controls.Count - 1
            If TypeOf (gv.Controls(i)) Is LinkButton Then
                l.Text = CType(gv.Controls(i), LinkButton).Text
                gv.Controls.Remove(gv.Controls(i))
                gv.Controls.AddAt(i, l)
            ElseIf TypeOf (gv.Controls(i)) Is DropDownList Then
                l.Text = CType(gv.Controls(i), DropDownList).SelectedItem.Text
                gv.Controls.Remove(gv.Controls(i))
                gv.Controls.AddAt(i, l)
            ElseIf TypeOf (gv.Controls(i)) Is CheckBox Then
                l.Text = IIf(CType(gv.Controls(i), CheckBox).Checked, "True", "False")
                gv.Controls.Remove(gv.Controls(i))
                gv.Controls.AddAt(i, l)
            End If

            If (gv.Controls(i).HasControls()) Then
                PrepareGridViewForExport(gv.Controls(i))
            End If
        Next

    End Sub

    Protected Sub expExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles expExcel.Click
        '------------ export excel ----------------
        PrepareGridViewForExport(Me.GridView1)

        Response.Clear()
        'Response.Buffer = True
        Response.BufferOutput = True
        Response.Charset = "windows-874"
        Response.ContentEncoding = Encoding.UTF8
        Response.AddHeader("content-disposition", "attachment; filename=ReportSale.xls")
        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.ms-excel"

        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim frm As HtmlForm = New HtmlForm()
        Dim style As String = "<style> .text { mso-number-format:\@; } </style> "

        'Me.Controls.Add(frm)
        'frm.Controls.Add(div_report1) '//<---------write div
        'frm.Attributes("runat") = "server"
        'frm.RenderControl(hw)
        ''GridView1.RenderControl(hw)
        div_ChequeBB.RenderControl(hw)
        Me.EnableViewState = False
        Response.Write(style)
        Response.Write(tw.ToString())
        Response.End()
        '------------ export excel ----------------
        div_ChequeBB.Visible = False
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'MyBase.VerifyRenderingInServerForm(control) 
    End Sub

End Class
