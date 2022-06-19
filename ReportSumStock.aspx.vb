Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Threading
Imports System.web
Imports System.Globalization.CultureInfo
Partial Class ReportSumStock
    Inherits System.Web.UI.Page
    Dim C As New Cls_Data
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ShowHeader()
        Dim pCompany As String = Request.QueryString("com")
        Dim FromArea As String = Request.QueryString("sarea")
        Dim FromProvince As String = Request.QueryString("sprovince")
        Dim FromBranch As String = Request.QueryString("sbranch")
        Dim ToBranch As String = Request.QueryString("ebranch")
        Dim FromGroup As String = Request.QueryString("smain")
        Dim ToGroup As String = Request.QueryString("emain")
        Dim FromSubGroup As String = Request.QueryString("ssub")
        Dim ToSubGroup As String = Request.QueryString("esub")
        Dim FromProduct As String = Request.QueryString("sproduct")
        Dim ToProduct As String = Request.QueryString("eproduct")
        Dim DT As New DataTable
        Dim strSql As String
        Dim strSqlP As String
        Dim DTP As New DataTable
        Dim DTB As New DataTable
        Dim strSqlB As String



        If Request.QueryString("option") = "a" Then
            strSql = "select o.f03 'Area',s.f04 'ItemCode', i.f05 'ItemName'"
            strSql += " ,sum(case s.f10 when 'FG' then s.f18 else 0 end) 'FG'"
            strSql += " ,sum(case s.f10 when 'NG' then s.f18 else 0 end) 'NG'"
            strSql += " ,sum(case s.f10 when 'OG' then s.f18 else 0 end) 'OG'"
            strSql += " ,sum(case s.f10 when 'RG' then s.f18 else 0 end) 'RG'"
            strSql += " ,sum(case s.f10 when 'TS' then s.f18 else 0 end) 'TS'"
            strSql += " ,sum(case s.f10 when 'WJ' then s.f18 else 0 end) 'WJ'"
            strSql += " ,sum(case s.f10 when 'WP' then s.f18 else 0 end) 'WP'"
            strSql += " from r01170 s, m00030 o, m02020 p, m00210 i"
            strSql += " where i.f03 = s.f04 and s.f02 = o.f02 and o.f12 = p.f02 and s.f18 <> 0  and convert(varchar(10),isnull(o.f20,getdate()),23) >=  convert(varchar(10),getdate(),23)"
            strSql += " and s.f01 ='" + pCompany + "'"
            If FromArea <> "00" Then
                strSql += " and o.f03 ='" + FromArea + "'"
            End If
            'If FromProvince <> "00" Then
            '    strSql += " and p.f02 ='" + FromProvince + "'"
            'End If
            'If FromBranch <> "00" Then
            '    strSql += " and o.f02 >='" + FromBranch + "'"
            'End If
            'If ToBranch <> "ZZ" Then
            '    strSql += " and o.f02 <='" + ToBranch + "'"
            'End If
            If FromGroup <> "00" Then
                strSql += " and i.f08 >= '" + FromGroup + "'"
            End If
            If ToGroup <> "ZZ" Then
                strSql += " and i.f08 <= '" + ToGroup + "'"
            End If
            If FromSubGroup <> "00" Then
                strSql += " and i.f09 >='" + FromSubGroup + "'"
            End If
            If ToSubGroup <> "ZZ" Then
                strSql += "and i.f09 <='" + ToSubGroup + "'"
            End If
            If FromProduct <> "00" Then
                strSql += " and i.f02 >='" + FromProduct + "'"
            End If
            If ToProduct <> "ZZ" Then
                strSql += "and i.f02 <='" + ToProduct + "'"
            End If
            strSql += " group by o.f03,s.f04,i.f05"
            strSql += " having sum(s.f18) <> 0"
            strSql += " order by o.f03,s.f04"
            DT = C.GetDataTableRepweb(strSql)
            Me.GridView1.Columns(1).Visible = False
            Me.GridView1.Columns(2).Visible = False
            Me.GridView1.Columns(5).Visible = False
            Me.GridView1.Columns(13).Visible = False
            Me.GridView1.DataSource = DT
            Me.GridView1.DataBind()
            If DT.Rows.Count = 0 Then
                lblNodata.Visible = True
                detail_serial.Visible = False
            End If


        ElseIf Request.QueryString("option") = "p" Then
            strSqlP = "select o.f03 'Area',p.f03 'Province',s.f04 'ItemCode', i.f05 'ItemName'"
            strSqlP += " ,sum(case s.f10 when 'FG' then s.f18 else 0 end) 'FG'"
            strSqlP += " ,sum(case s.f10 when 'NG' then s.f18 else 0 end) 'NG'"
            strSqlP += " ,sum(case s.f10 when 'OG' then s.f18 else 0 end) 'OG'"
            strSqlP += " ,sum(case s.f10 when 'RG' then s.f18 else 0 end) 'RG'"
            strSqlP += " ,sum(case s.f10 when 'TS' then s.f18 else 0 end) 'TS'"
            strSqlP += " ,sum(case s.f10 when 'WJ' then s.f18 else 0 end) 'WJ'"
            strSqlP += " ,sum(case s.f10 when 'WP' then s.f18 else 0 end) 'WP'"
            strSqlP += " from r01170 s, m00030 o, m02020 p, m00210 i"
            strSqlP += " where i.f03 = s.f04 and s.f02 = o.f02 and o.f12 = p.f02 and s.f18 <> 0  and convert(varchar(10),isnull(o.f20,getdate()),23) >=  convert(varchar(10),getdate(),23)"
            strSqlP += " and s.f01 ='" + pCompany + "'"
            If FromArea <> "00" Then
                strSqlP += " and o.f03 ='" + FromArea + "'"
            End If
            If FromProvince <> "00" Then
                strSqlP += " and p.f06 ='" + FromProvince + "'"
            End If
            'If FromBranch <> "00" Then
            '    strSqlP += " and o.f02 >='" + FromBranch + "'"
            'End If
            'If ToBranch <> "ZZ" Then
            '    strSqlP += " and o.f02 <='" + ToBranch + "'"
            'End If
            If FromGroup <> "00" Then
                strSqlP += " and i.f08 >= '" + FromGroup + "'"
            End If
            If ToGroup <> "ZZ" Then
                strSqlP += " and i.f08 <= '" + ToGroup + "'"
            End If
            If FromSubGroup <> "00" Then
                strSqlP += " and i.f09 >='" + FromSubGroup + "'"
            End If
            If ToSubGroup <> "ZZ" Then
                strSqlP += "and i.f09 <='" + ToSubGroup + "'"
            End If
            If FromProduct <> "00" Then
                strSqlP += " and i.f02 >='" + FromProduct + "'"
            End If
            If ToProduct <> "ZZ" Then
                strSqlP += "and i.f02 <='" + ToProduct + "'"
            End If
            strSqlP += " group by o.f03,p.f03,s.f04,i.f05"
            strSqlP += " having sum(s.f18) <> 0"
            strSqlP += " order by o.f03,p.f03,s.f04"

            DTP = C.GetDataTableRepweb(strSqlP)
            Me.GridView1.Columns(2).Visible = False
            Me.GridView1.Columns(5).Visible = False
            Me.GridView1.Columns(13).Visible = False
            Me.GridView1.DataSource = DTP
            Me.GridView1.DataBind()
            If DTP.Rows.Count = 0 Then
                lblNodata.Visible = True
                detail_serial.Visible = False
            End If
        Else
            strSqlB = "select o.f03 'Area',p.f03 'Province',s.f02 'ShopCode',s.f04 'ItemCode', i.f05 'ItemName',s.f09 'WH'"
            strSqlB += " ,sum(case s.f10 when 'FG' then s.f18 else 0 end) 'FG'"
            strSqlB += " ,sum(case s.f10 when 'NG' then s.f18 else 0 end) 'NG'"
            strSqlB += " ,sum(case s.f10 when 'OG' then s.f18 else 0 end) 'OG'"
            strSqlB += " ,sum(case s.f10 when 'RG' then s.f18 else 0 end) 'RG'"
            strSqlB += " ,sum(case s.f10 when 'TS' then s.f18 else 0 end) 'TS'"
            strSqlB += " ,sum(case s.f10 when 'WJ' then s.f18 else 0 end) 'WJ'"
            strSqlB += " ,sum(case s.f10 when 'WP' then s.f18 else 0 end) 'WP'"
            strSqlB += " from r01170 s, m00030 o, m02020 p, m00210 i"
            strSqlB += " where i.f03 = s.f04 and s.f02 = o.f02 and o.f12 = p.f02 and s.f18 <> 0  and convert(varchar(10),isnull(o.f20,getdate()),23) >=  convert(varchar(10),getdate(),23)"
            strSqlB += " and s.f01 ='" + pCompany + "'"
            If FromArea <> "00" Then
                strSqlB += " and o.f03 ='" + FromArea + "'"
            End If
            If FromProvince <> "00" Then
                strSqlB += " and p.f06 ='" + FromProvince + "'"
            End If
            If FromBranch <> "00" Then
                strSqlB += " and o.f02 >='" + FromBranch + "'"
            End If
            If ToBranch <> "ZZ" Then
                strSqlB += " and o.f02 <='" + ToBranch + "'"
            End If
            If FromGroup <> "00" Then
                strSqlB += " and i.f08 >= '" + FromGroup + "'"
            End If
            If ToGroup <> "ZZ" Then
                strSqlB += " and i.f08 <= '" + ToGroup + "'"
            End If
            If FromSubGroup <> "00" Then
                strSqlB += " and i.f09 >='" + FromSubGroup + "'"
            End If
            If ToSubGroup <> "ZZ" Then
                strSqlB += "and i.f09 <='" + ToSubGroup + "'"
            End If
            If FromProduct <> "00" Then
                strSqlB += " and i.f02 >='" + FromProduct + "'"
            End If
            If ToProduct <> "ZZ" Then
                strSqlB += "and i.f02 <='" + ToProduct + "'"
            End If
            strSqlB += " group by o.f03,p.f03,s.f02,s.f04,i.f05,s.f09"
            strSqlB += " having sum(s.f18) <> 0"
            strSqlB += " order by o.f03,p.f03,s.f02,s.f04,s.f09"

            DTB = C.GetDataTableRepweb(strSqlB)
            Me.GridView1.DataSource = DTB
            Me.GridView1.DataBind()
            If DTB.Rows.Count = 0 Then
                lblNodata.Visible = True
                detail_serial.Visible = False
            End If
        End If

        'If Request.QueryString("exp") IsNot Nothing Then
        '    If Request.QueryString("exp") = "true" Then
        '        '------------ export excel ----------------
        '        PrepareGridViewForExport(Me.GridView1)
        '        Response.Clear()
        '        Response.BufferOutput = True
        '        Response.Charset = "windows-874"
        '        Response.ContentEncoding = Encoding.UTF8
        '        Response.AddHeader("content-disposition", "attachment; filename=SumStock.xls")
        '        Response.ContentType = "application/vnd.ms-excel"
        '        Dim tw As New System.IO.StringWriter()
        '        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        '        Dim frm As HtmlForm = New HtmlForm()
        '        'Me.Controls.Add(frm)
        '        'frm.Controls.Add(show_report) '//<---------write div
        '        'frm.Attributes("runat") = "server"
        '        'frm.RenderControl(hw)
        '        show_report.RenderControl(hw)
        '        Me.EnableViewState = False
        '        Response.Write(tw.ToString())
        '        Response.End()
        '        '------------ export excel ----------------
        '        show_report.Visible = False
        '    End If
        'End If


    End Sub
    Private sumFG As Decimal = 0.0
    Private sumNG As Decimal = 0.0
    Private sumOG As Decimal = 0.0
    Private sumRG As Decimal = 0.0
    Private sumTS As Decimal = 0.0
    Private sumWJ As Decimal = 0.0
    Private sumWP As Decimal = 0.0


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Request.QueryString("option") <> "a" And Request.QueryString("option") <> "p" Then
                Dim link1 As HyperLink
                link1 = CType(e.Row.Cells(13).FindControl("HyperLink1"), HyperLink)
                link1.NavigateUrl = "~/ReportSumStockSN.aspx?shop=" & e.Row.Cells(2).Text & "&item=" & e.Row.Cells(3).Text
            End If


            sumFG += CDec(DataBinder.Eval(e.Row.DataItem, "FG"))
            sumNG += CDec(DataBinder.Eval(e.Row.DataItem, "NG"))
            sumOG += CDec(DataBinder.Eval(e.Row.DataItem, "OG"))
            sumRG += CDec(DataBinder.Eval(e.Row.DataItem, "RG"))
            sumTS += CDec(DataBinder.Eval(e.Row.DataItem, "TS"))
            sumWJ += CDec(DataBinder.Eval(e.Row.DataItem, "WJ"))
            sumWP += CDec(DataBinder.Eval(e.Row.DataItem, "WP"))


            Dim FG As Double = e.Row.Cells(6).Text
            FG = String.Format(e.Row.Cells(6).Text, "{0:00,00.00}")
            e.Row.Cells(6).Text = Format(FG, "#,##0")

            Dim NG As Double = e.Row.Cells(7).Text
            NG = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(NG, "#,##0")

            Dim OG As Double = e.Row.Cells(8).Text
            OG = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(OG, "#,##0")

            Dim RG As Double = e.Row.Cells(9).Text
            RG = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(RG, "#,##0")

            Dim TS As Double = e.Row.Cells(10).Text
            TS = String.Format(e.Row.Cells(10).Text, "{0:00,00.00}")
            e.Row.Cells(10).Text = Format(TS, "#,##0")

            Dim WJ As Double = e.Row.Cells(11).Text
            WJ = String.Format(e.Row.Cells(11).Text, "{0:00,00.00}")
            e.Row.Cells(11).Text = Format(WJ, "#,##0")

            Dim WP As Double = e.Row.Cells(12).Text
            WP = String.Format(e.Row.Cells(12).Text, "{0:00,00.00}")
            e.Row.Cells(12).Text = Format(WP, "#,##0")


            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(12).HorizontalAlign = HorizontalAlign.Right


        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(4).Text = "รวม::"
            e.Row.Cells(6).Text = sumFG.ToString
            e.Row.Cells(7).Text = sumNG.ToString
            e.Row.Cells(8).Text = sumOG.ToString
            e.Row.Cells(9).Text = sumRG.ToString
            e.Row.Cells(10).Text = sumTS.ToString
            e.Row.Cells(11).Text = sumWJ.ToString
            e.Row.Cells(12).Text = sumWP.ToString
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Left
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(12).HorizontalAlign = HorizontalAlign.Right

            Dim sumFGT As Double = e.Row.Cells(6).Text
            sumFGT = String.Format(e.Row.Cells(6).Text, "{0:00,00.00}")
            e.Row.Cells(6).Text = Format(sumFGT, "#,##0")

            Dim sumNGT As Double = e.Row.Cells(7).Text
            sumNGT = String.Format(e.Row.Cells(7).Text, "{0:00,00.00}")
            e.Row.Cells(7).Text = Format(sumNGT, "#,##0")

            Dim sumOGT As Double = e.Row.Cells(8).Text
            sumOGT = String.Format(e.Row.Cells(8).Text, "{0:00,00.00}")
            e.Row.Cells(8).Text = Format(sumOGT, "#,##0")

            Dim sumRGT As Double = e.Row.Cells(9).Text
            sumRGT = String.Format(e.Row.Cells(9).Text, "{0:00,00.00}")
            e.Row.Cells(9).Text = Format(sumRGT, "#,##0")

            Dim sumTST As Double = e.Row.Cells(10).Text
            sumTST = String.Format(e.Row.Cells(10).Text, "{0:00,00.00}")
            e.Row.Cells(10).Text = Format(sumTST, "#,##0")

            Dim sumWJT As Double = e.Row.Cells(11).Text
            sumWJT = String.Format(e.Row.Cells(11).Text, "{0:00,00.00}")
            e.Row.Cells(11).Text = Format(sumWJT, "#,##0")

            Dim sumWPT As Double = e.Row.Cells(12).Text
            sumWPT = String.Format(e.Row.Cells(12).Text, "{0:00,00.00}")
            e.Row.Cells(12).Text = Format(sumWPT, "#,##0")


        End If
    End Sub
    Private Sub ShowHeader()
        Dim vDate As String
        Dim currThead As System.Globalization.CultureInfo
        currThead = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
        vDate = Format(Date.Now, "dd/MM/yyyy")
        If Mid(vDate, 7, 4) > 2500 Then
            vDate = Mid(vDate, 1, 2) + "/" + Mid(vDate, 4, 2) + "/" + (CDbl(Mid(vDate, 7, 4)) - 543).ToString
        End If
        Thread.CurrentThread.CurrentCulture = currThead
        LblHeader.Text = "ข้อมูลที่ Server ณ. วันที่ " + vDate + " เวลา " + Format(Now, "HH:mm")

        Try
            Dim vTitle As String = Request.QueryString("com")
            If vTitle <> "" Then
                Dim strsql As String = "select f01,f02,f04 Phone_NO,f05 Fax,AdrNo,AdrBldName,AdrMuu,AdrRd,AdrSub,AdrDis,Province,Country,ZipCode from v00010 where f01='" + vTitle + "'"
                Dim DT As New DataTable
                DT = C.GetDataTableRepweb(strsql)
                If DT.Rows.Count > 0 Then
                    Me.ShowHead.Text = DT.Rows(0).Item("f02").ToString

                    ShowHead.Visible = True
                End If
            End If

            Dim vHeader() As String

            If Session("HeaderSale") IsNot Nothing Then
                vHeader = Split(Session("HeaderSale").ToString, "|")
                comp_param.Text = vHeader(0).ToString
                report_name.Text = "[10070] รายงานสรุปยอดสินค้าคงคลัง"
                ro_param.Text = vHeader(1).ToString
                prov_param.Text = vHeader(2).ToString 
                branch_param.Text = vHeader(3).ToString + vHeader(4).ToString
                main_param.Text = vHeader(5).ToString + vHeader(6).ToString
                sub_param.Text = vHeader(7).ToString + vHeader(8).ToString
                product_param.Text = vHeader(9).ToString + vHeader(10).ToString
            End If
        Catch ex As Exception

            'Finally
            '    If LblParam1.Text.Trim.Length = 0 Then LblParam1.Visible = False
            '    If LblParam2.Text.Trim.Length = 0 Then LblParam2.Visible = False
            '    If LblParam3.Text.Trim.Length = 0 Then LblParam3.Visible = False
            '    If LblParam4.Text.Trim.Length = 0 Then LblParam4.Visible = False
        End Try
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

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'MyBase.VerifyRenderingInServerForm(control) 
    End Sub
End Class
