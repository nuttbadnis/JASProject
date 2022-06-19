Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Net.IPAddress
Imports System.Threading

Public Class Cls_RequestFlow
    Inherits System.Web.UI.Page
    Dim DB105 As New Cls_Data105
    Dim DBShopStock As New Cls_DataShopStock
    Dim CP As New Cls_Panu
    Dim CUL As New Cls_UploadFile

    Public Shared Dim global_path As String = "upload/"
    Public Shared Dim url_3bbshop As String = "https://posbcs.triplet.co.th/3bbShop/default.aspx"
    Public Shared Dim file_wait_request_end = "<span class='txt-gray'>รอปิดคำขอ</span>"
    Public Shared Dim file_dont_request_permiss = "<span class='txt-red'>เฉพาะผู้เกี่ยวข้อง</span>"

    Public Shared Dim system_admin As Integer = 999
    Public Shared Dim audit_file As Integer = 888
    Public Shared Dim team_act_redebt As Integer = 10
    Public Shared Dim team_bil_redebt As Integer = 20
    Public Shared Dim team_cre_80 As Integer = 18

    Dim LinkDB_VAS as String = System.Configuration.ConfigurationSettings.AppSettings("LinkDB_VAS")
    Dim callback_api_3bbshop as String = System.Configuration.ConfigurationSettings.AppSettings("callback_api_3bbshop")
    Dim append_mail as String = System.Configuration.ConfigurationSettings.AppSettings("append_mail")

    Dim vRes_Ajax_SaveFlow As String = 0 'response สำหรับ Function SaveFlowAjax

#Region "loadSql"
    Function rSqlSearchAllRequest() As String
        Dim xTitleID As String = HttpContext.Current.Request.QueryString("request_title_id")
        Dim xSubject As String = HttpContext.Current.Request.QueryString("subject_id")
        Dim xStatus As String = HttpContext.Current.Request.QueryString("status_id")

        Dim xRadDate As String = HttpContext.Current.Request.QueryString("rad_date")
        Dim xStartDate As String = HttpContext.Current.Request.QueryString("start_date")
        Dim xEndDate As String = HttpContext.Current.Request.QueryString("end_date")

        Dim xStatusEpay As String = HttpContext.Current.Request.QueryString("status_epay")
        Dim xRO As String = HttpContext.Current.Request.QueryString("area_ro")
        Dim xCreateRO As String = HttpContext.Current.Request.QueryString("create_ro")
        
        Dim kw As String = HttpContext.Current.Request.QueryString("kw")
        Dim acc As String = HttpContext.Current.Request.QueryString("acc")

        Dim xDepartID As String = HttpContext.Current.Request.QueryString("depart_id")
        Dim xAfterEndStatusID As String = HttpContext.Current.Request.QueryString("after_end_status_id")

        Dim xPickRefundID As String = HttpContext.Current.Request.QueryString("pick_refund")
        Dim xRecProvince As String = HttpContext.Current.Request.QueryString("rec_province")

        Dim xExport As String = HttpContext.Current.Request.QueryString("export")

        Dim vSql As String
        vSql += "    from request "
        vSql += "    left join cnepay.dbo.vw_cn_epay epay on epay.cn_no = request.redebt_number "

        vSql += "    left join request_title_dim on request_title_dim.request_title_id = request.request_title_id "
        vSql += "    left join subject_dim on subject_dim.subject_id = request.subject_id "
        vSql += "    left join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "    left join request_status on request_status.status_id = request.request_status "
        vSql += "    left join department next_depart on next_depart.depart_id = request.next_depart "
        vSql += "    left join pick_refund on pick_refund.pick_refund_id = request.pick_refund "

        vSql += "   left join ( "
        vSql += "       select request_id, after_end_remark.after_end_status_id, after_end_status_name "
        vSql += "       from after_end_remark "
        vSql += "       join after_end_status on after_end_status.after_end_status_id = after_end_remark.after_end_status_id "
        vSql += "       where after_end_lasted = 1 "
        vSql += "   ) after_end "
        vSql += "   on after_end.request_id = request.request_id "

        If xExport <> Nothing Or acc <> Nothing Then
            vSql += "    left join [10.11.5.106].[RMSDAT01].[dbo].[r11090] cndot "
            vSql += "    on cndot.f11 = request.redebt_number and request.redebt_number <> '' "

            vSql += "    left join (select f05, f08 from [10.11.5.106].[RMSDAT01].[dbo].[r17510] where f08 <> '') cnbill "
            vSql += "    on cnbill.f08 = request.redebt_number and request.redebt_number <> '' "

            vSql += "    left join redebt_bank_code on redebt_bank_code.bank_code = request.nx01 "

            vSql += "    left join redebt_cause_title on redebt_cause_title.redebt_cause_id = request.redebt_cause_id "
        End If

        vSql += "    where 1=1 "

        If xTitleID <> Nothing Then
            vSql += "   and request_title_dim.request_title_id in (" + xTitleID + ") "
        End If

        If xSubject <> Nothing Then
            vSql += "   and subject_dim.subject_id in (" + xSubject + ") "
        End If

        If xStatus <> Nothing Then
            vSql += "   and request.request_status in (" + xStatus + ") "
        End If

        If xStatusEpay <> Nothing Then
            vSql += "   and " + xStatusEpay + " "
        End If

        If xRadDate = Nothing Then
            xRadDate = "request.create_date"
        End If

        If xStartDate <> Nothing Then
            vSql += "   and convert(date, " + xRadDate + ", 103) >= convert(date, '" + xStartDate + "', 103) "
        End If

        If xEndDate <> Nothing Then
            vSql += "   and convert(date, " + xRadDate + ", 103) <= convert(date,'" + xEndDate + "', 103) "
        End If

        If kw <> Nothing Then
            vSql += "and ( "
            vSql += "request.create_by like '%" + kw + "%' "
            vSql += "or request.request_id like '%" + kw + "%' "
            vSql += "or request.account_number like '%" + kw + "%' "
            vSql += "or request.account_name like '%" + kw + "%' "
            vSql += "or request.account_number_to like '%" + kw + "%' "
            vSql += "or request.account_name_to like '%" + kw + "%' "
            vSql += "or request.doc_number like '%" + kw + "%' "
            vSql += "or request.bcs_number like '%" + kw + "%' "
            vSql += "or request.redebt_number like '%" + kw + "%' "
            vSql += "or rp_no like '%" + kw + "%' "
            vSql += ") "
        End If

        If acc <> Nothing Then
            ' vSql += "and request.account_number like '%" + acc + "%' "
            vSql += "and request.account_number = '" + acc + "' "
        End If

        If xRO <> Nothing Then
            vSql += "   and area_ro = '" + xRO + "' "
        End If

        If xCreateRO <> Nothing Then
            vSql += "   and create_ro = '" + xCreateRO + "' "
        End If

        If xDepartID <> Nothing Then
            vSql += "   and request.next_depart in (" + xDepartID + ") "
        End If

        If xAfterEndStatusID <> Nothing Then
            If xAfterEndStatusID = 0 Then
                vSql += "and after_end_status_id is null "
            Else
                vSql += "and after_end_status_id = '" & xAfterEndStatusID & "' "
            End If
        End If

        If xPickRefundID <> Nothing Then
            vSql += "   and request.pick_refund in (" + xPickRefundID + ") "
        End If

        If xRecProvince <> Nothing Then
            vSql += "   and request.shop_code in (" + xRecProvince + ") "
        End If

        Return vSql
    End Function
#End Region

#Region "loadReportData"

    Public Function rSqlDDCompany() As String
        Dim vSql As String
        vSql = "select f01 as ComCode,f01 + ': ' + f02 as ComName " 
        vSql += "from m00010 with(nolock) order by f01"

        Return vSql
    End Function

    Public Function rSqlDDArea(ByVal User As String,ByVal value As String) As String
        Dim vSql As String
        vSql = "select distinct o.f03 as AreaCode,mo.f02 + ' :: ' + mo.f03 as AreaName "
        vSql += "from m00860 ma, m00030 o, m02300 mo "
        vSql += "where o.f03 = mo.f02 "
        If value = 0 then
            vSql += "and ma.f03 = o.f02 "
        Else
            vSql += "and ma.f03 = 'ALL' "
        End If
        vSql += "and ma.f02 = '" & User & "' "
        vSql += "order by o.f03" 

        Return vSql
    End Function

    Public Function rSqlDDCluster_DAT(ByVal User As String,ByVal value As String, Optional ByVal Wherecause As String = Nothing) As String
        Dim vSql As String
        vSql = "select distinct pv.y04 as ClusterCode, pv.y04 as ClusterName "
        vSql += "from m00860 ma with(nolock), m00030 o with(nolock), m02300 mo with(nolock), m02020 pv with(nolock) "
        vSql += "where o.f03 = mo.f02 and o.f12 = pv.f02 and pv.Y05 <> 1 "
        If value = 0 then
            vSql += "and ma.f03 = o.f02 "
        Else
            vSql += "and ma.f03 = 'ALL' "
        End If
        vSql += "and ma.f02 = '" & User & "' "
        vSql += "" & IIf(Wherecause <> "", "and " + Wherecause, "") & " order by pv.y04 "

        Return vSql
    End Function

    Public Function rSqlDDProvince_DAT(ByVal User As String,ByVal value As String, Optional ByVal Wherecause As String = Nothing) As String
        Dim vSql As String
        vSql = "select distinct pv.f06 as ProvinceCode, pv.f06+': '+pv.f03 as ProvinceName "
        vSql += "from m00860 ma with(nolock), m00030 o with(nolock), m02300 mo with(nolock), m02020 pv with(nolock) "
        vSql += "where o.f03 = mo.f02 and o.f12 = pv.f02 and pv.Y05 <> 1 "
        If value = 0 then
            vSql += "and ma.f03 = o.f02 "
        Else
            vSql += "and ma.f03 = 'ALL' "
        End If
        vSql += "and ma.f02 = '" & User & "' "
        vSql += "" + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by pv.f06 "

        Return vSql
    End Function

    Public Function rSqlDDShop(ByVal User As String,ByVal value As String, Optional ByVal Wherecause As String = Nothing) As String
        Dim vSql As String
        vSql = "select distinct o.f02 as ShopCode, o.f02+': '+o.f04 as ShopName "
        vSql += "from m00860 ma with(nolock), m00030 o with(nolock), m02300 mo with(nolock), m02020 pv with(nolock) "
        vSql += "where o.f03 = mo.f02 and o.f12 = pv.f02 "
        If value = 0 then
            vSql += "and ma.f03 = o.f02 "
        Else
            vSql += "and ma.f03 = 'ALL' "
        End If
        vSql += "and ma.f02 = '" & User & "' "
        vSql += "" + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02 "

        Return vSql
    End Function

    Public Function rSqlDDMainGroup(ByVal value As String) As String
        Dim vSql As String
        vSql = "select distinct M70.f02 as GroupCode ,M70.f02+ ' :: '+M70.f03 as GroupName "
        vSql += "from M00170 M70 with(nolock) "
        vSql += "join M00210 M10 with(nolock) "
        vSql += "on M10.F08 = M70.F02 "
        vSql += "where M70.f01='" & value & "' "
        vSql += "and M10.F07 <> 'SRV' "
        vSql += "order by M70.f02 "
    
        Return vSql
    End Function

    Public Function rSqlDDSubGroup(ByVal value As String, Optional ByVal Wherecause As String = Nothing) As String
        Dim vSql As String
        vSql = "select distinct M80.f03 as subGroupCode,M80.f03+' :: '+ M80.f04 as subGroupName "
        vSql += "from m00180 M80 with(nolock) "
        vSql += "join M00210 M10 with(nolock) "
        vSql += "on M10.F09 = M80.F03 "
        vSql += "where M80.f01='" & value & "' "
        vSql += "and M10.F07 <> 'SRV' "
        vSql += "" + IIf(Wherecause <> "", " And " + Wherecause, "")
        vSql += "order by M80.f03 "
        
        Return vSql
    End Function

    Public Function rSqlDDItem(ByVal value As String, Optional ByVal Wherecause As String = Nothing) As String
        Dim vSql As String
        vSql = "select f02 as ItemCode,f02+' :: '+ f05 as ItemName "
        vSql += "from m00210 with(nolock) "
        vSql += "where f01='" & value & "' "
        vSql += "and F07 <> 'SRV' "
        vSql += "" + IIf(Wherecause <> "", " And " + Wherecause, "")
        vSql += "order by f02 "

        Return vSql
    End Function

    Public Function rSqlDDMainProduct(ByVal value As String) As String
        Dim vSql As String
        vSql = "select distinct g.f02 as GroupCode, g.f02 + ' :: ' + g.f03 as GroupName "
        vSql += "from m00170 g, m00180 sg "
        vSql += "where g.f01='" & value & "' "
        vSql += "and g.f02=sg.f02 and g.f01=sg.f01 "
        vSql += "order by g.f02 "
        Return vSql
    End Function

    Public Function rSqlDDSubProduct(ByVal value As String, Optional ByVal Wherecause As String = Nothing) As String
        Dim vSql As String
        vSql = "select sg.f03 as SubGroupCode, sg.f03 + ' :: ' + sg.f04 as SubGroupName "
        vSql += "from m00180 sg "
        vSql += "where sg.f01='" + value + "' "
        vSql += "" + IIf(WhereCause <> "", "and " + WhereCause, "") + " "
        vSql += "order by sg.f03 "      
        Return vSql
    End Function
    
    Public Function rSqlDDProduct(ByVal value As String, Optional ByVal Wherecause As String = Nothing) As String
        Dim vSql As String
        vSql = "select distinct st.f02 as StockCode, st.f02 + ' :: ' + st.f05 as StockName "
        vSql += "from m00180 sg, m00210 st "
        vSql += "where st.f01='" + value + "' and st.f08=sg.f02 and st.f09=sg.f03 "
        vSql += "" + IIf(WhereCause <> "", "and " + WhereCause, "") + " "
        vSql += "order by st.f02 "
        Return vSql
    End Function

    Public Function rSqlDDMonth() As String
        Dim vSql As String = ""
        Dim month as integer 
		for month =1 to 12
            If month <> 1 then
                vSql += "union "
            End If 
            vSql += "select '"+String.Format("{0:00}", month)+"' 'f07','"+String.Format("{0:00}", month)+"' 'month'"	
	    next
        Return vSql
    End Function

    Public Function rSqlDDYear() As String
        Dim vSql As String = ""
        Dim month as integer 
		for month =0 to 5
            If month <> 0 then
               vSql += "union "
            End If 
            vSql += "select cast( YEAR(GETDATE())-"+month.ToString+"  as varchar(4)) 'f06', cast(YEAR(GETDATE())-"+month.ToString+" as varchar(4)) 'year'"	
	    next
        vSql += "order by 1 desc"
        Return vSql
    End Function
#End Region

#Region "loadInsertPage"
    Public Function rLoadSubject(ByVal subject_id As String, ByVal pageUrl As String) As DataTable
        Dim vSql As String 
        vSql = "select subject_id, subject_dim.project_id, flow_id, project_prefix+subject_prefix prefix_id, subject_prefix, "
        vSql += "project_name, subject_name, subject_desc, subject_url, pick_refund_in "
        vSql += "from subject_dim "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "where subject_id = " & subject_id

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() = 0 Then
            CP.kickDefault("pagefailed")
        Else If vDT.Rows(0).Item("subject_url") <> pageUrl Then
            CP.kickDefault("pagefailed")
        End If

        Return vDT
    End Function

    Public Function rSqlDDSubject(ByVal project_id As String) As String
        Dim vSql As String 
        vSql = "select subject_id, subject_name, subject_url  "
        vSql += "from subject_dim "
        vSql += "where disable = 0 "
        vSql += "and project_id = " & project_id & " "
        vSql += "order by subject_prefix "

        Return vSql
    End Function

    Public Function rSqlDDTitle(ByVal subject_id As String) As String
        Dim vSql As String 
        vSql = "select request_title_id, request_title "
        vSql += "from request_title_dim "
        vSql += "where disable = 0 "
        vSql += "and subject_id = " + subject_id + " "
        vSql += "order by request_title "

        Return vSql
    End Function


    Public Function rSqlDDRO() As String
        Dim vSql As String 
        vSql = "select ro ro_value, 'RO' + ro ro_title "
        vSql += "from [10.11.5.106].[RMSDAT01].[dbo].[vw_ro_cluster_province] "
        vSql += "group by ro "
        vSql += "order by ro "

        Return vSql
    End Function

    Public Function rSqlDDCluster(ByVal xRo As String)
        Dim vSql As String = "select ro , cluster "
        vSql += "from [10.11.5.106].[RMSDAT01].[dbo].[vw_branch_shop] "

        If xRo <> Nothing Then
            vSql += "where ro = '" & xRo & "' "
        End If

        vSql += "group by ro, cluster "
        vSql += "order by ro, cluster "

        Return vSql
    End Function

    Public Function rSqlDDProvince(ByVal xRo As String, ByVal xCluster As String)
        Dim vSql As String = "select ro, cluster, province_code, province_short "
        vSql += ", province_short + ': ' + province_name province_name "
        vSql += ", '[' + cluster + '] ' + province_short + ': ' + province_name cluster_province_name "
        vSql += "from [10.11.5.106].[RMSDAT01].[dbo].[vw_branch_shop] "

        If xRo <> Nothing Then
            vSql += "where ro = '" & xRo & "' "
        End If

        If xCluster <> Nothing Then
            vSql += "where cluster = '" & xCluster & "' "
        End If

        vSql += "group by ro, cluster, province_code, province_short, province_name "
        vSql += "order by ro, cluster, province_short, province_name "

        Return vSql
    End Function

    Public Function rSqlDDstorePlaceType()
        Dim vSql As String = "select storeplacetype_id, storeplacetype_name "
        vSql += "from storeplacetype "
        vSql += "order by storeplacetype_id, storeplacetype_name "

        Return vSql
    End Function

    Public Function rSqlDDcontractPhase()
        Dim vSql As String = "select phase_id, phase_title "
        vSql += "from contract_phase "
        vSql += "order by phase_id, phase_title "

        Return vSql
    End Function

    Public Function rSqlDDRequestShopType()
        Dim vSql As String = "select Cast(Type_ID as int) as shoptype_id, TypeName as shoptype_name "
        vSql += "from [RequestShop].[dbo].[ShopType] "
        vSql += "order by Type_ID "

        Return vSql
    End Function

    Public Function rSqlDDRequestPayBack()
        Dim vSql As String = "select payback_id, payback_name "
        vSql += "from payback_status "
        'vSql += "from [follow2020].[dbo].[payback_status] "
        vSql += "where disable = 0 "
        vSql += "order by payback_id "

        Return vSql
    End Function

    Public Function rSqlDDInvidocPayment()
        Dim vSql As String = "select payment_id, payment_title "
        vSql += "from invdoc_payment "
        vSql += "where disable = 0 "
        vSql += "order by payment_id "

        Return vSql
    End Function

    Public Function rSqlDDInvidocBanktitle()
        Dim vSql As String = "select bank_code, real_title "
        vSql += "from redebt_bank_code "
        vSql += "where disable = 0 and bank_code <> 0 "
        vSql += "order by bank_popular, bank_code "

        Return vSql
    End Function
    
#End Region


#Region "loadUpdatePage"
    Public Sub checkPage(ByVal vRequest_id As String, ByVal pageUrl As String)
        Dim vSql As String 
        vSql = "select subject_url from request "
        vSql += "join subject_dim on request.subject_id = subject_dim.subject_id "
        vSql += "where request.request_id = '" + vRequest_id + "'"

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() = 0 Then
            CP.kickDefault("norequest")
        Else If vDT.Rows(0).Item("subject_url") <> pageUrl Then
            CP.kickDefault("pagefailed")
        End If
    End Sub

    Public Function rSqlLoadDetailRedebt(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select request.subject_id, subject_prefix, subject_name, project_name, "
        vSql += "request.request_id, request.request_title_id, request_title_dim.request_title, "
        vSql += "status_name, request_status, request_step, request_remark, "
        vSql += "request_file1, request_file2, request_file3, request_file4, request_file5, "
        vSql += "case when upload_date1 is null then '' else CONVERT(VARCHAR(7), upload_date1, 126) end path_file1, "
        vSql += "case when upload_date2 is null then '' else CONVERT(VARCHAR(7), upload_date2, 126) end path_file2, "
        vSql += "case when upload_date3 is null then '' else CONVERT(VARCHAR(7), upload_date3, 126) end path_file3, "
        vSql += "case when upload_date4 is null then '' else CONVERT(VARCHAR(7), upload_date4, 126) end path_file4, "
        vSql += "case when upload_date5 is null then '' else CONVERT(VARCHAR(7), upload_date5, 126) end path_file5, "
        vSql += "account_number, account_name, coalesce(account_number_to, '') account_number_to, coalesce(account_name_to, '') account_name_to, "
        vSql += "coalesce(convert(varchar(10), dx01, 103), '') dx01, " 'dx01=ันที่ชำระผิด
        vSql += "coalesce(convert(varchar(10), dx02, 103), '') dx02, " 'dx02=วันที่ยกเลิก
        vSql += "coalesce(convert(varchar(10), dx03, 103), '') dx03, " 'dx03=วันที่ลูกค้าขอลดหนี้
        vSql += "doc_number, amount, bcs_number, redebt_number, area_ro, shop_code, "
        vSql += "uemail_verify1, uemail_verify2, uemail_approve, uemail_takecn, uemail_cc1, uemail_cc2, uemail_ccv1, request.create_by, request.create_ro, request.create_shop, "
        vSql += "coalesce(convert(varchar(10), request.create_date, 103), '') create_date, "
        vSql += "case when request.update_by is null then request.create_by else request.update_by end update_by, "
        vSql += "case when request.update_date is null then request.create_date else request.update_date end update_date, "
        vSql += "mx01, mx02, mx03, " 'mx01=คำนวณจาก, mx02=ใบเสร็จออกจากช่องทาง, mx03=ชื่อบัญชี
        vSql += "tx01, tx02, tx03, " 'tx01=ลูกค้าต้องการคืนเงินเป็น, tx02=ไม่คืนเงินลูกค้า*ฟิลนี้ไม่ใช้แล้ว, tx03=สาเหตุใบรับคืนต่าง Account
        vSql += "fx01, fx02, fx03, " 'fx01=รหัสสาขาธนาคาร, fx02=เลขที่บัญชี, fx03=accno ลูกค้าต่าง Account 
        vSql += "nx01, bank_title, sx01, " 'nx01=bank_code, sx01=ประเภทใบรับคืน
        vSql += "gx01, gx02, gx03, " 'gx01=เลขที่ใบรับคืนอุปกรณ์, gx02=ไเลขที่ใบรับคืน Adapter, gx03=ชื่อลูกค้าต่าง Account
        vSql += "pick_refund, isnull(pick_refund_title,'null') pick_refund_title, isnull(pick_refund_type,0) pick_refund_type, " 
        vSql += "request.redebt_cause_id, isnull(redebt_cause_title,'null') redebt_cause_title "
        vSql += "from request "

        vSql += "left join request_file rfile "
        vSql += "on rfile.request_id = request.request_id "

        vSql += "left join cnepay.dbo.vw_cn_epay epay "
        vSql += "on epay.cn_no = request.redebt_number "

        vSql += "left join subject_dim "
        vSql += "on subject_dim.subject_id = request.subject_id "

        vSql += "left join project_dim "
        vSql += "on project_dim.project_id = subject_dim.project_id "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = request.request_status "

        vSql += "left join redebt_cause_title rt "
        vSql += "on rt.redebt_cause_id = request.redebt_cause_id "

        vSql += "left join request_title_dim "
        vSql += "on request_title_dim.request_title_id = request.request_title_id "

        vSql += "left join pick_refund "
        vSql += "on pick_refund.pick_refund_id = request.pick_refund "

        vSql += "left join redebt_bank_code "
        vSql += "on redebt_bank_code.bank_code = request.nx01 "

        vSql += "where request.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rSqlLoadBackof(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select request.subject_id, subject_prefix, subject_name, project_name, "
        vSql += "request.request_id, request.request_title_id, request_title_dim.request_title, "
        vSql += "status_name, request_status, request_step, request_remark, "
        vSql += "request_file1, request_file2, request_file3, "
        vSql += "case when upload_date1 is null then '' else CONVERT(VARCHAR(7), upload_date1, 126) end path_file1, "
        vSql += "case when upload_date2 is null then '' else CONVERT(VARCHAR(7), upload_date2, 126) end path_file2, "
        vSql += "case when upload_date3 is null then '' else CONVERT(VARCHAR(7), upload_date3, 126) end path_file3, "
        vSql += "coalesce(convert(varchar(10), dx01, 103), '') dx01, " 'dx01=เริ่มแสดงผลวันที่
        vSql += "coalesce(convert(varchar(10), dx02, 103), '') dx02, " 'dx02=แสดงผลถึงวันที่
        vSql += "coalesce(convert(varchar(10), dx03, 103), '') dx03, " 'dx03=เริ่มปิดวันที่
        vSql += "coalesce(convert(varchar(10), dx04, 103), '') dx04, " 'dx04=ปิดถึงวันที่
        vSql += "area_ro, shop_code, fx01, tx01, tx02, mx01, " 'fx01=สาขาที่ต้องการแจ้ง
        vSql += "uemail_verify1, uemail_verify2, uemail_approve, uemail_cc1, uemail_cc2, uemail_ccv1, request.create_by, request.create_ro, request.create_shop, "
        vSql += "coalesce(convert(varchar(10), request.create_date, 103), '') create_date, "
        vSql += "case when request.update_by is null then request.create_by else request.update_by end update_by, "
        vSql += "case when request.update_date is null then request.create_date else request.update_date end update_date "
        vSql += "from request "

        vSql += "left join request_file rfile "
        vSql += "on rfile.request_id = request.request_id "

        vSql += "left join subject_dim "
        vSql += "on subject_dim.subject_id = request.subject_id "

        vSql += "left join project_dim "
        vSql += "on project_dim.project_id = subject_dim.project_id "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = request.request_status "

        vSql += "left join request_title_dim "
        vSql += "on request_title_dim.request_title_id = request.request_title_id "

        vSql += "where request.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rSqlLoadDetailProjectC(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select request.subject_id, subject_prefix, subject_name, project_name, "
        vSql += "request.request_id, request.request_title_id, request_title_dim.request_title, "
        vSql += "status_name, request_status, request_step, request_remark, "
        vSql += "request_file1, request_file2, request_file3, request_file4, request_file5, "
        vSql += "request_file6, request_file7, request_file8, request_file9, request_file10, "
        vSql += "request_file11, request_file12, request_file13, request_file14, request_file15, "
        vSql += "case when upload_date1 is null then '' else CONVERT(VARCHAR(7), upload_date1, 126) end path_file1, "
        vSql += "case when upload_date2 is null then '' else CONVERT(VARCHAR(7), upload_date2, 126) end path_file2, "
        vSql += "case when upload_date3 is null then '' else CONVERT(VARCHAR(7), upload_date3, 126) end path_file3, "
        vSql += "case when upload_date4 is null then '' else CONVERT(VARCHAR(7), upload_date4, 126) end path_file4, "
        vSql += "case when upload_date5 is null then '' else CONVERT(VARCHAR(7), upload_date5, 126) end path_file5, "
        vSql += "case when upload_date6 is null then '' else CONVERT(VARCHAR(7), upload_date6, 126) end path_file6, "
        vSql += "case when upload_date7 is null then '' else CONVERT(VARCHAR(7), upload_date7, 126) end path_file7, "
        vSql += "case when upload_date8 is null then '' else CONVERT(VARCHAR(7), upload_date8, 126) end path_file8, "
        vSql += "case when upload_date9 is null then '' else CONVERT(VARCHAR(7), upload_date9, 126) end path_file9, "
        vSql += "case when upload_date10 is null then '' else CONVERT(VARCHAR(7), upload_date10, 126) end path_file10, "
        vSql += "case when upload_date11 is null then '' else CONVERT(VARCHAR(7), upload_date11, 126) end path_file11, "
        vSql += "case when upload_date12 is null then '' else CONVERT(VARCHAR(7), upload_date12, 126) end path_file12, "
        vSql += "case when upload_date13 is null then '' else CONVERT(VARCHAR(7), upload_date13, 126) end path_file13, "
        vSql += "case when upload_date14 is null then '' else CONVERT(VARCHAR(7), upload_date14, 126) end path_file14, "
        vSql += "case when upload_date15 is null then '' else CONVERT(VARCHAR(7), upload_date15, 126) end path_file15, "
        vSql += "amount, area_ro, shop_code, fx01, fx02, fx03, mx01, mx02, mx03, tx01, tx02, sx01, nx01, nx02, nx03, "
        vSql += "ax01, ax02, ax03, ax04, ax05, ax06, ax07, ax08, ax09, ax10, ax11, ax12, ax13, ax14, ax15, "
        vSql += "ax16, ax17, ax18, ax19, ax20,"
        vSql += "uemail_verify1, uemail_verify2, uemail_approve, uemail_cc1, uemail_cc2, uemail_ccv1, request.create_by, request.create_ro, request.create_shop, "
        vSql += "coalesce(convert(varchar(10), request.create_date, 103), '') create_date, "
        vSql += "case when request.update_by is null then request.create_by else request.update_by end update_by, "
        vSql += "case when request.update_date is null then request.create_date else request.update_date end update_date "
        vSql += "from request "

        vSql += "left join request_file rfile "
        vSql += "on rfile.request_id = request.request_id "

        vSql += "left join subject_dim "
        vSql += "on subject_dim.subject_id = request.subject_id "

        vSql += "left join project_dim "
        vSql += "on project_dim.project_id = subject_dim.project_id "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = request.request_status "

        vSql += "left join request_title_dim "
        vSql += "on request_title_dim.request_title_id = request.request_title_id "

        vSql += "where request.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rSqlLoadDetailProjectD(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select request.subject_id, subject_prefix, subject_name, project_name, "
        vSql += "request.request_id, request.request_title_id, request_title_dim.request_title, "
        vSql += "shop_type.TypeName, payback_status.payback_name, "
        vSql += "status_name, request_status, request_step, request_remark, "
        vSql += "request_file1, request_file2, request_file3, request_file4, request_file5, "
        vSql += "request_file6, request_file7, request_file8, request_file9, request_file10, "
        vSql += "request_file11, request_file12, request_file13, request_file14, request_file15, "
        vSql += "request_file16, request_file17, "
        vSql += "case when upload_date1 is null then '' else CONVERT(VARCHAR(7), upload_date1, 126) end path_file1, "
        vSql += "case when upload_date2 is null then '' else CONVERT(VARCHAR(7), upload_date2, 126) end path_file2, "
        vSql += "case when upload_date3 is null then '' else CONVERT(VARCHAR(7), upload_date3, 126) end path_file3, "
        vSql += "case when upload_date4 is null then '' else CONVERT(VARCHAR(7), upload_date4, 126) end path_file4, "
        vSql += "case when upload_date5 is null then '' else CONVERT(VARCHAR(7), upload_date5, 126) end path_file5, "
        vSql += "case when upload_date6 is null then '' else CONVERT(VARCHAR(7), upload_date6, 126) end path_file6, "
        vSql += "case when upload_date7 is null then '' else CONVERT(VARCHAR(7), upload_date7, 126) end path_file7, "
        vSql += "case when upload_date8 is null then '' else CONVERT(VARCHAR(7), upload_date8, 126) end path_file8, "
        vSql += "case when upload_date9 is null then '' else CONVERT(VARCHAR(7), upload_date9, 126) end path_file9, "
        vSql += "case when upload_date10 is null then '' else CONVERT(VARCHAR(7), upload_date10, 126) end path_file10, "
        vSql += "case when upload_date11 is null then '' else CONVERT(VARCHAR(7), upload_date11, 126) end path_file11, "
        vSql += "case when upload_date12 is null then '' else CONVERT(VARCHAR(7), upload_date12, 126) end path_file12, "
        vSql += "case when upload_date13 is null then '' else CONVERT(VARCHAR(7), upload_date13, 126) end path_file13, "
        vSql += "case when upload_date14 is null then '' else CONVERT(VARCHAR(7), upload_date14, 126) end path_file14, "
        vSql += "case when upload_date15 is null then '' else CONVERT(VARCHAR(7), upload_date15, 126) end path_file15, "
        vSql += "case when upload_date16 is null then '' else CONVERT(VARCHAR(7), upload_date16, 126) end path_file16, "
        vSql += "case when upload_date17 is null then '' else CONVERT(VARCHAR(7), upload_date17, 126) end path_file17, "
        vSql += "amount, area_ro, shop_code, fx01, fx02, fx03, mx01, mx02, mx03, tx01, tx02, "
        vSql += "coalesce(convert(varchar(10), dx01, 103), '') dx01, "
        vSql += "sx01, sx02, nx01, nx02, nx03, "
        vSql += "ax01, ax02, ax03, ax04, ax05, ax06, ax07, ax08, ax09, ax10, ax11, ax12, ax13, ax14, ax15, "
        vSql += "ax16, ax17, ax18, ax19, ax20,"
        vSql += "uemail_verify1, uemail_verify2, uemail_approve, uemail_cc1, uemail_cc2, uemail_ccv1, request.create_by, request.create_ro, request.create_shop, "
        vSql += "coalesce(convert(varchar(10), request.create_date, 103), '') create_date, "
        vSql += "case when request.update_by is null then request.create_by else request.update_by end update_by, "
        vSql += "case when request.update_date is null then request.create_date else request.update_date end update_date "
        vSql += "from request "

        vSql += "left join request_file rfile "
        vSql += "on rfile.request_id = request.request_id "

        vSql += "left join subject_dim "
        vSql += "on subject_dim.subject_id = request.subject_id "

        vSql += "left join project_dim "
        vSql += "on project_dim.project_id = subject_dim.project_id "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = request.request_status "

        vSql += "left join request_title_dim "
        vSql += "on request_title_dim.request_title_id = request.request_title_id "

        vSql += "left join [RequestShop].[dbo].[ShopType] as shop_type "
        vSql += "on shop_type.Type_ID = request.sx01 "

        vSql += "left join payback_status "
        vSql += "on payback_status.payback_id = request.sx02 "

        vSql += "where request.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rSqlLoadDetailProjectE(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select request.subject_id, subject_prefix, subject_name, project_name, "
        vSql += "request.request_id, request.request_title_id, request_title_dim.request_title, "
        vSql += "shop_type.TypeName, payback_status.payback_name, invdoc_payment.payment_title, redebt_bank_code.real_title, "
        vSql += "status_name, request_status, request_step, request_remark, next_depart, "
        vSql += "request_file1, request_file2, request_file3, request_file4, request_file5, "
        vSql += "request_file6, request_file7, request_file8, request_file9, request_file10, "
        vSql += "request_file11, request_file12, request_file13, request_file14, request_file15, "
        vSql += "request_file16, request_file17, "
        vSql += "case when upload_date1 is null then '' else CONVERT(VARCHAR(7), upload_date1, 126) end path_file1, "
        vSql += "case when upload_date2 is null then '' else CONVERT(VARCHAR(7), upload_date2, 126) end path_file2, "
        vSql += "case when upload_date3 is null then '' else CONVERT(VARCHAR(7), upload_date3, 126) end path_file3, "
        vSql += "case when upload_date4 is null then '' else CONVERT(VARCHAR(7), upload_date4, 126) end path_file4, "
        vSql += "case when upload_date5 is null then '' else CONVERT(VARCHAR(7), upload_date5, 126) end path_file5, "
        vSql += "case when upload_date6 is null then '' else CONVERT(VARCHAR(7), upload_date6, 126) end path_file6, "
        vSql += "case when upload_date7 is null then '' else CONVERT(VARCHAR(7), upload_date7, 126) end path_file7, "
        vSql += "case when upload_date8 is null then '' else CONVERT(VARCHAR(7), upload_date8, 126) end path_file8, "
        vSql += "case when upload_date9 is null then '' else CONVERT(VARCHAR(7), upload_date9, 126) end path_file9, "
        vSql += "case when upload_date10 is null then '' else CONVERT(VARCHAR(7), upload_date10, 126) end path_file10, "
        vSql += "case when upload_date11 is null then '' else CONVERT(VARCHAR(7), upload_date11, 126) end path_file11, "
        vSql += "case when upload_date12 is null then '' else CONVERT(VARCHAR(7), upload_date12, 126) end path_file12, "
        vSql += "case when upload_date13 is null then '' else CONVERT(VARCHAR(7), upload_date13, 126) end path_file13, "
        vSql += "case when upload_date14 is null then '' else CONVERT(VARCHAR(7), upload_date14, 126) end path_file14, "
        vSql += "case when upload_date15 is null then '' else CONVERT(VARCHAR(7), upload_date15, 126) end path_file15, "
        vSql += "case when upload_date16 is null then '' else CONVERT(VARCHAR(7), upload_date16, 126) end path_file16, "
        vSql += "case when upload_date17 is null then '' else CONVERT(VARCHAR(7), upload_date17, 126) end path_file17, "
        vSql += "doc_number, amount, area_ro, shop_code, fx01, fx02, fx03, mx01, mx02, mx03, tx01, tx02, tx03, "
        vSql += "coalesce(convert(varchar(10), dx01, 103), '') dx01, "
        vSql += "coalesce(convert(varchar(10), dx02, 103), '') dx02, "
        vSql += "coalesce(convert(varchar(10), dx03, 103), '') dx03, "
        vSql += "coalesce(convert(varchar(10), dx04, 103), '') dx04, "
        vSql += "sx01, sx02, nx01, nx02, nx03, "
        vSql += "ax01, ax02, ax03, ax04, ax05, ax06, ax07, ax08, ax09, ax10, ax11, ax12, ax13, ax14, ax15, "
        vSql += "ax16, ax17, ax18, ax19, ax20,"
        vSql += "uemail_verify1, uemail_verify2, uemail_approve, uemail_cc1, uemail_cc2, uemail_ccv1, request.create_by, request.create_ro, request.create_shop, "
        vSql += "coalesce(convert(varchar(10), request.create_date, 103), '') create_date, "
        vSql += "case when request.update_by is null then request.create_by else request.update_by end update_by, "
        vSql += "case when request.update_date is null then request.create_date else request.update_date end update_date, "
        vSql += "isnull(invdoc_runid, '') invdoc_runid, isnull(invdoc_ref, '') invdoc_ref, "
        vSql += "coalesce(convert(varchar(10), invdoc_update, 103), '') invdoc_update "
        vSql += "from request "

        vSql += "left join request_file rfile "
        vSql += "on rfile.request_id = request.request_id "

        vSql += "left join subject_dim "
        vSql += "on subject_dim.subject_id = request.subject_id "

        vSql += "left join project_dim "
        vSql += "on project_dim.project_id = subject_dim.project_id "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = request.request_status "

        vSql += "left join request_title_dim "
        vSql += "on request_title_dim.request_title_id = request.request_title_id "

        vSql += "left join [RequestShop].[dbo].[ShopType] as shop_type "
        vSql += "on shop_type.Type_ID = request.sx01 "

        vSql += "left join payback_status "
        vSql += "on payback_status.payback_id = request.sx02 "

        vSql += "left join request_invdoc " ' << panu
        vSql += "on request_invdoc.request_id = request.request_id "

        vSql += "left join invdoc_payment " 
        vSql += "on invdoc_payment.payment_id = request.nx01 "

        vSql += "left join redebt_bank_code " 
        vSql += "on redebt_bank_code.bank_code = request.nx02 "

        vSql += "where request.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rSqlRequestFlow(ByVal vRequest_id As String) As String
        Dim vSql As String = ""
        vSql += "   select no, flow_id, depart_id, flow_step, 0 flow_sub_step, next_step, back_step, "
        vSql += "   send_uemail, uemail, approval, flow_remark, flow_file, flow_status, flow_complete, "
        vSql += "   begin_date, update_by, update_date, require_remark, require_file, '' flow_sub, add_next "
        vSql += "   from request_flow "
        vSql += "   where request_id = '" + vRequest_id + "' "
        vSql += "   and disable = '0' "
        vSql += "   union all "
        vSql += "   select no, flow_id, depart_id, flow_step, flow_sub_step, next_step, back_step, "
        vSql += "   send_uemail, uemail, approval, flow_remark, flow_file, flow_status, flow_complete, "
        vSql += "   begin_date, update_by, update_date, 0 require_remark, 0 require_file, '_sub' flow_sub, add_next "
        vSql += "   from request_flow_sub "
        vSql += "   where request_id = '" + vRequest_id + "' "
        vSql += "   and disable = '0' "

        Return vSql
    End Function

    Public Function rSqlRequestFlowReject(ByVal vRequest_id As String) As String
        Dim vSql As String = "select reject_no, flow_step, flow_sub_step, next_step, back_step, "
        vSql += "CASE WHEN send_uemail <> '' THEN send_uemail + ';' END send_uemail, "
        vSql += "CASE WHEN uemail <> '' THEN uemail + ';' END uemail, "
        vSql += "rf.depart_id, depart_name, flow_status, status_name, 1 flow_complete, "
        vSql += "rf.update_date, rf.update_by, flow_remark, flow_file, "
        vSql += "case when rf.update_date is null then '' else CONVERT(VARCHAR(7), rf.update_date, 126) end path_file "
        vSql += "from ( "
        vSql += "   select reject_no, flow_id, depart_id, flow_step, flow_sub_step, next_step, back_step, "
        vSql += "   send_uemail, uemail, flow_remark, flow_file, flow_status, "
        vSql += "   begin_date, update_by, update_date"
        vSql += "   from request_flow_xback_reject "
        vSql += "   where request_id = '" + vRequest_id + "' "
        vSql += ") rf "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = rf.flow_status "

        vSql += "left join department dm "
        vSql += "on dm.depart_id = rf.depart_id "

        vSql += "order by reject_no, flow_step, next_step, flow_sub_step "

        Return vSql
    End Function

    Public Function rLoadFlowDT(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select rf.no flow_no, flow_step, flow_sub_step, next_step, back_step, replace(next_step, '-', 'x') sort_step, "
        vSql += "CASE WHEN send_uemail <> '' THEN send_uemail + ';' END send_uemail, "
        vSql += "CASE WHEN uemail <> '' THEN uemail + ';' END uemail, "
        vSql += "rf.depart_id, depart_name, flow_status, status_name, rs.nexted, flow_complete, "
        vSql += "rf.update_date, rf.update_by, flow_remark, flow_file, "
        vSql += "case when rf.update_date is null then '' else CONVERT(VARCHAR(7), rf.update_date, 126) end path_file, "
        vSql += "rf.approval, require_remark, require_file, flow_sub, rf.add_next  "
        vSql += "from ( "
        vSql += rSqlRequestFlow(vRequest_id)
        vSql += ") rf "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = rf.flow_status "

        vSql += "left join department dm "
        vSql += "on dm.depart_id = rf.depart_id "

        vSql += "order by flow_step, sort_step, flow_sub_step "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rLoadFlowDT_pdf(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select rf.no flow_no, flow_step, flow_sub_step, next_step, back_step, replace(next_step, '-', 'x') sort_step, "
        vSql += "CASE WHEN send_uemail <> '' THEN send_uemail + ';' END send_uemail, "
        vSql += "CASE WHEN uemail <> '' THEN uemail + ';' END uemail, "
        vSql += "rf.depart_id, depart_name, flow_status, status_name, flow_complete, "
        vSql += "rf.update_date, rf.update_by, flow_remark, flow_file, "
        vSql += "case when rf.update_date is null then '' else CONVERT(VARCHAR(7), rf.update_date, 126) end path_file, "
        vSql += "rf.approval, require_remark, require_file, flow_sub, rf.add_next  "
        vSql += "from ( "

        vSql += "   select no, flow_id, depart_id, flow_step, 0 flow_sub_step, next_step, back_step, "
        vSql += "   send_uemail, uemail, approval, flow_remark, flow_file, flow_status, flow_complete, "
        vSql += "   update_by, update_date, require_remark, require_file, '' flow_sub, add_next "
        vSql += "   from request_flow "
        vSql += "   where request_id = '" + vRequest_id + "' "
        vSql += "   and disable = '0' "
        vSql += "   and next_step <> 'end' "
        vSql += "   union all "
        vSql += "   select no, flow_id, depart_id, flow_step, flow_sub_step, next_step, back_step, "
        vSql += "   send_uemail, uemail, approval, flow_remark, flow_file, flow_status, flow_complete, "
        vSql += "   update_by, update_date, 0 require_remark, 0 require_file, '_sub' flow_sub, add_next "
        vSql += "   from request_flow_sub "
        vSql += "   where request_id = '" + vRequest_id + "' "
        vSql += "   and disable = '0' "

        vSql += ") rf "

        vSql += "left join request_status rs "
        vSql += "on rs.status_id = rf.flow_status "

        vSql += "left join department dm "
        vSql += "on dm.depart_id = rf.depart_id "

        vSql += "order by flow_step, sort_step, flow_sub_step "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rViewDetail(ByVal vRequest_id As String) As Integer
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)
        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")
        Dim uemail_verify1 As String = vDTU.Rows(0).Item("uemail_verify1")
        Dim uemail_verify2 As String = vDTU.Rows(0).Item("uemail_verify2")
        Dim uemail_approve As String = vDTU.Rows(0).Item("uemail_approve")
        Dim uemail_takecn As String = vDTU.Rows(0).Item("uemail_takecn")

        Dim vDT As New DataTable
        vDT = rLoadFlowDT(vRequest_id)

        Dim vView As Integer = 0

        For i As Integer = 0 To vDT.Rows().Count() - 1
            ''''''''''''''' แก้ปัญหา Error เนื่องจาก InsertRequest&Flow ไม่สมบูรณ์ '''''''''''''''''''''''''''
            Dim checkEmpty As Boolean = False

            If IsDBNull(vDT.Rows(i).Item("uemail")) Then
                checkEmpty = True
            Else If vDT.Rows(i).Item("uemail") = "" Then
                checkEmpty = True
            End If

            If checkEmpty = True Then
                Dim vSqlIn As String = "DECLARE @newid varchar(12) "
                vSqlIn += "SET @newid = '" + vRequest_id + "' "
                vSqlIn += rSqlSetDepartRequestFlow(uemail_verify1, uemail_verify2, uemail_approve, create_by, uemail_cc1, uemail_cc2, uemail_ccv1, uemail_takecn)

                If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
                    CP.InteruptRefresh()
                End If
            End If
            ''''''''''''''' แก้ปัญหา Error เนื่องจาก InsertRequest&Flow ไม่สมบูรณ์ '''''''''''''''''''''''''''

            If rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True Or vDT.Rows(i).Item("uemail") Like "*" + Session("Uemail") + ";*"
                vView = 1
            End If
        Next

        ''''''''''''''' ถ้ามีสถานะ รอข้อมูล '''''''''''''''''''''''''''
        For i2 As Integer = 0 To vDT.Rows().Count() - 1

            If vDT.Rows(i2).Item("flow_complete") = 0 And vDT.Rows(i2).Item("flow_status") = 110 _
            And (rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True Or vDT.Rows(i2).Item("uemail") Like "*" + Session("Uemail") + ";*")
                vView = 2
            End If
        Next
        ''''''''''''''' ถ้ามีสถานะ รอข้อมูล '''''''''''''''''''''''''''

        ''''''''''''''' สิทธิ์ system_admin สามารถดูคำขอ แล้ไฟล์ได้ทั้งหมด '''''''''''''''''''''''''''
        If vView = 0 Then
            If rSpecialDepart(system_admin) = 1 Or rSpecialDepart(audit_file) = 1 Then
                vView = 1
            End If
        End If

        Return vView
    End Function

    Public Function rSpecialDepart(ByVal vDepart_id As String) As Integer
        Dim vReturn As Integer = 0

        Dim vDT As New DataTable
        vDT = CP.userDepartmentTB(Session("Uemail"))

        For i As Integer = 0 To vDT.Rows().Count() - 1
            If vDT.Rows(i).Item("depart_id") = vDepart_id Then
                vReturn = 1
            End If
        Next

        Return vReturn
    End Function

    Public Function rCheckInDepart(ByVal vRequest_id As String, ByVal vDepart_id As String) As Integer
        Dim vReturn As Integer = 0

        Dim inDepart As String = CP.userDepartment(Session("Uemail"))
        Dim vSplit_depart As String() = Regex.Split(inDepart, ",")

        For Each eSplit As String In vSplit_depart
            If vDepart_id = eSplit Then
                vReturn = 1
            End If
        Next

        Return vReturn
    End Function

    Public Function rCheckNotEndAndIsRequestor(ByVal vRequest_id As String) As Integer
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)

        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")
        Dim request_status As String = vDTU.Rows(0).Item("request_status")

        Dim vReturn As Integer = 0

        If rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True And request_status <> 100 Then
            vReturn = 1
        End If

        Return vReturn
    End Function

    Public Function rCheckNotWaitReply(ByVal vRequest_id As String) As Integer
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)

        Dim request_status As String = vDTU.Rows(0).Item("request_status")

        Dim vReturn As Integer = 0

        If request_status <> 110 Then
            vReturn = 1
        End If

        Return vReturn
    End Function

    Public Function rCheckCanEditAndIsRequestor(ByVal vRequest_id As String) As Integer
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)

        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")
        Dim request_status As String = vDTU.Rows(0).Item("request_status")

        Dim vReturn As Integer = 0

        If rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True And IsDBNull(vDTU.Rows(0).Item("last_update")) _
        And request_status <> 100 And request_status <> 105 Then
            vReturn = 1
        End If

        Return vReturn
    End Function

    Public Function rCheckCanSaveEditAndIsRequestor(ByVal vRequest_id As String) As Integer
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)

        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")
        Dim request_status As String = vDTU.Rows(0).Item("request_status")

        Dim vReturn As Integer = 0

        If IsDBNull(vDTU.Rows(0).Item("last_update")) Or request_status = 110 Then
            vReturn = 1
        End If

        Return vReturn
    End Function

    Function checkCanEditItem(ByVal vRequest_id As String, ByVal vRequest_status As String, ByVal vRequest_step As String, ByVal vNext_depart As Integer) As Integer
        Dim vReturn As Integer = 0
        
        If vRequest_status < 100 Then
            If rCheckCanEditAndIsRequestor(vRequest_id) = 1 And vRequest_step = 1 Then 
                vReturn = 1 'ถ้าพึ่งสร้างคำขอ ผู้สร้างคำขอจะยังแก้รายการได้
            End If

            If rSpecialDepart(vNext_depart) = 1 Then
                If rSpecialDepart(91) = 1 Or rSpecialDepart(92) = 1 Or rSpecialDepart(94) = 1 Then
                    vReturn = 2 'ถ้าถึง step ของ Account ตรวจสอบ ข้อมูลแจ้งหนี้ หรือ Account ออกเอกสาร แจ้งหนี้
                End If
            End If
        End If

        Return vReturn
    End Function

    Function checkCanEditInvFile(ByVal vRequest_id As String, ByVal vRequest_status As String, ByVal vRequest_step As String) As Integer
        Dim vReturn As Integer = 0
        
        If rCheckIsRequestor(vRequest_id) = 1 And vRequest_status < 100 And vRequest_step = 5 Then 
            vReturn = 1 'ผู้สร้างคำขอเท่านั้น ถึงจะแนบหลักฐานการชำระเงิน 
        End If

        Return vReturn
    End Function

    Function checkPreviewInvDocRef(ByVal vRequest_id As String, ByVal vRequest_status As String, ByVal vRequest_step As String) As Integer
        Dim vReturn As Integer = 0
        
        If vRequest_status < 100 Then
            If rSpecialDepart(91) = 1 Or rSpecialDepart(92) = 1 Or rSpecialDepart(93) = 1 Or rSpecialDepart(94) = 1 Or rSpecialDepart(95) = 1 Or rSpecialDepart(96) = 1 Then
                vReturn = 1 'preview ไฟล์หน้าเอกสารใบแจ้งหนี้มใบเสร็จรับเงิน ให้ทาง ACCOUNT ออกใบแจ้งหนี้,  ACCOUNT ตรวจสอบ และ  ACCOUNT อนุมัติ 
            End If
        End If

        Return vReturn
    End Function

    Public Function rCheckHideRedebtFileIsRequestor(ByVal vRequest_id As String, ByVal vRequest_permiss As Integer) As Integer
        Dim vSql As String = "select hide_redebt_file from request "
        vSql += "join subject_dim on subject_dim.subject_id = request.subject_id "
        vSql += "where request_id = '" + vRequest_id + "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Dim hide_redebt_file As String = vDT.Rows(0).Item("hide_redebt_file")

        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)

        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")

        Dim vReturn As Integer = 0

        If rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True And hide_redebt_file = 1 Then
            vReturn = 1
        Else If vRequest_permiss = 0 Then
            vReturn = 1
        End If

        Return vReturn
    End Function

    Public Function rCheckIsRequestor(ByVal vRequest_id As String) As Integer
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)

        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")

        Dim vReturn As Integer = 0

        If rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True Then
            vReturn = 1
        End If

        Return vReturn
    End Function

    Public Function rCheckIsTakeCN(ByVal vRequest_id As String) As Integer
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)

        Dim uemail_takecn As String = vDTU.Rows(0).Item("uemail_takecn")

        Dim vReturn As Integer = 0

        If Session("uemail") = uemail_takecn Then
            vReturn = 1
        End If

        Return vReturn
    End Function

    Public Function rGetRequestor(ByVal vRequest_id As String) As DataTable
        Dim vSql As String = "select create_ro, create_by, uemail_cc1, uemail_cc2, uemail_ccv1, uemail_verify1, uemail_verify2, uemail_approve, uemail_takecn, "
        vSql += "request_status, last_update from request "
        vSql += "where request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rLoadFlowBody(ByVal vRequest_id As String, ByVal vRequest_status As String, ByVal vRequest_step As String, Optional ByVal vReqeust_permiss As Integer = 0) As String
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)
        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")

        Dim vDT As New DataTable
        vDT = rLoadFlowDT(vRequest_id)

        Dim vTbody As String = ""
        Dim vShow_form As Integer = 0
        Dim vStep_Current As Integer = 0

        For i As Integer = 0 To vDT.Rows().Count() - 1
            Dim sub_class As String = ""
            Dim flow_step As String = vDT.Rows(i).Item("flow_step")

            If vDT.Rows(i).Item("flow_sub_step") <> 0 Then
                flow_step += "." & vDT.Rows(i).Item("flow_sub_step")
                sub_class = "flow-sub"
            End If

            vTbody += "<tr class='replace-class " & i & " " + sub_class + "' >"
            vTbody += "<td>" & (i+1) & "</td>"
            vTbody += "<td>" & flow_step & "</td>"
            vTbody += "<td>" & vDT.Rows(i).Item("next_step") & "</td>"
            vTbody += "<td>" + vDT.Rows(i).Item("depart_name") + "</td>"

            vTbody += "<td>" 
            vTbody += rSplit_uemail(vDT.Rows(i).Item("send_uemail"), vDT.Rows(i).Item("uemail"))
            vTbody += "</td>"

            Dim vLabel As String = "default"
            Dim vFile As String = ""

            If vDT.Rows(i).Item("flow_complete") = 1 Then
                vLabel = "complete"
                vFile = "-"
                If vDT.Rows(i).Item("nexted") = 3 Then
                    vLabel = "reject"
                End If
                    
                vDT.Rows(i).Item("flow_remark") = CP.rNullHyphen(vDT.Rows(i).Item("flow_remark"))

                If vDT.Rows(i).Item("flow_file").Trim() <> "" Then
                    If vReqeust_permiss = 0 Then
                        vFile = file_dont_request_permiss

                    Else If (rCheckLoginNotRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True) _
                    Or (rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True And (vRequest_status = 100 Or vDT.Rows(i).Item("flow_status") = 30 Or vDT.Rows(i).Item("flow_status") = 30))  Then
                        vFile = rLinkOpenfile(vRequest_id, vDT.Rows(i).Item("path_file"), vDT.Rows(i).Item("flow_file"))
                    Else
                        vFile = file_wait_request_end
                    End If
                End If
            End If

            vTbody += "<td><span class='flow-sts label label-" + vLabel + "'>" + vDT.Rows(i).Item("status_name") + "</span></td>"
            vTbody += "<td>" + vDT.Rows(i).Item("update_date") + "</td>"
            vTbody += "<td>" + vDT.Rows(i).Item("update_by") + "</td>"

            '***** สถานะล่าสุด ต้องไม่เท่ากับ ปิดคำขอ, ยกเลิกคำขอ, รอข้อมูล
            If vShow_form = 0  _
            And vRequest_status <> 100 And vRequest_status <> 105 And vRequest_status <> 110 _
            And vDT.Rows(i).Item("flow_complete") = 0 _
            And (vDT.Rows(i).Item("flow_step") = vRequest_step _
            Or (vDT.Rows(i).Item("flow_step") <= vRequest_step  And vDT.Rows(i).Item("next_step") = "-" )) _
            And vDT.Rows(i).Item("approval") <> "4" _
            And vDT.Rows(i).Item("uemail") Like "*" + Session("Uemail") + ";*" Then
            ' And And vStep_Current = 0 _
            ' And vDT.Rows(i).Item("next_step") <> "-" _

                vTbody += "<td colspan='2'>"

                If vDT.Rows(i).Item("add_next") = 1 Then
                    Dim vSql3 As String
                    vSql3 = "select department.depart_id, depart_name "
                    vSql3 += "from department "
                    vSql3 += "join depart_user on depart_user.depart_id = department.depart_id "
                    vSql3 += "where disable = 0 and hide_add_next = 0 "
                    vSql3 += "group by department.depart_id, depart_name "
                    vSql3 += "order by 1, 2 "

                    Dim vDT3 As New DataTable
                    vDT3 = DB105.GetDataTable(vSql3)

                    vTbody += "<div class='panel panel-danger'>"
                    vTbody += "    <div class='panel-heading panel-fonting'>แทรกลำดับถัดไป</div>"
                    vTbody += "    <div class='panel-body'>"
                    vTbody += "        <div class='form-horizontal'>"
                    vTbody += "            <div class='form-group'>"
                    vTbody += "            <label class='col-sm-12 txt-red'>- หากต้องการขอข้อมูลเพิ่ม กรุณาขอข้อมูลเพิ่มให้เสร็จสิ้น ก่อนแทรกลำดับถัดไป</label>"
                    vTbody += "            <label class='col-sm-12 txt-red'>- หากต้องการแทรกลำดับถัดไป กรุณาแทรกลำดับถัดไป ก่อนทำการอนุมัติ</label>"
                    vTbody += "            </div>"
                    vTbody += "            <div class='form-group required-n'>"
                    vTbody += "                <label class='col-sm-2 control-label'>ส่วนงาน</label>"
                    vTbody += "                <div class='col-sm-10'>"
                    vTbody += "                    <select id='sel_depart_id' class='form-control input-sm' >"
                    vTbody += "                         <option value=''>เลือกส่วนงาน</option>"

                    For i3 As Integer = 0 To vDT3.Rows().Count() - 1
                        vTbody += "<option value='" & vDT3.Rows(i3).Item("depart_id") & "'>" & vDT3.Rows(i3).Item("depart_name") & "</option>"
                    Next

                    vTbody += "                    </select>"
                    vTbody += "                </div>"
                    vTbody += "            </div>"
                    vTbody += "            <div class='form-group'>"
                    vTbody += "                <div class='col-sm-offset-2 col-sm-10'>"
                    vTbody += "                    <button type='button' class='btn btn-sm btn-danger' id='btn_add_next_submit'>"
                    vTbody += "                        <span class='glyphicon glyphicon-floppy-save' aria-hidden='true'></span> แทรก"
                    vTbody += "                    </button>"
                    vTbody += "                </div>"
                    vTbody += "            </div>"
                    vTbody += "        </div>"
                    vTbody += "    </div>"
                    vTbody += "</div>"
                End If

                Dim require_remark As String = ""
                Dim require_file As String = ""

                If vDT.Rows(i).Item("require_remark") = 1 Then
                    require_remark = "required-f"
                End If
                If vDT.Rows(i).Item("require_file") = 1 Then
                    require_file = "required-f"
                End If

                Dim vDT2 As New DataTable
                vDT2 = rLoadStatusFormApprove(vRequest_id, vDT.Rows(i).Item("flow_step"), vDT.Rows(i).Item("approval"))

                vTbody += "<div class='panel panel-danger'>"
                vTbody += "    <div class='panel-heading panel-fonting'>ฟอร์มอนุมัติ</div>"
                vTbody += "    <div class='panel-body'>"
                vTbody += "        <div class='form-horizontal'>"
                vTbody += "            <input id='flow_no' type='hidden' value='" & vDT.Rows(i).Item("flow_no") & "'>"
                vTbody += "            <input id='flow_sub' type='hidden' value='" & vDT.Rows(i).Item("flow_sub") & "'>"
                vTbody += "            <input id='next_step' type='hidden' value='" & vDT.Rows(i).Item("next_step") & "'>"
                vTbody += "            <input id='back_step' type='hidden' value='" & vDT.Rows(i).Item("back_step") & "'>"
                vTbody += "            <input id='department' type='hidden' value='" & vDT.Rows(i).Item("depart_id") & "'>"
                vTbody += "            <div class='form-group required-f'>"
                vTbody += "                <label class='col-sm-2 control-label'>สถานะ</label>"
                vTbody += "                <div class='col-sm-10'>"
                vTbody += "                    <select id='sel_flow_status' class='form-control input-sm' >"
                vTbody += "                         <option value=''>เลือกสถานะ</option>"

                For i2 As Integer = 0 To vDT2.Rows().Count() - 1
                    vTbody += "<option value='" & vDT2.Rows(i2).Item("status_id") & "'>" & vDT2.Rows(i2).Item("status_name") & "</option>"
                Next

                vTbody += "                    </select>"
                vTbody += "                </div>"
                vTbody += "            </div>"
                vTbody += "            <div class='form-group " + require_remark + "'>"
                vTbody += "                <label class='col-sm-2 control-label'>หมายเหตุ</label>"
                vTbody += "                <div class='col-sm-10'>"
                vTbody += "                    <textarea type='text' id='txt_flow_remark' class='form-control input-sm' rows='2' placeholder='กรอกหมายเหตุ..'></textarea>"
                vTbody += "                </div>"
                vTbody += "            </div>"
                vTbody += "            <div class='form-group " + require_file + "'>"
                vTbody += "                <label class='col-sm-2 control-label'>เอกสารประกอบ</label>"
                vTbody += "                <div class='col-sm-10'>"
                vTbody += "                    <input id='flow_file' name='flow_file' type='file' class='form-control input-sm'>"
                vTbody += "                </div>"
                vTbody += "            </div>"
                vTbody += "            <div class='form-group'>"
                vTbody += "                <div class='col-sm-offset-2 col-sm-10'>"
                vTbody += "                    <button type='button' class='btn btn-sm btn-success' id='btn_flow_submit'>"
                vTbody += "                        <span class='glyphicon glyphicon-floppy-save' aria-hidden='true'></span> บันทึก"
                vTbody += "                    </button>"
                vTbody += "                </div>"
                vTbody += "            </div>"
                vTbody += "        </div>"
                vTbody += "    </div>"
                vTbody += "</div>"
                vTbody += "</td>"

                ' If vDT.Rows(i).Item("next_step") <> "-"
                '     vShow_form = 1
                ' End If
            Else
                vTbody += "<td>" + vDT.Rows(i).Item("flow_remark") + "</td>"
                vTbody += "<td>" + vFile + "</td>"
            End If

            If vStep_Current = 0 _
            And vDT.Rows(i).Item("flow_complete") = 0 And vDT.Rows(i).Item("flow_step") = vRequest_step And vDT.Rows(i).Item("next_step") <> "-" _
            Then
                vStep_Current = 1
                vTbody = vTbody.Replace("replace-class " & i, "current-flow")
            End If

            vTbody += "</tr>"
        Next

        If vReqeust_permiss = 0 Then
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('No permission!!'); window.location = 'default.aspx';", True)
            'CP.kickDefault("nopermiss")
            'Session("request_permiss") = 1
        End If

        return vTbody
    End Function

    Public Function rLoadFlowBody_pdf(ByVal vRequest_id As String, ByVal vRequest_status As String, ByVal vRequest_step As String, Optional ByVal vReqeust_permiss As Integer = 0) As String
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)
        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")

        Dim vDT As New DataTable
        vDT = rLoadFlowDT_pdf(vRequest_id)

        Dim vTbody As String = ""
        Dim vShow_form As Integer = 0
        Dim vStep_Current As Integer = 0

        For i As Integer = 0 To vDT.Rows().Count() - 1
            Dim flow_step As String = vDT.Rows(i).Item("flow_step")

            If vDT.Rows(i).Item("flow_sub_step") <> 0 Then
                flow_step += "." & vDT.Rows(i).Item("flow_sub_step")
            End If

            vTbody += "<tr class='replace-class " & i & "'>"
            vTbody += "<td style='border: 1px solid black;'>" & (i + 1) & "</td>"
            vTbody += "<td style='border: 1px solid black;'>" & flow_step & "</td>"
            vTbody += "<td style='border: 1px solid black;'>" & vDT.Rows(i).Item("next_step") & "</td>"
            vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("depart_name") + "</td>"

            Dim vLabel As String = "default"
            Dim vFile As String = ""

            If vDT.Rows(i).Item("flow_complete") = 1 Then
                vLabel = "complete"
                vFile = "-"

                vDT.Rows(i).Item("flow_remark") = CP.rNullHyphen(vDT.Rows(i).Item("flow_remark"))

                If vDT.Rows(i).Item("flow_file").Trim() <> "" Then
                    If vReqeust_permiss = 0 Then
                        vFile = file_dont_request_permiss

                    ElseIf (rCheckLoginNotRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True) _
                    Or (rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True And (vRequest_status = 100 Or vDT.Rows(i).Item("flow_status") = 30 Or vDT.Rows(i).Item("flow_status") = 30)) Then
                        vFile = rLinkOpenfile(vRequest_id, vDT.Rows(i).Item("path_file"), vDT.Rows(i).Item("flow_file"))
                    Else
                        vFile = file_wait_request_end
                    End If
                End If
            End If

            vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("status_name") + "</td>"
            vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("update_date") + "</td>"
            vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("update_by") + "</td>"

            '***** สถานะล่าสุด ต้องไม่เท่ากับ ปิดคำขอ, ยกเลิกคำขอ, รอข้อมูล
            If vShow_form = 0 And vStep_Current = 0 _
            And vRequest_status <> 100 And vRequest_status <> 105 And vRequest_status <> 110 _
            And vDT.Rows(i).Item("flow_complete") = 0 _
            And (vDT.Rows(i).Item("flow_step") = vRequest_step _
            Or (vDT.Rows(i).Item("flow_step") <= vRequest_step And vDT.Rows(i).Item("next_step") = "-")) _
            And vDT.Rows(i).Item("next_step") <> "-" _
            And vDT.Rows(i).Item("uemail") Like "*" + Session("Uemail") + ";*" Then

                vTbody += "<td colspan='2' style='border: 1px solid black;'>"

                If vDT.Rows(i).Item("add_next") = 1 Then
                    Dim vSql3 As String
                    vSql3 = "select department.depart_id, depart_name "
                    vSql3 += "from department "
                    vSql3 += "join depart_user on depart_user.depart_id = department.depart_id "
                    vSql3 += "where disable = 0 and hide_add_next = 0 "
                    vSql3 += "group by department.depart_id, depart_name "

                    Dim vDT3 As New DataTable
                    vDT3 = DB105.GetDataTable(vSql3)

                End If

                Dim require_remark As String = ""
                Dim require_file As String = ""

                If vDT.Rows(i).Item("require_remark") = 1 Then
                    require_remark = "required-f"
                End If
                If vDT.Rows(i).Item("require_file") = 1 Then
                    require_file = "required-f"
                End If

                Dim vDT2 As New DataTable
                vDT2 = rLoadStatusFormApprove(vRequest_id, vDT.Rows(i).Item("flow_step"), vDT.Rows(i).Item("approval"))

                For i2 As Integer = 0 To vDT2.Rows().Count() - 1

                Next

                vTbody += "</td>"

                If vDT.Rows(i).Item("next_step") <> "-" Then
                    vShow_form = 1
                End If
            Else
                vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("flow_remark") + "</td>"
            End If

            If vStep_Current = 0 _
            And vDT.Rows(i).Item("flow_complete") = 0 And vDT.Rows(i).Item("flow_step") = vRequest_step And vDT.Rows(i).Item("next_step") <> "-" _
            Then
                vStep_Current = 1
                vTbody = vTbody.Replace("replace-class " & i, "current-flow")
            End If

            vTbody += "</tr>"
        Next

        If vReqeust_permiss = 0 Then
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('No permission!!'); window.location = 'default.aspx';", True)
            'CP.kickDefault("nopermiss")
            'Session("request_permiss") = 1
        End If

        Return vTbody
    End Function

    Public Function rLoadFlowBody_easy(ByVal vRequest_id As String, ByVal vRequest_status As String, ByVal vRequest_step As String, Optional ByVal vReqeust_permiss As Integer = 0) As String
        Dim vDTU As New DataTable
        vDTU = rGetRequestor(vRequest_id)
        Dim create_by As String = vDTU.Rows(0).Item("create_by")
        Dim uemail_cc1 As String = vDTU.Rows(0).Item("uemail_cc1")
        Dim uemail_cc2 As String = vDTU.Rows(0).Item("uemail_cc2")
        Dim uemail_ccv1 As String = vDTU.Rows(0).Item("uemail_ccv1")

        Dim vDT As New DataTable
        vDT = rLoadFlowDT(vRequest_id)

        Dim vTbody As String = ""
        Dim vShow_form As Integer = 0
        Dim vStep_Current As Integer = 0

        For i As Integer = 0 To vDT.Rows().Count() - 1
            Dim flow_step As String = vDT.Rows(i).Item("flow_step")

            If vDT.Rows(i).Item("flow_sub_step") <> 0 Then
                flow_step += "." & vDT.Rows(i).Item("flow_sub_step")
            End If

            vTbody += "<tr class='replace-class " & i & "'>"
            vTbody += "<td style='border: 1px solid black;'>" & (i + 1) & "</td>"
            vTbody += "<td style='border: 1px solid black;'>" & flow_step & "</td>"
            vTbody += "<td style='border: 1px solid black;'>" & vDT.Rows(i).Item("next_step") & "</td>"
            vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("depart_name") + "</td>"

            Dim vLabel As String = "default"
            Dim vFile As String = ""

            If vDT.Rows(i).Item("flow_complete") = 1 Then
                vLabel = "complete"
                vFile = "-"

                vDT.Rows(i).Item("flow_remark") = CP.rNullHyphen(vDT.Rows(i).Item("flow_remark"))

                If vDT.Rows(i).Item("flow_file").Trim() <> "" Then
                    If vReqeust_permiss = 0 Then
                        vFile = file_dont_request_permiss

                    ElseIf (rCheckLoginNotRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True) _
                    Or (rCheckLoginIsRequestor(create_by, uemail_cc1, uemail_cc2, uemail_ccv1) = True And (vRequest_status = 100 Or vDT.Rows(i).Item("flow_status") = 30 Or vDT.Rows(i).Item("flow_status") = 30)) Then
                        vFile = rLinkOpenfile(vRequest_id, vDT.Rows(i).Item("path_file"), vDT.Rows(i).Item("flow_file"))
                    Else
                        vFile = file_wait_request_end
                    End If
                End If
            End If

            vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("status_name") + "</td>"
            vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("update_date") + "</td>"
            vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("update_by") + "</td>"

            '***** สถานะล่าสุด ต้องไม่เท่ากับ ปิดคำขอ, ยกเลิกคำขอ, รอข้อมูล
            If vShow_form = 0 And vStep_Current = 0 _
            And vRequest_status <> 100 And vRequest_status <> 105 And vRequest_status <> 110 _
            And vDT.Rows(i).Item("flow_complete") = 0 _
            And (vDT.Rows(i).Item("flow_step") = vRequest_step _
            Or (vDT.Rows(i).Item("flow_step") <= vRequest_step And vDT.Rows(i).Item("next_step") = "-")) _
            And vDT.Rows(i).Item("next_step") <> "-" _
            And vDT.Rows(i).Item("uemail") Like "*" + Session("Uemail") + ";*" Then

                vTbody += "<td colspan='2' style='border: 1px solid black;'>"

                If vDT.Rows(i).Item("add_next") = 1 Then
                    Dim vSql3 As String
                    vSql3 = "select department.depart_id, depart_name "
                    vSql3 += "from department "
                    vSql3 += "join depart_user on depart_user.depart_id = department.depart_id "
                    vSql3 += "where disable = 0 and hide_add_next = 0 "
                    vSql3 += "group by department.depart_id, depart_name "

                    Dim vDT3 As New DataTable
                    vDT3 = DB105.GetDataTable(vSql3)

                End If

                Dim require_remark As String = ""
                Dim require_file As String = ""

                If vDT.Rows(i).Item("require_remark") = 1 Then
                    require_remark = "required-f"
                End If
                If vDT.Rows(i).Item("require_file") = 1 Then
                    require_file = "required-f"
                End If

                Dim vDT2 As New DataTable
                vDT2 = rLoadStatusFormApprove(vRequest_id, vDT.Rows(i).Item("flow_step"), vDT.Rows(i).Item("approval"))

                For i2 As Integer = 0 To vDT2.Rows().Count() - 1

                Next

                vTbody += "</td>"

                If vDT.Rows(i).Item("next_step") <> "-" Then
                    vShow_form = 1
                End If
            Else
                vTbody += "<td style='border: 1px solid black;'>" + vDT.Rows(i).Item("flow_remark") + "</td>"
            End If

            If vStep_Current = 0 _
            And vDT.Rows(i).Item("flow_complete") = 0 And vDT.Rows(i).Item("flow_step") = vRequest_step And vDT.Rows(i).Item("next_step") <> "-" _
            Then
                vStep_Current = 1
                vTbody = vTbody.Replace("replace-class " & i, "current-flow")
            End If

            vTbody += "</tr>"
        Next

        If vReqeust_permiss = 0 Then
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('No permission!!'); window.location = 'default.aspx';", True)
            'CP.kickDefault("nopermiss")
            'Session("request_permiss") = 1
        End If

        Return vTbody
    End Function

    Public Function rCheckLoginNotRequestor(ByVal create_by As String, ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String) As Boolean
        Dim vReturn As Boolean = False

        If Session("uemail") <> create_by And Session("uemail") <> uemail_cc1 And Session("uemail") <> uemail_cc2 And Session("uemail") <> uemail_ccv1 Then
            vReturn = True
        Else If rGetLoginIsGroupEmail(uemail_cc1, uemail_cc2, uemail_ccv1) = 0 Then
            vReturn = True
        End If

        Return vReturn
    End Function

    Public Function rCheckLoginIsRequestor(ByVal create_by As String, ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String) As Boolean
        Dim vReturn As Boolean = False

        If Session("uemail") = create_by Or Session("uemail") = uemail_cc1 Or Session("uemail") = uemail_cc2 Or Session("uemail") = uemail_ccv1 Then
            vReturn = True
        Else If rGetLoginIsGroupEmail(uemail_cc1, uemail_cc2, uemail_ccv1) > 0 Then
            vReturn = True
        End If

        Return vReturn
    End Function

    Public Function rCheckLoginCanOpenFile(ByVal vRequest_id As String) As Integer

        ''''''''''''''' สิทธิ์ system_admin สามารถดูคำขอ และไฟล์ได้ทั้งหมด '''''''''''''''''''''''''''
        If rSpecialDepart(system_admin) = 1 Or rSpecialDepart(audit_file) = 1 Then
            Return 1

        Else 
            ' Dim vSql As String = "declare @uemail varchar(50) = '" & Session("Uemail") & "'"

            ' vSql += "select * from ( "
            ' vSql += "    select send_uemail + ';' send_uemail, uemail + ';' uemail "
            ' vSql += "    from request_flow "
            ' vSql += "    where request_id = '" & vRequest_id & "' "
            ' vSql += ") r "
            ' vSql += "where uemail like '%' + @uemail + ';%' "
            ' vSql += "or send_uemail like '%' + @uemail + ';%' "

            Dim vSql As String = ""
            vSql += "declare @request_id varchar(50) = '" & vRequest_id & "' "
            vSql += "declare @uemail varchar(50) = '" & Session("Uemail") & "' "
            vSql += "declare @gemail_cc1 varchar(50) "
            vSql += "declare @gemail_cc2 varchar(50) "
            vSql += "declare @gemail_ccv1 varchar(50) "

            vSql += "select @gemail_cc1 = uemail_cc1, @gemail_cc2 = uemail_cc2, @gemail_ccv1 = uemail_ccv1 "
            vSql += "from request "
            vSql += "where request_id = @request_id "

            vSql += "select uemail from ( "
            vSql += "    select send_uemail + ';' send_uemail, uemail + ';' uemail "
            vSql += "    from request_flow "
            vSql += "    where request_id = @request_id "
            vSql += ") r "
            vSql += "where uemail like '%' + @uemail + ';%' "
            vSql += "or send_uemail like '%' + @uemail + ';%' "

            vSql += "union all "

            vSql += "select uemail " ', group_email, depart_name 
            vSql += "from department "
            vSql += "join depart_user on depart_user.depart_id = department.depart_id "
            vSql += "where department.disable = 0 "
            vSql += "and (expired_date is null or expired_date >= getdate()) "
            vSql += "and (group_email = @gemail_cc1 or group_email = @gemail_cc2 or group_email = @gemail_ccv1) "
            vSql += "and uemail = @uemail "

            insertLogDebugQry(vSql)
            Dim vDT As New DataTable
            vDT = DB105.GetDataTable(vSql)

            Return vDT.Rows().Count()
        End If

    End Function

    Public Function rGetLoginIsGroupEmail(ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String) As Integer
        Dim vSql As String = "select uemail, group_email, depart_name from department "
        vSql += "join depart_user "
        vSql += "on depart_user.depart_id = department.depart_id "
        vSql += "where department.disable = 0 "
        vSql += "and (expired_date is null or expired_date >= getdate()) "
        vSql += "and (group_email = '" + uemail_cc1 + "' or group_email = '" + uemail_cc2 + "' or group_email = '" + uemail_ccv1 + "') "
        vSql += "and uemail = '" + Session("uemail") + "' "

        'CP.echo(vSql)

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Return vDT.Rows().Count()
    End Function

    Public Function rLoadStatusFormApprove(ByVal vRequest_id As String, ByVal vFlow_step As String, ByVal vApproval As String) As DataTable
        Dim vSql As String
        vSql = "select status_id, status_name "
        vSql += "from ( "
        vSql += "    select *, 'jj' for_join "
        vSql += "    from request_status rs "
        vSql += ") rs "
        vSql += "join ( "
        vSql += "    select count(1) count_sub, 'jj' for_join "
        vSql += "    from request_flow_sub "
        vSql += "    where request_id = '" & vRequest_id & "' "
        vSql += "    and flow_step = '" & vFlow_step & "' "
        vSql += "    and flow_reply = 0 "
        vSql += "    and disable = 0 "
        vSql += ") cs "
        vSql += "on cs.for_join = rs.for_join "
        vSql += "and (count_sub = 0 or (count_sub > 0 and nexted <> 2)) "
        'vSql += "and count_sub > 0 and nexted <> 2 "

        vSql += "where disable = 0 "
        ' vSql += "and (approval = 0 or approval = " & vApproval & ") "
        vSql += "and (approval = " & vApproval & " or (approval = 0 and (" & vApproval & " <> 3)))"

        '***** เพิ่มเงื่อนไขถ้าเป็น approval = 3 (step - ขีด) ให้รับทราบเฉยๆ ไม่มีขอข้อมูลเพิ่ม

        vSql += "order by status_id "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rLoadRequestLastUpdate(ByVal vRequest_id As String, ByVal num_last_update As String) As Integer
        Dim vSql As String
        vSql = "SELECT request_id, request_status, next_depart, last_depart "
        vSql += ", dbo.numDatetime(last_update) num_last_update "
        vSql += ", dbo.numDatetime(update_date) num_update_date "
        vSql += ", dbo.numDatetime(create_date) num_create_date "
        vSql += "FROM  request "
        vSql += "where request_id = '" & vRequest_id & "' "
        vSql += "and ( "
        vSql += "   dbo.numDatetime(last_update) = '" & num_last_update & "' "
        vSql += "   or "
        vSql += "   dbo.numDatetime(create_date) = '" & num_last_update & "' "
        vSql += ") "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Return vDT.Rows().Count()
    End Function

    Public Function rLoadRequestFlowCurrentStep(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select all_request_flow.* "
        vSql += "from vw_all_request_flow all_request_flow "

        vSql += "join vw_all_request_flow_on "
        vSql += "flow_on on flow_on.flow_step = all_request_flow.flow_step "
        vSql += "and flow_on.flow_sub_step = all_request_flow.flow_sub_step "
        vSql += "and flow_on.request_id = all_request_flow.request_id "

        vSql += "join ( "
        vSql += "    select request_id, min(str_step) min_str_step "
        vSql += "    from vw_all_request_flow_on "
        vSql += "    group by request_id "
        vSql += ") flow_next_step "
        vSql += "on flow_next_step.min_str_step = flow_on.str_step "
        vSql += "and flow_next_step.request_id = flow_on.request_id "

        vSql += "where all_request_flow.request_id = '" & vRequest_id & "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rSplit_uemail(ByVal vSend_uemail As String, ByVal vUemail As String, Optional ByVal vJson As Integer = 0) As String
        Dim vSplit_uemail As String() = Regex.Split(vUemail, ";")
        Dim vAll_email AS String = ""

        For Each eSplit As String In vSplit_uemail
            If eSplit.Trim() <> "" Then
                vAll_email += "<p>" + eSplit + "@jasmine.com</p>"
            End If
        Next

        If vSend_uemail <> vUemail And vJson = 0 Then
            vSend_uemail = vSend_uemail.Replace(";", "") + "@jasmine.com"
            vAll_email = "<strong data-container='body' data-toggle='popover' data-placement='bottom' data-content='" + vAll_email + "'>" + vSend_uemail + "</strong>"
        End If
            
        Return vAll_email
    End Function

    Public Function rZeroRO(ByVal vRO As String) As String
        If vRO.Length = 1 Then
            Return "0" + vRO
        Else
            Return vRO
        End If
    End Function
#End Region

#Region "SaveRequest"
    Public Sub InsertLogRedebtAPI(ByVal log_no As String)
        Try
            Dim vSqlIn As String = ""
            vSqlIn += "INSERT INTO log_api_redebt ("
            vSqlIn += "log_no"
            vSqlIn += ", url"
            vSqlIn += ", ip"
            vSqlIn += ") VALUES ("
            vSqlIn += " '" + log_no + "'"
            vSqlIn += ", '" + HttpContext.Current.Request.Url.AbsoluteUri() + "'"
            vSqlIn += ", '" + HttpContext.Current.Request.UserHostAddress() + "'"
            vSqlIn += ") "

            DB105.ExecuteNonQuery(vSqlIn)

        Catch ex As Exception
        End Try
    End Sub

    Public Sub UpdateLogRedebtAPI(ByVal log_no As String, ByVal request_id As String, Optional ByVal log_upload As String = "")
        Try
            Dim vSqlIn As String 
            vSqlIn += "update log_api_redebt set "
            vSqlIn += "log_complete = 1, request_id = '" + request_id + "', log_update_date = getdate() "

            If log_upload <> "" Then
                vSqlIn += ", log_upload = '" + log_upload + "' "
            End If

            vSqlIn += "where log_no = '" + log_no + "' "

            DB105.ExecuteNonQuery(vSqlIn)

        Catch ex As Exception
        End Try
    End Sub
    
    Public Function InsertRequestAPIPhase2(ByVal request_id As String, ByVal flow_id As String _
        , ByVal uemail_verify1 As String, ByVal uemail_verify2 As String, ByVal uemail_approve As String _
        , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String, ByVal create_by As String  _
    ) As Boolean

        Dim vSql As String = "select flow_id from request_flow "
        vSql += "where request_id = '" + request_id + "' and disable = 0 "     

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() = 0 Then
            '''''''''''''''''''''''''''''''''''''''''''''''' Phase2 Insert Request Flow & depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''
            Dim vSqlIn As String = "DECLARE @newid varchar(12) "
            vSqlIn += "SET @newid = '" + request_id + "' "

            vSqlIn += rSqlInsertRequestFlow(flow_id)
            vSqlIn += rSqlSetDepartRequestFlow(uemail_verify1, uemail_verify2, uemail_approve, create_by, uemail_cc1, uemail_cc2, uemail_ccv1)
            '''''''''''''''''''''''''''''''''''''''''''''''' Phase2 Insert Request Flow & depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''

            If DB105.ExecuteNonQuery(vSqlIn).ToString() > 0 Then
                sendMailAndRedirect("Open_Flow", request_id, "", "ajax")
                Return True
            End If
        End If

        Return False
    End Function

    Public Function uploadFileAPIPhase2(ByVal by_log As String) As Boolean
        Dim vSql As String = "select request_id, bank_code, bank_acc, request_file1, request_file3, upload_date "
        vSql += "from " + LinkDB_VAS + ".FollowRequestAPI.dbo.log_api_upload "
        vSql += "where log_no = '" + by_log + "' "     

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() > 0 Then
            '''''''''''''''''''''''''''''''''''''''''''''''' update by API ''''''''''''''''''''''''''''''''''''''''''''''''
            Dim vSqlIn As String = ""
            vSqlIn += "update request set "
            vSqlIn += "nx01 = '" & vDT.Rows(0).Item("bank_code") & "' "
            vSqlIn += ", fx02 = '" & vDT.Rows(0).Item("bank_acc") & "' "
            vSqlIn += "where request_id = '" & vDT.Rows(0).Item("request_id") & "' "

            vSqlIn += "update request_file set "
            vSqlIn += "request_file1 = '" & vDT.Rows(0).Item("request_file1") & "' "
            vSqlIn += ", request_file3 = '" & vDT.Rows(0).Item("request_file3") & "' "
            vSqlIn += ", upload_date1 = (select upload_date from " + LinkDB_VAS + ".FollowRequestAPI.dbo.log_api_upload where log_no = '" + by_log + "') "
            vSqlIn += ", upload_date3 = (select upload_date from " + LinkDB_VAS + ".FollowRequestAPI.dbo.log_api_upload where log_no = '" + by_log + "') "
            vSqlIn += "where request_id = '" & vDT.Rows(0).Item("request_id") & "' "
            '''''''''''''''''''''''''''''''''''''''''''''''' update by API ''''''''''''''''''''''''''''''''''''''''''''''''

            If DB105.ExecuteNonQuery(vSqlIn).ToString() > 0 Then
                ' sendMailAndRedirect("Open_Flow", request_id, "", "ajax")
                Return True
            End If
        End If

        Return False
    End Function

    Public Sub InsertRequest8CN(ByVal pageUrl As String, ByVal subject_id As String, ByVal prefix_id As String _
        , ByVal flow_id As String, ByVal request_title_id As String, ByVal request_title As String _
        , ByVal request_remark As String, ByVal uemail_verify1 As String, ByVal uemail_verify2 As String, ByVal uemail_approve As String, ByVal uemail_takecn As String _
        , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String, ByVal create_by As String  _
        , ByVal create_ro As String, ByVal create_shop As String, ByVal pick_refund As String _
        , Optional ByVal bcs_number As String = "", Optional ByVal doc_number As String = "", Optional ByVal amount As String = "" _
        , Optional ByVal account_number As String = "", Optional ByVal account_name As String = "" _
        , Optional ByVal account_number_to As String = "", Optional ByVal account_name_to As String = "" _
        , Optional ByVal redebt_cause_id As String = "", Optional ByVal area_ro As String = "", Optional ByVal shop_code As String = "" _
        , Optional ByVal dx01 As String = "", Optional ByVal dx02 As String = "", Optional ByVal dx03 As String = "" _
        , Optional ByVal mx01 As String = "", Optional ByVal mx02 As String = "", Optional ByVal mx03 As String = "" _
        , Optional ByVal tx01 As String = "", Optional ByVal tx02 As String = "", Optional ByVal tx03 As String = "" _
        , Optional ByVal fx01 As String = "", Optional ByVal fx02 As String = "", Optional ByVal fx03 As String = "" _
        , Optional ByVal nx01 As Integer = 0, Optional ByVal nx02 As Integer = 0, Optional ByVal nx03 As Integer = 0 _
        , Optional ByVal sx01 As Integer = 0, Optional ByVal sx02 As Integer = 0, Optional ByVal sx03 As Integer = 0 _
        , Optional ByVal byAjax As String = "", Optional ByVal dx04 As String = "", Optional ByVal specFile As Integer = 0 _
        , Optional ByVal ax01 As String = "", Optional ByVal ax02 As String = "", Optional ByVal ax03 As String = "" _
        , Optional ByVal ax04 As String = "", Optional ByVal ax05 As String = "", Optional ByVal ax06 As String = "" _
        , Optional ByVal ax07 As String = "", Optional ByVal ax08 As String = "", Optional ByVal ax09 As String = "" _
        , Optional ByVal ax10 As String = "", Optional ByVal ax11 As String = "", Optional ByVal ax12 As String = "" _
        , Optional ByVal ax13 As String = "", Optional ByVal ax14 As String = "", Optional ByVal ax15 As String = "" _
        , Optional ByVal ax16 As String = "", Optional ByVal ax17 As String = "", Optional ByVal ax18 As String = "" _
        , Optional ByVal ax19 As String = "", Optional ByVal ax20 As String = "" _
        , Optional ByVal gx01 As String = "", Optional ByVal gx02 As String = "", Optional ByVal gx03 As String = "" _
        , Optional ByVal gx04 As String = "", Optional ByVal gx05 As String = "" _
    )
        InsertRequest( _
            pageUrl, subject_id, prefix_id _
            , flow_id, request_title_id, request_title _
            , request_remark, uemail_verify1, uemail_verify2, uemail_approve _
            , uemail_cc1, uemail_cc2, uemail_ccv1, create_by  _
            , create_ro, create_shop, pick_refund _
            , bcs_number, doc_number, amount _
            , account_number, account_name _
            , account_number_to, account_name_to _
            , redebt_cause_id, area_ro, shop_code _
            , dx01, dx02, dx03 _
            , mx01, mx02, mx03 _
            , tx01, tx02, tx03 _
            , fx01, fx02, fx03 _
            , nx01, nx02, nx03 _
            , sx01, sx02, sx03 _
            , byAjax, dx04, specFile _
            , ax01, ax02, ax03 _
            , ax04, ax05, ax06 _
            , ax07, ax08, ax09 _
            , ax10, ax11, ax12 _
            , ax13, ax14, ax15 _
            , ax16, ax17, ax18 _
            , ax19, ax20 _
            , gx01, gx02, gx03 _
            , gx04, gx05 _
            , uemail_takecn _
        )
    End Sub

    Public Sub InsertRequest(ByVal pageUrl As String, ByVal subject_id As String, ByVal prefix_id As String _
        , ByVal flow_id As String, ByVal request_title_id As String, ByVal request_title As String _
        , ByVal request_remark As String, ByVal uemail_verify1 As String, ByVal uemail_verify2 As String, ByVal uemail_approve As String _
        , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String, ByVal create_by As String  _
        , ByVal create_ro As String, ByVal create_shop As String, ByVal pick_refund As String _
        , Optional ByVal bcs_number As String = "", Optional ByVal doc_number As String = "", Optional ByVal amount As String = "" _
        , Optional ByVal account_number As String = "", Optional ByVal account_name As String = "" _
        , Optional ByVal account_number_to As String = "", Optional ByVal account_name_to As String = "" _
        , Optional ByVal redebt_cause_id As String = "", Optional ByVal area_ro As String = "", Optional ByVal shop_code As String = "" _
        , Optional ByVal dx01 As String = "", Optional ByVal dx02 As String = "", Optional ByVal dx03 As String = "" _
        , Optional ByVal mx01 As String = "", Optional ByVal mx02 As String = "", Optional ByVal mx03 As String = "" _
        , Optional ByVal tx01 As String = "", Optional ByVal tx02 As String = "", Optional ByVal tx03 As String = "" _
        , Optional ByVal fx01 As String = "", Optional ByVal fx02 As String = "", Optional ByVal fx03 As String = "" _
        , Optional ByVal nx01 As Integer = 0, Optional ByVal nx02 As Integer = 0, Optional ByVal nx03 As Integer = 0 _
        , Optional ByVal sx01 As Integer = 0, Optional ByVal sx02 As Integer = 0, Optional ByVal sx03 As Integer = 0 _
        , Optional ByVal byAjax As String = "", Optional ByVal dx04 As String = "", Optional ByVal specFile As Integer = 0 _
        , Optional ByVal ax01 As String = "", Optional ByVal ax02 As String = "", Optional ByVal ax03 As String = "" _
        , Optional ByVal ax04 As String = "", Optional ByVal ax05 As String = "", Optional ByVal ax06 As String = "" _
        , Optional ByVal ax07 As String = "", Optional ByVal ax08 As String = "", Optional ByVal ax09 As String = "" _
        , Optional ByVal ax10 As String = "", Optional ByVal ax11 As String = "", Optional ByVal ax12 As String = "" _
        , Optional ByVal ax13 As String = "", Optional ByVal ax14 As String = "", Optional ByVal ax15 As String = "" _
        , Optional ByVal ax16 As String = "", Optional ByVal ax17 As String = "", Optional ByVal ax18 As String = "" _
        , Optional ByVal ax19 As String = "", Optional ByVal ax20 As String = "" _
        , Optional ByVal gx01 As String = "", Optional ByVal gx02 As String = "", Optional ByVal gx03 As String = "" _
        , Optional ByVal gx04 As String = "", Optional ByVal gx05 As String = "" _
        , Optional ByVal uemail_takecn As String = "" _
    )
        'specFile 5 หรือ 10 หรือ 15 เท่านั้น

        Dim vSqlIn As String = ""
        Try
        vSqlIn += declareDX("dx01", dx01)
        vSqlIn += declareDX("dx02", dx02)
        vSqlIn += declareDX("dx03", dx03)
        vSqlIn += declareDX("dx04", dx04)

        vSqlIn += "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + prefix_id + "' + RIGHT(LEFT(CONVERT(varchar, GETDATE(),112),6),4)"
        vSqlIn += "SET @newid = @newid + "
        vSqlIn += "    dbo.run4digit(COALESCE("
        vSqlIn += "        (select COUNT(1)+1 from request where LEFT(request_id, 7) = @newid)"
        vSqlIn += "    ,0)) "
        '''''''''''''''''''''''''''''''''''''''''''''''' Phase1 Insert Request ''''''''''''''''''''''''''''''''''''''''''''''''
        vSqlIn += "INSERT INTO request ("
        vSqlIn += "request_id"
        vSqlIn += ", subject_id"
        vSqlIn += ", request_title_id"
        vSqlIn += ", request_title"
        vSqlIn += ", request_remark"
        vSqlIn += ", uemail_verify1"
        vSqlIn += ", uemail_verify2"
        vSqlIn += ", uemail_approve"
        vSqlIn += ", uemail_takecn"
        vSqlIn += ", uemail_cc1"
        vSqlIn += ", uemail_cc2"
        vSqlIn += ", uemail_ccv1"
        vSqlIn += ", create_by"
        vSqlIn += ", create_ro"
        vSqlIn += ", create_shop"
        vSqlIn += ", create_date "
        vSqlIn += ", create_amount "
        vSqlIn += ", bcs_number "
        vSqlIn += ", doc_number"
        vSqlIn += ", amount"
        vSqlIn += ", account_number"
        vSqlIn += ", account_name"
        vSqlIn += ", account_number_to"
        vSqlIn += ", account_name_to "
        vSqlIn += ", redebt_cause_id"
        vSqlIn += ", area_ro"
        vSqlIn += ", shop_code "
        vSqlIn += ", pick_refund "
        vSqlIn += ", fx01 "
        vSqlIn += ", fx02 "
        vSqlIn += ", fx03 "
        vSqlIn += ", tx01 "
        vSqlIn += ", tx02 "
        vSqlIn += ", tx03 "
        vSqlIn += ", mx01 "
        vSqlIn += ", mx02 "
        vSqlIn += ", mx03 "
        vSqlIn += ", nx01 "
        vSqlIn += ", nx02 "
        vSqlIn += ", nx03 "
        vSqlIn += ", sx01 "
        vSqlIn += ", sx02 "
        vSqlIn += ", sx03 "
        vSqlIn += ", dx01 "
        vSqlIn += ", dx02 "
        vSqlIn += ", dx03 "
        vSqlIn += ", dx04 "
        vSqlIn += ", ax01 "
        vSqlIn += ", ax02 "
        vSqlIn += ", ax03 "
        vSqlIn += ", ax04 "
        vSqlIn += ", ax05 "
        vSqlIn += ", ax06 "
        vSqlIn += ", ax07 "
        vSqlIn += ", ax08 "
        vSqlIn += ", ax09 "
        vSqlIn += ", ax10 "
        vSqlIn += ", ax11 "
        vSqlIn += ", ax12 "
        vSqlIn += ", ax13 "
        vSqlIn += ", ax14 "
        vSqlIn += ", ax15 "
        vSqlIn += ", ax16 "
        vSqlIn += ", ax17 "
        vSqlIn += ", ax18 "
        vSqlIn += ", ax19 "
        vSqlIn += ", ax20 "
        vSqlIn += ", gx01 "
        vSqlIn += ", gx02 "
        vSqlIn += ", gx03 "
        vSqlIn += ", gx04 "
        vSqlIn += ", gx05 "
        vSqlIn += ") VALUES ("
        vSqlIn += "@newid"
        vSqlIn += ", " + subject_id + ""
        vSqlIn += ", '" + request_title_id + "'"
        vSqlIn += ", '" + request_title + "'"
        vSqlIn += ", '" + request_remark + "'"
        vSqlIn += ", '" + uemail_verify1 + "'"
        vSqlIn += ", '" + uemail_verify2 + "'"
        vSqlIn += ", '" + uemail_approve + "'"
        vSqlIn += ", '" + uemail_takecn + "'"
        vSqlIn += ", '" + uemail_cc1 + "'"
        vSqlIn += ", '" + uemail_cc2 + "'"
        vSqlIn += ", '" + uemail_ccv1 + "'"
        vSqlIn += ", '" + create_by + "'"
        vSqlIn += ", '" + create_ro + "'"
        vSqlIn += ", '" + create_shop + "'"
        vSqlIn += ", GETDATE()"
        vSqlIn += ", '" + CP.rReplaceSpace(amount) + "'"
        vSqlIn += ", '" + CP.rReplaceSpace(bcs_number) + "'"
        vSqlIn += ", '" + CP.rReplaceSpace(doc_number) + "'"
        vSqlIn += ", '" + CP.rReplaceSpace(amount) + "'"
        vSqlIn += ", '" + CP.rReplaceSpace(account_number) + "'"
        vSqlIn += ", '" + account_name + "'"
        vSqlIn += ", '" + CP.rReplaceSpace(account_number_to) + "'"
        vSqlIn += ", '" + account_name_to + "' "
        vSqlIn += ", '" + redebt_cause_id + "'"
        vSqlIn += ", '" + area_ro + "'"
        vSqlIn += ", '" + shop_code + "' "
        vSqlIn += ", '" + pick_refund + "' "
        vSqlIn += ", '" + fx01 + "' "
        vSqlIn += ", '" + fx02 + "' "
        vSqlIn += ", '" + fx03 + "' "
        vSqlIn += ", '" + tx01 + "' "
        vSqlIn += ", '" + tx02 + "' "
        vSqlIn += ", '" + tx03 + "' "
        vSqlIn += ", '" + mx01 + "' "
        vSqlIn += ", '" + mx02 + "' "
        vSqlIn += ", '" + mx03 + "' "
        vSqlIn += ", '" & nx01 & "' "
        vSqlIn += ", '" & nx02 & "' "
        vSqlIn += ", '" & nx03 & "' "
        vSqlIn += ", '" & sx01 & "' "
        vSqlIn += ", '" & sx02 & "' "
        vSqlIn += ", '" & sx03 & "' "
        vSqlIn += ", @dx01 "
        vSqlIn += ", @dx02 "
        vSqlIn += ", @dx03 "
        vSqlIn += ", @dx04 "
        vSqlIn += ", '" & ax01 & "' "
        vSqlIn += ", '" & ax02 & "' "
        vSqlIn += ", '" & ax03 & "' "
        vSqlIn += ", '" & ax04 & "' "
        vSqlIn += ", '" & ax05 & "' "
        vSqlIn += ", '" & ax06 & "' "
        vSqlIn += ", '" & ax07 & "' "
        vSqlIn += ", '" & ax08 & "' "
        vSqlIn += ", '" & ax09 & "' "
        vSqlIn += ", '" & ax10 & "' "
        vSqlIn += ", '" & ax11 & "' "
        vSqlIn += ", '" & ax12 & "' "
        vSqlIn += ", '" & ax13 & "' "
        vSqlIn += ", '" & ax14 & "' "
        vSqlIn += ", '" & ax15 & "' "
        vSqlIn += ", '" & ax16 & "' "
        vSqlIn += ", '" & ax17 & "' "
        vSqlIn += ", '" & ax18 & "' "
        vSqlIn += ", '" & ax19 & "' "
        vSqlIn += ", '" & ax20 & "' "
        vSqlIn += ", '" & gx01 & "' "
        vSqlIn += ", '" & gx02 & "' "
        vSqlIn += ", '" & gx03 & "' "
        vSqlIn += ", '" & gx04 & "' "
        vSqlIn += ", '" & gx05 & "' "
        vSqlIn += ") "
        'response.write(vSqlIn)
        vSqlIn += "select @newid newid"

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSqlIn)

        Dim vRequest_id As String = vDT.Rows(0).Item("newid")
        '''''''''''''''''''''''''''''''''''''''''''''''' Phase1 Insert Request ''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''''''''''''''' Phase2 Insert Request Flow & depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''
        vSqlIn = "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + vRequest_id + "' "

        vSqlIn += rSqlInsertRequestFlow(flow_id)
        vSqlIn += rSqlSetDepartRequestFlow(uemail_verify1, uemail_verify2, uemail_approve, create_by, uemail_cc1, uemail_cc2, uemail_ccv1, uemail_takecn)
        

        Dim vExecute As Integer = 0
        vExecute += DB105.ExecuteNonQuery(vSqlIn).ToString()
        '''''''''''''''''''''''''''''''''''''''''''''''' Phase2 Insert Request Flow & depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request File ''''''''''''''''''''''''''''''''''''''''''''''''
        If byAjax = "" Then
            If specFile <= 3 Then
                vExecute += rUploadRequestFile(vRequest_id)

            Else If specFile = 5 Then
                vExecute += rUploadRequest5File(vRequest_id)

            Else If specFile = 10 Then
                vExecute += rUploadRequest10File(vRequest_id)

            Else If specFile = 15 Then
                vExecute += rUploadRequest15File(vRequest_id)

            Else If specFile = 20 Then
                vExecute += rUploadRequest20File(vRequest_id)

            Else 
                vExecute += rUploadRequestFile(vRequest_id)
            End If
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request File '''''''''''''''''''''''''''''''''''''''''''''''' 

        '''''''''''''''''''''''''''''''''''''''''''''''' Update Pick Refund ''''''''''''''''''''''''''''''''''''''''''''''''
        ' vExecute += rUpdatePickRefund(vRequest_id)
        '''''''''''''''''''''''''''''''''''''''''''''''' Update Pick Refund '''''''''''''''''''''''''''''''''''''''''''''''' 

        If vExecute >= 1 Then
            ' Dim vUrl_Redirect As String = "update_" & pageUrl & ".aspx?request_id=" & vRequest_id

            If byAjax = "ajax" Then
                sendMailAndRedirect("Open_Flow", vRequest_id, "", "ajax")
                HttpContext.Current.Response.Write(vRequest_id)

            Else
                Dim vAlert_Mss As String = "บันทึกคำขอสำเร็จ เลขที่คำขอ " & vRequest_id & " และส่งอีเมล์แจ้ง ไปยังผู้ดำเนินการท่านแรกแล้ว"
                Dim vUrl_Redirect As String = "mode_data.aspx?all=1"

                sendMailAndRedirect("Open_Flow", vRequest_id, vAlert_Mss, vUrl_Redirect)
            End If
        Else
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');console.log('error vExecute');", True)
        End If

        Catch ex As Exception
            insertLogError("", "InsertRequest", vSqlIn)

            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');console.log('error try catch');", True)
        End Try
    End Sub

    Public Sub checkRequestFlowBroken(ByVal vRequest_id As String)
        Dim page As Page = HttpContext.Current.Handler

        Dim vSqlIn As String = ""
        vSqlIn += "select request.request_id, subject_dim.flow_id, subject_dim.subject_url  "
        vSqlIn += ",uemail_verify1, uemail_verify2, uemail_approve, uemail_takecn, uemail_cc1, uemail_cc2, request.create_by "
        vSqlIn += "from request "
        vSqlIn += "join subject_dim on subject_dim.subject_id = request.subject_id "
        ' vSqlIn += "left join request_flow on request.request_id = request_flow.request_id "
        ' vSqlIn += "where request_flow.disable = 0 and request_flow.request_id is null and request_status <> 105 "
        vSqlIn += "left join request_flow on request.request_id = request_flow.request_id and request_flow.disable = 0 "
        vSqlIn += "where request_flow.request_id is null and request_status <> 105 "
        vSqlIn += "and request.request_id = '" & vRequest_id & "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSqlIn)

        If vDT.Rows().Count() > 0 Then
            Dim flow_id As String = vDT.Rows(0).Item("flow_id")
            Dim pageUrl As String = vDT.Rows(0).Item("subject_url")
            Dim uemail_verify1 As String = vDT.Rows(0).Item("uemail_verify1")
            Dim uemail_verify2 As String = vDT.Rows(0).Item("uemail_verify2")
            Dim uemail_approve As String = vDT.Rows(0).Item("uemail_approve")
            Dim uemail_takecn As String = vDT.Rows(0).Item("uemail_takecn")
            Dim create_by As String = vDT.Rows(0).Item("create_by")
            Dim uemail_cc1 As String = vDT.Rows(0).Item("uemail_cc1")
            Dim uemail_cc2 As String = vDT.Rows(0).Item("uemail_cc2")

            '''''''''''''''''''''''''''''''''''''''''''''''' Phase2 Insert Request Flow & depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''
            vSqlIn = "DECLARE @newid varchar(12) "
            vSqlIn += "SET @newid = '" + vRequest_id + "' "

            vSqlIn += rSqlInsertRequestFlow(flow_id)
            vSqlIn += rSqlSetDepartRequestFlow(uemail_verify1, uemail_verify2, uemail_approve, create_by, uemail_cc1, uemail_cc2, uemail_verify1, uemail_takecn)

            Dim vExecute As Integer = 0
            vExecute += DB105.ExecuteNonQuery(vSqlIn).ToString()
            '''''''''''''''''''''''''''''''''''''''''''''''' Phase2 Insert Request Flow & depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''

            If vExecute >= 1 Then
                insertLogAutoRepair(vRequest_id, "checkRequestFlowBroken")

                Dim vUrl_Redirect As String = "update_" & pageUrl & ".aspx?request_id=" & vRequest_id
                sendMailAndRedirect("Open_Flow", vRequest_id, "", vUrl_Redirect)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
            End If
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "console.log('[request flow not broken]');", True)
        End If

        checkRequestEndBroken(vRequest_id)
    End Sub

    Public Sub checkRequestEndBroken(ByVal vRequest_id As String)
        Dim page As Page = HttpContext.Current.Handler

        Dim vSqlIn As String = ""
        vSqlIn += "select request.request_id, request_status, flow_status, subject_url "
        vSqlIn += "from request "
        vSqlIn += "join ( "
        vSqlIn += "    select request_id, next_step, flow_status from request_flow "
        vSqlIn += "    where disable = 0 and next_step = 'end' "
        vSqlIn += "    and request_id = '" & vRequest_id & "' "
        vSqlIn += ") rf on request.request_id = rf.request_id "
        vSqlIn += "join subject_dim on subject_dim.subject_id = request.subject_id "
        vSqlIn += "where request_status <> 100 and flow_status = 100 "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSqlIn)

        If vDT.Rows().Count() > 0 Then
            Dim pageUrl As String = vDT.Rows(0).Item("subject_url")

            vSqlIn = rSqlUpdateRequestEnd(vRequest_id)

            Dim vExecute As Integer = 0
            vExecute += DB105.ExecuteNonQuery(vSqlIn).ToString()
            '''''''''''''''''''''''''''''''''''''''''''''''' Phase2 Insert Request Flow & depart uemail ''''''''''''''''''''''''''''''''''''''''''''''''

            If vExecute >= 1 Then
                insertLogAutoRepair(vRequest_id, "checkRequestEndBroken")

                Dim vUrl_Redirect As String = "update_" & pageUrl & ".aspx?request_id=" & vRequest_id
                sendMailAndRedirect("End_Flow", vRequest_id, "", vUrl_Redirect)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
            End If
        Else
            ' Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "console.log('[request flow not broken]');", True)
        End If
    End Sub

    Public Sub UpdateRequest8CN(ByVal vRequest_id As String _
        , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String, ByVal update_by As String _
        , ByVal create_by As String, ByVal create_ro As String _
        , ByVal create_shop As String, ByVal pick_refund As String _
        , ByVal uemail_verify1 As String, ByVal uemail_verify2 As String, ByVal uemail_approve As String, ByVal uemail_takecn As String _
        , ByVal request_title_id As String, ByVal request_title As String, ByVal request_remark As String _
        , Optional ByVal bcs_number As String = "", Optional ByVal doc_number As String = "", Optional ByVal amount As String = "" _
        , Optional ByVal account_number As String = "", Optional ByVal account_name As String = "" _
        , Optional ByVal account_number_to As String = "", Optional ByVal account_name_to As String = "" _
        , Optional ByVal redebt_cause_id As String = "", Optional ByVal area_ro As String = "", Optional ByVal shop_code As String = "" _
        , Optional ByVal dx01 As String = "", Optional ByVal dx02 As String = "", Optional ByVal dx03 As String = "" _
        , Optional ByVal mx01 As String = "", Optional ByVal mx02 As String = "", Optional ByVal mx03 As String = "" _
        , Optional ByVal tx01 As String = "", Optional ByVal tx02 As String = "", Optional ByVal tx03 As String = "" _
        , Optional ByVal fx01 As String = "", Optional ByVal fx02 As String = "", Optional ByVal fx03 As String = "" _
        , Optional ByVal nx01 As Integer = 0, Optional ByVal nx02 As Integer = 0, Optional ByVal nx03 As Integer = 0 _
        , Optional ByVal sx01 As Integer = 0, Optional ByVal sx02 As Integer = 0, Optional ByVal sx03 As Integer = 0 _
        , Optional ByVal dx04 As String = "" , Optional ByVal specFile As Integer = 0 _
        , Optional ByVal ax01 As String = "", Optional ByVal ax02 As String = "", Optional ByVal ax03 As String = "" _
        , Optional ByVal ax04 As String = "", Optional ByVal ax05 As String = "", Optional ByVal ax06 As String = "" _
        , Optional ByVal ax07 As String = "", Optional ByVal ax08 As String = "", Optional ByVal ax09 As String = "" _
        , Optional ByVal ax10 As String = "", Optional ByVal ax11 As String = "", Optional ByVal ax12 As String = "" _
        , Optional ByVal ax13 As String = "", Optional ByVal ax14 As String = "", Optional ByVal ax15 As String = "" _
        , Optional ByVal ax16 As String = "", Optional ByVal ax17 As String = "", Optional ByVal ax18 As String = "" _
        , Optional ByVal ax19 As String = "", Optional ByVal ax20 As String = "" _
        , Optional ByVal gx01 As String = "", Optional ByVal gx02 As String = "", Optional ByVal gx03 As String = "" _
        , Optional ByVal gx04 As String = "", Optional ByVal gx05 As String = "" _
    )
        UpdateRequest(vRequest_id _
            , uemail_cc1, uemail_cc2, uemail_ccv1, update_by _
            , create_by, create_ro _
            , create_shop, pick_refund _
            , uemail_verify1, uemail_verify2, uemail_approve _
            , request_title_id, request_title, request_remark _
            , bcs_number, doc_number, amount _
            , account_number, account_name _
            , account_number_to, account_name_to _
            , redebt_cause_id, area_ro, shop_code _
            , dx01, dx02, dx03 _
            , mx01, mx02, mx03 _
            , tx01, tx02, tx03 _
            , fx01, fx02, fx03 _
            , nx01, nx02, nx03 _
            , sx01, sx02, sx03 _
            , dx04 , specFile _
            , ax01, ax02, ax03 _
            , ax04, ax05, ax06 _
            , ax07, ax08, ax09 _
            , ax10, ax11, ax12 _
            , ax13, ax14, ax15 _
            , ax16, ax17, ax18 _
            , ax19, ax20 _
            , gx01, gx02, gx03 _
            , gx04, gx05 _
            , uemail_takecn _
        )
    End Sub

    Public Sub UpdateRequest(ByVal vRequest_id As String _
        , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String, ByVal update_by As String _
        , ByVal create_by As String, ByVal create_ro As String _
        , ByVal create_shop As String, ByVal pick_refund As String _
        , ByVal uemail_verify1 As String, ByVal uemail_verify2 As String, ByVal uemail_approve As String _
        , ByVal request_title_id As String, ByVal request_title As String, ByVal request_remark As String _
        , Optional ByVal bcs_number As String = "", Optional ByVal doc_number As String = "", Optional ByVal amount As String = "" _
        , Optional ByVal account_number As String = "", Optional ByVal account_name As String = "" _
        , Optional ByVal account_number_to As String = "", Optional ByVal account_name_to As String = "" _
        , Optional ByVal redebt_cause_id As String = "", Optional ByVal area_ro As String = "", Optional ByVal shop_code As String = "" _
        , Optional ByVal dx01 As String = "", Optional ByVal dx02 As String = "", Optional ByVal dx03 As String = "" _
        , Optional ByVal mx01 As String = "", Optional ByVal mx02 As String = "", Optional ByVal mx03 As String = "" _
        , Optional ByVal tx01 As String = "", Optional ByVal tx02 As String = "", Optional ByVal tx03 As String = "" _
        , Optional ByVal fx01 As String = "", Optional ByVal fx02 As String = "", Optional ByVal fx03 As String = "" _
        , Optional ByVal nx01 As Integer = 0, Optional ByVal nx02 As Integer = 0, Optional ByVal nx03 As Integer = 0 _
        , Optional ByVal sx01 As Integer = 0, Optional ByVal sx02 As Integer = 0, Optional ByVal sx03 As Integer = 0 _
        , Optional ByVal dx04 As String = "" , Optional ByVal specFile As Integer = 0 _
        , Optional ByVal ax01 As String = "", Optional ByVal ax02 As String = "", Optional ByVal ax03 As String = "" _
        , Optional ByVal ax04 As String = "", Optional ByVal ax05 As String = "", Optional ByVal ax06 As String = "" _
        , Optional ByVal ax07 As String = "", Optional ByVal ax08 As String = "", Optional ByVal ax09 As String = "" _
        , Optional ByVal ax10 As String = "", Optional ByVal ax11 As String = "", Optional ByVal ax12 As String = "" _
        , Optional ByVal ax13 As String = "", Optional ByVal ax14 As String = "", Optional ByVal ax15 As String = "" _
        , Optional ByVal ax16 As String = "", Optional ByVal ax17 As String = "", Optional ByVal ax18 As String = "" _
        , Optional ByVal ax19 As String = "", Optional ByVal ax20 As String = "" _
        , Optional ByVal gx01 As String = "", Optional ByVal gx02 As String = "", Optional ByVal gx03 As String = "" _
        , Optional ByVal gx04 As String = "", Optional ByVal gx05 As String = "" _
        , Optional ByVal uemail_takecn As String = "" _
    )
        'specFile 5 หรือ 10 หรือ 15 เท่านั้น

        Dim vSqlIn As String 

        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request '''''''''''''''''''''''''''''''''''''''''''''''' 
        vSqlIn += declareDX("dx01", dx01)
        vSqlIn += declareDX("dx02", dx02)
        vSqlIn += declareDX("dx03", dx03)
        vSqlIn += declareDX("dx04", dx04)

        vSqlIn += "update request set "
        vSqlIn += "last_depart = 0 "
        vSqlIn += ", request_status = 0 "
        vSqlIn += ", request_title_id = '" + request_title_id + "'"
        vSqlIn += ", request_title = '" + request_title + "'"
        vSqlIn += ", request_remark = '" + request_remark + "'"
        vSqlIn += ", uemail_cc1 = '" + uemail_cc1 + "'"
        vSqlIn += ", uemail_cc2 = '" + uemail_cc2 + "'"
        vSqlIn += ", uemail_ccv1 = '" + uemail_ccv1 + "'"
        vSqlIn += ", uemail_verify1 = '" + uemail_verify1 + "'"
        vSqlIn += ", uemail_verify2 = '" + uemail_verify2 + "'"
        vSqlIn += ", uemail_approve = '" + uemail_approve + "'"
        vSqlIn += ", uemail_takecn = '" + uemail_takecn + "'"
        vSqlIn += ", create_ro = '" + create_ro + "'"
        vSqlIn += ", create_shop = '" + create_shop + "'"
        vSqlIn += ", update_by = '" + update_by + "'"
        vSqlIn += ", update_date = getdate() "
        vSqlIn += ", bcs_number = '" + CP.rReplaceSpace(bcs_number) + "'"
        vSqlIn += ", doc_number = '" + CP.rReplaceSpace(doc_number) + "'"
        vSqlIn += ", amount = '" + CP.rReplaceSpace(amount) + "'"
        vSqlIn += ", account_number = '" + CP.rReplaceSpace(account_number) + "'"
        vSqlIn += ", account_name = '" + account_name + "'"
        vSqlIn += ", account_number_to = '" + CP.rReplaceSpace(account_number_to) + "'"
        vSqlIn += ", account_name_to = '" + account_name_to + "'"
        vSqlIn += ", redebt_cause_id = '" + redebt_cause_id + "'"
        vSqlIn += ", area_ro = '" + area_ro + "'"
        vSqlIn += ", shop_code = '" + shop_code + "'"
        vSqlIn += ", pick_refund = '" + pick_refund + "'"
        vSqlIn += ", fx01 = '" + fx01 + "'"
        vSqlIn += ", fx02 = '" + fx02 + "'"
        vSqlIn += ", fx03 = '" + fx03 + "'"
        vSqlIn += ", mx01 = '" + mx01 + "'"
        vSqlIn += ", mx02 = '" + mx02 + "'"
        vSqlIn += ", mx03 = '" + mx03 + "'"
        vSqlIn += ", tx01 = '" + tx01 + "'"
        vSqlIn += ", tx02 = '" + tx02 + "'"
        vSqlIn += ", tx03 = '" + tx03 + "'"
        vSqlIn += ", nx01 = '" & nx01 & "'"
        vSqlIn += ", nx02 = '" & nx02 & "'"
        vSqlIn += ", nx03 = '" & nx03 & "'"
        vSqlIn += ", sx01 = '" & sx01 & "'"
        vSqlIn += ", sx02 = '" & sx02 & "'"
        vSqlIn += ", sx03 = '" & sx03 & "'"
        vSqlIn += ", dx01 = @dx01 "
        vSqlIn += ", dx02 = @dx02 "
        vSqlIn += ", dx03 = @dx03 "
        vSqlIn += ", dx04 = @dx04 "
        vSqlIn += ", ax01 = '" & ax01 & "'"
        vSqlIn += ", ax02 = '" & ax02 & "'"
        vSqlIn += ", ax03 = '" & ax03 & "'"
        vSqlIn += ", ax04 = '" & ax04 & "'"
        vSqlIn += ", ax05 = '" & ax05 & "'"
        vSqlIn += ", ax06 = '" & ax06 & "'"
        vSqlIn += ", ax07 = '" & ax07 & "'"
        vSqlIn += ", ax08 = '" & ax08 & "'"
        vSqlIn += ", ax09 = '" & ax09 & "'"
        vSqlIn += ", ax10 = '" & ax10 & "'"
        vSqlIn += ", ax11 = '" & ax11 & "'"
        vSqlIn += ", ax12 = '" & ax12 & "'"
        vSqlIn += ", ax13 = '" & ax13 & "'"
        vSqlIn += ", ax14 = '" & ax14 & "'"
        vSqlIn += ", ax15 = '" & ax15 & "'"
        vSqlIn += ", ax16 = '" & ax16 & "'"
        vSqlIn += ", ax17 = '" & ax17 & "'"
        vSqlIn += ", ax18 = '" & ax18 & "'"
        vSqlIn += ", ax19 = '" & ax19 & "'"
        vSqlIn += ", ax20 = '" & ax20 & "'"
        vSqlIn += ", gx01 = '" & gx01 & "'"
        vSqlIn += ", gx02 = '" & gx02 & "'"
        vSqlIn += ", gx03 = '" & gx03 & "'"
        vSqlIn += ", gx04 = '" & gx04 & "'"
        vSqlIn += ", gx05 = '" & gx05 & "'"
        vSqlIn += "where request_id = '" + vRequest_id + "' "

        Dim vExecute As Integer = 0
        vExecute += DB105.ExecuteNonQuery(vSqlIn).ToString()
        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request ''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request File ''''''''''''''''''''''''''''''''''''''''''''''''
        If specFile <= 3 Then
            vExecute += rUploadRequestFile(vRequest_id)

        Else If specFile = 5 Then
            vExecute += rUploadRequest5File(vRequest_id)

        Else If specFile = 10 Then
            vExecute += rUploadRequest10File(vRequest_id)

        Else If specFile = 15 Then
            vExecute += rUploadRequest15File(vRequest_id)

        Else If specFile = 20 Then
            vExecute += rUploadRequest20File(vRequest_id)

        Else 
            vExecute += rUploadRequestFile(vRequest_id)
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request File '''''''''''''''''''''''''''''''''''''''''''''''' 

        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request Flow ''''''''''''''''''''''''''''''''''''''''''''''''
        vSqlIn = "DECLARE @thisdate dateTime = getdate() "
        vSqlIn += "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + vRequest_id + "' "

        vSqlIn += "if exists (select * from request_flow_sub where request_id = @newid and disable = 0 and flow_complete = 0 and flow_status = 110) "
        vSqlIn += "begin "
        vSqlIn += "     update request set last_update = @thisdate where request_id = @newid "
        vSqlIn += "end "

        vSqlIn += "DECLARE @begin_date datetime = (select begin_date from request_flow_sub where request_id = @newid and disable = 0 and flow_complete = 0 and flow_status = 110) "

        vSqlIn += "if @begin_date is not null " 'update reply new ข้อหมูเพิ่ลแบบใหม่
        vSqlIn += "begin "
        vSqlIn += "     update request_flow_sub set "
        vSqlIn += "     flow_complete = 1 "
        vSqlIn += "     , flow_status = 115 "
        vSqlIn += "     , update_by = '" + update_by + "'"
        vSqlIn += "     , update_date = @thisdate "
        vSqlIn += "     where request_id = @newid and disable = 0 and flow_complete = 0 and flow_status = 110 "

        vSqlIn += "     update request_flow_sub set "
        vSqlIn += "     begin_date = @thisdate "
        vSqlIn += "     where request_id = @newid and disable = 0 and flow_complete = 0 and flow_status = 0 "
        vSqlIn += "end "

        vSqlIn += "else " 'update reply old ข้อหมูเพิ่ลแบบเก่า
        vSqlIn += "begin "
        vSqlIn += "     update request_flow set "
        vSqlIn += "     flow_status = 0 "
        vSqlIn += "     where request_id = @newid and disable = 0 and flow_complete = 0 and flow_status = 110 "

        vSqlIn += "     update request_flow_sub set "
        vSqlIn += "     flow_status = 0 "
        vSqlIn += "     where request_id = @newid and disable = 0 and flow_complete = 0 and flow_status = 110 "
        vSqlIn += "end "

        vSqlIn += rSqlSetDepartRequestFlow(uemail_verify1, uemail_verify2, uemail_approve, create_by, uemail_cc1, uemail_cc2, uemail_ccv1, uemail_takecn)
        
        vExecute += DB105.ExecuteNonQuery(vSqlIn).ToString()
        '''''''''''''''''''''''''''''''''''''''''''''''' Update Request Flow ''''''''''''''''''''''''''''''''''''''''''''''''

        If vExecute >= 1 Then
            'Dim vAlert_Mss As String = "ส่งอีเมล์แจ้ง ผู้สร้างคำขอและ ผู้ที่ขอข้อมูลเพิ่ม"
            Dim vAlert_Mss As String = "ส่งอีเมล์แจ้ง มีการแก้ไขข้อมูลคำขอ"
            sendMailAndRedirect("Reply_2", vRequest_id, vAlert_Mss)

        Else
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub

    Public Function rUpdatePickRefund(ByVal vRequest_id As String) As String
        Dim vSqlIn As String = "update request set pick_refund = "
        vSqlIn += "case when DATALENGTH(tx01) > 0 then 2 when DATALENGTH(tx02) > 0 then 3 else 1 end "
        vSqlIn += "where request_id = '" + vRequest_id + "'"

        Return DB105.ExecuteNonQuery(vSqlIn).ToString()
    End Function

    Public Sub UpdateRedebtNumber(ByVal vRequest_id As String, ByVal redebt_number As String, ByVal update_by As String)
        Dim redebt_file As String = CUL.rUploadFileDriveF("redebt_file", vRequest_id + "_CN")

        Dim vSqlIn As String 
        vSqlIn += "update request set "
        vSqlIn += "redebt_number = '" + redebt_number + "' "

        If redebt_file <> "" Then
            vSqlIn += ", redebt_file = '" + redebt_file + "' "
        End If

        vSqlIn += ", redebt_update_by = '" + update_by + "' "
        vSqlIn += ", redebt_update = getdate() "
        vSqlIn += "where request_id = '" + vRequest_id + "' "

        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            Dim vAlert_Mss As String = "บันทึกข้อมูลการลดหนี้แล้ว"
            sendMailAndRedirect("Update_Redebt", vRequest_id, vAlert_Mss)
        Else
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub

	Public Sub UpdateInvDocfile(ByVal vRequest_id As String, ByVal update_by As String, ByVal tx02 As String)
        Dim invfile_file As String = CUL.rUploadFileDriveF("invfile_file", vRequest_id + "_E1")

        Dim vDT As New DataTable
        vDT = rLoadRequestFlowCurrentStep(vRequest_id)

        Dim vTable_flow As String = "request_flow"
        Dim vFlow_no As String = vDT.Rows(0).Item("no")
        Dim vNext_step As String = vDT.Rows(0).Item("next_step")
        Dim vDepart_id As String = vDT.Rows(0).Item("depart_id")
        Dim vRequest_status As Integer = 60 'ดำเนินการเรียบร้อย

        Dim vSqlIn As String 

        vSqlIn += "update request set "
        vSqlIn += "redebt_number = 'InvDocFile' "
        vSqlIn += ", tx02 = '" + tx02 + "'"
        vSqlIn += ", redebt_update_by = '" + update_by + "' "
        vSqlIn += ", redebt_update = getdate() "
        vSqlIn += ", request_status = '" & vRequest_status & "', request_step = '" & vNext_step & "' "
        vSqlIn += ", last_update = getdate(), last_depart = '" & vDepart_id & "' "
        vSqlIn += "where request_id = '" + vRequest_id + "' "

        vSqlIn += "update " + vTable_flow + " set "
        vSqlIn += "flow_status = '" & vRequest_status & "', flow_remark = '', flow_file = '', " 
        vSqlIn += "flow_complete = 1, update_by = '" + update_by + "', update_date = getdate(), "
        vSqlIn += "begin_date = '" + rGetBeginDate(vTable_flow, vFlow_no) + "' "
        vSqlIn += "where no = '" & vFlow_no & "' "

        If invfile_file <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file2 = '" + invfile_file + "', upload_date2 = getdate() "
            vSqlIn += "where request_id = '" + vRequest_id + "' "
        End If

        vSqlIn += "exec dbo.SP_runInvDocID '" + vRequest_id + "' "

        insertLogDebugQry(vSqlIn)
        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            Dim vAlert_Mss As String = "บันทึกหลักฐานการชำระเงินแล้ว"
            sendMailAndRedirect("Next_Flow", vRequest_id, vAlert_Mss)
        Else
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub  

    Public Sub UpdateInvDocRef(ByVal vRequest_id As String, ByVal update_by As String, ByVal dx03 As String, ByVal invdoc_ref As String)
        Dim vSqlIn As String 
        vSqlIn += declareDX("dx03", dx03)

        vSqlIn += "update request set "
        vSqlIn += "dx03 = @dx03 "
        vSqlIn += "where request_id = '" + vRequest_id + "' "

        vSqlIn += "if exists (select * from request_invdoc where request_id = '" + vRequest_id + "') "
        vSqlIn += "begin "
        vSqlIn += "   update request_invdoc set invdoc_ref = '" & invdoc_ref & "', invdoc_update_by = '" & update_by & "', invdoc_update = getdate() "
        vSqlIn += "   where request_id = '" + vRequest_id + "' "
        vSqlIn += "end "
        vSqlIn += "else "
        vSqlIn += "begin "
        vSqlIn += "   insert into request_invdoc (request_id, invdoc_ref, invdoc_update_by, invdoc_update) "
        vSqlIn += "   values ('" + vRequest_id + "', '" & invdoc_ref & "', '" & update_by & "', getdate()) "
        vSqlIn += "end "

        insertLogDebugQry(vSqlIn)
        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            Dim vAlert_Mss As String = "บันทึกวันที่รับเงินแล้ว"
            sendMailAndRedirect("Update_InvDocRef", vRequest_id, vAlert_Mss)
        Else
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub  

    Public Sub CancleRequestInvdoc(ByVal vRequest_id As String, ByVal update_by As String)
        Dim vSqlIn As String 
        vSqlIn += "update request set "
        vSqlIn += "request_status = '105' " '***** จบคำขอ ให้สถานะคำขอเป็น ยกเลิกคำขอ
        vSqlIn += ", next_depart = '0' " '***** จบคำขอ ให้ลำดับถัดไปเป็น คนสร้างคำขอ
        vSqlIn += ", update_by = '" + update_by + "'"
        vSqlIn += ", update_date = getdate() "
        vSqlIn += "where request_id = '" + vRequest_id + "' "
        vSqlIn += "update request_invdoc set "
        vSqlIn += "invdoc_runid = '' "
        vSqlIn += "where request_id = '" + vRequest_id + "' "

        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            Dim vAlert_Mss As String = "ส่งอีเมล์แจ้ง ยกเลิกคำขอ ให้ผู้สร้างคำขอแล้ว"
            sendMailAndRedirect("Cancle_Flow", vRequest_id, vAlert_Mss)
        Else
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub

    Public Sub CancleRequest(ByVal vRequest_id As String, ByVal update_by As String)
        Dim vSqlIn As String 
        vSqlIn += "update request set "
        vSqlIn += "request_status = '105' " '***** จบคำขอ ให้สถานะคำขอเป็น ยกเลิกคำขอ
        vSqlIn += ", next_depart = '0' " '***** จบคำขอ ให้ลำดับถัดไปเป็น คนสร้างคำขอ
        vSqlIn += ", update_by = '" + update_by + "'"
        vSqlIn += ", update_date = getdate() "
        vSqlIn += "where request_id = '" + vRequest_id + "' "

        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            Dim vAlert_Mss As String = "ส่งอีเมล์แจ้ง ยกเลิกคำขอ ให้ผู้สร้างคำขอแล้ว"
            sendMailAndRedirect("Cancle_Flow", vRequest_id, vAlert_Mss)
        Else
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub

    Public Sub UpdateBackend(ByVal log_by As String, ByVal log_remark As String _
        , ByVal log_ref_id As String, ByVal request_id As String _
        , ByVal subject_id As String, ByVal request_title_id As String, ByVal request_title As String _
        , ByVal request_status As String, ByVal request_step As String, ByVal request_remark As String _
        , ByVal uemail_verify1 As String, ByVal uemail_verify2 As String, ByVal uemail_approve As String _
        , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String _
        , ByVal create_date As String, ByVal create_by As String _
        , ByVal create_ro As String, ByVal create_shop As String, ByVal create_amount As String _
        , ByVal update_date As String, ByVal update_by As String _
        , ByVal last_update As String, ByVal last_depart As String, ByVal next_depart As String _
        , ByVal account_number As String, ByVal account_name As String _
        , ByVal account_number_to As String, ByVal account_name_to As String _
        , ByVal doc_number As String, ByVal bcs_number As String _
        , ByVal redebt_number As String, ByVal redebt_file As String _
        , ByVal redebt_update As String, ByVal redebt_update_by As String _
        , ByVal redebt_cause_id As String, ByVal amount As String _
        , ByVal area_ro As String, ByVal shop_code As String _
        , ByVal pick_refund As String, ByVal lock_receipt As String _
        , ByVal fx01 As String, ByVal fx02 As String, ByVal fx03 As String _
        , ByVal mx01 As String, ByVal mx02 As String, ByVal mx03 As String _
        , ByVal tx01 As String, ByVal tx02 As String, ByVal tx03 As String _
        , ByVal nx01 As String, ByVal nx02 As String, ByVal nx03 As String _
        , ByVal sx01 As String, ByVal sx02 As String, ByVal sx03 As String _
        , ByVal dx01 As String, ByVal dx02 As String, ByVal dx03 As String, ByVal dx04 As String _
        , ByVal ax01 As String, ByVal ax02 As String, ByVal ax03 As String, ByVal ax04 As String, ByVal ax05 As String _
        , ByVal ax06 As String, ByVal ax07 As String, ByVal ax08 As String, ByVal ax09 As String, ByVal ax10 As String _
        , ByVal ax11 As String, ByVal ax12 As String, ByVal ax13 As String, ByVal ax14 As String, ByVal ax15 As String _
        , ByVal ax16 As String, ByVal ax17 As String, ByVal ax18 As String, ByVal ax19 As String, ByVal ax20 As String _
        , ByVal gx01 As String, ByVal gx02 As String, ByVal gx03 As String, ByVal gx04 As String, ByVal gx05 As String _
    )
    
        Dim vExecute As Integer = 0
        vExecute += UpdateBackendLog(log_by, log_remark _
            , log_ref_id, request_id _
            , subject_id, request_title_id, request_title _
            , request_status, request_step, request_remark _
            , uemail_verify1, uemail_verify2, uemail_approve _
            , uemail_cc1, uemail_cc2, uemail_ccv1 _
            , create_date, create_by _
            , create_ro, create_shop, create_amount _
            , update_date, update_by _
            , last_update, last_depart, next_depart _
            , account_number, account_name _
            , account_number_to, account_name_to _
            , doc_number, bcs_number _
            , redebt_number, redebt_file _
            , redebt_update, redebt_update_by _
            , redebt_cause_id, amount _
            , area_ro, shop_code _
            , pick_refund, lock_receipt _
            , fx01, fx02, fx03 _
            , mx01, mx02, mx03 _
            , tx01, tx02, tx03 _
            , nx01, nx02, nx03 _
            , sx01, sx02, sx03 _
            , dx01, dx02, dx03, dx04 _
            , ax01, ax02, ax03, ax04, ax05 _
            , ax06, ax07, ax08, ax09, ax10 _
            , ax11, ax12, ax13, ax14, ax15 _
            , ax16, ax17, ax18, ax19, ax20 _
            , gx01, gx02, gx03, gx04, gx05 _
        )

        If vExecute >= 1 Then
            Dim vSqlIn As String 
            vSqlIn += "update request set request_id = request_id "

            If subject_id.Trim() <> "valueEmpty" Then
                vSqlIn += ", subject_id = '" & subject_id & "'"
            End If

            If request_title_id.Trim() <> "valueEmpty" Then
                vSqlIn += ", request_title_id = '" & request_title_id & "'"
            End If

            If request_title.Trim() <> "valueEmpty" Then
                vSqlIn += ", request_title = '" & request_title & "'"
            End If

            If request_status.Trim() <> "valueEmpty" Then
                vSqlIn += ", request_status = '" & request_status & "'"
            End If

            If request_step.Trim() <> "valueEmpty" Then
                vSqlIn += ", request_step = '" & request_step & "'"
            End If

            If request_remark.Trim() <> "valueEmpty" Then
                vSqlIn += ", request_remark = '" & request_remark & "'"
            End If

            If uemail_verify1.Trim() <> "valueEmpty" Then
                vSqlIn += ", uemail_verify1 = '" & uemail_verify1 & "'"
            End If

            If uemail_verify2.Trim() <> "valueEmpty" Then
                vSqlIn += ", uemail_verify2 = '" & uemail_verify2 & "'"
            End If

            If uemail_approve.Trim() <> "valueEmpty" Then
                vSqlIn += ", uemail_approve = '" & uemail_approve & "'"
            End If

            If uemail_cc1.Trim() <> "valueEmpty" Then
                vSqlIn += ", uemail_cc1 = '" & uemail_cc1 & "'"
            End If

            If uemail_cc2.Trim() <> "valueEmpty" Then
                vSqlIn += ", uemail_cc2 = '" & uemail_cc2 & "'"
            End If

            If uemail_ccv1.Trim() <> "valueEmpty" Then
                vSqlIn += ", uemail_ccv1 = '" & uemail_ccv1 & "'"
            End If

            If create_date.Trim() <> "valueEmpty" Then
                vSqlIn += ", create_date = '" & create_date & "'"
            End If

            If create_by.Trim() <> "valueEmpty" Then
                vSqlIn += ", create_by = '" & create_by & "'"
            End If

            If create_ro.Trim() <> "valueEmpty" Then
                vSqlIn += ", create_ro = '" & create_ro & "'"
            End If

            If create_shop.Trim() <> "valueEmpty" Then
                vSqlIn += ", create_shop = '" & create_shop & "'"
            End If

            If create_amount.Trim() <> "valueEmpty" Then
                vSqlIn += ", create_amount = '" & create_amount & "'"
            End If

            If update_date.Trim() <> "valueEmpty" Then
                vSqlIn += ", update_date = '" & update_date & "'"
            End If

            If update_by.Trim() <> "valueEmpty" Then
                vSqlIn += ", update_by = '" & update_by & "'"
            End If

            If last_update.Trim() <> "valueEmpty" Then
                vSqlIn += ", last_update = '" & last_update & "'"
            End If

            If last_depart.Trim() <> "valueEmpty" Then
                vSqlIn += ", last_depart = '" & last_depart & "'"
            End If

            If next_depart.Trim() <> "valueEmpty" Then
                vSqlIn += ", next_depart = '" & next_depart & "'"
            End If

            If account_number.Trim() <> "valueEmpty" Then
                vSqlIn += ", account_number = '" & CP.rReplaceSpace(account_number) & "'"
            End If

            If account_name.Trim() <> "valueEmpty" Then
                vSqlIn += ", account_name = '" & account_name & "'"
            End If

            If account_number_to.Trim() <> "valueEmpty" Then
                vSqlIn += ", account_number_to = '" & CP.rReplaceSpace(account_number_to) & "'"
            End If

            If account_name_to.Trim() <> "valueEmpty" Then
                vSqlIn += ", account_name_to = '" & account_name_to & "'"
            End If

            If doc_number.Trim() <> "valueEmpty" Then
                vSqlIn += ", doc_number = '" & CP.rReplaceSpace(doc_number) & "'"
            End If

            If bcs_number.Trim() <> "valueEmpty" Then
                vSqlIn += ", bcs_number = '" & CP.rReplaceSpace(bcs_number) & "'"
            End If

            If redebt_number.Trim() <> "valueEmpty" Then
                vSqlIn += ", redebt_number = '" & CP.rReplaceSpace(redebt_number) & "'"
            End If

            If redebt_file.Trim() <> "valueEmpty" Then
                vSqlIn += ", redebt_file = '" & CP.rReplaceSpace(redebt_file) & "'"
            End If

            If redebt_update.Trim() <> "valueEmpty" Then
                vSqlIn += ", redebt_update = '" & redebt_update & "'"
            End If

            If redebt_update_by.Trim() <> "valueEmpty" Then
                vSqlIn += ", redebt_update_by = '" & redebt_update_by & "'"
            End If

            If redebt_cause_id.Trim() <> "valueEmpty" Then
                vSqlIn += ", redebt_cause_id = '" & redebt_cause_id & "'"
            End If

            If amount.Trim() <> "valueEmpty" Then
                vSqlIn += ", amount = '" & amount & "'"
            End If

            If area_ro.Trim() <> "valueEmpty" Then
                vSqlIn += ", area_ro = '" & area_ro & "'"
            End If

            If shop_code.Trim() <> "valueEmpty" Then
                vSqlIn += ", shop_code = '" & shop_code & "'"
            End If

            If pick_refund.Trim() <> "valueEmpty" Then
                vSqlIn += ", pick_refund = '" & pick_refund & "'"
            End If

            If lock_receipt.Trim() <> "valueEmpty" Then
                vSqlIn += ", lock_receipt = '" & lock_receipt & "'"
            End If

            If fx01.Trim() <> "valueEmpty" Then
                vSqlIn += ", fx01 = '" & fx01 & "'"
            End If

            If fx02.Trim() <> "valueEmpty" Then
                vSqlIn += ", fx02 = '" & fx02 & "'"
            End If

            If fx03.Trim() <> "valueEmpty" Then
                vSqlIn += ", fx03 = '" & fx03 & "'"
            End If

            If mx01.Trim() <> "valueEmpty" Then
                vSqlIn += ", mx01 = '" & mx01 & "'"
            End If

            If mx02.Trim() <> "valueEmpty" Then
                vSqlIn += ", mx02 = '" & mx02 & "'"
            End If

            If mx03.Trim() <> "valueEmpty" Then
                vSqlIn += ", mx03 = '" & mx03 & "'"
            End If

            If tx01.Trim() <> "valueEmpty" Then
                vSqlIn += ", tx01 = '" & tx01 & "'"
            End If

            If tx02.Trim() <> "valueEmpty" Then
                vSqlIn += ", tx02 = '" & tx02 & "'"
            End If

            If tx03.Trim() <> "valueEmpty" Then
                vSqlIn += ", tx03 = '" & tx03 & "'"
            End If

            If nx01.Trim() <> "valueEmpty" Then
                vSqlIn += ", nx01 = '" & nx01 & "'"
            End If

            If nx02.Trim() <> "valueEmpty" Then
                vSqlIn += ", nx02 = '" & nx02 & "'"
            End If

            If nx03.Trim() <> "valueEmpty" Then
                vSqlIn += ", nx03 = '" & nx03 & "'"
            End If

            If sx01.Trim() <> "valueEmpty" Then
                vSqlIn += ", sx01 = '" & sx01 & "'"
            End If

            If sx02.Trim() <> "valueEmpty" Then
                vSqlIn += ", sx02 = '" & sx02 & "'"
            End If

            If sx03.Trim() <> "valueEmpty" Then
                vSqlIn += ", sx03 = '" & sx03 & "'"
            End If

            If dx01.Trim() <> "valueEmpty" Then
                vSqlIn += ", dx01 = " & dateDX(dx01) & " "
            End If

            If dx02.Trim() <> "valueEmpty" Then
                vSqlIn += ", dx02 = " & dateDX(dx02) & " "
            End If

            If dx03.Trim() <> "valueEmpty" Then
                vSqlIn += ", dx03 = " & dateDX(dx03) & " "
            End If

            If dx04.Trim() <> "valueEmpty" Then
                vSqlIn += ", dx04 = " & dateDX(dx04) & " "
            End If

            If ax01.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax01 = '" & ax01 & "'"
            End If

            If ax02.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax02 = '" & ax02 & "'"
            End If

            If ax03.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax03 = '" & ax03 & "'"
            End If

            If ax04.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax04 = '" & ax04 & "'"
            End If

            If ax05.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax05 = '" & ax05 & "'"
            End If

            If ax06.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax06 = '" & ax06 & "'"
            End If

            If ax07.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax07 = '" & ax07 & "'"
            End If

            If ax08.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax08 = '" & ax08 & "'"
            End If

            If ax09.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax09 = '" & ax09 & "'"
            End If

            If ax10.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax10 = '" & ax10 & "'"
            End If

            If ax11.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax11 = '" & ax11 & "'"
            End If

            If ax12.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax12 = '" & ax12 & "'"
            End If

            If ax13.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax13 = '" & ax13 & "'"
            End If

            If ax14.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax14 = '" & ax14 & "'"
            End If

            If ax15.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax15 = '" & ax15 & "'"
            End If

            If ax16.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax16 = '" & ax16 & "'"
            End If

            If ax17.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax17 = '" & ax17 & "'"
            End If

            If ax18.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax18 = '" & ax18 & "'"
            End If

            If ax19.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax19 = '" & ax19 & "'"
            End If

            If ax20.Trim() <> "valueEmpty" Then
                vSqlIn += ", ax20 = '" & ax20 & "'"
            End If

            If gx01.Trim() <> "valueEmpty" Then
                vSqlIn += ", gx01 = '" & gx01 & "'"
            End If

            If gx02.Trim() <> "valueEmpty" Then
                vSqlIn += ", gx02 = '" & gx02 & "'"
            End If

            If gx03.Trim() <> "valueEmpty" Then
                vSqlIn += ", gx03 = '" & gx03 & "'"
            End If

            If gx04.Trim() <> "valueEmpty" Then
                vSqlIn += ", gx04 = '" & gx04 & "'"
            End If

            If gx05.Trim() <> "valueEmpty" Then
                vSqlIn += ", gx05 = '" & gx05 & "'"
            End If

            vSqlIn += "where request_id = '" + request_id + "' "

            vExecute += DB105.ExecuteNonQuery(vSqlIn).ToString()

            If vExecute >= 1 Then
                Dim vAlert_Mss As String = "บันทึกข้อมูล เรียบร้อยแล้ว"
                Dim vUrl_Redirect As String = "update_backend.aspx"
                RedirectSubmit(vAlert_Mss, vUrl_Redirect)

            Else
                Dim page As Page = HttpContext.Current.Handler
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
            End If
        End If
    End Sub

    Public Function UpdateBackendLog(ByVal log_by As String, ByVal log_remark As String _
        , ByVal log_ref_id As String, ByVal request_id As String _
        , ByVal subject_id As String, ByVal request_title_id As String, ByVal request_title As String _
        , ByVal request_status As String, ByVal request_step As String, ByVal request_remark As String _
        , ByVal uemail_verify1 As String, ByVal uemail_verify2 As String, ByVal uemail_approve As String _
        , ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String _
        , ByVal create_date As String, ByVal create_by As String _
        , ByVal create_ro As String, ByVal create_shop As String, ByVal create_amount As String _
        , ByVal update_date As String, ByVal update_by As String _
        , ByVal last_update As String, ByVal last_depart As String, ByVal next_depart As String _
        , ByVal account_number As String, ByVal account_name As String _
        , ByVal account_number_to As String, ByVal account_name_to As String _
        , ByVal doc_number As String, ByVal bcs_number As String _
        , ByVal redebt_number As String, ByVal redebt_file As String _
        , ByVal redebt_update As String, ByVal redebt_update_by As String _
        , ByVal redebt_cause_id As String, ByVal amount As String _
        , ByVal area_ro As String, ByVal shop_code As String _
        , ByVal pick_refund As String, ByVal lock_receipt As String _
        , ByVal fx01 As String, ByVal fx02 As String, ByVal fx03 As String _
        , ByVal mx01 As String, ByVal mx02 As String, ByVal mx03 As String _
        , ByVal tx01 As String, ByVal tx02 As String, ByVal tx03 As String _
        , ByVal nx01 As String, ByVal nx02 As String, ByVal nx03 As String _
        , ByVal sx01 As String, ByVal sx02 As String, ByVal sx03 As String _
        , ByVal dx01 As String, ByVal dx02 As String, ByVal dx03 As String, ByVal dx04 As String _
        , ByVal ax01 As String, ByVal ax02 As String, ByVal ax03 As String, ByVal ax04 As String, ByVal ax05 As String _
        , ByVal ax06 As String, ByVal ax07 As String, ByVal ax08 As String, ByVal ax09 As String, ByVal ax10 As String _
        , ByVal ax11 As String, ByVal ax12 As String, ByVal ax13 As String, ByVal ax14 As String, ByVal ax15 As String _
        , ByVal ax16 As String, ByVal ax17 As String, ByVal ax18 As String, ByVal ax19 As String, ByVal ax20 As String _
        , ByVal gx01 As String, ByVal gx02 As String, ByVal gx03 As String, ByVal gx04 As String, ByVal gx05 As String _
    ) As Integer

        Dim vSqlIn As String 
        vSqlIn += "insert into log_update_backup (  "
        vSqlIn += "     bak_dtm, request_id, subject_id, request_title_id, request_title, request_status, request_step, request_remark "
        vSqlIn += "     , uemail_verify1, uemail_verify2, uemail_approve, uemail_cc1, uemail_cc2, uemail_ccv1 "
        vSqlIn += "     , create_date, create_by, create_ro, create_shop, create_amount, update_date, update_by, last_update, last_depart, next_depart "
        vSqlIn += "     , account_number, account_name, account_number_to, account_name_to, doc_number, bcs_number "
        vSqlIn += "     , redebt_number, redebt_file, redebt_update, redebt_update_by, redebt_cause_id "
        vSqlIn += "     , amount, area_ro, shop_code, pick_refund, lock_receipt "
        vSqlIn += "     , fx01, fx02, fx03, mx01, mx02, mx03, tx01, tx02, tx03, nx01, nx02, nx03, sx01, sx02, sx03, dx01, dx02, dx03, dx04 "
        vSqlIn += "     , ax01, ax02, ax03, ax04, ax05, ax06, ax07, ax08, ax09, ax10, ax11, ax12, ax13, ax14, ax15, ax16, ax17, ax18, ax19, ax20 "
        vSqlIn += "     , gx01, gx02, gx03, gx04, gx05 "
        vSqlIn += ") "
        vSqlIn += "select getdate(), request_id, subject_id, request_title_id, request_title, request_status, request_step, request_remark "
        vSqlIn += "     , uemail_verify1, uemail_verify2, uemail_approve, uemail_cc1, uemail_cc2, uemail_ccv1 "
        vSqlIn += "     , create_date, create_by, create_ro, create_shop, create_amount, update_date, update_by, last_update, last_depart, next_depart "
        vSqlIn += "     , account_number, account_name, account_number_to, account_name_to, doc_number, bcs_number "
        vSqlIn += "     , redebt_number, redebt_file, redebt_update, redebt_update_by, redebt_cause_id "
        vSqlIn += "     , amount, area_ro, shop_code, pick_refund, lock_receipt "
        vSqlIn += "     , fx01, fx02, fx03, mx01, mx02, mx03, tx01, tx02, tx03, nx01, nx02, nx03, sx01, sx02, sx03, dx01, dx02, dx03, dx04 "
        vSqlIn += "     , ax01, ax02, ax03, ax04, ax05, ax06, ax07, ax08, ax09, ax10, ax11, ax12, ax13, ax14, ax15, ax16, ax17, ax18, ax19, ax20 "
        vSqlIn += "     , gx01, gx02, gx03, gx04, gx05 "
        vSqlIn += "from request where request_id = '" & request_id & "' "

        vSqlIn += "insert into log_update_backend ( "
        vSqlIn += "     bak_no, log_dtm, log_by, log_remark, log_ref_id, request_id "
        vSqlIn += ") values ( "
        vSqlIn += "     @@IDENTITY, getdate(), '" & log_by & "', '" & log_remark & "', '" & log_ref_id & "', '" & request_id & "' "
        vSqlIn += ") "

        vSqlIn += "update log_update_backend set "
        vSqlIn += " subject_id = "          & CP.rSQLvalueEmpty(subject_id)
        vSqlIn += ", request_title_id = "   & CP.rSQLvalueEmpty(request_title_id)
        vSqlIn += ", request_title = "      & CP.rSQLvalueEmpty(request_title)
        vSqlIn += ", request_status = "     & CP.rSQLvalueEmpty(request_status)
        vSqlIn += ", request_step = "       & CP.rSQLvalueEmpty(request_step)
        vSqlIn += ", request_remark = "     & CP.rSQLvalueEmpty(request_remark)
        vSqlIn += ", uemail_verify1 = "     & CP.rSQLvalueEmpty(uemail_verify1)
        vSqlIn += ", uemail_verify2 = "     & CP.rSQLvalueEmpty(uemail_verify2)
        vSqlIn += ", uemail_approve = "     & CP.rSQLvalueEmpty(uemail_approve)
        vSqlIn += ", uemail_cc1 = "         & CP.rSQLvalueEmpty(uemail_cc1)
        vSqlIn += ", uemail_cc2 = "         & CP.rSQLvalueEmpty(uemail_cc2)
        vSqlIn += ", uemail_ccv1 = "        & CP.rSQLvalueEmpty(uemail_ccv1)
        vSqlIn += ", create_date = "        & CP.rSQLvalueEmpty(create_date)
        vSqlIn += ", create_by = "          & CP.rSQLvalueEmpty(create_by)
        vSqlIn += ", create_ro = "          & CP.rSQLvalueEmpty(create_ro)
        vSqlIn += ", create_shop = "        & CP.rSQLvalueEmpty(create_shop)
        vSqlIn += ", create_amount = "      & CP.rSQLvalueEmpty(create_amount)
        vSqlIn += ", update_date = "        & CP.rSQLvalueEmpty(update_date)
        vSqlIn += ", update_by = "          & CP.rSQLvalueEmpty(update_by)
        vSqlIn += ", last_update = "        & CP.rSQLvalueEmpty(last_update)
        vSqlIn += ", last_depart = "        & CP.rSQLvalueEmpty(last_depart)
        vSqlIn += ", next_depart = "        & CP.rSQLvalueEmpty(next_depart)
        vSqlIn += ", account_number = "     & CP.rSQLvalueEmpty(account_number)
        vSqlIn += ", account_name = "       & CP.rSQLvalueEmpty(account_name)
        vSqlIn += ", account_number_to = "  & CP.rSQLvalueEmpty(account_number_to)
        vSqlIn += ", account_name_to = "    & CP.rSQLvalueEmpty(account_name_to)
        vSqlIn += ", doc_number = "         & CP.rSQLvalueEmpty(doc_number)
        vSqlIn += ", bcs_number = "         & CP.rSQLvalueEmpty(bcs_number)
        vSqlIn += ", redebt_number = "      & CP.rSQLvalueEmpty(redebt_number)
        vSqlIn += ", redebt_file = "        & CP.rSQLvalueEmpty(redebt_file)
        vSqlIn += ", redebt_update = "      & CP.rSQLvalueEmpty(redebt_update)
        vSqlIn += ", redebt_update_by = "   & CP.rSQLvalueEmpty(redebt_update_by)
        vSqlIn += ", redebt_cause_id = "    & CP.rSQLvalueEmpty(redebt_cause_id)
        vSqlIn += ", amount = "             & CP.rSQLvalueEmpty(amount)
        vSqlIn += ", area_ro = "            & CP.rSQLvalueEmpty(area_ro)
        vSqlIn += ", shop_code = "          & CP.rSQLvalueEmpty(shop_code)
        vSqlIn += ", pick_refund = "        & CP.rSQLvalueEmpty(pick_refund)
        vSqlIn += ", lock_receipt = "       & CP.rSQLvalueEmpty(lock_receipt)
        vSqlIn += ", fx01 = "               & CP.rSQLvalueEmpty(fx01)
        vSqlIn += ", fx02 = "               & CP.rSQLvalueEmpty(fx02)
        vSqlIn += ", fx03 = "               & CP.rSQLvalueEmpty(fx03)
        vSqlIn += ", mx01 = "               & CP.rSQLvalueEmpty(mx01)
        vSqlIn += ", mx02 = "               & CP.rSQLvalueEmpty(mx02)
        vSqlIn += ", mx03 = "               & CP.rSQLvalueEmpty(mx03)
        vSqlIn += ", tx01 = "               & CP.rSQLvalueEmpty(tx01)
        vSqlIn += ", tx02 = "               & CP.rSQLvalueEmpty(tx02)
        vSqlIn += ", tx03 = "               & CP.rSQLvalueEmpty(tx03)
        vSqlIn += ", nx01 = "               & CP.rSQLvalueEmpty(nx01)
        vSqlIn += ", nx02 = "               & CP.rSQLvalueEmpty(nx02)
        vSqlIn += ", nx03 = "               & CP.rSQLvalueEmpty(nx03)
        vSqlIn += ", sx01 = "               & CP.rSQLvalueEmpty(sx01)
        vSqlIn += ", sx02 = "               & CP.rSQLvalueEmpty(sx02)
        vSqlIn += ", sx03 = "               & CP.rSQLvalueEmpty(sx03)
        vSqlIn += ", dx01 = "               & dateDX(dx01)
        vSqlIn += ", dx02 = "               & dateDX(dx02)
        vSqlIn += ", dx03 = "               & dateDX(dx03)
        vSqlIn += ", dx04 = "               & dateDX(dx04)
        vSqlIn += ", ax01 = "               & CP.rSQLvalueEmpty(ax01)
        vSqlIn += ", ax02 = "               & CP.rSQLvalueEmpty(ax02)
        vSqlIn += ", ax03 = "               & CP.rSQLvalueEmpty(ax03)
        vSqlIn += ", ax04 = "               & CP.rSQLvalueEmpty(ax04)
        vSqlIn += ", ax05 = "               & CP.rSQLvalueEmpty(ax05)
        vSqlIn += ", ax06 = "               & CP.rSQLvalueEmpty(ax06)
        vSqlIn += ", ax07 = "               & CP.rSQLvalueEmpty(ax07)
        vSqlIn += ", ax08 = "               & CP.rSQLvalueEmpty(ax08)
        vSqlIn += ", ax09 = "               & CP.rSQLvalueEmpty(ax09)
        vSqlIn += ", ax10 = "               & CP.rSQLvalueEmpty(ax10)
        vSqlIn += ", ax11 = "               & CP.rSQLvalueEmpty(ax11)
        vSqlIn += ", ax12 = "               & CP.rSQLvalueEmpty(ax12)
        vSqlIn += ", ax13 = "               & CP.rSQLvalueEmpty(ax13)
        vSqlIn += ", ax14 = "               & CP.rSQLvalueEmpty(ax14)
        vSqlIn += ", ax15 = "               & CP.rSQLvalueEmpty(ax15)
        vSqlIn += ", ax16 = "               & CP.rSQLvalueEmpty(ax16)
        vSqlIn += ", ax17 = "               & CP.rSQLvalueEmpty(ax17)
        vSqlIn += ", ax18 = "               & CP.rSQLvalueEmpty(ax18)
        vSqlIn += ", ax19 = "               & CP.rSQLvalueEmpty(ax19)
        vSqlIn += ", ax20 = "               & CP.rSQLvalueEmpty(ax20)
        vSqlIn += ", gx01 = "               & CP.rSQLvalueEmpty(gx01)
        vSqlIn += ", gx02 = "               & CP.rSQLvalueEmpty(gx02)
        vSqlIn += ", gx03 = "               & CP.rSQLvalueEmpty(gx03)
        vSqlIn += ", gx04 = "               & CP.rSQLvalueEmpty(gx04)
        vSqlIn += ", gx05 = "               & CP.rSQLvalueEmpty(gx05)
        vSqlIn += "where log_no = @@IDENTITY "

        ' CP.echo(vSqlIn)
        Return DB105.ExecuteNonQuery(vSqlIn).ToString()
    End Function
#End Region

#Region "UploadRequestFile"

    'original เริ่มแรกอัพได้ 3 ไฟล์
    Public Function rUploadRequestFile(ByVal vRequest_id As String) As String
        Dim request_file1 As String = CUL.rUploadFileDriveF("request_file1", vRequest_id + "_M01")
        Dim request_file2 As String = CUL.rUploadFileDriveF("request_file2", vRequest_id + "_M02")
        Dim request_file3 As String = CUL.rUploadFileDriveF("request_file3", vRequest_id + "_M03")

        Dim vSqlIn As String = "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + vRequest_id + "' "

        If request_file1 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file1 = '" + request_file1 + "' "
            vSqlIn += ", upload_date1 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file2 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file2 = '" + request_file2 + "'"
            vSqlIn += ", upload_date2 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file3 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file3 = '" + request_file3 + "'"
            vSqlIn += ", upload_date3 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        Return DB105.ExecuteNonQuery(vSqlIn).ToString()
    End Function

    Public Function rUploadRequest5File(ByVal vRequest_id As String) As String
        Dim request_file1 As String = CUL.rUploadFileDriveF("request_file1", vRequest_id + "_M01")
        Dim request_file2 As String = CUL.rUploadFileDriveF("request_file2", vRequest_id + "_M02")
        Dim request_file3 As String = CUL.rUploadFileDriveF("request_file3", vRequest_id + "_M03")
        Dim request_file4 As String = CUL.rUploadFileDriveF("request_file4", vRequest_id + "_M04")
        Dim request_file5 As String = CUL.rUploadFileDriveF("request_file5", vRequest_id + "_M05")

        Dim vSqlIn As String = "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + vRequest_id + "' "

        If request_file1 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file1 = '" + request_file1 + "' "
            vSqlIn += ", upload_date1 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file2 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file2 = '" + request_file2 + "'"
            vSqlIn += ", upload_date2 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file3 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file3 = '" + request_file3 + "'"
            vSqlIn += ", upload_date3 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file4 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file4 = '" + request_file4 + "'"
            vSqlIn += ", upload_date4 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file5 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file5 = '" + request_file5 + "'"
            vSqlIn += ", upload_date5 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        Return DB105.ExecuteNonQuery(vSqlIn).ToString()
    End Function

    Public Function rUploadRequest10File(ByVal vRequest_id As String) As String
        Dim request_file1 As String = CUL.rUploadFileDriveF("request_file1", vRequest_id + "_M01")
        Dim request_file2 As String = CUL.rUploadFileDriveF("request_file2", vRequest_id + "_M02")
        Dim request_file3 As String = CUL.rUploadFileDriveF("request_file3", vRequest_id + "_M03")
        Dim request_file4 As String = CUL.rUploadFileDriveF("request_file4", vRequest_id + "_M04")
        Dim request_file5 As String = CUL.rUploadFileDriveF("request_file5", vRequest_id + "_M05")
        Dim request_file6 As String = CUL.rUploadFileDriveF("request_file6", vRequest_id + "_M06")
        Dim request_file7 As String = CUL.rUploadFileDriveF("request_file7", vRequest_id + "_M07")
        Dim request_file8 As String = CUL.rUploadFileDriveF("request_file8", vRequest_id + "_M08")
        Dim request_file9 As String = CUL.rUploadFileDriveF("request_file9", vRequest_id + "_M09")
        Dim request_file10 As String = CUL.rUploadFileDriveF("request_file10", vRequest_id + "_M10")

        Dim vSqlIn As String = "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + vRequest_id + "' "

        If request_file1 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file1 = '" + request_file1 + "' "
            vSqlIn += ", upload_date1 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file2 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file2 = '" + request_file2 + "'"
            vSqlIn += ", upload_date2 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file3 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file3 = '" + request_file3 + "'"
            vSqlIn += ", upload_date3 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file4 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file4 = '" + request_file4 + "'"
            vSqlIn += ", upload_date4 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file5 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file5 = '" + request_file5 + "'"
            vSqlIn += ", upload_date5 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file6 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file6 = '" + request_file6 + "'"
            vSqlIn += ", upload_date6 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file7 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file7 = '" + request_file7 + "'"
            vSqlIn += ", upload_date7 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file8 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file8 = '" + request_file8 + "'"
            vSqlIn += ", upload_date8 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file9 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file9 = '" + request_file9 + "'"
            vSqlIn += ", upload_date9 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file10 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file10 = '" + request_file10 + "'"
            vSqlIn += ", upload_date10 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        Return DB105.ExecuteNonQuery(vSqlIn).ToString()
    End Function

    Public Function rUploadRequest15File(ByVal vRequest_id As String) As String
        Dim request_file1 As String = CUL.rUploadFileDriveF("request_file1", vRequest_id + "_M01")
        Dim request_file2 As String = CUL.rUploadFileDriveF("request_file2", vRequest_id + "_M02")
        Dim request_file3 As String = CUL.rUploadFileDriveF("request_file3", vRequest_id + "_M03")
        Dim request_file4 As String = CUL.rUploadFileDriveF("request_file4", vRequest_id + "_M04")
        Dim request_file5 As String = CUL.rUploadFileDriveF("request_file5", vRequest_id + "_M05")
        Dim request_file6 As String = CUL.rUploadFileDriveF("request_file6", vRequest_id + "_M06")
        Dim request_file7 As String = CUL.rUploadFileDriveF("request_file7", vRequest_id + "_M07")
        Dim request_file8 As String = CUL.rUploadFileDriveF("request_file8", vRequest_id + "_M08")
        Dim request_file9 As String = CUL.rUploadFileDriveF("request_file9", vRequest_id + "_M09")
        Dim request_file10 As String = CUL.rUploadFileDriveF("request_file10", vRequest_id + "_M10")
        Dim request_file11 As String = CUL.rUploadFileDriveF("request_file11", vRequest_id + "_M11")
        Dim request_file12 As String = CUL.rUploadFileDriveF("request_file12", vRequest_id + "_M12")
        Dim request_file13 As String = CUL.rUploadFileDriveF("request_file13", vRequest_id + "_M13")
        Dim request_file14 As String = CUL.rUploadFileDriveF("request_file14", vRequest_id + "_M14")
        Dim request_file15 As String = CUL.rUploadFileDriveF("request_file15", vRequest_id + "_M15")

        Dim vSqlIn As String = "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + vRequest_id + "' "

        If request_file1 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file1 = '" + request_file1 + "' "
            vSqlIn += ", upload_date1 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file2 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file2 = '" + request_file2 + "'"
            vSqlIn += ", upload_date2 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file3 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file3 = '" + request_file3 + "'"
            vSqlIn += ", upload_date3 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file4 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file4 = '" + request_file4 + "'"
            vSqlIn += ", upload_date4 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file5 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file5 = '" + request_file5 + "'"
            vSqlIn += ", upload_date5 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file6 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file6 = '" + request_file6 + "'"
            vSqlIn += ", upload_date6 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file7 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file7 = '" + request_file7 + "'"
            vSqlIn += ", upload_date7 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file8 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file8 = '" + request_file8 + "'"
            vSqlIn += ", upload_date8 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file9 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file9 = '" + request_file9 + "'"
            vSqlIn += ", upload_date9 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file10 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file10 = '" + request_file10 + "'"
            vSqlIn += ", upload_date10 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file11 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file11 = '" + request_file11 + "'"
            vSqlIn += ", upload_date11 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file12 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file12 = '" + request_file12 + "'"
            vSqlIn += ", upload_date12 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file13 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file13 = '" + request_file13 + "'"
            vSqlIn += ", upload_date13 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file14 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file14 = '" + request_file14 + "'"
            vSqlIn += ", upload_date14 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file15 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file15 = '" + request_file15 + "'"
            vSqlIn += ", upload_date15 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        Return DB105.ExecuteNonQuery(vSqlIn).ToString()
    End Function

    Public Function rUploadRequest20File(ByVal vRequest_id As String) As String
        Dim request_file1 As String = CUL.rUploadFileDriveF("request_file1", vRequest_id + "_M01")
        Dim request_file2 As String = CUL.rUploadFileDriveF("request_file2", vRequest_id + "_M02")
        Dim request_file3 As String = CUL.rUploadFileDriveF("request_file3", vRequest_id + "_M03")
        Dim request_file4 As String = CUL.rUploadFileDriveF("request_file4", vRequest_id + "_M04")
        Dim request_file5 As String = CUL.rUploadFileDriveF("request_file5", vRequest_id + "_M05")
        Dim request_file6 As String = CUL.rUploadFileDriveF("request_file6", vRequest_id + "_M06")
        Dim request_file7 As String = CUL.rUploadFileDriveF("request_file7", vRequest_id + "_M07")
        Dim request_file8 As String = CUL.rUploadFileDriveF("request_file8", vRequest_id + "_M08")
        Dim request_file9 As String = CUL.rUploadFileDriveF("request_file9", vRequest_id + "_M09")
        Dim request_file10 As String = CUL.rUploadFileDriveF("request_file10", vRequest_id + "_M10")
        Dim request_file11 As String = CUL.rUploadFileDriveF("request_file11", vRequest_id + "_M11")
        Dim request_file12 As String = CUL.rUploadFileDriveF("request_file12", vRequest_id + "_M12")
        Dim request_file13 As String = CUL.rUploadFileDriveF("request_file13", vRequest_id + "_M13")
        Dim request_file14 As String = CUL.rUploadFileDriveF("request_file14", vRequest_id + "_M14")
        Dim request_file15 As String = CUL.rUploadFileDriveF("request_file15", vRequest_id + "_M15")
        Dim request_file16 As String = CUL.rUploadFileDriveF("request_file16", vRequest_id + "_M16")
        Dim request_file17 As String = CUL.rUploadFileDriveF("request_file17", vRequest_id + "_M17")
        Dim request_file18 As String = CUL.rUploadFileDriveF("request_file18", vRequest_id + "_M18")
        Dim request_file19 As String = CUL.rUploadFileDriveF("request_file19", vRequest_id + "_M19")
        Dim request_file20 As String = CUL.rUploadFileDriveF("request_file20", vRequest_id + "_M20")

        Dim vSqlIn As String = "DECLARE @newid varchar(12) "
        vSqlIn += "SET @newid = '" + vRequest_id + "' "

        If request_file1 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file1 = '" + request_file1 + "' "
            vSqlIn += ", upload_date1 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file2 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file2 = '" + request_file2 + "'"
            vSqlIn += ", upload_date2 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file3 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file3 = '" + request_file3 + "'"
            vSqlIn += ", upload_date3 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file4 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file4 = '" + request_file4 + "'"
            vSqlIn += ", upload_date4 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file5 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file5 = '" + request_file5 + "'"
            vSqlIn += ", upload_date5 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file6 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file6 = '" + request_file6 + "'"
            vSqlIn += ", upload_date6 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file7 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file7 = '" + request_file7 + "'"
            vSqlIn += ", upload_date7 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file8 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file8 = '" + request_file8 + "'"
            vSqlIn += ", upload_date8 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file9 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file9 = '" + request_file9 + "'"
            vSqlIn += ", upload_date9 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file10 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file10 = '" + request_file10 + "'"
            vSqlIn += ", upload_date10 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file11 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file11 = '" + request_file11 + "'"
            vSqlIn += ", upload_date11 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file12 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file12 = '" + request_file12 + "'"
            vSqlIn += ", upload_date12 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file13 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file13 = '" + request_file13 + "'"
            vSqlIn += ", upload_date13 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file14 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file14 = '" + request_file14 + "'"
            vSqlIn += ", upload_date14 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file15 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file15 = '" + request_file15 + "'"
            vSqlIn += ", upload_date15 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file16 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file16 = '" + request_file16 + "'"
            vSqlIn += ", upload_date16 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file17 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file17 = '" + request_file17 + "'"
            vSqlIn += ", upload_date17 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file18 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file18 = '" + request_file18 + "'"
            vSqlIn += ", upload_date18 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file19 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file19 = '" + request_file19 + "'"
            vSqlIn += ", upload_date19 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        If request_file20 <> "" Then
            vSqlIn += "update request_file set "
            vSqlIn += "request_file20 = '" + request_file20 + "'"
            vSqlIn += ", upload_date20 = getdate() "
            vSqlIn += "where request_id = @newid "
        End If

        Return DB105.ExecuteNonQuery(vSqlIn).ToString()
    End Function
#End Region

#Region "FlowStep"

    Public Function SaveFlowAjax(ByVal update_by As String, ByVal flow_no As String, ByVal flow_sub As String _
        ,ByVal next_step As String, ByVal back_step As String, ByVal department As String _
        ,ByVal flow_status As String, ByVal flow_remark As String _
        ,ByVal xRequest_id As String _
    ) As Integer
        SaveFlow(update_by, flow_no, flow_sub _
            , next_step, back_step, department _
            , flow_status, flow_remark _
            , xRequest_id, "ajax" _
        )

        Return vRes_Ajax_SaveFlow
    End Function

    'รวม Process SaveFlow + SaveFlowAjax เดิม เข้าด้วยกัน
    'แล้วให้ SaveFlowAjax เรียก SaveFlow เอา เพื่อโค้ดจะได้ไม่ซ้ำซ้อน
    Public Sub SaveFlow(ByVal update_by As String, ByVal flow_no As String, ByVal flow_sub As String _
        , ByVal next_step As String, ByVal back_step As String, ByVal department As String _
        , ByVal flow_status As String, ByVal flow_remark As String _
        , Optional ByVal xRequest_id As String = "", Optional ByVal byAjax As String = "" _
    )
        Dim flow_file As String = ""

        'ถ้าเป็น Ajax ต้องมีค่า xRequest_id มาด้วยเสมอ
        If byAjax = "" Then
            xRequest_id = HttpContext.Current.Request.QueryString("request_id")
            flow_file = CUL.rUploadFileDriveF("flow_file", xRequest_id + "_F")
        End If

        vRes_Ajax_SaveFlow = 0 'response สำหรับ Ajax

        Dim vCase As String = ""
        Dim vAlert_Mss As String = ""
        Dim vSqlIn As String
        Dim vSql As String
        vSql = "select nexted, approval "
        vSql += "from request_status "
        vSql += "where status_id = " + flow_status

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Dim vTable_flow As String = "request_flow" + flow_sub
        Dim vNexted As Integer = vDT.Rows(0).Item("nexted")
        Dim vApproval As Integer = vDT.Rows(0).Item("approval")

        If vNexted = 1 Then '***** Next_Flow

            If IsNumeric(next_step) = True Then
                Dim vSql2 As String = "select flow_step, next_step "
                vSql2 += "from request_flow "
                vSql2 += "where request_id = '" + xRequest_id + "' and flow_step = " & (CInt(next_step)-1) & " "
                vSql2 += "and flow_complete = 0 and next_step <> '-' "
                vSql2 += "and disable = 0 "
                vSql2 += "union all "
                vSql2 += "select flow_step, next_step "
                vSql2 += "from request_flow_sub "
                vSql2 += "where request_id = '" + xRequest_id + "' and flow_step = " & (CInt(next_step)-1) & " "
                vSql2 += "and flow_complete = 0 and next_step <> '-' "
                vSql2 += "and disable = 0 "

                Dim vDT2 As New DataTable
                vDT2 = DB105.GetDataTable(vSql2)

                If vDT2.Rows().Count() > 1 Then
                    next_step = vDT2.Rows(0).Item("flow_step")
                End If
            End If

            vSqlIn = "update " + vTable_flow + " set "
            vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "
            vSqlIn += "flow_complete = 1, update_by = '" + update_by + "', update_date = getdate(), "
            vSqlIn += "begin_date = '" + rGetBeginDate(vTable_flow, flow_no) + "' "
            vSqlIn += "where no = '" & flow_no & "' "

            vSqlIn += "update request set "
            vSqlIn += "request_status = '" + flow_status + "', request_step = '" & next_step & "' "
            vSqlIn += ", last_update = getdate(), last_depart = '" & department & "' "
            vSqlIn += "where request_id = '" + xRequest_id + "' "

            vCase = "Next_Flow"
            vAlert_Mss = "ส่งอีเมล์แจ้ง ส่วนงานใน Flow Step ถัดไปแล้ว"

        Else If vNexted = 3 Then '***** Cancle_Flow
            If back_step = 0 Then '***** back_step = 0 ให้ ยกเลิกคำขอ อัตโนมัติ
                vSqlIn = "update " + vTable_flow + " set "
                vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "
                vSqlIn += "flow_complete = 1, update_by = '" + update_by + "', update_date = getdate(), "
                vSqlIn += "begin_date = '" + rGetBeginDate(vTable_flow, flow_no) + "' "
                vSqlIn += "where no = '" & flow_no & "' "

                vSqlIn += "update request set "
                vSqlIn += "request_status = '105' " '***** จบคำขอ ให้สถานะคำขอเป็น ยกเลิกคำขอ
                vSqlIn += ", last_update = getdate(), last_depart = '" & department & "' "
                vSqlIn += ", next_depart = '0' " '***** จบคำขอ ให้ลำดับถัดไปเป็น คนสร้างคำขอ
                vSqlIn += "where request_id = '" + xRequest_id + "' "

                vCase = "Cancle_Flow"
                vAlert_Mss = "ส่งอีเมล์แจ้ง ยกเลิกคำขอ ให้ผู้สร้างคำขอแล้ว"
            Else
                '***** ถอยไป flow step ตาม back_step
                Dim vDTF As New DataTable
                vDTF = rLoadRequestFlow(xRequest_id, back_step)
                
                vSqlIn = "update " + vTable_flow + " set "
                vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "
                vSqlIn += "flow_complete = 1, update_by = '" + update_by + "', update_date = getdate(), "
                vSqlIn += "begin_date = '" + rGetBeginDate(vTable_flow, flow_no) + "' "
                vSqlIn += "where no = '" & flow_no & "' "

                vSqlIn += rSqlInsertRequestFlowXbackReject(xRequest_id, next_step, update_by)

                vSqlIn += "DECLARE @newid varchar(12) "
                vSqlIn += "SET @newid = '" + xRequest_id + "' "
                vSqlIn += rSqlDisableRequestFlow(xRequest_id, vDTF.Rows(0).Item("flow_id"), back_step)
                vSqlIn += rSqlInsertRequestFlow(vDTF.Rows(0).Item("flow_id"), back_step)
                vSqlIn += rSqlSetDepartRequestFlow( _
                    vDTF.Rows(0).Item("uemail_verify1"), vDTF.Rows(0).Item("uemail_verify2"), vDTF.Rows(0).Item("uemail_approve"), _
                    vDTF.Rows(0).Item("create_by"), vDTF.Rows(0).Item("uemail_cc1"), vDTF.Rows(0).Item("uemail_cc2"), vDTF.Rows(0).Item("uemail_ccv1"), vDTF.Rows(0).Item("uemail_takecn") )

                '***** update request ให้ข้อมูลของเป็น back_step
                vSqlIn += "update request set "
                ' vSqlIn += "request_status = '" + flow_status + "', request_step = '" & back_step & "' "
                vSqlIn += "request_status = 0 " ' แก้เป็นเมื่อ back_step ให้สถานะคำขอเป็น เปิด1   
                vSqlIn += ", request_step = '" & back_step & "' " 
                vSqlIn += ", last_update = getdate(), last_depart = '" & department & "' "
                vSqlIn += ", next_depart = '" & vDTF.Rows(0).Item("depart_id") & "' "
                vSqlIn += "where request_id = '" + xRequest_id + "' "
                '***** update request ให้ข้อมูลของเป็น back_step

                vCase = "Back_Flow"
                vAlert_Mss = "ส่งอีเมล์แจ้งไม่อนุมัติ ให้ผู้สร้างคำขอทราบ และผู้ที่เกี่ยวข้องเพื่อตรวจสอบข้อมูลใหม่อีกครั้ง"
            End If

        Else If vNexted = 2 Then '***** Reply_1 ขอเพิ่มข้อมูล
            vSqlIn = "DECLARE @flow_sub_step int select @flow_sub_step = count(rfs.no) + 1 "
            vSqlIn += "from request_flow_sub rfs "
            vSqlIn += "right join ( "
            vSqlIn += "    select request_id, flow_step "
            vSqlIn += "    from " + vTable_flow + " "
            vSqlIn += "    where no = '" & flow_no & "' "
            vSqlIn += ") rf "
            vSqlIn += "on rf.request_id = rfs.request_id and rf.flow_step = rfs.flow_step "

            vSqlIn += "DECLARE @flow_sub_step2 int = @flow_sub_step + 1 "

            vSqlIn += "DECLARE @depart0_uemail varchar(255) "
            vSqlIn += "SET @depart0_uemail = (select top 1 uemail from request_flow where disable = 0 and depart_id = 0 and request_id = '" + xRequest_id + "') "

            vSqlIn += "DECLARE @reply_uemail varchar(255) "
            vSqlIn += "SET @reply_uemail = (select uemail from request_flow where disable = 0 and depart_id <> 2 and depart_id = '" & department & "' and request_id = '" + xRequest_id + "') "

            vSqlIn += "if @reply_uemail <> '' "
            vSqlIn += "begin "
            vSqlIn += "    SET @depart0_uemail = @depart0_uemail + ';' + @reply_uemail "
            vSqlIn += "end "

            vSqlIn += "update " + vTable_flow + " set "
            vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "
            vSqlIn += "flow_complete = 1, update_by = '" + update_by + "', update_date = getdate(), "
            vSqlIn += "begin_date = '" + rGetBeginDate(vTable_flow, flow_no) + "' "
            vSqlIn += "where no = '" & flow_no & "' "

            vSqlIn += "insert request_flow_sub ( "
            vSqlIn += "flow_sub_step, request_id, flow_id, "
            vSqlIn += "depart_id, flow_step, next_step, back_step, "
            vSqlIn += "send_uemail, uemail, approval, flow_status, add_next, flow_reply, begin_date) "
            vSqlIn += "select @flow_sub_step, request_id, flow_id, "
            vSqlIn += "9, flow_step, next_step, back_step, "
            vSqlIn += "@depart0_uemail, @depart0_uemail, approval, 110, 0, 1, getdate() "
            vSqlIn += "from " + vTable_flow + " "
            vSqlIn += "where no = '" & flow_no & "' "

            vSqlIn += "insert request_flow_sub ( "
            vSqlIn += "flow_sub_step, request_id, flow_id, "
            vSqlIn += "depart_id, flow_step, next_step, back_step, "
            vSqlIn += "send_uemail, uemail, approval, flow_status, add_next, flow_reply, begin_date) "
            vSqlIn += "select @flow_sub_step2, request_id, flow_id, "
            vSqlIn += "depart_id, flow_step, next_step, back_step, "
            vSqlIn += "send_uemail, uemail, approval, 0, 0, 1, getdate() "
            vSqlIn += "from " + vTable_flow + " "
            vSqlIn += "where no = '" & flow_no & "' "

            vSqlIn += "update request set "
            vSqlIn += "request_status = 110 "
            vSqlIn += ", last_update = getdate(), last_depart = '" & department & "' "
            vSqlIn += ", next_depart = '0' " '***** ขอเพิ่มข้อมูล ให้ลำดับถัดไปเป็น คนสร้างคำขอ
            vSqlIn += "where request_id = '" + xRequest_id + "' "

            vCase = "Reply_1"
            vAlert_Mss = "ส่งอีเมล์แจ้ง ผู้สร้างคำขอ ให้เพิ่มเติมข้อมูล"
        Else '***** ignore (รับเรื่อง, กำลังดำเนินการ) + รับทราบ
            vSqlIn = "update " + vTable_flow + " set "
            vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '" + flow_remark + "', flow_file = '" + flow_file + "', "
            If vApproval = 3 Then
                vSqlIn += "flow_complete = 1, "
            End If
            vSqlIn += "update_by = '" + update_by + "', update_date = getdate(), "
            vSqlIn += "begin_date = '" + rGetBeginDate(vTable_flow, flow_no) + "' "
            vSqlIn += "where no = '" & flow_no & "' "

            If vApproval <> 3 Then ' ถ้ารับทราบ (step - ขีด) เป็นผู้บันทึก ไม่ต้องอัพเดทสถานะล่าสุด
                vSqlIn += "update request set "
                vSqlIn += "request_status = '" + flow_status + "', "
                vSqlIn += "last_update = getdate(), last_depart = '" & department & "' "
                vSqlIn += "where request_id = '" + xRequest_id + "' "
            End If

            vCase = "ignore"
        End If

        ' insertLogDebugQry(vSqlIn)
        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            vRes_Ajax_SaveFlow = 1

            If vCase = "Next_Flow" Then
                'ถ้า step ถัดไปเป็น user เดียวกัน
                If rCheckNextFlowSameUser(xRequest_id, next_step, flow_status, update_by) = 1 Then
                    vCase = "Next_Flow"
                    vAlert_Mss = "ส่งอีเมล์แจ้ง ส่วนงานใน Flow Step ถัดไปแล้ว (อัพเดทสถานะ step ถัดไปของคุณอัตโนมัติ)"
                End If
                'ถ้า step ถัดไปเป็น user เดียวกัน

                autoNextFlowStepDash(xRequest_id, next_step, flow_status) 'ถ้า step ถัดไปเป็น (step - ขีด) ให้เปลี่ยนสถานะเป็น "รับทราบ" (requirement จาก requestShop|พีเกริก|พี่ยุ้ย)
            End If

            If rCheckEndFlow(xRequest_id) = 1 Then
                vCase = "End_Flow"
                vAlert_Mss = "ถึงลำดับสุดท้ายแล้ว ทำการปิดคำขออัตโนมัติ และส่งอีเมล์แจ้งผู้สร้างคำขอ"
            End If

            Dim vUrl_Redirect As String = "default.aspx"

            'ถ้าเป็น Ajax ไม่ต้อง redirect และไม่มี Alert Mss
            If byAjax = "ajax" Then
                vAlert_Mss = ""
                vUrl_Redirect = "ajax"
            End If

            sendMailAndRedirect(vCase, xRequest_id, vAlert_Mss, vUrl_Redirect)
        Else
            vRes_Ajax_SaveFlow = 0

            'ถ้าเป็น Ajax ไม่ต้อง Alert
            If byAjax = "" Then
                Dim page As Page = HttpContext.Current.Handler
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
            End If
        End If
    End Sub

    Public Sub update_Next_Flow(ByVal vRequest_id As String, ByVal next_depart As String)
        Dim vSqlIn As String =  ""

        Try
            vSqlIn = "update request set next_depart = '" & next_depart & "' "
            vSqlIn += "where request_id = '" & vRequest_id & "' "

            DB105.ExecuteNonQuery(vSqlIn)

        Catch ex As Exception
            insertLogError(vRequest_id, "update_Next_Flow", vSqlIn)
        End Try
    End Sub

    Public Sub AddNext(ByVal uemail As String, ByVal flow_no As String, ByVal flow_sub As String, ByVal depart_id As String)
        Dim xRequest_id = HttpContext.Current.Request.QueryString("request_id")

        Dim vTable_flow As String = "request_flow" + flow_sub
        Dim vSql As String = "select * "
        vSql += "from " + vTable_flow + " "
        vSql += "where no = '" & flow_no & "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Dim vSql2 As String = "SELECT "
        vSql2 += "  dm.depart_id "
        vSql2 += ", dm.depart_name "
        vSql2 += ", dm.add_next "
        vSql2 += ", dm.group_email "
        vSql2 += ", uemail = STUFF(( "
        vSql2 += "      SELECT ';' + du.uemail "
        vSql2 += "      FROM depart_user du "
        vSql2 += "      WHERE dm.depart_id = du.depart_id "
        vSql2 += "      and start_date <= getdate() "
        vSql2 += "      and (expired_date is null or expired_date >= getdate()) "
        vSql2 += "      FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql2 += "FROM department dm "
        vSql2 += "where depart_id = " & depart_id & " "

        Dim vDT2 As New DataTable
        vDT2 = DB105.GetDataTable(vSql2)

        Dim vSend_uemail As String = vDT2.Rows(0).Item("uemail")

        If Not IsDBNull(vDT2.Rows(0).Item("group_email"))  Then
            vSend_uemail = vDT2.Rows(0).Item("group_email")
        End If

        Dim vSqlIn As String = "DECLARE @flow_sub_step int "
        vSqlIn += "select @flow_sub_step = count(1) + 1 from request_flow_sub "
        vSqlIn += "where request_id = '" + xRequest_id + "' and flow_step = '" & vDT.Rows(0).Item("flow_step") & "' "
        vSqlIn += "and disable = 0 "
        vSqlIn += "select @flow_sub_step "

        vSqlIn += "insert request_flow_sub ( "
        vSqlIn += "flow_sub_step, request_id, flow_id, "
        vSqlIn += "depart_id, flow_step, next_step, back_step, "
        vSqlIn += "send_uemail, uemail, approval, flow_status, add_next "
        vSqlIn += ") values ("
        vSqlIn += "@flow_sub_step, '" & xRequest_id & "',  '" & vDT.Rows(0).Item("flow_id") & "' "
        vSqlIn += ", '" & depart_id & "',  '" & vDT.Rows(0).Item("flow_step") & "',  '" & vDT.Rows(0).Item("next_step") & "' ,  '" & vDT.Rows(0).Item("back_step") & "' "
        vSqlIn += ", '" & vSend_uemail & "', '" & vDT2.Rows(0).Item("uemail") & "',  2,  0, '" & vDT2.Rows(0).Item("add_next") & "') "

        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            Dim vAlert_Mss As String = "แทรกลำดับถัดไป และส่งอีเมล์ให้ส่วนงานที่ถูกแทรกแล้ว"
            sendMailAndRedirect("Add_Next", xRequest_id, vAlert_Mss)
        Else
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
        End If
    End Sub

    Public Function rCheckEndFlow(ByVal vRequest_id As String) As String
        Dim check_end As String = 0

        Dim vSql As String = "select request_status from request "
        vSql += "where request_id = '" + vRequest_id + "' "

        Dim vSqlIn As String = ""

        Try

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows(0).Item("request_status") <> 100 And vDT.Rows(0).Item("request_status") <> 105 Then

            vSql = "select no, flow_step, next_step from request_flow "
            vSql += "where request_id = '" + vRequest_id + "' and flow_complete = 0 and next_step <> '-' "
            vSql += "and disable = 0 "
            vSql += "union "
            vSql += "select no, flow_step, next_step from request_flow_sub "
            vSql += "where request_id = '" + vRequest_id + "' and flow_complete = 0 and next_step <> '-' "
            vSql += "and disable = 0 "
            vSql += "order by flow_step "

            vDT = DB105.GetDataTable(vSql)

            If vDT.Rows().Count() > 0 Then
                If vDT.Rows(0).Item("next_step") = "end" Then

                    For i As Integer = 0 To vDT.Rows().Count() - 1
                        vSqlIn += "update request_flow set "
                        vSqlIn += "flow_status = 100, flow_complete = 1, update_by = 'auto_end', update_date = getdate(), "
                        vSqlIn += "begin_date = '" & rGetBeginDate("request_flow", vDT.Rows(i).Item("no")) & "' "
                        vSqlIn += "where no = '" & vDT.Rows(i).Item("no") & "' "
                    Next

                    vSqlIn += rSqlUpdateRequestEnd(vRequest_id)
                    ' vSqlIn += "update request set "
                    ' vSqlIn += "request_status = 100 "
                    ' vSqlIn += ", last_update = getdate(), last_depart = 0 "
                    ' vSqlIn += ", next_depart = '0' " '***** จบคำขอ ให้ลำดับถัดไปเป็น คนสร้างคำขอ
                    ' vSqlIn += "where request_id = '" + vRequest_id + "' "

                    If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
                        check_end = 1
                        autoAfterEnd(vRequest_id)
                    Else
                        Dim page As Page = HttpContext.Current.Handler
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('failed! ติดต่อ support pos');", True)
                    End If

                End If
            End If

        End If

        Return check_end

        Catch ex As Exception
            ' Dim vTxtAlert As String = "error rCheckEndFlow Request_id:" & vRequest_id & " uemail:" & Session("Uemail")
            ' CP.rLineAlert("POS Monitor", vTxtAlert)

            insertLogError(vRequest_id, "rCheckEndFlow", vSqlIn)
        End Try
    End Function

    Public Sub autoAfterEnd(ByVal vRequest_id As String)
        Dim vSql As String = ""
        vSql += "select project_prefix, request_status from request "
        vSql += "join subject_dim on subject_dim.subject_id = request.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "where request_id = '" + vRequest_id + "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() > 0 Then
            If vDT.Rows(0).Item("project_prefix") = "B" Then
                ' CP.rRequestAddress("https://posbcs.triplet.co.th/3bbShop/updateStockShopJson.aspx?qrs=followSubmitNewStockShopNote&request_id=" + vRequest_id)
                ' Dim callback_api_3bbshop As String = "https://posbcs.triplet.co.th/3bbShop/"

                If vDT.Rows(0).Item("request_status") = "100" Then
                    CP.rRequestAddress(callback_api_3bbshop + "updateStockShopJson.aspx?qrs=followEndSubmit&request_id=" + vRequest_id)
                
                Else If vDT.Rows(0).Item("request_status") = "105" Then
                    CP.rRequestAddress(callback_api_3bbshop + "updateStockShopJson.aspx?qrs=followRejectSubmit&request_id=" + vRequest_id)
                End If
            End If
        End If
    End Sub

    Public Function rSqlUpdateRequestEnd(ByVal vRequest_id As String) As String
        Dim vSqlIn As String
        vSqlIn += "update request set "
        vSqlIn += "request_status = 100 "
        vSqlIn += ", last_update = getdate(), last_depart = 0 "
        vSqlIn += ", next_depart = '0' " '***** จบคำขอ ให้ลำดับถัดไปเป็น คนสร้างคำขอ
        vSqlIn += "where request_id = '" + vRequest_id + "' "

        Return vSqlIn
    End Function

    Public Function rSqlInsertRequestFlow(ByVal flow_id As String, Optional ByVal flow_step As String = "0") As String
        Dim vSqlIn As String
        vSqlIn = "INSERT INTO request_flow ( "
        vSqlIn += "request_id, flow_id, depart_id, flow_step, next_step, back_step, "
        vSqlIn += "send_uemail, uemail, approval, require_remark, require_file, add_next) "
        vSqlIn += "select @newid, fp.flow_id, fp.depart_id, fp.flow_step, fp.next_step, fp.back_step, "
        vSqlIn += "isnull(dp.group_email, dp.uemail) send_uemail, dp.uemail, fp.approval, fp.require_remark, fp.require_file, dp.add_next "
        vSqlIn += "from flow_pattern fp "
        vSqlIn += "join ( "
        vSqlIn += " SELECT "
        vSqlIn += "      dm.depart_id "
        vSqlIn += "    , dm.depart_name "
        vSqlIn += "    , dm.add_next "
        vSqlIn += "    , dm.group_email "
        vSqlIn += "    , uemail = STUFF(( "
        vSqlIn += "          SELECT ';' + du.uemail "
        vSqlIn += "          FROM depart_user du "
        vSqlIn += "          WHERE dm.depart_id = du.depart_id "
        vSqlIn += "          and start_date <= getdate() "
        vSqlIn += "          and (expired_date is null or expired_date >= getdate()) "
        vSqlIn += "          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSqlIn += "     FROM department dm "
        vSqlIn += ") dp on dp.depart_id = fp.depart_id "
        vSqlIn += "where flow_id = '" + flow_id + "' "
        vSqlIn += "and fp.flow_step >= '" + flow_step + "' "
        vSqlIn += "order by fp.flow_step "

        Return vSqlIn
    End Function

    Public Function rSqlInsertRequestFlowXbackReject(ByVal xRequest_id As String, ByVal next_step As String, ByVal reject_by As String) As String
        Dim vSqlIn As String
        vSqlIn = "INSERT INTO request_flow_xback_reject ( "
        vSqlIn += "flow_no, request_id, flow_id, depart_id, flow_step, flow_sub_step, next_step, back_step, "
        vSqlIn += "send_uemail, uemail, flow_remark, flow_file, flow_status, begin_date, update_date, update_by, "
        vSqlIn += "reject_date, reject_by, reject_no) "

        vSqlIn += "select no, '" & xRequest_id & "' request_id, flow_id, depart_id, flow_step, flow_sub_step, next_step, back_step, "
        vSqlIn += "send_uemail, uemail, flow_remark, flow_file, flow_status, begin_date, update_date, update_by, "
        vSqlIn += "getdate() reject_date, '" & reject_by & "' reject_by, dbo.runRejectNO('" & xRequest_id & "') reject_no "
        vSqlIn += "from ( "
        vSqlIn += rSqlRequestFlow(xRequest_id)
        vSqlIn += ") rfx "
        vSqlIn += "where flow_step < '" + next_step + "' "
        vSqlIn += "order by flow_step, next_step, flow_sub_step  "

        Return vSqlIn
    End Function

    Public Function rSqlSetDepartRequestFlow(ByVal uemail_verify1 As String, ByVal uemail_verify2 As String, ByVal uemail_approve As String _
        , ByVal create_by As String, ByVal uemail_cc1 As String, ByVal uemail_cc2 As String, ByVal uemail_ccv1 As String _
        , Optional ByVal uemail_takecn As String = "" _
    ) As String
        Dim vSqlIn As String = ""

        '------------------- ให้ uemail_verify1 เป็น uemail_ccv1 เสมอ -------------------
        vSqlIn += "update request set uemail_ccv1 = '" + uemail_verify1 + "' "
        vSqlIn += "where request_id = @newid "

        uemail_ccv1 = uemail_verify1
        '------------------- ให้ uemail_verify1 เป็น uemail_ccv1 เสมอ -------------------

        vSqlIn += "update request_flow set send_uemail = '" + uemail_verify1 + "', uemail = '" + rCheckGroupMail(uemail_verify1) + "' "
        vSqlIn += "where depart_id = 2 and request_id = @newid "

        vSqlIn += "update request_flow set send_uemail = '" + uemail_verify2 + "', uemail = '" + rCheckGroupMail(uemail_verify2) + "' "
        vSqlIn += "where depart_id = 3 and request_id = @newid "

        vSqlIn += "update request_flow set send_uemail = '" + uemail_approve + "', uemail = '" + rCheckGroupMail(uemail_approve) + "' "
        vSqlIn += "where depart_id = 1 and request_id = @newid "

        vSqlIn += "update request_flow set send_uemail = '" + uemail_takecn + "', uemail = '" + rCheckGroupMail(uemail_takecn) + "' "
        vSqlIn += "where depart_id = 8 and request_id = @newid "

        If uemail_cc1 <> "" Then
            create_by += ";" + uemail_cc1
        End If

        If uemail_cc2 <> "" Then
            create_by += ";" + uemail_cc2
        End If

        If uemail_ccv1 <> "" Then
            create_by += ";" + uemail_ccv1
        End If
        
        vSqlIn += "update request_flow set send_uemail = '" + create_by + "', uemail = '" + create_by + "' "
        vSqlIn += "where depart_id = 0 and request_id = @newid "

        Return vSqlIn
    End Function

    Public Function rSqlDisableRequestFlow(ByVal vRequest_id As String, ByVal flow_id As String, ByVal flow_step As String) As String
        Dim vSqlIn As String
        vSqlIn = "update request_flow set disable = 1 "
        vSqlIn += "where request_id = '" + vRequest_id + "' "
        vSqlIn += "and flow_id = '" + flow_id + "' "
        vSqlIn += "and flow_step >= '" + flow_step + "' "

        vSqlIn += "update request_flow_sub set disable = 1 "
        vSqlIn += "where request_id = '" + vRequest_id + "' "
        vSqlIn += "and flow_id = '" + flow_id + "' "
        vSqlIn += "and flow_step >= '" + flow_step + "' "

        Return vSqlIn
    End Function

    Public Function rLoadRequestFlow(ByVal vRequest_id As String, ByVal flow_step As String) As DataTable
        Dim vSql As String 
        vSql = "select flow_id, depart_id, uemail_verify1, uemail_verify2, uemail_approve, uemail_takecn, create_by, uemail_cc1, uemail_cc2, uemail_ccv1 "
        vSql += "from request "
        vSql += "join ( "
        vSql += "    select top 1 request_id, flow_id, depart_id from request_flow "
        vSql += "    where request_id = '" + vRequest_id + "' "
        vSql += "    and flow_step = '" + flow_step + "' "
        vSql += "    and disable = 0 "
        vSql += "    order by flow_step, next_step "
        vSql += ") rf on rf.request_id = request.request_id "
        vSql += "where request.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function rCheckGroupMail(ByVal vUemail As String) As String
        Dim vSql As String 
        vSql = "select dm.depart_id, dm.depart_name, dm.add_next "
        vSql += ", uemail = STUFF(( "
        vSql += "    SELECT ';' + du.uemail "
        vSql += "    FROM depart_user du "
        vSql += "    WHERE dm.depart_id = du.depart_id "
        vSql += "    and start_date <= getdate() "
        vSql += "    and (expired_date is null or expired_date >= getdate()) "
        vSql += "    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')  "
        vSql += "from department dm "
        vSql += "where dm.disable = 0 and dm.group_email = '" + vUemail + "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() > 0 Then
            Return vDT.Rows(0).Item("uemail")
        Else
            Return vUemail
        End If
    End Function

    Public Function rGetBeginDate(ByVal vTable_flow As String, ByVal vFlow_no As String) As String
        Dim vSql As String 

        Try

        If vTable_flow = "request_flow" Then
            vSql = "select no, request_id, flow_step, 0 flow_sub_step from request_flow where no = " & vFlow_no

        Else If vTable_flow = "request_flow_sub"
            vSql = "select no, request_id, flow_step, flow_sub_step from request_flow_sub where no = " & vFlow_no

        Else
            Return ""
        End If

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vTable_flow = "request_flow" And vDT.Rows(0).Item("flow_step") = 1 Then
            vSql = "select create_date begin_date from request where request_id = '" & vDT.Rows(0).Item("request_id") & "' "

        Else If vTable_flow = "request_flow" And vDT.Rows(0).Item("flow_step") > 1 Then
            vSql = "select top 1 update_date begin_date from ( "
            vSql += "    select no, request_id, flow_step, 0 flow_sub_step, update_date "
            vSql += "    from request_flow "
            vSql += "    where disable = 0 and flow_complete = 1 "
            vSql += "    union all "
            vSql += "    select no, request_id, flow_step, flow_sub_step, update_date "
            vSql += "    from request_flow_sub "
            vSql += "    where disable = 0 and flow_complete = 1 "
            vSql += ") all_flow "
            vSql += "where request_id = '" & vDT.Rows(0).Item("request_id") & "' "
            vSql += "and flow_step < " & vDT.Rows(0).Item("flow_step")
            vSql += "order by flow_step desc, flow_sub_step desc "

        Else If vTable_flow = "request_flow_sub"
            vSql = "select top 1 update_date begin_date from ( "
            vSql += "    select no, request_id, flow_step, 0 flow_sub_step, update_date "
            vSql += "    from request_flow "
            vSql += "    where disable = 0 and flow_complete = 1 "
            vSql += "    union all "
            vSql += "    select no, request_id, flow_step, flow_sub_step, update_date "
            vSql += "    from request_flow_sub "
            vSql += "    where disable = 0 and flow_complete = 1 "
            vSql += ") all_flow "
            vSql += "where request_id = '" & vDT.Rows(0).Item("request_id") & "' "
            vSql += "and flow_step = " & vDT.Rows(0).Item("flow_step") & " "
            vSql += "and flow_sub_step < " & vDT.Rows(0).Item("flow_sub_step") & " "
            vSql += "order by flow_step desc, flow_sub_step desc "

        Else
            Return ""
        End If

        vDT = DB105.GetDataTable(vSql)

        Dim begin_date As DateTime = Convert.ToDateTime(vDT.Rows(0).Item("begin_date"))
        Return begin_date.ToString("yyyy-MM-dd HH:mm:ss.fff")

        Catch ex As Exception
            insertLogError(HttpContext.Current.Request.QueryString("request_id"), "rGetBeginDate", vSql)
        End Try
    End Function

    Public Function rCheckNextFlowSameUser(ByVal vRequest_id As String, ByVal next_step As String, ByVal flow_status As String _
        , ByVal update_by As String, Optional ByVal check_next_same As Integer = 0 _
    ) As Integer

        Dim vSql, vSqlIn As String 
        vSql = "select no, depart_id, next_step "
        vSql += "from request_flow "
        vSql += "where request_id = '" + vRequest_id + "' and flow_step = " & next_step & " "
        vSql += "and flow_complete = 0 and next_step <> '-' "
        vSql += "and disable = 0 "
        vSql += "and depart_id in (1, 2, 3) "
        vSql += "and uemail = '" + update_by + "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() > 0 Then
            vSqlIn = "update request_flow set "
            vSqlIn += "flow_status = '" + flow_status + "', flow_remark = '', flow_file = '', "
            vSqlIn += "flow_complete = 1, update_by = '" + update_by + "', update_date = getdate(), "
            vSqlIn += "begin_date = '" + rGetBeginDate("request_flow", vDT.Rows(0).Item("no")) + "' "
            vSqlIn += "where no = '" & vDT.Rows(0).Item("no") & "' "

            vSqlIn += "update request set "
            vSqlIn += "request_status = '" + flow_status + "', request_step = '" & vDT.Rows(0).Item("next_step") & "' "
            vSqlIn += ", last_update = getdate(), last_depart = '" & vDT.Rows(0).Item("depart_id") & "' "
            vSqlIn += "where request_id = '" + vRequest_id + "' "

            If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
                check_next_same = 1

                rCheckNextFlowSameUser(vRequest_id, vDT.Rows(0).Item("next_step"), flow_status, update_by, check_next_same)
            End If
        End If

        Return check_next_same
    End Function

    Public Sub autoNextFlowStepDash(ByVal vRequest_id As String, ByVal next_step As String, ByVal flow_status As String)
        'ถ้า step ถัดไปเป็น (step - ขีด) ให้เปลี่ยนสถานะเป็น "รับทราบ" (requirement จาก requestShop|พีเกริก|พี่ยุ้ย)

        Dim vSql As String 
        vSql = "select no, depart_id, next_step "
        vSql += "from request_flow "
        vSql += "where request_id = '" + vRequest_id + "' and flow_step = " & next_step & " "
        vSql += "and next_step = '-' "
        vSql += "and flow_complete = 0 "
        vSql += "and disable = 0 "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Dim vSqlIn As String = ""

        For i As Integer = 0 To vDT.Rows().Count() - 1
            vSqlIn += "update request_flow set flow_status = '1' "
            vSqlIn += "where no = '" & vDT.Rows(i).Item("no") & "' "
        Next

        If vSqlIn.Length() > 0 Then
            DB105.ExecuteNonQuery(vSqlIn).ToString()
        End If
    End Sub

    Public Function rGetRequestStatus(ByVal vRequest_id As String) As String
        Dim vSql As String = "select request_id, request_status from request "
        vSql += "where request_id = '" + vRequest_id + "'"

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Return vDT.Rows(0).Item("request_status")
    End Function

    Public Sub insertLogError(ByVal vRequest_id As String, ByVal catch_in As String, ByVal log_remark As String)
        Dim vSqlIn As String = "insert into log_error (catch_in, request_id, uemail, log_remark) values ("
        vSqlIn &= "'" & catch_in & "',"
        vSqlIn &= "'" & vRequest_id & "', " 
        vSqlIn &= "'" & Session("Uemail") & "', "
        vSqlIn &= "'" & CP.rReplaceQuote(log_remark) & "') " 

        DB105.ExecuteNonQuery(vSqlIn)
    End Sub

    Public Sub insertLogAutoRepair(ByVal vRequest_id As String, ByVal repair_in As String)
        Dim vSqlIn As String = "insert into log_auto_repair (repair_in, request_id, uemail, url) values ("
        vSqlIn &= "'" & repair_in & "',"
        vSqlIn &= "'" & vRequest_id & "', " 
        vSqlIn &= "'" & Session("Uemail") & "', "
        vSqlIn &= "'" & CP.rReplaceQuote(HttpContext.Current.Request.Url.ToString()) & "') " 

        DB105.ExecuteNonQuery(vSqlIn)
    End Sub

    Public Sub insertLogDebugQry(ByVal qry As String)
        Dim vSqlIn As String = "insert into log_debug_qry (qry, uemail_session) values ("
        vSqlIn &= "'" & CP.rReplaceQuote(qry) & "',"
        vSqlIn &= "'" & Session("Uemail") & "') " 

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "qry = [" + qry + "] [" + CP.rReplaceQuote(qry) + "]end", True)
        DB105.ExecuteNonQuery(vSqlIn)
    End Sub
#End Region


#Region "SendMail & Redirect"
    'Private 
    Public Sub sendMailAndRedirect(ByVal vCase As String, ByVal vRequest_id As String, ByVal vAlert_Mss As String, Optional ByVal vUrl_Redirect As String = "")
        If vCase = "ignore" Then
            CP.InteruptRefresh()
        End If

        Dim vUrl As String = CP.rGetCurrentUrl()'HttpContext.Current.Request.Url.AbsoluteUri()

        SendMailSubmitAndUpdateNextFlow(vCase, vRequest_id)

        If vUrl_Redirect <> "ajax" Then
            RedirectSubmit(vAlert_Mss, vUrl_Redirect)
        End If
    End Sub

    Public Sub RedirectSubmit(Optional ByVal vAlert_Mss As String = "", Optional ByVal vUrl_Redirect As String = "")
        Dim page As Page = HttpContext.Current.Handler

        If vUrl_Redirect.Trim() = "" Then
            vUrl_Redirect = CP.rGetCurrentUrl()'HttpContext.Current.Request.Url.AbsoluteUri()
        End If

        If vAlert_Mss <> "" Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('" + vAlert_Mss + "'); window.location = '" + vUrl_Redirect + "';", True)
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "window.location = '" + vUrl_Redirect + "';", True)
        End If
    End Sub

    Public Sub SendMailSubmitAndUpdateNextFlow(ByVal vCase As String, ByVal vRequest_id As String)
        Dim vDT As New DataTable
        Dim vMain_Point As String = "" '"รบกวนตรวจสอบคำขอเพื่อดำเนินการ"
        Dim vRequest_title As String = "" '"ลดหนี้กรณีชำระผิด Acc. โดยยอด APO และตัดบิลไปแล้ว  และมีการเพิ่มหนี้จากคุณ amornrat.ka เรียบร้อยแล้ว"
        Dim vSubject_name As String = "" '"ลดหนี้ชำระผิด Account"
        Dim vSubject_url As String = "" '"redebtpage"
        Dim vProduct_name As String = "" '"ลดหนี้"
        Dim vSend_uemail As String = "" '"panupong.pa;test.t;test.t;"
        Dim vSend_uemail_cc As String = "" '"panupong.pa"

        Dim vAccount_number As String = ""
        Dim vAccount_name As String = ""
        Dim vAmount As String = ""

        Select Case vCase
            Case "Open_Flow"
                vMain_Point = "มีคำขอใหม่ รบกวนตรวจสอบคำขอเพื่อดำเนินการ"
                vDT = loadDT_Open_Flow(vRequest_id)
                update_Next_Flow(vRequest_id, vDT.Rows(0).Item("next_depart_id"))

                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vSend_uemail = vDT.Rows(0).Item("send_uemail")
                vSend_uemail_cc = load_Request_Owner(vRequest_id, vSend_uemail_cc)
                vAccount_number = vDT.Rows(0).Item("account_number")
                vAccount_name = vDT.Rows(0).Item("account_name")
                vAmount = vDT.Rows(0).Item("amount")

            Case "Cancle_Flow"
                autoAfterEnd(vRequest_id)

                vMain_Point = "แจ้งคำขอถูกยกเลิก"
                vDT = loadDT_Cancle_Flow(vRequest_id)

                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vSend_uemail = vDT.Rows(0).Item("send_uemail")
                vAccount_number = vDT.Rows(0).Item("account_number")
                vAccount_name = vDT.Rows(0).Item("account_name")
                vAmount = vDT.Rows(0).Item("amount")

            Case "Back_Flow"
                vMain_Point = "คำขอไม่อนุมัติ/ไม่ดำเนินการ กรูณาตรวจสอบข้อมูลีกครั้ง"
                vDT = loadDT_Back_Flow(vRequest_id)

                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vSend_uemail = vDT.Rows(0).Item("send_uemail")
                vAccount_number = vDT.Rows(0).Item("account_number")
                vAccount_name = vDT.Rows(0).Item("account_name")
                vAmount = vDT.Rows(0).Item("amount")

            Case "Next_Flow"
                vMain_Point = "ถึงลำดับของท่านแล้ว รบกวนตรวจสอบคำขอเพื่อดำเนินการ"
                vDT = loadDT_Next_Flow(vRequest_id)
                update_Next_Flow(vRequest_id, vDT.Rows(0).Item("next_depart_id"))

                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vSend_uemail = vDT.Rows(0).Item("send_uemail")
                vAccount_number = vDT.Rows(0).Item("account_number")
                vAccount_name = vDT.Rows(0).Item("account_name")
                vAmount = vDT.Rows(0).Item("amount")

            Case "Add_Next"
                vMain_Point = "ท่านได้ถูกแทรกเข้าเป็นผู้ดำเนินการคำขอนี้"
                vDT = loadDT_Add_Next(vRequest_id)

                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vSend_uemail = vDT.Rows(0).Item("send_uemail")
                vAccount_number = vDT.Rows(0).Item("account_number")
                vAccount_name = vDT.Rows(0).Item("account_name")
                vAmount = vDT.Rows(0).Item("amount")

            Case "Reply_1"
                vMain_Point = "คำขอต้องการข้อมูลเพิ่มเติม กรุณาให้ข้อมูลภายใน7วัน ไม่เช่นนั้นคำขอจะถูกยกเลิก"
                vDT = loadDT_Reply_1(vRequest_id)

                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vSend_uemail = vDT.Rows(0).Item("send_uemail")
                vAccount_number = vDT.Rows(0).Item("account_number")
                vAccount_name = vDT.Rows(0).Item("account_name")
                vAmount = vDT.Rows(0).Item("amount")

            Case "Reply_2"
                vMain_Point = "คำขอมีการบันทึกแก้ไขข้อมูลแล้ว ตรวจสอบคำขอเพื่อดำเนินการต่อไป"
                vDT = loadDT_Reply_2(vRequest_id)

                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vSend_uemail = vDT.Rows(0).Item("send_uemail")
                vAccount_number = vDT.Rows(0).Item("account_number")
                vAccount_name = vDT.Rows(0).Item("account_name")
                vAmount = vDT.Rows(0).Item("amount")

                vDT = loadDT_Next_Flow(vRequest_id)
                update_Next_Flow(vRequest_id, vDT.Rows(0).Item("next_depart_id"))

            Case "End_Flow"
                vMain_Point = "ปิดคำขอเรียบร้อยแล้ว (จบกระบวนการ)"
                vDT = loadDT_End_Flow(vRequest_id)

                vRequest_title = vDT.Rows(0).Item("request_title")
                vSubject_name = vDT.Rows(0).Item("subject_name")
                vSubject_url = vDT.Rows(0).Item("subject_url")
                vProduct_name = vDT.Rows(0).Item("project_name")
                vSend_uemail = vDT.Rows(0).Item("send_uemail")
                vAccount_number = vDT.Rows(0).Item("account_number")
                vAccount_name = vDT.Rows(0).Item("account_name")
                vAmount = vDT.Rows(0).Item("amount")

        End Select

        Try

        Dim vUrl As String = CP.rGetCurrentUrl()'HttpContext.Current.Request.Url.AbsoluteUri()
        ' If vUrl.ToLower().Contains("followrequest") Then

            Dim vSplit_uemail As String() = Regex.Split(vSend_uemail, ";")
            Dim vSplit_uemail_cc As String() = Regex.Split(vSend_uemail_cc, ";")

            Dim mail As New MailMessage()
            mail.From = New MailAddress(append_mail + "FollowRequest@jasmine.com")

            For Each sMail As String In vSplit_uemail
                If sMail.Trim() <> "" Then
                    mail.To.Add(append_mail + sMail + "@jasmine.com")
                End If
            Next

            For Each sMail_cc As String In vSplit_uemail_cc
                If sMail_cc.Trim() <> "" Then
                    mail.CC.Add(append_mail + sMail_cc + "@jasmine.com")
                End If
            Next

           ' mail.CC.Add("panupong.pa@jasmine.com")

            mail.Subject = vRequest_id + ": " + vMain_Point

            mail.Body = rMailBody(vRequest_id, vRequest_title, vSubject_name, vSubject_url, vProduct_name, vMain_Point, vAccount_number, vAccount_name, vAmount)

            mail.IsBodyHtml = True

            Dim smtp As New SmtpClient("smtp.jasmine.com")
            smtp.Credentials = New NetworkCredential("chancharas.w", "311227")

            smtp.Send(mail)
        ' End If

        Catch ex As Exception
            'CP.InteruptRefresh()
        End Try
    End Sub

    Public Function rMailBody(ByVal vRequest_id As String, ByVal vRequest_title As String _
        , ByVal vSubject_name As String, ByVal vSubject_url As String, ByVal vProduct_name As String, ByVal vMain_Point As String _
        , Optional ByVal vAccount_number As String = "", Optional ByVal vAccount_name As String = "", Optional ByVal vAmount As String = "" _
        , Optional ByVal vFreeText As String = "") As String
        Dim herf_url As String = "https://posbcs.triplet.co.th/FollowRequest/update_" + vSubject_url + ".aspx?request_id=" + vRequest_id
        Dim main_desc As String = ""

        If vFreeText.Trim() <> "" Then
            main_desc += vFreeText + "<br>"
        End If

        main_desc += "<p><span style='font-family:tahoma;font-size: 13px;'><b>ระบบ:</b> " + vProduct_name + "</span></p>"
        main_desc += "<p><span style='font-family:tahoma;font-size: 13px;'><b>หัวข้อ:</b> " + vSubject_name + "</span></p>"
        main_desc += "<p><span style='font-family:tahoma;font-size: 13px;'><b>เรื่องที่แจ้ง:</b> " + vRequest_title + "</span></p>"
        main_desc += "<p><span style='font-family:tahoma;font-size: 13px;'><b>เลขที่คำขอ:</b> </span><span style='font-family:tahoma;'>" + vRequest_id + "</span></p>"

        If vAccount_number.Trim() <> "" Then
            main_desc += "<p><span style='font-family:tahoma;font-size: 13px;'><b>Account:</b> " + vAccount_number + "</span></p>"
        End If

        If vAccount_name.Trim() <> "" Then
            main_desc += "<p><span style='font-family:tahoma;font-size: 13px;'><b>ชื่อลูกค้า:</b> " + vAccount_name + "</span></p>"
        End If

        If vAmount.Trim() <> "" Then
            main_desc += "<p><span style='font-family:tahoma;font-size: 13px;'><b>จำนวนที่ต้องการลดหนี้:</b> " + vAmount + " บาท</span></p>"
        End If

        return _
        "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' 'http://www.w3.org/TR/html4/loose.dtd'>" + _
        "<html lang='th'>" + _
        "<head>" + _
        "  <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>" + _
        "  <meta name='viewport' content='width=device-width, initial-scale=1'>" + _
        "  <meta http-equiv='X-UA-Compatible' content='IE=edge'>" + _
        "  <meta name='format-detection' content='telephone=no'>" + _
        "</head>" + _
        "<body style='margin:0; padding:0;' leftmargin='0' topmargin='0' marginwidth='0' marginheight='0'>" + _
        "  <table border='0' width='100%' height='100%' cellpadding='0' cellspacing='0'>" + _
        "    <tr>" + _
        "      <td align='center' valign='top'>" + _
        "        <br>" + _
        "        <table border='0' width='700' cellpadding='0' cellspacing='0' class='container' style='width:700px;max-width:700px;margin-top:-20px;'>" + _
        "          <tr>" + _
        "            <td class='container-padding header' align='left' style='font-family:sans-serif;font-size:24px;font-weight:bold;color:#0c86e6;padding-left:12px;padding-right:12px'>" + _
        "             Follow Request" + _
        "           </td>" + _
        "         </tr>" + _
        "         <tr>" + _
        "          <td class='container-padding content' align='left' style='padding:12px;background-color:#ffffff'>" + _
        "            <div class='title' style='font-family:tahoma;font-size:16px;color:#ff5501;font-weight:bold;'>" + vMain_Point + "</div>" + _
        "            <div class='body-text' style='font-family:tahoma;font-size:16px;line-height:12px;text-align:left;color:#333333'>" + _
        main_desc + _
        "              <br/>" + _
        "              <p><center><span style='font-family:tahoma;font-size: 13px;font-weight: bold;'>โปรดตรวจสอบและดำเนินการ</span></center></p>" + _
        "              <br/>" + _
        "              <center><a style='font-family:tahoma;font-size:35px;font-weight: bold;' href='" + herf_url + "'>Click link for more details</a></center>" + _
        "              <br/><center><span style='font-family:tahoma;font-size:13px;font-weight: bold;'>โปรดใช้โปรแกรมเปิดเว็บ Google Chrome หรือ Firefox เท่านั้น</span></center><br/>" + _
        "            </div>" + _
        "          </td>" + _
        "        </tr>" + _
        "        <tr>" + _
        "          <td class='container-padding footer-text' align='left' style='font-family:Helvetica, Arial, sans-serif;font-size: 12px;line-height:16px;color:#666;padding-left:12px;padding-right:12px'>" + _
        "            <span style='font-family:tahoma;font-size:11px;'>หากมีปัญหาการใช้งานโปรดติดต่อ:</span> support_pos@jasmine.com" + _
        "            <br><br><br><br>" + _
        "          </td>" + _
        "        </tr>" + _
        "      </table>" + _
        "    </td>" + _
        "  </tr>" + _
        "</table>" + _
        "</body>" + _
        "</html>"
    End Function

    Public Sub SendMailSubmitRefund(ByVal vRequest_id As String)
        Dim vDT As New DataTable
        vDT = loadDT_Edit_Refund(vRequest_id)

        Dim vMain_Point As String = "แก้ไขรูปแบบการคินเงิน หลังปิดคำขอ"
        Dim vRequest_title As String = vDT.Rows(0).Item("request_title")
        Dim vSubject_name As String = vDT.Rows(0).Item("subject_name")
        Dim vSubject_url As String = vDT.Rows(0).Item("subject_url")
        Dim vProduct_name As String = vDT.Rows(0).Item("project_name")
        Dim vSend_uemail As String = vDT.Rows(0).Item("uemail_approve")
        Dim vSend_uemail_cc As String = vDT.Rows(0).Item("uemail_create_cc_verify")

        Dim vAccount_number As String = vDT.Rows(0).Item("account_number")
        Dim vAccount_name As String = vDT.Rows(0).Item("account_name")
        Dim vAmount As String = vDT.Rows(0).Item("amount")

        Dim vFreeText As String = ""
        vFreeText += "<p><span style='font-family:tahoma;font-size:14px;color:#BD0000;'><b>เดิม:</b> " + vDT.Rows(0).Item("pick_refund_title") + " " + vDT.Rows(0).Item("tx01") + "</span></p>"
        vFreeText += "<p><span style='font-family:tahoma;font-size:14px;color:#BD0000;'><b>ที่แก้ไข:</b> " + vDT.Rows(0).Item("edit_pick_refund_title") + " " + vDT.Rows(0).Item("edit_tx01") + "</span></p>"
        vFreeText += "<p><span style='font-family:tahoma;font-size:14px;color:#BD0000;'><b>ผู้แก้ไข:</b> " + vDT.Rows(0).Item("edit_by") + "</span></p>"

        Try

        Dim vUrl As String = CP.rGetCurrentUrl()'HttpContext.Current.Request.Url.AbsoluteUri()
        ' If vUrl.ToLower().Contains("followrequest") Then

            Dim vSplit_uemail As String() = Regex.Split(vSend_uemail, ";")
            Dim vSplit_uemail_cc As String() = Regex.Split(vSend_uemail_cc, ";")

            Dim mail As New MailMessage()
            mail.From = New MailAddress(append_mail + "FollowRequest@jasmine.com")

            For Each sMail As String In vSplit_uemail
                If sMail.Trim() <> "" Then
                    mail.To.Add(append_mail + sMail + "@jasmine.com")
                End If
            Next

            For Each sMail_cc As String In vSplit_uemail_cc
                If sMail_cc.Trim() <> "" Then
                    mail.CC.Add(append_mail + sMail_cc + "@jasmine.com")
                End If
            Next

            ' mail.CC.Add("panupong.pa@jasmine.com")

            mail.Subject = vRequest_id + ": " + vMain_Point

            mail.Body = rMailBody(vRequest_id, vRequest_title, vSubject_name, vSubject_url, vProduct_name, vMain_Point, vAccount_number, vAccount_name, vAmount, vFreeText)

            mail.IsBodyHtml = True

            Dim smtp As New SmtpClient("smtp.jasmine.com")
            smtp.Credentials = New NetworkCredential("chancharas.w", "311227")

            smtp.Send(mail)
        ' End If

        Catch ex As Exception
            'CP.InteruptRefresh()
        End Try
    End Sub
#End Region


#Region "loadDT Detail for SendMail"
    Public Function load_Request_Owner(ByVal vRequest_id As String, ByVal vSend_uemail_cc As String) As String
        Dim vOwner As String
        Dim vSql As String
        vSql = "select create_by, uemail_cc1, uemail_cc2 from request "
        vSql += "where request_id = '" + vRequest_id + "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        vOwner = vDT.Rows(0).Item("create_by")

        If vDT.Rows(0).Item("uemail_cc1") <> "" Then
            vOwner += ";" + vDT.Rows(0).Item("uemail_cc1")
        End If

        If vDT.Rows(0).Item("uemail_cc2") <> "" Then
            vOwner += ";" + vDT.Rows(0).Item("uemail_cc2")
        End If

        If vSend_uemail_cc <> "" Then
            vOwner += ";" + vSend_uemail_cc
        End If

        Return vOwner
    End Function

    Public Function loadDT_Open_Flow(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, send_uemail, next_depart_id "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM request_flow ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        and ru.flow_step = '1' and ru.disable = 0 "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "

        '***** เอา department ถัดไปเพื่อไป stamp ที่ request
        vSql += "left join ( "
        vSql += "    select request_id, depart_id next_depart_id from request_flow "
        vSql += "    where request_id = '" + vRequest_id + "' and disable = 0 and flow_step = '1' "
        vSql += ") nx "
        vSql += "on nx.request_id = rq.request_id "
        '***** เอา department ถัดไปเพื่อไป stamp ที่ request

        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_End_Flow(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, send_uemail "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM request_flow ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        and ru.next_step = 'end' and ru.disable = 0 "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Cancle_Flow(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, "
        vSql += "rq.create_by + isnull(case when uemail_cc1 <> '' Then ';' + uemail_cc1 else '' end + case when uemail_cc2 <> '' Then ';' + uemail_cc2 else '' end + case when uemail_ccv1 <> '' Then ';' + uemail_ccv1 else '' end ,'') send_uemail "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Back_Flow(ByVal vRequest_id As String) As DataTable
        ' ส่งเมล์ให้ผู้สร้าง + cc + คนใน step ที่ back กลับไป 

        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, "
        vSql += "rq.create_by + isnull(case when uemail_cc1 <> '' Then ';' + uemail_cc1 else '' end + case when uemail_cc2 <> '' Then ';' + uemail_cc2 else '' end + case when uemail_ccv1 <> '' Then ';' + uemail_ccv1 else '' end ,'') + ';' + isnull(send_uemail,'') as send_uemail "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM ( "
        vSql += "            select top 1 * from request_flow "
        vSql += "            where request_id = '" + vRequest_id + "' "
        vSql += "            and flow_complete = 0 "
        vSql += "            and disable = 0 "
        vSql += "            order by flow_step, next_step "
        vSql += "        ) ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "

        ' insertLogDebugQry(vSql)
        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Reply_1(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, "
        vSql += "rq.create_by + isnull(case when uemail_cc1 <> '' Then ';' + uemail_cc1 else '' end + case when uemail_cc2 <> '' Then ';' + uemail_cc2 else '' end + case when uemail_ccv1 <> '' Then ';' + uemail_ccv1 else '' end ,'') send_uemail "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Reply_2(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, "
        vSql += "rq.create_by + isnull(case when uemail_cc1 <> '' Then ';' + uemail_cc1 else '' end + case when uemail_cc2 <> '' Then ';' + uemail_cc2 else '' end + case when uemail_ccv1 <> '' Then ';' + uemail_ccv1 else '' end ,'') + ';' + isnull(send_uemail,'') as send_uemail "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM ( "
        vSql += "            select top 1 * from request_flow_sub "
        vSql += "            where request_id = '" + vRequest_id + "' "
        vSql += "            and flow_complete = 0 "
        vSql += "            and disable = 0 "
        vSql += "            order by flow_step desc, flow_sub_step desc "
        vSql += "        ) ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "
    
        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Add_Next(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, send_uemail "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM ( "
        vSql += "            select top 1 * from request_flow_sub "
        vSql += "            where request_id = '" + vRequest_id + "' "
        vSql += "            and flow_complete = 0 "
        vSql += "            and disable = 0 "
        vSql += "            order by flow_step desc, flow_sub_step desc "
        vSql += "        ) ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "
        vSql += "where rq.request_id = '" + vRequest_id + "' "
    
        Return DB105.GetDataTable(vSql)
    End Function

    Public Function loadDT_Next_Flow(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select top 1 * from ( "
        vSql += "    select request_id, flow_id "
        vSql += "    , flow_step, 0 flow_sub_step, next_step, flow_complete "
        vSql += "    from request_flow "
        vSql += "    where request_id = '" + vRequest_id + "' "
        vSql += "    and flow_complete = 0 "
        vSql += "    and next_step <> '-' "
        vSql += "    and next_step <> 'end' "
        vSql += "    and disable = 0 "
        vSql += "    union "
        vSql += "    select request_id, flow_id "
        vSql += "    , flow_step, flow_sub_step, next_step, flow_complete "
        vSql += "    from request_flow_sub "
        vSql += "    where request_id = '" + vRequest_id + "' "
        vSql += "    and flow_complete = 0 "
        vSql += "    and next_step <> '-' "
        vSql += "    and next_step <> 'end' "
        vSql += "    and disable = 0 "
        vSql += ") rf_union "
        vSql += "order by flow_step, flow_sub_step "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Try
            If vDT.Rows(0).Item("flow_sub_step") = 0 Then
                vSql = loadDT_Next_Flow_Main(vRequest_id, vDT.Rows(0).Item("flow_step"))
            Else
                vSql = loadDT_Next_Flow_Sub(vRequest_id)
            End If

            Return DB105.GetDataTable(vSql)
        Catch ex As Exception
            CP.InteruptRefresh()
        End Try
    End Function

    Public Function loadDT_Next_Flow_Main(ByVal vRequest_id As String, ByVal vNext_step As String) As String
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, send_uemail, next_depart_id "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM request_flow ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        and ru.flow_step = '" + vNext_step + "' and ru.disable = 0 "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "

        '***** เอา department ถัดไปเพื่อไป stamp ที่ request (next_depart) 
        vSql += "left join ( "
        vSql += "    select request_id, depart_id next_depart_id from request_flow "
        vSql += "    where request_id = '" + vRequest_id + "' and flow_step = '" + vNext_step + "' "
        vSql += "    and disable = 0 "
        vSql += ") nx "
        vSql += "on nx.request_id = rq.request_id "
        '***** เอา department ถัดไปเพื่อไป stamp ที่ request (next_depart)

        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return vSql
    End Function

    Public Function loadDT_Next_Flow_Sub(ByVal vRequest_id As String) As String
        Dim vSql As String
        vSql = "declare @temp table (request_id varchar(20), send_uemail varchar(255), next_depart_id int) "
        vSql += "insert into @temp select top 1 request_id, send_uemail, depart_id next_depart_id "
        vSql += "from request_flow_sub  "
        vSql += "where request_id = '" + vRequest_id + "' and flow_complete = 0 "
        vSql += "and disable = 0 "
        vSql += "order by flow_step, flow_sub_step "

        vSql += "select project_name, subject_name, subject_url, request_title, rf.send_uemail, next_depart_id "
        vSql += ", account_number, account_name, amount "
        vSql += "from request rq "
        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "
        vSql += "join ( "
        vSql += "    SELECT rm.request_id, send_uemail = STUFF(( "
        vSql += "        SELECT ';' + ru.send_uemail "
        vSql += "        FROM ( "
        vSql += "            select * from @temp "
        vSql += "        ) ru "
        vSql += "        WHERE rm.request_id = ru.request_id "
        vSql += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') "
        vSql += "    FROM request rm "
        vSql += "    where rm.request_id = '" + vRequest_id + "' "
        vSql += ") rf "
        vSql += "on rf.request_id = rq.request_id "

        '***** เอา department ถัดไปเพื่อไป stamp ที่ request (next_depart)
        vSql += "left join @temp next_sub "
        vSql += "on next_sub.request_id = rq.request_id "
        '***** เอา department ถัดไปเพื่อไป stamp ที่ request (next_depart)

        vSql += "where rq.request_id = '" + vRequest_id + "' "

        Return vSql
    End Function

    Function loadDT_Edit_Refund(ByVal vRequest_id As String) As DataTable
        Dim vSql As String
        vSql = "select project_name ,subject_name, subject_url, request_title, account_number, account_name, amount, uemail_approve, "
        vSql += "rq.create_by + isnull(case when uemail_cc1 <> '' Then ';' + uemail_cc1 else '' end + case when uemail_cc2 <> '' Then ';' + uemail_cc2 else '' end + case when uemail_verify1 <> '' Then ';' + uemail_verify1 else '' end + case when uemail_verify2 <> '' Then ';' + uemail_verify2 else '' end ,'') uemail_create_cc_verify, "
        vSql += "refund.pick_refund_title, refund.tx01, refund.edit_pick_refund_title, refund.edit_tx01, refund.edit_by, refund.edit_date "
        vSql += "from request rq "

        vSql += "join ("
        vSql += "    select top 1 request_id, p1.pick_refund_title, tx01, "
        vSql += "    p2.pick_refund_title edit_pick_refund_title, edit_tx01, "
        vSql += "    edit_by, edit_date "
        vSql += "    from log_edit_refund ed "
        vSql += "    join pick_refund p1 on p1.pick_refund_id = ed.pick_refund "
        vSql += "    join pick_refund p2 on p2.pick_refund_id = ed.edit_pick_refund "
        vSql += "    where request_id = '" + vRequest_id + "' "
        vSql += "    order by edit_date desc "
        vSql += ") refund on refund.request_id = rq.request_id "

        vSql += "join subject_dim on subject_dim.subject_id = rq.subject_id "
        vSql += "join project_dim on project_dim.project_id = subject_dim.project_id "

        vSql += "where rq.request_id = '" + vRequest_id + "'"

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function dateDX(ByVal dxdt As String)
        If dxdt.Trim() = "valueEmpty" Then
            Return "NULL "
        Else If dxdt.Trim().Length = 10 Then
            Return "CONVERT(DATE,'" + dxdt + "',103) "
        Else
            Return "NULL "
        End If
    End Function

    Public Function declareDX(ByVal dxstr As String, ByVal dxdt As String) As String
        If dxdt.Trim() = "" Then
            Return "DECLARE @" + dxstr + " DATETIME = NULL "

        Else If dxdt.Trim().Length = 10 Then
            Return "DECLARE @" + dxstr + " DATETIME = CONVERT(DATE,'" + dxdt + "',103) "

        Else
            Return "DECLARE @" + dxstr + " DATETIME = " + dxdt + " "
        End If
    End Function

    Public Function declareDX01(ByVal dx01 As String)
        If dx01.Trim() = "" Then
            Return "DECLARE @dx01 DATETIME = NULL "
        Else
            Return "DECLARE @dx01 DATETIME = CONVERT(DATE,'" + dx01 + "',103) "
        End If
    End Function

    Public Function declareDX02(ByVal dx02 As String)
        If dx02.Trim() = "" Then
            Return "DECLARE @dx02 DATETIME = NULL "
        Else
            Return "DECLARE @dx02 DATETIME = CONVERT(DATE,'" + dx02 + "',103) "
        End If
    End Function

    Public Function declareDX03(ByVal dx03 As String)
        If dx03.Trim() = "" Then
            Return "DECLARE @dx03 DATETIME = NULL "
        Else
            Return "DECLARE @dx03 DATETIME = CONVERT(DATE,'" + dx03 + "',103) "
        End If
    End Function

    Public Function rLinkOpenfile(ByVal vRequest_id As String, ByVal vPath_file As String, ByVal vRequest_file As String) As String
        Return "<a href='openfile.aspx?request_id=" & vRequest_id & "&path=" & vPath_file & "&file=" & vRequest_file & "' target='_blank'>เปิดไฟล์..</a>"
    End Function

    Public Function itemRefNumber()
        Return DateTime.Now.ToString("yyMMddHHmmssfff") & CP.rRandomKeyGen(5)
    End Function

#End Region

End Class

