Imports System.IO
Imports System.Threading
Imports System.ComponentModel
Imports Gigasoft.ProEssentials
Imports Gigasoft.ProEssentials.Enums
Imports OxyPlot
Imports MathNet.Numerics
Public Class ScanWindow
    Dim SpectrumLength As UInt32 = 1024
    Dim MaxNumberOfChannel As UInt32 = 32
    Dim logmode As Boolean = False
    Public addressData As UInt32
    Public addressWait As UInt32
    Public addressMask As UInt32
    Public addressMode As UInt32
    Public addressArm As UInt32
    Public addressStatus As UInt32
    Public addressSync As UInt32
    Dim nchannels As UInt32
    Dim MutexSpe As New Mutex
    Dim MutexCumulative As New Mutex
    Public spectra(MaxNumberOfChannel, SpectrumLength) As UInt64
    Public realtimeimage(MaxNumberOfChannel) As Double
    Public integralimage(MaxNumberOfChannel) As Double
    Dim MutexFile As New Mutex
    Dim fileName As String
    Public fileEnable As Boolean = False
    Dim objRawWriter As StreamWriter
    Dim saveQueue As Queue
    Public running As Boolean = False
    Dim SS_totalEvents As ULong = 0
    Public EnabledChannel() As Boolean

    Dim t1 As Thread
    Dim StopT1 As Boolean = False
    Dim TotalCps() As Double
    Dim sumIndex = 0

    Dim ScanXValue As Double = 0
    Dim oScanXValue As Double = 0

    Dim ScanMode As String = "Time"

    Dim isRunning As Boolean = False
    Dim discard_next = 0



    Structure tPointRate
        Dim PointData As DateTime
        Dim PointIntegralTime As Double
        Dim YValue As Double
        Dim XValue As Double
        Dim ScanValue As Double
    End Structure

    Dim PointHistory() As List(Of tPointRate)

    Class ListViewItemComparer
        Implements IComparer
        Private col As Integer

        Public Sub New()
            col = 0
        End Sub

        Public Sub New(column As Integer)
            col = column
        End Sub

        Public Function Compare(x As Object, y As Object) As Integer _
                            Implements System.Collections.IComparer.Compare
            Dim returnVal As Integer = -1

            returnVal = [String].Compare(CType(x,
                        ListViewItem).SubItems(col).Text,
                        CType(y, ListViewItem).SubItems(col).Text)
            Return returnVal
        End Function
    End Class

    Public Sub UpdateChannels(lch As List(Of pSpectra.ChnData))
        ListView1.Items.Clear()
        ListView1.Columns.Clear()
        ListView1.Columns.Add("")
        ListView1.Columns.Add("ID")
        ListView1.Columns.Add("X")
        ListView1.Columns.Add("Y")
        ListView1.Columns.Add("BOARD")
        ListView1.Columns.Add("MODE")
        ListView1.Columns.Add("ORDER")
        ListView1.Columns.Add("RATE")
        Me.ListView1.ListViewItemSorter = Nothing
        ListView1.Columns.Item(0).Width = 18
        ListView1.Columns.Item(1).Width = 0
        ListView1.Columns.Item(2).Width = 30
        ListView1.Columns.Item(3).Width = 30
        ListView1.Columns.Item(4).Width = 50
        ListView1.Columns.Item(5).Width = 0
        ListView1.Columns.Item(6).Width = 0
        ListView1.Columns.Item(7).Width = 50
        For Each c In lch
            If c.MODE = "sum" Then
                ListView1.Items.Add("").SubItems.AddRange({c.ID, 0, 0, "SUM (ALL)", c.MODE, 0, 0})
                sumIndex = ListView1.Items.Count - 1
            Else
                ListView1.Items.Add("").SubItems.AddRange({c.ID, c.X, c.Y, c.BOARD, c.MODE, (c.BOARD * 1000000 + c.Y * 1000 + c.X).ToString().PadLeft(10), 0})
            End If

            ListView1.Items(ListView1.Items.Count - 1).SubItems(0).Name = "CHECKED"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(1).Name = "ID"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(2).Name = "X"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(3).Name = "Y"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(4).Name = "BOARD"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(5).Name = "MODE"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(7).Name = "RATE"

        Next
        Me.ListView1.ListViewItemSorter = New ListViewItemComparer(6)
        ' Call the sort method to manually sort.
        ListView1.Sort()

        ReDim PointHistory(ListView1.Items.Count)
        For i = 0 To PointHistory.Count - 1
            PointHistory(i) = New List(Of tPointRate)
        Next
    End Sub

    Public Sub UpdateChannels_AllChannels()
        ListView1.Items.Clear()
        ListView1.Columns.Clear()
        ListView1.Columns.Add("")
        ListView1.Columns.Add("ID")
        ListView1.Columns.Add("CHANNELS")
        ListView1.Columns.Add("MODE")
        ListView1.Columns.Item(0).Width = 18
        ListView1.Columns.Item(1).Width = 0
        ListView1.Columns.Item(2).Width = 100
        ListView1.Columns.Item(3).Width = 0
        ListView1.Items.Add("").SubItems.AddRange({0, "All Chanels", "0"})
        ListView1.Items(ListView1.Items.Count - 1).SubItems(0).Name = "CHECKED"
        ListView1.Items(ListView1.Items.Count - 1).SubItems(1).Name = "ID"
        ListView1.Items(ListView1.Items.Count - 1).SubItems(2).Name = "CHANNELS"
        ListView1.Items(ListView1.Items.Count - 1).SubItems(3).Name = "MODE"

    End Sub

    Public Sub UpdateChannels_ASIC(lch As List(Of pSpectra.ChnData))
        ListView1.Items.Clear()
        ListView1.Columns.Clear()
        ListView1.Columns.Add("")
        ListView1.Columns.Add("ID")
        ListView1.Columns.Add("ASIC")
        ListView1.Columns.Add("BOARD")
        ListView1.Columns.Add("MODE")
        ListView1.Columns.Item(0).Width = 18
        ListView1.Columns.Item(1).Width = 0
        ListView1.Columns.Item(2).Width = 50
        ListView1.Columns.Item(3).Width = 30
        ListView1.Columns.Item(4).Width = 0
        For Each c In lch
            ListView1.Items.Add(0).SubItems.AddRange({c.ID, c.X, c.BOARD, c.MODE})
            ListView1.Items(ListView1.Items.Count - 1).SubItems(0).Name = "CHECKED"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(1).Name = "ID"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(2).Name = "ASIC"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(3).Name = "BOARD"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(4).Name = "MODE"
        Next

    End Sub

    Public Sub Producer()


    End Sub

    Public Sub StartReceiverThread()

    End Sub
    Public Sub StopReceiverThread()

    End Sub

    Dim lastUpdate As DateTime = Now()

    Dim totAn As Integer = 0
    Public Sub UpdatePlot()
        Dim l = 0
        Dim checked_ch = ListView1.CheckedItems.Count
        Pesgo1.PeData.Points = PointHistory(0).Count
        Pesgo1.PeData.Subsets = checked_ch
        SetYLabel(sMode.Text)

        Dim newp As New List(Of MainForm.ChannelScanPoint)

        For z = 0 To ListView1.CheckedItems.Count - 1
            Dim newpp As New MainForm.ChannelScanPoint


            Dim sid = ListView1.CheckedItems.Item(z).Index

            newpp.channel = ListView1.CheckedItems(z).SubItems(1).Text
            newpp.value = PointHistory(sid).Last.YValue
            For p = 0 To PointHistory(sid).Count - 1
                Pesgo1.PeData.X(l, p) = PointHistory(sid)(p).XValue
                If logmode = False Then
                    Pesgo1.PeData.Y(l, p) = PointHistory(sid)(p).YValue 'spectra(MainForm.acquisition.CHList(s).id - 1, p)
                Else
                    Pesgo1.PeData.Y(l, p) = Math.Log10(PointHistory(sid)(p).YValue + 1)  'Math.Log10(spectra(MainForm.acquisition.CHList(s).id - 1, p) + 1)
                End If



            Next p


            Pesgo1.PeColor.SubsetColors(l) = colorList(sid Mod 30)
            Pesgo1.PePlot.SubsetLineTypes(l) = LineType.ThinSolid
            Pesgo1.PeString.SubsetLabels(l) = "" 'MainForm.acquisition.CHList(s).name
            l = l + 1

            newp.Add(newpp)
        Next

        MainForm.LastScanResult.PushData(PointHistory(0).Last.XValue, PointHistory(0).Last.ScanValue, newp)



        totAn =0
        For p = 0 To PointHistory(0).Count - 1
            If oScanXValue <> PointHistory(0)(p).ScanValue Then
                Pesgo1.PeAnnotation.Graph.X(totAn) = PointHistory(0)(p).XValue
                Pesgo1.PeAnnotation.Graph.Y(totAn) = Math.Round(Pesgo1.PeData.Y(0, p), 3)
                Pesgo1.PeAnnotation.Graph.Text(totAn) = PointHistory(0)(p).ScanValue
                Pesgo1.PeAnnotation.Graph.Type(totAn) = GraphAnnotationType.Pointer
                Pesgo1.PeAnnotation.Graph.Axis(totAn) = 0
                Pesgo1.PeAnnotation.Graph.Color(totAn) = Color.White
                Pesgo1.PeAnnotation.Graph.Show = True
                totAn = totAn + 1
                oScanXValue = PointHistory(0)(p).ScanValue
            End If




        Next

        Pesgo1.PeAnnotation.Show = True
        Pesgo1.Refresh()
    End Sub

    Dim startTimeHistory As DateTime = Now
    Sub ResetHistory()
        For i = 0 To PointHistory.Count - 1
            PointHistory(i).Clear()
        Next
        startTimeHistory = Now
    End Sub



    Public Function UpdateCPS(arr() As UInt32)
        If (isRunning = False) Then
            Return False
        End If
        ReDim TotalCps(arr.Length - 1)

        Dim sum As UInt64 = 0


        For i = 0 To ListView1.Items.Count - 2

            Dim idx As Integer = -1
            Dim q = 0
            For Each cGC In MainForm.GC
                If cGC.X + 1 = ListView1.Items(i).SubItems(2).Text And cGC.Y + 1 = ListView1.Items(i).SubItems(3).Text Then
                    idx = (cGC.Asic * 32) + cGC.Channel
                    Exit For
                End If
                q = q + 1
            Next
            If (idx = -1) Then
                Continue For
            End If
            TotalCps(i) = arr(idx)


            ListView1.Items(i).SubItems(7).Text = TotalCps(i)

            If isRunning And discard_next = 0 Then
                sum += TotalCps(i)
                Dim tt As New tPointRate
                tt.PointData = Now
                tt.ScanValue = ScanXValue
                tt.YValue = TotalCps(i)
                tt.PointIntegralTime = (Now - lastUpdate).TotalSeconds

                ' If ScanMode = "Time" Then
                tt.XValue = (Now - startTimeHistory).TotalSeconds
                ' End If
                PointHistory(i).Add(tt)


            End If
            lastUpdate = Now()
        Next
        ListView1.Items(sumIndex).SubItems(7).Text = sum


        If isRunning And discard_next = 0 Then
            UpdatePlot()
        End If
        If discard_next > 0 Then
            discard_next = discard_next - 1
        End If
    End Function

    Private Sub DisegnaGrafico(ByRef grafico As Pesgo, Gtyle As SGraphPlottingMethod)

        Dim s As Integer
        Dim p As Integer

        '// Enable middle mouse dragging //
        grafico.PeUserInterface.Scrollbar.MouseDraggingX = True
        grafico.PeUserInterface.Scrollbar.MouseDraggingY = True

        '// Enable Bar Glass Effect //
        grafico.PePlot.Option.BarGlassEffect = True

        '// Enable Plotting style gradient and bevel features //
        grafico.PePlot.Option.AreaGradientStyle = PlotGradientStyle.RadialBottomRight
        grafico.PePlot.Option.AreaBevelStyle = BevelStyle.MediumSmooth
        grafico.PePlot.Option.SplineGradientStyle = PlotGradientStyle.RadialBottomRight
        grafico.PePlot.Option.SplineBevelStyle = SplineBevelStyle.MediumSmooth

        '// Prepare images in memory //
        grafico.PeConfigure.PrepareImages = True

        '// Pass Data //
        grafico.PeData.Subsets = 1
        grafico.PeData.Points = 10


        For p = 0 To grafico.PeData.Points - 1
            grafico.PeData.X(0, p) = p
            grafico.PeData.Y(0, p) = 0
        Next p


        '// Set DataShadows to show 3D //
        grafico.PePlot.DataShadows = DataShadows.None

        grafico.PeUserInterface.Allow.FocalRect = False
        grafico.PePlot.Method = SGraphPlottingMethod.Line
        grafico.PeGrid.LineControl = GridLineControl.Both
        grafico.PeGrid.Style = GridStyle.Dot
        grafico.PeUserInterface.Allow.Zooming = AllowZooming.HorzAndVert
        grafico.PeUserInterface.Allow.ZoomStyle = ZoomStyle.Ro2Not

        grafico.PeString.MainTitle = ""
        grafico.PeString.SubTitle = ""
        grafico.PeString.YAxisLabel = ""
        grafico.PeString.XAxisLabel = "(seconds)"

        '// subset labels //
        'loopbackspectrum.PeString.SubsetLabels(0) = "CH 1"


        '// subset colors //
        grafico.PeColor.SubsetColors(0) = Color.FromArgb(255, 255, 0, 0)
        '     grafico.PeColor.SubsetColors(1) = Color.FromArgb(255, 0, 255, 0)


        '// subset line types
        grafico.PeLegend.SubsetLineTypes(0) = LineType.ThinSolid


        '// subset point types
        grafico.PeLegend.SubsetPointTypes(0) = PointType.DotSolid


        grafico.PeLegend.SimplePoint = True
        grafico.PeLegend.SimpleLine = True
        grafico.PeLegend.Style = LegendStyle.OneLine
        grafico.PeGrid.Option.MultiAxisStyle = MultiAxisStyle.SeparateAxes

        '// Various other features //
        grafico.PeFont.Fixed = True
        grafico.PeColor.BitmapGradientMode = True
        grafico.PeColor.QuickStyle = QuickStyle.LightNoBorder

        grafico.PePlot.Option.GradientBars = 8
        grafico.PeConfigure.TextShadows = TextShadows.BoldText
        grafico.PeFont.MainTitle.Bold = True
        grafico.PeFont.MainTitle.Font = "Microsoft Sans Serif"
        grafico.PeFont.SubTitle.Bold = True
        grafico.PeFont.Label.Bold = True
        grafico.PePlot.Option.LineShadows = True
        grafico.PeFont.FontSize = FontSize.Large
        grafico.PeUserInterface.Scrollbar.ScrollingHorzZoom = True
        grafico.PeData.Precision = DataPrecision.OneDecimal

        '// Improves Metafile Export //
        grafico.PeSpecial.DpiX = 600
        grafico.PeSpecial.DpiY = 600



        '// Manually Control Y Axis Grid Line Density //
        'loopbackspectrum.PeGrid.Configure.ManualYAxisLine = 250 '// Grid line every 250 units
        'loopbackspectrum.PeGrid.Configure.ManualYAxisTick = 10  '// Tick line every 25 units
        'loopbackspectrum.PeGrid.Configure.ManualYAxisTicknLine = True

        '// Manually Control Y Axis//

        '    loopbackspectrum.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        '    loopbackspectrum.PeGrid.Configure.ManualMinY = -128
        '    loopbackspectrum.PeGrid.Configure.ManualMaxY = 128

        '// Set Various Other Properties //
        grafico.PeColor.BitmapGradientMode = True
        grafico.PeColor.QuickStyle = QuickStyle.DarkNoBorder
        grafico.PeGrid.LineControl = GridLineControl.Both
        grafico.PeGrid.Option.YAxisLongTicks = True


        '// Set plotting method to line and allow zooming //
        grafico.PePlot.Method = Gtyle
        grafico.PeUserInterface.Allow.Zooming = AllowZooming.HorzAndVert
        grafico.PeUserInterface.Scrollbar.ScrollingHorzZoom = True
        grafico.PeUserInterface.Dialog.PlotCustomization = False

        '// Helps speed large data se3ts, default is 3, good range is 0 to 20 //
        grafico.PeData.SpeedBoost = 10

        '// Set the padding between data and edge of chart //
        grafico.PeGrid.Configure.AutoMinMaxPadding = 1


        '// Disable auto scaling of data //
        grafico.PeData.AutoScaleData = False

        grafico.PePlot.SubsetLineTypes(0) = LineType.ThinSolid
        grafico.PePlot.Option.LineShadows = False

        grafico.PeConfigure.RenderEngine = RenderEngine.GdiTurbo
        grafico.PeConfigure.AntiAliasGraphics = False
        grafico.PeConfigure.AntiAliasText = True

        grafico.PeAnnotation.Line.TextSize = 80
        grafico.PeAnnotation.Show = True

        grafico.PeUserInterface.Allow.ZoomStyle = ZoomStyle.Ro2Not

        grafico.PeUserInterface.Cursor.Mode = CursorMode.FloatingXY
        grafico.PeUserInterface.Cursor.PromptTracking = True
        grafico.PeUserInterface.Cursor.PromptStyle = CursorPromptStyle.XYValues
        grafico.PeUserInterface.HotSpot.Data = True
        grafico.PeUserInterface.Cursor.MouseCursorControl = True

        grafico.PeUserInterface.Scrollbar.MouseDraggingX = True
        grafico.PeUserInterface.Scrollbar.MouseDraggingY = True
        grafico.PeUserInterface.Scrollbar.ScrollingHorzZoom = True
        grafico.PePlot.ZoomWindow.Show = True
        grafico.PeData.NullDataValue = -10000000000


        grafico.PeAnnotation.Graph.TextLocation(0) = 270


        grafico.PeFont.GraphAnnotationTextSize = 85
        grafico.PeUserInterface.Menu.AnnotationControl = True

        grafico.PeAnnotation.Show = True
        grafico.PeUserInterface.HotSpot.GraphAnnotation = AnnotationHotSpot.GraphOnly
        grafico.PeAnnotation.Graph.Moveable = Convert.ToBoolean(GraphAnnotMoveable.Pointer)


        grafico.Refresh()
        '// Generally call ReinitializeResetImage at end **'
        grafico.PeFunction.ReinitializeResetImage()

    End Sub





    Private Sub pSpectra_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        DisegnaGrafico(Pesgo1, SGraphPlottingMethod.Step)
        Pesgo1.PeString.MainTitle = "Counting Scan"

        addressData = MainForm.CurrentMCA.Address
        nchannels = MainForm.CurrentMCA.Channels

        reload()

        sMode.Items.Add("Time")
        sMode.Items.Add("Time Trigger")
        sMode.Items.Add("HV")
        sMode.Items.Add("Gain LG")
        sMode.Items.Add("Gain HG")
        sMode.Items.Add("Input DAC")
        sMode.Items.Add("Time Threshold Correction")
        sMode.Items.Add("Sample/Hold Delay")
        sMode.Items.Add("External Trigger Delay")

        sMode.SelectedIndex = 0
    End Sub

    Public Sub reload()
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

        '   MainForm.acquisition.CHList(CheckedListBox1.SelectedIndex).spectra_checked = IIf(CheckedListBox1.GetItemCheckState(CheckedListBox1.SelectedIndex).ToString = "Checked", True, False)


        'Model.Series.Add(series2)


    End Sub

    Public Sub StartDataCaptureOnFile(file As String)
        SS_totalEvents = 0
        MutexFile.WaitOne()
        If fileEnable = False Then
            saveQueue = New Queue
            saveQueue.Clear()
            fileName = file
            fileEnable = True
            MainForm.plog.TextBox1.AppendText(vbCrLf & "Starting Spectrum Recording...")
        End If
        MutexFile.ReleaseMutex()

    End Sub

    Public Sub StopDataCaptureOnFile()
        ' MainForm.AppendToLog("[INFO] Stopping file capture")
        MutexFile.WaitOne()
        SS_totalEvents = 0
        If fileEnable = True Then
            objRawWriter = New System.IO.StreamWriter(fileName)
            ' objRawWriter.Write(Header & vbCrLf & vbCrLf)
            While saveQueue.Count > 0
                Dim ev As Evento = saveQueue.Dequeue
                objRawWriter.WriteLine(ev.eventId & ";" & ev.timecode.ToString & ";" & String.Join(";", ev.energy).Replace(",", "."))
            End While

        End If

        fileEnable = False

        If IsNothing(objRawWriter) Then
        Else

            objRawWriter.Close()
            MainForm.plog.TextBox1.AppendText(vbCrLf & "Stopping Spectrum Recording.")

        End If

        MutexFile.ReleaseMutex()
        ' MainForm.AppendToLog("[INFO] Capture ended")
        SS_totalEvents = 0
    End Sub


    Class Evento
        Public timecode As UInt64
        Public energy(32) As Double
        Public valid As Boolean
        Public eventId As UInt32
        Public size
        Public ValidEvent As UInt32
        Public Sub New(_size As Integer)
            size = _size
        End Sub
    End Class

    Dim colorList() As Color = {Color.Green, Color.Magenta, Color.LightBlue, Color.Yellow, Color.Aqua, Color.Red, Color.Lime, Color.DarkGreen, Color.Orange, Color.Pink, Color.Gold, Color.Fuchsia,
        Color.White, Color.Maroon, Color.MediumPurple, Color.Purple, Color.LightCyan, Color.LightCoral, Color.Lavender, Color.Ivory, Color.LightPink, Color.YellowGreen, Color.Violet,
        Color.Azure, Color.Salmon, Color.LightSkyBlue, Color.LightYellow, Color.LimeGreen, Color.Beige, Color.SeaShell, Color.Silver}

    Public Sub resetspectrum()

    End Sub

    Public Sub stopspectrum()
        Timer1.Enabled = False
    End Sub

    Public Sub startspectrum()
        Timer1.Enabled = True
        Timer1.Interval = 500
    End Sub

    Public Sub PostData(edata(,) As UInt64, _spectrumLength As Integer)
        spectra = edata
        SpectrumLength = _spectrumLength
    End Sub



    Private Sub Pesgo1_Click(sender As Object, e As EventArgs) Handles Pesgo1.Click

    End Sub

    Dim plotS As Integer = 0
    Private Sub Pesgo1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Pesgo1.KeyPress
        If e.KeyChar = "a" Or e.KeyChar = "A" Then
            Pesgo1.PeFunction.UndoZoom()
        End If

        If e.KeyChar = "v" Or e.KeyChar = "v" Then
            Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Vertical
        End If


        If e.KeyChar = "h" Or e.KeyChar = "H" Then
            Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.Horizontal
        End If


        If e.KeyChar = "z" Or e.KeyChar = "Z" Then
            Pesgo1.PeUserInterface.Allow.Zooming = AllowZooming.HorzAndVert
        End If

        If e.KeyChar = "o" Or e.KeyChar = "O" Then
            plotS = plotS + 1
            If plotS = 9 Then
                plotS = 0
            End If
            Select Case plotS
                Case 0
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.Step
                Case 1
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.Line
                Case 2
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.Spline
                Case 3
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.Bar
                Case 4
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.Area
                Case 5
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.SplineArea
                Case 6
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.Point
                Case 7
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.PointsPlusLine
                Case 8
                    Pesgo1.PePlot.Method = SGraphPlottingMethod.PointsPlusSpline
            End Select
        End If

        If e.KeyChar = "l" Or e.KeyChar = "L" Then
            logmode = Not logmode
        End If

        Pesgo1.Refresh()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub
    Private Sub StopScan()
        isRunning = False
        Timer1.Enabled = False
        Button1.Text = "Start"
    End Sub

    Public Sub SetYLabel(Str As String)
        If logmode Then
            Pesgo1.PeString.YAxisLabel = "Log (" & Str & ")"
        Else
            Pesgo1.PeString.YAxisLabel = Str
        End If

    End Sub


    Private Sub StartScan()

        If Not IsNumeric(sStep.Text) Or Not IsNumeric(sMax.Text) Or Not IsNumeric(sMin.Text) Or Not IsNumeric(sTime.Text) Then
            MsgBox("Scan value must be NUMERIC only", vbCritical + vbOKOnly)
            Exit Sub
        End If


        Timer1.Interval = sTime.Text * 1000
        Timer1.Enabled = True
        Button1.Text = "Stop"
        ScanXValue = Double.Parse(sMin.Text)

        Select Case ScanMode
            Case "Time"
                Pesgo1.PeString.MainTitle = "No Scan - Time Acquisition"
            Case "Time Trigger"
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanTimeThreshold, ScanXValue)
                Pesgo1.PeString.MainTitle = "Time Trigger Scan"
            Case "HV"
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanHV, ScanXValue)
                Pesgo1.PeString.MainTitle = "HV Scan"
            Case "Gain LG"
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanGain_LG, ScanXValue)
                Pesgo1.PeString.MainTitle = "Gain (LG) Scan"
            Case "Gain HG"
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanGain_HG, ScanXValue)
                Pesgo1.PeString.MainTitle = "Gain (hG) Scan"
            Case "Input DAC"
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanInputDAC, ScanXValue)
                Pesgo1.PeString.MainTitle = "Input DAC Scan"
            Case "Time Threshold Correction"
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanCorrThreshold, ScanXValue)
                Pesgo1.PeString.MainTitle = "Time Threshold Correction Scan"
            Case "Sample/Hold Delay"
                Pesgo1.PeString.MainTitle = "Sample/Hold delay Scan"
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanHoldDelay, ScanXValue)
            Case "External Trigger Delay"
                Pesgo1.PeString.MainTitle = "External Trigger Delay Scan"
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanExternalDelay, ScanXValue)
        End Select

        MainForm.LastScanResult.Title = Pesgo1.PeString.MainTitle
        MainForm.LastScanResult.Clear()

        System.Threading.Thread.Sleep(2000)
        ResetHistory()
        isRunning = True
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        discard_next = 1
        If Not IsNumeric(sStep.Text) Or Not IsNumeric(sMax.Text) Or Not IsNumeric(sMin.Text) Or Not IsNumeric(sTime.Text) Then
            Exit Sub
        End If


        If ScanXValue + Double.Parse(sStep.Text) > Double.Parse(sMax.Text) Then
            StopScan()
        End If

        Select Case ScanMode
            Case "Time"
                ScanXValue = 0
            Case "Time Trigger"
                ScanXValue += Double.Parse(sStep.Text)
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanTimeThreshold, ScanXValue)
            Case "HV"
                ScanXValue += Double.Parse(sStep.Text)
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanHV, ScanXValue)
            Case "Gain LG"
                ScanXValue += Double.Parse(sStep.Text)
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanGain_LG, ScanXValue)
            Case "Gain HG"
                ScanXValue += Double.Parse(sStep.Text)
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanGain_HG, ScanXValue)
            Case "Input DAC"
                ScanXValue += Double.Parse(sStep.Text)
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanInputDAC, ScanXValue)
            Case "Time Threshold Correction"
                ScanXValue += Double.Parse(sStep.Text)
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanCorrThreshold, ScanXValue)
            Case "Sample/Hold Delay"
                ScanXValue += Double.Parse(sStep.Text)
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanHoldDelay, ScanXValue)
            Case "External Trigger Delay"
                ScanXValue += Double.Parse(sStep.Text)
                MainForm.sets_citiroc.SCAN_PARAMETER(WeeRocAsicCommonSettings.ScanMode.ScanExternalDelay, ScanXValue)
        End Select



    End Sub

    Private Sub sMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sMode.SelectedIndexChanged
        ScanMode = sMode.Text
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Start" Then

            StartScan()

        Else
            StopScan()
        End If



    End Sub
End Class
