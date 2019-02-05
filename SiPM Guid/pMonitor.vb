Imports Gigasoft.ProEssentials.Enums

Public Class pMonitor


    Public Sub CreatePlotTopPetiroc()

        Pesgo1.PeData.Subsets = 8
        Pesgo1.PeData.Points = 2048
        Pesgo1.PeSpecial.AutoImageReset = False

        Pesgo1.PeGrid.MultiAxesSubsets(0) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(1) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(2) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(3) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(4) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(5) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(6) = 1
        Pesgo1.PeGrid.MultiAxesSubsets(7) = 1


        'Pesgo1.PeGrid.OverlapMultiAxes(0) = 2
        'Pesgo1.PeGrid.OverlapMultiAxes(1) = 1
        'Pesgo1.PeGrid.OverlapMultiAxes(2) = 1

        Pesgo1.PeLegend.Show = False

        Pesgo1.PeConfigure.CacheBmp = True
        Pesgo1.PeConfigure.PrepareImages = True

        Pesgo1.PeColor.BitmapGradientMode = True

        Pesgo1.PeColor.QuickStyle = QuickStyle.DarkNoBorder
        Pesgo1.PeConfigure.AntiAliasGraphics = True
        Pesgo1.PeFont.Fixed = True
        Pesgo1.PeColor.SubsetColors(0) = Color.FromArgb(255, 255, 255)
        Pesgo1.PeColor.SubsetColors(1) = Color.FromArgb(0, 255, 255)
        Pesgo1.PeColor.SubsetColors(2) = Color.FromArgb(255, 255, 0)
        Pesgo1.PeColor.SubsetColors(3) = Color.FromArgb(0, 255, 0)

        Pesgo1.PePlot.DataShadows = DataShadows.None
        Pesgo1.PePlot.SubsetLineTypes(0) = LineType.ThinSolid
        Pesgo1.PePlot.SubsetLineTypes(1) = LineType.ThinSolid
        Pesgo1.PePlot.SubsetLineTypes(2) = LineType.ThinSolid
        Pesgo1.PePlot.SubsetLineTypes(3) = LineType.ThinSolid

        Pesgo1.PeConfigure.ImageAdjustRight = 100
        Pesgo1.PeConfigure.ImageAdjustLeft = 20


        Pesgo1.PeSpecial.DpiX = 600
        Pesgo1.PeSpecial.DpiY = 600

        ' Setting to help with New Direct3D rendering And example expanded to 400K points //
        Pesgo1.PePlot.Option.NullDataGaps = False
        Pesgo1.PeUserInterface.Cursor.HourGlassThreshold = 10000000
        Pesgo1.PeData.Filter2D = Filter2D.Fastest
        Pesgo1.PeAnnotation.Show = True
        Pesgo1.PeAnnotation.Line.RightMargin = "XXXXXXXXXXXX"
        Pesgo1.PeConfigure.ImageAdjustLeft = 100
        Pesgo1.PeSpecial.AutoImageReset = False '  // important For Direct3D rendering 

        If (Pesgo1.IsDxAvailable) Then
            Pesgo1.PeConfigure.RenderEngine = RenderEngine.Direct3D
        Else
            Pesgo1.PeConfigure.RenderEngine = RenderEngine.GdiTurbo
        End If
        '// Composite2D3D // Faster settings are 1-force only one D2D layer in back, 2-force only one D2D layer in front 
        '// Default setting 0 creates 2 D2D layers, one in back ground And one in foreground, allowing normal separation 
        '// of graphics as properties dictate.
        Pesgo1.PeConfigure.Composite2D3D = Composite2D3D.Foreground

        Pesgo1.PeData.NullDataValue = Double.MinValue

        Pesgo1.PeGrid.WorkingAxis = 0
        Pesgo1.PeString.YAxisLabel = "AN"
        Pesgo1.PeGrid.WorkingAxis = 1
        Pesgo1.PeString.YAxisLabel = "D PROBE"
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 2
        Pesgo1.PeString.YAxisLabel = "TRIGGER"
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 3
        Pesgo1.PeString.YAxisLabel = "A CLK"
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 4
        Pesgo1.PeString.YAxisLabel = "A DIN"
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 5
        Pesgo1.PeString.YAxisLabel = "LEMO"
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 6
        Pesgo1.PeString.YAxisLabel = "GLOBAL"
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1
        Pesgo1.PeGrid.WorkingAxis = 7
        Pesgo1.PeString.YAxisLabel = "TRIG MUX"
        Pesgo1.PeGrid.Configure.ManualScaleControlY = ManualScaleControl.MinMax
        Pesgo1.PeGrid.Configure.ManualMinY = 0
        Pesgo1.PeGrid.Configure.ManualMaxY = 1.1


        Pesgo1.PeGrid.WorkingAxis = 0


        Dim tmpXData(2048 * 8) As Single
        For i = 0 To 2047
            tmpXData(i) = i * 1 / 80000000
            tmpXData(i + (2048 * 1)) = i * 1 / 80000000
            tmpXData(i + (2048 * 2)) = i * 1 / 80000000
            tmpXData(i + (2048 * 3)) = i * 1 / 80000000
            tmpXData(i + (2048 * 4)) = i * 1 / 80000000
            tmpXData(i + (2048 * 5)) = i * 1 / 80000000
            tmpXData(i + (2048 * 6)) = i * 1 / 80000000
            tmpXData(i + (2048 * 7)) = i * 1 / 80000000
        Next
        Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, tmpXData, 2048 * 8) '18252)
        Pesgo1.PeData.Y(0, 0) = 0
        Pesgo1.PeData.Y(0, 1) = 0
        Pesgo1.PeData.Y(0, 2) = 0
        Pesgo1.PeData.Y(0, 3) = 0

        Dim y1(2048), y2(2048), y3(2048), y4(2048) As Single
        Dim tmpYData2(2048 * 9) As Single
        For i = 0 To 2047
            y1(i) = 0 'Math.Sin(i / 10) * 34
            y2(i) = 0 'IIf(y1(i) > 0, 1, 0)
            y3(i) = 0 'IIf(y1(i) > 10, 1, 0)
            y4(i) = 0 'IIf(y1(i) < 10, 1, 0)
        Next
        Array.Copy(y1, 0, tmpYData2, 0, 2048)
        Array.Copy(y2, 0, tmpYData2, 2048, 2048)
        Array.Copy(y3, 0, tmpYData2, 4096, 2048)
        Array.Copy(y4, 0, tmpYData2, 6144, 2048)

        Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, tmpYData2, 2048 * 8) '18252)

        'Pesgo1.PeUserInterface.Allow.MultiAxesSizing = True
        Pesgo1.PeGrid.MultiAxesProportions(0) = 0.3
        Pesgo1.PeGrid.MultiAxesProportions(1) = 0.1
        Pesgo1.PeGrid.MultiAxesProportions(2) = 0.1
        Pesgo1.PeGrid.MultiAxesProportions(3) = 0.1
        Pesgo1.PeGrid.MultiAxesProportions(4) = 0.1
        Pesgo1.PeGrid.MultiAxesProportions(5) = 0.1
        Pesgo1.PeGrid.MultiAxesProportions(6) = 0.1
        Pesgo1.PeGrid.MultiAxesProportions(7) = 0.1

        '// Reset image //
        Pesgo1.PeFunction.Force3dxVerticeRebuild = True
        Pesgo1.PeFunction.Force3dxNewColors = True
        Pesgo1.PeFunction.ReinitializeResetImage()




    End Sub


    Public Sub Plot(PMD As DT5550W_P_lib.DT5550W.PetirocMonitorData)
        Dim tmpYData2(2048 * 9) As Single
        Dim v1 = 0
        Dim v2 = 0
        Dim v3 = 0
        Dim v4 = 0
        For i = 0 To PMD.charge_mux_a.Length - 1
            v1 = v1 * 0.6 + PMD.charge_mux_a(i) * 0.4
            PMD.charge_mux_a(i) = v1
            v2 = v2 * 0.6 + PMD.charge_mux_b(i) * 0.4
            PMD.charge_mux_b(i) = v2
            v3 = v3 * 0.6 + PMD.charge_mux_c(i) * 0.4
            PMD.charge_mux_c(i) = v3
            v3 = v3 * 0.6 + PMD.charge_mux_d(i) * 0.4
            PMD.charge_mux_d(i) = v4

        Next
        If ComboBox2.SelectedIndex = 0 Then
            Array.Copy(PMD.charge_mux_a, 0, tmpYData2, 2048 * 0, 2048)
            'Array.Copy(PMD.an_probe_a, 0, tmpYData2, 2048 * 1, 2048)
            Array.Copy(PMD.dig_probe_a, 0, tmpYData2, 2048 * 1, 2048)
            Array.Copy(PMD.trigger_a, 0, tmpYData2, 2048 * 2, 2048)
            Array.Copy(PMD.sr_clk_a, 0, tmpYData2, 2048 * 3, 2048)
            Array.Copy(PMD.sr_din_a, 0, tmpYData2, 2048 * 4, 2048)
            Array.Copy(PMD.lemo1, 0, tmpYData2, 2048 * 5, 2048)
            Array.Copy(PMD.global_trigger, 0, tmpYData2, 2048 * 6, 2048)
            Array.Copy(PMD.trig_b_mux_a, 0, tmpYData2, 2048 * 7, 2048)
        End If

        If ComboBox2.SelectedIndex = 1 Then
            Array.Copy(PMD.charge_mux_b, 0, tmpYData2, 2048 * 0, 2048)
            '    Array.Copy(PMD.an_probe_b, 0, tmpYData2, 2048 * 1, 2048)
            Array.Copy(PMD.dig_probe_b, 0, tmpYData2, 2048 * 1, 2048)
            Array.Copy(PMD.trigger_b, 0, tmpYData2, 2048 * 2, 2048)
            Array.Copy(PMD.sr_clk_b, 0, tmpYData2, 2048 * 3, 2048)
            Array.Copy(PMD.sr_din_b, 0, tmpYData2, 2048 * 4, 2048)
            Array.Copy(PMD.lemo1, 0, tmpYData2, 2048 * 5, 2048)
            Array.Copy(PMD.global_trigger, 0, tmpYData2, 2048 * 6, 2048)
            Array.Copy(PMD.trig_b_mux_b, 0, tmpYData2, 2048 * 7, 2048)
        End If

        If ComboBox2.SelectedIndex = 2 Then
            Array.Copy(PMD.charge_mux_c, 0, tmpYData2, 2048 * 0, 2048)
            '  Array.Copy(PMD.an_probe_c, 0, tmpYData2, 2048 * 1, 2048)
            Array.Copy(PMD.dig_probe_c, 0, tmpYData2, 2048 * 1, 2048)
            Array.Copy(PMD.trigger_c, 0, tmpYData2, 2048 * 2, 2048)
            Array.Copy(PMD.sr_clk_c, 0, tmpYData2, 2048 * 3, 2048)
            Array.Copy(PMD.sr_din_c, 0, tmpYData2, 2048 * 4, 2048)
            Array.Copy(PMD.lemo1, 0, tmpYData2, 2048 * 5, 2048)
            Array.Copy(PMD.global_trigger, 0, tmpYData2, 2048 * 6, 2048)
            Array.Copy(PMD.trig_b_mux_c, 0, tmpYData2, 2048 * 7, 2048)
        End If

        If ComboBox2.SelectedIndex = 3 Then
            Array.Copy(PMD.charge_mux_d, 0, tmpYData2, 2048 * 0, 2048)
            '  Array.Copy(PMD.an_probe_d, 0, tmpYData2, 2048 * 1, 2048)
            Array.Copy(PMD.dig_probe_d, 0, tmpYData2, 2048 * 1, 2048)
            Array.Copy(PMD.trigger_d, 0, tmpYData2, 2048 * 2, 2048)
            Array.Copy(PMD.sr_clk_d, 0, tmpYData2, 2048 * 3, 2048)
            Array.Copy(PMD.sr_din_d, 0, tmpYData2, 2048 * 4, 2048)
            Array.Copy(PMD.lemo1, 0, tmpYData2, 2048 * 5, 2048)
            Array.Copy(PMD.global_trigger, 0, tmpYData2, 2048 * 6, 2048)
            Array.Copy(PMD.trig_b_mux_d, 0, tmpYData2, 2048 * 7, 2048)
        End If


        Gigasoft.ProEssentials.Api.PEvsetW(Pesgo1.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, tmpYData2, 2048 * 8)



        If (Pesgo1.PeConfigure.RenderEngine = RenderEngine.Direct3D) Then
            Pesgo1.PeFunction.Force3dxVerticeRebuild = True
        Else

            Pesgo1.PeFunction.Reinitialize()
            Pesgo1.PeFunction.ResetImage(0, 0)
        End If

        Pesgo1.Invalidate()

            Pesgo1.PeFunction.ReinitializeResetImage()

    End Sub

    Private Sub Pesgo1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub CheckedListBox2_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Pesgo2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub pMonitor_Load(sender As Object, e As EventArgs) Handles Me.Load
        CreatePlotTopPetiroc()
        ComboBox2.Items.Add("A")
        ComboBox2.Items.Add("B")
        ComboBox2.Items.Add("C")
        ComboBox2.Items.Add("D")
        ComboBox2.SelectedIndex = 0
    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs)
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Pesgo1_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Pesgo1_Click_2(sender As Object, e As EventArgs) Handles Pesgo1.Click

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Dim plotS As Integer = 0
    Dim logmode As Boolean
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
End Class
