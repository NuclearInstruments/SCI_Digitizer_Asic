Public Class WaveformCaptureSelect
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub WaveformCaptureSelect_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For i = 0 To MainForm.acquisition.CHList.Count - 1
            ChList.Items.Add(MainForm.acquisition.CHList(i).name)
        Next
        For i = 0 To ChList.Items.Count - 1
            ChList.SetItemChecked(i, True)
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            FileName.Text = SaveFileDialog1.FileName
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        For i = 0 To ChList.Items.Count - 1
            ChList.SetItemChecked(i, True)
        Next
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        For i = 0 To ChList.Items.Count - 1
            ChList.SetItemChecked(i, False)
        Next
    End Sub
End Class