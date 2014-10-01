Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports System.Management
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class Form1


    Private Declare Function GetWindowRect Lib "user32.dll" (ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    Private Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure

    Dim AutoUpdateIn As Integer = 300
    Dim GeneralTime As Integer = 0
    Dim CurTimer As Integer = 0
    Dim CurStatus As Integer = 0


    Dim ClientAPI As New ClientAPIStructure
    Dim ClientFunctions As New Client
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Set List of Valid MD5 Hashes of Path of Exile
        ClientAPI.SetClient("MD5List", ClientFunctions.HTTPRequest("http://exilebro.com/common/client/md5.ini"))

        'Create Client Enviroment
        
        Form2.Hide()
        loader.Hide()
        loader.Visible = False
        loader.WindowState = FormWindowState.Minimized
        loader.Hide()
        loader.Opacity = 0
        loader.ShowInTaskbar = False
        Label17.Text = ClientAPI.GetClient("NewestVersion")
        Label18.Text = ClientAPI.GetClient("CurrentVersion")

    End Sub

    Public Sub StoreIcon_Click(ByVal sender As Object, ByVal e As EventArgs)
        Process.Start("http://klayver.pwx.me/currency/shops/manage/" & sender.name.ToString.Replace("id", "") & "/")
    End Sub

    Private Sub PictureBox10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox10.Click
        Panel1.Visible = False
        Panel3.Visible = True
        PictureBox9.Image = My.Resources.dashboard
        PictureBox10.Image = My.Resources.settings_down
    End Sub

    Private Sub PictureBox9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox9.Click
        Panel1.Visible = True
        Panel3.Visible = False
        PictureBox9.Image = My.Resources.dashboard_active
        PictureBox10.Image = My.Resources.panel_settings
    End Sub

    Private Sub Label19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label19.Click
        If sender.Text = "ON" Then
            sender.Text = "OFF"
            ClientAPI.SetClient("Settings:SearchUpdatesOnStart", False)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)
        Else
            sender.Text = "ON"
            ClientAPI.SetClient("Settings:SearchUpdatesOnStart", True)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClientCheck.Tick
        Dim ProcessNameX As String = ""
        Try
            If Not Process.GetProcessesByName("PathOfExileSteam").Length = 0 Then
                ProcessNameX = "PathOfExileSteam"
                POEMD5.Text = "Invalid"
            End If
            If Not Process.GetProcessesByName("PathOfExile").Length = 0 Then
                ProcessNameX = "PathOfExile"
                POEMD5.Text = "Invalid"
            End If

        Catch ex As Exception

        End Try

        If Not ProcessNameX = "" Then
            Try
                Dim p() As Process = Process.GetProcessesByName(ProcessNameX)
                If ClientFunctions.GetMD5Validation(p(0).Modules(0).FileName, ClientAPI.GetClient("MD5List")) = True Then
                    Dim windowsizeinfo As New RECT
                    POECID.Text = p(0).Id
                    POEMD5.Text = "Valid"
                    POEMD5.ForeColor = Color.Green
                    GetWindowRect(p(0).MainWindowHandle, windowsizeinfo)
                    POEXY.Text = "Y:" & windowsizeinfo.top & ", " & "X:" & windowsizeinfo.bottom
                    'windowsizeinfo.right - windowsizeinfo.left POEWindowResolutionInfo
                    POER.Text = windowsizeinfo.right - windowsizeinfo.left & "x" & windowsizeinfo.bottom - windowsizeinfo.top
                    POESTATUS.Text = "Running"
                    POESTATUS.ForeColor = Color.Green

                Else
                    POEMD5.Text = "Invalid"
                    POESTATUS.Text = ""
                    POECID.Text = ""
                    POESTATUS.ForeColor = Color.DimGray
                    POEMD5.ForeColor = Color.Red
                    POEXY.Text = "Y: 0, X:0"
                    POER.Text = "0x0"

                End If
            Catch ex As Exception
            End Try
        Else
            POEMD5.Text = ""
            POESTATUS.Text = ""
            POECID.Text = ""
            POESTATUS.ForeColor = Color.DimGray
            POEXY.Text = "Y: 0, X:0"
            POER.Text = "0x0"
        End If
    End Sub

    Private Sub PictureBox17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox17.Click
        Try

        Catch ex As Exception
        End Try
    End Sub

    Private Sub Label10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label10.Click
        If sender.Text = "ON" Then
            sender.Text = "OFF"
            ClientAPI.SetClient("Settings:LoadOnlyCharsWithStore", False)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)
        Else
            sender.Text = "ON"
            ClientAPI.SetClient("Settings:LoadOnlyCharsWithStore", True)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)
        End If
    End Sub


    Dim StatusInt As Integer = 0
    Dim OldLocation As String = ""
    Private Sub UpdateStatus(ByVal AccountKey As String, ByVal StatusID As Integer)
        CurStatus = StatusID
        ClientFunctions.HTTPRequest("http://exilebro.com/common/actions/online_client.php?code=" & AccountKey & "&status=" & StatusID)
        'MAKE EXTERNAL CHECK FUNCTION
    End Sub

    Private Sub StatusTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusTimer.Tick
        'NEEDS OVERWORK
        Dim MouseInfo As String = Cursor.Position.ToString
        If MouseInfo = OldLocation Then
            StatusInt += 1
        Else
            StatusInt = 0
            OldLocation = Cursor.Position.ToString
        End If

        If StatusInt >= 600 And POEMD5.Text = "Valid" Then
            PictureBox5.Image = My.Resources.status_afk_m
            If Not CurStatus = 4 Then
                UpdateStatus(ClientAPI.GetClient("AccountKey"), 4)
            End If

        Else
            If POEMD5.Text = "Valid" Then
                PictureBox5.Image = My.Resources.status_online
                If Not CurStatus = 1 Then
                    UpdateStatus(ClientAPI.GetClient("AccountKey"), 1)
                End If
            Else
                PictureBox5.Image = My.Resources.status_offline
                If Not CurStatus = 2 Then
                    UpdateStatus(ClientAPI.GetClient("AccountKey"), 2)
                End If
            End If

        End If


    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub

   
    Dim SystemTray As Boolean = False
    Private Sub GeneralTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeneralTimer.Tick

        If SystemTray = False And Me.WindowState = FormWindowState.Minimized And Label37.Text = "ON" Then
            SystemTray = True
            Me.ShowInTaskbar = False
            Me.Hide()
        End If

        CurTimer = CurTimer + 1
        GeneralTime = AutoUpdateIn - CurTimer


        Label3.Text = "Next Client Update in " & GeneralTime & " seconds."

        If GeneralTime <= 0 Then
            UpdateStatus(ClientAPI.GetClient("AccountKey"), CurStatus)
            CurTimer = 0
            GeneralTime = 0
        End If

    End Sub
    Dim SystemTrayEnabled As Boolean = False
    
    Private Sub Label20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label20.Click
        If sender.Text = "ON" Then
            sender.Text = "OFF"
            ClientAPI.SetClient("Settings:LoadOnStartUp", False)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)
        Else
            sender.Text = "ON"
            ClientAPI.SetClient("Settings:LoadOnStartUp", True)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)
        End If
    End Sub

    Private Sub Label37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label37.Click
        If sender.Text = "ON" Then
            sender.Text = "OFF"
            ClientAPI.SetClient("Settings:SystemTrayActive", False)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)
        Else
            sender.Text = "ON"
            ClientAPI.SetClient("Settings:SystemTrayActive", True)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.LoadSettings(ClientAPI.ReturnAPIEntrys)
            ClientFunctions.EstablishSettings(ClientAPI.ReturnAPIEntrys)
        End If
    End Sub

    Private Sub Label30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label30.Click

        'RESTART CLIENT WITHOUT KEY
        ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys, False)
        
        Process.Start(Application.ExecutablePath)
        Application.Exit()
    End Sub

    Private Sub Label29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label29.Click
        If ClientFunctions.CheckForUpdates(ClientAPI.ReturnAPIEntrys) = False Then
            MsgBox("Your ExileBro Client has the latest version.", MsgBoxStyle.Information, "")
        End If
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        SystemTray = False
        Me.WindowState = FormWindowState.Normal
        Me.ShowInTaskbar = True
        Me.Show()
        ClientFunctions.BringOnTop(Me)
    End Sub

    Private Sub PictureBox17_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox17.MouseHover
        PictureBox17.Image = My.Resources.btn_refresh
    End Sub
    Private Sub PictureBox17_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox17.MouseLeave
        PictureBox17.Image = My.Resources.btn_refresh_grey
    End Sub
    Private Sub PictureBox17_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox17.MouseMove
        PictureBox17.Image = My.Resources.btn_refresh
    End Sub
End Class
