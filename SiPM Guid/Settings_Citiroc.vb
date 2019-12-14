
Imports DT5550W_P_lib
Imports SciDigitizerAsic.Settings.ClassSettings
Imports System.Reflection
Public Class Settings_Citiroc
    Dim gridList As New List(Of DataGridView)

    Public Sub EnableDoubleBuffered(ByVal dgv As DataGridView)

        Dim dgvType As Type = dgv.[GetType]()

        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered",
                                                 BindingFlags.Instance Or BindingFlags.NonPublic)

        pi.SetValue(dgv, True, Nothing)

    End Sub
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

        cfg.TriggerMode = TriggerMode.Text
        cfg.TriggerLatch = LatchTrigger.Checked
        cfg.HoldDelay = HoldDelay.Value

        ReDim cfg.sA(asicCount)

        For q = 0 To asicCount - 1
            cfg.sA(q) = New WeeRocAsicCommonSettings.SingleAsicCFG()
            ReDim cfg.sA(q).sC(31)
            For z = 0 To 31
                cfg.sA(q).sC(z) = New WeeRocAsicCommonSettings.SingleAsicCFG.SingleChannelCFG
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


    Public Function GetDefaultSettingsClass() As WeeRocAsicCommonSettings
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
        cfg.ChargeThreshold = 300
        cfg.TimeThreshold = 300

        cfg.TransferSize = "100 Events"
        cfg.T0Mode = "EXTERNAL - LEMO 1"
        cfg.HvOutputOn = False
        cfg.HVVoltage = 56
        cfg.T0Freq = 10

        cfg.SelfTRiggerFreq = 1000
        cfg.MonitorMux = "None"
        cfg.Channel = 0
        cfg.MonitorMuxDigital = "None"
        cfg.ChannelDigital = 0

        cfg.ProcessMode = "FULL"
        cfg.FileFormat = "CSV"
        cfg.ClusterTimens = 1000

        cfg.EnergyModeHG = "Peak Sense"
        cfg.EnergyModeLG = "Peak Sense"
        cfg.ShapingTimeLG = 87.5
        cfg.ShapingTimeHG = 87.5
        cfg.FastShaperSource = "Low Gain"
        cfg.SCABias = "Normal"
        cfg.InputDacReference = "4.5V"
        cfg.PreampBias = "Normal"

        cfg.TriggerMode = "Time"
        cfg.TriggerLatch = True
        cfg.HoldDelay = 10

        ReDim cfg.sA(asicCount)

        For q = 0 To asicCount - 1
            cfg.sA(q) = New WeeRocAsicCommonSettings.SingleAsicCFG()
            ReDim cfg.sA(q).sC(31)
            For z = 0 To 31
                cfg.sA(q).sC(z) = New WeeRocAsicCommonSettings.SingleAsicCFG.SingleChannelCFG
                cfg.sA(q).sC(z).BiasCompEnable = True
                cfg.sA(q).sC(z).BiasComp = 128
                cfg.sA(q).sC(z).TimeMask = False
                cfg.sA(q).sC(z).TestHG = False
                cfg.sA(q).sC(z).TestLG = False
                cfg.sA(q).sC(z).GainHG = 0
                cfg.sA(q).sC(z).GainLG = 4
                cfg.sA(q).sC(z).THcompTime = 7
                cfg.sA(q).sC(z).THcompCharge = 7
                cfg.sA(q).sC(z).Gain = 1
                cfg.sA(q).sC(z).Offset = 0
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

            TriggerMode.Text = cfg.TriggerMode
            LatchTrigger.Checked = cfg.TriggerLatch
            HoldDelay.Value = cfg.HoldDelay

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

                EnableDoubleBuffered(dgw)

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
                chk = New DataGridViewCheckBoxColumn
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
                    dgw.Rows.Add(("CHANNEL " + myString), 1, 128, 0, 0, 0, 0, 4, 7, 7, 1.0, 0)
                    k = (k + 1)
                Loop
                TabControl1.TabPages.Add(ControlPage)
            Next


        Next

        EnergyModeHG.Items.Add("Peak Sense")
        EnergyModeHG.Items.Add("SCA")
        EnergyModeHG.SelectedIndex = 0

        EnergyModeLG.Items.Add("Peak Sense")
        EnergyModeLG.Items.Add("SCA")
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

        ScaBias.Items.Add("Normal")
        ScaBias.Items.Add("Weak")
        ScaBias.SelectedIndex = 0

        A_ChargeTHR.Value = 300
        A_TimeTHR.Value = 300

        TransferSize.Items.Add("10 Events")
        TransferSize.Items.Add("100 Events")
        TransferSize.Items.Add("1000 Events")
        TransferSize.Items.Add("10000 Events")
        TransferSize.SelectedIndex = 0

        'T0Mode.Items.Add("FIRST PHOTON")
        'T0Mode.Items.Add("FIRST PHOTON ASIC 0")
        T0Mode.Items.Add("EXTERNAL - LEMO 1")
        T0Mode.Items.Add("INTERNAL - PERIODIC")
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



        TempSensor.Items.Add("Internal (Avg)")
        TempSensor.Items.Add("External")
        TempSensor.SelectedIndex = 0

        TriggerMode.Items.Add("Time")
        TriggerMode.Items.Add("Charge")
        TriggerMode.Items.Add("External")
        TriggerMode.Items.Add("Common (Time)")
        TriggerMode.Items.Add("Common (Charge)")
        TriggerMode.Items.Add("Self Trigger")
        TriggerMode.SelectedIndex = 0

    End Sub

    Private Sub TabPage4_Click(sender As Object, e As EventArgs) Handles TabPage4.Click

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Public Sub UpdateSettings()
        MainForm.Timer3.Enabled = False
        Dim TriggerExtMode = 0
        Select Case TriggerMode.Text
            Case "Time"
                TriggerExtMode = 0
            Case "Charge"
                TriggerExtMode = 0
            Case "External"
                TriggerExtMode = 1
            Case "Common (Time)"
                TriggerExtMode = 1
            Case "Common (Charge)"
                TriggerExtMode = 1
            Case "Self Trigger"
                TriggerExtMode = 1
        End Select


        For i = 0 To MainForm.DTList.Count - 1
            MainForm.DTList(i).CitirocClass.pCFG.Clear()
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1
                Dim strPtrc As String
                Dim ProgramWord() As UInt32 = New UInt32((36) - 1) {}
                Dim pC As New DT5550W_CITIROC.CitirocConfig
                For z = 0 To 31
                    pC.sc_cmdInputDac(z) = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("Enableb").Value = 0, 0, 1)
                    pC.sc_inputDac(z) = gridList(i * BI.totalAsics + j).Rows(z).Cells("DACb").Value
                    pC.sc_mask(z) = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("TriggerMask").Value = 0, 1, 0)
                    pC.sc_paHgGain(z) = gridList(i * BI.totalAsics + j).Rows(z).Cells("gainHG").Value
                    pC.sc_paLgGain(z) = gridList(i * BI.totalAsics + j).Rows(z).Cells("gainLG").Value
                    pC.sc_CtestHg(z) = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("testHG").Value = 0, 0, 1)
                    pC.sc_CtestLg(z) = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("testLG").Value = 0, 0, 1)
                    pC.sc_calibDacT(z) = gridList(i * BI.totalAsics + j).Rows(z).Cells("THcompTime").Value
                    pC.sc_calibDacQ(z) = gridList(i * BI.totalAsics + j).Rows(z).Cells("THcompCharge").Value
                    MainForm.CorrPoints(j, z).Gain = gridList(i * BI.totalAsics + j).Rows(z).Cells("Gain").Value
                    MainForm.CorrPoints(j, z).Offset = gridList(i * BI.totalAsics + j).Rows(z).Cells("Offset").Value

                    pC.sc_enPa(z) = 0

                Next


                pC.sc_scaOrPdHg = IIf(EnergyModeHG.Text = "Peak Sense", 0, 1)  ' peak detector on HG
                pC.sc_scaOrPdLg = IIf(EnergyModeLG.Text = "Peak Sense", 0, 1)  ' peak detector on LG
                pC.sc_bypassPd = 0 ' Bypass Peak Sensing
                pC.sc_selTrigExtPd = TriggerExtMode 'Select peak sensing cell trigger [0 : internal trigger – 1 : external trigger] 
                pC.sc_shapingTimeLg = ShapingTimeLG.SelectedIndex
                pC.sc_shapingTimeHg = ShapingTimeHG.SelectedIndex
                pC.sc_latchDiscri = IIf(LatchTrigger.Checked, 1, 0)  'Select latched (RS : 1) or direct output (trigger : 0) on charge discriminator
                pC.sc_dacRef = IIf(InputDacReference.Text = "4.5V", 1, 0)  '8-bit input DAC Voltage Reference (1 = internal 4,5V , 0 = internal 2,5V) 
                pC.sc_threshold1 = A_ChargeTHR.Value
                pC.sc_threshold2 = A_TimeTHR.Value
                pC.sc_testBitOtaQ = 1 'Output OTA buffer bias automatic off [0 : auto-bias – 1 : force on]
                pC.sc_triggerPolarity = 0 'Output trigger polarity choice [0 : positive(rising edge) – 1 : negative(falling edge)]

                pC.sc_paLgBias = IIf(PreampBias.Text = "Normal", 0, 1) 'Low Gain PreAmp bias [0: normal bias - 1: weak bias]

                pC.sc_fshOnLg = IIf(FastShaperSource.Text = "Low Gain", 1, 0) ' Select preamp to connect to Fast Shaper [0: fast shaper on HG preamp – 1: fast shaper on LG preamp]

                pC.sc_biasSca = IIf(ScaBias.Text = "Normal", 0, 1) 'SCA bias ( 1 = weak bias, 0 = high bias 5MHz ReadOut Speed) 


                pC.sc_enDiscri = 1
                pC.sc_ppDiscri = 1
                pC.sc_enDiscriT = 1
                pC.sc_ppDiscriT = 1
                pC.sc_enCalibDacQ = 1
                pC.sc_ppCalibDacQ = 1
                pC.sc_enCalibDacT = 1
                pC.sc_ppCalibDacT = 1
                pC.sc_ppThHg = 1
                pC.sc_enThHg = 1
                pC.sc_ppThLg = 1
                pC.sc_enThLg = 1

                pC.sc_ppPdetHg = 1
                pC.sc_enPdetHg = 1
                pC.sc_ppPdetLg = 1
                pC.sc_enPdetLg = 1
                pC.sc_ppFshBuffer = 1
                pC.sc_enFsh = 1
                pC.sc_ppFsh = 1
                pC.sc_ppSshLg = 1
                pC.sc_enSshLg = 1
                pC.sc_ppSshHg = 1
                pC.sc_enSshHg = 1

                pC.sc_ppPaLg = 1
                pC.sc_enPaLg = 1
                pC.sc_ppPaHg = 1
                pC.sc_enPaHg = 1

                pC.sc_enInputDac = 1

                pC.sc_ppTemp = 1
                pC.sc_enTemp = 1
                pC.sc_ppBg = 1
                pC.sc_enBg = 1
                pC.sc_enThresholdDac1 = 1
                pC.sc_ppThresholdDac1 = 1
                pC.sc_enThresholdDac2 = 1
                pC.sc_ppThresholdDac2 = 1
                pC.sc_enHgOtaQ = 1
                pC.sc_ppHgOtaQ = 1
                pC.sc_enLgOtaQ = 1
                pC.sc_ppLgOtaQ = 1
                pC.sc_enProbeOtaQ = 1
                pC.sc_ppProbeOtaQ = 1

                pC.sc_enValEvtReceiver = 1
                pC.sc_ppValEvtReceiver = 1
                pC.sc_enRazChnReceiver = 1
                pC.sc_ppRazChnReceiver = 1
                pC.sc_enDigitalMuxOutput = 1
                pC.sc_enOr32 = 1
                pC.sc_enNor32Oc = 1

                pC.sc_enNor32TOc = 1
                pC.sc_enTriggersOutput = 1

                pC.GenerateUint32Config(ProgramWord)

                Dim boolDV(1143) As Boolean
                Dim StringDV As String = ""
                pC.GenerateBitConfig(boolDV)
                For tt = 0 To 1143
                    StringDV &= IIf(boolDV(tt), "1", "0")
                Next

                MainForm.AppendToLog(MainForm.LogMode.mCONFIGURATION, "Configure ASIC: " & j & " - " & MainForm.DTList(i).SerialNumber & vbCrLf & StringDV)


                Select Case j
                    Case 0
                        MainForm.DTList(i).ConfigureAsic(True, False, False, False, ProgramWord)
                        MainForm.DTList(i).CitirocClass.pCFG.Add(pC)
                    Case 1
                        MainForm.DTList(i).ConfigureAsic(False, True, False, False, ProgramWord)
                        MainForm.DTList(i).CitirocClass.pCFG.Add(pC)
                    Case 2
                        MainForm.DTList(i).ConfigureAsic(False, False, True, False, ProgramWord)
                        MainForm.DTList(i).CitirocClass.pCFG.Add(pC)
                    Case 3
                        MainForm.DTList(i).ConfigureAsic(False, False, False, True, ProgramWord)
                        MainForm.DTList(i).CitirocClass.pCFG.Add(pC)
                End Select


            Next

            MainForm.DTList(i).SetHV(HVon.Checked, Voltage.Value, MaxV.Value)
            MainForm.DTList(i).ConfigureSignalGenerator(True, True, True, True,
                                         SelfFreq.Value)

            MainForm.DTList(i).SetASICVeto(swVeto1.Checked,
            swVeto2.Checked,
            swVeto3.Checked,
            swVeto4.Checked)

             MainForm.DTList(i).EnableExternalVeto(EnableExternalVeto.Checked)
            Select Case TriggerMode.Text
                Case "Time"
                    MainForm.DTList(i).CITIROC_SetTriggerMode(DT5550W_P_lib.TriggerMode.TIME_TRIG)
                Case "Charge"
                    MainForm.DTList(i).CITIROC_SetTriggerMode(DT5550W_P_lib.TriggerMode.CHARGE_TRIG)
                Case "External"
                    MainForm.DTList(i).CITIROC_SetTriggerMode(DT5550W_P_lib.TriggerMode.EXT_TRIG)
                Case "Common (Time)"
                    MainForm.DTList(i).CITIROC_SetTriggerMode(DT5550W_P_lib.TriggerMode.GBL_TRIG_TIME)
                Case "Common (Charge)"
                    MainForm.DTList(i).CITIROC_SetTriggerMode(DT5550W_P_lib.TriggerMode.GBL_TRIG_CHARGE)
                Case "Self Trigger"
                    MainForm.DTList(i).CITIROC_SetTriggerMode(DT5550W_P_lib.TriggerMode.SELF_TRIG)
            End Select

            Select Case T0Mode.Text
                Case "EXTERNAL - LEMO 1"
                    MainForm.DTList(i).ConfigureT0(DT5550W_P_lib.T0Mode.EXTERNAL, T0Freq.Value)
                Case "INTERNAL - PERIODIC"
                    MainForm.DTList(i).ConfigureT0(DT5550W_P_lib.T0Mode.SOFTWARE_PERIODIC, T0Freq.Value)
            End Select


            MainForm.DTList(i).CITIROC_SetHoldDelay(HoldDelay.Value)

        Next




        MainForm.hvon.Enabled = Not HVon.Checked
        MainForm.hvoff.Enabled = HVon.Checked

        MainForm.SoftwareThreshold = SoftwareTrigger.Value
        MainForm.InputPolarity = DT5550W_P_lib.tPOLARITY.POSITIVE



        MainForm.TempSensorSource = TempSensor.SelectedIndex
        MainForm.DisableTempReadingAcq = DisableTempRead.Checked
        MainForm.EnableTempComp = TempComp.Checked
        MainForm.TempCompCoef = tempConmpCoef.Value
        MainForm.CurrentHVSet = Voltage.Value
        MainForm.CurrentHVON = HVon.Checked
        MainForm.CurrentHVMax = MaxV.Value

        MainForm.SumSpectrumGain = SumSpectrumGain.Value

        MainForm.Timer3.Enabled = True
        MainForm.TransferSize = Math.Pow(10, TransferSize.SelectedIndex + 1)

        Select Case aProcessingMode.SelectedIndex
            Case 0
                MainForm.CurrentProcessMode = MainForm.ProcessMode.ALL
            Case 1
                MainForm.CurrentProcessMode = MainForm.ProcessMode.EVENT_DECODE
            Case 2
                MainForm.CurrentProcessMode = MainForm.ProcessMode.OFF
        End Select

        Select Case aFileFormat.SelectedIndex
            Case 0
                MainForm.SaveFileType = MainForm.FileType.CSV
            Case 1
                MainForm.SaveFileType = MainForm.FileType.BINARY
        End Select

        Try
            MainForm.ClusterMaxTime = aClusterTime.Value
        Catch ex As Exception

        End Try

    End Sub



    Public Sub SetMonitor()
        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1

                Dim MonitorWord() As UInt32 = New UInt32(7) {}
                Dim pC As New DT5550W_CITIROC.CitirocConfig

                Select Case monitorMuxAnalog.Text
                    Case "None"
                        pC.AnalogProble = DT5550W_CITIROC.CitirocConfig.tAnalogProbe.NONE
                    Case "Preamp Output (LG)"
                        pC.AnalogProble = DT5550W_CITIROC.CitirocConfig.tAnalogProbe.LG_PRE
                    Case "Slow Shaper Output (LG)"
                        pC.AnalogProble = DT5550W_CITIROC.CitirocConfig.tAnalogProbe.LG_SHAPER
                    Case "Preamp Output (HG)"
                        pC.AnalogProble = DT5550W_CITIROC.CitirocConfig.tAnalogProbe.HG_PRE
                    Case "Slow Shaper Output (HG)"
                        pC.AnalogProble = DT5550W_CITIROC.CitirocConfig.tAnalogProbe.HG_SHAPER
                    Case "Fast Shaper"
                        pC.AnalogProble = DT5550W_CITIROC.CitirocConfig.tAnalogProbe.FAST_SHAPER
                End Select

                Select Case monitorMuxDigital.Text
                    Case "None"
                        pC.DigitalProble = DT5550W_CITIROC.CitirocConfig.tDigitalProbe.NONE
                    Case "Peak Detector (LG)"
                        pC.DigitalProble = DT5550W_CITIROC.CitirocConfig.tDigitalProbe.LG_PEAK_DET_MODE
                    Case "Peak Detector (HG)"
                        pC.DigitalProble = DT5550W_CITIROC.CitirocConfig.tDigitalProbe.HG_PEAK_DET_MODE

                End Select



                pC.ChannelOutputAnalog = moniorCHAnalog.Value
                pC.ChannelOutputDigital = moniorCHDigital.Value
                pC.GenerateUint32Monitor(MonitorWord)


                Dim boolDV(256) As Boolean
                Dim StringDV As String = ""
                pC.GenerateBitMonitor(boolDV)
                For tt = 0 To 255
                    StringDV &= IIf(boolDV(tt), "1", "0")
                Next

                MainForm.AppendToLog(MainForm.LogMode.mCONFIGURATION, "Monitor ASIC: " & j & " - " & MainForm.DTList(i).SerialNumber & vbCrLf & StringDV)


                Select Case j
                    Case 0
                        MainForm.DTList(i).ConfigureMonitor(True, False, False, False, MonitorWord)
                    Case 1
                        MainForm.DTList(i).ConfigureMonitor(False, True, False, False, MonitorWord)
                    Case 2
                        MainForm.DTList(i).ConfigureMonitor(False, False, True, False, MonitorWord)
                    Case 3
                        MainForm.DTList(i).ConfigureMonitor(False, False, False, True, MonitorWord)
                End Select


            Next
        Next
    End Sub

    Private Sub ButtonSetCfg_Click(sender As Object, e As EventArgs) Handles ButtonSetCfg.Click
        UpdateSettings()

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub Label27_Click(sender As Object, e As EventArgs) Handles Label27.Click

    End Sub

    Private Sub TriggerMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TriggerMode.SelectedIndexChanged

    End Sub

    Private Sub ButtonSetMonitor_Click(sender As Object, e As EventArgs) Handles ButtonSetMonitor.Click
        SetMonitor()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1
                For k = 0 To BI.channelsPerAsic - 1
                    gridList(j + BI.totalAsics * i).Rows(k).Cells("Offset").Value = Math.Round(-(MainForm.MatrixCumulativePerAsic(j, k) / MainForm.MatrixCumulativePerAsicCount(j, k) - 40))

                Next
            Next
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim max = 0

        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1
                For k = 0 To BI.channelsPerAsic - 1
                    Dim point = MainForm.MatrixCumulativePerAsic(j, k) / MainForm.MatrixCumulativePerAsicCount(j, k)
                    max = IIf(point > max, point, max)
                Next
            Next
        Next


        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1
                For k = 0 To BI.channelsPerAsic - 1
                    Dim point = MainForm.MatrixCumulativePerAsic(j, k) / MainForm.MatrixCumulativePerAsicCount(j, k)
                    gridList(j + BI.totalAsics * i).Rows(k).Cells("Gain").Value = Math.Round(max / point, 3)
                Next
            Next
        Next
    End Sub


    Public Sub SCAN_PARAMETER(sm As WeeRocAsicCommonSettings.ScanMode, val As Double)
        Dim AsicCount
        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            AsicCount += BI.totalAsics
        Next
        Select Case sm
            Case WeeRocAsicCommonSettings.ScanMode.ScanTimeThreshold
                A_TimeTHR.Value = val
                UpdateSettings()
            Case WeeRocAsicCommonSettings.ScanMode.ScanHV
                Voltage.Value = val
                UpdateSettings()
            Case WeeRocAsicCommonSettings.ScanMode.ScanInputDAC
                For q = 0 To AsicCount - 1
                    For z = 0 To 31
                        gridList(q).Rows(z).Cells("DACb").Value = val
                    Next
                Next
                UpdateSettings()
            Case WeeRocAsicCommonSettings.ScanMode.ScanGain_HG
                For q = 0 To AsicCount - 1
                    For z = 0 To 31
                        gridList(q).Rows(z).Cells("gainHG").Value = val
                    Next
                Next
                UpdateSettings()
            Case WeeRocAsicCommonSettings.ScanMode.ScanGain_LG
                For q = 0 To AsicCount - 1
                    For z = 0 To 31
                        gridList(q).Rows(z).Cells("gainLG").Value = val
                    Next
                Next
                UpdateSettings()
            Case WeeRocAsicCommonSettings.ScanMode.ScanCorrThreshold
                For q = 0 To AsicCount - 1
                    For z = 0 To 31
                        gridList(q).Rows(z).Cells("THcompTime").Value = val
                    Next
                Next
                UpdateSettings()
            Case WeeRocAsicCommonSettings.ScanMode.ScanHoldDelay
                HoldDelay.Value = val
                UpdateSettings()

            Case WeeRocAsicCommonSettings.ScanMode.ScanExternalDelay

        End Select
    End Sub

    Private Sub Label16_Click(sender As Object, e As EventArgs) Handles Label16.Click

    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub
End Class
