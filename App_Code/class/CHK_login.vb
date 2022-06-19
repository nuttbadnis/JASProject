Imports System.Data
Imports System.Data.OleDb


Namespace WebApplication2

Public Module CHK_login
    Dim connectDB As String = DBConnect.getStrDBConnect()
    Function verifyLogin(ByVal email As String) As Boolean
        Dim myconn As New OleDbConnection(connectDB)
        myconn.Open()

        Dim mycommand As OleDbCommand
        Dim sql As String
        Dim result As Boolean

        sql = "select * from m00860 where f02 = '" & email & "'"
        mycommand = New OleDbCommand(sql, myconn)
        Dim Ans_ID As OleDbDataReader = mycommand.ExecuteReader()

        If Ans_ID.Read Then
            result = True
        Else
            result = False
        End If


        Ans_ID.Close()
        mycommand.Dispose()
        myconn.Close()

        Return result
    End Function

  

End Module

End Namespace
