Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports WeifenLuo.WinFormsUI.Docking
Imports Newtonsoft.Json
Imports DT5550W_P_lib
Imports SciDigitizerAsic.DataStructures
Imports System.Text
Imports DT5550W_P_lib.DT5550W_HAL
Imports DT5550W_P_lib.DT5550W_PETIROC

Public Class MainForm

    Public GBL_ASIC_MODEL As t_AsicModels
    Public TransferSize As Integer = 10
    Public DTList As List(Of DT5550W_HAL)
    Public dt_obj As DT5550W_HAL = New DT5550W_HAL()
    Public nboard As Integer = 0

    Public GC As New List(Of AsicChannel)

    Public Ts As Double = 12.5
    'Dim scope As pScope = Nothing
    Dim msgcoda As New Queue
    Dim CurrentResources As SciCompilerExportClass
    Public CurrentOscilloscope As New Oscilloscope
    Public CurrentMCA As New FrameTransfers
    Public CurrentI2C As New iduec
    Public CurrentRegisterList As New List(Of Register)

    Public SoftwareThreshold As Integer
    Public InputPolarity As Integer

    Public TempSensorSource As Integer = 0
    Public DisableTempReadingAcq As Boolean = False
    Public EnableTempComp As Boolean = True
    Public TempCompCoef As Double = 0
    Public CurrentHVSet As Double = 25
    Public CurrentHVON As Boolean = False
    Public CurrentHVMax As Double = 25

    Public SumSpectrumGain As Double = 25

    Public Structure CorrPoint
        Public Offset
        Public Gain
    End Structure

    Public CorrPoints(16, 128) As CorrPoint

    Public fit_enabled = False
    Public board1_id As String
    Public board2_id As String
    Dim list_dockPanel As New List(Of DockContent)
    '  Public pprop As New pProperties
    Public dockPanel As New DockPanel()
    Public plog As New logwin
    Public fit As fit_win
    Public charge_trap As Boolean = True
    Public pRT4 As New pSpectra
    Public pRT4_HG As New pSpectra
    Public pRT5 As New pSpectra
    Public pRT6 As New pSpectra
    Public pRT7 As New pSpectra
    Public pRT8 As New pMonitor

    Public pRT9 As New ScanWindow
    '  Public stat As New Statistics
    Public map As New Mapping
    Public sets_petiroc As New Settings
    Public sets_citiroc As New Settings_Citiroc
    ' Public pRT3 As New pScope
    Public pRT As New pImmediate(True)
    Public pRT2 As New pImmediate(False)

    Public ACQLOCK As Boolean = False

    Public Enum TargetMode
        FreeRunning = 0
        Time_ns = 1
        Events = 2
        Clusters = 3
    End Enum
    Public RUN_TARGET_MODE As TargetMode
    Public RUN_TARGET_VALUE As Double

    Public sEventCounter As Long
    Public sClusterCounter As Long
    Public sTarget As Double
    Public sByteCounter As Long
    Public sTargetPercent As Double
    Public sTime As String
    Public sProcTime As Double
    Public sAcqTime As Double
    Public sStatus As String
    Public sTargetMode As String

    Public TriggerModeCharge As Boolean

    Public Enum FileType
        CSV = 0
        BINARY = 1
        JSON = 2
    End Enum

    Public Enum ProcessMode
        OFF = 0
        EVENT_DECODE = 1
        ALL = 2
    End Enum

    Public EnableSaveFile As Boolean
    Public SaveFilePath As String
    Public SaveFileType As FileType = 0
    Public CurrentProcessMode As ProcessMode = 2

    Public ClusterMaxTime As Int64 = 1000
    Public RunCompleted As Boolean = False

    Public Structure ChannelScanPoint
        Public channel As Integer
        Public value As Double
        Public Overrides Function ToString() As String
            Return String.Format("{0};{1}", channel, value.ToString.Replace(",", "."))
        End Function
    End Structure
    Public Structure ScanPoint
        Public Time As Double
        Public ParamValue As Double
        Public ScanValue As List(Of ChannelScanPoint)
    End Structure

    Public Class ScanResult
        Public Title As String
        Public LitOfScanPoint As List(Of ScanPoint)
        Public Valid As Boolean
        Public Sub New()
            LitOfScanPoint = New List(Of ScanPoint)
        End Sub

        Public Sub Clear()
            Valid = False
            LitOfScanPoint.Clear()
        End Sub

        Public Sub PushData(_time As Double, _pv As Double, _sv As List(Of ChannelScanPoint))
            Dim newSP As New ScanPoint
            newSP.Time = _time
            newSP.ParamValue = _pv
            newSP.ScanValue = _sv
            LitOfScanPoint.Add(newSP)
            Valid = True
        End Sub

    End Class

    Public LastScanResult As New ScanResult


    Public Sub UpdateOscilloscope()

        Select Case GBL_ASIC_MODEL
            Case t_AsicModels.PETIROC
                Dim PMD As New DT5550W_P_lib.DT5550W_PETIROC.PetirocMonitorData(2048)
                DTList(pRT8.ComboBox1.SelectedIndex).GetMonitorPetiroc(PMD, 2048)
                pRT8.Plot(PMD)

            Case t_AsicModels.CITIROC
                Dim PMD As New CitirocMonitorData(1024)
                DTList(pRT8.ComboBox1.SelectedIndex).GetMonitorCitiroc(PMD)
                pRT8.Plot(PMD)


        End Select



    End Sub


    'Public Sub LoadEEData()
    '    If ReadAFEBoardParametersI2C(SN_AFE, AFE_MINBIAS, AFE_MAXBIAS) = False Then
    '        AppendToLog("[ERROR] No AFE BOARD Connected")
    '    Else
    '        AppendToLog("[INFO] Connected to AFE SN: " & SN_AFE)
    '    End If

    '    If ReadDETECTORBoardParametersI2C(SN_SENSORE, SENSORE_TIPO, SN_MANSENSORE, BIAS_SENSORE) = False Then
    '        AppendToLog("[ERROR] No Detector Board Connected")
    '    Else
    '        AppendToLog("[INFO] Connected to Detector Board SN: " & SN_SENSORE)
    '        AppendToLog("[INFO] Detector is: " & SENSORE_TIPO & vbTab & " MAN/SN: " & SN_MANSENSORE)
    '        AppendToLog("[INFO] Detector operation voltage is: " & BIAS_SENSORE)
    '        '  pprop.vBias.Value = BIAS_SENSORE
    '    End If
    'End Sub
    Public Enum LogMode
        mINFO
        mWARNING
        mERROR
        mMESSAGE
        mACQUISITION
        mPROCESS
        mCONFIGURATION
    End Enum

    Public Function EnableDisableUpdate(stato As Boolean)

        Select Case GBL_ASIC_MODEL
            Case t_AsicModels.PETIROC
                sets_petiroc.EnableDisableUpdate(False)
            Case t_AsicModels.CITIROC
                sets_citiroc.EnableDisableUpdate(False)
            Case t_AsicModels.MAROC
            Case t_AsicModels.PHOTOROC
            Case t_AsicModels.GEMROC
        End Select

    End Function
    Public Sub AppendToLog(mode As LogMode, text As String, Optional boardid As String = "")
        Dim space = 4
        Select Case mode
            Case LogMode.mINFO
                msgcoda.Enqueue("[INFO  " & Now & "] ".PadRight(space, " ") & text)
            Case LogMode.mWARNING
                msgcoda.Enqueue("[WRNG  " & Now & "] ".PadRight(space, " ") & text)
            Case LogMode.mERROR
                msgcoda.Enqueue("[ERROR " & Now & "] ".PadRight(space, " ") & text)
            Case LogMode.mMESSAGE
                msgcoda.Enqueue("[" & Now & "] ".PadRight(space, " ") & text)
            Case LogMode.mACQUISITION
                msgcoda.Enqueue("[ACQ  (" & boardid & ") " & Now & "] ".PadRight(space, " ") & text)
            Case LogMode.mPROCESS
                msgcoda.Enqueue("[CORE (" & boardid & ") " & Now & "] ".PadRight(space, " ") & text)
            Case LogMode.mCONFIGURATION
                msgcoda.Enqueue("[CFG  (" & boardid & ") " & Now & "] ".PadRight(space, " ") & text)


        End Select


    End Sub


    Private Function GetDockContentForm(name As String, showHint As DockState, backColour As Color) As DockContent
        Dim content1 As New DockContent()
        content1.Name = name
        content1.TabText = name
        content1.Text = name
        content1.ShowHint = showHint
        content1.BackColor = backColour
        Return content1
    End Function



    Public Sub CreateGUI()

        dockPanel.Dock = DockStyle.Fill
        dockPanel.BackColor = Color.White
        Controls.Add(dockPanel)
        dockPanel.BringToFront()

        Dim content4 As DockContent = GetDockContentForm("Log File", DockState.DockBottom, Color.White)
        content4.Show(dockPanel)
        content4.CloseButtonVisible = False
        plog.Dock = DockStyle.Fill
        content4.Controls.Add(plog)
        dockPanel.DockBottomPortion = 0.15
        list_dockPanel.Add(content4)


        '  
        If GBL_ASIC_MODEL = t_AsicModels.PETIROC Then
            Dim content1a As DockContent = GetDockContentForm("Energy", DockState.Document, Color.White)
            content1a.Show(dockPanel)
            content1a.CloseButtonVisible = False
            content1a.Controls.Add(pRT4)
            pRT4.Dock = DockStyle.Fill
            list_dockPanel.Add(content1a)
            pRT4.Pesgo1.PeString.MainTitle = "Energy Spectrum"
            AppendToLog(LogMode.mINFO, "Creating panel Energy Spectrum")

        End If

        If GBL_ASIC_MODEL = t_AsicModels.CITIROC Then
            Dim content1a As DockContent = GetDockContentForm("Energy LG", DockState.Document, Color.White)
            content1a.Show(dockPanel)
            content1a.CloseButtonVisible = False
            content1a.Controls.Add(pRT4)
            pRT4.Dock = DockStyle.Fill
            list_dockPanel.Add(content1a)
            pRT4.Pesgo1.PeString.MainTitle = "Energy Spectrum - Low Gain"
            pRT4.SetRebin(8)
            AppendToLog(LogMode.mINFO, "Creating panel Energy Spectrum")

            Dim content1a_HG As DockContent = GetDockContentForm("Energy HG", DockState.Document, Color.White)
            content1a_HG.Show(dockPanel)
            content1a_HG.CloseButtonVisible = False
            content1a_HG.Controls.Add(pRT4_HG)
            pRT4_HG.Dock = DockStyle.Fill
            list_dockPanel.Add(content1a)
            pRT4_HG.Pesgo1.PeString.MainTitle = "Energy Spectrum - High Gain"
            pRT4_HG.SetRebin(8)
            AppendToLog(LogMode.mINFO, "Creating panel Energy Spectrum for high Gain")
        End If


        If GBL_ASIC_MODEL = t_AsicModels.PETIROC Then
            Dim content1e As DockContent = GetDockContentForm("Time Distribution", DockState.Document, Color.White)
            content1e.Show(dockPanel)
            content1e.CloseButtonVisible = False

            content1e.Controls.Add(pRT5)
            pRT5.Dock = DockStyle.Fill
            list_dockPanel.Add(content1e)
            pRT5.Pesgo1.PeString.MainTitle = "Time distribution"
            AppendToLog(LogMode.mINFO, "Creating panel Time Distribution")

        End If

        Dim content1f As DockContent = GetDockContentForm("Hit per channel", DockState.Document, Color.White)
        content1f.Show(dockPanel)
        content1f.CloseButtonVisible = False
        content1f.Controls.Add(pRT6)
        pRT6.Dock = DockStyle.Fill
        list_dockPanel.Add(content1f)
        '        Dim strlist As New List(Of String)
        '        strlist.Add("All Channels")
        pRT6.UpdateChannels_AllChannels()
        pRT6.Pesgo1.PeString.MainTitle = "Hit per channel"
        AppendToLog(LogMode.mINFO, "Creating panel Hit per channel")


        Dim content1g As DockContent = GetDockContentForm("Analog Monitor", DockState.Document, Color.White)
        content1g.Show(dockPanel)
        content1g.CloseButtonVisible = False
        content1g.Controls.Add(pRT7)
        pRT7.Dock = DockStyle.Fill
        list_dockPanel.Add(content1g)
        pRT7.Pesgo1.PeString.MainTitle = "Analog Monitor"
        AppendToLog(LogMode.mINFO, "Creating panel Analog Monitor")

        Dim content1h As DockContent = GetDockContentForm("ASIC Monitor", DockState.Document, Color.White)
        content1h.Show(dockPanel)
        content1h.CloseButtonVisible = False
        content1h.Controls.Add(pRT8)
        pRT8.Dock = DockStyle.Fill
        list_dockPanel.Add(content1h)
        AppendToLog(LogMode.mINFO, "Creating panel ASIC Monitor")

        Dim content5 As DockContent = GetDockContentForm("Scan", DockState.Document, Color.White)
        content5.Show(dockPanel)
        content5.CloseButtonVisible = False
        content5.Controls.Add(pRT9)
        pRT9.Dock = DockStyle.Fill
        list_dockPanel.Add(content5)
        AppendToLog(LogMode.mINFO, "Creating panel Rate & Scan")

        Select Case GBL_ASIC_MODEL
            Case t_AsicModels.PETIROC
                pRT8.CreatePlotTopPetiroc()
            Case t_AsicModels.CITIROC
                pRT8.CreatePlotTopCitiroc()
        End Select

        Dim content1c As DockContent = GetDockContentForm("Settings", DockState.Document, Color.White)
        content1c.Show(dockPanel)
        content1c.CloseButtonVisible = False
        Select Case GBL_ASIC_MODEL
            Case t_AsicModels.PETIROC
                content1c.Controls.Add(sets_petiroc)
                sets_petiroc.Dock = DockStyle.Fill
            Case t_AsicModels.CITIROC
                content1c.Controls.Add(sets_citiroc)
                sets_citiroc.Dock = DockStyle.Fill
            Case t_AsicModels.MAROC
            Case t_AsicModels.PHOTOROC
            Case t_AsicModels.GEMROC
        End Select
        list_dockPanel.Add(content1c)
        AppendToLog(LogMode.mINFO, "Creating panel Settings")

        Dim content1d As DockContent = GetDockContentForm("Mapping", DockState.Document, Color.White)
        content1d.Show(dockPanel)
        content1d.CloseButtonVisible = False
        content1d.Controls.Add(map)
        map.Dock = DockStyle.Fill
        list_dockPanel.Add(content1d)
        AppendToLog(LogMode.mINFO, "Creating panel Mapping")

        Dim content2 As DockContent = GetDockContentForm("Real Time View", DockState.DockRight, Color.White)
        content2.Show(dockPanel)
        content2.CloseButtonVisible = False
        content2.Controls.Add(pRT)
        pRT.Dock = DockStyle.Fill
        list_dockPanel.Add(content2)
        AppendToLog(LogMode.mINFO, "Creating panel Energy Hit Map (Realtime)")

        Dim content3 As DockContent = GetDockContentForm("Cumulative", DockState.Float, Color.White)
        content3.Show(dockPanel)
        content3.CloseButtonVisible = False
        content3.DockHandler.FloatPane.DockTo(dockPanel.DockWindows(DockState.DockRight))
        content3.Controls.Add(pRT2)
        pRT2.Dock = DockStyle.Fill
        list_dockPanel.Add(content3)
        AppendToLog(LogMode.mINFO, "Creating panel Energy (Cumulative)")





    End Sub

    Public Function OpenConfiguration()




        Dim oDialog As New OpenFileDialog
        oDialog.Filter = "Nuclear Instruments Application Settings (*.nias)|*.nias"

        Try

            If oDialog.ShowDialog() = DialogResult.OK Then

                Using sReader As New StreamReader(oDialog.FileName)
                    Dim sc As New WeeRocAsicCommonSettings
                    Dim x As New Xml.Serialization.XmlSerializer(sc.GetType)
                    sc = x.Deserialize(sReader)


                    If GBL_ASIC_MODEL = t_AsicModels.PETIROC Then
                        sets_petiroc.SetSettingsFromClass(sc)
                    Else
                        If GBL_ASIC_MODEL = t_AsicModels.CITIROC Then
                            sets_citiroc.SetSettingsFromClass(sc)
                        End If
                    End If

                    Dim asicCount = 0
                    Dim ChannelCount = 0
                    For i = 0 To DTList.Count - 1
                        Dim BI As t_BoardInfo = DTList(i).GetBoardInfo
                        asicCount += BI.totalAsics
                        ChannelCount += BI.channelsPerAsic * BI.totalAsics
                    Next

                    Dim asC As Integer = IIf(sc.sMap.Count >= ChannelCount, ChannelCount, sc.sMap.Count)


                    If sc.AsicCount > asicCount Then
                        AppendToLog(MainForm.LogMode.mWARNING, "The configuration file has a number of ASIC different from the current setup")
                    End If

                    For i = 0 To sc.sMap.Count - 1
                        Dim found = False
                        For j = 0 To GC.Count - 1
                            If GC(j).Board = sc.sMap(i).Board And
                                GC(j).Asic = sc.sMap(i).Asic And
                                GC(j).Channel = sc.sMap(i).Channel Then
                                GC(j).X = sc.sMap(i).X
                                GC(j).Y = sc.sMap(i).Y
                                GC(j).ValidLocation = sc.sMap(i).ValidLocation
                                GC(j).Veto = sc.sMap(i).Veto
                                found = True
                            End If
                        Next
                        If found = False Then
                            AppendToLog(MainForm.LogMode.mWARNING, "Unable to map " & sc.sMap(i).Channel & " on board " & sc.sMap(i).Board & " ASIC " & sc.sMap(i).Asic)
                        End If
                    Next
                    map.DefaultLayout()


                End Using

                'My.Settings.Save()
                'ReloadSettings()
                MessageBox.Show("Import of settings successfull. Rember to apply settings!", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            AppendToLog(LogMode.mINFO, "Configuration from file: " & oDialog.SafeFileName & " loaded successfully")
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
            AppendToLog(LogMode.mERROR, "Error loading file: " & oDialog.SafeFileName & " - " & ex.Message)
        End Try

    End Function
    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click
        OpenConfiguration()
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "File di testo (*.txt)|*.txt|Tutti i file (*.*)|*.*"

        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName
            ' TODO: aggiungere qui il codice per il salvataggio del contenuto corrente del form in un file.

            AppendToLog(LogMode.mINFO, "Save to file: " & FileName)
        End If
    End Sub


    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub



    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        Me.ToolStrip.Visible = Me.ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Chiude tutti i form figlio del form padre.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private m_ChildFormNumber As Integer

    Public Shared acquisition As New AcquisitionClass(32)
    Public SaveFolderDefault As String

    Public Sub ReloadSettings()
        'pprop.bBias.Checked = My.Settings.bBIAS
        ''pprop.vBias.Value = My.Settings.BIAS
        ''  pprop.trMode.SelectedIndex = My.Settings.trMode
        'pprop.trLevel.Value = My.Settings.trLevel
        'pprop.dDelay.Value = My.Settings.dDelay
        'pprop.trDelay.Value = My.Settings.trDelay
        'pprop.dDelay2.Value = My.Settings.dDelay2
        'pprop.trDelay2.Value = My.Settings.trDelay2
        'pprop.trHold.Value = My.Settings.trHold
        'pprop.prEn.Checked = My.Settings.prEn
        'pprop.prTime.Value = My.Settings.prTime
        'pprop.iTime.Value = My.Settings.iTime
        'pprop.blCor.SelectedIndex = My.Settings.blCor
        'pprop.blOffset.Value = My.Settings.blOffset
        'pprop.nf_b.Checked = My.Settings.nf_b
        'pprop.dGain.Value = My.Settings.dGain
        'pprop.cBoard.Checked = My.Settings.cBoard
        'SaveFolderDefault = My.Settings.folderpos
        'pprop.bLowRate.Checked = My.Settings.bLowRate
        'pprop.bLowRate.Checked = My.Settings.bseparateThreshold
        'Try
        '    Dim pvbias = My.Settings.biasGainSettings.ToString.Split(";")
        '    For i = 0 To pvbias.Length - 1
        '        pprop.nudBiasVar(i).Value = pvbias(i)
        '    Next
        'Catch ex As Exception

        'End Try
        'Try
        '    Dim pst = My.Settings.separateTresholdValues.ToString.Split(";")
        '    For i = 0 To pst.Length - 1
        '        pprop.nThresholdTrigger(i).Value = pst(i)
        '    Next
        'Catch ex As Exception

        'End Try
    End Sub
    Public Sub AssignDefaultBoardChannelMapping()

        Dim boardCM() As DT5550W_P_lib.tChannelMapping
        Dim Xoffset = 0
        Dim pixOffset = 0
        For Each dt In DTList
            If (dt.GetDefaultChannelMapping(boardCM)) Then
                For i = 0 To boardCM.Length - 1
                    GC(pixOffset).Asic = boardCM(i).ASICID
                    GC(pixOffset).X = Xoffset + boardCM(i).X
                    GC(pixOffset).Y = boardCM(i).Y

                    pixOffset += 1
                Next
            End If

            Dim BI = dt.GetBoardInfo
            Xoffset += BI.totalAsics * 4
        Next


    End Sub

    Public Sub AssignDefaultChannelMapping()
        GC.Clear()
        Dim brdCnt = 0
        For Each dt In DTList
            Dim boardInfo As t_BoardInfo = dt.GetBoardInfo
            If boardInfo.DefaultDetectorLayout = t_BoardInfo.t_DefaultDetectorLayout.MATRIX_8x8 Then
                For i = 0 To boardInfo.totalAsics - 1
                    For j = 0 To boardInfo.channelsPerAsic - 1
                        Dim ch As New AsicChannel
                        ch.Board = boardInfo.SerialNumber
                        ch.Asic = i
                        ch.Channel = j
                        ch.Y = Math.Floor(j / 4) + 8 * brdCnt
                        ch.X = (i * 4) + (j Mod 4)
                        ch.Veto = False
                        ch.ValidLocation = True
                        GC.Add(ch)
                    Next
                Next
            End If

            If boardInfo.DefaultDetectorLayout = t_BoardInfo.t_DefaultDetectorLayout.LINEAR Then
                For i = 0 To boardInfo.totalAsics - 1
                    For j = 0 To boardInfo.channelsPerAsic - 1
                        Dim ch As New AsicChannel
                        ch.Board = boardInfo.SerialNumber
                        ch.Asic = i
                        ch.Channel = j
                        ch.Y = i
                        ch.X = j + (boardInfo.totalAsics * brdCnt)
                        ch.Veto = False
                        ch.ValidLocation = True
                        GC.Add(ch)
                    Next
                Next
            End If

            brdCnt += 1
        Next



        Exit Sub
        Dim xx = {2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1}
        Dim yy = {0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 7, 7, 6, 6, 5, 5, 4, 4, 3, 3, 2, 2, 1, 1, 0, 0}
        'Dim xx = {1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2}
        'Dim yy = {3, 3, 2, 2, 1, 1, 0, 0, 7, 7, 6, 6, 5, 5, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7, 0, 0, 1, 1, 2, 2, 3, 3}

        For j = 0 To 31
            GC(j).Y = yy(j)
            GC(j).X = xx(j)

            '    If j < 16 Then
            '    GC(j).Y = yy(j + 16)
            '    GC(j).X = xx(j + 16)
            '    Else
            '    GC(j).Y = yy(j - 16)
            '    GC(j).X = xx(j - 16)
            '    End If
        Next

        For j = 0 To 31
            GC(j + 32).Y = yy(j)
            GC(j + 32).X = 4 + xx(j)

            '   If j < 16 Then
            '       GC(j + 32).Y = yy(j + 16)
            '   GC(j + 32).X = 4 + xx(j + 16)
            '   Else
            '   GC(j + 32).Y = yy(j - 16)
            '   GC(j + 32).X = 4 + xx(j - 16)
            '   End If
            '            GC(j + 32).Y = yy(j)
            '           GC(j + 32).X = 4 + xx(j)

        Next

        AppendToLog(LogMode.mINFO, "Restored defaul channel mapping")
    End Sub

    Public Sub UpdateChannels()
        'Dim chl As New List(Of String)
        Dim EX As Int16, EY As Int16
        EX = 0
        EY = 0

        Dim CHnL As New pSpectra.ChnData
        Dim CHL As New List(Of pSpectra.ChnData)

        Dim i = 0
        For Each eGC In GC
            EX = IIf(eGC.X > EX, eGC.X, EX)
            EY = IIf(eGC.Y > EY, eGC.Y, EY)
            'chl.Add("[" & eGC.Y + 1 & "," & eGC.X + 1 & "]")
            If eGC.ValidLocation Then
                CHnL.ID = i
                CHnL.X = eGC.X + 1
                CHnL.Y = eGC.Y + 1
                CHnL.BOARD = eGC.Board.Substring(4)
                CHnL.MODE = "channel"
            End If
            CHL.Add(CHnL)
            i = i + 1
        Next

        pRT5.UpdateChannels(CHL)
        CHnL.ID = SperctrumSumIndex
        CHnL.X = 0
        CHnL.Y = 0
        CHnL.BOARD = "0000"
        CHnL.MODE = "sum"
        CHL.Add(CHnL)
        pRT4.UpdateChannels(CHL)
        pRT4_HG.UpdateChannels(CHL)

        pRT9.UpdateChannels(CHL)

        'pRT5.UpdateChannels(CHL)
        pRT.pImmediate_ReLoad(EY + 1, EX + 1)
        pRT2.pImmediate_ReLoad(EY + 1, EX + 1)

        'Dim anChList As New List(Of String)
        Dim anChList As New List(Of pSpectra.ChnData)
        Dim q = 0
        For Each dt In DTList
            Dim BI = dt.GetBoardInfo
            For i = 0 To BI.totalAsics - 1
                CHnL.ID = i + q * BI.totalAsics
                CHnL.X = i
                CHnL.BOARD = dt.SerialNumber.Substring(4)
                anChList.Add(CHnL)
                'anChList.Add("ASIC " & i & " - " & dt.SerialNumber)
            Next
            q = q + 1
        Next

        pRT7.UpdateChannels_ASIC(anChList)
        pRT8.ComboBox1.Items.Clear()
        For Each dt In DTList
            pRT8.ComboBox1.Items.Add(dt.SerialNumber)
        Next
        If pRT8.ComboBox1.Items.Count > 0 Then
            pRT8.ComboBox1.SelectedIndex = 0
        End If


    End Sub

    Public Sub MDIParent_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '  rdc = New MADAReadOut
        '  rdc.caller = Me
        ' rdc.ConnectToBoard(board1, board2)
        DTList = New List(Of DT5550W_HAL)
        If Connection.ShowDialog() Then
            Dim newDt As New DT5550W_HAL
            If Connection.ComboBox3.Text <> "Auto" Then
                Dim AM As t_AsicModels = t_AsicModels.PETIROC
                If Connection.ComboBox2.Text.ToUpper.StartsWith("PETIROC") Then
                    AM = t_AsicModels.PETIROC
                End If
                If Connection.ComboBox2.Text.ToUpper.StartsWith("CITIROC") Then
                    AM = t_AsicModels.CITIROC
                End If
                GBL_ASIC_MODEL = AM
                newDt.Connect(Connection.ComboBox1.Text, AM, Connection.ComboBox3.Text)
                AppendToLog(LogMode.mINFO, "Manual Daughter Board configuration: ")
                AppendToLog(LogMode.mINFO, " --> MODEL:  " & Connection.ComboBox2.Text)
                AppendToLog(LogMode.mINFO, " --> ASIC N: " & Connection.ComboBox3.Text)


            Else
                Dim model As String = "AAAAAAAAAAAAAAAAAAAAAAAAA"
                Dim asic_count As Integer
                Dim SN As Integer
                newDt.Connect(Connection.ComboBox1.Text)
                newDt.GetDGCardModel(model, asic_count, SN)
                AppendToLog(LogMode.mINFO, " --> MODEL:  " & model)
                AppendToLog(LogMode.mINFO, " --> ASIC N: " & asic_count)
                AppendToLog(LogMode.mINFO, " --> SN:     " & SN)

                If model = "PET" Then
                    GBL_ASIC_MODEL = t_AsicModels.PETIROC
                End If
                If model = "CIT" Then
                    GBL_ASIC_MODEL = t_AsicModels.CITIROC
                End If
            End If


            AppendToLog(LogMode.mINFO, "Connected to board: " & Connection.ComboBox1.Text)
            Dim build As UInt32
            newDt.GetBuild(build)
            AppendToLog(LogMode.mINFO, "Build firmware version: " & Hex(build))

            DTList.Add(newDt)
            AssignDefaultChannelMapping()
            AssignDefaultBoardChannelMapping()
        Else
            End
            wRun.Enabled = False
            wSingle.Enabled = False
            wRunStop.Enabled = False
            Wspectrum.Enabled = False
            sRun.Enabled = False
            WspectrumStop.Enabled = False
            hvon.Enabled = False
            hvoff.Enabled = False
            RunToolStripMenuItem.Enabled = False
            MonitorToolStripMenuItem.Enabled = False
            ConfigurationToolStripMenuItem.Enabled = False

        End If

        If GBL_ASIC_MODEL = t_AsicModels.PETIROC Then
            Allocator(1024)
            EnegySpectrumHGToolStripMenuItem.Visible = False
            ScanResultToolStripMenuItem.Visible = False
        End If

        If GBL_ASIC_MODEL = t_AsicModels.CITIROC Then
            Allocator(16384)
            EnegySpectrumHGToolStripMenuItem.Visible = True
            ExportEnergySpectrumToolStripMenuItem.Text = "Energy Spectrum (LG)"
            ScanResultToolStripMenuItem.Visible = True
        End If


        'Dim file As String = My.Settings.JsnFile 'Connection.ComClass.JsonFilePath
        'Create(file)
        acquisition = New AcquisitionClass(CurrentOscilloscope.Channels)
        CreateGUI()
        ReloadSettings()

        UpdateChannels()



        Timer3.Enabled = True 

    End Sub



    Private Sub wRun_Click(sender As Object, e As EventArgs) Handles wRun.Click
        UpdateOscilloscope()



    End Sub

    Private Sub MainForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed

    End Sub
    Public Sub RunAcquisitionRun()
        'running = True

        Dim g As New RunStart
        If g.ShowDialog = DialogResult.OK Then
            Select Case g.cTargetMode.SelectedIndex
                Case 0
                    RUN_TARGET_MODE = TargetMode.FreeRunning

                Case 1
                    RUN_TARGET_MODE = TargetMode.Events

                Case 2
                    RUN_TARGET_MODE = TargetMode.Clusters

                Case 3
                    RUN_TARGET_MODE = TargetMode.Time_ns
            End Select
            RUN_TARGET_VALUE = g.TargetValue
            RunCompleted = False
            EnableSaveFile = True
            SaveFilePath = g.FilePathName

            sStatus = "RUNNING"

            StartRun()

        End If

    End Sub
    Private Sub sRun_Click(sender As Object, e As EventArgs) Handles sRun.Click
        RunAcquisitionRun()
    End Sub

    Private Sub MenuStrip_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip.ItemClicked

    End Sub

    Public Sub RunOscilloscope()
        Timer3.Enabled = False
        AppendToLog(LogMode.mINFO, "Starting monitor oscilloscope with periodic update")
        Timer2.Enabled = True

        wSingle.Enabled = False
        wRun.Enabled = False
        sRun.Enabled = False
        Wspectrum.Enabled = False
        wRun.Enabled = False
        wRunStop.Enabled = True
        WspectrumStop.Enabled = False


        StartFreeRunToolStripMenuItem.Enabled = False
        StartAcquisitionRunToolStripMenuItem.Enabled = False
        StopRunToolStripMenuItem.Enabled = True
        SingleShotToolStripMenuItem.Enabled = False
        PeriodicToolStripMenuItem.Enabled = False
        StopToolStripMenuItem.Enabled = False
        ConfigureAllASICToolStripMenuItem.Enabled = False
        ACQLOCK = True

        EnableDisableUpdate(True)
        map.EnableDisableUpdate(True)
    End Sub
    Private Sub wSingle_Click(sender As Object, e As EventArgs) Handles wSingle.Click
        RunOscilloscope()
        '  rdc.cTargetMode = 0

    End Sub


    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs)
        pRT4.StopDataCaptureOnFile()
        pRT4_HG.StopDataCaptureOnFile()
        'wSingle.Enabled = True
        ' wRun.Enabled = False
        sRun.Enabled = True
        'Wspectrum.Enabled = False
        'WspectrumStop.Enabled = True
        '   hvoff.Enabled = Not hvon.Enabled
    End Sub

    Public Sub StopRun()

        pRT4.stopspectrum()
        pRT4_HG.stopspectrum()
        pRT5.stopspectrum()
        pRT6.stopspectrum()
        pRT7.stopspectrum()
        'rdc.StopAcquisition()
        'rdc.StopDataCaptureOnFile()
        pRT.StopLive()
        pRT2.StopLive()

        wSingle.Enabled = True
        wRun.Enabled = True
        sRun.Enabled = True
        wRunStop.Enabled = False
        Wspectrum.Enabled = True
        WspectrumStop.Enabled = False
        '  pprop.Button1.Enabled = True
        System.Threading.Thread.Sleep(100)
        running = False
        sStatus = "IDLE"


        StartFreeRunToolStripMenuItem.Enabled = True
        StartAcquisitionRunToolStripMenuItem.Enabled = True
        StopRunToolStripMenuItem.Enabled = False
        SingleShotToolStripMenuItem.Enabled = True
        PeriodicToolStripMenuItem.Enabled = True
        StopToolStripMenuItem.Enabled = False
        ConfigureAllASICToolStripMenuItem.Enabled = True

        EnableDisableUpdate(False)


        map.EnableDisableUpdate(False)

        AppendToLog(LogMode.mACQUISITION, "Run completed")

        ACQLOCK = False
        Timer3.Enabled = True
    End Sub
    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles WspectrumStop.Click

        StopRun()

    End Sub

    Public MatrixCumulative(1) As UInt64
    Public MatrixCumulativePerAsic(16, 64) As UInt64
    Public MatrixCumulativePerAsicCount(16, 64) As UInt64
    Public MatrixInst(1) As UInt64
    Public RawESpectrum(1, 1024) As UInt64
    Public RawE_HG_Spectrum(1, 1024) As UInt64
    Public RawTSpectrum(1, 1024) As UInt64
    Public RawHitCounter(1, 1) As UInt64
    Public AnalogMonitor(1, 1) As UInt64
    Public ConnectedChannels As UInt32
    Public SperctrumSumIndex As UInt32
    Public Sub Allocator(EnergyChannels As Integer)
        Dim TotalChannels = 0
        Dim TotalAsic = 0
        Dim maxChnA = 0
        For Each dt In DTList
            Dim BI As t_BoardInfo = dt.GetBoardInfo
            TotalChannels += BI.totalChannels
            TotalAsic += BI.totalAsics
            maxChnA = IIf(BI.channelsPerAsic > maxChnA, BI.channelsPerAsic, maxChnA)

        Next

        ReDim RawESpectrum(TotalChannels + 1, EnergyChannels)
        ReDim RawE_HG_Spectrum(TotalChannels + 1, EnergyChannels)
        ReDim RawTSpectrum(TotalChannels, 8192)
        ReDim RawHitCounter(1, TotalChannels)
        ReDim MatrixCumulative(TotalChannels)
        ReDim MatrixInst(TotalChannels)
        ReDim AnalogMonitor(TotalAsic, maxChnA)
        SperctrumSumIndex = TotalChannels
    End Sub

    Public Sub ResetLive()
        Dim r1, r2
        r1 = RawESpectrum.GetUpperBound(0)
        r2 = RawESpectrum.GetUpperBound(1)
        For i = 0 To r1
            For j = 0 To r2
                RawESpectrum(i, j) = 0
            Next
        Next

        r1 = RawE_HG_Spectrum.GetUpperBound(0)
        r2 = RawE_HG_Spectrum.GetUpperBound(1)
        For i = 0 To r1
            For j = 0 To r2
                RawE_HG_Spectrum(i, j) = 0
            Next
        Next

        r1 = RawTSpectrum.GetUpperBound(0)
        r2 = RawTSpectrum.GetUpperBound(1)
        For i = 0 To r1
            For j = 0 To r2
                RawTSpectrum(i, j) = 0
            Next
        Next

        r1 = MatrixCumulative.GetUpperBound(0)
        For i = 0 To r1
            MatrixCumulative(i) = 0
        Next

        For i = 0 To 15
            For j = 0 To 63
                MatrixCumulativePerAsic(i, j) = 0
                MatrixCumulativePerAsicCount(i, j) = 0
            Next
        Next


        r1 = MatrixInst.GetUpperBound(0)
        For i = 0 To r1
            MatrixInst(i) = 0
        Next

        r1 = RawHitCounter.GetUpperBound(1)
        For i = 0 To r1
            RawHitCounter(0, i) = 0
        Next

        AppendToLog(LogMode.mACQUISITION, "Reset live data")
    End Sub
    Public Class Cluster
        Public timecode As UInt64
        Public timecode_ns As Double
        Public Runtimecode As UInt64
        Public Runtimecode_ns As Double

        Public id As UInt64
        Public Events As New List(Of t_DataPETIROC)
    End Class


    Public Class Cluster_CITIROC
        Public timecode As UInt64
        Public timecode_ns As Double
        Public Runtimecode As UInt64
        Public Runtimecode_ns As Double

        Public id As UInt64
        Public Events As New List(Of t_DataCITIROC)
    End Class

    Public Enum TimeSpectrumMode
        T0REF = 0
        FIRST_REF = 1
        FIRST_REF_ASIC_0 = 2
    End Enum

    Public TSM As TimeSpectrumMode = 2
    Public TimePsBin As UInt64 = 70
    Dim StartTime As Date



    Public Sub AcquisitionThread(board As DT5550W_HAL, file As String, TransferSize As Integer, BoardArrayOffset As Integer)


        Dim BI As t_BoardInfo = board.GetBoardInfo
        Dim Buffer(BI.DigitalDataPacketSize * TransferSize * 2) As UInt32
        Dim ValidWord As UInt32 = 0
        board.FlushFIFO()

        board.RUNControl(False)
        Dim Events As New Queue(Of DT5550W_PETIROC.t_DataPETIROC)
        Dim Clusters As New List(Of Cluster)
        Dim DataDecodedCitiroc As New Queue(Of DT5550W_P_lib.t_DataCITIROC)
        Dim Clusters_Citiroc As New List(Of Cluster_CITIROC)
        Dim TotalEvents As Int64 = 0
        Dim DecodedEvents As Int64 = 0
        Dim EventsBefore As Int64 = 0
        Dim TotalClusters As Int64 = 0
        Dim DecodedClusters As Int64 = 0
        Dim ClustersBefore As Int64 = 0
        Dim DURATION As TimeSpan
        Dim DwnTime As Date
        Dim tDwnTime As TimeSpan
        Dim ProcTime As Date
        Dim tProcTime As TimeSpan

        Dim CurrentTimecode As Double = 0
        Dim EventCounter As Int64 = 0

        Dim AsicCount = 4
        Dim tx As StreamWriter = Nothing
        Dim bwriter As BinaryWriter = Nothing

        AppendToLog(LogMode.mACQUISITION, "Starting acquisition", BI.SerialNumber)
        If EnableSaveFile = True Then
            AppendToLog(LogMode.mACQUISITION, "List mode save file is enabled", BI.SerialNumber)
            If SaveFileType = FileType.CSV Then
                tx = New StreamWriter(SaveFilePath)
                AppendToLog(LogMode.mACQUISITION, "File is CSV", BI.SerialNumber)
                AppendToLog(LogMode.mACQUISITION, "Saving on file: " & SaveFilePath, BI.SerialNumber)
            End If
            If SaveFileType = FileType.BINARY Then
                bwriter = New BinaryWriter(New FileStream(SaveFilePath, IO.FileMode.Create))
                AppendToLog(LogMode.mACQUISITION, "File is BINARY", BI.SerialNumber)
                AppendToLog(LogMode.mACQUISITION, "Saving on file: " & SaveFilePath, BI.SerialNumber)
            End If
        End If

        If CurrentProcessMode = ProcessMode.OFF Then
            AppendToLog(LogMode.mPROCESS, "Live Process is disabled", BI.SerialNumber)
        End If

        If CurrentProcessMode = ProcessMode.EVENT_DECODE Then
            AppendToLog(LogMode.mPROCESS, "Live Process is enabled: EVENTS only decoded", BI.SerialNumber)
        End If

        If CurrentProcessMode = ProcessMode.ALL Then
            AppendToLog(LogMode.mPROCESS, "Live Process is enabled: EVENTS and CLUSTERS decoded", BI.SerialNumber)
        End If


        If CurrentProcessMode = ProcessMode.EVENT_DECODE Then

            If SaveFileType = FileType.CSV And EnableSaveFile = True Then
                Dim strline = ""
                Select Case GBL_ASIC_MODEL
                    Case t_AsicModels.PETIROC
                        strline = "ID;ASIC;EventCounter;RUN_EventTimeCodeLSB;RUN_EventTimecode_ns;T0_to_Event_Timecode;T0_to_Event_Timecode_ns;"
                        For i = 0 To BI.channelsPerAsic - 1
                            strline &= $"HIT_{i};"
                        Next
                        For i = 0 To BI.channelsPerAsic - 1
                            strline &= $"CHARGE_{i};"
                        Next
                        For i = 0 To BI.channelsPerAsic - 1
                            strline &= $"COARSE_{i};"
                        Next
                        For i = 0 To BI.channelsPerAsic - 1
                            strline &= $"FINE_{i};"
                        Next
                        For i = 0 To BI.channelsPerAsic - 1
                            strline &= $"RELATIVETIME_{i};"
                        Next
                        strline = strline.Remove(strline.Length - 1)


                    Case t_AsicModels.CITIROC
                        strline = "ID;ASIC;EventCounter;RUN_EventTimeCodeLSB;RUN_EventTimecode_ns;T0_to_Event_Timecode;T0_to_Event_Timecode_ns;"
                        For i = 0 To BI.channelsPerAsic - 1
                            strline &= $"HIT_{i};"
                        Next
                        For i = 0 To BI.channelsPerAsic - 1
                            strline &= $"CHARGE_LG_{i};"
                        Next
                        For i = 0 To BI.channelsPerAsic - 1
                            strline &= $"CHARGE_HG_{i};"
                        Next
                        strline = strline.Remove(strline.Length - 1)

                End Select



                tx.WriteLine(strline)

            End If
        End If

        If CurrentProcessMode > ProcessMode.EVENT_DECODE Then
            If SaveFileType = FileType.CSV And EnableSaveFile = True Then
                Dim strline = ""
                Select Case GBL_ASIC_MODEL
                    Case t_AsicModels.PETIROC
                        strline = "ID_CLUSTER;CLUSTER_RUN_Timecode_ns;CLUSTER_Timecode_ns;NEventsInCluster;"

                        For asi = 0 To BI.totalAsics - 1
                            strline &= $"ASIC_{asi};EventCounter_{asi};RUN_EventTimeCodeLSB_{asi};RUN_EventTimecode_ns_{asi};T0_to_Event_Timecode_{asi};T0_to_Event_Timecode_ns_{asi};"
                            For i = 0 To BI.channelsPerAsic - 1
                                strline &= $"HIT_{asi}_{i};"
                            Next
                            For i = 0 To BI.channelsPerAsic - 1
                                strline &= $"CHARGE_{asi}_{i};"
                            Next
                            For i = 0 To BI.channelsPerAsic - 1
                                strline &= $"COARSE_{asi}_{i};"
                            Next
                            For i = 0 To BI.channelsPerAsic - 1
                                strline &= $"FINE_{asi}_{i};"
                            Next
                            For i = 0 To BI.channelsPerAsic - 1
                                strline &= $"RELATIVETIME_{asi}_{i};"
                            Next
                        Next

                        strline = strline.Remove(strline.Length - 1)

                    Case t_AsicModels.CITIROC
                        strline = "ID_CLUSTER;CLUSTER_RUN_Timecode_ns;CLUSTER_Timecode_ns;NEventsInCluster;"

                        For asi = 0 To BI.totalAsics - 1
                            strline &= $"ASIC_{asi};EventCounter_{asi};RUN_EventTimeCodeLSB_{asi};RUN_EventTimecode_ns_{asi};T0_to_Event_Timecode_{asi};T0_to_Event_Timecode_ns_{asi};"
                            For i = 0 To BI.channelsPerAsic - 1
                                strline &= $"HIT_{asi}_{i};"
                            Next
                            For i = 0 To BI.channelsPerAsic - 1
                                strline &= $"CHARGE_LG_{asi}_{i};"
                            Next
                            For i = 0 To BI.channelsPerAsic - 1
                                strline &= $"CHARGE_HG_{asi}_{i};"
                            Next

                        Next

                        strline = strline.Remove(strline.Length - 1)
                End Select

                tx.WriteLine(strline)

            End If
        End If

        Dim TempTime = Now
        sByteCounter = 0
        StartTime = Now

        board.RUNControl(True)
        While running
            DwnTime = Now
            If DisableTempReadingAcq = False Then
                If (Now - TempTime).TotalMilliseconds > 10000 Then
                    Dim tA = -1000, tb = -1000
                    If TempSensorSource = 0 Then
                        board.GetSensTemperature(0, tA)
                        If board.GetBoardInfo.totalAsics > 2 Then
                            board.GetSensTemperature(1, tb)
                        End If
                    Else
                        board.GetSensTemperature(2, tA)
                    End If

                    If EnableTempComp Then
                        Dim tAv = 0
                        If tb > -150 Then
                            tAv = (tA + tb) / 2
                        Else
                            tAv = tA
                        End If
                        board.SetHVTempFB(CurrentHVON, CurrentHVSet, CurrentHVMax, TempCompCoef, tAv)
                    End If

                    Me.Invoke(Sub()
                                  plog.UpdateSiPMTemp(tA, tb)
                                  Dim enable, voltage, current
                                  If board.GetHV(enable, voltage, current) Then
                                      plog.UpdateHvStatus(enable, voltage, current)
                                  End If

                              End Sub
                    )


                    TempTime = Now
                End If
            End If


            'boa
            tDwnTime = Now - DwnTime
            sAcqTime = tDwnTime.TotalMilliseconds

            'If CurrentProcessMode = ProcessMode.OFF Then
            DURATION = DateTime.Now - StartTime
            sTime = DURATION.Hours.ToString.PadLeft(2, "0"c) & ":" &
                        DURATION.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                        DURATION.Seconds.ToString.PadLeft(2, "0"c) & "." &
                        DURATION.Milliseconds.ToString.PadLeft(3, "0"c)
            'End If
            ProcTime = Now
            DecodedClusters = 0
            If GBL_ASIC_MODEL = t_AsicModels.PETIROC Then

                board.GetRawBuffer(Buffer, TransferSize, 4000, BI.DigitalDataPacketSize, ValidWord)
                If EnableSaveFile = True Then   'FILE SAVE
                    If SaveFileType = FileType.BINARY Then
                        For i = 0 To ValidWord - 1
                            bwriter.Write(Buffer(i))
                        Next
                        sByteCounter += ValidWord * 4
                    End If
                End If


                If CurrentProcessMode > ProcessMode.OFF Then
                    EventsBefore = TotalEvents
                    ClustersBefore = TotalClusters
                    DecodedEvents = board.DecodePetirocRowEvents(Buffer, ValidWord, Events, 0, InputPolarity)



                    If CurrentProcessMode = ProcessMode.EVENT_DECODE Then
                        While Events.Count > 0


                            Dim strline = ""
                            Dim e = Events.Dequeue

                            For j = 0 To BI.channelsPerAsic - 1
                                Dim vtemp As Double = (CType(e.charge(j), Double) + CorrPoints(e.AsicID, j).Offset) * CorrPoints(e.AsicID, j).Gain
                                vtemp = IIf(vtemp < 0, 0, vtemp)
                                vtemp = IIf(vtemp > 1023, 1023, vtemp)
                                e.charge(j) = vtemp
                                e.charge(j) = IIf(e.charge(j) > SoftwareThreshold, e.charge(j), 0)
                            Next

                            If SaveFileType = FileType.CSV And EnableSaveFile = True Then
                                Dim hitNumber(e.hit.Count - 1) As UInt16
                                For i = 0 To e.hit.Count - 1
                                    hitNumber(i) = IIf(e.hit(i), 1, 0)
                                Next
                                strline &= TotalEvents & ";" & e.AsicID & ";" & e.EventCounter & ";" & e.RunEventTimecode & ";" & e.RunEventTimecode_ns & ";" & e.EventTimecode & ";" & e.EventTimecode_ns & ";" & String.Join(";", hitNumber) & ";" & String.Join(";", e.charge) & ";" & String.Join(";", e.CoarseTime) & ";" & String.Join(";", e.FineTime) & ";" & String.Join(";", e.relative_time)
                                tx.WriteLine(strline)
                                sByteCounter += strline.Length
                            End If
                            TotalEvents += 1
                            CurrentTimecode = e.RunEventTimecode_ns

                            For t = 0 To MatrixInst.GetUpperBound(0)
                                MatrixInst(t) = 0
                            Next

                            Dim EnergySum = 0
                            Dim SpXIndx = BoardArrayOffset + (e.AsicID * BI.channelsPerAsic)
                            For j = 0 To BI.channelsPerAsic - 1
                                Dim c = SpXIndx + j
                                If c < RawESpectrum.GetUpperBound(0) Then
                                    If e.charge(j) > 4 Then
                                        RawESpectrum(c, e.charge(j)) += 1
                                        EnergySum += e.charge(j)
                                        'RawESpectrum(SperctrumSumIndex, e.charge(j)) += 1
                                    End If
                                    MatrixCumulative(c) += e.charge(j) ' quest due righe erano dentro hit
                                    MatrixCumulativePerAsic(e.AsicID, j) = MatrixCumulative(c)
                                    MatrixCumulativePerAsicCount(e.AsicID, j) += 1
                                    MatrixInst(c) = e.charge(j)

                                    If e.hit(j) = True Then
                                        Dim time As Double = 0
                                        time = e.Time(j) 'e.EventTimecode_ns + e.relative_time(j)
                                        Dim deltaTime = time
                                        deltaTime = (deltaTime * 1000) / (TimePsBin)
                                        If deltaTime >= 0 And deltaTime < RawTSpectrum.GetUpperBound(1) Then
                                            RawTSpectrum(c, deltaTime) += 1
                                        End If
                                        RawHitCounter(0, c) += 1
                                    End If
                                End If
                            Next


                            Dim spesum_bin
                            spesum_bin = Math.Round(Math.Min(EnergySum * SumSpectrumGain, 1023))
                            If spesum_bin > 0 Then
                                RawESpectrum(SperctrumSumIndex, spesum_bin) += 1
                            End If

                            TotalEvents += DecodedEvents
                            pRT4.PostData(RawESpectrum, RawESpectrum.GetUpperBound(1))
                            pRT5.PostData(RawTSpectrum, RawTSpectrum.GetUpperBound(1))
                            pRT6.PostData(RawHitCounter, RawHitCounter.GetUpperBound(1))
                            If Clusters.Count > 0 Then
                                For Each e In Clusters.Last.Events
                                    For j = 0 To e.charge.GetUpperBound(0) - 1
                                        AnalogMonitor(e.AsicID, j) = e.charge(j)
                                    Next
                                Next
                            End If
                            pRT7.PostData(AnalogMonitor, AnalogMonitor.GetUpperBound(1))

                        End While
                    End If
                    If CurrentProcessMode > ProcessMode.EVENT_DECODE Then
                        Clusters.Clear()
                        Try
                            While Events.Count >= AsicCount
                                Dim strline = ""
                                Dim first = Events.Dequeue
                                Dim newCluster = New Cluster

                                newCluster.timecode = first.EventTimecode
                                newCluster.Runtimecode = first.RunEventTimecode
                                newCluster.timecode_ns = newCluster.timecode * BI.FPGATimecode_ns
                                newCluster.Runtimecode_ns = newCluster.Runtimecode * BI.FPGATimecode_ns
                                newCluster.id = EventCounter
                                CurrentTimecode = newCluster.Runtimecode_ns

                                EventCounter += 1

                                newCluster.Events.Add(first)
                                While Math.Abs(CType((Events.Peek).RunEventTimecode, Int64) - CType(newCluster.Runtimecode, Int64)) < ClusterMaxTime
                                    newCluster.Events.Add(Events.Dequeue)
                                    If Events.Count = 0 Then
                                        Exit While
                                    End If
                                End While

                                'aggiusta i timecode relativi degli eventi
                                Dim timecode_min As UInt64 = UInt64.MaxValue
                                Dim Runtimecode_min As UInt64 = UInt64.MaxValue
                                For Each e In newCluster.Events
                                    timecode_min = IIf(e.EventTimecode < timecode_min, e.EventTimecode, timecode_min)
                                    Runtimecode_min = IIf(e.RunEventTimecode < Runtimecode_min, e.RunEventTimecode, Runtimecode_min)
                                Next

                                If EnableSaveFile = True Then   'FILE SAVE
                                    If SaveFileType = FileType.CSV Then
                                        strline = EventCounter & ";" & newCluster.Runtimecode_ns & ";" & newCluster.timecode_ns
                                        strline &= ";" & newCluster.Events.Count
                                    End If
                                End If

                                newCluster.timecode = timecode_min
                                newCluster.Runtimecode = Runtimecode_min
                                newCluster.timecode_ns = newCluster.timecode * BI.FPGATimecode_ns
                                newCluster.Runtimecode_ns = newCluster.Runtimecode * BI.FPGATimecode_ns
                                For Each e In newCluster.Events

                                    For j = 0 To BI.channelsPerAsic - 1
                                        Dim vtemp As Double = (CType(e.charge(j), Double) + CorrPoints(e.AsicID, j).Offset) * CorrPoints(e.AsicID, j).Gain
                                        vtemp = IIf(vtemp < 0, 0, vtemp)
                                        vtemp = IIf(vtemp > 1023, 1023, vtemp)
                                        e.charge(j) = vtemp

                                        e.charge(j) = vtemp '(CType(e.charge(j), Double) + CorrPoints(e.AsicID, j).Offset) * CorrPoints(e.AsicID, j).Gain
                                        e.charge(j) = IIf(e.charge(j) > SoftwareThreshold, e.charge(j), 0)
                                    Next


                                    e.EventTimecode = timecode_min
                                    e.EventTimecode_ns = e.EventTimecode * BI.FPGATimecode_ns
                                    e.RunEventTimecode = Runtimecode_min
                                    e.RunEventTimecode_ns = e.RunEventTimecode * BI.FPGATimecode_ns
                                    If EnableSaveFile = True Then   'FILE SAVE
                                        If SaveFileType = FileType.CSV Then
                                            Dim hitNumber(e.hit.Count - 1) As UInt16
                                            For i = 0 To e.hit.Count - 1
                                                hitNumber(i) = IIf(e.hit(i), 1, 0)
                                            Next
                                            strline &= ";" & e.AsicID & ";" & e.EventCounter & ";" & e.RunEventTimecode & ";" & e.RunEventTimecode_ns & ";" & e.EventTimecode & ";" & e.EventTimecode_ns & ";" & String.Join(";", hitNumber) & ";" & String.Join(";", e.charge) & ";" & String.Join(";", e.CoarseTime) & ";" & String.Join(";", e.FineTime) & ";" & String.Join(";", e.relative_time)
                                        End If
                                    End If
                                Next

                                Clusters.Add(newCluster)
                                DecodedClusters += 1

                                If EnableSaveFile = True Then       'FILE SAVE
                                    If SaveFileType = FileType.CSV Then
                                        tx.WriteLine(strline)
                                        sByteCounter += strline.Length
                                    End If
                                End If


                            End While

                        Catch ex As Exception
                            'MsgBox(ex.Message)
                            AppendToLog(LogMode.mPROCESS, "Process ERROR: " & ex.Message, BI.SerialNumber)
                        End Try


                        'For i = ClustersBefore To ClustersBefore + DecodedClusters - 1
                        For i = 0 To Clusters.Count - 1
                            Try
                                Dim TimeRef As Int64 = 1024 * 25000
                                If TSM = TimeSpectrumMode.FIRST_REF Then

                                    For Each e In Clusters(i).Events
                                        For j = 0 To BI.channelsPerAsic - 1
                                            If e.hit(j) Then
                                                Dim time As Double = e.Time(j) ' e.EventTimecode_ns + e.relative_time(j)
                                                TimeRef = IIf(time < TimeRef, time, TimeRef)
                                            End If
                                            'Console.WriteLine(e.FineTime(j))

                                        Next
                                    Next
                                End If


                                If TSM = TimeSpectrumMode.FIRST_REF_ASIC_0 Then

                                    For Each e In Clusters(i).Events
                                        If e.AsicID = 0 Then
                                            For j = 0 To BI.channelsPerAsic - 1
                                                If e.hit(j) Then
                                                    Dim time As Double = e.Time(j) ' e.EventTimecode_ns + e.relative_time(j)
                                                    TimeRef = IIf(time < TimeRef, time, TimeRef)
                                                End If
                                                'Console.WriteLine(e.FineTime(j))

                                            Next
                                        End If
                                    Next
                                End If
                                For t = 0 To MatrixInst.GetUpperBound(0)
                                    MatrixInst(t) = 0
                                Next
                                Dim EnergySum As Double = 0
                                For Each e In Clusters(i).Events

                                    Dim SpXIndx = BoardArrayOffset + (e.AsicID * BI.channelsPerAsic)

                                    For j = 0 To BI.channelsPerAsic - 1
                                        Dim c = SpXIndx + j
                                        If c < RawESpectrum.GetUpperBound(0) Then
                                            If e.charge(j) > 4 Then
                                                RawESpectrum(c, e.charge(j)) += 1
                                                EnergySum += e.charge(j)
                                            End If

                                            MatrixCumulative(c) += e.charge(j) ' quest due righe erano dentro hit
                                            MatrixCumulativePerAsic(e.AsicID, j) = MatrixCumulative(c)
                                            MatrixCumulativePerAsicCount(e.AsicID, j) += 1
                                            MatrixInst(c) = e.charge(j)
                                            If e.hit(j) = True Then

                                                Dim time As Double = 0


                                                If TSM = TimeSpectrumMode.FIRST_REF Or TimeSpectrumMode.FIRST_REF_ASIC_0 Then
                                                    time = e.Time(j) 'e.EventTimecode_ns + e.relative_time(j)
                                                End If

                                                If TSM = TimeSpectrumMode.T0REF Then
                                                    time = e.Time(j) 'Clusters(i).timecode_ns + e.EventTimecode_ns + e.relative_time(j)
                                                    TimeRef = 0
                                                End If

                                                Dim deltaTime = time - TimeRef
                                                deltaTime = (deltaTime * 1000) / (TimePsBin)
                                                If deltaTime >= 0 And deltaTime < RawTSpectrum.GetUpperBound(1) Then
                                                    RawTSpectrum(c, deltaTime) += 1
                                                End If
                                                RawHitCounter(0, c) += 1
                                            End If
                                        End If
                                    Next



                                Next

                                Dim spesum_bin
                                spesum_bin = Math.Round(Math.Min(EnergySum * SumSpectrumGain, 1023))

                                If spesum_bin > 0 Then
                                    RawESpectrum(SperctrumSumIndex, spesum_bin) += 1
                                End If
                            Catch ex As Exception
                                MsgBox(ex.Message)
                                AppendToLog(LogMode.mPROCESS, "Process ERROR: " & ex.Message, BI.SerialNumber)
                            End Try
                        Next
                        TotalClusters += DecodedClusters
                        TotalEvents += DecodedEvents
                        pRT4.PostData(RawESpectrum, RawESpectrum.GetUpperBound(1))
                        pRT5.PostData(RawTSpectrum, RawTSpectrum.GetUpperBound(1))
                        pRT6.PostData(RawHitCounter, RawHitCounter.GetUpperBound(1))
                        If Clusters.Count > 0 Then
                            For Each e In Clusters.Last.Events
                                For j = 0 To e.charge.GetUpperBound(0) - 1
                                    AnalogMonitor(e.AsicID, j) = e.charge(j)
                                Next
                            Next
                        End If
                        pRT7.PostData(AnalogMonitor, AnalogMonitor.GetUpperBound(1))
                    End If
                End If

            End If


            If GBL_ASIC_MODEL = t_AsicModels.CITIROC Then
                board.GetRawBuffer(Buffer, TransferSize, 4000, BI.DigitalDataPacketSize, ValidWord)
                If EnableSaveFile = True Then   'FILE SAVE
                    If SaveFileType = FileType.BINARY Then
                        For i = 0 To ValidWord - 1
                            bwriter.Write(Buffer(i))
                        Next
                        sByteCounter += ValidWord * 4
                    End If
                End If
                If CurrentProcessMode > ProcessMode.OFF Then
                    board.CitirocPushRawDataInBuffer(Buffer, ValidWord)
                    DecodedEvents = board.DecodeCitirocRowEvents(DataDecodedCitiroc, SoftwareThreshold)
                    If CurrentProcessMode = ProcessMode.EVENT_DECODE Then

                        While (DataDecodedCitiroc.Count > 0)
                            Dim e = DataDecodedCitiroc.Dequeue()
                            TotalEvents += 1


                            Dim strline = ""


                            For j = 0 To BI.channelsPerAsic - 1
                                Dim vtemp As Double = (CType(e.chargeLG(j), Double) + CorrPoints(e.AsicID, j).Offset) * CorrPoints(e.AsicID, j).Gain
                                vtemp = IIf(vtemp < 0, 0, vtemp)
                                vtemp = IIf(vtemp > 16383, 16383, vtemp)
                                e.chargeLG(j) = vtemp
                                e.chargeLG(j) = IIf(e.chargeLG(j) > SoftwareThreshold, e.chargeLG(j), 0)
                            Next

                            If SaveFileType = FileType.CSV And EnableSaveFile = True Then
                                Dim hitNumber(e.hit.Count - 1) As UInt16
                                For i = 0 To e.hit.Count - 1
                                    hitNumber(i) = IIf(e.hit(i), 1, 0)
                                Next
                                strline &= TotalEvents & ";" & e.AsicID & ";" & e.EventCounter & ";" & e.RunEventTimecode & ";" & e.RunEventTimecode_ns & ";" & e.EventTimecode & ";" & e.EventTimecode_ns & ";" & String.Join(";", hitNumber) & ";" & String.Join(";", e.chargeLG) & ";" & String.Join(";", e.chargeHG)
                                tx.WriteLine(strline)
                                sByteCounter += strline.Length
                            End If
                            TotalEvents += 1
                            CurrentTimecode = e.RunEventTimecode_ns

                            For t = 0 To MatrixInst.GetUpperBound(0)
                                MatrixInst(t) = 0
                            Next

                            Dim EnergySum = 0
                            Dim EnergySum_HG = 0
                            Dim SpXIndx = BoardArrayOffset + (e.AsicID * BI.channelsPerAsic)
                            For j = 0 To BI.channelsPerAsic - 1
                                Dim c = SpXIndx + j
                                If c < RawESpectrum.GetUpperBound(0) Then
                                    If e.chargeLG(j) > 0 Then
                                        RawESpectrum(c, e.chargeLG(j)) += 1
                                        EnergySum += e.chargeLG(j)
                                        'RawESpectrum(SperctrumSumIndex, e.charge(j)) += 1
                                    End If

                                    If e.chargeHG(j) > 0 Then
                                        RawE_HG_Spectrum(c, e.chargeHG(j)) += 1
                                        EnergySum_HG += e.chargeHG(j)
                                    End If
                                    MatrixCumulative(c) += e.chargeLG(j) ' quest due righe erano dentro hit
                                    MatrixCumulativePerAsic(e.AsicID, j) = MatrixCumulative(c)
                                    MatrixCumulativePerAsicCount(e.AsicID, j) += 1
                                    MatrixInst(c) = e.chargeLG(j)

                                    If e.hit(j) = True Then
                                        Dim time As Double = 0
                                        time = e.chargeLG(j) 'e.EventTimecode_ns + e.relative_time(j)
                                        Dim deltaTime = time
                                        deltaTime = (deltaTime * 1000) / (TimePsBin)
                                        If deltaTime >= 0 And deltaTime < RawTSpectrum.GetUpperBound(1) Then
                                            RawTSpectrum(c, deltaTime) += 1
                                        End If
                                        RawHitCounter(0, c) += 1
                                    End If
                                End If
                            Next


                            Dim spesum_bin
                            spesum_bin = Math.Round(Math.Min(EnergySum * SumSpectrumGain, 1023))
                            If spesum_bin > 0 Then
                                RawESpectrum(SperctrumSumIndex, spesum_bin) += 1
                            End If

                            Dim spesum_bin_HG
                            spesum_bin_HG = Math.Round(Math.Min(EnergySum_HG * SumSpectrumGain, 1023))
                            If spesum_bin > 0 Then
                                RawE_HG_Spectrum(SperctrumSumIndex, spesum_bin_HG) += 1
                            End If


                            TotalEvents += DecodedEvents
                            pRT4.PostData(RawESpectrum, RawESpectrum.GetUpperBound(1))
                            pRT4_HG.PostData(RawE_HG_Spectrum, RawE_HG_Spectrum.GetUpperBound(1))
                            pRT5.PostData(RawTSpectrum, RawTSpectrum.GetUpperBound(1))
                            pRT6.PostData(RawHitCounter, RawHitCounter.GetUpperBound(1))

                            pRT7.PostData(AnalogMonitor, AnalogMonitor.GetUpperBound(1))

                        End While
                    End If ' if EVENT DECODE


                    If CurrentProcessMode = ProcessMode.ALL Then
                        Clusters_Citiroc.Clear()
                        Try
                            While (DataDecodedCitiroc.Count > 0)

                                Dim strline = ""
                                Dim first = DataDecodedCitiroc.Dequeue()
                                Dim newCluster = New Cluster_CITIROC

                                newCluster.timecode = first.EventTimecode
                                newCluster.Runtimecode = first.RunEventTimecode
                                newCluster.timecode_ns = newCluster.timecode * BI.FPGATimecode_ns
                                newCluster.Runtimecode_ns = newCluster.Runtimecode * BI.FPGATimecode_ns
                                newCluster.id = EventCounter
                                CurrentTimecode = newCluster.Runtimecode_ns

                                EventCounter += 1

                                newCluster.Events.Add(first)
                                While DataDecodedCitiroc.Count > 0
                                    If Math.Abs(CType((DataDecodedCitiroc.Peek).RunEventTimecode, Int64) - CType(newCluster.Runtimecode, Int64)) < ClusterMaxTime Then
                                        newCluster.Events.Add(DataDecodedCitiroc.Dequeue)
                                    Else
                                        Exit While
                                    End If
                                    '

                                End While

                                'aggiusta i timecode relativi degli eventi
                                Dim timecode_min As UInt64 = UInt64.MaxValue
                                Dim Runtimecode_min As UInt64 = UInt64.MaxValue
                                For Each e In newCluster.Events
                                    timecode_min = IIf(e.EventTimecode < timecode_min, e.EventTimecode, timecode_min)
                                    Runtimecode_min = IIf(e.RunEventTimecode < Runtimecode_min, e.RunEventTimecode, Runtimecode_min)
                                Next

                                If EnableSaveFile = True Then   'FILE SAVE
                                    If SaveFileType = FileType.CSV Then
                                        strline = EventCounter & ";" & newCluster.Runtimecode_ns & ";" & newCluster.timecode_ns
                                        strline &= ";" & newCluster.Events.Count
                                    End If
                                End If

                                newCluster.timecode = timecode_min
                                newCluster.Runtimecode = Runtimecode_min
                                newCluster.timecode_ns = newCluster.timecode * BI.FPGATimecode_ns
                                newCluster.Runtimecode_ns = newCluster.Runtimecode * BI.FPGATimecode_ns
                                For Each e In newCluster.Events

                                    For j = 0 To BI.channelsPerAsic - 1
                                        Dim vtemp As Double = (CType(e.chargeLG(j), Double) + CorrPoints(e.AsicID, j).Offset) * CorrPoints(e.AsicID, j).Gain
                                        vtemp = IIf(vtemp < 0, 0, vtemp)
                                        vtemp = IIf(vtemp > 16384, 16384, vtemp)
                                        e.chargeLG(j) = vtemp

                                        e.chargeLG(j) = vtemp '(CType(e.charge(j), Double) + CorrPoints(e.AsicID, j).Offset) * CorrPoints(e.AsicID, j).Gain
                                        e.chargeLG(j) = IIf(e.chargeLG(j) > SoftwareThreshold, e.chargeLG(j), 0)
                                    Next


                                    e.EventTimecode = timecode_min
                                    e.EventTimecode_ns = e.EventTimecode * BI.FPGATimecode_ns
                                    e.RunEventTimecode = Runtimecode_min
                                    e.RunEventTimecode_ns = e.RunEventTimecode * BI.FPGATimecode_ns
                                    If EnableSaveFile = True Then   'FILE SAVE
                                        If SaveFileType = FileType.CSV Then
                                            Dim hitNumber(e.hit.Count - 1) As UInt16
                                            For i = 0 To e.hit.Count - 1
                                                hitNumber(i) = IIf(e.hit(i), 1, 0)
                                            Next
                                            strline &= ";" & e.AsicID & ";" & e.EventCounter & ";" & e.RunEventTimecode & ";" & e.RunEventTimecode_ns & ";" & e.EventTimecode & ";" & e.EventTimecode_ns & ";" & String.Join(";", hitNumber) & ";" & String.Join(";", e.chargeLG) & ";" & String.Join(";", e.chargeHG)
                                        End If
                                    End If
                                Next

                                Clusters_Citiroc.Add(newCluster)
                                DecodedClusters += 1

                                If EnableSaveFile = True Then       'FILE SAVE
                                    If SaveFileType = FileType.CSV Then
                                        tx.WriteLine(strline)
                                        sByteCounter += strline.Length
                                    End If
                                End If


                            End While

                        Catch ex As Exception
                            'MsgBox(ex.Message)
                            AppendToLog(LogMode.mPROCESS, "Process ERROR: " & ex.Message, BI.SerialNumber)
                        End Try

                        Dim view_event = Clusters_Citiroc.Count / 2
                        If (view_event > 0) Then
                            For Each e In Clusters_Citiroc(view_event).Events
                                Dim SpXIndx = BoardArrayOffset + (e.AsicID * BI.channelsPerAsic)

                                For j = 0 To BI.channelsPerAsic - 1
                                    Dim c = SpXIndx + j
                                    If c < RawESpectrum.GetUpperBound(0) Then
                                        MatrixInst(c) = e.chargeLG(j)
                                    End If
                                Next
                            Next
                        End If
                        'For i = ClustersBefore To ClustersBefore + DecodedClusters - 1
                        For i = 0 To Clusters_Citiroc.Count - 1
                                Try

                                    'For t = 0 To MatrixInst.GetUpperBound(0)
                                    'MatrixInst(t) = 0
                                    'Next
                                    Dim EnergySum As Double = 0
                                    Dim EnergySum_HG As Double = 0
                                    For Each e In Clusters_Citiroc(i).Events

                                        Dim SpXIndx = BoardArrayOffset + (e.AsicID * BI.channelsPerAsic)

                                        For j = 0 To BI.channelsPerAsic - 1
                                            Dim c = SpXIndx + j
                                            If c < RawESpectrum.GetUpperBound(0) Then
                                                If e.chargeLG(j) > 0 Then
                                                    RawESpectrum(c, e.chargeLG(j)) += 1
                                                    EnergySum += e.chargeLG(j)
                                                End If

                                                If e.chargeHG(j) > 0 Then
                                                    RawE_HG_Spectrum(c, e.chargeHG(j)) += 1
                                                    EnergySum_HG += e.chargeHG(j)
                                                End If

                                                MatrixCumulative(c) += e.chargeLG(j) ' quest due righe erano dentro hit
                                                MatrixCumulativePerAsic(e.AsicID, j) = MatrixCumulative(c)
                                                MatrixCumulativePerAsicCount(e.AsicID, j) += 1
                                                'MatrixInst(c) = e.chargeLG(j)
                                                If e.hit(j) = True Then


                                                    RawHitCounter(0, c) += 1
                                                End If
                                            End If
                                        Next



                                    Next

                                    Dim spesum_bin
                                    spesum_bin = Math.Round(Math.Min(EnergySum * SumSpectrumGain, 1023))
                                    If spesum_bin > 0 Then
                                        RawESpectrum(SperctrumSumIndex, spesum_bin) += 1
                                    End If

                                    Dim spesum_bin_HG
                                    spesum_bin_HG = Math.Round(Math.Min(EnergySum_HG * SumSpectrumGain, 1023))
                                    If spesum_bin_HG > 0 Then
                                        RawE_HG_Spectrum(SperctrumSumIndex, spesum_bin_HG) += 1
                                    End If
                                Catch ex As Exception
                                    MsgBox(ex.Message)
                                    AppendToLog(LogMode.mPROCESS, "Process ERROR: " & ex.Message, BI.SerialNumber)
                                End Try
                            Next
                            TotalClusters += DecodedClusters
                            TotalEvents += DecodedEvents
                            pRT4.PostData(RawESpectrum, RawESpectrum.GetUpperBound(1))
                            pRT4_HG.PostData(RawE_HG_Spectrum, RawE_HG_Spectrum.GetUpperBound(1))
                            pRT5.PostData(RawTSpectrum, RawTSpectrum.GetUpperBound(1))
                            pRT6.PostData(RawHitCounter, RawHitCounter.GetUpperBound(1))
                            If Clusters_Citiroc.Count > 0 Then
                                For Each e In Clusters_Citiroc.Last.Events
                                    For j = 0 To e.chargeLG.GetUpperBound(0) - 1
                                        AnalogMonitor(e.AsicID, j) = e.chargeLG(j)
                                    Next
                                Next
                            End If
                            pRT7.PostData(AnalogMonitor, AnalogMonitor.GetUpperBound(1))
                        End If 'if CLUSTER_DECODE

                    End If
            End If

            tProcTime = Now - ProcTime
            sProcTime = tProcTime.TotalMilliseconds
            sEventCounter = TotalEvents
            sClusterCounter = TotalClusters

            If RUN_TARGET_MODE = TargetMode.Time_ns Then
                If CurrentProcessMode = ProcessMode.OFF Then
                    If RUN_TARGET_VALUE <= DURATION.TotalMilliseconds * 1000000 Then
                        running = False
                        RunCompleted = True
                        AppendToLog(LogMode.mACQUISITION, "Acquisition completed. Time: " & DURATION.TotalMilliseconds & "ms", BI.SerialNumber)
                    End If
                    sTargetPercent = DURATION.TotalMilliseconds * 1000000 / RUN_TARGET_VALUE * 100
                Else
                    sTargetPercent = CurrentTimecode / RUN_TARGET_VALUE * 100
                    If RUN_TARGET_VALUE <= CurrentTimecode Then
                        running = False
                        RunCompleted = True
                        AppendToLog(LogMode.mACQUISITION, "Acquisition completed. Time: " & CurrentTimecode & "ms", BI.SerialNumber)
                    End If
                End If
            End If
            If RUN_TARGET_MODE = TargetMode.Events Then
                If CurrentProcessMode = ProcessMode.OFF Then

                Else
                    sTargetPercent = TotalEvents / RUN_TARGET_VALUE * 100
                    If RUN_TARGET_VALUE <= TotalEvents Then
                        running = False
                        RunCompleted = True
                        AppendToLog(LogMode.mACQUISITION, "Acquisition completed. Events: " & TotalEvents & "ms", BI.SerialNumber)
                    End If
                End If
            End If
            If RUN_TARGET_MODE = TargetMode.Clusters Then
                If CurrentProcessMode = ProcessMode.OFF Then

                Else
                    sTargetPercent = TotalClusters / RUN_TARGET_VALUE * 100
                    If RUN_TARGET_VALUE <= TotalClusters Then
                        running = False
                        RunCompleted = True
                        AppendToLog(LogMode.mACQUISITION, "Acquisition completed. Clusters: " & TotalEvents & "ms", BI.SerialNumber)
                    End If
                End If
            End If
        End While
        If Not IsNothing(tx) Then
            tx.Close()
            AppendToLog(LogMode.mACQUISITION, "File is closed", BI.SerialNumber)
        End If
        If Not IsNothing(bwriter) Then
            bwriter.Close()
            AppendToLog(LogMode.mACQUISITION, "File is closed", BI.SerialNumber)
        End If


        board.RUNControl(False)
    End Sub

    Public Sub StartRun()
        Timer3.Enabled = False

        running = True
        ACQLOCK = True
        System.Threading.Thread.Sleep(100)
        ' rdc.cTargetMode = 0
        ' rdc.StartAcquisition(nboard)
        'pRT4.startspectrum()
        'pRT.Timer1.Enabled = True
        'pRT2.Timer1.Enabled = True

        wSingle.Enabled = False
        wRun.Enabled = False
        wRunStop.Enabled = False
        sRun.Enabled = False
        Wspectrum.Enabled = False
        WspectrumStop.Enabled = True

        StartFreeRunToolStripMenuItem.Enabled = False
        StartAcquisitionRunToolStripMenuItem.Enabled = False
        StopRunToolStripMenuItem.Enabled = True
        SingleShotToolStripMenuItem.Enabled = False
        PeriodicToolStripMenuItem.Enabled = False
        StopToolStripMenuItem.Enabled = False
        ConfigureAllASICToolStripMenuItem.Enabled = False

        EnableDisableUpdate(True)

        map.EnableDisableUpdate(True)
        pRT4.startspectrum()
        pRT4_HG.startspectrum()
        pRT5.startspectrum()
        pRT6.startspectrum()
        pRT7.startspectrum()
        pRT.StartLive()
        pRT2.StartLive()


        Dim BoardLaunch = 0

        Dim t As New Task(Sub()
                              Dim CHO = 0
                              For i = 0 To BoardLaunch - 1
                                  Dim BI As t_BoardInfo = DTList(i).GetBoardInfo
                                  CHO += BI.totalChannels
                              Next
                              AcquisitionThread(DTList(BoardLaunch), "c:\temp\temp.txt", TransferSize, CHO)
                          End Sub)

        t.Start()
    End Sub
    Public Sub RunFreerun()
        sStatus = "FREE RUNNING"
        RUN_TARGET_MODE = TargetMode.FreeRunning
        EnableSaveFile = False
        RunCompleted = False
        StartRun()
    End Sub
    Private Sub Wspectrum_Click(sender As Object, e As EventArgs) Handles Wspectrum.Click
        RunFreerun()
    End Sub

    Private Sub ToolStripButton1_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        ResetLive()


    End Sub

    Public Sub StopRunMonitor()
        Timer3.Enabled = False
        Timer2.Enabled = False
        wSingle.Enabled = True
        wRun.Enabled = True
        sRun.Enabled = True

        Wspectrum.Enabled = True
        wRunStop.Enabled = False
        WspectrumStop.Enabled = False
        ' pprop.Button1.Enabled = True
        System.Threading.Thread.Sleep(100)
        running = False

        StartFreeRunToolStripMenuItem.Enabled = True
        StartAcquisitionRunToolStripMenuItem.Enabled = True
        StopRunToolStripMenuItem.Enabled = False
        SingleShotToolStripMenuItem.Enabled = True
        PeriodicToolStripMenuItem.Enabled = True
        StopToolStripMenuItem.Enabled = False
        ConfigureAllASICToolStripMenuItem.Enabled = True
        EnableDisableUpdate(False)
        map.EnableDisableUpdate(False)

        AppendToLog(LogMode.mINFO, "Stop monitor periodic acquisition")

        ACQLOCK = False
        Timer3.Enabled = True
    End Sub
    Private Sub wRunStop_Click(sender As Object, e As EventArgs) Handles wRunStop.Click

        'hvon.Enabled = False 'Not pprop.bBias.Checked
        'hvoff.Enabled = Not hvon.Enabled
        StopRunMonitor()
    End Sub

    Private Sub ToolStripButton2_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        CPSMonitor.Show()

        If Not IsNothing(CPSMonitor) Then
            CPSMonitor.cps.Rows.Clear()
            For Each dt In DTList
                For i = 0 To dt.GetBoardInfo.totalAsics - 1
                    For j = 0 To dt.GetBoardInfo.channelsPerAsic
                        CPSMonitor.cps.Rows.Add(j & "(" & Chr(i + Asc("A")) & ") " & dt.GetBoardInfo.SerialNumber.Substring(4), 0)
                    Next
                Next

            Next

        End If

    End Sub
    Dim lastCheck As DateTime = Now

    Dim lastTempCheck As DateTime = Now
    Dim running As Boolean
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If RunCompleted = True Then
            sStatus = "COMPLETED"
            StopRun()
            RunCompleted = False
        End If

        If IsNothing(plog) Then

        Else
            If msgcoda.Count > 0 Then
                plog.TextBox1.AppendText(msgcoda.Dequeue & vbCrLf)
            End If
        End If
    End Sub

    Private Sub MainForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        pRT4.Pesgo1.Dispose()

        pRT4.Timer1.Enabled = False
        pRT4.Dispose()
        pRT4_HG.Pesgo1.Dispose()

        pRT4_HG.Timer1.Enabled = False
        pRT4_HG.Dispose()
        Try
            For i As Integer = System.Windows.Forms.Application.OpenForms.Count - 1 To 1 Step -1
                Dim form As Form = System.Windows.Forms.Application.OpenForms(i)
                form.Dispose()
            Next i
        Catch ex As Exception
        Finally
            Me.Dispose()
            End
        End Try

    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs)

    End Sub


    Public filemode As Boolean = False
    Public OSC_Filename As String = ""
    Private Sub ToolStripButton3_Click_1(sender As Object, e As EventArgs)


    End Sub

    Public Function SaveConfiguration()
        Dim sDialog As New SaveFileDialog()
        sDialog.DefaultExt = ".nias"
        sDialog.Filter = "Nuclear Instruments Application Settings (*.nias)|*.nias"

        Try




            If sDialog.ShowDialog() = DialogResult.OK Then

                Using sWriter As New StreamWriter(sDialog.FileName)
                    Dim sc As WeeRocAsicCommonSettings

                    If GBL_ASIC_MODEL = t_AsicModels.PETIROC Then
                        sc = sets_petiroc.GetSettingsClass()
                    Else
                        If GBL_ASIC_MODEL = t_AsicModels.CITIROC Then
                            sc = sets_citiroc.GetSettingsClass
                        End If
                    End If

                    ReDim sc.sMap(GC.Count - 1)
                    For i = 0 To GC.Count - 1
                        sc.sMap(i) = New WeeRocAsicCommonSettings.chMap
                        sc.sMap(i).Asic = GC(i).Asic
                        sc.sMap(i).Board = GC(i).Board
                        sc.sMap(i).Channel = GC(i).Channel
                        sc.sMap(i).ValidLocation = GC(i).ValidLocation
                        sc.sMap(i).Veto = GC(i).Veto
                        sc.sMap(i).X = GC(i).X
                        sc.sMap(i).Y = GC(i).Y
                    Next
                    Dim x As New Xml.Serialization.XmlSerializer(sc.GetType)
                    x.Serialize(sWriter, sc)


                End Using

                My.Settings.Save()
                AppendToLog(MainForm.LogMode.mINFO, "Setting saved successfully")
                MessageBox.Show("Settings has been saved to the specified file", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Function
    Private Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        SaveConfiguration()



    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs)
    End Sub


    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        If fit_enabled = False Then
            fit_enabled = True
            fit = New fit_win
            Dim content5 As DockContent = GetDockContentForm("Fit", DockState.DockBottom, Color.White)
            content5.Show(dockPanel)
            content5.CloseButtonVisible = False
            fit.Dock = DockStyle.Fill
            content5.Controls.Add(fit)
            dockPanel.DockBottomPortion = 0.15
            list_dockPanel.Add(content5)

        Else
            fit_enabled = False
            For i = 0 To list_dockPanel.Count - 1
                If list_dockPanel(i).Name = "Fit" Then
                    list_dockPanel(i).Dispose()
                    list_dockPanel.RemoveAt(i)
                    Exit For
                End If
            Next


        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        UpdateOscilloscope()
    End Sub
    Public Sub SetBias(onoff As Boolean)
        If onoff = True Then

            If ACQLOCK Then
                AppendToLog(LogMode.mERROR, "Unable to control BIAS while acquisition is runing")
            Else

                Select Case GBL_ASIC_MODEL
                    Case t_AsicModels.PETIROC
                        sets_petiroc.HVon.Checked = True
                        For Each dt In DTList
                            dt.SetHV(sets_petiroc.HVon.Checked, sets_petiroc.Voltage.Value, sets_petiroc.MaxV.Value)
                        Next
                    Case t_AsicModels.CITIROC
                        sets_citiroc.HVon.Checked = True
                        For Each dt In DTList
                            dt.SetHV(sets_citiroc.HVon.Checked, sets_citiroc.Voltage.Value, sets_citiroc.MaxV.Value)
                        Next
                    Case t_AsicModels.MAROC
                    Case t_AsicModels.PHOTOROC
                    Case t_AsicModels.GEMROC
                End Select



                hvon.Enabled = False
                hvoff.Enabled = True

                AppendToLog(LogMode.mINFO, "Power ON Sipm bias")
            End If

        Else
            If ACQLOCK Then
                AppendToLog(LogMode.mERROR, "Unable to control BIAS while acquisition is runing")
            Else
                Select Case GBL_ASIC_MODEL
                    Case t_AsicModels.PETIROC
                        sets_petiroc.HVon.Checked = False
                        For Each dt In DTList
                            dt.SetHV(sets_petiroc.HVon.Checked, sets_petiroc.Voltage.Value, sets_petiroc.MaxV.Value)
                        Next
                    Case t_AsicModels.CITIROC
                        sets_citiroc.HVon.Checked = False
                        For Each dt In DTList
                            dt.SetHV(sets_citiroc.HVon.Checked, sets_citiroc.Voltage.Value, sets_citiroc.MaxV.Value)
                        Next
                    Case t_AsicModels.MAROC
                    Case t_AsicModels.PHOTOROC
                    Case t_AsicModels.GEMROC
                End Select
                hvon.Enabled = True
                hvoff.Enabled = False
                AppendToLog(LogMode.mINFO, "Power OFF Sipm bias")
            End If
        End If
    End Sub

    Private Sub hvon_Click(sender As Object, e As EventArgs) Handles hvon.Click
        SetBias(True)
    End Sub

    Private Sub hvoff_Click(sender As Object, e As EventArgs) Handles hvoff.Click
        SetBias(False)
    End Sub

    Private Sub StopRunToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopRunToolStripMenuItem.Click
        StopRun()
    End Sub

    Private Sub PeriodicToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PeriodicToolStripMenuItem.Click
        RunOscilloscope()
    End Sub

    Private Sub StartFreeRunToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartFreeRunToolStripMenuItem.Click
        RunFreerun()
    End Sub

    Private Sub SingleShotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SingleShotToolStripMenuItem.Click
        UpdateOscilloscope()
    End Sub

    Private Sub StartAcquisitionRunToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartAcquisitionRunToolStripMenuItem.Click
        RunAcquisitionRun()
    End Sub

    Private Sub StopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem.Click
        StopRunMonitor()
    End Sub

    Private Sub ResetRealtimeDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetRealtimeDataToolStripMenuItem.Click
        ResetLive()
    End Sub

    Private Sub ConfigureAllASICToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureAllASICToolStripMenuItem.Click
        Select Case GBL_ASIC_MODEL
            Case t_AsicModels.PETIROC
                sets_petiroc.UpdateSettings()
            Case t_AsicModels.CITIROC
                sets_citiroc.UpdateSettings()
            Case t_AsicModels.MAROC
            Case t_AsicModels.PHOTOROC
            Case t_AsicModels.GEMROC
        End Select


    End Sub

    Private Function GetRow(matrix As ULong(,), row_number As Integer) As ULong()
        'get the number of columns of your matrix
        Dim number_of_columns As Integer = matrix.GetLength(1)

        'define empty array, at the end of the 'for' cycle it will contain requested row's values  
        Dim values As ULong() = Nothing

        For i As Integer = 0 To number_of_columns - 1
            'Resize array
            ReDim Preserve values(i)
            'Populate array element
            values(i) = matrix(row_number, i)
        Next

        Return values

    End Function

    Private Sub ExportEnergySpectrumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportEnergySpectrumToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Column Separated Values (*.csv)|*.csv"
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            Try

                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(SaveFileDialog1.FileName, False)

                For i = 0 To RawESpectrum.GetUpperBound(0) - 1
                    file.WriteLine(String.Join(";", GetRow(RawESpectrum, i)))
                Next
                file.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Error exporting data")
            End Try

        End If
        'pRT4.PostData(RawESpectrum, RawESpectrum.GetUpperBound(1))
        'pRT5.PostData(RawTSpectrum, RawTSpectrum.GetUpperBound(1))
        'pRT6.PostData(RawHitCounter, RawHitCounter.GetUpperBound(1))
    End Sub

    Private Sub TimeSpectrumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TimeSpectrumToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Column Separated Values (*.csv)|*.csv"
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            Try

                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(SaveFileDialog1.FileName, False)

                For i = 0 To RawTSpectrum.GetUpperBound(0) - 1
                    file.WriteLine(String.Join(";", GetRow(RawTSpectrum, i)))
                Next
                file.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Error exporting data")
            End Try

        End If

    End Sub

    Private Sub HitmapCumulativeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HitmapCumulativeToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Column Separated Values (*.csv)|*.csv"
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            Try

                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(SaveFileDialog1.FileName, False)


                file.WriteLine(String.Join(";", MatrixCumulative))

                file.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Error exporting data")
            End Try

        End If
    End Sub

    Private Sub HitmapToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HitmapToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Column Separated Values (*.csv)|*.csv"
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            Try

                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(SaveFileDialog1.FileName, False)


                file.WriteLine(String.Join(";", MatrixInst))

                file.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Error exporting data")
            End Try
        End If
    End Sub

    Private Sub ChannelsMappingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChannelsMappingToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Column Separated Values (*.csv)|*.csv"
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            Try

                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(SaveFileDialog1.FileName, False)
                file.WriteLine("ID;BOARD;ASIC;CHANNEL;X;Y")
                For i = 0 To GC.Count - 1
                    If GC(i).ValidLocation Then
                        file.WriteLine(i & ";" & GC(i).Board & ";" & GC(i).Asic & ";" & GC(i).Channel & ";" & GC(i).X & ";" & GC(i).Y)
                    End If
                Next

                file.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Error exporting data")
            End Try
        End If
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Dim enable As Boolean = False
        Dim voltage As Double = 0
        Dim current As Double = 0
        Dim tA As Double = -1000
        Dim tB As Double = -1000
        Timer3.Enabled = False
        Dim genable = -1
        For Each dt In DTList
            If dt.GetHV(enable, voltage, current) Then
                plog.UpdateHvStatus(enable, voltage, current)
                If enable Then
                    genable = 1
                Else
                    genable = Math.Max(genable, 0)
                End If
            End If

            dt.GetSensTemperature(0, tA)
            If dt.GetBoardInfo.totalAsics > 2 Then
                dt.GetSensTemperature(1, tB)
            End If

            If TempSensorSource = 0 Then
                dt.GetSensTemperature(0, tA)
                If dt.GetBoardInfo.totalAsics > 2 Then
                    dt.GetSensTemperature(1, tB)
                End If
            Else
                dt.GetSensTemperature(2, tA)
            End If

            If EnableTempComp Then
                Dim tAv = 0
                If tB > -150 Then
                    tAv = (tA + tB) / 2
                Else
                    tAv = tA
                End If
                dt.SetHVTempFB(CurrentHVON, CurrentHVSet, CurrentHVMax, TempCompCoef, tAv)
            End If


            plog.UpdateSiPMTemp(tA, tB)


        Next
        If genable >= 0 Then
                If genable = 1 Then
                    hvon.Enabled = False
                    hvoff.Enabled = True

                Else
                    hvon.Enabled = True
                    hvoff.Enabled = False

                End If

            End If

            Dim cps(1024) As UInt32
        If Not IsNothing(CPSMonitor) Then
            ' CPSMonitor.cps.Rows.Clear()
            For Each dt In DTList
                dt.GetCountRate(cps)
                For i = 0 To CPSMonitor.cps.Rows.Count - 1
                    CPSMonitor.cps.Rows(i).Cells(1).Value = cps(i)
                Next
                pRT9.UpdateCPS(cps)
                'For i = 0 To dt.GetBoardInfo.totalAsics - 1
                '    For j = 0 To dt.GetBoardInfo.channelsPerAsic
                '        If j < CPSMonitor.cps.Rows.Count - 1 Then
                '            CPSMonitor.cps.Rows(j).Cells(1).Value = 15'cps(j + dt.GetBoardInfo.channelsPerAsic * i)
                '        End If

                '    Next
                'Next

            Next

        End If
        Timer3.Enabled = True
    End Sub

    Private Sub DefaultConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DefaultConfigurationToolStripMenuItem.Click
        GenerateEmptyConfigration()
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub LoadConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadConfigurationToolStripMenuItem.Click
        OpenConfiguration()
    End Sub

    Private Sub SaveConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveConfigurationToolStripMenuItem.Click
        SaveConfiguration()

    End Sub
    Public Sub GenerateEmptyConfigration()
        Dim sc As WeeRocAsicCommonSettings
        If GBL_ASIC_MODEL = t_AsicModels.PETIROC Then
            sc = sets_petiroc.GetDefaultSettingsClass()
            sets_petiroc.SetSettingsFromClass(sc)
        Else
            If GBL_ASIC_MODEL = t_AsicModels.CITIROC Then
                sc = sets_citiroc.GetDefaultSettingsClass
                sets_citiroc.SetSettingsFromClass(sc)
            End If
        End If
    End Sub
    Private Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click
        GenerateEmptyConfigration()
    End Sub

    Public Sub SetRebin(ByVal rebin)
        If Not IsNothing(pRT4) Then
            pRT4.SetRebin(rebin)
        End If

        If Not IsNothing(pRT4_HG) Then
            pRT4_HG.SetRebin(rebin)
        End If

    End Sub

    Private Sub EnegySpectrumHGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnegySpectrumHGToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Column Separated Values (*.csv)|*.csv"
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            Try

                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(SaveFileDialog1.FileName, False)

                For i = 0 To RawE_HG_Spectrum.GetUpperBound(0) - 1
                    file.WriteLine(String.Join(";", GetRow(RawE_HG_Spectrum, i)))
                Next
                file.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Error exporting data")
            End Try

        End If
    End Sub

    Private Sub ScanResultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScanResultToolStripMenuItem.Click
        If LastScanResult.Valid = False Then
            MsgBox("No valid scan to be exported stored in memory.", vbCritical + vbOKOnly)
            Exit Sub
        End If
        SaveFileDialog1.Filter = "Column Separated Values (*.csv)|*.csv"
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            Try

                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(SaveFileDialog1.FileName, False)
                file.WriteLine("TIME;PARAMETER_VALUE;CHANNEL[n];VALUE[n]")
                For Each sp In LastScanResult.LitOfScanPoint
                    file.WriteLine(sp.Time.ToString.Replace(",", ".") & ";" & sp.ParamValue.ToString.Replace(",", ".") & ";" & String.Join(";", sp.ScanValue))
                Next

                file.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Error exporting data")
            End Try

        End If

    End Sub
End Class
