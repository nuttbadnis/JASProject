Imports System.IO

Public Class Cls_UploadFile
    Inherits System.Web.UI.Page

    Public Function rAutoCreateDirectory() As String
        Dim vUploadPath As String = "F:\"
        vUploadPath &= rGetPathParent() & "File\upload" 
        vUploadPath &= "\" & DateTime.Now.ToString("yyyy")
        vUploadPath &= "\" & DateTime.Now.ToString("MM")

        IF Not Directory.Exists(vUploadPath) then
            Directory.CreateDirectory(vUploadPath)

            HttpContext.Current.Response.Write("auto create directory " + vUploadPath)
        Else
            HttpContext.Current.Response.Write("exist directory " + vUploadPath)
        End IF
    End Function

    Public Function rUploadFile(ByVal vInput As String, ByVal vPrefix As String) As String
        Dim vFileUpload As HttpPostedFile = HttpContext.Current.Request.Files(vInput)
        Dim vFilePath AS String = ""
        Dim vFileType As String
        Dim vFileName AS String
        Dim vTempFile As String = ""

        If vFileUpload.FileName <> "" Then
            vFileName = System.IO.Path.GetFileName(vFileUpload.FileName)

            If vFileName.Length >= 1 Then
                vTempFile = vFileName    ' FileName
                vFileType = System.IO.Path.GetExtension(vFileName) 
            End If

            vFilePath = vPrefix 
            vFilePath += DateTime.Now.ToString("_yyMMdd_HHmmss")
            vFilePath += vFileType

            vFileUpload.SaveAs(Server.MapPath("upload/" & vTempFile))

            Dim TheFile As FileInfo = New FileInfo(Server.MapPath("upload/" & vTempFile))

            If TheFile.Exists Then
                System.IO.File.Move(Server.MapPath("upload/" & vTempFile), Server.MapPath("upload/" & vFilePath))  'Move File (à»ÅÕèÂ¹ª×èÍä¿Åì)
            Else
                vFilePath = ""

                Throw New FileNotFoundException()
            End If
        End If

        Return vFilePath
    End Function

    Public Function rUploadFileDriveF(ByVal vInput As String, ByVal vPrefix As String) As String
        Dim vFileUpload As HttpPostedFile = HttpContext.Current.Request.Files(vInput)
        Dim Rename AS String
        Dim vFileType As String
        Dim vFileName AS String
        Dim vTempFile As String = ""
        Dim vTempPath As String = Server.MapPath("temp\")

        'Dim vFilePath As String = "F:\FollowRequestFile\upload" 
        Dim vUploadPath As String = "F:\"
        vUploadPath &= rGetPathParent() & "File\upload" 
        vUploadPath &= "\" & DateTime.Now.ToString("yyyy")
        vUploadPath &= "\" & DateTime.Now.ToString("MM")

        IF Not Directory.Exists(vTempPath) then
            Directory.CreateDirectory(vTempPath)
        End IF

        If vFileUpload.FileName <> "" Then
            IF Not Directory.Exists(vUploadPath) then
                Directory.CreateDirectory(vUploadPath)
            End IF

            vUploadPath &= "\"

            vFileName = System.IO.Path.GetFileName(vFileUpload.FileName)

            If vFileName.Length >= 1 Then
                vTempFile = vFileName    'FileName
                vFileType = System.IO.Path.GetExtension(vFileName) 
            End If

            Rename = vPrefix 
            Rename &= DateTime.Now.ToString("_yyMMdd_HHmmss")
            Rename &= vFileType

            vFileUpload.SaveAs(Server.MapPath("temp\" & vTempFile))

            Dim TheFile As FileInfo = New FileInfo(vTempPath & vTempFile)
            If TheFile.Exists Then
                System.IO.File.Move(vTempPath & vTempFile, vUploadPath & Rename)  'Move File
            Else
                Rename = ""

                Throw New FileNotFoundException()
            End If
        End If

        Return Rename
    End Function

    Public Function rUploadFileMutivalue(ByVal vInput As String, ByVal vPrefix As String) As String()
        Dim vFileUpload As HttpPostedFile = HttpContext.Current.Request.Files(vInput)
        Dim Rename AS String = ""
        Dim vFileType As String
        Dim vFileName AS String
        Dim vTempFile As String = ""

        'Dim vFilePath As String = "FollowRequestFile\upload" 
        Dim vFilePath As String = rGetPathParent() & "File\upload" 
        vFilePath &= "\" & DateTime.Now.ToString("yyyy")
        vFilePath &= "\" & DateTime.Now.ToString("MM")

        Dim vUploadPath As String = "F:\" & vFilePath

        IF Not Directory.Exists(vUploadPath) then
            Directory.CreateDirectory(vUploadPath)
        End IF

        vUploadPath &= "\"

        If vFileUpload.FileName <> "" Then
            vFileName = System.IO.Path.GetFileName(vFileUpload.FileName)

            If vFileName.Length >= 1 Then
                vTempFile = vFileName    ' FileName
                vFileType = System.IO.Path.GetExtension(vFileName) 
            End If

            Rename = vPrefix 
            Rename &= DateTime.Now.ToString("_yyMMdd_HHmmss")
            Rename &= vFileType

            vFileUpload.SaveAs(Server.MapPath("temp/" & vTempFile))

            'Dim TheFile As FileInfo = New FileInfo(MapPath(".") & "\" & "temp\" & vTempFile)
            Dim TheFile As FileInfo = New FileInfo(Server.MapPath("temp/" & vTempFile))
            If TheFile.Exists Then
                'System.IO.File.Move(MapPath(".") & "\" & "temp\" & vTempFile, vUploadPath & Rename)  'Move File
                System.IO.File.Move(Server.MapPath("temp/" & vTempFile), vUploadPath & Rename)  'Move File
            Else
                Rename = ""

                Throw New FileNotFoundException()
            End If
        End If

        'vFilePath = "https://posbcs.triplet.co.th/" & vFilePath.Replace("\","/") & "/"
        vFilePath = rGetHost & "/" & vFilePath.Replace("\","/") & "/"

        Return New String() {vUploadPath, vFilePath, Rename}
    End Function

    Public Function rGetPathParent() As String
        'example url = https://posbcs.triplet.co.th/FlowDemo/testupload.aspx
        'Request.Url.AbsolutePath = /FlowDemo/testupload.aspx
        'vPath(1) = FlowDemo

        Dim vPath As Array = HttpContext.Current.Request.Url.AbsolutePath.Split("/")
        Return vPath(1)
    End Function

    Public Function rGetHost() As String
        Return HttpContext.Current.Request.Url.Host
    End Function

End Class

