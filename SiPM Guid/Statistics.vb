Public Class Statistics
    Dim icrocrtext() As TextBox
    Dim deadtimetext() As TextBox
    Dim toteventsinouttext() As TextBox

    Private Sub Statistics_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Statistics_ReLoad(MainForm.acquisition.CHList.Count)
    End Sub

    Public Sub Statistics_ReLoad(n As Integer)
        Panel1.Controls.Clear()
        ReDim icrocrtext(n - 1)
        ReDim deadtimetext(n - 1)
        ReDim toteventsinouttext(n - 1)
        For i = 0 To n - 1
            Dim chgroup As New GroupBox
            chgroup.Text = MainForm.acquisition.CHList(i).name '"CHANNEL " & (i + 1).ToString
            chgroup.Width = 260
            chgroup.Height = 110
            chgroup.Top = (i * chgroup.Height) + 10
            chgroup.Anchor = AnchorStyles.Left + AnchorStyles.Top
            Dim lab1 As New Label
            lab1.Text = "Input/Output Count Rate"
            lab1.Width = 123
            lab1.Height = 13
            lab1.Left = 7
            lab1.Top = 26
            lab1.Anchor = AnchorStyles.Left + AnchorStyles.Top
            chgroup.Controls.Add(lab1)
            Dim lab2 As New Label
            lab2.Text = "Dead Time (s)"
            lab2.Width = 73
            lab2.Height = 13
            lab2.Left = 7
            lab2.Top = 54
            lab2.Anchor = AnchorStyles.Left + AnchorStyles.Top
            chgroup.Controls.Add(lab2)
            Dim lab3 As New Label
            lab3.Text = "Input/Output Total Events"
            lab3.Width = 128
            lab3.Height = 13
            lab3.Left = 7
            lab3.Top = 82
            lab3.Anchor = AnchorStyles.Left + AnchorStyles.Top
            chgroup.Controls.Add(lab3)
            icrocrtext(i) = New TextBox
            icrocrtext(i).Left = 150
            icrocrtext(i).Top = 23
            icrocrtext(i).Anchor = AnchorStyles.Left + AnchorStyles.Top
            icrocrtext(i).ReadOnly = True
            icrocrtext(i).Enabled = False
            icrocrtext(i).BackColor = Color.White
            chgroup.Controls.Add(icrocrtext(i))
            deadtimetext(i) = New TextBox
            deadtimetext(i).Left = 150
            deadtimetext(i).Top = 51
            deadtimetext(i).Anchor = AnchorStyles.Left + AnchorStyles.Top
            deadtimetext(i).ReadOnly = True
            deadtimetext(i).Enabled = False
            deadtimetext(i).BackColor = Color.White
            chgroup.Controls.Add(deadtimetext(i))
            toteventsinouttext(i) = New TextBox
            toteventsinouttext(i).Left = 150
            toteventsinouttext(i).Top = 79
            toteventsinouttext(i).Anchor = AnchorStyles.Left + AnchorStyles.Top
            toteventsinouttext(i).ReadOnly = True
            toteventsinouttext(i).Enabled = False
            toteventsinouttext(i).BackColor = Color.White
            chgroup.Controls.Add(toteventsinouttext(i))

            Panel1.Controls.Add(chgroup)
        Next
    End Sub

    Public Sub UpdateStat()
        For i = 0 To MainForm.acquisition.CHList.Count - 1
            icrocrtext(i).Text = MainForm.acquisition.CHList(i).stat.icr & " / " & MainForm.acquisition.CHList(i).stat.ocr
            toteventsinouttext(i).Text = MainForm.acquisition.CHList(i).stat.toteventin & " / " & MainForm.acquisition.CHList(i).stat.toteventout
            deadtimetext(i).Text = MainForm.acquisition.CHList(i).stat.deadtime
        Next
    End Sub
End Class
