Imports System.Data
Imports System.Data.OleDb
Imports System.Net.IPAddress
Namespace WebApplication2
    Partial Class cheque_BB
        Inherits System.Web.UI.Page
        Dim connectDB As String = DBConnect.getStrDBConnect()
        Dim Log As New Cls_LogReport
        Dim C As New Cls_Data
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Session("email") = "varut.v"
            'Session("email") = "chancharasw"
            If Session("email") Is Nothing Then
                Response.Redirect("~/Chksession.aspx")
                ClientScript.RegisterStartupScript(Page.GetType, "open", "window.close()", True)
                'ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('default.aspx?','_blank');self.focus();", True)
            End If
            If Not Page.IsPostBack Then
                showArea1(Nothing)
                showArea2(Nothing)
                showprovince(Nothing)
                'showprovince1(Nothing)
                showShop(Nothing)
                'showShop1(Nothing)
                '////////// Cookie ///////
                Try
                    company.SelectedValue = Request.Cookies("ChequeBB")("Company")
                    area.SelectedValue = Request.Cookies("ChequeBB")("AreaFrom")
                    area1.SelectedValue = Request.Cookies("ChequeBB")("AreaTo")
                    prov.SelectedValue = Request.Cookies("ChequeBB")("ProvinceFrom")
                    prov1.SelectedValue = Request.Cookies("ChequeBB")("ProvinceTo")
                    shop.SelectedValue = Request.Cookies("ChequeBB")("BranchFrom")
                    shop1.SelectedValue = Request.Cookies("ChequeBB")("BranchTo")

                Catch ex As Exception
                    Response.Cookies("ChequeBB").Expires = DateTime.MaxValue
                    Response.Cookies("ChequeBB")("Login") = Session("email").ToString
                    Response.Cookies("ChequeBB")("Company") = ""

                    Response.Cookies("ChequeBB")("AreaFrom") = ""
                    Response.Cookies("ChequeBB")("AreaTo") = ""

                    Response.Cookies("ChequeBB")("ProvinceFrom") = ""
                    Response.Cookies("ChequeBB")("ProvinceTo") = ""

                    Response.Cookies("ChequeBB")("BranchFrom") = ""
                    Response.Cookies("ChequeBB")("BranchTo") = ""

                End Try
                '////////////////////////
            End If
        End Sub


        Private Sub company_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles company.Init
            'Dim myconn As New OleDbConnection(connectDB)
            'myconn.Open()
            '' Dim email As String = Session("email")
            ''Dim strsql As String = "select distinct a.f01,a.f02 ,a.f01+' '+':'+' '+a.f02 as ncom from m00010 a ,m00030 o,m00860 ma where   ma.f03 = o.f02 order by a.f01 asc"
            Dim strsql As String
            'If Session("TypeLog") = "TTTi" Then
            '    strsql = "select distinct a.f01,a.f02 ,a.f01+' '+':'+' '+a.f02 as ncom from m00010 a ,m00030 o,m00860 ma where   ma.f03 = o.f02 order by a.f01 asc"
            'Else
            '    strsql = "select distinct a.f01,a.f02 ,a.f01+' '+':'+' '+a.f02 as ncom from m00010 a ,m00030 o,m00860 ma where   ma.f03 = o.f02 order by a.f01 asc"
            'End If
            'Dim myda As New OleDbDataAdapter(strsql, myconn)
            'Dim ds As New DataSet
            'myda.Fill(ds, "Table1")

            'If ds.Tables("Table1").Rows.Count <> 0 Then
            '    '  La.Text = ""
            'Else
            '    If Session("TypeLog") = "TTTi" Then
            '        strsql = "select distinct a.f01,a.f02 ,a.f01+' '+':'+' '+a.f02 as ncom from m00010 a ,m00030 o,m00860 ma where ma.f03='all' "
            '    Else
            '        strsql = "select distinct a.f01,a.f02 ,a.f01+' '+':'+' '+a.f02 as ncom from m00010 a ,m00030 o,m00860 ma where ma.f03='all' "
            '    End If
            '    'strsql = "select distinct a.f01,a.f02 ,a.f01+' '+':'+' '+a.f02 as ncom from m00010 a ,m00030 o,m00860 ma where ma.f03='all' "
            '    myda = New OleDbDataAdapter(strsql, myconn)
            '    myda.Fill(ds, "Table1")
            'End If
            'Dim drow As DataRow = ds.Tables("Table1").NewRow
            'drow.Item("f01") = 0  'valueField
            'drow.Item("ncom") = "โปรดระบุ"   'TextField
            'ds.Tables("Table1").Rows.InsertAt(drow, 0)

            'company.DataSource = ds.Tables("Table1")
            'company.DataTextField = "ncom"
            'company.DataValueField = "f01"
            'company.DataBind()

            'myconn.Close()
            strsql = "select * from xv_POSREPORT_LOADCOMPANY"
            C.SetDropDownListCompany(company, strsql, "comp_name", "comp_code")
        End Sub
        Private Sub showArea1(ByVal company As String)
            Dim myconn As New OleDbConnection(connectDB)
            myconn.Open()

            Dim strsql As String = "select distinct o.f03 ,a.f02+' '+':'+' '+a.f03  as narea from m00860 ma, m00030 o ,m02300 a where ma.f03 = o.f02 and a.f02 = o.f03 and ma.f02='" & Session("email") & "'"
            Dim myda As New OleDbDataAdapter(strsql, myconn)
            Dim ds As New DataSet
            myda.Fill(ds, "Table1")
            If ds.Tables("Table1").Rows.Count <> 0 Then
                'Label1.Text = "กรุณาเลือกเขต"
            Else
                strsql = "select  distinct o.f03 ,a.f02+' '+':'+' '+a.f03  as narea from m00860 ma, m00030 o ,m02300 a where  ma.f03='all' and a.f02 = o.f03  and ma.f02='" & Session("email") & "'"
                myda = New OleDbDataAdapter(strsql, myconn)
                myda.Fill(ds, "Table1")
            End If
            Dim drow As DataRow = ds.Tables("Table1").NewRow
            drow.Item("f03") = 0  'valueField
            drow.Item("narea") = "โปรดระบุเขตการขาย"   'TextField
            ds.Tables("Table1").Rows.InsertAt(drow, 0)

            area.DataSource = ds.Tables("Table1")
            area.DataTextField = "narea"
            area.DataValueField = "f03"
            area.DataBind()
            myconn.Close()
        End Sub
        Private Sub showArea2(ByVal company As String)
            Dim myconn As New OleDbConnection(connectDB)
            myconn.Open()

            Dim strsql As String = "select distinct o.f03 ,a.f02+' '+':'+' '+a.f03  as narea from m00860 ma, m00030 o ,m02300 a where ma.f03 = o.f02 and a.f02 = o.f03  and ma.f02='" & Session("email") & "'"
            Dim myda As New OleDbDataAdapter(strsql, myconn)
            Dim ds As New DataSet
            myda.Fill(ds, "Table1")

            If ds.Tables("Table1").Rows.Count <> 0 Then
                'Label1.Text = "กรุณาเลือกเขต"
            Else
                strsql = "select  distinct o.f03 ,a.f02+' '+':'+' '+a.f03  as narea from m00860 ma, m00030 o ,m02300 a where  ma.f03='all' and a.f02 = o.f03 and ma.f02='" & Session("email") & "'"
                myda = New OleDbDataAdapter(strsql, myconn)
                myda.Fill(ds, "Table1")
            End If
            Dim drow As DataRow = ds.Tables("Table1").NewRow
            drow.Item("f03") = 0  'valueField
            drow.Item("narea") = "โปรดระบุเขตการขาย"   'TextField
            ds.Tables("Table1").Rows.InsertAt(drow, 0)
            area1.DataSource = ds.Tables("Table1")
            area1.DataTextField = "narea"
            area1.DataValueField = "f03"
            area1.DataBind()

            myconn.Close()
        End Sub
        Private Sub showprovince(ByVal area11 As String)
            Dim myconn As New OleDbConnection(connectDB)
            myconn.Open()
            Dim strsql As String
            '///////////////////////////

            Dim strW As String = ""
            If area.SelectedIndex <> 0 Then
                strW = " and o.f03 >= '" & area.SelectedValue.ToString & "'"
            End If
            If area1.SelectedIndex <> 0 Then
                If strW <> "" Or area.SelectedIndex = 0 Then strW = strW + " and "
                strW = strW + "o.f03 <= '" & area1.SelectedValue.ToString & "'"
            End If

            '//////////////////////////

            If area.SelectedIndex <> 0 Or area1.SelectedIndex <> 0 Then
                strsql = "select distinct o.f12, m02.f06 , m02.f06,m02.f06+' '+':'+' '+m02.f03 as nprov from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12" + strW + " and ma.f02='" & Session("email") & "' order by m02.f06 "
            Else
                strsql = "select distinct o.f12, m02.f06 , m02.f06,m02.f06+' '+':'+' '+m02.f03 as nprov from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12  and ma.f02='" & Session("email") & "' order by m02.f06 "
            End If

            Dim myda As New OleDbDataAdapter(strsql, myconn)

            Dim ds As New DataSet
            myda.Fill(ds, "Table1")

            If ds.Tables("Table1").Rows.Count <> 0 Then
                ' Label2.Text = "กรุณาเลือกจังหวัด"
            Else
                If area.SelectedIndex <> 0 Or area1.SelectedIndex <> 0 Then
                    strsql = "select distinct o.f12,m02.f06 , m02.f06,m02.f06+' '+':'+' '+m02.f03 as nprov  from m02020 m02,m00030 o , m00860 ma where m02.f02= o.f12" + strW + "and  ma.f02='" & Session("email") & "'order by m02.f06"
                Else
                    strsql = "select distinct o.f12,m02.f06 , m02.f06,m02.f06+' '+':'+' '+m02.f03 as nprov  from m02020 m02,m00030 o , m00860 ma where m02.f02= o.f12 and  ma.f02='" & Session("email") & "'order by m02.f06"

                End If
                '  strsql = "select distinct o.f12,mo2.f03   from m02020 mo2,m00030 o , m00860 ma where mo2.f02= o.f12  and o.f03 = '" & area1 & "'and  ma.f02='" & Session("email") & "'"
                myda = New OleDbDataAdapter(strsql, myconn)
                myda.Fill(ds, "Table1")
            End If
            Dim drow As DataRow = ds.Tables("Table1").NewRow
            drow.Item("f12") = 0
            drow.Item("nprov") = "โปรดระบุจังหวัด"
            ds.Tables("Table1").Rows.InsertAt(drow, 0)

            prov.DataSource = ds.Tables("Table1")
            prov.DataTextField = "nprov"
            prov.DataValueField = "f06"
            prov.DataBind()

            prov1.DataSource = ds.Tables("Table1")
            prov1.DataTextField = "nprov"
            prov1.DataValueField = "f06"
            prov1.DataBind()

            myconn.Close()
        End Sub
        ' Don't Use!
        Private Sub showprovince1(ByVal area1 As String)
            Dim myconn As New OleDbConnection(connectDB)
            myconn.Open()
            Dim strsql As String
            If area1 IsNot Nothing Then
                strsql = "select distinct o.f12, m02.f06 ,m02.f06+' '+':'+' '+m02.f03 as nprov  from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12 and o.f03 = '" & area1 & "' and ma.f02='" & Session("email") & "'order by m02.f06"
            Else
                strsql = "select distinct o.f12, m02.f06 ,m02.f06+' '+':'+' '+m02.f03 as nprov  from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12 and ma.f02='" & Session("email") & "'order by m02.f06"
            End If
            Dim myda As New OleDbDataAdapter(strsql, myconn)
            Dim ds As New DataSet
            myda.Fill(ds, "Table1")
            If ds.Tables("Table1").Rows.Count <> 0 Then
                'Label2.Text = "กรุณาเลือกจังหวัด"
            Else
                If area1 IsNot Nothing Then
                    strsql = "select distinct o.f12,m02.f06  ,m02.f06+' '+':'+' '+m02.f03 as nprov  from m02020 m02,m00030 o , m00860 ma where m02.f02= o.f12  and o.f03 = '" & area1 & "'and  ma.f02='" & Session("email") & "'order by m02.f06"
                Else
                    strsql = "select distinct o.f12,m02.f06  ,m02.f06+' '+':'+' '+m02.f03 as nprov  from m02020 m02,m00030 o , m00860 ma where m02.f02= o.f12 and  ma.f02='" & Session("email") & "'order by m02.f06"
                End If
                myda = New OleDbDataAdapter(strsql, myconn)
                myda.Fill(ds, "Table1")
            End If
            Dim drow As DataRow = ds.Tables("Table1").NewRow
            drow.Item("f12") = 0
            drow.Item("nprov") = "โปรดระบุจังหวัด"
            ds.Tables("Table1").Rows.InsertAt(drow, 0)

            prov1.DataSource = ds.Tables("Table1")
            prov1.DataTextField = "nprov"
            prov1.DataValueField = "f12"
            prov1.DataBind()

            myconn.Close()
        End Sub
        Private Sub showShop(ByVal prov00 As String)
            Dim myconn As New OleDbConnection(connectDB)
            myconn.Open()
            Dim strsql As String
            '////////////////////////

            Dim strW As String = ""

            If area.SelectedIndex <> 0 Then
                strW = " and o.f03 >= '" & area.SelectedValue.ToString & "'"
            End If
            If area1.SelectedIndex <> 0 Then
                If strW <> "" Or area.SelectedIndex = 0 Then strW = strW + " and "
                strW = strW + "o.f03 <= '" & area1.SelectedValue.ToString & "'"
            End If
            If prov.SelectedIndex <> 0 Then
                If strW <> "" Or (area.SelectedIndex = 0 And area1.SelectedIndex = 0) Then strW = strW + " and "
                strW = strW + "m02.f06 >= '" & prov.SelectedValue.ToString & "'"
            End If
            If prov1.SelectedIndex <> 0 Then
                If strW <> "" Or (area.SelectedIndex = 0 And area1.SelectedIndex = 0 And prov.SelectedIndex = 0) Then strW = strW + " and "
                strW = strW + "m02.f06 <= '" & prov1.SelectedValue.ToString & "'"
            End If

            '///////////////////////

            'If province00 IsNot Nothing Then
            If area.SelectedIndex <> 0 Or area1.SelectedIndex <> 0 Or prov.SelectedIndex <> 0 Or prov1.SelectedIndex <> 0 Then
                strsql = "select distinct o.f02,o.f02+' '+':'+' '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end )as nShop  from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') " + strW + " and ma.f02='" & Session("email") & "'" + " order by o.f02"
            Else
                strsql = "select distinct o.f02,o.f02+' '+':'+' '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as nShop  from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" & Session("email") & "'" + " order by o.f02"
            End If

            Dim myda As New OleDbDataAdapter(strsql, myconn)
            Dim ds As New DataSet
            myda.Fill(ds, "Table1")

            If ds.Tables("Table1").Rows.Count <> 0 Then

            Else
                'If province00 IsNot Nothing Then
                If area.SelectedIndex <> 0 Or area1.SelectedIndex <> 0 Or prov.SelectedIndex <> 0 Or prov1.SelectedIndex <> 0 Then
                    strsql = "select distinct o.f02,o.f02+' '+':'+' '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as nShop  from m00030 o, m02020 m02, m00860 ma where  m02.f02 = o.f12 and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') " + strW + " and ma.f02='" & Session("email") & "'" + " order by o.f02"
                Else
                    strsql = "select distinct o.f02,o.f02+' '+':'+' '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end )as nShop  from m00030 o, m02020 m02, m00860 ma where  m02.f02 = o.f12 and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" & Session("email") & "'" + " order by o.f02"
                End If

                myda = New OleDbDataAdapter(strsql, myconn)
                myda.Fill(ds, "Table1")
            End If

            Dim drow As DataRow = ds.Tables("Table1").NewRow
            drow.Item("f02") = 0  'valueField
            drow.Item("nShop") = "โปรดระบุสาขา"   'TextField
            ds.Tables("Table1").Rows.InsertAt(drow, 0)

            shop.DataSource = ds.Tables("Table1")
            shop.DataTextField = "nShop"
            shop.DataValueField = "f02"
            shop.DataBind()

            shop1.DataSource = ds.Tables("Table1")
            shop1.DataTextField = "nShop"
            shop1.DataValueField = "f02"
            shop1.DataBind()

            myconn.Close()
        End Sub
        ' Don't Use!
        Private Sub showShop1(ByVal prov1 As String)
            Dim myconn As New OleDbConnection(connectDB)
            myconn.Open()
            Dim strsql As String
            If prov1 IsNot Nothing Then
                strsql = "select distinct o.f02 ,o.f02+' '+':'+' '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as nShop from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and o.f12 = '" & prov1 & "' and ma.f02='" & Session("email") & "'"
            Else
                strsql = "select distinct o.f02 ,o.f02+' '+':'+' '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as nShop from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01')  and ma.f02='" & Session("email") & "'"
            End If
            Dim myda As New OleDbDataAdapter(strsql, myconn)
            Dim ds As New DataSet
            myda.Fill(ds, "Table1")

            If ds.Tables("Table1").Rows.Count <> 0 Then
                'Label3.Text = "กรุณาเลือกสำนักงาน"
            Else
                If prov1 IsNot Nothing Then
                    strsql = "select distinct o.f02 ,o.f02+' '+':'+' '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as nShop from m00030 o, m02020 mo2, m00860 ma where  mo2.f02 = o.f12 and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and o.f12 = '" & prov1 & "' and ma.f02='" & Session("email") & "'"
                Else
                    strsql = "select distinct o.f02 ,o.f02+' '+':'+' '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as nShop from m00030 o, m02020 mo2, m00860 ma where  mo2.f02 = o.f12 and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" & Session("email") & "'"
                End If
                myda = New OleDbDataAdapter(strsql, myconn)
                myda.Fill(ds, "Table1")
            End If

            Dim drow As DataRow = ds.Tables("Table1").NewRow
            drow.Item("f02") = 0  'valueField
            drow.Item("nShop") = "โปรดระบุสาขา"   'TextField
            ds.Tables("Table1").Rows.InsertAt(drow, 0)

            shop1.DataSource = ds.Tables("Table1")
            shop1.DataTextField = "nShop"
            shop1.DataValueField = "f02"
            shop1.DataBind()
            myconn.Close()
        End Sub
        Private Sub company_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles company.SelectedIndexChanged
            showArea1(company.SelectedValue)
            showArea2(company.SelectedValue)
        End Sub

        Protected Sub OK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OK.Click
            'If company.SelectedIndex = 0 Then
            '    ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณาระบุบริษัท!');", True)
            'Else
            If startdate.Text = "" Or startdate.Text.Length <> 10 Then
                ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณาระบุช่วงวันที่ให้ถูกต้อง!');", True)
            ElseIf enddate.Text = "" Or enddate.Text.Length <> 10 Then
                ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณาระบุช่วงวันที่ให้ถูกต้อง!');", True)
            Else
                If chkParameter.Checked = True Then
                    Response.Cookies("ChequeBB").Expires = DateTime.MaxValue
                    Response.Cookies("ChequeBB")("Login") = Session("email").ToString
                    Response.Cookies("ChequeBB")("Company") = company.SelectedValue

                    Response.Cookies("ChequeBB")("AreaFrom") = area.SelectedValue
                    Response.Cookies("ChequeBB")("AreaTo") = area1.SelectedValue

                    Response.Cookies("ChequeBB")("ProvinceFrom") = prov.SelectedValue
                    Response.Cookies("ChequeBB")("ProvinceTo") = prov.SelectedValue

                    Response.Cookies("ChequeBB")("BranchFrom") = shop.SelectedValue
                    Response.Cookies("ChequeBB")("BranchTo") = shop1.SelectedValue

                Else
                    Response.Cookies("ChequeBB").Expires = DateTime.MaxValue
                    Response.Cookies("ChequeBB")("Login") = Session("email").ToString
                    Response.Cookies("ChequeBB")("Company") = ""

                    Response.Cookies("ChequeBB")("AreaFrom") = ""
                    Response.Cookies("ChequeBB")("AreaTo") = ""

                    Response.Cookies("ChequeBB")("ProvinceFrom") = ""
                    Response.Cookies("ChequeBB")("ProvinceTo") = ""

                    Response.Cookies("ChequeBB")("BranchFrom") = ""
                    Response.Cookies("ChequeBB")("BranchTo") = ""

                End If
                ShowRpt()
                Session("ExportChequeBB") = "no"
                Dim url As String
                url = "ReportCheque_BB.aspx?shop=" & IIf(shop.SelectedIndex <> 0, shop.SelectedValue, "000") _
                & "&shop1=" & IIf(shop1.SelectedIndex <> 0, shop1.SelectedValue, "ZZZ") _
                & "&area1=" & IIf(area1.SelectedIndex <> 0, area1.SelectedValue, "ZZZ") _
                 & "&area=" & IIf(area.SelectedIndex <> 0, area.SelectedValue, "000") _
                 & "&prov=" & IIf(prov.SelectedIndex <> 0, prov.SelectedValue, "000") _
                 & "&prov1=" & IIf(prov1.SelectedIndex <> 0, prov1.SelectedValue, "ZZZ") _
                  & "&company=" & company.SelectedValue _
                 & "&startdate=" & startdate.Text _
                & "&enddate=" & enddate.Text


                Dim rDate As DateTime = Now()
                'Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
                Dim Parameter As String = "Area F:" + area.SelectedValue.ToString + " T:" + area1.SelectedValue.ToString + " Province F:" + prov.SelectedValue.ToString + " T:" + prov1.SelectedValue.ToString + " Shop F:" + shop.SelectedValue.ToString + " T:" + shop1.SelectedValue.ToString + " Date F:" + Log.CDateText(startdate.Text) + " T:" + Log.CDateText(enddate.Text)
                Session("Parameter") = Parameter
                Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
                'Log.LogReport("22020", "รายงานรับเช็ค", "Area F:" + area.SelectedValue.ToString + " T:" + area1.SelectedValue.ToString + " Province F:" + prov.SelectedValue.ToString + " T:" + prov1.SelectedValue.ToString + " Shop F:" + shop.SelectedValue.ToString + " T:" + shop1.SelectedValue.ToString + " Date F:" + Log.CDateText(startdate.Text) + " T:" + Log.CDateText(enddate.Text), vIPAddress, Session("email"))


                ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('" + url + "','_blank','height=300,width=620,left=130,top=150,resizable=1,scrollbars=1');", True)
            End If
        End Sub


        Protected Sub area_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles area.SelectedIndexChanged
            ' showprovince(area.SelectedValue)
            If area.SelectedIndex = 0 Then
                showprovince(Nothing)
            Else
                showprovince(area.SelectedValue)
            End If
            showShop(Nothing)
        End Sub
        Protected Sub area1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles area1.SelectedIndexChanged
            ' showprovince1(area1.SelectedValue)
            If area1.SelectedIndex = 0 Then
                showprovince(Nothing)
            Else
                showprovince(area1.SelectedValue)
            End If
            showShop(Nothing)
        End Sub

        Protected Sub prov_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles prov.SelectedIndexChanged
            'showShop(prov.SelectedValue)
            If prov.SelectedIndex = 0 Then
                showShop(Nothing)
            Else
                showShop(prov.SelectedValue)
            End If
        End Sub

        Protected Sub prov1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles prov1.SelectedIndexChanged
            ' showShop1(prov1.SelectedValue)
            If prov1.SelectedIndex = 0 Then
                showShop(Nothing)
            Else
                showShop(prov1.SelectedValue)
            End If
        End Sub

        Protected Sub Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Clear.Click
            company.ClearSelection()
            area.ClearSelection()
            area1.ClearSelection()
            prov.ClearSelection()
            prov1.ClearSelection()
            shop.ClearSelection()
            shop1.ClearSelection()
        End Sub

        Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
            'If company.SelectedIndex = 0 Then
            '    ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณาระบุบริษัท!');", True)
            'Else
            If startdate.Text = "" Or startdate.Text.Length <> 10 Then
                ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณาระบุช่วงวันที่ให้ถูกต้อง!');", True)
            ElseIf enddate.Text = "" Or enddate.Text.Length <> 10 Then
                ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณาระบุช่วงวันที่ให้ถูกต้อง!');", True)
            Else
                ShowRpt()
                Session("ExportChequeBB") = "yes"
                Dim url As String
                url = "ReportCheque_BB.aspx?shop=" & IIf(shop.SelectedIndex <> 0, shop.SelectedValue, "000") _
                & "&shop1=" & IIf(shop1.SelectedIndex <> 0, shop1.SelectedValue, "ZZZ") _
                & "&area1=" & IIf(area1.SelectedIndex <> 0, area1.SelectedValue, "ZZZ") _
                 & "&area=" & IIf(area.SelectedIndex <> 0, area.SelectedValue, "000") _
                 & "&prov=" & IIf(prov.SelectedIndex <> 0, prov.SelectedValue, "000") _
                 & "&prov1=" & IIf(prov1.SelectedIndex <> 0, prov1.SelectedValue, "ZZZ") _
                  & "&company=" & company.SelectedValue _
                 & "&startdate=" & startdate.Text _
                & "&enddate=" & enddate.Text
                'If cboFormat.SelectedIndex = 1 Then
                '    Session("Format") = 1
                'ElseIf cboFormat.SelectedIndex = 2 Then
                '    Session("Format") = 2
                'ElseIf cboFormat.SelectedIndex = 3 Then
                '    Session("Format") = 3
                'ElseIf cboFormat.SelectedIndex = 4 Then
                '    Session("Format") = 4
                'End If
                'If cboFormat.SelectedIndex <> 0 Then

                Dim rDate As DateTime = Now()
                Dim Parameter As String = "Area F:" + area.SelectedValue.ToString + " T:" + area1.SelectedValue.ToString + " Province F:" + prov.SelectedValue.ToString + " T:" + prov1.SelectedValue.ToString + " Shop F:" + shop.SelectedValue.ToString + " T:" + shop1.SelectedValue.ToString + " Date F:" + Log.CDateText(startdate.Text) + " T:" + Log.CDateText(enddate.Text)
                Session("Parameter") = Parameter
                Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))


                ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('" + url + "','_blank','height=300,width=620,left=130,top=150,resizable=1,scrollbars=1');", True)
                'Else
                'ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณาเลือก Format ที่ต้องการ Export');", True)
                'End If
            End If
        End Sub

        Private Sub ShowRpt()
            Dim vHeader As String = ""
            Dim SAreaName As String = ""
            Dim EAreaName As String = ""
            Dim SprovinceName As String = ""
            Dim EprovinceName As String = ""
            Dim SbranchName As String = ""
            Dim EbranchName As String = ""
            Dim vCompany As String = ""

            If startdate.Text.Length <> 0 Then
                vHeader += " จากวันที่ " + startdate.Text
            End If
            vHeader += "|"
            If enddate.Text.Length <> 0 Then
                vHeader += " ถึงวันที่ " + enddate.Text
            End If
            vHeader += "|"
            If Not (area.SelectedIndex = 0 Or area.Items.Count = 0) Then
                SAreaName = Replace(area.SelectedItem.ToString, " : ", "/")
                SAreaName = Mid(SAreaName, InStr(Replace(area.SelectedItem.ToString, " : ", "/"), "/") + 1, Len(SAreaName))
                vHeader += " จาก " + SAreaName.ToString
            Else
                vHeader += " จากเขต ทั้งหมด "
            End If
            vHeader += "|"
            If Not (area1.SelectedIndex = 0 Or area1.Items.Count = 0) Then
                EAreaName = Replace(area1.SelectedItem.ToString, " : ", "/")
                EAreaName = Mid(EAreaName, InStr(Replace(area1.SelectedItem.ToString, " : ", "/"), "/") + 1, Len(EAreaName))
                vHeader += " ถึง " + EAreaName.ToString
            Else
                vHeader += " ถึงเขต ทั้งหมด "
            End If
            vHeader += "|"
            If Not (prov.SelectedIndex = 0 Or prov.Items.Count = 0) Then
                SprovinceName = Replace(prov.SelectedItem.ToString, " : ", "/")
                SprovinceName = Mid(SprovinceName, InStr(Replace(prov.SelectedItem.ToString, " : ", "/"), "/") + 1, Len(SprovinceName))
                vHeader += " จากจังหวัด " + SprovinceName.ToString
            Else
                vHeader += " จากจังหวัด ทั้งหมด "
            End If
            vHeader += "|"
            If Not (prov1.SelectedIndex = 0 Or prov1.Items.Count = 0) Then
                EprovinceName = Replace(prov1.SelectedItem.ToString, " : ", "/")
                EprovinceName = Mid(EprovinceName, InStr(Replace(prov1.SelectedItem.ToString, " : ", "/"), "/") + 1, Len(EprovinceName))
                vHeader += " ถึงจังหวัด " + SprovinceName.ToString
            Else
                vHeader += " ถึงจังหวัด ทั้งหมด "
            End If
            vHeader += "|"
            If Not (shop.SelectedIndex = 0 Or shop.Items.Count = 0) Then
                SbranchName = Replace(shop.SelectedItem.ToString, " : ", "/")
                SbranchName = Mid(SbranchName, InStr(Replace(shop.SelectedItem.ToString, " : ", "/"), "/") + 1, Len(SbranchName))
                vHeader += " จากสำนักงาน " + SbranchName.ToString
            Else
                vHeader += " จากสำนักงาน ทั้งหมด "
            End If
            vHeader += "|"
            If Not (shop1.SelectedIndex = 0 Or shop1.Items.Count = 0) Then
                EbranchName = Replace(shop1.SelectedItem.ToString, " : ", "/")
                EbranchName = Mid(EbranchName, InStr(Replace(shop1.SelectedItem.ToString, " : ", "/"), "/") + 1, Len(EbranchName))
                vHeader += " ถึงสำนักงาน " + EbranchName.ToString
            Else
                vHeader += " ถึงสำนักงาน ทั้งหมด "
            End If
            vHeader += "|"
            'If Not (company.SelectedIndex = 0 Or company.Items.Count = 0) Then
            vCompany = Replace(company.SelectedItem.ToString, " : ", "/")
            vCompany = Mid(vCompany, InStr(Replace(company.SelectedItem.ToString, " : ", "/"), "/") + 1, Len(vCompany))
            vHeader += vCompany.ToString
            'End If
            Session("rptCheque_BB") = vHeader
        End Sub
    End Class
End Namespace

