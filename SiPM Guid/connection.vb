Imports System.ComponentModel

Public Class Connection

    Public Sub RefreshDevices()
        Dim devs = MainForm.dt_obj.GetDeviceList()
        ComboBox1.Items.Clear()
        For Each dev In devs
            ComboBox1.Items.Add(dev)
        Next
        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If
        ComboBox2.Items.Clear()
        ComboBox2.Items.Add("Auto")
        ComboBox2.Items.Add("Petiroc 2A")
        'ComboBox2.Items.Add("Citiroc")
        'ComboBox2.Items.Add("Maroc")
        ComboBox2.SelectedIndex = 0

        ComboBox3.Items.Clear()
        ComboBox3.Items.Add("Auto")
        ComboBox3.Items.Add("1")
        ComboBox3.Items.Add("2")
        ComboBox3.Items.Add("3")
        ComboBox3.Items.Add("4")
        ComboBox3.SelectedIndex = 0


    End Sub

    Private Sub Connection_Load(sender As Object, e As EventArgs) Handles Me.Load
        RefreshDevices()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Connection_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'If ComboBox2.SelectedIndex = "Auto" Or ComboBox2.SelectedIndex = "Petiroc 2A" Then

        'End If
        Me.DialogResult = DialogResult.OK
    End Sub
End Class