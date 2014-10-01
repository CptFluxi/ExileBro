Public Class Form2

    Private Sub Form2_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.Count = TextBox1.MaxLength Then
            PictureBox1.Enabled = True
            PictureBox1.Image = My.Resources.btn_arrow_right
        Else
            PictureBox1.Enabled = False
            PictureBox1.Image = My.Resources.btn_arrow_right_grey_faded
        End If
    End Sub
    Private Sub PictureBox15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox15.Click
        Process.Start("http://exilebro.com/ign/online/")
    End Sub
    Private Sub PictureBox15_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox15.MouseHover
        PictureBox15.Image = My.Resources.help_down
    End Sub
    Private Sub PictureBox15_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox15.MouseLeave
        PictureBox15.Image = My.Resources.help_up
    End Sub
    Private Sub PictureBox15_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox15.MouseMove
        PictureBox15.Image = My.Resources.help_down
    End Sub

    Dim ClientAPI As New ClientAPIStructure
    Dim ClientFunctions As New Client
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        If ClientFunctions.CheckLogin(TextBox1.Text) = False Then
            MsgBox("Wrong Account Key.", MsgBoxStyle.Critical, "")
        Else
            Me.Hide()
            Me.Visible = False
            ClientAPI.SetClient("AccountKey", TextBox1.Text)
            ClientFunctions.SaveSettings(ClientAPI.ReturnAPIEntrys)

            ClientFunctions.LoadClient(Form1.Panel2, ClientAPI.ReturnAPIEntrys)

            ClientFunctions.BringOnTop(Form1)
        End If

    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        loader.TopMost = False
        loader.Hide()
        loader.WindowState = FormWindowState.Minimized
    End Sub
End Class