Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Series

Public Class pImmediate
    Dim immediateCumulative As Boolean
    Public Sub New(ic)
        immediateCumulative = ic
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub plot1_Click(sender As Object, e As EventArgs) Handles plot1.Click

    End Sub
    Dim model As New PlotModel()
    Dim data(,) As Double

    Private Shared Function _formatter(d As Double) As String
        If d < 1000.0 Then
            Return [String].Format("{0}", d)
        ElseIf d >= 1000.0 AndAlso d < 1000000.0 Then
            Return [String].Format("{0}K", d / 1000.0)
        ElseIf d >= 1000000.0 AndAlso d < 1000000000.0 Then
            Return [String].Format("{0}M", d / 1000000.0)
        ElseIf d >= 1000000000.0 Then
            Return [String].Format("{0}B", d / 1000000000.0)
        Else
            Return [String].Format("{0}", d)
        End If
    End Function
    Private Sub pImmediate_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub
    Private Sub pImmediate_resize(sender As Object, e As EventArgs) Handles MyBase.Resize


    End Sub
    Public Sub pImmediate_ReLoad(r As Integer, c As Integer)

        ReDim data(c - 1, r - 1)
        Dim xlabel(c - 1) As String
        Dim ylabel(r - 1) As String
        For i = 0 To c - 1
            xlabel(i) = (i + 1).ToString
        Next
        For j = 0 To r - 1
            ylabel(j) = (j + 1).ToString
        Next
        model.Axes.Clear()
        model.Series.Clear()
        model.Axes.Add(New CategoryAxis() With {
            .Position = AxisPosition.Bottom,
            .Key = "pxb",
            .ItemsSource = xlabel
        })
        model.Axes.Add(New CategoryAxis() With {
            .Position = AxisPosition.Left,
            .Key = "pxl",
            .StartPosition = 1,
            .EndPosition = 0,
        .ItemsSource = ylabel
        })
        model.Axes.Add(New LinearColorAxis() With {
         .Palette = OxyPalettes.Jet(200),
        .Key = "color"
         })
        'Dim rand = New Random()

        For x As Integer = 0 To c - 1
            For y As Integer = 0 To r - 1
                data(x, y) = 0
            Next
        Next
        ' neccessary to display the label
        Dim heatMapSeries
        If immediateCumulative = True Then
            heatMapSeries = New HeatMapSeries() With {
            .X0 = 0,
            .X1 = c - 1,
            .Y0 = 0,
            .Y1 = r - 1,
            .XAxisKey = "pxb",
            .YAxisKey = "pxl",
            .RenderMethod = HeatMapRenderMethod.Rectangles,
            .LabelFontSize = 0.2,
            .Data = data
        }
        Else
            heatMapSeries = New HeatMapSeries() With {
              .X0 = 0,
              .X1 = c - 1,
              .Y0 = 0,
              .Y1 = r - 1,
              .XAxisKey = "pxb",
              .YAxisKey = "pxl",
              .RenderMethod = HeatMapRenderMethod.Rectangles,
              .LabelFontSize = 0.0,
              .Data = data
          }
        End If

        model.Series.Add(heatMapSeries)

        plot1.Model = model
        '  model.ResetAllAxes()
        plot1.InvalidatePlot(True)


    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Dim tempdata() As Double
        Dim scale As Double = 1
        Dim max As Double = 1
        Dim rescale As Boolean = True
        Try
            If immediateCumulative = True Then


                For i = 0 To MainForm.GC.Count - 1

                    data(MainForm.GC(i).X, MainForm.GC(i).Y) = MainForm.MatrixInst(i)

                Next
            Else


                For i = 0 To MainForm.GC.Count - 1

                    data(MainForm.GC(i).X, MainForm.GC(i).Y) = MainForm.MatrixCumulative(i)

                Next
            End If
        Catch ex As Exception

        End Try


        '  For q = 0 To MainForm.oscilloscope.CHList.Count - 1
        ' max = Math.Max(tempdata(q), max)
        'Next
        '
        'q = 0

        '   For i = 0 To MainForm.oscilloscope.currentMAP.cols - 1
        '  For j = 0 To MainForm.oscilloscope.currentMAP.rows - 1


        '   Next
        'Next
        plot1.InvalidatePlot(True)

        Timer1.Enabled = True
    End Sub

    Public Sub StartLive()
        Timer1.Interval = 500
        Timer1.Enabled = True
    End Sub

    Public Sub StopLive()
        Timer1.Enabled = False
    End Sub


End Class
