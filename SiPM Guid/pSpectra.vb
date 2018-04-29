Imports System.IO
Imports System.Threading
Imports System.ComponentModel
Imports Gigasoft.ProEssentials
Imports Gigasoft.ProEssentials.Enums
Imports OxyPlot
Imports MathNet.Numerics

Public Class pSpectra
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

    Public Structure ChnData
        Public X As String
        Public Y As String
        Public BOARD As String
        Public ID As Integer
        Public MODE As String
    End Structure

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

    Public Sub UpdateChannels(lch As List(Of ChnData))
        ListView1.Items.Clear()
        ListView1.Columns.Clear()
        ListView1.Columns.Add("")
        ListView1.Columns.Add("ID")
        ListView1.Columns.Add("X")
        ListView1.Columns.Add("Y")
        ListView1.Columns.Add("BOARD")
        ListView1.Columns.Add("MODE")
        ListView1.Columns.Add("ORDER")
        Me.ListView1.ListViewItemSorter = Nothing
        ListView1.Columns.Item(0).Width = 18
        ListView1.Columns.Item(1).Width = 0
        ListView1.Columns.Item(2).Width = 30
        ListView1.Columns.Item(3).Width = 30
        ListView1.Columns.Item(4).Width = 50
        ListView1.Columns.Item(5).Width = 0
        ListView1.Columns.Item(6).Width = 0
        For Each c In lch
            ListView1.Items.Add("").SubItems.AddRange({c.ID, c.X, c.Y, c.BOARD, c.MODE, (c.BOARD * 1000000 + c.Y * 1000 + c.X).ToString().PadLeft(10)})
            ListView1.Items(ListView1.Items.Count - 1).SubItems(0).Name = "CHECKED"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(1).Name = "ID"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(2).Name = "X"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(3).Name = "Y"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(4).Name = "BOARD"
            ListView1.Items(ListView1.Items.Count - 1).SubItems(5).Name = "MODE"

        Next
        Me.ListView1.ListViewItemSorter = New ListViewItemComparer(6)
        ' Call the sort method to manually sort.
        ListView1.Sort()


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

    Public Sub UpdateChannels_ASIC(lch As List(Of ChnData))
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
        ListView1.Columns.Item(3).Width = 50
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
        grafico.PeData.Points = 65536


        For p = 0 To 65535
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
        grafico.PeString.XAxisLabel = "(channels)"

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
        grafico.Refresh()
        '// Generally call ReinitializeResetImage at end **'
        grafico.PeFunction.ReinitializeResetImage()

    End Sub





    Private Sub pSpectra_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        DisegnaGrafico(Pesgo1, SGraphPlottingMethod.Step)
        Pesgo1.PeString.MainTitle = "Real Time Spectra"

        addressData = MainForm.CurrentMCA.Address
        nchannels = MainForm.CurrentMCA.Channels

        reload()

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

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Dim checked_ch = ListView1.CheckedItems.Count
        If MainForm.fit_enabled = False Then
            Dim l = 0

            Pesgo1.PeData.Points = SpectrumLength
            Pesgo1.PeData.Subsets = checked_ch
            For z = 0 To ListView1.CheckedItems.Count - 1
                Dim sid = ListView1.CheckedItems.Item(z).SubItems("ID").Text
                For p = 0 To SpectrumLength - 1
                    Pesgo1.PeData.X(l, p) = p
                    If logmode = False Then
                        Pesgo1.PeData.Y(l, p) = spectra(sid, p) 'spectra(MainForm.acquisition.CHList(s).id - 1, p)
                    Else
                        Pesgo1.PeData.Y(l, p) = Math.Log10(spectra(sid, p) + 1) 'Math.Log10(spectra(MainForm.acquisition.CHList(s).id - 1, p) + 1)
                    End If

                Next p
                Pesgo1.PeColor.SubsetColors(l) = colorList(sid Mod 30)
                Pesgo1.PePlot.SubsetLineTypes(l) = LineType.ThinSolid
                Pesgo1.PeString.SubsetLabels(l) = "" 'MainForm.acquisition.CHList(s).name
                l = l + 1
            Next
            Pesgo1.Refresh()
        Else
            'Dim l = 0
            'Dim n_spettri = checked_ch + MainForm.fit.DataGridView1.Rows.Count
            'Pesgo1.PeData.Points = SpectrumLength
            'Pesgo1.PeData.Subsets = n_spettri
            'Dim graficisullospettro((n_spettri) * SpectrumLength) As Single
            'Dim graficisullospettro_x((n_spettri) * SpectrumLength) As Single

            'For Each s In CheckedListBox1.CheckedIndices
            '    For p = 0 To SpectrumLength - 1
            '        If logmode = False Then
            '            graficisullospettro(p * (s + 1)) = spectra(MainForm.acquisition.CHList(s).id - 1, p)
            '        Else
            '            graficisullospettro(p * (s + 1)) = Math.Log10(spectra(MainForm.acquisition.CHList(s).id - 1, p))
            '        End If
            '        graficisullospettro_x(p * (s + 1)) = p
            '    Next
            'Next

            'For k = 0 To MainForm.fit.DataGridView1.Rows.Count - 1
            '    Dim sx, ex As Integer

            '    Try
            '        sx = CInt(MainForm.fit.DataGridView1.Rows(k).Cells("Cursor 1").Value)
            '        ex = CInt(MainForm.fit.DataGridView1.Rows(k).Cells("Cursor 2").Value)

            '        Dim dataproc_y(ex - sx) As Double
            '        Dim dataproc_x(ex - sx) As Double
            '        Dim mean As Double = 0
            '        Dim q = 0
            '        Dim std_dev As Double = 0
            '        Dim max As Integer = 0
            '        Dim idx = 0
            '        Dim mu1, sg1, A1 As Double
            '        Dim areaUpeak As Double = 0
            '        Dim areaFit As Double = 0
            '        Dim dataCorretto As Double
            '        Dim selected_ch = CInt(MainForm.fit.DataGridView1.Rows(k).Cells("Channel").ToString.Replace("CHANNEL ", ""))

            '        For i = sx To ex
            '            dataCorretto = spectra(MainForm.acquisition.CHList(selected_ch).id - 1, i)

            '            mean = mean + dataCorretto * i
            '            q = q + dataCorretto
            '            If dataCorretto > max Then
            '                max = dataCorretto
            '            End If
            '            dataproc_y(idx) = Math.Log(dataCorretto + 1)
            '            If Double.IsNaN(dataproc_y(idx)) Then
            '                If idx > 0 Then
            '                    dataproc_y(idx) = dataproc_y(idx - 1)
            '                Else
            '                    dataproc_y(idx) = 0
            '                End If
            '            Else
            '                If Double.IsNegativeInfinity(dataproc_y(idx)) Then
            '                    If idx > 0 Then
            '                        dataproc_y(idx) = dataproc_y(idx - 1)
            '                    Else
            '                        dataproc_y(idx) = 0
            '                    End If
            '                End If
            '            End If

            '            dataproc_x(idx) = i
            '            idx = idx + 1
            '        Next
            '        mean = mean / q

            '        q = 0
            '        For i = sx To ex
            '            dataCorretto = spectra(MainForm.acquisition.CHList(selected_ch).id - 1, i)
            '            std_dev = std_dev + dataCorretto * Math.Pow(i - mean, 2)
            '            q = q + dataCorretto
            '        Next
            '        std_dev = std_dev / q
            '        std_dev = Math.Sqrt(std_dev)

            '        Dim fitres As Double() = Fit.Polynomial(dataproc_x, dataproc_y, 2)
            '        mu1 = -1 * fitres(1) / (2 * fitres(2))
            '        sg1 = Math.Sqrt(-1 / (2 * fitres(2)))
            '        A1 = Math.Exp(fitres(0) - (Math.Pow(fitres(1), 2) / (4 * fitres(2))))
            '        For i = sx To ex
            '            areaFit += A1 * Math.Exp(-Math.Pow((i - mu1), 2) / (2 * sg1 * sg1))
            '        Next
            '        If Double.IsNaN(mu1) Then
            '            sg1 = sg1
            '        End If

            '        MainForm.fit.DataGridView1.Rows(k).Cells("fitmu").Value = Math.Round(mu1, 2)
            '        MainForm.fit.DataGridView1.Rows(k).Cells("fitsigma").Value = Math.Round(2.35 * sg1, 3) & " (" & Math.Round(2.35 * sg1 / mu1 * 100, 2) & "%)"
            '        MainForm.fit.DataGridView1.Rows(k).Cells("mean").Value = Math.Round(mean, 2)
            '        MainForm.fit.DataGridView1.Rows(k).Cells("sigma").Value = Math.Round(2.35 * std_dev, 2) & " (" & Math.Round(2.35 * std_dev * 100 / mean, 2) & "%)"
            '        MainForm.fit.DataGridView1.Rows(k).Cells("areaUpeak").Value = areaUpeak
            '        MainForm.fit.DataGridView1.Rows(k).Cells("areaFit").Value = Math.Round(areaFit)

            '        Dim point As Double
            '        Dim startx = sx - 15
            '        Dim endx = ex + 15
            '        If startx < 0 Then
            '            startx = 0
            '        End If
            '        If endx > SpectrumLength - 1 Then
            '            endx = SpectrumLength - 1
            '        End If
            '        Dim g = 0
            '        For g = 1 To SpectrumLength
            '            If logmode Then
            '                graficisullospettro((k + checked_ch) * SpectrumLength + g) = 1
            '            Else
            '                graficisullospettro((k + checked_ch) * SpectrumLength + g) = 0
            '            End If
            '            graficisullospettro_x((k + checked_ch) * SpectrumLength + g) = g
            '        Next
            '        For i = sx To ex
            '            point = A1 * Math.Exp(-1 * Math.Pow(i - mu1, 2) / (2 * Math.Pow(sg1, 2))) '+ meanBG' fitrect(0) + fitrect(1) * i
            '            If logmode = False Then
            '                graficisullospettro((k + checked_ch) * SpectrumLength + i) = point
            '            Else
            '                graficisullospettro((k + checked_ch) * SpectrumLength + i) = point
            '            End If
            '            ' graficisullospettro_x((k + checked_ch) * SpectrumLength + i) = i
            '        Next
            '    Catch
            '    End Try

            '    Pesgo1.PeAnnotation.Line.XAxis(k) = sx
            '    Pesgo1.PeAnnotation.Line.XAxisType(k) = LineAnnotationType.ThinSolid
            '    Pesgo1.PeAnnotation.Line.XAxisColor(k) = Color.Red
            '    Pesgo1.PeAnnotation.Line.XAxisText(k) = k + 1

            '    Pesgo1.PeAnnotation.Line.XAxis(k + 1) = ex
            '    Pesgo1.PeAnnotation.Line.XAxisType(k + 1) = LineAnnotationType.ThinSolid
            '    Pesgo1.PeAnnotation.Line.XAxisColor(k + 1) = Color.Red
            '    Pesgo1.PeAnnotation.Line.XAxisText(k + 1) = k + 1

            '            Next
        End If


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
End Class
