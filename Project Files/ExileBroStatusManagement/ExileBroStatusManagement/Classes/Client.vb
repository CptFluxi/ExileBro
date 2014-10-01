Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports System.Management
Imports System.Drawing
Imports System.Drawing.Drawing2D
Public Class Client

    Dim UserAgend As String = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0"

    Public Function CheckForUpdates(ByVal APIHashtable As Hashtable)
        Dim NewestVersion As String = HTTPRequest("http://exilebro.com/common/client/c_v.ini").ToString.Split("|")(0)
        If Not APIHashtable("CurrentVersion") = NewestVersion Then
            Try
                Dim UpdaterFilePath As String = Environ$("appdata") & "\ExileBro\ExileBro Updater.exe"
                If Not System.IO.File.Exists(UpdaterFilePath) Then
                    My.Computer.Network.DownloadFile("http://exilebro.com/common/client/updater.exe", UpdaterFilePath)
                End If
                Dim PP As New ProcessStartInfo
                PP.FileName = UpdaterFilePath
                PP.Arguments = Application.ExecutablePath & "|" & APIHashtable("CurrentVersion")
                Dim myProcess As Process = Process.Start(PP)
                Try
                    Application.Exit()
                    Exit Function
                Catch ex As Exception
                End Try
            Catch ex As Exception
                MsgBox("There was a error while downloading the updater of the ExileBro Client", MsgBoxStyle.Critical, "")
            End Try
        End If
        Return False
    End Function

    Public Function SearchForRequiredFiles()
        Try
            Dim UpdaterFilePath As String = Environ$("appdata") & "\ExileBro\ExileBro Updater.exe"
            If Not System.IO.File.Exists(UpdaterFilePath) Then
                My.Computer.Network.DownloadFile("http://exilebro.com/common/client/updater.exe", UpdaterFilePath)
            End If
        Catch ex As Exception

        End Try
        Return True
    End Function
    'Load the Client for user usage including characters and username from ExileBro.com Account Key
    Public Function LoadClient(ByVal CharacterListPanel As Panel, ByVal APIHashtable As Hashtable)

        CharacterListPanel.Controls.Clear()
        Dim RQ As String = HTTPRequest("http://klayver.pwx.me/common/actions/fetch_characters.php?code=" & APIHashtable("AccountKey"))
        Dim CharacterList() As String = RQ.Split("@")(1).Split("|")
        Dim int0 As Integer = 0
        Dim AddValueINT As Integer = 0
        Dim AccountName As String = RQ.Split("@")(0).Split("#")(0)
        Dim AccountImageName As String = RQ.Split("@")(0).Split("#")(1)
        Dim AccountImageLink As String = "http://exilebro.com/img/avatar/" & AccountImageName
        Dim AccountImagePath As String = Environ$("AppData") & "\ExileBro\" & AccountImageName

        Try
            System.IO.File.Delete(AccountImagePath)
            My.Computer.Network.DownloadFile(AccountImageLink, AccountImagePath)
            Dim OriginalImg As Image = System.Drawing.Image.FromFile(AccountImagePath)
            Dim Resized As Image = ResizeImage(OriginalImg, New Point(40, 40))
            Form1.UserImage.BackgroundImage = Resized
            System.IO.File.Delete(AccountImagePath)
        Catch ex As Exception
        End Try

        Form1.Label1.Text = AccountName
        While int0 < CharacterList.Count

            Dim CharacterStatus As New PictureBox
            CharacterStatus.SizeMode = PictureBoxSizeMode.AutoSize
            CharacterStatus.Image = My.Resources.char_unknown
            CharacterStatus.Location = New Point(21, Val(6) + Val(36 * AddValueINT))

            Dim CharacterLeague As New Label
            CharacterLeague.Location = New Point(286, Val(6) + Val(36 * AddValueINT))
            CharacterLeague.Padding = Form1.Label4.Padding
            CharacterLeague.Text = CharacterList(int0).Split(":")(2)
            CharacterLeague.Font = Form1.Label4.Font
            CharacterLeague.ForeColor = Form1.Label4.ForeColor
            CharacterLeague.Size = New Point(71, 28)

            CharacterLeague.TextAlign = ContentAlignment.TopCenter

            Dim CharacterStore As New PictureBox
            CharacterStore.Location = New Point(368, Val(6) + Val(36 * AddValueINT))
            CharacterStore.Image = My.Resources.store_icon_off
            CharacterStore.SizeMode = PictureBoxSizeMode.AutoSize

            Dim CharacterName As New Label
            CharacterName.Location = New Point(63, Val(6) + Val(36 * AddValueINT))
            CharacterName.Padding = Form1.Label9.Padding
            CharacterName.BackColor = Form1.Label9.BackColor
            CharacterName.Font = Form1.Label9.Font
            CharacterName.ForeColor = Form1.Label9.ForeColor

            CharacterName.Text = CharacterList(int0).Split(":")(1)
            CharacterName.Size = New Point(215, 28)

            CharacterStatus.Visible = True
            CharacterName.Visible = True
            CharacterLeague.Visible = True
            CharacterStore.Visible = True

            If CharacterLeague.Text = "Hardcore" Then
                CharacterLeague.BackColor = Form1.Label4.BackColor
            ElseIf CharacterLeague.Text = "Beyond" Then
                CharacterLeague.BackColor = Color.FromArgb("226", "106", "106")
            ElseIf CharacterLeague.Text = "Standard" Then
                CharacterLeague.BackColor = Color.FromArgb("149", "165", "166")
            ElseIf CharacterLeague.Text = "Rampage" Then
                CharacterLeague.BackColor = Color.FromArgb("27", "188", "155")
            Else

            End If

            If CharacterList(int0).Split(":").Count = 4 Then
                AddHandler CharacterStore.Click, AddressOf Form1.StoreIcon_Click
                CharacterStore.Image = My.Resources.store_icon_grey
                CharacterStore.Name = "id" & CharacterList(int0).Split(":")(3)
                CharacterLeague.Cursor = Cursors.Hand
            Else
                CharacterStore.Image = My.Resources.store_icon_off
            End If
            If Form1.Label10.Text = "ON" Then
                If CharacterStore.Name.Contains("id") Then
                    CharacterListPanel.Controls.Add(CharacterStore)
                    CharacterListPanel.Controls.Add(CharacterStatus)
                    CharacterListPanel.Controls.Add(CharacterName)
                    CharacterListPanel.Controls.Add(CharacterLeague)
                    AddValueINT += 1
                End If
            Else
                CharacterListPanel.Controls.Add(CharacterStore)
                CharacterListPanel.Controls.Add(CharacterStatus)
                CharacterListPanel.Controls.Add(CharacterName)
                CharacterListPanel.Controls.Add(CharacterLeague)
                AddValueINT += 1
            End If

            int0 += 1
        End While
        CharacterListPanel.Refresh()

        Dim ExtraScroll As New Label
        ExtraScroll.Visible = True
        ExtraScroll.Location = New Point(1, Val(1) + Val(36 * AddValueINT))
        CharacterListPanel.Controls.Add(ExtraScroll)

        Return True

    End Function

    'Simple Account Key check for validation
    Public Function CheckLogin(ByVal AccountKey As String)
        Dim Request As String = HTTPRequest("http://klayver.pwx.me/common/actions/fetch_characters.php?code=" & AccountKey)
        If Request.Contains("ERROR:invalid_code") Or Request.Contains("ERROR:no_characters_found") Or Request.Contains("ERROR:") Then
            Return False
        Else
            Return True
        End If
    End Function

    'Execute Functions related to Settings if they are required
    Public Function EstablishSettings(ByVal APIHastable As Hashtable)
        If APIHastable("Settings:LoadOnStartUp") = True Then
            Form1.Label20.Text = "ON"
            Try
                My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "Exile Bro Client", Application.ExecutablePath)
            Catch ex As Exception
            End Try
        Else
            Try
                My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "Exile Bro Client", "")
            Catch ex As Exception
            End Try
        End If
        If APIHastable("Settings:LoadOnlyCharsWithStore") = True Then
            Form1.Label10.Text = "ON"

        End If
        If APIHastable("Settings:SystemTrayActive") = True Then
            Form1.Label37.Text = "ON"

        End If
        If APIHastable("Settings:SearchUpdatesOnStart") = True Then
            Form1.Label19.Text = "ON"

        End If
        Return APIHastable
    End Function

    'Write User Settings into AppData Save File
    Public Function SaveSettings(ByVal APIHashtable As Hashtable, Optional ByVal SaveKey As Boolean = True)
        Try
            CheckSettingsDirectory(APIHashtable("UserSettingsPath"), APIHashtable("UserSettingsFilename"))
            If SaveKey = True Then
                System.IO.File.WriteAllText(APIHashtable("UserSettingsPath") & "\" & APIHashtable("UserSettingsFilename"), Decrypt(APIHashtable("AccountKey") & "|" & APIHashtable("Settings:LoadOnStartUp") & "|" & APIHashtable("Settings:LoadOnlyCharsWithStore") & "|" & APIHashtable("Settings:SystemTrayActive") & "|" & APIHashtable("Settings:SearchUpdatesOnStart")))
            Else
                System.IO.File.WriteAllText(APIHashtable("UserSettingsPath") & "\" & APIHashtable("UserSettingsFilename"), Decrypt("|" & APIHashtable("Settings:LoadOnStartUp") & "|" & APIHashtable("Settings:LoadOnlyCharsWithStore") & "|" & APIHashtable("Settings:SystemTrayActive") & "|" & APIHashtable("Settings:SearchUpdatesOnStart")))
            End If
         Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    'Checks Appdata Folder and ini exsistence and Loads Settings
    Public Function LoadSettings(ByVal APIHashtable As Hashtable)
       
        CheckSettingsDirectory(APIHashtable("UserSettingsPath"), APIHashtable("UserSettingsFilename"))


        Dim SettingsFileContent() As String = Encrypt(System.IO.File.ReadAllText(APIHashtable("UserSettingsPath") & "\" & APIHashtable("UserSettingsFilename"))).ToString.Split("|")


        Try
            APIHashtable("AccountKey") = SettingsFileContent(0)
            APIHashtable("Settings:LoadOnStartUp") = ValidateBoolean(SettingsFileContent(1))
            APIHashtable("Settings:LoadOnlyCharsWithStore") = ValidateBoolean(SettingsFileContent(2))
            APIHashtable("Settings:SystemTrayActive") = ValidateBoolean(SettingsFileContent(3))
            APIHashtable("Settings:SearchUpdatesOnStart") = ValidateBoolean(SettingsFileContent(4))
        Catch ex As Exception
            APIHashtable("AccountKey") = False
            APIHashtable("Settings:LoadOnStartUp") = False
            APIHashtable("Settings:LoadOnlyCharsWithStore") = False
            APIHashtable("Settings:SystemTrayActive") = False
            APIHashtable("Settings:SearchUpdatesOnStart") = False
        End Try


        Return APIHashtable

    End Function

    'Establish User Settings and App Data Directory for use
    Private Sub CheckSettingsDirectory(ByVal Path As String, ByVal Filename As String)
        If System.IO.Directory.Exists(Path) = False Then
            System.IO.Directory.CreateDirectory(Path)
        End If
        If System.IO.File.Exists(Path & "\" & Filename) = False Then
            System.IO.File.WriteAllText(Path & "\" & Filename, Decrypt("|false|false|false|false"))
        End If
    End Sub

    'Convert String into a Boolean Variable securely
    Private Function ValidateBoolean(Optional ByVal C As String = "false")
        If C = True Then
            Return True
        Else
            Return False
        End If
    End Function

    'creates base64 String
    Private Function Decrypt(ByVal C As String)
        Dim b As Byte()
        b = Encoding.UTF8.GetBytes(C)
        Dim t As String
        t = Convert.ToBase64String(b)
        Return t
    End Function
    'decodes base64 String
    Private Function Encrypt(ByVal C As String)
        Dim t As Byte()
        t = Convert.FromBase64String(C)
        Dim b As String
        b = Encoding.UTF8.GetString(t)
        Return b
    End Function

    'Client check for own version (Will be checking his own MD5 in the future)
    Public Function CheckClientVersion(ByVal CurrentVersion As String, ByVal NewVersion As String)
        If CurrentVersion = NewVersion Then
            Return True
        Else
            Return False
        End If
    End Function

    'Loads the Client informations and return them directly into the sended as Hashtable
    Public Function LoadVersionInfo(ByVal VersionInfo As Hashtable)
        Dim NewestVersion() As String = HTTPRequest("http://exilebro.com/common/client/c_v.ini").ToString.Split("|")
        With VersionInfo
            .Add("NewestVersion", NewestVersion(0))
            .Add("VirustotalLink", NewestVersion(1))
            Try
                .Add("ChangeLogLink", NewestVersion(2))
            Catch ex As Exception
                .Add("ChangeLogLink", False)
            End Try
        End With
        Return True
    End Function

    'Standart HTTP web request function with error handling
    Public Function HTTPRequest(ByVal Link As String)
        Try
            Dim webReq As HttpWebRequest = HttpWebRequest.Create(New Uri(Link))
            webReq.ContentType = "application/x-www-form-urlencoded"
            webReq.UserAgent = UserAgend
            webReq.Method = "GET"
            Dim webRes As HttpWebResponse = webReq.GetResponse()
            Application.DoEvents()
            Dim ResStream As IO.StreamReader = New IO.StreamReader(webRes.GetResponseStream())
            Dim strResponse As String = ResStream.ReadToEnd()
            Return strResponse
        Catch ex As Exception
            MsgBox("ExileBro.com seems to be overloaded, please try again later.", MsgBoxStyle.Critical, "")
            Application.Exit()
            Exit Function
        End Try
    End Function

    'Simple Function to push a VB form ontop of all windows
    Public Function BringOnTop(ByVal TargetForm As Form)
        TargetForm.Show()
        TargetForm.TopMost = True
        TargetForm.TopMost = False
        Return True
    End Function

    'Simple Function to resize Images
    Public Shared Function ResizeImage(ByVal image As Image, ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
        Dim newWidth As Integer
        Dim newHeight As Integer
        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = If(percentHeight < percentWidth, percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
            newWidth = size.Width
            newHeight = size.Height
        End If
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function

    'Simple MD5 File Check Function
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