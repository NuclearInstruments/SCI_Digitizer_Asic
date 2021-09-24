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
    Dim SiPMT As New Label
    Dim HVVolt As New Label
    Dim HVCurrent As New Label

    Dim CUR_Trigger As New Label
    Dim CUR_Validation As New Label
    Dim CNTR_Validated As New Label
    Dim CNTR_Fake As New Label
    Dim CNTR_NotValidated As New Label


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

        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 16))
        tb2.RowStyles.Add(New RowStyle(SizeType.Absolute, 5))
        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 16))
        tb2.RowStyles.Add(New RowStyle(SizeType.Absolute, 5))
        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 16))
        tb2.RowStyles.Add(New RowStyle(SizeType.Absolute, 5))
        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 16))
        tb2.RowStyles.Add(New RowStyle(SizeType.Absolute, 5))
        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 16))
        tb2.RowStyles.Add(New RowStyle(SizeType.Absolute, 5))
        tb2.RowStyles.Add(New RowStyle(SizeType.Percent, 16))

        Dim l0 As New Label
        Dim l1 As New Label
        Dim l2 As New Label
        Dim l3 As New Label
        Dim l4 As New Label
        Dim l5 As New Label
        Dim l6 As New Label
        Dim l7 As New Label
        Dim l8 As New Label
        Dim l9 As New Label
        Dim l10 As New Label
        Dim l11 As New Label
        Dim l12 As New Label
        Dim l13 As New Label
        Dim l14 As New Label
        Dim l15 As New Label
        Dim l16 As New Label




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

        l9.Text = "SiPM T"
        l9.TextAlign = ContentAlignment.MiddleCenter
        l9.Dock = DockStyle.Fill
        tb2.Controls.Add(l9, 0, 6)
        tb2.Dock = DockStyle.Fill
        SiPMT.Text = ""
        SiPMT.BackColor = Color.LightGreen
        SiPMT.ForeColor = Color.Black
        SiPMT.TextAlign = ContentAlignment.MiddleCenter
        SiPMT.Dock = DockStyle.Fill
        tb2.Controls.Add(SiPMT, 1, 6)

        l10.Text = "V"
        l10.TextAlign = ContentAlignment.MiddleCenter
        l10.Dock = DockStyle.Fill
        tb2.Controls.Add(l10, 2, 6)
        tb2.Dock = DockStyle.Fill
        HVVolt.Text = "0 V"
        HVVolt.BackColor = Color.LightGreen
        HVVolt.ForeColor = Color.Black
        HVVolt.TextAlign = ContentAlignment.MiddleCenter
        HVVolt.Dock = DockStyle.Fill
        tb2.Controls.Add(HVVolt, 3, 6)

        l11.Text = "I"
        l11.TextAlign = ContentAlignment.MiddleCenter
        l11.Dock = DockStyle.Fill
        tb2.Controls.Add(l11, 4, 6)
        tb2.Dock = DockStyle.Fill
        HVCurrent.Text = "0 uA"
        HVCurrent.BackColor = Color.LightGreen
        HVCurrent.ForeColor = Color.Black
        HVCurrent.TextAlign = ContentAlignment.MiddleCenter
        HVCurrent.Dock = DockStyle.Fill
        tb2.Controls.Add(HVCurrent, 5, 6)


        l12.Text = "Trigger ID"
        l12.TextAlign = ContentAlignment.MiddleCenter
        l12.Dock = DockStyle.Fill
        tb2.Controls.Add(l12, 0, 8)
        tb2.Dock = DockStyle.Fill
        CUR_Trigger.Text = ""
        CUR_Trigger.BackColor = Color.Pink
        CUR_Trigger.ForeColor = Color.Black
        CUR_Trigger.TextAlign = ContentAlignment.MiddleCenter
        CUR_Trigger.Dock = DockStyle.Fill
        tb2.Controls.Add(CUR_Trigger, 1, 8)

        l13.Text = "Validation ID"
        l13.TextAlign = ContentAlignment.MiddleCenter
        l13.Dock = DockStyle.Fill
        tb2.Controls.Add(l13, 2, 8)
        tb2.Dock = DockStyle.Fill
        CUR_Validation.Text = ""
        CUR_Validation.BackColor = Color.Pink
        CUR_Validation.ForeColor = Color.Black
        CUR_Validation.TextAlign = ContentAlignment.MiddleCenter
        CUR_Validation.Dock = DockStyle.Fill
        tb2.Controls.Add(CUR_Validation, 3, 8)


        l13.Text = "Validated"
        l13.TextAlign = ContentAlignment.MiddleCenter
        l13.Dock = DockStyle.Fill
        tb2.Controls.Add(l13, 0, 10)
        tb2.Dock = DockStyle.Fill
        CNTR_Validated.Text = ""
        CNTR_Validated.BackColor = Color.Beige
        CNTR_Validated.ForeColor = Color.Black
        CNTR_Validated.TextAlign = ContentAlignment.MiddleCenter
        CNTR_Validated.Dock = DockStyle.Fill
        tb2.Controls.Add(CNTR_Validated, 1, 10)

        l14.Text = "Fake"
        l14.TextAlign = ContentAlignment.MiddleCenter
        l14.Dock = DockStyle.Fill
        tb2.Controls.Add(l14, 2, 10)
        tb2.Dock = DockStyle.Fill
        CNTR_Fake.Text = ""
        CNTR_Fake.BackColor = Color.Beige
        CNTR_Fake.ForeColor = Color.Black
        CNTR_Fake.TextAlign = ContentAlignment.MiddleCenter
        CNTR_Fake.Dock = DockStyle.Fill
        tb2.Controls.Add(CNTR_Fake, 3, 10)

        l15.Text = "Not Validated"
        l15.TextAlign = ContentAlignment.MiddleCenter
        l15.Dock = DockStyle.Fill
        tb2.Controls.Add(l15, 4, 10)
        tb2.Dock = DockStyle.Fill
        CNTR_NotValidated.Text = ""
        CNTR_NotValidated.BackColor = Color.Beige
        CNTR_NotValidated.ForeColor = Color.Black
        CNTR_NotValidated.TextAlign = ContentAlignment.MiddleCenter
        CNTR_NotValidated.Dock = DockStyle.Fill
        tb2.Controls.Add(CNTR_NotValidated, 5, 10)

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
        RunProgress.Value = IIf(MainForm.sTargetPercent > 100, 100, MainForm.sTargetPercent)
        Events.Text = MainForm.sEventCounter
        Clusters.Text = MainForm.sClusterCounter
        FileSize.Text = BytesToMegabytes(MainForm.sByteCounter) & " MB"
        DwnProcess.Text = MainForm.sAcqTime & "/" & MainForm.sProcTime
        CUR_Trigger.Text = MainForm.sTriggerId
        CUR_Validation.Text = MainForm.sValidationId
        CNTR_Fake.Text = MainForm.sCntrNotFake
        CNTR_NotValidated.Text = MainForm.sCntrNotValidated
        CNTR_Validated.Text = MainForm.sCntrValidated

        'TextBox1.AppendText("Validated: " & MainForm.sCntrValidated + " Not Validated: " & MainForm.sCntrNotValidated)
        '  Vali.Text = MainForm.sCntrValidated

        'If RunStatus.Text.Contains("RUNNING") Then

        'If MainForm.DisableTempReadingAcq Then
        'HVVolt.Visible = False
        'HVCurrent.Visible = False
        'SiPMT.Visible = False
        'End If
        'Else
        'HVVolt.Visible = True
        'HVCurrent.Visible = True
        'SiPMT.Visible = True
        '
        'End If

    End Sub

    Public Sub UpdateHvStatus(enable As Boolean, voltage As Double, current As Double)

        If enable = False Then

            HVVolt.BackColor = Color.LightGreen
            HVCurrent.BackColor = Color.LightGreen
        Else

            HVVolt.BackColor = Color.Red
            HVCurrent.BackColor = Color.Red

        End If

        HVVolt.Text = Math.Round(voltage, 3) & " V"
        HVCurrent.Text = Math.Round(current, 3) & " uA"
    End Sub

    Public Sub UpdateSiPMTemp(sensorA As Double, sensorB As Double)
        If sensorB > -128 Then
            SiPMT.Text = "R: " & Math.Round(sensorA, 1) & "°C  L:" & Math.Round(sensorB, 1) & "°C"
        Else
            SiPMT.Text = Math.Round(sensorA, 1) & "°C"
        End If



    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class
