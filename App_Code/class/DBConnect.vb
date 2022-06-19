Imports System.Data.SqlClient


Namespace WebApplication2

Public Module DBConnect
    Function getStrDBConnect() As String
        Dim connstatement As String
            ' connstatement = ConfigurationSettings.AppSettings("Conn")
            connstatement = ConfigurationManager.AppSettings("Conn")
        Return connstatement
    End Function

    Private Sub ExecuteSQL(ByVal cmd As SqlCommand, ByVal sSQL As String)

        cmd.CommandText = sSQL
        cmd.ExecuteNonQuery()

    End Sub

End Module

End Namespace
