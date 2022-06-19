Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Partial Class rptTransferDetail
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        showHeaderLbl()
        showData()
    End Sub

    Function showHeaderLbl() As Integer
        Dim vDate As String
        vDate = Format(Date.Now, "dd/MM/yyyy")
        If Mid(vDate, 7, 4) > 2500 Then
            vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) - 543).ToString
        End If
        LblHeader.Text = "ข้อมูลที่ Server ณ. วันที่ " + vDate + " เวลา " + Format(Now, "HH:mm")

        Dim mFromDate As String = Request.QueryString("mFromDate")
        Dim mToDate As String = Request.QueryString("mToDate")
        Dim mArea As String = Request.QueryString("mArea")
        Dim mProv As String = Request.QueryString("mProv")
        Dim mFromBranch As String = Request.QueryString("mFromBranch")
        Dim mToBranch As String = Request.QueryString("mToBranch")
        Dim mFromGroup As String = Request.QueryString("mFromGroup")
        Dim mFromSubGroup As String = Request.QueryString("mFromSubGroup")
        Dim mFromItem As String = Request.QueryString("mFromItem")
        Dim mToItem As String = Request.QueryString("mToItem")
        Dim mStatus As String = Request.QueryString("mStatus")
        Dim mLogistic As Boolean = Request.QueryString("mLogistic")

        'lblHeader1 text of branch
        If (mFromBranch = "ALL") Then
            mFromBranch = "ทั้งหมด"
        End If
        If (mToBranch = "ALL") Then
            mToBranch = "ทั้งหมด"
        End If
        report_param.Text = "รายงานการโอนสินค้าระหว่างสำนักงาน" + IIf(mLogistic = True, " Logistic", "")
        branch_param.Text = "จากสำนักงาน " + mFromBranch + " ถึงสำนักงาน " + mToBranch
        'lblHeader2 text of item
        If (mFromGroup = "ALL") Then
            mFromGroup = "ทั้งหมด"
        End If
        If (mFromSubGroup = "ALL") Then
            mFromSubGroup = "ทั้งหมด"
        End If
        If (mFromItem = "ALL") Then
            mFromItem = "ทั้งหมด"
        End If
        If (mToItem = "ALL") Then
            mToItem = "ทั้งหมด"
        End If
        group_param.Text = "เฉพาะสินค้าในกลุ่มหลัก " + mFromGroup
        subgroup_param.Text = "กลุ่มย่อย " + mFromSubGroup
        product_param.Text = "จากสินค้า " + mFromItem + " ถึงสินค้า " + mToItem
        'lblHeader3 text of date
        date_param.Text = "ที่โอนออกตั้งแต่วันที่ " + mFromDate + " ถึงวันที่ " + mToDate

    End Function

    Function showData() As Integer

        Dim mFromDate As String = Request.QueryString("mFromDate")
        Dim mToDate As String = Request.QueryString("mToDate")
        Dim mArea As String = Request.QueryString("mArea")
        Dim mProv As String = Request.QueryString("mProv")
        Dim mFromBranch As String = Request.QueryString("mFromBranch")
        Dim mToBranch As String = Request.QueryString("mToBranch")
        Dim mFromBranch2 As String = Request.QueryString("mFromBranch2")
        Dim mToBranch2 As String = Request.QueryString("mToBranch2")
        Dim mFromGroup As String = Request.QueryString("mFromGroup")
        Dim mFromSubGroup As String = Request.QueryString("mFromSubGroup")
        Dim mFromItem As String = Request.QueryString("mFromItem")
        Dim mToItem As String = Request.QueryString("mToItem")
        Dim mStatus As String = Request.QueryString("mStatus")
        Dim mLogistic As Boolean = Request.QueryString("mLogistic")

        Dim strQuery As String
        strQuery = "select main.f01 as company, main.f02 as shop, main.f05 as doc, cast(main.f06 as date) as doc_date  " + vbCrLf
        strQuery = strQuery + ", case main.f16 when '1' then 'รอรับ' when '3' then 'รับเข้าแล้ว' else 'ยกเลิกเอกสาร' end doc_status " + vbCrLf
        strQuery = strQuery + ", main.f25 Destination_shop " + vbCrLf
        strQuery = strQuery + ", detail.f08 as item, detail.f10 item_name, detail.f12 item_group, detail.f13 item_subgroup " + vbCrLf
        strQuery = strQuery + ", convert(int,detail.f18) item_quantity, detail.f17 item_unit, detail.f06 sequence /*, detail.f23 detail_status*/   " + vbCrLf
        strQuery = strQuery + ", main.f11 warehouse " + vbCrLf
        strQuery = strQuery + "from m00030 shop with(nolock) " + vbCrLf
        strQuery = strQuery + "join m02020 province with(nolock) " + vbCrLf
        strQuery = strQuery + "on shop.f12 = province.f02 " + vbCrLf
        If (mArea <> "ALL") Then
            strQuery = strQuery + "and shop.f03 = '" + mArea + "' " + vbCrLf
        End If
        If (mProv <> "ALL") Then
            strQuery = strQuery + "and province.f06 = '" + mProv + "' " + vbCrLf
        End If
        If (mFromBranch <> "ALL") Then
            strQuery = strQuery + "and shop.f02 >= '" + mFromBranch + "' " + vbCrLf
        End If
        If (mToBranch <> "ALL") Then
            strQuery = strQuery + "and shop.f02 <= '" + mToBranch + "' " + vbCrLf
        End If
        strQuery = strQuery + "join r01100 main with(nolock) " + vbCrLf
        strQuery = strQuery + "on shop.f02 = main.f02 " + vbCrLf
        strQuery = strQuery + "join r01101 detail with(nolock) " + vbCrLf
        strQuery = strQuery + "on main.f01 = detail.f01 and main.f02 = detail.f02 and main.f04 = detail.f04 " + vbCrLf
        strQuery = strQuery + "and main.f05 = detail.f05 " + vbCrLf
		strQuery = strQuery + "join m00030 shop2 with(nolock) " + vbCrLf
		strQuery = strQuery + "on main.f25 = shop2.f02 " + vbCrLf
        strQuery = strQuery + "where detail.f23 = '1' " + vbCrLf
        ' strQuery = strQuery + "and main.f01 = '05' " + vbCrLf
        strQuery = strQuery + "and main.f06 >= '" + mFromDate + "' " + vbCrLf
        strQuery = strQuery + "and main.f06 <= '" + mToDate + "' " + vbCrLf
        If (mFromGroup <> "ALL") Then
            strQuery = strQuery + "and detail.f12 = '" + mFromGroup + "' " + vbCrLf
        End If
        If (mFromSubGroup <> "ALL") Then
            strQuery = strQuery + "and detail.f13 = '" + mFromSubGroup + "' " + vbCrLf
        End If
        'status of document
        If (mStatus = "open") Then
            strQuery = strQuery + "and main.f16 = '1' " + vbCrLf
        ElseIf (mStatus = "close") Then
            strQuery = strQuery + "and main.f16 = '3' " + vbCrLf
        ElseIf (mStatus = "all") Then

        End If
        ' Destination_shop
        If (mFromBranch2 <> "ALL") Then
            strQuery = strQuery + "and main.f25 >= '" + mFromBranch2 + "' " + vbCrLf
        End If
        If (mToBranch2 <> "ALL") Then
            strQuery = strQuery + "and main.f25 <= '" + mToBranch2 + "' " + vbCrLf
        End If

        ' logistic checkbox
        If (mLogistic = True) Then
            strQuery = strQuery + "and (shop.f10 = 'L' or shop2.f10 = 'L') " + vbCrLf
        End If

        strQuery = strQuery + "order by company,shop,doc_date,doc,item " + vbCrLf

        Dim DT As DataTable
        DT = C.GetDataTable(strQuery)

        gvwData.DataSource = DT
        gvwData.DataBind()

        If (DT.Rows.Count = 0) Then
            NoQuery.Visible = True
        End If

    End Function

    Protected Sub gvwData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwData.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim link1 As HyperLink
            link1 = CType(e.Row.Cells(12).FindControl("HyperLink1"), HyperLink)
            link1.NavigateUrl = "~/rptTransferDetailSN.aspx?doc=" & e.Row.Cells(4).Text & "&seq=" & CType(e.Row.Cells(12).FindControl("lblSeq"), Label).Text & "&item=" & e.Row.Cells(8).Text
        End If
    End Sub

End Class
