Public NotInheritable Class loader

    Private Sub loader_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub


    Public Function CheckLogin(ByVal AccKey As String)
        Dim Request As String = Form1.Functions.HTTPRequest("http://klayver.pwx.me/common/actions/fetch_characters.php?code=" & AccKey)
        If Request.Contains("ERROR:invalid_code") Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared VersionNumber As String = "0.1"
    Private Sub loader_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim IniFileLoc As String = Application.StartupPath & "\exilebro_client.ini"
        If System.IO.File.Exists(IniFileLoc) Then
            Try
                Dim FileC() As String = System.IO.File.ReadAllText(IniFileLoc).Split("|")
                Dim AccountK As String = FileC(0)
                Dim SearchUpdates As String = FileC(4)
                Dim NewestVersion As String = Form1.Functions.HTTPRequest("http://exilebro.com/common/client/c_v.ini").ToString.Split("|")(0)
                If SearchUpdates = True And Not VersionNumber = NewestVersion Then
                    Dim ProcessProperties As New ProcessStartInfo
                    Try
                        ProcessProperties.FileName = Application.StartupPath & "\ExileBro Client Update.exe"
                        ProcessProperties.Arguments = Application.ExecutablePath & "|" & VersionNumber
                        Dim myProcess As Process = Process.Start(ProcessProperties)
                    Catch ex As Exception
                        Application.Exit()
                        Exit Sub
                    End Try
                    Application.Exit()
                    Exit Sub
                End If

                If CheckLogin(AccountK) = False Then
                    Form2.Show()
                Else
                    Form1.LoadSettings()
                    Form1.LoadCharacterList(AccountK)
                    Form1.Show()
                End If
            Catch ex As Exception
                Form2.Show()
            End Try
        Else
            Form1.SaveSettings()
            Form2.Show()
            Me.Hide()
        End If
        Me.Hide()
        Me.Visible = False
    End Sub
End Class
