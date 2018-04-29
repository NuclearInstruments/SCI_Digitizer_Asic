Public Class ChooseCh
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
    Public ch_chosen As String
    '  Public matrixch(,) As String
    Private Sub ChooseCh_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '  Dim listch() = {}
        ' Dim k = 0
        'For i = 0 To 31
        'For j = 0 To 31
        'If matrixch(i, j) <> "NC" Then
        'listch(k) = CInt(matrixch(i, j))
        'k += 1
        'End If
        'Next
        'Next
        'Array.Sort(listch)
        ComboBox1.Items.Add("NC")
        For i = 1 To MainForm.CurrentOscilloscope.Channels
            ComboBox1.Items.Add(i.ToString)
        Next
        ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ch_chosen = ComboBox1.SelectedItem

    End Sub
End Class