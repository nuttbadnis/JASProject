Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Threading
Imports System.Web.UI

Partial Class pmtTaxSale
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow
    'Dim Log As New Cls_LogReport
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Session("email") = "weraphon.r"
        ' If Session("email") Is Nothing Then
        '     Response.Redirect("~/Chksession.aspx")
        '     ClientScript.RegisterStartupScript(Page.GetType, "open", "window.close()", True)
        '     'ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('default.aspx?','_blank');self.focus();", True)
        ' End If

        If Not Page.IsPostBack Then
            LoadCompany()
            LoadMonth()
            LoadYear()
            LoadArea()
            LoadProvince("")
            LoadBranch("")
            '////////// Cookie ///////
            Try

                cboCompany.SelectedValue = Request.Cookies("TaxSumary")("Company")
                LoadArea()
                cboSArea.SelectedValue = Request.Cookies("TaxSumary")("AreaFrom")
                cboEArea.SelectedValue = Request.Cookies("TaxSumary")("AreaTo")

                LoadProvince("Where")
                cboSProvince.SelectedValue = Request.Cookies("TaxSumary")("ProvinceFrom")
                cboEProvince.SelectedValue = Request.Cookies("TaxSumary")("ProvinceTo")

                LoadBranch("Where")
                cboSBranch.SelectedValue = Request.Cookies("TaxSumary")("BranchFrom")
                cboEBranch.SelectedValue = Request.Cookies("TaxSumary")("BranchTo")

                LoadYear()
                'cboYear.SelectedValue = Request.Cookies("TaxSumary")("Year")

                LoadMonth()
                'cboMonth.SelectedValue = Request.Cookies("TaxSumary")("Month")
         
            Catch ex As Exception
            
                ' Response.Cookies("TaxSumary").Expires = DateTime.MaxValue
                ' Response.Cookies("TaxSumary")("Login") = Session("email").ToString
                ' Response.Cookies("TaxSumary")("Company") = ""
                ' Response.Cookies("TaxSumary")("AreaFrom") = ""
                ' Response.Cookies("TaxSumary")("AreaTo") = ""
                ' Response.Cookies("TaxSumary")("ProvinceFrom") = ""
                ' Response.Cookies("TaxSumary")("ProvinceTo") = ""
                ' Response.Cookies("TaxSumary")("BranchFrom") = ""
                ' Response.Cookies("TaxSumary")("BranchTo") = ""
                ' Response.Cookies("TaxSumary")("Month") = ""
                ' Response.Cookies("TaxSumary")("Year") = ""
            End Try
            '////////////////////////
        End If
    End Sub
    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        C.SetDropDownList(cboCompany, vSql, "ComName", "ComCode")
    End Sub
    Private Sub LoadMonth()
        vSql = CF.rSqlDDMonth()
        C.SetDropDownList(cboMonth, vSql, "month", "f07","--กรุณาเลือกเดือน--")
    End Sub
    Private Sub LoadYear()
        vSql = CF.rSqlDDYear()
        C.SetDropDownList(cboYear, vSql, "year", "f06", "--กรุณาเลือกปี--")
    End Sub

    Private Sub LoadArea()
        vSql = CF.rSqlDDArea(Session("Uemail"),0)
        C.SetDropDownList(cboSArea, vSql, "AreaName", "AreaCode", "--ทั้งหมด--",cboEArea)
        If cboSArea.Items.Count = 1 Then
            vSql = CF.rSqlDDArea(Session("Uemail"),1)
            C.SetDropDownList(cboSArea, vSql, "AreaName", "AreaCode", "--ทั้งหมด--",cboEArea)
        End If
    End Sub

    Protected Sub cboSArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSArea.SelectedIndexChanged
        If cboSArea.SelectedIndex = 0 Then
            Response.write("0kkkkkkkkkkkkkkkk")
            LoadProvince("")
            LoadBranch("")
        Else
            LoadProvince("Where")
            LoadBranch("Where")
        End If
    End Sub

    Protected Sub cboEArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEArea.SelectedIndexChanged
        If cboEArea.SelectedIndex = 0 Then
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

    Protected Sub cboEProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEProvince.SelectedIndexChanged
        If cboEProvince.SelectedIndex = 0 Then
            LoadBranch("")
        Else
            LoadBranch("Where")
        End If
    End Sub

    Private Sub LoadProvince(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            If cboSArea.SelectedIndex <> 0 Then
                cboSProvince.Enabled = True
                cboEProvince.Enabled = True
                cboSBranch.Enabled = True
                cboEBranch.Enabled = True
                strW = "o.f03 >= '" & cboSArea.SelectedValue.ToString & "'"
            End If
            If cboEArea.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "o.f03 <= '" & cboEArea.SelectedValue.ToString & "'"
            End If
            Wherecause = strW
        End If

        vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),0,Wherecause)
        C.SetDropDownList(cboSProvince, vSql, "ProvinceName", "ProvinceCode", "--ทั้งหมด--", cboEProvince)
        ' ถ้าสำนักงานเป็น ALL
        If cboSProvince.Items.Count = 1 Then
            vSql = CF.rSqlDDProvince_DAT(Session("Uemail"),1,Wherecause)
            C.SetDropDownList(cboSProvince, vSql, "ProvinceName", "ProvinceCode", "--ทั้งหมด--", cboEProvince)
        End If

    End Sub

    Private Sub LoadBranch(ByVal Wherecause As String)
        Try
            If Wherecause <> "" Then
                Dim strW As String = ""
                If cboSArea.SelectedIndex <> 0 Then
                    strW = "o.f03 >= '" & cboSArea.SelectedValue.ToString & "'"
                End If
                If cboEArea.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "o.f03 <= '" & cboEArea.SelectedValue.ToString & "'"
                End If

                If cboSProvince.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "pv.f06 = '" & cboSProvince.SelectedValue.ToString & "'"
                    'strW = strW + "m02.f02 >= '" & cboSProvince.SelectedValue.ToString & "'"
                End If
                If cboEProvince.SelectedIndex <> 0 Then
                    If strW <> "" Then strW = strW + " and "
                    strW = strW + "pv.f06 = '" & cboEProvince.SelectedValue.ToString & "'"
                    'strW = strW + "m02.f02 <= '" & cboEProvince.SelectedValue.ToString & "'"
                End If
                Wherecause = strW
            End If


            ' vSql = "select distinct o.f02 as BranchCode, "
            ' vSql += "o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName "
            ' vSql += " from m00860 ma with(nolock), m00030 o with(nolock) , m02020 m02 with(nolock) " 
            ' vSql += " where ma.f03 = o.f02 and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') "
            ' vSql += "and ma.f02='" + Session("email") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
            vSql = CF.rSqlDDShop(Session("Uemail"),0,Wherecause)
            C.SetDropDownList(cboSBranch, vSql, "BranchName", "BranchCode", "--ทั้งหมด--", cboEBranch)
            If cboSBranch.Items.Count = 1 Then
                ' vSql = "select distinct o.f02 as BranchCode,o.f02 + ' :: ' + (case o.f04 when 'สำนักงานใหญ่' then o.f17 else o.f04 end ) as BranchName from m00860 ma with(nolock), m00030 o with(nolock) , m02020 m02 with(nolock) where ma.f03 = 'all' and   m02.f02 = o.f12  and (o.f20 is null or convert(varchar(10),o.f20,111)>='2008/05/01') and ma.f02='" + Session("email") + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by o.f02"
                vSql = CF.rSqlDDShop(Session("Uemail"),1,Wherecause)
                C.SetDropDownList(cboSBranch, vSql, "BranchName", "BranchCode", "--ทั้งหมด--", cboEBranch)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        ShowRpt()

    End Sub
    Private Sub ShowRpt()
        Dim strSelect As String = ""

        Dim vHeader As String = ""
        Dim vComName As String = cboCompany.SelectedItem.ToString
        Dim vMonth As String = cboMonth.SelectedItem.ToString
        Dim vYear As String = cboYear.SelectedItem.ToString
        Dim vSArea As String = cboSArea.SelectedIndex.ToString
        Dim vEArea As String = cboEArea.SelectedIndex.ToString
        Dim vSProvince As String = cboSProvince.SelectedItem.ToString
        Dim vEProvince As String = cboEProvince.SelectedItem.ToString
        Dim vSBranch As String = cboSBranch.SelectedItem.ToString
        Dim vEBranch As String = cboEBranch.SelectedItem.ToString


        Dim sb As New StringBuilder

        If chkParameter.Checked = True Then
            Response.Cookies("TaxSumary").Expires = DateTime.MaxValue
            Response.Cookies("TaxSumary")("Login") = Session("email").ToString
            Response.Cookies("TaxSumary")("Company") = cboCompany.SelectedValue
            Response.Cookies("TaxSumary")("AreaFrom") = cboSArea.SelectedValue
            Response.Cookies("TaxSumary")("AreaTo") = cboEArea.SelectedValue
            Response.Cookies("TaxSumary")("ProvinceFrom") = cboSProvince.SelectedValue
            Response.Cookies("TaxSumary")("ProvinceTo") = cboEProvince.SelectedValue
            Response.Cookies("TaxSumary")("BranchFrom") = cboSBranch.SelectedValue
            Response.Cookies("TaxSumary")("BranchTo") = cboEBranch.SelectedValue
            Response.Cookies("TaxSumary")("Month") = cboMonth.SelectedValue
            Response.Cookies("TaxSumary")("Year") = cboYear.SelectedValue
        Else
            Response.Cookies("TaxSumary").Expires = DateTime.MaxValue
            Response.Cookies("TaxSumary")("Login") = Session("email").ToString
            Response.Cookies("TaxSumary")("Company") = ""
            Response.Cookies("TaxSumary")("AreaFrom") = ""
            Response.Cookies("TaxSumary")("AreaTo") = ""
            Response.Cookies("TaxSumary")("ProvinceFrom") = ""
            Response.Cookies("TaxSumary")("ProvinceTo") = ""
            Response.Cookies("TaxSumary")("BranchFrom") = ""
            Response.Cookies("TaxSumary")("BranchTo") = ""
            Response.Cookies("TaxSumary")("Month") = ""
            Response.Cookies("TaxSumary")("Year") = ""
        End If

        Try
            Dim rDate As DateTime = Now()
            Dim Parameter As String = "Area F:" + cboSArea.SelectedValue.ToString + " T:" + cboEArea.SelectedValue.ToString + " Province F:" + cboSProvince.SelectedValue.ToString + " T:" + cboEProvince.SelectedValue.ToString + " Shop F:" + cboSBranch.SelectedValue.ToString + " T:" + cboEBranch.SelectedValue.ToString + " Year:" + cboYear.SelectedValue.ToString + " Month:" + cboMonth.SelectedValue.ToString
            Session("Parameter") = Parameter
            Session("rDate") = rDate.ToString("yyyy-MM-dd HH:mm:ss.000") ', New Globalization.CultureInfo("en-US"))

            sb.Remove(0, sb.Length)


            strSelect = " select a.f03 'RO', t.f02 'CodeShop',(case a.f04 when 'สำนักงานใหญ่' then a.f17 else a.f04 end ) 'ShopName', " + vbCr
            strSelect += " convert(varchar(10),t.F08,103) 'DocDate', t.F05 'DocNo', t.f21 'CusName', t.f20 'TaxId', " + vbCr
            strSelect += " t.f15 'AmountWithTax', t.f16 'Tax', t.f17 'TotalAmount' " + vbCr
            strSelect += " from r17010 t with(nolock),m00030 a with(nolock),m02020 d with(nolock) ,m00031 b with(nolock), " + vbCr
            strSelect += " (select ad.f01,ad.f07,ad.f09,ad.f10,ad.f11,ad.f12,p.f03 provname from m00032 ad with(nolock), m02020 p with(nolock) where ad.f03 = p.f02) c " + vbCr
            strSelect += " where(t.f02 = a.f02) " + vbCr
            strSelect += " and a.f02 = b.f02  " + vbCr
            strSelect += " and a.f12 = d.f02 " + vbCr
            strSelect += " and b.f04 =c.f01 and b.f03 ='17' and t.f36<>'A' "



            '-----------Company----------

            'If cboCompany.SelectedIndex <> 0 Then
            strSelect += " and t.f01 = '" + cboCompany.SelectedValue.ToString + "' "
            vComName = Replace(cboCompany.SelectedItem.ToString, " :: ", "/")
            vComName = Mid(vComName, InStr(Replace(cboCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
            vHeader = vComName
            'End If
            vHeader += "|"
            '----------- Month -----------
            If cboMonth.SelectedIndex <> 0 Then
                strSelect += " and t.f07 = '" + cboMonth.SelectedValue.ToString + "' "
                vMonth = Replace(cboMonth.SelectedItem.ToString, " :: ", "/")
                vMonth = Mid(vMonth, InStr(Replace(cboMonth.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vMonth))
                vHeader += " รายงานภาษีขาย ประจำเดือน" + " " + vMonth + " "
            End If
            '---------------Year----------------

            If cboYear.SelectedIndex <> 0 Then
                strSelect += "  and t.f06 = '" + cboYear.SelectedValue.ToString + "' "
                vYear = Replace(cboYear.SelectedItem.ToString, " :: ", "/")
                vYear = Mid(vYear, InStr(Replace(cboYear.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vYear))
                vHeader += "ปี" + " " + vYear
            End If

            vHeader += "|"
            '----------- Area -----------

            If Not (cboSArea.SelectedIndex = 0 Or cboSArea.Items.Count = 0) Then
                strSelect += "  and a.f03>='" + cboSArea.SelectedValue.ToString + "'"

                vSArea = Replace(cboSArea.SelectedItem.ToString, " :: ", "/")
                vSArea = Mid(vSArea, InStr(Replace(cboSArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSArea))
                vHeader += " จากเขต " + vSArea + "  "

            End If
            If Not (cboEArea.SelectedIndex = 0 Or cboEArea.Items.Count = 0) Then
                strSelect += "  and a.f03<='" + cboEArea.SelectedValue.ToString + "'"

                vEArea = Replace(cboEArea.SelectedItem.ToString, " :: ", "/")
                vEArea = Mid(vEArea, InStr(Replace(cboEArea.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEArea))
                vHeader += " ถึงเขต " + vEArea + "  "

            End If

            '----------- Province -----------

            If Not (cboSProvince.SelectedIndex = 0 Or cboSProvince.Items.Count = 0) Then
                strSelect += "  and d.f02>='" + cboSProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020

                vSProvince = Replace(cboSProvince.SelectedItem.ToString, " :: ", "/")
                vSProvince = Mid(vSProvince, InStr(Replace(cboSProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "จากจังหวัด" + vSProvince + "  "

            End If
            If Not (cboEProvince.SelectedIndex = 0 Or cboEProvince.Items.Count = 0) Then
                strSelect += "  and d.f02<='" + cboEProvince.SelectedValue.ToString + "' "   '-- Province select f02,f03 from m02020

                vEProvince = Replace(cboEProvince.SelectedItem.ToString, " :: ", "/")
                vEProvince = Mid(vEProvince, InStr(Replace(cboEProvince.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSProvince))
                vHeader += "ถึงจังหวัด" + vEProvince + "  "

            End If

            '----------- Branch -----------

            If Not (cboSBranch.SelectedIndex = 0 Or cboSBranch.Items.Count = 0) Then
                strSelect += "  and t.f02>='" + cboSBranch.SelectedValue.ToString + "'"  '-- Office select f02,04 from m00030

                vSBranch = Replace(cboSBranch.SelectedItem.ToString, " :: ", "/")
                vSBranch = Mid(vSBranch, InStr(Replace(cboSBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vSBranch))
                vHeader += " จากสาขา " + vSBranch + "  "

            End If
            If Not (cboEBranch.SelectedIndex = 0 Or cboEBranch.Items.Count = 0) Then
                strSelect += "  and t.f02<='" + cboEBranch.SelectedValue.ToString + "'" '-- Office select f02,04 from m00030

                vEBranch = Replace(cboEBranch.SelectedItem.ToString, " :: ", "/")
                vEBranch = Mid(vEBranch, InStr(Replace(cboEBranch.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vEBranch))
                vHeader += " ถึงสาขา " + vEBranch + "  "

            End If


            vHeader += "|"


            strSelect += "  order by a.f03, t.f02, t.F08, t.f05 "

            Session("StringQuery") = strSelect.ToString

            Session("Header") = vHeader

            ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('rptTaxSale.aspx?','_blank');self.focus();", True)

        Catch ex As Exception
            ClientScript.RegisterStartupScript(Page.GetType, "Err", "alert('" + ex.Message + "');", True)
        End Try
    End Sub

End Class
