Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Net.IPAddress
Imports System.Threading

Public Class Cls_Panu
    Inherits System.Web.UI.Page
    Dim DB106 As New Cls_Data
    Dim DB105 As New Cls_Data105
    Dim DBShopStock As New Cls_DataShopStock

    Dim oauth_client_id as String = System.Configuration.ConfigurationSettings.AppSettings("oauth_client_id")
    Dim oauth_client_Secret as String = System.Configuration.ConfigurationSettings.AppSettings("oauth_client_Secret")
    Dim oauth_redirect_uri as String = System.Configuration.ConfigurationSettings.AppSettings("oauth_redirect_uri")

#Region "Checking"
    Public Sub checkLogin()
		' If Session("Uemail") <> "panupong.pa" Then
		' 	HttpContext.Current.Response.Redirect("~/xmaintenance.aspx")
		' End If
        ' checkLoginFRTest()
		
		'echo("HttpContext.Current.Request.Url.AbsoluteUri() = " & HttpContext.Current.Request.Url.AbsoluteUri())
		'echo("<br>fix replace rGetCurrentUrl = " & rGetCurrentUrl())
		
        If Session("Uemail") Is Nothing Then
            'Session("current_url") = HttpContext.Current.Request.Url.AbsoluteUri()
			Session("current_url") = rGetCurrentUrl()
			
            HttpContext.Current.Response.Redirect("https://oauthstg.bestftth.com/bestftth/oauth/oauth/authorize?response_type=code&client_id=" + oauth_client_id + "&redirect_uri=" + oauth_redirect_uri)
            ClientScript.RegisterStartupScript(Page.GetType, "open", "window.close()", True)
        End If
    End Sub

    Public Sub logout()
        ' logoutFRTest()

        Session().Clear()
        HttpContext.Current.Response.Redirect("https://oauthstg.bestftth.com/bestftth/oauth/jaslogout-page")
    End Sub

    Public Sub checkLoginFRTest()
        If Session("Uemail") Is Nothing Or Session("token") Is Nothing Then
            HttpContext.Current.Response.Redirect("~/login.aspx")
        End If
    End Sub

    Public Sub logoutFRTest()
        Dim Temp_token As String = Session("token")
        Session().Clear()

        Session("token") = Temp_token
        HttpContext.Current.Response.Redirect("~/login.aspx")
    End Sub

    Public Sub checkLoginmm()
        '******************************** Maintenance
        Session().Clear()
        Dim page As Page = HttpContext.Current.Handler
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('ระบบมีปัญหา กำลังตรวจสอบ ขอปิดปรับปรุงชั่วคราวครับ ***กรุณากดปิดหน้านี้'); window.location = 'default.aspx';", True)
        '******************************** Maintenance
    End Sub
    
    Public Sub SessionUemail()
        Dim Uemail As String = HttpContext.Current.Request.QueryString("em")

        If Uemail <> Nothing Then
            If Uemail.Trim() <> "" Then
                If Uemail = "clear" Then
                    Session.Clear()
                Else
                    Session("Uemail") = Uemail
                End If
            End If
        End If
    End Sub

    Function checkDepartApprove() As Integer
        ' Dim vSql As String = "select * from depart_user "
        Dim vSql As String = "select * from vw_depart_user depart_user "
        vSql += "join department "
        vSql += "on department.depart_id = depart_user.depart_id "
        vSql += "where uemail = '" & Session("Uemail") & "' "
        vSql += "and mode_approve = 1 "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Return vDT.Rows().Count()
    End Function

    Function checkSettingModeApprove() As Integer
        Dim vSql As String = "select * from setting_mode "
        vSql += "where uemail = '" & Session("Uemail") & "' "
        vSql += "and mode_approve = 1 "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Return vDT.Rows().Count()
    End Function

    Function checkSettingModeData() As Integer
        Dim vSql As String = "select * from setting_mode "
        vSql += "where uemail = '" & Session("Uemail") & "' "
        vSql += "and mode_data = 1 "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        Return vDT.Rows().Count()
    End Function

    Function getSettingTabPin() As String
        Dim vTab As String = ""

        Dim vSql As String = "select * from setting_tab_pin "
        vSql += "where uemail = '" & Session("Uemail") & "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() > 0 Then
            vTab = vDT.Rows(0).Item("tab_pin")
        End If

        Return vTab
    End Function

    Public Function userDepartmentTB(ByVal uemail As String) As DataTable
        Dim vSql As String
        vSql = "select department.depart_id ,depart_name, group_email "
        vSql += "from depart_user "
        vSql += "join department "
        vSql += "on department.depart_id = depart_user.depart_id "
        vSql += "where uemail = '" + uemail + "' "
        vSql += "and start_date <= getdate() "
        vSql += "and (expired_date is null or expired_date >= getdate()) "

        Return DB105.GetDataTable(vSql)
    End Function

    Public Function userDepartment(ByVal uemail As String) As String
        Dim vDT As New DataTable
        vDT = userDepartmentTB(uemail)

        Dim inDepart As String = ""

        If vDT.Rows().Count() = 0 Then
            inDepart = "999888898"
        Else
            For i As Integer = 0 To vDT.Rows().Count() - 1
                inDepart += vDT.Rows(i).Item("depart_id") & ","
            Next
            
            inDepart = inDepart.Remove(inDepart.Length - 1, 1)
        End If

        Return inDepart
    End Function

    Public Function userGroupEmail(ByVal uemail As String) As String
        Dim vDT As New DataTable
        vDT = userDepartmentTB(uemail)

        Dim inGroupEmail As String = ""

        If vDT.Rows().Count() > 0 Then
            For i As Integer = 0 To vDT.Rows().Count() - 1
                If Not IsDBNull(vDT.Rows(i).Item("group_email")) Then
                    inGroupEmail += "'" + vDT.Rows(i).Item("group_email") & "',"
                End If
            Next
        End If
            
        If inGroupEmail.Length > 0 Then
            If inGroupEmail.Substring(inGroupEmail.Length - 1) = "," Then
                inGroupEmail = inGroupEmail.Remove(inGroupEmail.Length - 1, 1)
            End If
        Else
            inGroupEmail = "'groupemail'"
        End If

        Return inGroupEmail
    End Function
    
    Public Sub savePushpin(ByVal tabSys As String)
        Dim vSqlIn As String
        vSqlIn = "delete from setting_tab_pin where uemail = '" & Session("Uemail") & "' "
        vSqlIn += "insert into setting_tab_pin (uemail, tab_pin) values ('" & Session("Uemail") & "', '" & tabSys & "') "

        If DB105.ExecuteNonQuery(vSqlIn).ToString() >= 1 Then
            HttpContext.Current.Response.Redirect(rGetCurrentUrl())
            'HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString())
        End If
    End Sub
#End Region

#Region "PatchUpdate"

    Public Sub stampPatchUserFirstCome()
        Dim vSql As String 
        vSql = "select patch_number from xpatch_close_popup where uemail = '" & Session("Uemail") & "' "

        Dim vDT As New DataTable
        vDT = DB105.GetDataTable(vSql)

        If vDT.Rows().Count() = 0 Then
            Dim vSqlIn As String = "insert into xpatch_close_popup (uemail, patch_number) values ( "
            vSqlIn += "'" & Session("Uemail") & "', "
            vSqlIn += "(select max(patch_number) patch_number from xpatch_update where patch_date <= cast(getdate() as date) and disable = 0) "
            vSqlIn += ") "

            DB105.ExecuteNonQuery(vSqlIn)
        End If
    End Sub

    Public Function rLoadCurrentPatch() As String
        Dim vSql As String = "select patch_date, patch_number, patch_title, patch_img "
        vSql += "from xpatch_update "
        vSql += "where patch_number = ( "
        vSql += "    select min(patch_number) "
        vSql += "    from xpatch_update "
        vSql += "    where patch_number > ( "
        vSql += "        select patch_number from xpatch_close_popup where uemail = '" & Session("Uemail") & "' "
        vSql += "    ) "
        vSql += "and patch_date >= cast(getdate()-15 as date) "
        vSql += "and patch_date <= cast(getdate() as date) "
        vSql += "and disable = 0 "
        vSql += ") "

        Return vSql
    End Function

    Public Function rLoadNotReadedPatch() As String
        Dim vSql As String = "select patch_date, patch_number, patch_title, patch_img "
        vSql += "from xpatch_update "
        vSql += "where patch_number not in ( "
        vSql += "    select patch_number from xpatch_readed where uemail = '" & Session("Uemail") & "' "
        vSql += ") "
        vSql += "and patch_date >= cast(getdate()-15 as date) "
        vSql += "and patch_date <= cast(getdate() as date) "
        vSql += "and disable = 0 "

        Return vSql
    End Function

#End Region

#Region "SendMail & LineAlert"

    Public Function rLineAlert(ByVal CHAT As String, ByVal MSS As String) As Boolean
        MSS = "http://10.11.15.102:8055/LINE/100000000/" + CHAT + "/" + MSS + "?passkey=botbcs"
        Return rRequestAddress(MSS)
    End Function

    Public Function rRequestAddress(ByVal URL As String) As Boolean
        Try
            Dim request As WebRequest = WebRequest.Create(URL)
            Dim response As WebResponse = request.GetResponse()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
#End Region

#Region "Json Data"

    Function rJsonDB(ByVal vTable As String, ByVal vSql As String, ByVal vField() As String, Optional ByVal vDB As String = "DB106") As String
        Dim vJson As String = ""
        Dim eTemp As String = ""

        Dim vRes As New DataTable
        vRes = rGetDataTable(vSql, vDB)

        For i As Integer = 0 To vRes.Rows().Count() - 1
            vJson += "{"
            For Each e As String In vField
                eTemp = "Null"
                If Not IsDbNull(vRes.Rows(i).Item(e)) Then
                    eTemp = vRes.Rows(i).Item(e)
                End If
                vJson += " """ + e + """:""" + CStr(eTemp) + """ ,"
            Next
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "},"
        Next

        If vJson <> Nothing Then
            vJson = "{ """ + vTable + """ : [" + vJson
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "]}"
        Else
            vJson = "{ """ + vTable + """ : []}"
        End If

        '******TAG SQL******'
        vJson = vJson.Remove(vJson.Length - 1, 1) + ", ""sql"" : """ + vSql + """ }"
        '******TAG SQL******'

        Return vJson
    End Function

    ''v2: update from v1
    ''append string
    ''change vFields is Array to Dictionary
    Function rJsonDBv2(ByVal vTable As String, ByVal vSql As String, ByVal vFields As Dictionary(Of String, String), Optional ByVal vDB As String = "DB106") As String
        Dim vJson As String = ""
        Dim vTemp As String = ""

        Dim vRes As New DataTable
        vRes = rGetDataTable(vSql, vDB)

        For i As Integer = 0 To vRes.Rows().Count() - 1
            vJson += "{"
            For Each k As KeyValuePair(Of String, String) In vFields
                vTemp = "Null"
                If Not IsDbNull(vRes.Rows(i).Item(k.Value)) Then
                    vTemp = vRes.Rows(i).Item(k.Value)
                End If
                vJson += " """ + k.Value + """:""" + CStr(vTemp) + """ ,"
            Next 
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "},"
        Next

        If vJson <> Nothing Then
            vJson = "{ """ + vTable + """ : [" + vJson
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "]}"
        Else
            vJson = "{ """ + vTable + """ : []}"
        End If

        '******TAG SQL******'
        vJson = vJson.Remove(vJson.Length - 1, 1) + ", ""sql"" : """ + vSql + """ }"
        '******TAG SQL******'

        Return vJson
    End Function

    ''v3: update from v2
    ''append string
    ''change vFields is Dictionary to ArrayList
    Function rJsonDBv3(ByVal vTable As String, ByVal vSql As String, ByVal vFields As ArrayList, Optional ByVal vDB As String = "DB106") As String
        Dim vJson As String = ""
        Dim vTemp As String = ""

        Dim vRes As New DataTable
        vRes = rGetDataTable(vSql, vDB)

        For i As Integer = 0 To vRes.Rows().Count() - 1
            vJson += "{"
            For Each vf As String In vFields
                vTemp = "Null"
                If Not IsDbNull(vRes.Rows(i).Item(vf)) Then
                    vTemp = vRes.Rows(i).Item(vf)
                End If
                vJson += " """ + vf + """:""" + CStr(vTemp) + """ ,"
            Next
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "},"
        Next

        If vJson <> Nothing Then
            vJson = "{ """ + vTable + """ : [" + vJson
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "]}"
        Else
            vJson = "{ """ + vTable + """ : []}"
        End If

        '******TAG SQL******'
        vJson = vJson.Remove(vJson.Length - 1, 1) + ", ""sql"" : """ + vSql + """ }"
        '******TAG SQL******'

        Return vJson
    End Function

    ''v3.2: update from v3
    ''array style
    ''upgrade vJson to List+Dictionary
    ''*** changed return value to List (is not have tag header AND tag sql)
    Function rJsonDBv3_2(ByVal vTable As String, ByVal vSql As String, ByVal vFields As ArrayList, Optional ByVal vDB As String = "DB106") As String
        Dim vJson = New List(Of Dictionary(Of String, String))()
        Dim vTemp As String = ""

        Dim vRes As New DataTable
        vRes = rGetDataTable(vSql, vDB)

        For i As Integer = 0 To vRes.Rows().Count() - 1
            Dim vRes_row = New Dictionary(Of String, String)

            For Each vf As String In vFields
                vTemp = "Null"
                If Not IsDbNull(vRes.Rows(i).Item(vf)) Then
                    vTemp = vRes.Rows(i).Item(vf)
                End If
                vRes_row.Add(vf, CStr(vTemp))
            Next 

            vJson.Add(vRes_row)
        Next

        Dim serializer as New JavaScriptSerializer()
        Dim arrayJson As String = serializer.Serialize(vJson)
        Return arrayJson
    End Function

    ''v4: update from v3.2
    ''array style
    ''upgrade vJson to List+Dictionary
    ''*** changed auto vFields (input just vSql ,wow it Easy)
    Function rJsonDBv4(ByVal vSql As String, Optional ByVal vDB As String = "DB106dat") As String
        Dim vJson = New List(Of Dictionary(Of String, String))()
        Dim vTemp As String = ""

        Dim vRes As New DataTable
        vRes = rGetDataTable(vSql, vDB)

        Dim vFields = New ArrayList
        For c As Integer = 0 To vRes.Columns.Count - 1
            vFields.Add(vRes.Columns(c).ColumnName)
        Next

        For i As Integer = 0 To vRes.Rows().Count() - 1
            Dim vRes_row = New Dictionary(Of String, String)

            For Each vf As String In vFields
                vTemp = "Null"
                If Not IsDbNull(vRes.Rows(i).Item(vf)) Then
                    vTemp = vRes.Rows(i).Item(vf)
                End If
                vRes_row.Add(vf, rValidJson(CStr(vTemp)))
            Next 

            vJson.Add(vRes_row)
        Next

        Dim serializer as New JavaScriptSerializer()
        Dim arrayJson As String = serializer.Serialize(vJson)
        Return arrayJson
    End Function


    ''v4s: update from v4
    ''array style
    ''downgrade to append string
    Function rJsonDBv4s(ByVal vSql As String, Optional ByVal vDB As String = "DB106dat") As String
        Dim vJson As String = ""
        Dim vTemp As String = ""

        Dim vRes As New DataTable
        vRes = rGetDataTable(vSql, vDB)

        Dim vFields = New ArrayList
        For c As Integer = 0 To vRes.Columns.Count - 1
            vFields.Add(vRes.Columns(c).ColumnName)
        Next

        For i As Integer = 0 To vRes.Rows().Count() - 1
            vJson += "{"
            For Each vf As String In vFields
                vTemp = "Null"
                If Not IsDbNull(vRes.Rows(i).Item(vf)) Then
                    vTemp = vRes.Rows(i).Item(vf)
                End If
                vJson += " """ + vf + """:""" + rValidJson(CStr(vTemp)) + """ ,"
            Next
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "},"
        Next

        If vJson <> Nothing Then
            vJson = "[" + vJson
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "]"
        Else
            vJson = "[]"
        End If

        '******TAG SQL******'
        'vJson = vJson.Remove(vJson.Length - 1, 1) + ", ""sql"" : """ + vSql + """ }"
        '******TAG SQL******'

        Return vJson
    End Function

    ''v1 append string
    Function rConvertDataTableToJSONv1(ByVal vRes As DataTable) As String
        Dim vJson As String = ""
        Dim vTemp As String = ""

        Dim vFields = New ArrayList
        For c As Integer = 0 To vRes.Columns.Count - 1
            vFields.Add(vRes.Columns(c).ColumnName)
        Next

        For i As Integer = 0 To vRes.Rows().Count() - 1
            vJson += "{"
            For Each vf As String In vFields
                vTemp = "Null"
                If Not IsDbNull(vRes.Rows(i).Item(vf)) Then
                    vTemp = vRes.Rows(i).Item(vf)
                End If
                vJson += " """ + vf + """:""" + CStr(vTemp) + """ ,"
            Next
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "},"
        Next

        If vJson <> Nothing Then
            vJson = "[" + vJson
            vJson = vJson.Remove(vJson.Length - 1, 1)
            vJson += "]"
        Else
            vJson = "[]"
        End If

        Return vJson
    End Function

    ''v2 List+Dictionary
    Function rConvertDataTableToJSONv2(ByVal vRes As DataTable) As String
        Dim vJson = New List(Of Dictionary(Of String, String))()
        Dim vTemp As String = ""

        Dim vFields = New ArrayList
        For c As Integer = 0 To vRes.Columns.Count - 1
            vFields.Add(vRes.Columns(c).ColumnName)
        Next

        For i As Integer = 0 To vRes.Rows().Count() - 1
            Dim vRes_row = New Dictionary(Of String, String)

            For Each vf As String In vFields
                vTemp = "Null"
                If Not IsDbNull(vRes.Rows(i).Item(vf)) Then
                    vTemp = vRes.Rows(i).Item(vf)
                End If
                vRes_row.Add(vf, CStr(vTemp))
            Next 

            vJson.Add(vRes_row)
        Next

        Dim serializer as New JavaScriptSerializer()
        Dim arrayJson As String = serializer.Serialize(vJson)
        Return arrayJson
    End Function

    Function rGetDataTable(ByVal vSql As String, Optional ByVal vDB As String = "DB106") As DataTable
        Dim vRes As New DataTable
        
        If vDB = "DB105" Then
            vRes = DB105.GetDataTable(vSql)

        ElseIf vDB = "DBShopStock" Then
            vRes = DBShopStock.GetDataTable(vSql)
            
        Else
            vRes = DB106.GetDataTable(vSql)
        End If

        return vRes
    End Function

    Function rMergeJson(ByVal vJsonA As String, ByVal vJsonB As String) As String
        vJsonA = vJsonA.Remove(vJsonA.Length - 1, 1) + ", "
        vJsonB = vJsonB.Remove(0, 1)

        Return vJsonA + vJsonB
    End Function

    Function rAddTagJson(ByVal vJson As String, ByVal vTag As String, ByVal vData As String) As String
        Return vJson.Remove(vJson.Length - 1, 1) + ", """ + vTag + """ : """ + vData + """ }"
    End Function
#End Region

#Region "OAuth"
    Public Function SetOAuthSingleSignOn(ByVal code As String)
        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)
        Dim DS As New DataSet
        Dim DT As New DataTable
        Dim HttpWReq As HttpWebRequest
        Dim httpWRes As HttpWebResponse = Nothing
        Dim address As Uri
        Dim strData As New StringBuilder
        Dim vUri As String = "https://oauthstg.bestftth.com/bestftth/oauth/oauth/token?client_id=" + oauth_client_id + "&redirect_uri=" + oauth_redirect_uri + "&grant_type=authorization_code&code=" + code

        address = New Uri(vUri)
        HttpWReq = DirectCast(WebRequest.Create(address), HttpWebRequest)
        HttpWReq.Method = "POST"
        HttpWReq.ContentType = "application/x-www-form-urlencoded"

        SetBasicAuthHeader(HttpWReq, oauth_client_id, oauth_client_Secret)

        httpWRes = DirectCast(HttpWReq.GetResponse(), HttpWebResponse)

        Dim reader As StreamReader = New StreamReader(httpWRes.GetResponseStream())
        Dim json As String = reader.ReadToEnd()

        Dim vHeader() As String
        Dim Token() As String
        Dim Access_Token As String = ""
        vHeader = Split(json.ToString, ",")
        If vHeader.Length > 2 Then
            Token = Split(vHeader(0).ToString, ":")
            Access_Token = Replace(Token(1).ToString, """", "").ToString.Trim
        End If

        Dim Request As HttpWebRequest
        Dim Response As HttpWebResponse
        Request = DirectCast(WebRequest.Create(New Uri("https://oauthstg.bestftth.com/bestftth/oauth/me")), HttpWebRequest)
        HttpWReq.Method = "GET"
        HttpWReq.ContentType = "application/x-www-form-urlencoded"
        Request.Headers("Authorization") = "Bearer " + Access_Token

        Response = DirectCast(Request.GetResponse(), HttpWebResponse)
        reader = New StreamReader(Response.GetResponseStream())
        json = reader.ReadToEnd()

        DT = ConvertJSONToDataTable(json)
        Dim username As String() = DT.Rows(0).Item("username").ToString.Split("@")
        'Session("Uemail") = username(0)
        Session("token") = Access_Token

        Return username(0)
    End Function

    Private Sub SetBasicAuthHeader(ByVal request As WebRequest, ByVal userName As String, ByVal userPassword As String)
        Dim authInfo As String = userName + ":" + userPassword
        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo))
        request.Headers("Authorization") = "Basic " + authInfo
    End Sub

    Private Function ConvertJSONToDataTable(ByVal jsonString As String) As DataTable
        Dim dt As New DataTable
        'strip out bad characters
        Dim jsonParts As String() = jsonString.Replace("[{", "{").Replace("}]", "}").Split("},{")

        'hold column names
        Dim dtColumns As New List(Of String)

        'get columns
        For Each jp As String In jsonParts
            'only loop thru once to get column names
            Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            For Each rowData As String In propData
                Try
                    If rowData.Split(":").Length - 1 <> 0 Then
                        Dim idx As Integer = rowData.IndexOf(":")
                        Dim n As String = rowData.Substring(0, idx - 1)
                        Dim v As String = rowData.Substring(idx + 1)
                        If Not dtColumns.Contains(n) Then
                            dtColumns.Add(n.Replace("""", ""))
                        End If
                    End If
                Catch ex As Exception
                    Throw New Exception(String.Format("Error Parsing Column Name : {0}", rowData))
                End Try

            Next
            Exit For
        Next

        'build dt
        For Each c As String In dtColumns
            dt.Columns.Add(c)
        Next
        'get table data
        For Each jp As String In jsonParts
            Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim nr As DataRow = dt.NewRow
            For Each rowData As String In propData
                Try
                    Dim idx As Integer = rowData.IndexOf(":")
                    Dim n As String = rowData.Substring(0, idx - 1).Replace("""", "")
                    Dim v As String = rowData.Substring(idx + 1).Replace("""", "")
                    nr(n) = v
                Catch ex As Exception
                    Continue For
                End Try

            Next
            dt.Rows.Add(nr)
        Next
        Return dt
    End Function

    Public Function rGetDataOAuth(ByVal vSearch As String, ByVal vToken As String) As Dictionary(Of String, String)
        Dim DT As New DataTable
        Dim DataOAuth = New Dictionary(Of String, String)
        Dim DataOAuthStatus As Boolean = false

        Dim json As String
        Dim reader As StreamReader

        Dim Request As HttpWebRequest
        Dim Response As HttpWebResponse

        Try
            Request = DirectCast(WebRequest.Create(New Uri("https://app.jasmine.com/contact-resource-api/v2/" + vSearch + "@jasmine.com/")), HttpWebRequest)
            'Request = DirectCast(WebRequest.Create(New Uri("https://app.jasmine.com/contact-resource-api/v2/")), HttpWebRequest)
            Request.Method = "GET"
            Request.ContentType = "application/x-www-form-urlencoded"
            Request.Headers("token") = vToken
            Request.Headers("scope") = "employee-information"
            Response = DirectCast(Request.GetResponse(), HttpWebResponse)
            reader = New StreamReader(Response.GetResponseStream())
            json = reader.ReadToEnd()
            DT = ConvertJSONToDataTable(json)

            If DT.Rows.Count > 0 Then
                For i As Integer = 0 To DT.Rows.Count - 2
                    If (DT.Rows(i).Item("dateExpired") = "" Or DT.Rows(i).Item("dateExpired") = "null") _
                    And DT.Rows(i).Item("accountStatus") = "true" _
                    And DT.Rows(i).Item("email") = (vSearch + "@jasmine.com") Then
                    'Or rCDateText(DT.Rows(i).Item("dateExpired")) > rGetDateNow) 
                        DataOAuth.Add("engInitialname", DT.Rows(i).Item("engInitialname"))
                        DataOAuth.Add("engFirstname", DT.Rows(i).Item("engFirstname"))
                        DataOAuth.Add("engLastname", DT.Rows(i).Item("engLastname"))
                        DataOAuth.Add("email", DT.Rows(i).Item("email"))
                        DataOAuth.Add("thaiInitialname", DT.Rows(i).Item("thaiInitialname"))
                        DataOAuth.Add("thaiFirstname", DT.Rows(i).Item("thaiFirstname"))
                        DataOAuth.Add("thaiLastname", DT.Rows(i).Item("thaiLastname"))
                        DataOAuth.Add("employeeId", DT.Rows(i).Item("employeeId"))
                        DataOAuth.Add("position", DT.Rows(i).Item("position"))
                        DataOAuth.Add("workPhone", DT.Rows(i).Item("workPhone"))
                        DataOAuth.Add("dateExpired", DT.Rows(i).Item("dateExpired"))
                        DataOAuth.Add("accountStatus", DT.Rows(i).Item("accountStatus"))
                        DataOAuthStatus = true
                    End If
                Next
            End If

        Catch ex As Exception
            DataOAuthStatus = false
        End Try

        'DataOAuth.Add("DTRowsCount", DT.Rows.Count)
        DataOAuth.Add("DataOAuthStatus", DataOAuthStatus)
        Return DataOAuth
    End Function

    Public Function rGetDataOAuthjson(ByVal vSearch As String, ByVal vToken As String) As String
        Dim DT, DT2 As New DataTable
        Dim json As String

        Dim reader As StreamReader
        Dim Request As HttpWebRequest
        Dim Response As HttpWebResponse

        Try
            Request = DirectCast(WebRequest.Create(New Uri("https://app.jasmine.com/contact-resource-api/v2/" + vSearch + "/")), HttpWebRequest)
            Request.Method = "GET"
            Request.ContentType = "application/x-www-form-urlencoded"
            Request.Headers("token") = vToken
            Request.Headers("scope") = "employee-information"
            Response = DirectCast(Request.GetResponse(), HttpWebResponse)
            reader = New StreamReader(Response.GetResponseStream())
            json = reader.ReadToEnd()
            DT = ConvertJSONToDataTable(json)

            DT2.Columns.Add("employeeId")
            DT2.Columns.Add("thaiFullname")
            DT2.Columns.Add("engFullname")
            DT2.Columns.Add("email")
            DT2.Columns.Add("position")
            DT2.Columns.Add("department")
            DT2.Columns.Add("section")
            DT2.Columns.Add("division")

            If DT.Rows.Count > 0 Then
                For i As Integer = 0 To DT.Rows.Count - 2
                    If (DT.Rows(i).Item("dateExpired") = "" Or DT.Rows(i).Item("dateExpired") = "null") _
                    And DT.Rows(i).Item("accountStatus") = "true" Then
                    'Or rCDateText(DT.Rows(i).Item("dateExpired")) > rGetDateNow) 

                        Dim DR As DataRow = DT2.NewRow
                        DR("employeeId") = DT.Rows(i).Item("employeeId")
                        DR("thaiFullname") = DT.Rows(i).Item("thaiFullname")
                        DR("engFullname") = DT.Rows(i).Item("engFullname")
                        DR("email") = DT.Rows(i).Item("email")
                        DR("position") = DT.Rows(i).Item("position")
                        DR("department") = DT.Rows(i).Item("department")
                        DR("section") = DT.Rows(i).Item("section")
                        DR("division") = DT.Rows(i).Item("division")
                        DT2.Rows.Add(DR)

                    End If
                Next
            End If

            json = rConvertDataTableToJSONv1(DT2)

        Catch ex As Exception
            json = "[]"
        End Try

        Return json
    End Function

    Public Function rGetDataOAuthjson_20170824(ByVal vSearch As String, ByVal vToken As String) As String
        Dim DT As New DataTable
        Dim DataOAuth = New Dictionary(Of String, String)
        Dim DataOAuthStatus As Boolean = false

        Dim json As String = "[]"
        Dim reader As StreamReader

        Dim Request As HttpWebRequest
        Dim Response As HttpWebResponse

        Try
            Request = DirectCast(WebRequest.Create(New Uri("https://app.jasmine.com/contact-resource-api/v2/" + vSearch + "/")), HttpWebRequest)
            Request.Method = "GET"
            Request.ContentType = "application/x-www-form-urlencoded"
            Request.Headers("token") = vToken
            Request.Headers("scope") = "employee-information"
            Response = DirectCast(Request.GetResponse(), HttpWebResponse)
            reader = New StreamReader(Response.GetResponseStream())
            json = reader.ReadToEnd()

        Catch ex As Exception
            json = "[{ ""nodata"" : """ + vSearch + """}]"
        End Try

        Return json
    End Function

    Public Function rGetDataOAuthDT(ByVal vSearch As String, ByVal vToken As String) As DataTable
        Dim DT As New DataTable
        Dim DataOAuth = New Dictionary(Of String, String)

        Dim json As String
        Dim reader As StreamReader

        Dim Request As HttpWebRequest
        Dim Response As HttpWebResponse

        Try
            Request = DirectCast(WebRequest.Create(New Uri("https://app.jasmine.com/contact-resource-api/v2/" + vSearch + "@jasmine.com/")), HttpWebRequest)
            Request.Method = "GET"
            Request.ContentType = "application/x-www-form-urlencoded"
            Request.Headers("token") = vToken
            Request.Headers("scope") = "employee-information"
            Response = DirectCast(Request.GetResponse(), HttpWebResponse)
            reader = New StreamReader(Response.GetResponseStream())
            json = reader.ReadToEnd()
            DT = ConvertJSONToDataTable(json)

        Catch ex As Exception

        End Try

        Return DT
    End Function
#End Region

#Region "Analytics"

    Public Sub Analytics()
        Try
            Dim vUrl As String = HttpContext.Current.Request.Url.AbsoluteUri()

            Dim vPage As String = rGetPageName()
            Dim vUemail As String = Session("Uemail")
            Dim vIP As String = HttpContext.Current.Request.UserHostAddress()
            Dim vBrowser As String = rGetBrowserName()
            Dim vOS As String = rGetOS()
            'Dim vComname As String = rGetClientComName()

            Dim vSqlIn = "INSERT INTO analyticsites (page,url,uemail,ip,browser,os_version) VALUES ("
            vSqlIn += "'" & vPage & "', "
            vSqlIn += "'" & vUrl & "', "
            vSqlIn += "'" & vUemail & "', "
            vSqlIn += "'" & vIP & "', "
            vSqlIn += "'" & vBrowser & "', "
            vSqlIn += "'" & vOS & "' "
            vSqlIn += ")"

            DB105.ExecuteNonQuery(vSqlIn)

        Catch ex As Exception
            Try
                Dim vUrl As String = HttpContext.Current.Request.Url.AbsoluteUri()

                Dim vPage As String = rGetPageName()
                Dim vIP As String = HttpContext.Current.Request.UserHostAddress()

                Dim vSqlIn = "INSERT INTO analyticsites (page,url,ip) VALUES ("
                vSqlIn += "'" & vPage & "', "
                vSqlIn += "'" & vUrl & "', "
                vSqlIn += "'" & vIP & "' "
                vSqlIn += ")"

                DB105.ExecuteNonQuery(vSqlIn)

            Catch ex2 As Exception
            End Try
        End Try
    End Sub

    Public Function rGetPageName(Optional ByVal vRecursive As String = "") As String
        Dim vRes As String = HttpContext.Current.Request.Url.AbsoluteUri()

        If vRecursive <> "" Then
            vRes = vRecursive
        End If

        Dim vTemp As String = vRes

        Try
            vRes = vRes.Substring(vRes.LastIndexOf("/") + 1)
            vRes = vRes.Substring(0, vRes.IndexOf("."))
        
            return vRes
        Catch ex As Exception
            vRes = vTemp
            vRes = vRes.Substring(0, vRes.LastIndexOf("/"))

            return rGetPageName(vRes)
        End Try

    End Function

    Public Function rGetClientComName() As String
        Try
            Return System.Net.Dns.GetHostByAddress(HttpContext.Current.Request.UserHostAddress()).HostName
        Catch ex As Exception
            Return "unknown"
        End Try
    End Function

    Public Function rGetBrowserName() As String
        Dim userAgent As String = HttpContext.Current.Request.UserAgent
        If userAgent.Contains("Firefox") Then
            Return userAgent.Substring(userAgent.IndexOf("Firefox"))
        ElseIf userAgent.Contains("Chrome") Then
            Dim agentPart As String = userAgent.Substring(userAgent.IndexOf("Chrome"))
            Return agentPart.Substring(0, agentPart.IndexOf("Safari") - 1)
        End If

        Return HttpContext.Current.Request.Browser.Browser & "/" & HttpContext.Current.Request.Browser.Version
    End Function

    Public Function rGetOS() As String
        Dim vClientAgent As String = HttpContext.Current.Request.UserAgent().ToLower()

        If vClientAgent.IndexOf("windows nt 10.0") >= 0 Then
            Return "Windows 10"
        ElseIf vClientAgent.IndexOf("windows nt 6.3") >= 0 Then
            Return "Windows 8.1"
        ElseIf vClientAgent.IndexOf("windows nt 6.2") >= 0 Then
            Return "Windows 8"
        ElseIf vClientAgent.IndexOf("windows nt 6.1") >= 0 Then
            Return "Windows 7"
        ElseIf vClientAgent.IndexOf("windows nt 6.0") >= 0 Then
            Return "Windows Vista"
        ElseIf vClientAgent.IndexOf("windows nt 5.2") >= 0 Then
            Return "Windows Server 2003"
        ElseIf vClientAgent.IndexOf("windows nt 5.1") >= 0 Then
            Return "Windows XP"
        ElseIf vClientAgent.IndexOf("windows nt 5.01") >= 0 Then
            Return "Windows 2000 (SP1)"
        ElseIf vClientAgent.IndexOf("windows nt 5.0") >= 0 Then
            Return "Windows 2000"
        ElseIf vClientAgent.IndexOf("windows nt 4.5") >= 0 Then
            Return "Windows NT 4.5"
        ElseIf vClientAgent.IndexOf("windows nt 4.0") >= 0 Then
            Return "Windows NT 4.0"
        ElseIf vClientAgent.IndexOf("win 9x 4.90") >= 0 Then
            Return "Windows ME"
        ElseIf vClientAgent.IndexOf("windows 98") >= 0 Then
            Return "Windows 98"
        ElseIf vClientAgent.IndexOf("windows 95") >= 0 Then
            Return "Windows 95"
        ElseIf vClientAgent.IndexOf("windows ce") >= 0 Then
            Return "Windows CE"
        ElseIf (vClientAgent.Contains("ipad")) Then
            Return String.Format("iPad OS {0}", GetMobileVersion(vClientAgent, "OS"))
        ElseIf (vClientAgent.Contains("iphone")) Then
            Return String.Format("iPhone OS {0}", GetMobileVersion(vClientAgent, "OS"))
        ElseIf (vClientAgent.Contains("linux") AndAlso vClientAgent.Contains("kfapwi")) Then
            Return "Kindle Fire"
        ElseIf (vClientAgent.Contains("rim tablet") OrElse (vClientAgent.Contains("bb") AndAlso vClientAgent.Contains("mobile"))) Then
            Return "Black Berry"
        ElseIf (vClientAgent.Contains("Windows phone")) Then
            Return String.Format("Windows Phone {0}", GetMobileVersion(vClientAgent, "Windows Phone"))
        ElseIf (vClientAgent.Contains("mac os")) Then
            Return "Mac OS"
        ElseIf vClientAgent.IndexOf("android") >= 0 Then
            Return String.Format("Android {0}", GetMobileVersion(vClientAgent, "ANDROID"))
        Else
            Return "OS is unknown."
        End If
    End Function

    Private Function GetMobileVersion(userAgent As String, device As String) As String
        Dim ReturnValue As String = String.Empty
        Dim RawVersion As String = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart()

        For Each character As Char In RawVersion
            If IsNumeric(character) Then
                ReturnValue &= character
            ElseIf (character = "." OrElse character = "_") Then
                ReturnValue &= "."
            Else
                Exit For
            End If
        Next

        Return ReturnValue
    End Function
#End Region
    
#Region "NoCat"

    Function rGetCurrentUrl() As String
        Dim vCurrentUrl As String = HttpContext.Current.Request.Url.AbsoluteUri()
		vCurrentUrl = vCurrentUrl.Replace("http:", "https:")
		vCurrentUrl = vCurrentUrl.Replace("postest", "posbcs")
		
        Return vCurrentUrl
    End Function
    Public Sub echo(ByVal vStr As String)
        HttpContext.Current.Response.Write(vStr)
    End Sub

    Function rAlphabet(ByVal vABC As String) As String
        Dim Alphabet As String = " ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        If Regex.IsMatch(vABC, "^[0-9 ]+$") Then
            Return vABC
        Else
            Dim vR As String = vABC.ToUpper()
            vR = Alphabet.IndexOf(vR, 0)
            vR = vR Mod 10

            Return vR
        End If
    End Function

    Function rGetIpHost() As String
        Dim vHostName As String = System.Net.Dns.GetHostName()
        Dim vIPAddress As String = System.Net.Dns.GetHostByName(vHostName).AddressList(0).ToString()

        Return vIPAddress
    End Function

    Function rNullToEmpty(ByVal vValue As String) As String
        If vValue.ToLower().Trim() = "null" Then
            Return ""
        Else
            Return vValue
        End if
    End Function

    Function rReplaceSpace(ByVal vValue As String) As String
        Try
            rReplaceSpace = RTrim(LTrim(vValue))
        Catch ex As Exception
            rReplaceSpace = vValue
        End Try
    End Function

    Function rSQL(ByVal vValue As String) As String
        If vValue <> Nothing Then
            Return LTRIM(RTRIM(vValue))
        Else
            Return vValue
        End if
    End Function

    Function rReplaceQuote(ByVal vValue As String) As String
        vValue = rNullToEmpty(vValue)

        If vValue <> Nothing Then
            Return vValue.Replace("'", "''")
        Else
            Return vValue
        End if
    End Function

    Function rReplaceConvertHTML(ByVal vValue As String) As String
        If vValue <> Nothing Then
            vValue = vValue.Replace("|BR|", "<br>")
            vValue = vValue.Replace("|nbs|", "&nbsp;")
            Return rReplaceQuote(vValue)
        Else
            Return vValue
        End if
    End Function

    Function rReplaceDBQuote(ByVal vValue As String) As String
        If vValue <> Nothing Then
            'Return vValue.Replace("""", "\""")
            vValue = vValue.Replace("""", "\""").Replace(vbCr, "").Replace(vbLf, "")
            Return Regex.Replace(vValue, " {2,}", " ")
        Else
            Return vValue
        End if
    End Function

    Function rReplaceForJson(ByVal vValue As String) As String
        If vValue <> Nothing Then
            vValue = vValue.Replace(vbCr, "").Replace(vbLf, "")
            Return Regex.Replace(vValue, " {2,}", " ")
        Else
            Return vValue
        End if
    End Function

    Function rJustOneSpace(ByVal vValue As String) As String
        If vValue <> Nothing Then
            Return Regex.Replace(vValue, " {2,}", " ")
        Else
            Return vValue
        End if
    End Function

    Function rReplaceSpecialSting(ByVal vValue As String) As String
        If vValue <> Nothing Then
            'vValue = Regex.Replace(vValue, "[^\u0E00-\u0E7Fa-zA-Z0-9' .,:;<>(){}_+*/=!@#$%&?\^\-\[\]\'\""\\]|^'|'$|''", " ")
            vValue = Regex.Replace(vValue, "[^\u0E00-\u0E7Fa-zA-Z0-9' .,:;<>(){}_+*/=!@#$%&?\^\-\[\]\'\""\\]", " ")
            vValue = Regex.Replace(vValue, " {2,}", " ") 'replace space > 2 space
            Return vValue
        Else
            Return vValue
        End if
    End Function

    Function rValidJson(ByVal vValue As String) As String
        If vValue <> Nothing Then
            vValue = rReplaceSpecialSting(vValue)
            vValue = vValue.Replace("\", "\\")
            vValue = vValue.Replace("""", "\""")
            vValue = vValue.Replace(vbCr, "").Replace(vbLf, "")
            Return Regex.Replace(vValue, " {2,}", " ")
        Else
            Return vValue
        End if
    End Function

    Function rNullHyphen(ByVal vValue As String) As String
        If vValue Is Nothing Or vValue.Trim() = "" Then
            Return "-"
        Else
            Return vValue
        End if
    End Function

    Public Function rSQLvalueNULL(ByVal vValue As String)
        If vValue Is Nothing Or vValue.Trim() = "" Then
            Return "NULL "
        Else
            Return "'" + vValue + "' "
        End If
    End Function

    Public Function rSQLvalueEmpty(ByVal vValue As String)
        If vValue.Trim() = "valueEmpty" Then
            Return "NULL "
        Else
            Return "'" + vValue + "' "
        End If
    End Function

    Sub myredirect()
        Dim vUrl As String = rGetCurrentUrl()'HttpContext.Current.Request.Url.AbsoluteUri()
        ClientScript.RegisterStartupScript(Page.GetType, "", "window.location = '" + vUrl + "';", True)
    End Sub

    Sub InteruptRefresh()
        'HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        HttpContext.Current.Response.Redirect(rGetCurrentUrl(), True)
    End Sub

    Sub kickDefault(Optional ByVal xError As String = "")
        Dim vError As String = "~/default.aspx"

        If xError <> "" Then
            vError += "?error=" + xError
        End If

        HttpContext.Current.Response.Redirect(vError, True)
    End Sub

    Sub AlertScript(ByVal vText As String)
        Dim page As Page = HttpContext.Current.Handler
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('" & vText & "');", True)
    End Sub

    Sub notIE()
        If HttpContext.Current.Request.Browser.Browser.ToLower() = "ie" Then
            Dim page As Page = HttpContext.Current.Handler
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert('ระบบไม่รองรับการใช้งานด้วยโปรแกรม Internet Explorer กรุณาใช้ Chrome หรือ Firefox');", True)
        End If
    End Sub

    Public Function rRandomKeyGen(ByVal numChars As Integer) As String
        Dim KeyGen As Cls_RandomKeyGenerator
        KeyGen = New Cls_RandomKeyGenerator
        
        KeyGen.KeyLetters = "abcdefghijklmnopqrstuvwxyz"
        KeyGen.KeyNumbers = "0123456789"
        KeyGen.KeyChars = numChars

        Return KeyGen.Generate()
    End Function
#End Region

End Class

