﻿Imports DT5550W_P_lib
Imports DT5550W_P_lib.DT5550W
Imports SiPM_Guid.AcquisitionClass

Public Class Settings

    Dim gridList As New List(Of DataGridView)


    Public Class ClassSettings
        Public Class SingleAsicCFG
            Public SerialNumerbOfTheBoard As String
            Public AsicId As Integer
            Public Class SignleChannelCFG
                Public BiasCompEnable As Boolean
                Public BiasComp As Integer
                Public ChargeMask As Boolean
                Public TimeMask As Boolean
                Public ThCompensation As Integer
                Public Gain As Double
                Public Offset As Double
            End Class

            Public sC() As SignleChannelCFG
        End Class

        Public Class chMap
            Public X As Int16
            Public Y As Int16
            Public ValidLocation As Boolean
            Public Board As String
            Public Asic As Int16
            Public Channel As Int16
            Public Veto As Boolean
        End Class
        Public AsicModel As String
        Public AsicCount As Integer
        Public Timestamp As DateTime
        Public AppVersion As String
        Public SignalPolarity As String
        Public ChargeThreshold As Integer
        Public TimeThreshold As Integer
        Public DelayTrigger As Integer
        Public ShaperCF As String
        Public ShaperCI As String
        Public TransferSize As String
        Public T0Mode As String
        Public psbin As Integer
        Public HvOutputOn As Boolean
        Public HVVoltage As Double
        Public T0Freq As Integer
        Public SelfTriggerEnable As Boolean
        Public SelfTRiggerFreq As Integer
        Public MonitorMux As String
        Public Channel As Integer
        Public AnalogReadout As Boolean
        Public ProcessMode As String
        Public FileFormat As String
        Public ClusterTimens As Integer

        Public sA() As SingleAsicCFG

        Public sMap() As chMap
    End Class
    Public Function GetSettingsClass() As ClassSettings
        Dim cfg As New ClassSettings
        Dim asicCount As Integer = 0

        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            asicCount += BI.totalAsics
        Next
        cfg.AsicModel = "PETIROC 2A"
        cfg.AsicCount = asicCount
        cfg.Timestamp = Now
        cfg.AppVersion = Application.ProductVersion.ToString
        cfg.SignalPolarity = A_polarity.Text
        cfg.ChargeThreshold = A_ChargeTHR.Value
        cfg.TimeThreshold = A_TimeTHR.Value
        cfg.DelayTrigger = A_DelayBox.Value
        cfg.ShaperCF = A_ShaperCF.Text
        cfg.ShaperCI = A_ShaperCI.Text
        cfg.TransferSize = TransferSize.Text
        cfg.T0Mode = T0Mode.Text
        cfg.psbin = TimePsBin.Value
        cfg.HvOutputOn = HVon.Checked
        cfg.HVVoltage = Voltage.Value
        cfg.T0Freq = T0Freq.Value
        cfg.SelfTriggerEnable = SelfEnable.Checked
        cfg.SelfTRiggerFreq = SelfFreq.Value
        cfg.MonitorMux = monitorMux.Text
        cfg.Channel = moniorCH.Value
        cfg.AnalogReadout = aAnalogRead.Checked
        cfg.ProcessMode = aProcessingMode.Text
        cfg.FileFormat = aFileFormat.Text
        cfg.ClusterTimens = aClusterTime.Value

        ReDim cfg.sA(asicCount)

        For q = 0 To asicCount - 1
            cfg.sA(q) = New ClassSettings.SingleAsicCFG()
            ReDim cfg.sA(q).sC(31)
            For z = 0 To 31
                cfg.sA(q).sC(z) = New ClassSettings.SingleAsicCFG.SignleChannelCFG
                cfg.sA(q).sC(z).BiasCompEnable = IIf(gridList(q).Rows(z).Cells("Enableb").Value = 0, False, True)
                cfg.sA(q).sC(z).BiasComp = gridList(q).Rows(z).Cells("DACb").Value = 0
                cfg.sA(q).sC(z).ChargeMask = IIf(gridList(q).Rows(z).Cells("DiscrQ").Value = 0, False, True)
                cfg.sA(q).sC(z).TimeMask = IIf(gridList(q).Rows(z).Cells("DiscrT").Value = 0, False, True)
                cfg.sA(q).sC(z).ThCompensation = gridList(q).Rows(z).Cells("THcomp").Value
                cfg.sA(q).sC(z).Gain = gridList(q).Rows(z).Cells("Gain").Value
                cfg.sA(q).sC(z).Offset = gridList(q).Rows(z).Cells("Offset").Value
            Next

        Next

        Return cfg

    End Function


    Public Function SetSettingsFromClass(ByRef cfg As ClassSettings) As Boolean

        Dim asicCount As Integer = 0

        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            asicCount += BI.totalAsics
        Next
        If cfg.AsicModel <> "PETIROC 2A" Then
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
            A_polarity.Text = cfg.SignalPolarity
            A_ChargeTHR.Value = cfg.ChargeThreshold
            A_TimeTHR.Value = cfg.TimeThreshold
            A_DelayBox.Value = cfg.DelayTrigger
            A_ShaperCF.Text = cfg.ShaperCF
            A_ShaperCI.Text = cfg.ShaperCI
            TransferSize.Text = cfg.TransferSize
            T0Mode.Text = cfg.T0Mode
            TimePsBin.Value = cfg.psbin
            HVon.Checked = cfg.HvOutputOn
            Voltage.Value = cfg.HVVoltage
            T0Freq.Value = cfg.T0Freq
            SelfEnable.Checked = cfg.SelfTriggerEnable
            SelfFreq.Value = cfg.SelfTRiggerFreq
            monitorMux.Text = cfg.MonitorMux
            moniorCH.Value = cfg.Channel
            aAnalogRead.Checked = cfg.AnalogReadout
            aProcessingMode.Text = cfg.ProcessMode
            aFileFormat.Text = cfg.FileFormat
            aClusterTime.Value = cfg.ClusterTimens


            Dim asC As Integer = IIf(cfg.AsicCount >= asicCount, asicCount, cfg.AsicCount)

            For q = 0 To asC - 1
                For z = 0 To 31
                    gridList(q).Rows(z).Cells("Enableb").Value = cfg.sA(q).sC(z).BiasCompEnable
                    gridList(q).Rows(z).Cells("DACb").Value = cfg.sA(q).sC(z).BiasComp
                    gridList(q).Rows(z).Cells("DiscrQ").Value = cfg.sA(q).sC(z).ChargeMask
                    gridList(q).Rows(z).Cells("DiscrT").Value = cfg.sA(q).sC(z).TimeMask
                    gridList(q).Rows(z).Cells("THcomp").Value = cfg.sA(q).sC(z).ThCompensation
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
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
                chk.HeaderText = "Mask Charge"
                chk.Name = "DiscrQ"
                chk = New DataGridViewCheckBoxColumn
                dgw.Columns.Add(chk)
                chk.HeaderText = "Mask Time"
                chk.Name = "DiscrT"
                dgw.Columns.Add("THcomp", "Threshold Compensation")
                dgw.Columns.Add("Gain", "Gain")
                dgw.Columns.Add("Offset", "Offset")
                dgw.Columns(0).Width = 120
                dgw.Columns(0).ReadOnly = True
                dgw.Columns(1).Width = 80
                dgw.Columns(2).Width = 80
                dgw.Columns(3).Width = 80
                dgw.Columns(4).Width = 80
                dgw.Columns(5).Width = 120
                dgw.Columns(6).Width = 80
                dgw.Columns(7).Width = 80
                dgw.RowHeadersVisible = False
                dgw.AllowUserToAddRows = False
                dgw.AllowUserToDeleteRows = False
                k = 0
                Do While (k < 32)
                    Dim kill As Integer = 0
                    If ((k = 15) _
                            OrElse ((k = 19) _
                            OrElse (k = 27))) Then
                        kill = 1
                    End If

                    Dim myString As String = k.ToString
                    dgw.Rows.Add(("CHANNEL " + myString), 1, 128, 0, kill, 32, 1.0, 0)
                    k = (k + 1)
                Loop
                TabControl1.TabPages.Add(ControlPage)
            Next


        Next

        A_polarity.Items.Add("Positive")
        A_polarity.Items.Add("Negative")
        A_polarity.SelectedIndex = 1

        A_ShaperCF.Items.Add("100fF")
        A_ShaperCF.Items.Add("200fF")
        A_ShaperCF.Items.Add("300fF")
        A_ShaperCF.Items.Add("400fF")
        A_ShaperCF.SelectedIndex = 0

        A_ShaperCI.Items.Add("1.25pF")
        A_ShaperCI.Items.Add("2.5pF")
        A_ShaperCI.Items.Add("3.75pF")
        A_ShaperCI.Items.Add("5pF")
        A_ShaperCI.SelectedIndex = 3

        A_ChargeTHR.Value = 800
        A_TimeTHR.Value = 800

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

        monitorMux.Items.Add("None")
        monitorMux.Items.Add("Bias DAC")
        monitorMux.Items.Add("Pream Output")
        monitorMux.Items.Add("Time Discriminator")
        monitorMux.Items.Add("Preamp Dummy")
        monitorMux.Items.Add("Ramp TDC")
        monitorMux.Items.Add("Chrage Discriminator")
        monitorMux.Items.Add("CR-RC")
        monitorMux.Items.Add("Start Of ADC Ramp")
        monitorMux.Items.Add("Hold")
        monitorMux.SelectedIndex = 0

        aProcessingMode.Items.Add("FULL")
        aProcessingMode.Items.Add("DECODE EVENTS")
        aProcessingMode.Items.Add("NONE")
        aProcessingMode.SelectedIndex = 0

        aFileFormat.Items.Add("CSV")
        aFileFormat.Items.Add("BINARY")
        aFileFormat.SelectedIndex = 0
    End Sub
    Public Sub UpdateSettings()
        For i = 0 To MainForm.DTList.Count - 1
            MainForm.DTList(i).pCFG.Clear()
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1

                Dim strPtrc As String
                Dim ProgramWord() As UInt32 = New UInt32((20) - 1) {}
                Dim pC As New PetirocConfig
                For z = 0 To 31
                    pC.inputDAC(z).enable = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("Enableb").Value = 0, False, True)
                    pC.inputDAC(z).value = gridList(i * BI.totalAsics + j).Rows(z).Cells("DACb").Value = 0
                    pC.InputDiscriminator(z).maskDiscriminatorQ = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("DiscrQ").Value = 0, False, True)
                    pC.InputDiscriminator(z).maskDiscriminatorT = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("DiscrT").Value = 0, False, True)
                    pC.InputDiscriminator(z).DACValue = gridList(i * BI.totalAsics + j).Rows(z).Cells("THcomp").Value
                    pC.Correction(i).Gain = gridList(i * BI.totalAsics + j).Rows(z).Cells("Gain").Value
                    pC.Correction(i).Offset = gridList(i * BI.totalAsics + j).Rows(z).Cells("Offset").Value
                Next

                pC.InputPolarity = IIf(A_polarity.SelectedIndex = 0, pC.tPOLARITY.POSITIVE, pC.tPOLARITY.NEGATIVE)
                pC.DAC_Q_threshold = A_ChargeTHR.Value
                pC.DAC_T_threshold = A_TimeTHR.Value
                pC.DelayTrigger = A_DelayBox.Value
                pC.SlowFeedbackC = A_ShaperCF.SelectedIndex
                pC.SlowInputC = A_ShaperCI.SelectedIndex
                pC.TriggerLatch = True
                pC.Raz_Ext = False
                pC.Raz_Int = True
                pC.GenerateUint32Config(ProgramWord)
                Dim boolDV(640) As Boolean
                Dim StringDV As String = ""
                pC.GenerateBitConfig(boolDV)
                For tt = 0 To 639
                    StringDV &= IIf(boolDV(tt), "1", "0")
                Next

                MainForm.AppendToLog(MainForm.LogMode.mCONFIGURATION, "Configure ASIC: " & j & " - " & MainForm.DTList(i).SerialNumber & vbCrLf & StringDV)

                Select Case j
                    Case 0
                        MainForm.DTList(i).ConfigPetiroc(True, False, False, False, ProgramWord)
                        MainForm.DTList(i).pCFG.Add(pC)
                    Case 1
                        MainForm.DTList(i).ConfigPetiroc(False, True, False, False, ProgramWord)
                        MainForm.DTList(i).pCFG.Add(pC)
                    Case 2
                        MainForm.DTList(i).ConfigPetiroc(False, False, True, False, ProgramWord)
                        MainForm.DTList(i).pCFG.Add(pC)
                    Case 3
                        MainForm.DTList(i).ConfigPetiroc(False, False, False, True, ProgramWord)
                        MainForm.DTList(i).pCFG.Add(pC)
                End Select
                Dim t0m As DT5550W_P_lib.DT5550W.T0Mode

                Select Case T0Mode.SelectedIndex
                    Case 0
                        t0m = DT5550W_P_lib.DT5550W.T0Mode.SOFTWARE_STARTRUN
                        MainForm.TSM = MainForm.TimeSpectrumMode.FIRST_REF
                    Case 1
                        t0m = DT5550W_P_lib.DT5550W.T0Mode.SOFTWARE_STARTRUN
                        MainForm.TSM = MainForm.TimeSpectrumMode.FIRST_REF_ASIC_0

                    Case 2
                        t0m = DT5550W_P_lib.DT5550W.T0Mode.SOFTWARE_PERIODIC
                        MainForm.TSM = MainForm.TimeSpectrumMode.T0REF

                    Case 3
                        t0m = DT5550W_P_lib.DT5550W.T0Mode.EXTERNAL
                        MainForm.TSM = MainForm.TimeSpectrumMode.T0REF

                End Select

                MainForm.DTList(i).ConfigureT0(T0Mode.SelectedIndex, T0Freq.Value)
                MainForm.TimePsBin = TimePsBin.Value
            Next
            System.Threading.Thread.Sleep(200)
            SetMonitor()
            MainForm.DTList(i).SetHV(HVon.Checked, Voltage.Value)
            MainForm.DTList(i).ConfigureSignalGenerator(SelfEnable.Checked,
                                            SelfEnable.Checked,
                                            SelfEnable.Checked,
                                            SelfEnable.Checked,
                                             SelfFreq.Value)
        Next

        MainForm.hvon.Enabled = Not HVon.Checked
        MainForm.hvoff.Enabled = HVon.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonSetCfg.Click

        UpdateSettings()

    End Sub

    Private Function SingleToHex(ByVal sing As Single)
        Dim arr = BitConverter.GetBytes(sing)
        Array.Reverse(arr)
        Return Convert.ToUInt32(BitConverter.ToString(arr).Replace("-", ""), 16)
    End Function

    Private Sub UploadSettings()

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub TransferSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TransferSize.SelectedIndexChanged
        MainForm.TransferSize = Math.Pow(10, TransferSize.SelectedIndex + 1)
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub T0Mode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles T0Mode.SelectedIndexChanged

    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles TimePsBin.ValueChanged

    End Sub

    Public Sub SetMonitor()
        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1

                Dim MonitorWord() As UInt32 = New UInt32((7) - 1) {}
                Dim pC As New PetirocConfig
                pC.PetirocMonitorSelector = monitorMux.SelectedIndex

                pC.MonitorChannel = moniorCH.Value
                pC.GenerateUint32Monitor(MonitorWord)


                Dim boolDV(195) As Boolean
                Dim StringDV As String = ""
                pC.GenerateBitMonitor(boolDV)
                For tt = 0 To 194
                    StringDV &= IIf(boolDV(tt), "1", "0")
                Next

                MainForm.AppendToLog(MainForm.LogMode.mCONFIGURATION, "Monitor ASIC: " & j & " - " & MainForm.DTList(i).SerialNumber & vbCrLf & StringDV)


                Select Case j
                    Case 0
                        MainForm.DTList(i).ConfigureMonitorPetiroc(True, False, False, False, MonitorWord)
                    Case 1
                        MainForm.DTList(i).ConfigureMonitorPetiroc(False, True, False, False, MonitorWord)
                    Case 2
                        MainForm.DTList(i).ConfigureMonitorPetiroc(False, False, True, False, MonitorWord)
                    Case 3
                        MainForm.DTList(i).ConfigureMonitorPetiroc(False, False, False, True, MonitorWord)
                End Select

                MainForm.DTList(i).EnableAnalogReadoutMonitor(aAnalogRead.Checked)

            Next
        Next
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonSetMonitor.Click
        SetMonitor()
    End Sub

    Private Sub aProcessingMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles aProcessingMode.SelectedIndexChanged
        Select Case aProcessingMode.SelectedIndex
            Case 0
                MainForm.CurrentProcessMode = MainForm.ProcessMode.ALL
            Case 1
                MainForm.CurrentProcessMode = MainForm.ProcessMode.EVENT_DECODE
            Case 2
                MainForm.CurrentProcessMode = MainForm.ProcessMode.OFF
        End Select

    End Sub

    Private Sub GroupBox3_Enter(sender As Object, e As EventArgs) Handles GroupBox3.Enter

    End Sub

    Private Sub aFileFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles aFileFormat.SelectedIndexChanged
        Select Case aFileFormat.SelectedIndex
            Case 0
                MainForm.SaveFileType = MainForm.FileType.CSV
            Case 1
                MainForm.SaveFileType = MainForm.FileType.BINARY
        End Select
    End Sub

    Private Sub aClusterTime_ValueChanged(sender As Object, e As EventArgs) Handles aClusterTime.ValueChanged
        Try
            MainForm.ClusterMaxTime = aClusterTime.Value
        Catch ex As Exception

        End Try

    End Sub

    Private Sub HVon_CheckedChanged(sender As Object, e As EventArgs) Handles HVon.CheckedChanged

    End Sub

    Private Sub aAnalogRead_CheckedChanged(sender As Object, e As EventArgs) Handles aAnalogRead.CheckedChanged

    End Sub
End Class