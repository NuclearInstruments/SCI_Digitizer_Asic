Public Class logwin
    Dim RunId As New Label
    Dim RunTime As New Label
    Dim RunStatus As New Label
    Dim RunProgress As New ProgressBar
    Dim Events As New Label
    Dim Clusters As New Label
    Dim FileSize As New Label
    Dim AvailableSize As New Label
    Dim DwnProcess As New Label

    Private Declare Function GetDiskFreeSpaceEx _
    Lib "kernel32" _
    Alias "GetDiskFreeSpaceExA" _
    (ByVal lpDirectoryName As String,
    ByRef lpFreeBytesAvailableToCaller As Long,
    ByRef lpTotalNumberOfBytes As Long,
    ByRef lpTotalNumberOfFreeBytes As Long) As Long

    Private Function BytesToMegabytes(ByVal Bytes As Long) _
    As String


        Dim dblAns As Double
        dblAns = (Bytes / 1024) / 1024
        BytesToMegabytes = Format(dblAns, "###,###,##0.00")

    End Function

    Public Function GetFreeSpace(ByVal Drive As String) As String
        'returns free space in MB, formatted to two decimal places
        'e.g., msgbox("Free Space on C: "& GetFreeSpace("C:\") & "MB")

        Dim lBytesTotal, lFreeBytes, lFreeBytesAvailable As Long

        Dim iAns As Long

        iAns = GetDiskFreeSpaceEx(Drive, lFreeBytesAvailable,
             lBytesTotal, lFreeBytes)
        If iAns > 0 Then

            Return BytesToMegabytes(lFreeBytes)
        Else
            Throw New Exception("Invalid or unreadable drive")
        End If


    End Function
    Private Sub logwin_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim tb2 As New TableLayoutPanel
        tb2.ColumnCount = 6
        tb2.RowCount = 5
        tb2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15))
        tb2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33))
        tb2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15))
        tb2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33))
        tb2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15))
        tb2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33))

        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 33))
        tb2.RowStyles.Add(New RowStyle(SizeType.Absolute, 5))
        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 33))
        tb2.RowStyles.Add(New RowStyle(SizeType.Absolute, 5))
        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 33))
        Dim l0 As New Label
        Dim l1 As New Label
        Dim l2 As New Label
        Dim l3 As New Label
        Dim l4 As New Label
        Dim l5 As New Label
        Dim l6 As New Label
        Dim l7 As New Label
        Dim l8 As New Label

        l0.Text = "Run ID"
        l0.TextAlign = ContentAlignment.MiddleCenter
        l0.Dock = DockStyle.Fill
        tb2.Controls.Add(l0, 0, 0)
        tb2.Dock = DockStyle.Fill
        RunId.Text = "0"
        RunId.BackColor = Color.Yellow
        RunId.ForeColor = Color.Black
        RunId.TextAlign = ContentAlignment.MiddleCenter
        RunId.Dock = DockStyle.Fill
        tb2.Controls.Add(RunId, 1, 0)


        l1.Text = "Run Status"
        l1.TextAlign = ContentAlignment.MiddleCenter
        l1.Dock = DockStyle.Fill
        tb2.Controls.Add(l1, 2, 0)
        tb2.Dock = DockStyle.Fill
        RunStatus.Text = "IDLE"
        RunStatus.BackColor = Color.DarkRed
        RunStatus.ForeColor = Color.White
        RunStatus.TextAlign = ContentAlignment.MiddleCenter
        RunStatus.Dock = DockStyle.Fill
        tb2.Controls.Add(RunStatus, 3, 0)


        l2.Text = "Run Time"
        l2.TextAlign = ContentAlignment.MiddleCenter
        l2.Dock = DockStyle.Fill
        tb2.Controls.Add(l2, 0, 2)
        tb2.Dock = DockStyle.Fill
        RunTime.Text = "00:00:00.000"
        RunTime.BackColor = Color.Black
        RunTime.ForeColor = Color.White
        RunTime.TextAlign = ContentAlignment.MiddleCenter
        RunTime.Dock = DockStyle.Fill
        tb2.Controls.Add(RunTime, 1, 2)


        l3.Text = "Run Progress"
        l3.TextAlign = ContentAlignment.MiddleCenter
        l3.Dock = DockStyle.Fill
        tb2.Controls.Add(l3, 2, 2)
        tb2.Dock = DockStyle.Fill
        RunProgress.Value = 0
        RunProgress.Style = ProgressBarStyle.Continuous
        RunProgress.Dock = DockStyle.Fill
        tb2.Controls.Add(RunProgress, 3, 2)


        l4.Text = "Events"
        l4.TextAlign = ContentAlignment.MiddleCenter
        l4.Dock = DockStyle.Fill
        tb2.Controls.Add(l4, 0, 4)
        tb2.Dock = DockStyle.Fill
        Events.Text = "0"
        Events.BackColor = Color.DarkMagenta
        Events.ForeColor = Color.White
        Events.TextAlign = ContentAlignment.MiddleCenter
        Events.Dock = DockStyle.Fill
        tb2.Controls.Add(Events, 1, 4)


        l5.Text = "Clusters"
        l5.TextAlign = ContentAlignment.MiddleCenter
        l5.Dock = DockStyle.Fill
        tb2.Controls.Add(l5, 2, 4)
        tb2.Dock = DockStyle.Fill
        Clusters.Text = "0"
        Clusters.BackColor = Color.DarkMagenta
        Clusters.ForeColor = Color.White
        Clusters.TextAlign = ContentAlignment.MiddleCenter
        Clusters.Dock = DockStyle.Fill
        tb2.Controls.Add(Clusters, 3, 4)


        l6.Text = "File Size"
        l6.TextAlign = ContentAlignment.MiddleCenter
        l6.Dock = DockStyle.Fill
        tb2.Controls.Add(l6, 4, 0)
        tb2.Dock = DockStyle.Fill
        FileSize.Text = "0 KB"
        FileSize.BackColor = Color.Gray
        FileSize.ForeColor = Color.Black
        FileSize.TextAlign = ContentAlignment.MiddleCenter
        FileSize.Dock = DockStyle.Fill
        tb2.Controls.Add(FileSize, 5, 0)


        l7.Text = "Disk Space"
        l7.TextAlign = ContentAlignment.MiddleCenter
        l7.Dock = DockStyle.Fill
        tb2.Controls.Add(l7, 4, 2)
        tb2.Dock = DockStyle.Fill
        AvailableSize.Text = "0 KB"
        AvailableSize.BackColor = Color.Gray
        AvailableSize.ForeColor = Color.Black
        AvailableSize.TextAlign = ContentAlignment.MiddleCenter
        AvailableSize.Dock = DockStyle.Fill
        tb2.Controls.Add(AvailableSize, 5, 2)

        l8.Text = "Download/Process"
        l8.TextAlign = ContentAlignment.MiddleCenter
        l8.Dock = DockStyle.Fill
        tb2.Controls.Add(l8, 4, 4)
        tb2.Dock = DockStyle.Fill
        DwnProcess.Text = "0 ms"
        DwnProcess.BackColor = Color.Gray
        DwnProcess.ForeColor = Color.Black
        DwnProcess.TextAlign = ContentAlignment.MiddleCenter
        DwnProcess.Dock = DockStyle.Fill
        tb2.Controls.Add(DwnProcess, 5, 4)

        tb2.Dock = DockStyle.Fill
        TableLayoutPanel1.Controls.Add(tb2, 1, 0)
        Timer1.Enabled = True
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        AvailableSize.Text = GetFreeSpace(My.Settings.folderpos) & " MB"
        RunId.Text = My.Settings.parRun

        RunTime.Text = MainForm.sTime
        RunStatus.Text = MainForm.sStatus
        RunProgress.Value = MainForm.sTargetPercent
        Events.Text = MainForm.sEventCounter
        Clusters.Text = MainForm.sClusterCounter
        FileSize.Text = BytesToMegabytes(MainForm.sByteCounter) & " MB"
        DwnProcess.Text = MainForm.sAcqTime & "/" & MainForm.sProcTime


    End Sub
End Class
