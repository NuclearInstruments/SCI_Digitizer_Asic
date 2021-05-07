Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters

Public Class RunStart

    Public TargetValue As Double = 0
    Public FilePathName As String
    Public mode As Integer
    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles pMRun.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles pRun.TextChanged

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles pBoard.TextChanged

    End Sub

    Public Sub LockMode(mode As Integer)
        If runMode.Items.Count > mode Then
            runMode.SelectedIndex = mode
        End If
        runMode.Enabled = False
    End Sub

    Private Sub RunStart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cTargetMode.Items.Clear()
        cTargetMode.Items.Add("Free running")
        cTargetMode.Items.Add("Events")
        cTargetMode.Items.Add("Clusters")
        cTargetMode.Items.Add("Time")



        cTargetMode.SelectedIndex = 0
        pRun.Text = My.Settings.parRun + 1
        pMRun.Text = My.Settings.parAcq + 1
        If MainForm.sets_petiroc.HVon.Checked Then
            pBias.Text = "0 V"
        Else
            pBias.Text = MainForm.sets_petiroc.Voltage.Value & " V"
        End If

        pBoard.Text = 1
        pAcq.Text = 0
        pDT.Text = Now
        Dim tmp As Integer

        'If MainForm.pprop.bBias.Checked Then
        '    pBias.Text = MainForm.pprop.vBias.Value
        'Else
        '    pBias.Text = "0"
        'End If

        TextBox1.Text = My.Settings.folderpos
        Me.StartPosition = FormStartPosition.CenterScreen

        runMode.Items.Add("Spectroscopy")
        runMode.Items.Add("Photon Counting")
        runMode.SelectedIndex = 0
    End Sub

    Public Structure RunGlobal
        Public RunId As String
        Public MachineRun As String
        Public DateTime As String
        Public PacketSize As String
        Public T0Mode As String
        Public T0freq As Integer
        Public psbin As Integer
        Public HVon As Boolean
        Public HVVoltage As Double
    End Structure

    Public Structure BoardConfig
        Public BoardSN As String
        Public ConfigPetiroc As List(Of DT5550W_P_lib.DT5550W_PETIROC.PetirocConfig)
        Public ConfigCitiroc As List(Of DT5550W_P_lib.DT5550W_CITIROC.CitirocConfig)
    End Structure

    Structure RunInfoGlobal
        Public SystemInfo As String
        Public RunMode As String
        Public AsicInfo As String
        Public AsicCount As String
        Public ApplicationVersion As String
        Public BuildData As String
        Public GlobalRun As RunGlobal
        Public BoardConfiguration As List(Of BoardConfig)
        Public TargetType As String
        Public TargetValue As Double
        Public Note As String
    End Structure


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Settings.parRun = pRun.Text
        My.Settings.parAcq = pMRun.Text
        MainForm.SaveFolderDefault = TextBox1.Text
        My.Settings.folderpos = MainForm.SaveFolderDefault

        My.Settings.Save()

        mode = runMode.SelectedIndex

        ' MainForm.rdc.ScaleFactor = 4 'MainForm.pprop.iTime.Value / MainForm.Ts
        Dim header As String
        Dim BI = MainForm.DTList(0).GetBoardInfo
        Dim build_date As Date = Date.FromOADate(Val(My.Application.Info.Version.Build) + 36526)
        Dim RIG As New RunInfoGlobal
        RIG.SystemInfo = "Nuclear Instruments - CAEN DT5550W"
        RIG.AsicInfo = BI.AsicType
        RIG.AsicCount = BI.totalAsics
        RIG.ApplicationVersion = Application.ProductVersion
        RIG.BuildData = build_date.ToString("dd-MM-yy")
        RIG.GlobalRun = New RunGlobal
        RIG.GlobalRun.RunId = pRun.Text
        RIG.GlobalRun.MachineRun = pMRun.Text
        RIG.GlobalRun.DateTime = pDT.Text

        Select Case MainForm.GBL_ASIC_MODEL
            Case DT5550W_P_lib.t_AsicModels.PETIROC
                RIG.RunMode = runMode.Text
                RIG.GlobalRun.PacketSize = MainForm.sets_petiroc.TransferSize.Text
                RIG.GlobalRun.T0Mode = MainForm.sets_petiroc.T0Mode.Text
                RIG.GlobalRun.T0freq = MainForm.sets_petiroc.T0Freq.Value
                RIG.GlobalRun.psbin = MainForm.sets_petiroc.TimePsBin.Value
                RIG.GlobalRun.HVon = MainForm.sets_petiroc.HVon.Checked
                RIG.GlobalRun.HVVoltage = MainForm.sets_petiroc.Voltage.Value
                RIG.BoardConfiguration = New List(Of BoardConfig)

                For Each dt In MainForm.DTList
                    Dim aCFG As New BoardConfig
                    aCFG.BoardSN = dt.SerialNumber
                    aCFG.ConfigPetiroc = New List(Of DT5550W_P_lib.DT5550W_PETIROC.PetirocConfig)
                    If dt.PetirocClass.pCFG.Count > 0 Then
                        For Each pc In dt.PetirocClass.pCFG
                            aCFG.ConfigPetiroc.Add(pc)
                        Next
                    End If
                    RIG.BoardConfiguration.Add(aCFG)
                Next
            Case DT5550W_P_lib.t_AsicModels.CITIROC
                RIG.RunMode = runMode.Text
                RIG.GlobalRun.PacketSize = MainForm.sets_citiroc.TransferSize.Text
                RIG.GlobalRun.T0Mode = MainForm.sets_citiroc.T0Mode.Text
                RIG.GlobalRun.T0freq = MainForm.sets_citiroc.T0Freq.Value
                RIG.GlobalRun.HVon = MainForm.sets_citiroc.HVon.Checked
                RIG.GlobalRun.HVVoltage = MainForm.sets_citiroc.Voltage.Value
                RIG.BoardConfiguration = New List(Of BoardConfig)

                For Each dt In MainForm.DTList
                    Dim aCFG As New BoardConfig
                    aCFG.BoardSN = dt.SerialNumber
                    aCFG.ConfigCitiroc = New List(Of DT5550W_P_lib.DT5550W_CITIROC.CitirocConfig)
                    If dt.CitirocClass.pCFG.Count > 0 Then
                        For Each pc In dt.CitirocClass.pCFG
                            aCFG.ConfigCitiroc.Add(pc)
                        Next
                    End If
                    RIG.BoardConfiguration.Add(aCFG)
                Next
        End Select

        RIG.TargetType = cTargetMode.Text
        Dim targmult As Double = 1
        If cTargetMode.Text = "Time" Then
            Select Case (UnitMult.SelectedIndex)
                Case 0
                    targmult = 1
                Case 1
                    targmult = 1000
                Case 2
                    targmult = 1000000
                Case 3
                    targmult = 1000000000
                Case 4
                    targmult = 1000000000L * 60
                Case 5
                    targmult = 1000000000L * 60 * 60
            End Select

        End If

        If cTargetMode.Text = "Events" Or cTargetMode.Text = "Clusters" Then
            Select Case (UnitMult.SelectedIndex)
                Case 0
                    targmult = 1
                Case 1
                    targmult = 1000
                Case 2
                    targmult = 1000000
            End Select

        End If

        targetvalue = cTargetValue.Text * targmult
        RIG.TargetValue = targetvalue

        Dim serializer As JsonSerializer = New JsonSerializer()
        serializer.Converters.Add(New JavaScriptDateTimeConverter())
        serializer.NullValueHandling = NullValueHandling.Ignore

        Dim sw As StreamWriter = New StreamWriter(TextBox1.Text & "\" & pRun.Text & ".json")
        Dim writer As JsonWriter = New JsonTextWriter(sw)
        serializer.Formatting = Formatting.Indented
        serializer.Serialize(writer, RIG)
        writer.Close()

        FilePathName = TextBox1.Text & "\" & pRun.Text & ".data"

        'header = "<XML_NUCLEAR_INSTRUMENTS_MADA>" & vbCrLf
        'header = header & vbTab & "<SYSTEM_INFO>" & vbCrLf
        'header = header & vbTab & vbTab & "<APPLICATION_NAME>Nuclear Instruments DT5550W Readout System</APPLICATION_NAME>" & vbCrLf
        'header = header & vbTab & vbTab & "<COMPANY_WEBSITE>http://www.nuclearinstruments.eu</COMPANY_WEBSITE>" & vbCrLf
        'header = header & vbTab & vbTab & "<APPLICATION_VERSION>" & Application.ProductVersion & "</APPLICATION_VERSION>" & vbCrLf
        'header = header & vbTab & vbTab & "<BUILD_DATE>" & build_date.ToString("dd-MM-yy") & "</BUILD_DATE>"
        'header = header & vbTab & "</SYSTEM_INFO>" & vbCrLf
        'header = header & vbTab & "<ACQUISITION_INFO>" & vbCrLf
        'header = header & vbTab & vbTab & "<RUN_INFO>" & vbCrLf
        'header = header & vbTab & vbTab & vbTab & "<RUN_ID>" & My.Settings.parRun & "</RUN_ID>" & vbCrLf
        'header = header & vbTab & vbTab & vbTab & "<MACHINE_RUN_ID>" & My.Settings.parAcq & "</MACHINE_RUN_ID>" & vbCrLf
        'header = header & vbTab & vbTab & vbTab & "<DATETIME>" & pDT.Text & "</DATETIME>" & vbCrLf
        'header = header & vbTab & vbTab & vbTab & "<BIAS>" & pBias.Text & "</BIAS>" & vbCrLf
        'header = header & vbTab & vbTab & vbTab & "<DETECTOR_BOARD_SN>" & pBoard.Text & "</DETECTOR_BOARD_SN>" & vbCrLf
        'header = header & vbTab & vbTab & vbTab & "<ACQUISITION_BOARD_SN>" & pAcq.Text & "</ACQUISITION_BOARD_SN>" & vbCrLf
        'header = header & vbTab & vbTab & vbTab & "<ACQUISITION_BOARD_RC>" & TextBox5.Text & "</ACQUISITION_BOARD_RC>" & vbCrLf
        'header = header & vbTab & vbTab & vbTab & "<BOARD_COUNT>" & MainForm.DTList.Count & "</BOARD_COUNT>" & vbCrLf
        'For i = 0 To MainForm.DTList.Count - 1
        '    header = header & vbTab & vbTab & vbTab & "<BOARD" & i "_ID>" & MainForm.DTList(i).SerialNumber & "</BOARD1_ID>" & vbCrLf
        'Next

        'header = header & vbTab & vbTab & vbTab & "<BOARD2_ID>" & MainForm.board2_id & "</BOARD2_ID>" & vbCrLf
        'header = header & vbTab & vbTab & "</RUN_INFO>" & vbCrLf
        'header = header & vbTab & vbTab & "<CONFIG_INFO>" & vbCrLf


        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
            My.Settings.folder = TextBox1.Text
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = DialogResult.Abort
        Me.Close()
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub pAcq_TextChanged(sender As Object, e As EventArgs) Handles pAcq.TextChanged

    End Sub

    Private Sub cTargetMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cTargetMode.SelectedIndexChanged
        UnitMult.Items.Clear()
        If cTargetMode.Text = "Time" Then
            UnitMult.Items.Add("ns")
            UnitMult.Items.Add("us")
            UnitMult.Items.Add("ms")
            UnitMult.Items.Add("s")
            UnitMult.Items.Add("min")
            UnitMult.Items.Add("hours")
            UnitMult.SelectedIndex = 3
        End If


        If cTargetMode.Text = "Events" Or cTargetMode.Text = "Clusters" Then
            UnitMult.Items.Add("x 1")
            UnitMult.Items.Add("x 1k")
            UnitMult.Items.Add("x 1M")
            UnitMult.SelectedIndex = 0
        End If


    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click

    End Sub
End Class