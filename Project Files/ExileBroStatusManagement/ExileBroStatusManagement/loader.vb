Imports System.Threading
Public NotInheritable Class loader

    Private Sub loader_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub

    Dim ClientAPI As New ClientAPIStructure
    Dim ClientFunctions As New Client
    Dim CurrentVersion As String = "0.2a" 'Current Client Version is set manually, own .exe MD5 Check is on its way


    Private Sub loader_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Establish Standart API Enviroment
        ClientAPI.SetClient("CurrentVersion", CurrentVersion)
        ClientAPI.SetClient("UserAgend", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0")
        ClientAPI.SetClient("UserSettingsPath", Environ$("appdata") & "\ExileBro")
        ClientAPI.SetClient("UserSettingsFilename", "exilebro_client.ini")
        ClientFunctions.SearchForRequiredFiles()
        '!IMPORTANT

        'Load Client Version Informations
        ClientFunctions.LoadVersionInfo(ClientAPI.ReturnAPIEntrys)

        'Load Settings & Establish Settings
        ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
        ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)


        If ClientAPI.GetClient("Settings:SearchUpdatesOnStart") = True Then
            ClientFunctions.CheckForUpdates(ClientAPI.ReturnAPIEntrys)
        End If

        If ClientFunctions.CheckLogin(ClientAPI.GetClient("AccountKey")) = False Then
            'False Login
            Me.TopMost = False
            Me.WindowState = FormWindowState.Minimized
            Form2.Show()
            ClientFunctions.BringOnTop(Form2)

        Else
            'True Login
            Me.TopMost = False
            Me.WindowState = FormWindowState.Minimized
            ClientFunctions.LoadClient(Form1.Panel2, ClientAPI.ReturnAPIEntrys)
            ClientFunctions.BringOnTop(Form1)
        End If

        'Application.Exit()

    End Sub


    

  

  
End Class
