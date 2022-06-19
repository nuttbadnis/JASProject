Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim DB105 As New Cls_Data105
    Dim CP As New Cls_Panu
    Dim C As New Cls_Data

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CP.SessionUemail()
        'Session("Uemail") = "nat.m"
        ' If Request.QueryString("code") <> Nothing Then
        '     Session("Uemail") = CP.SetOAuthSingleSignOn(Request.QueryString("code"))
            
        '     If Session("current_url") <> Nothing Then
        '         Response.Redirect(Session("current_url"))
        '     Else
        '         Response.Redirect("~/default.aspx")
        '     End If
        ' End If

         'CP.checkLogin()
         'GetAnnouncement()
         'checkError()
    End Sub

    Protected Sub checkError()
        Dim vError As String = Request.QueryString("error")
        
        If vError <> Nothing Then
            Dim vTxtAlert As String = "Page Failed!!"

            If vError = "nopermiss" Then
                vTxtAlert = "No Permission!!"
            Else If vError = "pagefailed" Then
                vTxtAlert = "Page Failed!!"
            Else If vError = "norequest" Then
                vTxtAlert = "No Request!!"
            End If

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('" + vTxtAlert + "'); window.location = 'default.aspx';", True)
        Else
            CP.stampPatchUserFirstCome()

            If CP.checkDepartApprove() > 0 Then
                If CP.checkSettingModeData() > 0 Then
                    Response.Redirect("~/mode_data.aspx")
                End If

                Response.Redirect("~/mode_approve.aspx")
            Else

                Response.Redirect("~/mode_data.aspx")
            End If
        End If
        
    End Sub
    Protected Sub GetAnnouncement()
        Dim DT As DataTable
        Dim Announcement_paragraph As String
        Dim Sql As String = "select * from BB_ANNOUNCEMENT A "
        sql += "inner join BB_AnnounceType B ON A.AnnounceType = B.AnnounceType "
            If Session("Uemail") IsNot Nothing And Session("Uemail") <> "" Then
                sql += "where (Area is null or Area in ('AL') or Area in "
                sql += "(select case when RO is null then Area else RO end "
                sql += "from [10.11.5.106].[rmsdat01].[dbo].[V_ClusterPerUser] "
                sql += "where UserName = '" + Session("Uemail") + "')) and hidden = '0' "
                sql += "order by Sequence,convert(varchar(10),CreateDate,111) desc, "
                sql += "convert(varchar(10),EffectiveDate,111) desc, AnounceID desc "
                
            Else 'not login
                sql += "where (Area is null or Area = 'AL') and hidden = '0' "
                sql += "order by  Sequence,convert(varchar(10),CreateDate,111) desc, "
                sql += "convert(varchar(10),EffectiveDate,111) desc, AnounceID desc "
            End If
        DT = C.GetDataTable_Posweb(sql)
        If DT.Rows.Count > 0 then
            For i=0 to 9'DT.Rows.Count-1 
                Announcement_paragraph +="<div class='row font-mitr'>"
                Announcement_paragraph +="<div class='col-lg-9 col-12'>"
                Announcement_paragraph +="<span class='badge badge-danger fs-1 mr-1'>"+DT.Rows(i).Item("AnnounceName")+"</span>"
                Announcement_paragraph +=DT.Rows(i).Item("Subject")						
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="<div class='col-lg-3 col-12 text-right' style='font-family: 'Roboto';font-weight: 500;'>"
                Announcement_paragraph +="<i class='mdi mdi-calendar-clock mdi-24px'></i>"+DT.Rows(i).Item("EffectiveDate")+" "
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="<div class='col-lg-11 col-12 mt-2 mb-2 ml-2 p-2 ml-auto bg-light' style='font-size:14px;'>"
                Announcement_paragraph +=DT.Rows(i).Item("Detail")			
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="<div class='col-lg-9 col-12 mb-2'>"
                Announcement_paragraph +="<div class='d-flex flex-wrap justify-content-xl-between'>"
                Announcement_paragraph +="<div class='d-flex mr-4 align-items-center item'>"
                Announcement_paragraph +="<i class='mdi mdi-cellphone-iphone mr-3 icon-lg text-danger'></i>"
                Announcement_paragraph +="<div class='d-flex flex-column justify-content-around'>"
                Announcement_paragraph +="<small class='mb-1 text-muted'>�ͺ��������������������</small>"
                Announcement_paragraph +="<h5 class='mr-2 mb-0' style='font-family: 'Roboto';'>"+DT.Rows(i).Item("Contact")+"</h5>"
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="<div class='d-flex flex-grow-1 align-items-center item'>"
                Announcement_paragraph +="<i class='mdi mdi-account-location mr-3 icon-lg text-danger'></i>"
                Announcement_paragraph +="<div class='d-flex flex-column justify-content-around'>"
                Announcement_paragraph +="<small class='mb-1 text-muted'>��С����</small>"
                Announcement_paragraph +="<h5 class=vmr-2 mb-0'>"+DT.Rows(i).Item("AnnounceBy")+"</h5>"
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="<i class='icon-lg'></i>"
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="<div class='col-lg-3 col-12 text-right'>"
                If DT.Rows(i).Item("FileLink") <> "" or DT.Rows(i).Item("FileLink") <> Nothing Then
                    Announcement_paragraph +="<a href='"+DT.Rows(i).Item("FileLink")+"' class='btn btn-sm btn-block btn-primary text-light'>"
                    Announcement_paragraph +="<i class='mdi mdi-file-document-box'></i>"
                    Announcement_paragraph +=" �������������/����Сͺ"
                    Announcement_paragraph +="</a>"
                End If 
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="</div>"
                Announcement_paragraph +="<hr>"
            Next
            'Response.Write("have_data")   
        Else
            Announcement_paragraph +="<div class='font-mitr'>����բ����Ż�С������ش</div>"
            Announcement_paragraph +="<hr>"
            'Response.Write("not data")
        End If
        announcement_total.InnerHtml = "������ "+DT.Rows.Count.ToString+" ��¡��"
        announcement_list.InnerHtml = Announcement_paragraph 
        
    End Sub
End Class
