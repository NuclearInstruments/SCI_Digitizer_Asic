Imports System.IO
Imports OxyPlot
Imports OxyPlot.Series

Public Class pScope

    Public addressDecimator As UInt32
    Public addressPre As UInt32
    Public addressMode As UInt32
    Public addressLevel As UInt32
    Public addressArm As UInt32
    Public addressStatus As UInt32
    Public addressPosition As UInt32
    Public wavecount = 0

    Dim inhibit = True
    Public EnabledChannel() As Boolean
    'Public Class DataContainer
    '    Public dataAnalog(1020) As DataPoint
    '    Public dataDigital0(1020) As DataPoint
    '    Public dataDigital1(1020) As DataPoint
    '    Public dataDigital2(1020) As DataPoint
    '    Public dataDigital3(1020) As DataPoint
    '    ' Public dataE(1020) As DataPoint
    '    Public Sub New()

    '    End Sub
    'End Class

    ' Dim dc() As DataContainer
    Dim selfTrigger As Boolean = False
    Dim title() As Label
    '  Dim triglabel() As Label
    '   Dim trig As New ComboBox

    Dim plots() As OxyPlot.WindowsForms.PlotView
    Dim models() As PlotModel
    Dim seriesAnalog() As OxyPlot.Series.LineSeries
    Dim seriesDigital0() As OxyPlot.Series.LineSeries
    Dim seriesDigital1() As OxyPlot.Series.LineSeries
    Dim seriesDigital2() As OxyPlot.Series.LineSeries
    '  Dim seriesDigital3() As OxyPlot.Series.LineSeries
    ' Dim seriesE(63) As OxyPlot.Series.LineSeries
    '   Dim q = 0
    Public running As Boolean = False


    Private Sub pScope_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pScope_ReLoad(MainForm.acquisition.CHList.Count)
    End Sub

    Public Sub pScope_ReLoad(n As Integer)

        Me.Controls.Clear()

        ReDim EnabledChannel(n - 1)
        '  ReDim dc(n - 1)
        ReDim title(n - 1)
        ' ReDim triglabel(n - 1)
        ReDim plots(n - 1)
        ReDim models(n - 1)
        ReDim seriesAnalog(n - 1)
        ReDim seriesDigital1(n - 1)
        ReDim seriesDigital2(n - 1)
        ' ReDim seriesDigital3(n - 1)
        ReDim seriesDigital0(n - 1)


        For i = 0 To n - 1
            EnabledChannel(i) = False
        Next

        Dim Model = New PlotModel()
        'For k = 0 To n - 1
        '    dc(k) = New DataContainer
        '    For i = 0 To (MainForm.CurrentOscilloscope.nsamples - 1)
        '        dc(k).dataAnalog(i) = New DataPoint(i * 1000 / 80, 0)
        '        dc(k).dataDigital0(i) = New DataPoint(i * 1000 / 80, 0)
        '        dc(k).dataDigital1(i) = New DataPoint(i * 1000 / 80, 0)
        '        dc(k).dataDigital2(i) = New DataPoint(i * 1000 / 80, 0)
        '        dc(k).dataDigital3(i) = New DataPoint(i * 1000 / 80, 0)
        '        '  dc(k).dataE(i) = New DataPoint(i, 0)
        '        '  q += 1
        '    Next

        '    Model.Title = "Signal Channel 1"
        '    'Dim series2 As New OxyPlot.Series.ScatterSeries() With {
        '    '    .ItemsSource = data
        '    '        }

        '    'Dim series1 As New OxyPlot.Series.LineSeries() With {
        '    '.Title = "Series 1",
        '    ' .MarkerType = MarkerType.None,
        '    '    .ItemsSource = data
        '    '    }
        'Next k

        '   Model.Series.Add(series1)

        '  Dim row, col
        For i = 0 To n - 1
            models(i) = New PlotModel()
            ' ChannelToLabel(i, col, row)
            ' models(i).Title = col & " - " & row
            seriesAnalog(i) = New OxyPlot.Series.LineSeries() With {
            .Title = "Analog",
             .MarkerType = MarkerType.None',  .ItemsSource = dc(i).dataAnalog
            }

            seriesDigital0(i) = New OxyPlot.Series.LineSeries() With {
            .Title = "Trigger",
             .MarkerType = MarkerType.None', .ItemsSource = dc(i).dataDigital0
            }
            seriesDigital1(i) = New OxyPlot.Series.LineSeries() With {
            .Title = "Energy Sample",
             .MarkerType = MarkerType.None',.ItemsSource = dc(i).dataDigital1
            }
            seriesDigital2(i) = New OxyPlot.Series.LineSeries() With {
            .Title = "Baseline Gate",
             .MarkerType = MarkerType.None'    .ItemsSource = dc(i).dataDigital2
            }
            ' seriesDigital3(i) = New OxyPlot.Series.LineSeries() With {
            '.Title = "Digital 3",
            '.MarkerType = MarkerType.None'          .ItemsSource = dc(i).dataDigital3
            ' }

            models(i).Axes.Add(New Axes.LinearAxis() With {
    .Position = Axes.AxisPosition.Bottom,
    .Maximum = MainForm.CurrentOscilloscope.nsamples * (MainForm.acquisition.General_settings.decimator) * 1000 / 80,
    .Minimum = 0,
    .FontSize = 10,
    .Key = "x",
    .Title = "Time (ns)"
})

            models(i).Axes.Add(New Axes.LinearAxis() With {
    .Position = Axes.AxisPosition.Left,
    .Maximum = 66000,
    .Minimum = 0,
     .FontSize = 10,
     .Key = "y",
     .Title = "Amplitude (lsb)"
})
            'seriesE(i) = New OxyPlot.Series.LineSeries() With {
            '.Title = "Pileup INIB",
            ' .MarkerType = MarkerType.None,
            '    .ItemsSource = dc(i).dataE
            '}
            models(i).Series.Add(seriesAnalog(i))
            models(i).Series.Add(seriesDigital0(i))
            models(i).Series.Add(seriesDigital1(i))
            models(i).Series.Add(seriesDigital2(i))
            '   models(i).Series.Add(seriesDigital3(i))
            'models(i).Series.Add(seriesE(i))

            title(i) = New Label
            '  trig = New ComboBox
            title(i).Height = 20

            plots(i) = New OxyPlot.WindowsForms.PlotView
            plots(i).Height = 450
            plots(i).Top = (i * (plots(i).Height + title(i).Height) + 30)
            plots(i).Anchor = AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top
            plots(i).Width = Me.Width - 20
            plots(i).Left = 10
            plots(i).Model = models(i)
            plots(i).Refresh()
            title(i).Text = MainForm.acquisition.CHList(i).name '"CHANNEL " & (i + 1).ToString

            title(i).Width = 90
            title(i).Font = New Font(title(i).Font, FontStyle.Bold)
            title(i).Top = (i * (plots(i).Height + title(i).Height) + 10)
            title(i).Anchor = AnchorStyles.Left + AnchorStyles.Top
            title(i).Left = 10
            'trig = New ComboBox
            'AddHandler trig.SelectedIndexChanged, AddressOf CLICK_EVENT_COMBOBOX
            'trig.Items.Add("Auto Trigger")
            'trig.Items.Add("Main Trigger")
            'trig.Name = i
            'If MainForm.oscilloscope.CHList(i).trigtype = OscilloscopeClass.Channel.triggertype.AUTO Then
            '    trig.SelectedIndex = 0
            'Else
            '    trig.SelectedIndex = 1
            'End If
            'trig.DropDownStyle = ComboBoxStyle.DropDownList
            'trig.Anchor = AnchorStyles.Left + AnchorStyles.Top
            'trig.Left = 150
            'trig.Top = (i * (plots(i).Height + title(i).Height) + 10)
            '  pan(i).Height = 15
            ' pan(i).Top = (i * (plots(i).Height + pan(i).Height) + 10)
            'pan(i).Width = Me.Width - 20
            'pan(i).Left = 10
            'pan(i).Anchor = AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top

            Me.Controls.Add(title(i))
            ' Me.Controls.Add(trig)

            'models(i).DefaultYAxis.IsZoomEnabled = False
            Me.Controls.Add(plots(i))
            plots(i).InvalidatePlot(True)
        Next

        inhibit = False

    End Sub

    Private Sub autosizey(plot As OxyPlot.WindowsForms.PlotView, max As Integer)
        For Each a In plot.Model.Axes
            If a.Key = "y" Then
                a.Maximum = max + max * 0.1
            End If
        Next
    End Sub

    Private Sub autosizex(max As Integer)
        For i = 0 To plots.Count - 1
            For Each a In plots(i).Model.Axes
                If a.Key = "x" Then
                    a.Maximum = max
                End If
            Next
        Next
    End Sub

    Private Sub plot1_Click(sender As Object, e As EventArgs)

    End Sub

    Public Sub CLICK_EVENT_COMBOBOX(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If inhibit = False Then
            If sender.selectedindex = 0 Then
                MainForm.acquisition.CHList(sender.name).trigtype = AcquisitionClass.Channel.triggertype.AUTO
            Else
                MainForm.acquisition.CHList(sender.name).trigtype = AcquisitionClass.Channel.triggertype.MAIN
            End If
        End If
    End Sub

    Public Sub StartFreeRunningMultiAcquisition()
        selfTrigger = True
        setOscilloscopeParam()
        Timer1.Enabled = True
        running = True
        MainForm.sets.ButtonSetCfg.Enabled = False
        MainForm.plog.TextBox1.AppendText(vbCrLf & "Starting Oscilloscope Free Running Acquisition...")
    End Sub

    Public Sub StartTriggeredMultiAcquisition()
        selfTrigger = False
        setOscilloscopeParam()
        Timer1.Enabled = True
        running = True
        MainForm.sets.ButtonSetCfg.Enabled = False
    End Sub

    Public Sub SingleShot()
        selfTrigger = True
        setOscilloscopeParam()
        MainForm.plog.TextBox1.AppendText(vbCrLf & "Oscilloscope Single Shot Acquisition!")

    End Sub

    Public Sub TriggeredSingledShot()

    End Sub
    Public Sub StopAcquisition()
        Timer1.Enabled = False
        running = False
        MainForm.sets.ButtonSetCfg.Enabled = True
        MainForm.plog.TextBox1.AppendText(vbCrLf & "Stopping Oscilloscope Free Running Acquisition.")

    End Sub

    Public Sub setOscilloscopeParam()


    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

    End Sub

    Private Sub pScope_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
        mwe.Handled = True
    End Sub
End Class
