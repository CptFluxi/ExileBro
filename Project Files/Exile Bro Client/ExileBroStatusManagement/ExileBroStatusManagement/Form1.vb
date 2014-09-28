Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports System.Management

Public Class Form1
    Public AccountKey As String
    Public MD5List As String = ""
    Private Declare Function GetWindowRect Lib "user32.dll" (ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    Private Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure
    Public Shared UseragendWindows As String = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0"


    Public Function LoadCharacterList(ByVal AccKey As String)
        Panel2.Controls.Clear()
        Dim RQ As String = Functions.HTTPRequest("http://klayver.pwx.me/common/actions/fetch_characters.php?code=" & AccKey)
        Dim CharacterList() As String = RQ.Split("@")(1).Split("|")
        Dim int0 As Integer = 0
        Dim AddValueINT As Integer = 0
        Dim AccountName As String = RQ.Split("@")(0).Split("#")(0)
        Label1.Text = AccountName
        While int0 < CharacterList.Count

            Dim CharacterStatus As New PictureBox
            CharacterStatus.SizeMode = PictureBoxSizeMode.AutoSize
            CharacterStatus.Image = My.Resources.char_unknown
            CharacterStatus.Location = New Point(21, Val(6) + Val(36 * AddValueINT))

            Dim CharacterLeague As New Label
            CharacterLeague.Location = New Point(286, Val(6) + Val(36 * AddValueINT))
            CharacterLeague.Padding = Label4.Padding



            CharacterLeague.Text = CharacterList(int0).Split(":")(2)



            CharacterLeague.Font = Label4.Font
            CharacterLeague.ForeColor = Label4.ForeColor
            CharacterLeague.Size = New Point(71, 28)

            CharacterLeague.TextAlign = ContentAlignment.TopCenter

            Dim CharacterStore As New PictureBox
            CharacterStore.Location = New Point(368, Val(6) + Val(36 * AddValueINT))
            CharacterStore.Image = My.Resources.store_icon_off
            CharacterStore.SizeMode = PictureBoxSizeMode.AutoSize

            Dim CharacterName As New Label
            CharacterName.Location = New Point(63, Val(6) + Val(36 * AddValueINT))
            CharacterName.Padding = Label9.Padding
            CharacterName.BackColor = Label9.BackColor
            CharacterName.Font = Label9.Font
            CharacterName.ForeColor = Label9.ForeColor

            CharacterName.Text = CharacterList(int0).Split(":")(1)
            CharacterName.Size = New Point(215, 28)

            CharacterStatus.Visible = True
            CharacterName.Visible = True
            CharacterLeague.Visible = True
            CharacterStore.Visible = True

            If CharacterLeague.Text = "Hardcore" Then
                CharacterLeague.BackColor = Label4.BackColor
            ElseIf CharacterLeague.Text = "Beyond" Then
                CharacterLeague.BackColor = Color.FromArgb("226", "106", "106")
            ElseIf CharacterLeague.Text = "Standard" Then
                CharacterLeague.BackColor = Color.FromArgb("149", "165", "166")
            ElseIf CharacterLeague.Text = "Rampage" Then
                CharacterLeague.BackColor = Color.FromArgb("27", "188", "155")
            Else

            End If

            If CharacterList(int0).Split(":").Count = 4 Then
                AddHandler CharacterStore.Click, AddressOf StoreIcon_Click
                CharacterStore.Image = My.Resources.store_icon_grey
                CharacterStore.Name = "id" & CharacterList(int0).Split(":")(3)
            Else
                CharacterStore.Image = My.Resources.store_icon_off
            End If
            If Label10.Text = "ON" Then
                If CharacterStore.Name.Contains("id") Then
                    Panel2.Controls.Add(CharacterStore)
                    Panel2.Controls.Add(CharacterStatus)
                    Panel2.Controls.Add(CharacterName)
                    Panel2.Controls.Add(CharacterLeague)
                    AddValueINT += 1
                End If
            Else
                Panel2.Controls.Add(CharacterStore)
                Panel2.Controls.Add(CharacterStatus)
                Panel2.Controls.Add(CharacterName)
                Panel2.Controls.Add(CharacterLeague)
                AddValueINT += 1
            End If






            int0 += 1
        End While
        Panel2.Refresh()

        Dim ExtraScroll As New Label
        ExtraScroll.Visible = True
        ExtraScroll.Location = New Point(1, Val(1) + Val(36 * AddValueINT))
        Panel2.Controls.Add(ExtraScroll)

        Return True

    End Function

    Private Sub StoreIcon_Click(ByVal sender As Object, ByVal e As EventArgs)
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
            SaveSettings()
            LoadSettings()
        Else
            sender.Text = "ON"
            SaveSettings()
            LoadSettings()
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
                If Functions.GetMD5Validation(p(0).Modules(0).FileName, MD5List) = True Then
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
            POESTATUS.Text = "-"
            POECID.Text = ""
            POESTATUS.ForeColor = Color.DimGray
            POEXY.Text = "Y: 0, X:0"
            POER.Text = "0x0"
        End If
    End Sub

    Private Sub PictureBox17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox17.Click
        LoadCharacterList(AccountKey)
    End Sub

    Private Sub Label10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label10.Click
        If sender.Text = "ON" Then
            sender.Text = "OFF"
            SaveSettings()
            LoadSettings()
        Else
            sender.Text = "ON"
            SaveSettings()
            LoadSettings()
        End If
    End Sub

    Dim OnlineStatus As Integer = 0
    Dim StatusInt As Integer = 0
    Dim OldLocation As String = ""
    Dim PoEProcessValid As Boolean = False

    Private Sub UpdateStatus(ByVal AccountKey As String, ByVal StatusID As Integer)
        CurStatus = StatusID
        Functions.HTTPRequest("http://exilebro.com/common/actions/online_client.php?code=" & AccountKey & "&status=" & StatusID)

    End Sub

    Private Sub StatusTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusTimer.Tick

        'AFK Check
        Dim MouseInfo As String = Cursor.Position.ToString
        If MouseInfo = OldLocation Then
            StatusInt += 1
        Else
            StatusInt = 0
            OldLocation = Cursor.Position.ToString
        End If


        If StatusInt >= 5 And POEMD5.Text = "Valid" Then
            PictureBox5.Image = My.Resources.status_afk_m
            If Not CurStatus = 4 Then
                UpdateStatus(AccountKey, 4)
            End If

        Else
            If POEMD5.Text = "Valid" Then
                PictureBox5.Image = My.Resources.status_online
                If Not CurStatus = 1 Then
                    UpdateStatus(AccountKey, 1)
                End If
            Else
                PictureBox5.Image = My.Resources.status_offline
                If Not CurStatus = 2 Then
                    UpdateStatus(AccountKey, 2)
                End If
            End If

        End If


    End Sub




    Public Class Functions


        Public Shared Function HTTPRequest(ByVal Link As String)
            Dim webReq As HttpWebRequest = HttpWebRequest.Create(New Uri(Link))
            webReq.ContentType = "application/x-www-form-urlencoded"
            webReq.UserAgent = UseragendWindows
            webReq.Method = "GET"
            Dim webRes As HttpWebResponse = webReq.GetResponse()
            Application.DoEvents()
            Dim ResStream As IO.StreamReader = New IO.StreamReader(webRes.GetResponseStream())
            Dim strResponse As String = ResStream.ReadToEnd()
            Return strResponse
        End Function


        Public Shared Function GetMD5Validation(ByVal filePath As String, ByVal MD5List As String)
            Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
            Dim f As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
            f = New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
            md5.ComputeHash(f)
            f.Close()
            Dim hash As Byte() = md5.Hash
            Dim buff As StringBuilder = New StringBuilder
            Dim hashByte As Byte
            For Each hashByte In hash
                buff.Append(String.Format("{0:X2}", hashByte))
            Next
            Dim md5string As String
            md5string = buff.ToString()

            If MD5List.Contains(md5string) Then
                Return True
            End If
            Return False

        End Function

    End Class

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub

    Dim AutoUpdateIn As Integer = 300
    Dim GeneralTime As Integer = 0
    Dim CurTimer As Integer = 0
    Dim CurStatus As Integer = 0
    Private Sub GeneralTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeneralTimer.Tick

        CurTimer = CurTimer + 1
        GeneralTime = AutoUpdateIn - CurTimer


        Label3.Text = "Next Client Update in " & GeneralTime & " seconds."

        If GeneralTime = 0 Then
            UpdateStatus(AccountKey, CurStatus)
            GeneralTime = 300
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MD5List = Functions.HTTPRequest("http://exilebro.com/common/client/md5.ini")
       
    End Sub

    Public Function LoadSettings()
        Dim IniFileLoc As String = Application.StartupPath & "\exilebro_client.ini"
        If System.IO.File.Exists(IniFileLoc) Then
            Dim FileC() As String = System.IO.File.ReadAllText(IniFileLoc).Split("|")
            Dim AccountKey As String = FileC(0)

            Dim StartOnWindows As Boolean = Boolean.Parse(FileC(1))
            Dim LoadCharsWithStore As Boolean = Boolean.Parse(FileC(2))
            Dim DisplayRefreshButton As Boolean = Boolean.Parse(FileC(3))
            Dim SearchUpdatesOnStart As Boolean = Boolean.Parse(FileC(4))

            Me.AccountKey = AccountKey

            If StartOnWindows = True Then
                My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "Exile Bro Client", Application.ExecutablePath)
                Label20.Text = "ON"
            Else
                My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "Exile Bro Client", "")
                Label20.Text = "OFF"
            End If

            If LoadCharsWithStore = True Then
                Label10.Text = "ON"
            Else
                Label10.Text = "OFF"
            End If

            If DisplayRefreshButton = True Then
                Me.PictureBox17.Visible = True
                Label37.Text = "ON"
            Else
                Label37.Text = "OFF"
                Me.PictureBox17.Visible = False
            End If

            If SearchUpdatesOnStart = True Then
                Label19.Text = "ON"
            Else
                Label19.Text = "OFF"
            End If

        Else
            Application.Exit()
        End If

        Return True
    End Function

    Public Function SaveSettings(Optional ByVal SaveAccountKey As Boolean = True)
        Dim IniFileLoc As String = Application.StartupPath & "\exilebro_client.ini"

        Dim StartOnWindows As Boolean = False
        Dim LoadCharsWithStore As Boolean = False
        Dim DisplayRefreshButton As Boolean = False
        Dim SearchUpdatesOnStart As Boolean = False

        If Label20.Text = "ON" Then
            StartOnWindows = True
        End If

        If Label10.Text = "ON" Then
            LoadCharsWithStore = True
        End If

        If Label37.Text = "ON" Then
            DisplayRefreshButton = True
        End If

        If Label19.Text = "ON" Then
            SearchUpdatesOnStart = True
        End If

        If SaveAccountKey = True Then
            System.IO.File.WriteAllText(IniFileLoc, AccountKey & "|" & StartOnWindows & "|" & LoadCharsWithStore & "|" & DisplayRefreshButton & "|" & SearchUpdatesOnStart)
        ElseIf SaveAccountKey = False Then
            System.IO.File.WriteAllText(IniFileLoc, "x|" & StartOnWindows & "|" & LoadCharsWithStore & "|" & DisplayRefreshButton & "|" & SearchUpdatesOnStart)
            
        End If


        Return True
    End Function

    Private Sub Label20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label20.Click
        If sender.Text = "ON" Then
            sender.Text = "OFF"
            SaveSettings()
            LoadSettings()
        Else
            sender.Text = "ON"
            SaveSettings()
            LoadSettings()
        End If
    End Sub

    Private Sub Label37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label37.Click
        If sender.Text = "ON" Then
            sender.Text = "OFF"
            SaveSettings()
            LoadSettings()
        Else
            sender.Text = "ON"
            SaveSettings()
            LoadSettings()
        End If
    End Sub


    Private Sub Label30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label30.Click
        SaveSettings(False)
        Process.Start(Application.ExecutablePath)
        Application.Exit()
    End Sub
End Class
