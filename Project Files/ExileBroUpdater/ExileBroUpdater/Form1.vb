Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports System.Management
Public Class Form1
    Dim ExePath As String = ""
    Dim OldVersion As String = ""
    Dim NewVersion As String = ""
    Public Shared UseragendWindows As String = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0"
    Dim VirustotalLink As String = ""

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            ExePath = My.Application.CommandLineArgs.Item(0).Split("|")(0)
            OldVersion = My.Application.CommandLineArgs.Item(0).Split("|")(1)

            Dim C() As String = HTTPRequest("http://exilebro.com/common/client/c_v.ini").ToString.Split("|")
            NewVersion = C(0)
            Label3.Text = "Current Version : " & OldVersion
            Label1.Text = "Newest Version : " & C(0)
            Label1.Text = "Click here to open Virustotal.com Scan for " & C(0)
            Label2.Text = "Click here to open Changelog for " & C(0)
            VirustotalLink = C(1)

        Catch ex As Exception
            Application.Exit()
        End Try
        
    End Sub

    Private Sub Ini()
        
    End Sub

    Private Sub client_ProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        Dim bytesIn As Double = Double.Parse(e.BytesReceived.ToString())
        Dim totalBytes As Double = Double.Parse(e.TotalBytesToReceive.ToString())
        Dim percentage As Double = bytesIn / totalBytes * 100
        Label2.Text = percentage & "%"
        ProgressBar1.Value = Int32.Parse(Math.Truncate(percentage).ToString())
    End Sub

    Private Sub client_DownloadCompleted()
        Label5.Text = "Download Complete"
        Process.Start(ExePath)
        Application.Exit()
        Exit Sub
    End Sub

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



    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click
        Dim client As WebClient = New WebClient
        AddHandler client.DownloadProgressChanged, AddressOf client_ProgressChanged
        AddHandler client.DownloadFileCompleted, AddressOf client_DownloadCompleted
        client.DownloadFileAsync(New Uri("http://exilebro.com/common/client/" & NewVersion & ".exe"), ExePath)
        sender.text = "Please wait"
        sender.enabled = False
    End Sub


    Private Function LoadChangelog(ByVal Version As String)
        Dim C As String = HTTPRequest("http://exilebro.com/common/client/" & NewVersion & ".txt")
        Return C
    End Function


    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click
        Process.Start(VirustotalLink)
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click

        changelog.TextBox1.Text = LoadChangelog("0.1b").ToString
        
        changelog.ShowDialog()
    End Sub
End Class
