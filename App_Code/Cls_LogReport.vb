Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Web.UI.Page
Imports System.Web.HttpServerUtility
Imports System.Net.IPAddress

Public Class Cls_LogReport
    Dim C As New Cls_Data

    Public Function LogReport(ByVal ReportNumber As String, ByVal ReportName As String, ByVal Parameter As String, ByVal IP As String, ByVal Login As String) As Integer
        Dim Check As Integer
        Try
            'C.ExecuteNonQuery("Insert into ReportLog(ReportNumber,ReportName,rDate,Parameter,IPAddress,Login) values('" + ReportNumber + "','" + ReportName + "',getdate(),'" + Parameter + "','" + IP + "','" + Login + "')")
            Check = 0
        Catch ex As Exception
            Check = -1
            Err.Raise(Err.Number, , ex.Message)
        End Try
        Return Check
    End Function


    Public Function LogReport(ByVal ReportNumber As String, ByVal ReportName As String, ByVal Parameter As String, ByVal IP As String, ByVal Login As String, ByVal rDate As String, ByVal fDate As String) As Integer
        Dim Check As Integer
        Try
            If fDate = "2001-01-01 00:00:00.000" Then
                C.ExecuteNonQuery("Insert into ReportLog(ReportNumber,ReportName,rDate,Parameter,IPAddress,Login) values('" + ReportNumber + "','" + ReportName + "','" + Format(CDate(rDate), "yyyy-MM-dd HH:mm:ss.000") + "','" + Parameter + "','" + IP + "','" + Login + "')")
                Check = 0
            Else
                C.ExecuteNonQuery("Insert into ReportLog(ReportNumber,ReportName,rDate,Parameter,IPAddress,Login,FinishDate) values('" + ReportNumber + "','" + ReportName + "','" + Format(CDate(rDate), "yyyy-MM-dd HH:mm:ss.000") + "','" + Parameter + "','" + IP + "','" + Login + "','" + Format(CDate(fDate), "yyyy-MM-dd HH:mm:ss.000") + "')")
		'C.ExecuteNonQuery("Insert into ReportLog(ReportNumber,ReportName,rDate,Parameter,IPAddress,Login,FinishDate) values('" + ReportNumber + "','" + ReportName + "','" + rDate + "','" + Parameter + "','" + IP + "','" + Login + "','" + Format(Now(), "yyyy-MM-dd HH:mm:ss.000") + "')")
                Check = 0
            End If

        Catch ex As Exception
            Check = -1
            Err.Raise(Err.Number, , ex.Message)
        End Try
        Return Check
    End Function

    Public Function CDateText(ByVal TDate As String) As String
        Dim TDate2 As String
        Try
            TDate2 = CStr(TDate)
            If TDate = "  /  /    " Or TDate = "__/__/____" Then
                CDateText = ""

            Else
                CDateText = Mid(TDate, 7, 4) + "/" + Mid(TDate, 4, 2) + "/" + Mid(TDate, 1, 2)
            End If
        Catch ex As Exception
            CDateText = TDate
        End Try
    End Function
End Class
