Imports System.Management
Public Class ClientAPIStructure

    Public Shared Client As New System.Collections.Hashtable

    Public Function GetClient(ByVal ID As String)
        Try
            Return Client(ID)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function SetClient(ByVal ID As String, ByVal Content As String)
        Try
            Client(ID) = Content
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function ReturnAPIEntrys()
        Return Client
    End Function

    Public Function ImportAPIEntry(ByVal APIHashtable As Hashtable)
        Client = APIHashtable
        Return True
    End Function
End Class
