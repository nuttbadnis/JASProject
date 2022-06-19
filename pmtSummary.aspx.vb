Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Net.IPAddress

Partial Class pmtSummary
    Inherits System.Web.UI.Page
    Dim strConn As String = WebConfigurationManager.ConnectionStrings("RMSDATConnectionString").ConnectionString.ToString
    Dim Log As New Cls_LogReport
    Dim C As New Cls_Data
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("email") = "kannikak"
        If Session("email") Is Nothing Then
            Response.Redirect("~/Chksession.aspx")
            ClientScript.RegisterStartupScript(Page.GetType, "open", "window.close()", True)

        End If
        If Not Page.IsPostBack Then
            Dim vDate As String
            Dim currThead As System.Globalization.CultureInfo
            currThead = Thread.CurrentThread.CurrentCulture
            Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            vDate = Format(Now, "dd/MM/yyyy")
            If Mid(vDate, 7, 4) > 2500 Then
                vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) - 543).ToString
            End If 'Default ให้แสดงวันที่ปัจจุบัน
            SDate.Text = vDate

            Thread.CurrentThread.CurrentCulture = currThead


            LoadArea()
            LoadProvince("")
            LoadBranch("")
            '////////// Cookie ///////
            Try

                LoadArea()
                cboSArea.SelectedValue = Request.Cookies("pmtSummary")("AreaFrom")

                LoadProvince("Where")
                cboSProvince.SelectedValue = Request.Cookies("pmtSummary")("ProvinceFrom")

                LoadBranch("Where")
                cboSBranch.SelectedValue = Request.Cookies("pmtSummary")("BranchFrom")


            Catch ex As Exception
                Response.Cookies("pmtSummary").Expires = DateTime.MaxValue
                Response.Cookies("pmtSummary")("Login") = Session("email").ToString
                Response.Cookies("pmtSummary")("AreaFrom") = ""
                Response.Cookies("pmtSummary")("ProvinceFrom") = ""
                Response.Cookies("pmtSummary")("BranchFrom") = ""
             

            End Try
            '////////////////////////
        Else

        End If
    End Sub
    Public Function GetDataTableRepweb(ByVal QryStr As String, Optional ByVal TableName As String = "DataTalble1") As DataTable
        Dim objDA As New SqlDataAdapter(QryStr, strConn)
        Dim objDT As New DataTable(TableName)
        Try
            objDA.Fill(objDT)
        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        End Try
        Return objDT
    End Function
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
    
    Public Function CDateText(ByVal TDate As String) As String
        Dim TDate2 As String
        Try
            TDate2 = CStr(TDate)
            If TDate = "  /  /    " Or TDate = "__/__/____" Or Trim(TDate) = "" Then
                CDateText = ""
               
            Else
                CDateText = Mid(TDate, 7, 4) + "/" + Mid(TDate, 4, 2) + "/" + Mid(TDate, 1, 2)
            End If
        Catch ex As Exception
            CDateText = TDate
        End Try
    End Function

    Private Sub LoadArea()
        Try

            Dim strSql As String = "select distinct o.f03 as AreaCode,mo.f03 as AreaName from m00860 ma, m00030 o, m02300 mo where o.f03=mo.f02 and ma.f03 = o.f02 and ma.f02='" + Session("email") + "' order by o.f03"
            SetDropDownList(cboSArea, strSql, "AreaName", "AreaCode", "")
            If cboSArea.Items.Count = 1 Then
                strSql = "select distinct o.f03 as AreaCode,mo.f03 as AreaName from m00860 ma, m00030 o, m02300 mo where o.f03=mo.f02 and ma.f03 = 'all' and ma.f02='" + Session("email") + "' order by o.f03"
                SetDropDownList(cboSArea, strSql, "AreaName", "AreaCode", "--ดูข้อมูลทั้งหมด--")
            End If
            EnableDropDown()
        Catch ex As Exception

        End Try

    End Sub


    Private Sub LoadProvince(ByVal Wherecause As String)
        Try
            Dim strSql As String
            Dim strW As String = ""
            If Wherecause <> "" Then
                If cboSArea.SelectedIndex <> 0 Then
                    strW = "o.f03 = '" & cboSArea.SelectedValue.ToString & "'"
                End If
              
                Wherecause = strW
            End If


            strSql = "select distinct m02.f06 as ProvinceCode,m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and m02.f02 = o.f12 and ma.f02='" + Session("email") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by m02.f06 "

            SetDropDownList(cboSProvince, strSql, "ProvinceName", "ProvinceCode", "")
            If cboSProvince.Items.Count = 1 Then
                strSql = "select distinct m02.f06 as ProvinceCode,m02.f03 as ProvinceName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and m02.f02 = o.f12 and ma.f02='" + Session("email") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by m02.f06 "
                SetDropDownList(cboSProvince, strSql, "ProvinceName", "ProvinceCode", "")
            End If
            EnableDropDown()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LoadBranch(ByVal Wherecause As String)
        Try
            Dim strSql As String
            If Wherecause <> "" Then
                Dim strW As String = ""
                If cboSArea.SelectedIndex <> 0 Then
                    strW = "o.f03 = '" & cboSArea.SelectedValue.ToString & "'"
                End If
              

                If cboSProvince.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "m02.f06 >= '" & cboSProvince.SelectedValue.ToString & "'"
                End If
               
                Wherecause = strW
            End If


            strSql = "select distinct o.f02 as BranchCode,o.f02+' : '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("email") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
            SetDropDownList(cboSBranch, strSql, "BranchName", "BranchCode", "")
            If cboSBranch.Items.Count = 1 Then
                strSql = "select distinct o.f02 as BranchCode,o.f02+' : '+(case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma, m00030 o , m02020 m02 where ma.f03 = 'all' and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("email") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
                SetDropDownList(cboSBranch, strSql, "BranchName", "BranchCode", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub cboSArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSArea.SelectedIndexChanged
        If cboSArea.SelectedIndex = 0 Then
            LoadProvince("")
            LoadBranch("")
        Else
            LoadProvince("Where")
            LoadBranch("Where")
        End If
    End Sub

    Protected Sub cboSProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSProvince.SelectedIndexChanged
        If cboSProvince.SelectedIndex = 0 Then
            LoadBranch("")
        Else
            LoadBranch("Where")
        End If
    End Sub

    Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        ShowData()
    End Sub

    Private Sub ShowData()

        If chkParameter.Checked = True Then

            Response.Cookies("pmtSummary")("Login") = Session("email").ToString
            Response.Cookies("pmtSummary")("AreaFrom") = cboSArea.SelectedValue
            Response.Cookies("pmtSummary")("ProvinceFrom") = cboSProvince.SelectedValue
            Response.Cookies("pmtSummary")("BranchFrom") = cboSBranch.SelectedValue
        Else
            Response.Cookies("pmtSummary").Expires = DateTime.MaxValue
            Response.Cookies("pmtSummary")("Login") = Session("email").ToString
            Response.Cookies("pmtSummary")("AreaFrom") = ""
            Response.Cookies("pmtSummary")("ProvinceFrom") = ""
            Response.Cookies("pmtSummary")("BranchFrom") = ""
        End If
        'ใช้ส่งค่าไปในหน้าที่จะแสดงรายงาน()
        Dim strSelect As String = ""
        Dim vOption As String = ""
        Dim vHeader As String = ""
        Dim vComName As String = ""
        Dim vAreaName As String = ""
        Dim SprovinceName As String = ""
        Dim EprovinceName As String = ""
        Dim SbranchName As String = ""
        Dim EbranchName As String = ""
        Dim sb As New StringBuilder


        Try

            sb.Remove(0, sb.Length)

          
       
            strSelect = "select '1.เงินสด' as 'Type',f17 'จำนวนเงินจากระบบ',f16 'จำนวนเงินนำส่ง'," + vbCr  'vbCr คือคำสั่งใช้ตัดบรรทัดหรือ enter ใน SQL
            strSelect += " (f17-f16) 'ผลต่าง',0 'จนนับ',0 'จนส่ง',0  'ผลต่างรายการ' from r16020 where f02='" + cboSBranch.SelectedItem.Value + "' and convert(varchar(10),f04,103)='" + SDate.Text + "' union" + vbCr

            strSelect += " select '2.บัตรเครดิต' as 'Type',f19 'จำนวนเงินจากระบบ',f18 'จำนวนเงินนำส่ง'," + vbCr
            strSelect += " (f19-f18) 'ผลต่าง',f21 'จนนับ',f20 'จนส่ง',(f21-f20) 'ผลต่างรายการ' from r16020 where f02='" + cboSBranch.SelectedItem.Value + "'  and convert(varchar(10),f04,103)='" + SDate.Text + "' union" + vbCr

            strSelect += " select '3.หักภาษี ณ ที่จ่าย' as 'Type',f31 'จำนวนเงินจากระบบ',f30 'จำนวนเงินนำส่ง'," + vbCr
            strSelect += " (f31-f30) 'ผลต่าง',f33 'จนนับ',f32 'จนส่ง',(f33-f32) 'ผลต่างรายการ' from r16020 where f02='" + cboSBranch.SelectedItem.Value + "'  and convert(varchar(10),f04,103)='" + SDate.Text + "' union" + vbCr

            strSelect += " select '4.เช็ค' as 'Type',f23 'จำนวนเงินจากระบบ',f22 'จำนวนเงินนำส่ง'," + vbCr
            strSelect += " (f23-f22) 'ผลต่าง',f25 'จนนับ',f24 'จนส่ง',(f25-f24) 'ผลต่างรายการ' from r16020 where f02='" + cboSBranch.SelectedItem.Value + "'  and convert(varchar(10),f04,103)='" + SDate.Text + "' union " + vbCr

            strSelect += " select '5.เงินโอน' as 'Type',f27 'จำนวนเงินจากระบบ',f26 'จำนวนเงินนำส่ง', " + vbCr
            strSelect += " (f27-f26) 'ผลต่าง',f29 'จนนับ',f28 'จนส่ง',(f29-f28) 'ผลต่างรายการ' from r16020  where f02='" + cboSBranch.SelectedItem.Value + "'  and convert(varchar(10),f04,103)='" + SDate.Text + "' union" + vbCr

            strSelect += " select '6.อื่นๆ' as 'Type',f36 'จำนวนเงินจากระบบ',f36 'จำนวนเงินนำส่ง'," + vbCr
            strSelect += " (f36-f36) 'ผลต่าง',0 'จนนับ',0 'จนส่ง',0 'ผลต่างรายการ' from r16020 where f02='" + cboSBranch.SelectedItem.Value + "'  and convert(varchar(10),f04,103)='" + SDate.Text + "'"
        


            sb.Append(strSelect)


            Dim rDate As DateTime = Now()
            'Dim vIPAddress As String = Request.ServerVariables("REMOTE_ADDR")
            Dim Parameter As String = "Area:" + cboSArea.SelectedValue.ToString + " Province:" + cboSProvince.SelectedValue.ToString + " Shop:" + cboSBranch.SelectedValue.ToString + " Date:" + Log.CDateText(SDate.Text)
            Session("Parameter") = Parameter
            Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))
            'Log.LogReport("21040", "รายงานสรุปการนำส่งสิ้นวัน", "Area:" + cboSArea.SelectedValue.ToString + " Province:" + cboSProvince.SelectedValue.ToString + " Shop:" + cboSBranch.SelectedValue.ToString + " Date:" + Log.CDateText(SDate.Text), vIPAddress, Session("email"))


            Session("StringQuery") = sb.ToString
            Session("Header") = "03"
            Session("SDate") = " ประจำวันที่ " + SDate.Text
            Session("Shop") = cboSBranch.SelectedItem.Value

            ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('showSummary.aspx','_blank');", True) 'self.focus();

        Catch ex As Exception
            ClientScript.RegisterStartupScript(Page.GetType, "Err", "alert('" + ex.Message + "');", True)
        End Try
    End Sub
    Private Sub EnableDropDown()
       
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ClientScript.RegisterStartupScript(Page.GetType, "Popup", "var win=window.showModalDialog('manual1.aspx?help=ShowRptSum','Modal1','dialogWidth:496px;dialogHeight:400px;');win.focus();", True)
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
        ClientScript.RegisterStartupScript(Page.GetType, "open", "var win=window.open('ShowManual.aspx?filename=Report1009.doc','_blank');win.focus();", True)
    End Sub
End Class


