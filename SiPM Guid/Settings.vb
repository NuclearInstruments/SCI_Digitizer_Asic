Imports DT5550W_P_lib
Imports DT5550W_P_lib.DT5550W
Imports SiPM_Guid.AcquisitionClass
Imports System.Reflection
Public Class Settings

    Dim gridList As New List(Of DataGridView)
    Public Sub EnableDoubleBuffered(ByVal dgv As DataGridView)

        Dim dgvType As Type = dgv.[GetType]()

        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered",
                                                 BindingFlags.Instance Or BindingFlags.NonPublic)

        pi.SetValue(dgv, True, Nothing)

    End Sub

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
        Public TriggerMode As Integer
        Public ExternalStartDelay As Integer
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
    Public Function GetSettingsClass() As WeeRocAsicCommonSettings
        Dim cfg As New WeeRocAsicCommonSettings
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
        cfg.TriggerMode = TriggerMode.SelectedIndex
        cfg.ExternalStartDelay = ExtTrigDelay.Value
        ReDim cfg.sA(asicCount)

        For q = 0 To asicCount - 1
            cfg.sA(q) = New WeeRocAsicCommonSettings.SingleAsicCFG()
            ReDim cfg.sA(q).sC(31)
            For z = 0 To 31
                cfg.sA(q).sC(z) = New WeeRocAsicCommonSettings.SingleAsicCFG.SingleChannelCFG
                cfg.sA(q).sC(z).BiasCompEnable = IIf(gridList(q).Rows(z).Cells("Enableb").Value = 0, False, True)
                cfg.sA(q).sC(z).BiasComp = gridList(q).Rows(z).Cells("DACb").Value
                cfg.sA(q).sC(z).ChargeMask = IIf(gridList(q).Rows(z).Cells("DiscrQ").Value = 0, False, True)
                cfg.sA(q).sC(z).TimeMask = IIf(gridList(q).Rows(z).Cells("DiscrT").Value = 0, False, True)
                cfg.sA(q).sC(z).ThCompensation = gridList(q).Rows(z).Cells("THcomp").Value
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
        cfg.AsicModel = "PETIROC 2A"
        cfg.AsicCount = asicCount
        cfg.Timestamp = Now
        cfg.AppVersion = Application.ProductVersion.ToString
        cfg.SignalPolarity = "Positive"
        cfg.ChargeThreshold = 1000
        cfg.TimeThreshold = 1000
        cfg.DelayTrigger = 0
        cfg.ShaperCF = "100fF"
        cfg.ShaperCI = "5pF"
        cfg.TransferSize = "10 Events"
        cfg.T0Mode = "FIRST PHOTON"
        cfg.psbin = 50
        cfg.HvOutputOn = False
        cfg.HVVoltage = 56
        cfg.T0Freq = 1
        cfg.SelfTriggerEnable = False
        cfg.SelfTRiggerFreq = 1000
        cfg.MonitorMux = "None"
        cfg.Channel = 0
        cfg.AnalogReadout = False
        cfg.ProcessMode = "FULL"
        cfg.FileFormat = "CSV"
        cfg.ClusterTimens = 1000
        cfg.TriggerMode = "Internal Trigger"
        cfg.ExternalStartDelay = 0
        ReDim cfg.sA(asicCount)

        For q = 0 To asicCount - 1
            cfg.sA(q) = New WeeRocAsicCommonSettings.SingleAsicCFG()
            ReDim cfg.sA(q).sC(31)
            For z = 0 To 31
                cfg.sA(q).sC(z) = New WeeRocAsicCommonSettings.SingleAsicCFG.SingleChannelCFG
                cfg.sA(q).sC(z).BiasCompEnable = True
                cfg.sA(q).sC(z).BiasComp = 128
                cfg.sA(q).sC(z).ChargeMask = False
                cfg.sA(q).sC(z).TimeMask = False
                cfg.sA(q).sC(z).ThCompensation = 32
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
            TriggerMode.SelectedIndex = cfg.TriggerMode
            ExtTrigDelay.Value = cfg.ExternalStartDelay


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

    Private Sub DataGridView_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)

        If sender.SelectedCells.Count > 1 Then

            For Each r As DataGridViewCell In sender.SelectedCells
                If r.RowIndex <> e.RowIndex Then
                    sender.Rows(r.RowIndex).Cells(e.ColumnIndex).Value = sender.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                End If
            Next

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

        A_polarity.Items.Add("Positive")
        A_polarity.Items.Add("Negative")
        A_polarity.SelectedIndex = 0

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

        A_ChargeTHR.Value = 1000
        A_TimeTHR.Value = 1000

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


        TriggerSelector.Items.Add("Time Trigger")
        TriggerSelector.Items.Add("Charge Trigger")
        TriggerSelector.SelectedIndex = 0

        TempSensor.Items.Add("Internal (Avg)")
        TempSensor.Items.Add("External")
        TempSensor.SelectedIndex = 0

        TriggerMode.Items.Add("Internal Trigger")
        TriggerMode.Items.Add("INT/EXT Trigger")
        TriggerMode.Items.Add("External")
        TriggerMode.SelectedIndex = 0
    End Sub

    Public Sub UpdateSettings()
        MainForm.Timer3.Enabled = False
        MainForm.TriggerModeCharge = IIf(TriggerSelector.SelectedIndex = 1, True, False)

        For i = 0 To MainForm.DTList.Count - 1
            MainForm.DTList(i).PetirocClass.pCFG.Clear()
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1

                Dim strPtrc As String
                Dim ProgramWord() As UInt32 = New UInt32((20) - 1) {}
                Dim pC As New DT5550W_PETIROC.PetirocConfig
                For z = 0 To 31
                    pC.inputDAC(z).enable = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("Enableb").Value = 0, False, True)
                    pC.inputDAC(z).value = gridList(i * BI.totalAsics + j).Rows(z).Cells("DACb").Value
                    pC.InputDiscriminator(z).maskDiscriminatorQ = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("DiscrQ").Value = 0, False, True)
                    pC.InputDiscriminator(z).maskDiscriminatorT = IIf(gridList(i * BI.totalAsics + j).Rows(z).Cells("DiscrT").Value = 0, False, True)
                    pC.InputDiscriminator(z).DACValue = gridList(i * BI.totalAsics + j).Rows(z).Cells("THcomp").Value
                    pC.Correction(i).Gain = gridList(i * BI.totalAsics + j).Rows(z).Cells("Gain").Value
                    pC.Correction(i).Offset = gridList(i * BI.totalAsics + j).Rows(z).Cells("Offset").Value
                    MainForm.CorrPoints(j, z).gain = pC.Correction(i).Gain
                    MainForm.CorrPoints(j, z).Offset = pC.Correction(i).Offset

                Next

                pC.InputPolarity = IIf(A_polarity.SelectedIndex = 0, tPOLARITY.POSITIVE, tPOLARITY.NEGATIVE)
                pC.DAC_Q_threshold = IIf(A_polarity.SelectedIndex = 0, 1024 - A_ChargeTHR.Value, A_ChargeTHR.Value)
                pC.DAC_T_threshold = IIf(A_polarity.SelectedIndex = 0, 1024 - A_TimeTHR.Value, A_TimeTHR.Value)
                pC.DelayTrigger = A_DelayBox.Value
                pC.SlowFeedbackC = A_ShaperCF.SelectedIndex
                pC.SlowInputC = A_ShaperCI.SelectedIndex

                If TriggerSelector.SelectedIndex = 1 Then
                    pC.TriggerOut = False
                Else
                    pC.TriggerOut = True
                End If

                'disable T disciminator if external trigger is selected
                If TriggerMode.SelectedIndex = 2 Then
                    pC.PowerControl.DiscrT = False
                    pC.PowerControl.Delay_Discr = False
                    pC.PowerControl.Delay_Ramp = False
                    pC.PowerControl.TDC_ramp = False
                    pC.PulseMode.DiscrT = False
                    pC.PulseMode.Delay_Discr = False
                    pC.PulseMode.Delay_Ramp = False
                    pC.PulseMode.TDC_ramp = False
                    For z = 0 To 31
                        pC.InputDiscriminator(z).maskDiscriminatorQ = True
                        pC.InputDiscriminator(z).maskDiscriminatorT = True
                    Next
                Else
                    pC.PowerControl.DiscrT = True
                    pC.PowerControl.Delay_Discr = True
                    pC.PowerControl.Delay_Ramp = True
                    pC.PowerControl.TDC_ramp = True
                    pC.PulseMode.DiscrT = True
                    pC.PulseMode.Delay_Discr = True
                    pC.PulseMode.Delay_Ramp = True
                    pC.PulseMode.TDC_ramp = True
                End If

                MainForm.SoftwareThreshold = SoftwareTrigger.Value
                MainForm.InputPolarity = IIf(A_polarity.SelectedIndex = 0, tPOLARITY.POSITIVE, tPOLARITY.NEGATIVE)
                'pC.TriggerLatch = True
                'pC.Raz_Ext = False
                'pC.Raz_Int = True
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
                        MainForm.DTList(i).ConfigureAsic(True, False, False, False, ProgramWord)
                        MainForm.DTList(i).PetirocClass.pCFG.Add(pC)
                    Case 1
                        MainForm.DTList(i).ConfigureAsic(False, True, False, False, ProgramWord)
                        MainForm.DTList(i).PetirocClass.pCFG.Add(pC)
                    Case 2
                        MainForm.DTList(i).ConfigureAsic(False, False, True, False, ProgramWord)
                        MainForm.DTList(i).PetirocClass.pCFG.Add(pC)
                    Case 3
                        MainForm.DTList(i).ConfigureAsic(False, False, False, True, ProgramWord)
                        MainForm.DTList(i).PetirocClass.pCFG.Add(pC)
                End Select
                Dim t0m As DT5550W_P_lib.T0Mode

                Select Case T0Mode.SelectedIndex
                    Case 0
                        t0m = DT5550W_P_lib.T0Mode.SOFTWARE_STARTRUN
                        MainForm.TSM = MainForm.TimeSpectrumMode.FIRST_REF
                    Case 1
                        t0m = DT5550W_P_lib.T0Mode.SOFTWARE_STARTRUN
                        MainForm.TSM = MainForm.TimeSpectrumMode.FIRST_REF_ASIC_0

                    Case 2
                        t0m = DT5550W_P_lib.T0Mode.SOFTWARE_PERIODIC
                        MainForm.TSM = MainForm.TimeSpectrumMode.T0REF

                    Case 3
                        t0m = DT5550W_P_lib.T0Mode.EXTERNAL
                        MainForm.TSM = MainForm.TimeSpectrumMode.T0REF

                End Select

                MainForm.DTList(i).ConfigureT0(t0m, T0Freq.Value)
                MainForm.TimePsBin = TimePsBin.Value
            Next
            System.Threading.Thread.Sleep(200)
            SetMonitor()
            MainForm.DTList(i).SetHV(HVon.Checked, Voltage.Value, MaxV.Value)
            MainForm.DTList(i).ConfigureSignalGenerator(SelfEnable.Checked,
                                            SelfEnable.Checked,
                                            SelfEnable.Checked,
                                            SelfEnable.Checked,
                                             SelfFreq.Value)
            MainForm.DTList(i).SelectTriggerMode(MainForm.TriggerModeCharge)

            MainForm.DTList(i).EnableTriggerFrame(EnableGlobalTrigger.Checked)

            If TriggerMode.SelectedIndex = 0 Then
                MainForm.DTList(i).EnableExternalTrigger(False)
                MainForm.DTList(i).ConfigureExtHold(1, False)
            End If
            If TriggerMode.SelectedIndex = 1 Then
                MainForm.DTList(i).EnableExternalTrigger(True)
                MainForm.DTList(i).ConfigureExtHold(1, False)
            End If
            If TriggerMode.SelectedIndex = 2 Then
                MainForm.DTList(i).EnableExternalTrigger(False)
                MainForm.DTList(i).ConfigureExtHold(ExtTrigDelay.Value, True)
            End If


            MainForm.DTList(i).EnableExternalVeto(EnableExternalVeto.Checked)
            MainForm.DTList(i).EnableResetTDCOnT0(ResetTDConT0.Checked)
        Next

        MainForm.hvon.Enabled = Not HVon.Checked
        MainForm.hvoff.Enabled = HVon.Checked


        MainForm.TempSensorSource = TempSensor.SelectedIndex
        MainForm.DisableTempReadingAcq = DisableTempRead.Checked
        MainForm.EnableTempComp = TempComp.Checked
        MainForm.TempCompCoef = tempConmpCoef.Value
        MainForm.CurrentHVSet = Voltage.Value
        MainForm.CurrentHVON = HVon.Checked
        MainForm.CurrentHVMax = MaxV.Value

        MainForm.SumSpectrumGain = SumSpectrumGain.Value

        MainForm.Timer3.Enabled = True
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
                Dim pC As New DT5550W_PETIROC.PetirocConfig
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
                        MainForm.DTList(i).ConfigureMonitor(True, False, False, False, MonitorWord)
                    Case 1
                        MainForm.DTList(i).ConfigureMonitor(False, True, False, False, MonitorWord)
                    Case 2
                        MainForm.DTList(i).ConfigureMonitor(False, False, True, False, MonitorWord)
                    Case 3
                        MainForm.DTList(i).ConfigureMonitor(False, False, False, True, MonitorWord)
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

    Private Sub GroupBox3_Enter(sender As Object, e As EventArgs)

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
        If HVon.Checked = False Then
            MainForm.CurrentHVON = False
        End If
    End Sub

    Private Sub aAnalogRead_CheckedChanged(sender As Object, e As EventArgs) Handles aAnalogRead.CheckedChanged

    End Sub

    Private Sub NumericUpDown1_ValueChanged_1(sender As Object, e As EventArgs) Handles tempConmpCoef.ValueChanged

    End Sub

    Private Sub Label15_Click(sender As Object, e As EventArgs) Handles Label15.Click

    End Sub

    Private Sub TempComp_CheckedChanged(sender As Object, e As EventArgs) Handles TempComp.CheckedChanged
        If TempComp.Checked Then
            DisableTempRead.Checked = False
            DisableTempRead.Enabled = False
        Else
            DisableTempRead.Enabled = True
        End If
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
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

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        For i = 0 To MainForm.DTList.Count - 1
            Dim BI As t_BoardInfo = MainForm.DTList(i).GetBoardInfo
            For j = 0 To BI.totalAsics - 1
                For k = 0 To BI.channelsPerAsic - 1
                    gridList(j + BI.totalAsics * i).Rows(k).Cells("Offset").Value = Math.Round(-(MainForm.MatrixCumulativePerAsic(j, k) / MainForm.MatrixCumulativePerAsicCount(j, k) - 40))

                Next
            Next
        Next

    End Sub

    Private Sub TempSensor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TempSensor.SelectedIndexChanged

    End Sub

    Private Sub TabPage4_Click(sender As Object, e As EventArgs) Handles TabPage4.Click

    End Sub

    Private Sub TriggerMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TriggerMode.SelectedIndexChanged

    End Sub

    Private Sub SumSpectrumGain_ValueChanged(sender As Object, e As EventArgs) Handles SumSpectrumGain.ValueChanged

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

            Case WeeRocAsicCommonSettings.ScanMode.ScanCorrThreshold
                For q = 0 To AsicCount - 1
                    For z = 0 To 31
                        gridList(q).Rows(z).Cells("THcomp").Value = val
                    Next
                Next
                UpdateSettings()
            Case WeeRocAsicCommonSettings.ScanMode.ScanHoldDelay
                A_DelayBox.Value = val
                UpdateSettings()

            Case WeeRocAsicCommonSettings.ScanMode.ScanExternalDelay
                ExtTrigDelay.Value = val
                UpdateSettings()

        End Select
    End Sub



    Private Sub TabPage3_Click(sender As Object, e As EventArgs) Handles TabPage3.Click

    End Sub
End Class
