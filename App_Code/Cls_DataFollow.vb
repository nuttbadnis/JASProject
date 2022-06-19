Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.IO

Public Class Cls_DataFollow

    Private Function GetConnectionString() As String
        Dim strConn As String
        strConn = Configuration.WebConfigurationManager.ConnectionStrings("POS105FollowConnectionString").ConnectionString.ToString
        Return strConn
    End Function
    Public Function DecryDat(ByVal DecryTxt As String) As String
        Dim txtDecry As String = ""
        Try
            Dim Buff1 As Char
            Dim Buff2 As Char
            Dim TxtBuff1 As String = ""
            Dim TxtBuff2 As String = ""
            Dim i As Integer
            Dim DecryCode As Integer
            If Trim(DecryTxt) <> "" Then
                DecryCode = Asc(Right(DecryTxt, 1)) - Asc(Mid(DecryTxt, 2, 1))
                For i = Len(DecryTxt) - 1 To 1 Step -1
                    TxtBuff1 &= Mid(DecryTxt, i, 1)
                Next i

                For i = 1 To Len(TxtBuff1)
                    Buff1 = Mid(TxtBuff1, i, 1)
                    Buff2 = Nothing
                    Buff2 = Chr(Asc(Buff1) - DecryCode)
                    TxtBuff2 &= Buff2
                Next i
                txtDecry = TxtBuff2
            End If
        Catch ex As Exception
        End Try
        Return txtDecry
    End Function

    Public Function EncryDat(ByVal EncryTxt As String) As String
        Dim txtEncry As String = ""
        Try
            Dim EncryCode As Integer
            Dim Buff1 As Char
            Dim Buff2 As Char
            Dim TxtBuff1 As String = ""
            Dim TxtBuff2 As String = ""
            Dim i As Integer
            Randomize()
            EncryCode = Int((9 * Rnd()) + 1)
            For i = 1 To Len(EncryTxt)
                Buff1 = Mid(EncryTxt, i, 1)
                Buff2 = Nothing
                Buff2 = Chr(Asc(Buff1) + EncryCode)
                TxtBuff1 &= Buff2
            Next i
            For i = Len(TxtBuff1) To 1 Step -1
                TxtBuff2 &= Mid(TxtBuff1, i, 1)
            Next i
            EncryCode = Asc(Mid(TxtBuff2, 2, 1)) + EncryCode
            txtEncry = TxtBuff2 & Chr(EncryCode)
        Catch ex As Exception
        End Try
        Return txtEncry
    End Function

#Region "DataNonTransaction"

    Public Function GetDataTable(ByVal QryStr As String, Optional ByVal TableName As String = "DataTalble1") As DataTable
        Dim objDA As New SqlDataAdapter(QryStr, GetConnectionString)
        Dim objDT As New DataTable(TableName)
        Try
            objDA.Fill(objDT)
        Catch ex As Exception
            Err.Raise(Err.Number, , ex.Message)
        End Try
        Return objDT
    End Function
    Public Function ExecuteNonQuery(ByVal QryStr As String) As Integer
        Dim intReturn As Integer
        Dim objConn As New SqlConnection(GetConnectionString)
        Dim objCmd As SqlCommand
        Try
            objCmd = New SqlCommand(QryStr, objConn)
            objConn.Open()
            intReturn = objCmd.ExecuteNonQuery()
        Catch ex As SqlException
            intReturn = -1
            Err.Raise(Err.Number, , ex.Message)
        Finally
            objConn.Close()
        End Try
        Return intReturn
    End Function
    Public Sub SetDropDownList(ByRef ddl As DropDownList, ByVal CmdText As String, ByVal TextField As String, ByVal ValueField As String, Optional ByVal Row0 As String = Nothing)
        Try
            Dim DT As DataTable = GetDataTable(CmdText)
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
#End Region

#Region "DataTransaction"
    Private pvObjConn As SqlConnection
    Private pvObjTran As SqlTransaction
    Private Sub OpenConnection()
        pvObjConn = New SqlConnection(Me.GetConnectionString)
        pvObjConn.Open()
    End Sub
    Private Sub CloseConnection()
        If pvObjConn.State = ConnectionState.Open Then pvObjConn.Close()
    End Sub
    Public Function BeginTran() As Boolean
        Try
            Me.OpenConnection()
            pvObjTran = pvObjConn.BeginTransaction
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function Commit() As Boolean
        Try
            If pvObjConn.State = ConnectionState.Open Then pvObjTran.Commit()
            Return True
        Catch ex As Exception
            pvObjTran.Rollback()
            Return False
        Finally
            Me.CloseConnection()
        End Try
    End Function
    Public Function RollBack() As Boolean
        Try
            pvObjTran.Rollback()
            Return True
        Catch ex As Exception
            Return False
        Finally
            Me.CloseConnection()
        End Try
    End Function
    Public Function ExecuteNonQuery(ByVal QryStr As String, ByVal Transaction As Boolean) As Integer
        If (Transaction) Then
            Dim objComm = New SqlCommand(QryStr, pvObjConn, pvObjTran)
            Return objComm.ExecuteNonQuery()
        Else
            Return Me.ExecuteNonQuery(QryStr)
        End If
    End Function
    Public Function ExecuteScalar(ByVal QryStr As String) As Integer
        Dim strReturn As String
        Dim objConn As New SqlConnection(GetConnectionString)
        Dim objCmd As SqlCommand
        Try
            objCmd = New SqlCommand(QryStr, objConn)
            objConn.Open()
            strReturn = objCmd.ExecuteScalar()
        Catch ex As SqlException
            strReturn = "null"
            Err.Raise(Err.Number, , ex.Message)
        Finally
            objConn.Close()
        End Try
        Return strReturn
    End Function
#End Region

#Region "Utility"
    Public Function CDateText(ByVal TDate As String) As String
        Dim TDate2 As String
        Try
            TDate2 = CStr(TDate)
            If TDate = "  /  /    " Or TDate = "__/__/____" Then
                CDateText = ""
            ElseIf IsDate(TDate) = False Then
                CDateText = ""
            Else
                CDateText = Mid(TDate, 7, 4) + "/" + Mid(TDate, 4, 2) + "/" + Mid(TDate, 1, 2)
            End If
        Catch ex As Exception
            CDateText = TDate
        End Try
    End Function
    'Public Function GetDateNow() As String
    '    Try
    '        Dim vDate As String
    '        Dim curCulture As Globalization.CultureInfo
    '        curCulture = Thread.CurrentThread.CurrentCulture
    '        Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US")
    '        vDate = Format(Now, "yyyy/MM/dd")
    '        If Mid(vDate, 1, 4) > 2500 Then
    '            vDate = Mid(vDate, 1, 4) - 543 + "/" + Mid(vDate, 6, 2) + "/" + Mid(vDate, 9, 2)
    '        End If
    '        Thread.CurrentThread.CurrentCulture = curCulture
    '        Return vDate
    '    Catch ex As Exception
    '        Return ""
    '    End Try
    'End Function
    Public Function Get_TextDate(ByVal DStr) As String
        Dim Str2$
        Str2 = (DStr)
        If Str2 = "" Then
            Get_TextDate = "__/__/____"
        ElseIf Len(Str2) < 10 Then
            Get_TextDate = "__/__/____"
        Else
            Get_TextDate = Mid(Str2, 9, 2) + "/" + Mid(Str2, 6, 2) + "/" + Mid(Str2, 1, 4)
        End If
    End Function
    Public Function rpQuoted(ByVal Str As String) As String
        Try
            rpQuoted = RTrim(LTrim(Replace(Str, Chr(39), Chr(39) & Chr(39))))
        Catch ex As Exception
            rpQuoted = Str
        End Try
    End Function

#End Region

End Class

