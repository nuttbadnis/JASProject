
Partial Class master_request
    Inherits System.Web.UI.MasterPage
    Dim CP As New Cls_Panu

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CP.SessionUemail()
        'CP.checkLogin()

        ' สำหรับทดสอบ
        Session("Uemail") = "kanoktip.s"

        If Session("Uemail") IsNot Nothing Then
            profile_name.InnerHtml = Session("Uemail")
        End If

        If Session("Empid") IsNot Nothing Then
			profile_pic.src = "https://intranet.jasmine.com/hr/office/Data/"+Session("Empid")+".jpg"		
        End If
    End Sub
End Class

