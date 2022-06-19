Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Web.UI
Imports System.Threading
Imports System.Net.IPAddress

Partial Class SpacialPayin_search_productprice
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Dim CF As New Cls_RequestFlow
    Dim vSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            LoadCompany()
            LoadCatagory()
            LoadGroup()
            LoadSubGroup("")
            LoadItem("")
            '////////// Cookie ///////
            Try
                ddlCompany.SelectedValue = Request.Cookies("ProductPrice")("Company")
                LoadCatagory()
                SCatagory.SelectedValue = Request.Cookies("ProductPrice")("CatagoryFrom")
                ECatagory.SelectedValue = Request.Cookies("ProductPrice")("CatagoryTo")
                LoadGroup()
                SGroup.SelectedValue = Request.Cookies("ProductPrice")("GroupFrom")
                EGroup.SelectedValue = Request.Cookies("ProductPrice")("GroupTo")
                LoadSubGroup("Where")
                SsubGroup.SelectedValue = Request.Cookies("ProductPrice")("SubGroupFrom")
                EsubGroup.SelectedValue = Request.Cookies("ProductPrice")("SubGroupTo")
                LoadItem("Where")
                SProduct.SelectedValue = Request.Cookies("ProductPrice")("stockFrom")
                EProduct.SelectedValue = Request.Cookies("ProductPrice")("stockTo")
            Catch ex As Exception
                Response.Cookies("ProductPrice").Expires = DateTime.MaxValue
                Response.Cookies("ProductPrice")("Login") = Session("Uemail").ToString
                Response.Cookies("ProductPrice")("Company") = ""
                Response.Cookies("ProductPrice")("CatagoryFrom") = ""
                Response.Cookies("ProductPrice")("CatagoryTo") = ""
                Response.Cookies("ProductPrice")("GroupFrom") = ""
                Response.Cookies("ProductPrice")("GroupTo") = ""
                Response.Cookies("ProductPrice")("SubGroupFrom") = ""
                Response.Cookies("ProductPrice")("SubGroupTo") = ""
                Response.Cookies("ProductPrice")("stockFrom") = ""
                Response.Cookies("ProductPrice")("stockTo") = ""

            End Try
            '////////////////////////
        End If
    End Sub
    Private Sub LoadCompany()
        vSql = CF.rSqlDDCompany()
        C.SetDropDownList(ddlCompany, vSql, "ComName", "ComCode")
    End Sub

    Private Sub LoadCatagory()
        vSql = "select distinct f02 as CatagoryCode,f02+' :: '+f03 as CatagoryName from m00160 where f01='" + ddlCompany.SelectedValue + "'"
        C.SetDropDownList(SCatagory, vSql, "CatagoryName", "CatagoryCode", "--กรุณาเลือกประเภทสินค้า--",ECatagory)
    End Sub
    Private Sub LoadGroup()
        vSql  = "select distinct f02 as GroupCode ,f02+ ' :: '+f03 as GroupName from m00170 where f01='" + ddlCompany.SelectedValue + "' order by f02"
        C.SetDropDownList(SGroup, vSql, "GroupName", "GroupCode", "--กรุณาเลือกกลุ่มหลัก--",EGroup)
    End Sub
    Private Sub LoadSubGroup(ByVal Wherecause As String)
        Dim strW As String = ""
        If Wherecause <> "" Then
            If SGroup.SelectedIndex <> 0 Then
                strW = "f02 >= '" & SGroup.SelectedValue.ToString & "'"
            End If
            If EGroup.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "f02 <= '" & EGroup.SelectedValue.ToString & "'"
            End If
            Wherecause = strW
        End If
        vSql  = "select distinct f03 as subGroupCode,f03+' :: '+ f04 as subGroupName from m00180 where f01='" + ddlCompany.SelectedValue + "'" + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by f03"
        C.SetDropDownList(SsubGroup, vSql, "subGroupName", "subGroupCode", "--กรุณาเลือกกลุ่มย่อย--", EsubGroup)
    End Sub
    Private Sub LoadItem(ByVal Wherecause As String)
        If Wherecause <> "" Then
            Dim strW As String = ""
            If SCatagory.SelectedIndex <> 0 Then
                strW = "f07 >= '" & SCatagory.SelectedValue.ToString & "'"
            End If
            If ECatagory.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "f07 <= '" & ECatagory.SelectedValue.ToString & "'"
            End If

            If SGroup.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "f08 >= '" & SGroup.SelectedValue.ToString & "'"
            End If
            If EGroup.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "f08 <= '" & EGroup.SelectedValue.ToString & "'"
            End If

            If SsubGroup.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "f09 >= '" & SsubGroup.SelectedValue.ToString & "'"
            End If
            If EsubGroup.SelectedIndex <> 0 Then
                If strW <> "" Then strW = strW + " and "
                strW = strW + "f09 <= '" & EsubGroup.SelectedValue.ToString & "'"
            End If
            Wherecause = strW
        End If
            vSql = "select f02 as ItemCode,f02+' :: '+ f05 as ItemName from m00210 where f01='" + ddlCompany.SelectedValue + "' " + IIf(Wherecause <> "", "and " + Wherecause, "") + " order by f02"
            C.SetDropDownList(SProduct, vSql, "ItemName", "ItemCode", "--กรุณาเลือกรหัสสินค้า--", EProduct)
    End Sub

    Protected Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        LoadCatagory()
        LoadGroup()
        LoadSubGroup("Where")
        LoadItem("Where")
    End Sub
    Protected Sub SCatagory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SCatagory.SelectedIndexChanged
        LoadItem("Where")
    End Sub
    Protected Sub ECatagory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ECatagory.SelectedIndexChanged
        LoadItem("Where")
    End Sub
    Protected Sub SGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SGroup.SelectedIndexChanged
        LoadSubGroup("Where")
        LoadItem("Where")
    End Sub
    Protected Sub EGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EGroup.SelectedIndexChanged
        LoadSubGroup("Where")
        LoadItem("Where")
    End Sub
    Protected Sub SsubGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SsubGroup.SelectedIndexChanged
        LoadItem("Where")
    End Sub
    Protected Sub EsubGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EsubGroup.SelectedIndexChanged
        LoadItem("Where")
    End Sub

    Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
        If chkParameter.Checked = True Then
            Response.Cookies("ProductPrice").Expires = DateTime.MaxValue
            Response.Cookies("ProductPrice")("Login") = Session("Uemail").ToString
            Response.Cookies("ProductPrice")("Company") = ddlCompany.SelectedValue
            Response.Cookies("ProductPrice")("CatagoryFrom") = SCatagory.SelectedValue
            Response.Cookies("ProductPrice")("CatagoryTo") = ECatagory.SelectedValue
            Response.Cookies("ProductPrice")("GroupFrom") = SGroup.SelectedValue
            Response.Cookies("ProductPrice")("GroupTo") = EGroup.SelectedValue
            Response.Cookies("ProductPrice")("SubGroupFrom") = SsubGroup.SelectedValue
            Response.Cookies("ProductPrice")("SubGroupTo") = EsubGroup.SelectedValue
            Response.Cookies("ProductPrice")("stockFrom") = SProduct.SelectedValue
            Response.Cookies("ProductPrice")("stockTo") = EProduct.SelectedValue
        Else
            Response.Cookies("ProductPrice").Expires = DateTime.MaxValue
            Response.Cookies("ProductPrice")("Login") = Session("Uemail").ToString
            Response.Cookies("ProductPrice")("Company") = ""
            Response.Cookies("ProductPrice")("CatagoryFrom") = ""
            Response.Cookies("ProductPrice")("CatagoryTo") = ""
            Response.Cookies("ProductPrice")("GroupFrom") = ""
            Response.Cookies("ProductPrice")("GroupTo") = ""
            Response.Cookies("ProductPrice")("SubGroupFrom") = ""
            Response.Cookies("ProductPrice")("SubGroupTo") = ""
            Response.Cookies("ProductPrice")("stockFrom") = ""
            Response.Cookies("ProductPrice")("stockTo") = ""
        End If
        'If ddlCompany.SelectedIndex = 0 Then
        '    ClientScript.RegisterStartupScript(Page.GetType, "", "alert('กรุณาระบุบริษัท!');", True)
        'Else
        ShowReport()
        'End If
    End Sub
    Private Sub ShowRpt()
        Dim vHeader As String = "รายการราคาสินค้า"
        Dim vComName = Replace(ddlCompany.SelectedItem.ToString, " :: ", "/")
        vComName = Mid(vComName, InStr(Replace(ddlCompany.SelectedItem.ToString, " :: ", "/"), "/") + 1, Len(vComName))
        vHeader += "|" + vComName + "|"
      
        If SCatagory.SelectedIndex > 0 Then
            vHeader += " จากประเภทสินค้า " + SCatagory.SelectedValue.ToString + " "
        Else
            vHeader += " จากประเภทสินค้าทั้งหมด "
        End If
        If ECatagory.SelectedIndex > 0 Then
            vHeader += " ถึงประเภทสินค้า " + ECatagory.SelectedValue.ToString + " "
        Else
            vHeader += " ถึงประเภทสินค้าทั้งหมด "
        End If
        If SGroup.SelectedIndex > 0 Then
            vHeader += " จากกลุ่มหลัก " + SGroup.SelectedValue.ToString + " "
        Else
            vHeader += " จากกลุ่มหลักทั้งหมด "
        End If
        If EGroup.SelectedIndex > 0 Then
            vHeader += " ถึงกลุ่มหลัก " + EGroup.SelectedValue.ToString + " "
        Else
            vHeader += " ถึงกลุ่มหลัก ทั้งหมด "
        End If
        If SsubGroup.SelectedIndex > 0 Then
            vHeader += " จากกลุ่มย่อย " + SsubGroup.SelectedValue.ToString + " "
        Else
            vHeader += " จากกลุ่มย่อยทั้งหมด "
        End If
        If EsubGroup.SelectedIndex > 0 Then
            vHeader += " ถึงกลุ่มย่อย " + EsubGroup.SelectedValue.ToString
        Else
            vHeader += " ถึงกลุ่มย่อยทั้งหมด"
        End If
        vHeader += "|"
        
        If SProduct.SelectedIndex > 0 Then
            vHeader += " จากรหัสสินค้า " + SProduct.SelectedValue.ToString + " "
        Else
            vHeader += " จากรหัสสินค้าทั้งหมด "
        End If
        If EProduct.SelectedIndex > 0 Then
            vHeader += " ถึงรหัสสินค้า " + EProduct.SelectedValue.ToString
        Else
            vHeader += " ถึงรหัสสินค้าทั้งหมด"
        End If
        Session("Header") = vHeader.ToString
    End Sub
    Private Sub ShowReport()
        
        Dim url As String
        ShowRpt()
        url = "ShowSearch.aspx?company=" & ddlCompany.SelectedValue _
        & "&SCatagory=" & IIf(SCatagory.SelectedIndex <> 0, SCatagory.SelectedValue, "00") _
        & "&ECatagory=" & IIf(ECatagory.SelectedIndex <> 0, ECatagory.SelectedValue, "ZZ") _
        & "&SGroup=" & IIf(SGroup.SelectedIndex <> 0, SGroup.SelectedValue, "00") _
        & "&EGroup=" & IIf(EGroup.SelectedIndex <> 0, EGroup.SelectedValue, "ZZ") _
        & "&SsubGroup=" & IIf(SsubGroup.SelectedIndex <> 0, SsubGroup.SelectedValue, "00") _
        & "&EsubGroup=" & IIf(EsubGroup.SelectedIndex <> 0, EsubGroup.SelectedValue, "ZZ") _
        & "&SProduct=" & IIf(SProduct.SelectedIndex <> 0, SProduct.SelectedValue, "00") _
        & "&EProduct=" & IIf(EProduct.SelectedIndex <> 0, EProduct.SelectedValue, "ZZ")
        ClientScript.RegisterStartupScript(Page.GetType, "open", "window.open('" + url + "','_blank','height=300,width=620,left=130,top=150,resizable=1,scrollbars=1');", True)
    End Sub
End Class
