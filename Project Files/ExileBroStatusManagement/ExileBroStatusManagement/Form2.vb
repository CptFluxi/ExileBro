Public Class Form2

    Private Sub Form2_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.Count = TextBox1.MaxLength Then
            PictureBox1.Image = My.Resources.btn_arrow_right
        Else
            PictureBox1.Image = My.Resources.btn_arrow_right_grey_faded
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click

        If loader.CheckLogin(TextBox1.Text) = True Then
            System.IO.File.WriteAllText(Application.StartupPath & "\exilebro_client.ini", TextBox1.Text & "|false|false|false|false")
            Form1.LoadSettings()
            Form1.LoadCharacterList(TextBox1.Text)
            Me.Hide()
            loader.Hide()
            Form1.Show()
        Else
            MsgBox("Account Key could not be found.", MsgBoxStyle.Critical, "")
        End If

    End Sub

    Private Sub PictureBox15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox15.Click

        Process.Start("http://exilebro.com/ign/online/")
    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
       
        Me.TopMost = True
        Me.TopMost = False
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
End Class