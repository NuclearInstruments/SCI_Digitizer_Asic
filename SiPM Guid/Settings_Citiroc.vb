
Imports DT5550W_P_lib
Imports SciDigitizerAsic.Settings.ClassSettings

Public Class Settings_Citiroc
    Dim gridList As New List(Of DataGridView)


    Public Function GetSettingsClass() As WeeRocAsicCommonSettings
        Dim cfg As New WeeRocAsicCommonSettings
        Dim asicCount As Integer = 0

        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            asicCount += BI.totalAsics
        Next
        cfg.AsicModel = "CITIROC 1A"
        cfg.AsicCount = asicCount
        cfg.Timestamp = Now
        cfg.AppVersion = Application.ProductVersion.ToString
        cfg.ChargeThreshold = A_ChargeTHR.Value
        cfg.TimeThreshold = A_TimeTHR.Value

        cfg.TransferSize = TransferSize.Text
        cfg.T0Mode = T0Mode.Text
        cfg.HvOutputOn = HVon.Checked
        cfg.HVVoltage = Voltage.Value
        cfg.T0Freq = T0Freq.Value
        cfg.SelfTriggerEnable = SelfEnable.Checked
        cfg.SelfTRiggerFreq = SelfFreq.Value
        cfg.MonitorMux = monitorMuxAnalog.Text
        cfg.Channel = moniorCHAnalog.Value
        cfg.MonitorMuxDigital = monitorMuxDigital.Text
        cfg.ChannelDigital = moniorCHDigital.Value

        cfg.ProcessMode = aProcessingMode.Text
        cfg.FileFormat = aFileFormat.Text
        cfg.ClusterTimens = aClusterTime.Value

        cfg.EnergyModeHG = EnergyModeHG.Text
        cfg.EnergyModeLG = EnergyModeLG.Text
        cfg.ShapingTimeLG = ShapingTimeLG.Text
        cfg.ShapingTimeHG = ShapingTimeHG.Text
        cfg.FastShaperSource = FastShaperSource.Text
        cfg.SCABias = ScaBias.Text
        cfg.InputDacReference = InputDacReference.Text
        cfg.PreampBias = PreampBias.Text
        cfg.TriggerSelector = TriggerSelector.Text
        cfg.TriggerMode = TriggerMode.Text
        cfg.TriggerLatch = LatchTrigger.Checked

        ReDim cfg.sA(asicCount)

        For q = 0 To asicCount - 1
            cfg.sA(q) = New WeeRocAsicCommonSettings.SingleAsicCFG()
            ReDim cfg.sA(q).sC(31)
            For z = 0 To 31
                cfg.sA(q).sC(z) = New WeeRocAsicCommonSettings.SingleAsicCFG.SignleChannelCFG
                cfg.sA(q).sC(z).BiasCompEnable = IIf(gridList(q).Rows(z).Cells("Enableb").Value = 0, False, True)
                cfg.sA(q).sC(z).BiasComp = gridList(q).Rows(z).Cells("DACb").Value
                cfg.sA(q).sC(z).TimeMask = IIf(gridList(q).Rows(z).Cells("TriggerMask").Value = 0, False, True)
                cfg.sA(q).sC(z).TestHG = IIf(gridList(q).Rows(z).Cells("testHG").Value = 0, False, True)
                cfg.sA(q).sC(z).TestLG = IIf(gridList(q).Rows(z).Cells("testLG").Value = 0, False, True)
                cfg.sA(q).sC(z).GainHG = gridList(q).Rows(z).Cells("gainHG").Value
                cfg.sA(q).sC(z).GainLG = gridList(q).Rows(z).Cells("gainLG").Value
                cfg.sA(q).sC(z).THcompTime = gridList(q).Rows(z).Cells("THcompTime").Value
                cfg.sA(q).sC(z).THcompCharge = gridList(q).Rows(z).Cells("THcompCharge").Value
                cfg.sA(q).sC(z).Gain = gridList(q).Rows(z).Cells("Gain").Value
                cfg.sA(q).sC(z).Offset = gridList(q).Rows(z).Cells("Offset").Value
            Next

        Next

        Return cfg

    End Function


    Public Function SetSettingsFromClass(ByRef cfg As WeeRocAsicCommonSettings) As Boolean

        Dim asicCount As Integer = 0

        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            asicCount += BI.totalAsics
        Next
        If cfg.AsicModel <> "CITIROC 1A" Then
            MainForm.AppendToLog(MainForm.LogMode.mERROR, "Invalid configuration file. This file is not for PETIROC 2A")
        End If

        If cfg.AsicCount < asicCount Then
            MainForm.AppendToLog(MainForm.LogMode.mWARNING, "Only the first " & asicCount & " will be configured")
        End If

        If cfg.AsicCount > asicCount Then
            MainForm.AppendToLog(MainForm.LogMode.mWARNING, "The configuration file has more ASIC that in the current setup")
        End If

        If cfg.AppVersion <> Application.ProductVersion.ToString Then
            MainForm.AppendToLog(MainForm.LogMode.mWARNING, "Configuration file saved with a different version of application")
        End If

        Try


            A_ChargeTHR.Value = cfg.ChargeThreshold
            A_TimeTHR.Value = cfg.TimeThreshold

            TransferSize.Text = cfg.TransferSize
            T0Mode.Text = cfg.T0Mode
            HVon.Checked = cfg.HvOutputOn
            Voltage.Value = cfg.HVVoltage
            T0Freq.Value = cfg.T0Freq
            SelfEnable.Checked = cfg.SelfTriggerEnable
            SelfFreq.Value = cfg.SelfTRiggerFreq
            monitorMuxAnalog.Text = cfg.MonitorMux
            moniorCHAnalog.Value = cfg.Channel
            monitorMuxDigital.Text = cfg.MonitorMuxDigital
            moniorCHDigital.Value = cfg.ChannelDigital

            aProcessingMode.Text = cfg.ProcessMode
            aFileFormat.Text = cfg.FileFormat
            aClusterTime.Value = cfg.ClusterTimens

            EnergyModeHG.Text = cfg.EnergyModeHG
            EnergyModeLG.Text = cfg.EnergyModeLG
            ShapingTimeLG.Text = cfg.ShapingTimeLG
            ShapingTimeHG.Text = cfg.ShapingTimeHG
            FastShaperSource.Text = cfg.FastShaperSource
            ScaBias.Text = cfg.SCABias
            InputDacReference.Text = cfg.InputDacReference
            PreampBias.Text = cfg.PreampBias
            TriggerSelector.Text = cfg.TriggerSelector
            TriggerMode.Text = cfg.TriggerMode
            LatchTrigger.Checked = cfg.TriggerLatch


            Dim asC As Integer = IIf(cfg.AsicCount >= asicCount, asicCount, cfg.AsicCount)

            For q = 0 To asC - 1
                For z = 0 To 31
                    gridList(q).Rows(z).Cells("Enableb").Value = cfg.sA(q).sC(z).BiasCompEnable
                    gridList(q).Rows(z).Cells("DACb").Value = cfg.sA(q).sC(z).BiasComp
                    gridList(q).Rows(z).Cells("TriggerMask").Value = cfg.sA(q).sC(z).TimeMask
                    gridList(q).Rows(z).Cells("testHG").Value = cfg.sA(q).sC(z).TestHG
                    gridList(q).Rows(z).Cells("testLG").Value = cfg.sA(q).sC(z).TestLG
                    gridList(q).Rows(z).Cells("gainHG").Value = cfg.sA(q).sC(z).GainHG
                    gridList(q).Rows(z).Cells("gainLG").Value = cfg.sA(q).sC(z).GainLG
                    gridList(q).Rows(z).Cells("THcompTime").Value = cfg.sA(q).sC(z).THcompTime
                    gridList(q).Rows(z).Cells("THcompCharge").Value = cfg.sA(q).sC(z).THcompCharge
                    gridList(q).Rows(z).Cells("Gain").Value = cfg.sA(q).sC(z).Gain
                    gridList(q).Rows(z).Cells("Offset").Value = cfg.sA(q).sC(z).Offset
                Next

            Next
        Catch ex As Exception
            MainForm.AppendToLog(MainForm.LogMode.mERROR, "Invalid configuration file. This file is not for PETIROC 2A")
        End Try


    End Function
    Public Sub EnableDisableUpdate(disable As Boolean)
        If disable Then
            ButtonSetMonitor.Enabled = False
            ButtonSetCfg.Enabled = False
        Else
            ButtonSetMonitor.Enabled = True
            ButtonSetCfg.Enabled = True
        End If
    End Sub

    Private Sub DataGridView_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)

        If sender.SelectedCells.Count > 1 Then

            For Each r As DataGridViewCell In sender.SelectedCells
                If r.RowIndex <> e.RowIndex Then
                    sender.Rows(r.RowIndex).Cells(e.ColumnIndex).Value = sender.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                End If
            Next

        End If
    End Sub


    Private Sub Settings_Citiroc_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages.Clear()
        gridList.Clear()
        Dim ABCDLETTERS As String() = {"A", "B", "C", "D"}
        Dim k
        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1


                Dim ControlPage As New TabPage
                Dim dgw As New DataGridView
                gridList.Add(dgw)
                ControlPage.Controls.Add(dgw)
                ControlPage.Text = "(" & ABCDLETTERS(j) & ") " & MainForm.DTList(i).SerialNumber
                dgw.Dock = DockStyle.Fill

                dgw.Columns.Clear()
                dgw.Columns.Add("Channel", "Channel")
                Dim chk As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn
                dgw.Columns.Add(chk)
                chk.HeaderText = "Input Bias (En)"
                chk.Name = "Enableb"
                dgw.Columns.Add("DACb", "Bias Offset")
                chk = New DataGridViewCheckBoxColumn
                dgw.Columns.Add(chk)
                chk.HeaderText = "Mask Trigger"
                chk.Name = "TriggerMask"
                chk = New DataGridViewCheckBoxColumn
                dgw.Columns.Add(chk)
                chk.HeaderText = "Test (LG)"
                chk.Name = "testLG"
                dgw.Columns.Add(chk)
                chk.HeaderText = "Test (HG)"
                chk.Name = "testHG"
                dgw.Columns.Add("gainLG", "Gain (LG)")
                dgw.Columns.Add("gainHG", "Gain (HG)")
                dgw.Columns.Add("THcompTime", "Time Th. Corr")
                dgw.Columns.Add("THcompCharge", "Charge Th. Corr")
                dgw.Columns.Add("Gain", "Gain")
                dgw.Columns.Add("Offset", "Offset")

                dgw.Columns(0).Width = 120
                dgw.Columns(0).ReadOnly = True
                dgw.Columns(1).Width = 80
                dgw.Columns(2).Width = 80
                dgw.Columns(3).Width = 80
                dgw.Columns(4).Width = 80
                dgw.Columns(5).Width = 80
                dgw.Columns(6).Width = 80
                dgw.Columns(7).Width = 80
                dgw.Columns(8).Width = 120
                dgw.Columns(9).Width = 120
                dgw.Columns(10).Width = 80
                dgw.Columns(11).Width = 80

                dgw.RowHeadersVisible = False
                dgw.AllowUserToAddRows = False
                dgw.AllowUserToDeleteRows = False

                AddHandler dgw.CellValueChanged, AddressOf DataGridView_CellValueChanged
                k = 0
                Do While (k < 32)
                    Dim kill As Integer = 0
                    'If ((k = 15) _
                    '        OrElse ((k = 19) _
                    '        OrElse (k = 27))) Then
                    '    kill = 1
                    'End If

                    Dim myString As String = k.ToString
                    dgw.Rows.Add(("CHANNEL " + myString), 1, 128, 0, kill, 32, 1.0, 0)
                    k = (k + 1)
                Loop
                TabControl1.TabPages.Add(ControlPage)
            Next


        Next

        EnergyModeHG.Items.Add("SCA")
        EnergyModeHG.Items.Add("Peak Sense")
        EnergyModeHG.SelectedIndex = 0

        EnergyModeLG.Items.Add("SCA")
        EnergyModeLG.Items.Add("Peak Sense")
        EnergyModeLG.SelectedIndex = 0

        For i = 0 To 7
            ShapingTimeHG.Items.Add(87.5 - i * 12.5)
            ShapingTimeLG.Items.Add(87.5 - i * 12.5)
        Next
        ShapingTimeLG.SelectedIndex = 0
        ShapingTimeHG.SelectedIndex = 0

        FastShaperSource.Items.Add("Low Gain")
        FastShaperSource.Items.Add("High Gain")
        FastShaperSource.SelectedIndex = 0

        InputDacReference.Items.Add("4.5V")
        InputDacReference.Items.Add("2.5V")
        InputDacReference.SelectedIndex = 0

        PreampBias.Items.Add("Normal")
        PreampBias.Items.Add("Weak")
        PreampBias.SelectedIndex = 0


        A_ChargeTHR.Value = 300
        A_TimeTHR.Value = 300

        TransferSize.Items.Add("10 Events")
        TransferSize.Items.Add("100 Events")
        TransferSize.Items.Add("1000 Events")
        TransferSize.Items.Add("10000 Events")
        TransferSize.SelectedIndex = 0

        T0Mode.Items.Add("FIRST PHOTON")
        T0Mode.Items.Add("FIRST PHOTON ASIC 0")
        T0Mode.Items.Add("INTERNAL - PERIODIC")
        T0Mode.Items.Add("EXTERNAL - LEMO 1")
        T0Mode.SelectedIndex = 0

        monitorMuxAnalog.Items.Add("None")
        monitorMuxAnalog.Items.Add("Preamp Output (LG)")
        monitorMuxAnalog.Items.Add("Slow Shaper Output (LG)")
        monitorMuxAnalog.Items.Add("Preamp Output (HG)")
        monitorMuxAnalog.Items.Add("Slow Shaper Output (HG)")
        monitorMuxAnalog.Items.Add("Fast Shaper")
        monitorMuxAnalog.SelectedIndex = 0

        monitorMuxDigital.Items.Add("None")
        monitorMuxDigital.Items.Add("Peak Detector (LG)")
        monitorMuxDigital.Items.Add("Peak Detector (HG)")
        monitorMuxDigital.SelectedIndex = 0


        aProcessingMode.Items.Add("FULL")
        aProcessingMode.Items.Add("DECODE EVENTS")
        aProcessingMode.Items.Add("NONE")
        aProcessingMode.SelectedIndex = 0

        aFileFormat.Items.Add("CSV")
        aFileFormat.Items.Add("BINARY")
        aFileFormat.SelectedIndex = 0


        TriggerSelector.Items.Add("Time Trigger")
        TriggerSelector.Items.Add("Charge Trigger")
        TriggerSelector.SelectedIndex = 0

        TempSensor.Items.Add("Internal (Avg)")
        TempSensor.Items.Add("External")
        TempSensor.SelectedIndex = 0

        TriggerMode.Items.Add("Self Trigger")
        TriggerMode.Items.Add("Common Trigger")
        TriggerMode.Items.Add("External Trigger")
        TriggerSelector.SelectedIndex = 0

    End Sub
End Class
